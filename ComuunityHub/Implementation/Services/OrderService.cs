using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ComuunityHub.DTOs;
using ComuunityHub.Implementation.Repositories;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Interfaces.Services;
using ComuunityHub.Models;
using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;
using Org.BouncyCastle.Math.EC;

namespace ComuunityHub.Implementation.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IShippingService _shippingService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBuyerRepository _buyerRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IPaystackService _paystackService;
    private readonly ISubOrderRepository _subOrderRepository;
    private readonly ISellerRepository _sellerRepository;
    private readonly IEmailService _emailService;
    private readonly INotificationRepository _notificationRepository;

    public OrderService(IOrderRepository orderRepository, IUserRepository userRepository,
        IProductRepository productRepository, IShippingService shippingService, IHttpContextAccessor httpContextAccessor,
        IBuyerRepository buyerRepository,ICartRepository cartRepository,IPaystackService paystackService, INotificationRepository notificationRepository,
        ISubOrderRepository subOrderRepository, ISellerRepository sellerRepository, IEmailService emailService)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _shippingService = shippingService;
        _httpContextAccessor = httpContextAccessor;
        _buyerRepository = buyerRepository;
        _cartRepository = cartRepository;
        _paystackService = paystackService;
        _subOrderRepository = subOrderRepository;
        _sellerRepository = sellerRepository;
        _notificationRepository = notificationRepository;
        _emailService = emailService;
    }

    public async Task<CheckoutPreviewResponseModel> PreviewCheckOutAsync(AddressRequestModel deliveryAddrress)
    {
        var userId = _httpContextAccessor.HttpContext.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var buyer = await _buyerRepository.GetByUserIdAsync(userId);
        var cart =await _cartRepository.GetCartByUserId(userId);
        if (cart == null || !cart.Items.Any())
        {
            return new CheckoutPreviewResponseModel()
            {
                Message = "Cart is empty",
                Status = false
            };
        }

        decimal subtotal = cart.Items.Sum(x => x.Quantity * x.Product.Price);
        decimal shippingfee = 0;
        var groupedBySeller = cart.Items
            .GroupBy(x => x.Product.SellerId)
            .ToList();
        foreach (var seller in groupedBySeller)
        {
            var items = seller.Select(x => new ItemPreviewDTO()
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList();
            var sellerShippingFee = _shippingService.CalculateShippingFeeAsync(seller.Key, deliveryAddrress, items);
            shippingfee += sellerShippingFee;
        }
        decimal total = subtotal + shippingfee;
        return new CheckoutPreviewResponseModel()
        {
            Total = total,
            Items = cart.Items.Select(x => new ItemPreviewDTO()
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Price = x.Product.Price,
                ProductName = x.Product.Name,
                ProductImageUrl = x.Product.ImageUrl
            }).ToList(),
            Status = true,
            Subtotal = subtotal,
            ShippingFee = shippingfee,
            Message = "Successfully returned order preview"
        };
    }

    public async Task<OrderResponseModel> CreateOrderResponseModel(AddressRequestModel deliveryAddrress)
    {
        var userId = _httpContextAccessor.HttpContext.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var buyer = await _buyerRepository.GetByUserIdAsync(userId);
        var cart = await _cartRepository.GetCartByUserId(userId);
        if (cart == null || !cart.Items.Any())
        {
            return new OrderResponseModel()
            {
                Message = "Cart is empty",
                Status = false
            };
        }
        decimal subtotal = cart.Items.Sum(x => x.Quantity * x.Product.Price);
        decimal shippingfee = 0;
        var groupedBySeller = cart.Items.GroupBy(x => x.Product.SellerId).ToList();
        foreach (var seller in groupedBySeller)
        {
            var items = seller.Select(x => new ItemPreviewDTO()
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList();
            var sellerShippingfee = _shippingService.CalculateShippingFeeAsync(seller.Key, deliveryAddrress, items);
            shippingfee += sellerShippingfee;
        }

        var order = new Order()
        {
            BuyerId = buyer.Id,
            TotalAmount = shippingfee + subtotal,
            Status = OrderStatus.PendingPayment,
            DeliveryAddress = new Address()
            {
                Street = deliveryAddrress.Street,
                City = deliveryAddrress.City,
                State = deliveryAddrress.State,
                Country = deliveryAddrress.Country,
                PostalCode = deliveryAddrress.PostalCode,
                Landmark = deliveryAddrress.Landmark
            },
            CreatedAt = DateTime.UtcNow,
            SubOrders = new List<SubOrder>()
        };
        foreach (var sellerorder in groupedBySeller)
        {
            var suborder = new SubOrder()
            {
                OrderId = order.Id,
                SellerId = sellerorder.Key,
                SubOrderStatus = SubOrderStatus.Pending,
                ShipmentStatus = ShipmentStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                SubTotal = sellerorder.Sum(x => x.Quantity * x.Product.Price),
                SubOrderItems = sellerorder.Select(x => new SubOrderItem()
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    TotalPrice = x.Quantity * x.Product.Price
                }).ToList()
            };
            order.SubOrders.Add(suborder);
        }
        await _orderRepository.AddOrderAsync(order);
        await _cartRepository.EmptyCartAsync(cart);
        return new OrderResponseModel()
        {
            Status = true,
            Message = "Successfully created order",
            OrderId = order.Id
        };
    }

    public async Task<MakePaymentResponseModel> MakePayment(string orderId)
    {
        var userId = _httpContextAccessor.HttpContext.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var buyer =await _buyerRepository.GetByUserIdAsync(userId);
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null || order.BuyerId != buyer.Id)
        {
            return new MakePaymentResponseModel()
            {
                Status = false,
                Message = "Order not found"
            };
        }

        if (order.Status != OrderStatus.PendingPayment)
        {
            return new MakePaymentResponseModel()
            {
                Message = "Order is not awaiting payment",
                Status = false
            };
        }
        var initializationResponse =
            await _paystackService.InitializePayment(order.Buyer.User.Email, order.TotalAmount, orderId);
        if (string.IsNullOrEmpty(initializationResponse.Data.AuthorizationUrl))
        {
            return new MakePaymentResponseModel()
            {
                Status = false,
                Message = "Failed to initialize payment"
            };
        }

        order.PaymentReference = initializationResponse.Data.Reference;
        await _orderRepository.UpdateOrderAsync(order);
        return new MakePaymentResponseModel()
        {
            Status = true,
            Message = "Successfully initialized payment",
            AuthorizationUrl = initializationResponse.Data.AuthorizationUrl
        };
    }

    public async Task<BaseResponse> HandlePaymentVerification(string reference)
    {
        var verifyResponse =await _paystackService.VerifyPayment(reference);
        if (!verifyResponse.Status && verifyResponse.Data.Status != "success")
        {
            return new BaseResponse()
            {
                Status = false,
                Message = "Payment verification failed"
            };
        }

        var order = await _orderRepository.GetByPaymentReferenceAsync(reference);
        if (order == null)
        {
            return new BaseResponse()
            {
                Status = false,
                Message = "Order not found"
            };
        }

        order.Status = OrderStatus.Paid;
        await _orderRepository.UpdateOrderAsync(order);
        return new BaseResponse()
        {
            Status = true,
            Message = "Successfully verified payment"
        };
    }

    public async Task<GetOrderResponseModel> GetOrderById(string orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            return new GetOrderResponseModel()
            {
                Status = false,
                Message = "Order not found"
            };
        }

        return new GetOrderResponseModel()
        {
            Status = true,
            Message = "Successfully retrieved order",
            Data = new OrderDTO()
            {
                BuyerId = order.Buyer.Id,
                TotalAmount = order.TotalAmount,
                BuyerName = order.Buyer.User.Username,
                DeliveryAddress = new AddressDTO()
                {
                    Street = order.DeliveryAddress.Street,
                    City = order.DeliveryAddress.City,
                    State = order.DeliveryAddress.State,
                    Country = order.DeliveryAddress.Country
                },
                OrderId = order.Id,
                Status = order.Status.ToString(),
                SubOrders = order.SubOrders.Select(x => new SubOrderDTO()
                {
                    SubOrderId = x.Id,
                    Status = x.SubOrderStatus.ToString(),
                    SubTotal = x.SubTotal,
                    SellerId = x.SellerId,
                    Items = x.SubOrderItems.Select(y => new OrderItemDTO()
                    {
                        ProductId = y.ProductId,
                        ProductName = y.Product.Name,
                        Quantity = y.Quantity,
                        UnitPrice = y.UnitPrice
                    }).ToList()
                }).ToList()
            }
        };
    }

    public async Task<GetOrdersResponseModel> GetOrdersByBuyerId(string buyerId)
    {
        var orders = await _orderRepository.GetByBuyerIdAsync(buyerId);
        if (orders.Count == 0)
        {
            return new GetOrdersResponseModel()
            {
                Status = false,
                Message = "Buyer does not have any order history"
            };
        }

        return new GetOrdersResponseModel()
        {
            Status = true,
            Message = "Successfully retrieved orders",
            Data = orders.Select(x => new OrderDTO()
            {
                BuyerId = x.Buyer.Id,
                TotalAmount = x.TotalAmount,
                BuyerName = x.Buyer.User.Username,
                DeliveryAddress = new AddressDTO()
                {
                    Street = x.DeliveryAddress.Street,
                    City = x.DeliveryAddress.City,
                    State = x.DeliveryAddress.State,
                    Country = x.DeliveryAddress.Country
                },
                OrderId = x.Id,
                Status = x.Status.ToString(),
                SubOrders = x.SubOrders.Select(x => new SubOrderDTO()
                {
                    SellerId = x.SellerId,
                    Status = x.SubOrderStatus.ToString(),
                    SubTotal = x.SubTotal,
                    Items = x.SubOrderItems.Select(x => new OrderItemDTO()
                    {
                        ProductId = x.ProductId,
                        ProductName = x.Product.Name,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice
                    }).ToList()
                }).ToList()
            }).ToList()
        };
    }

    public async Task<GetSubordersResponseModel> GetSubordersBySellerId(string sellerId)
    {
        var suborders = await _subOrderRepository.GetBySellerId(sellerId);
        if (suborders.Count == 0)
        {
            return new GetSubordersResponseModel()
            {
                Status = false,
                Message = "Seller does not have any suborder history"
            };
        }

        return new GetSubordersResponseModel()
        {
            Status = true,
            Message = "Successfully retrieved suborders",
            Data = suborders.Select(x => new SubOrderDTO()
            {
                SubOrderId = x.Id,
                Status = x.SubOrderStatus.ToString(),
                SubTotal = x.SubTotal,
                SellerId = x.SellerId,
                Items = x.SubOrderItems.Select(x => new OrderItemDTO()
                {
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                }).ToList()
            }).ToList()
        };
    }

    public async Task<BaseResponse> NotifySellers(string orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            return new BaseResponse()
            {
                Status = false,
                Message = "Order does not exist"
            };
        }

        foreach (var suborder in order.SubOrders)
        {
            var seller = await _sellerRepository.GetByIdAsync(suborder.SellerId);
            var emailBody = $@"
            <h3>New Order Received</h3>
            <p>Hello {seller.User.FirstName},</p>
            <p>You have received a new order (Order #{order.Id}) with {suborder.SubOrderItems.Count} item(s).</p>
            <p>Please log in to your dashboard to view details and prepare for shipment.</p>
            <p>
                <a href='link'>
                    View Order Details
                </a>
            </p>";
            await _emailService.SendEmailAsync(seller.User.Email,"New Order Notification", emailBody,true);
            var notification = new Notification
            {
                UserId = seller.UserId,
                Title = "New Order",
                Message = $"Order #{order.Id} with {suborder.SubOrderItems.Count} items. Click to view details.",
                Url = $"/seller/orders/{suborder.Id}"
            };
            await _notificationRepository.AddAsync(notification);
        }

        return new BaseResponse()
        {
            Status = true,
            Message = "Successfully notified seller"
        };

    }
    
}