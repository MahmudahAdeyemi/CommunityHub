using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ComuunityHub.DTOs;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Interfaces.Services;
using ComuunityHub.Models;
using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;

namespace ComuunityHub.Implementation.Services;

public class SellerService : ISellerService
{
    private readonly ISellerRepository _sellerRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICommunityRepository _communityRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommunityMemberRepository _communityMemberRepository;
    private readonly IAddressRepository _addressRepository;

    public SellerService(ISellerRepository sellerRepository, IUserRepository userRepository, ICommunityRepository communityRepository, IHttpContextAccessor httpContextAccessor, ICommunityMemberRepository communityMemberRepository, IAddressRepository addressRepository)
    {
        _sellerRepository = sellerRepository;
        _userRepository = userRepository;
        _communityRepository = communityRepository;
        _httpContextAccessor = httpContextAccessor;
        _communityMemberRepository = communityMemberRepository;
        _addressRepository = addressRepository;
    }

    public async Task<BaseResponse> RegisterSeller(string communityId,RegisterSellerRequestModel request)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var user = await _userRepository.GetUserAsync(userId);
        var community = await _communityRepository.GetByIdAsync(communityId);
        if (community == null)
        {
            return new BaseResponse
            {
                Message = "Community doesn't exist",
                Status = false
            };
        }
        var existingSeller = await _sellerRepository.GetByUserAndCommunityAsync(userId, communityId);
        if (existingSeller != null)
        {
            return new BaseResponse()
            {
                Message = "User is already a seller in this community",
                Status = false
            };
        }

        Address address = new Address()
        {
            City = request.Address.City,
            Country = request.Address.Country,
            PostalCode = request.Address.PostalCode,
            Street = request.Address.Street,
            Landmark = request.Address.Landmark,
            UserId = userId,
            State = request.Address.State
        };
        address = await _addressRepository.AddAsync(address);
        Seller seller = new Seller()
        {
            CommunityId = communityId,
            UserId = userId,
            Status = SellerStatus.Pending,
            StoreName = request.StoreName,
            Products = new List<Product>(),
            Address = address,
            AddressId = address.Id
            
        };
        await _sellerRepository.AddAsync(seller);
        var member = await _communityMemberRepository.GetMembershipAsync(userId, communityId);
        CommunityMemberRole communityMemberRole = new CommunityMemberRole()
        {
            CommunityMemberId = member.Id,
            Role = CommunityRole.Seller
        };
        member.CommunityRole.Add(communityMemberRole);
        await _communityMemberRepository.UpdateMemberAsync(member);
        return new BaseResponse()
        {
            Message = "Seller has been registered successfully",
            Status = true
        };
    }

    public async Task<SellerResponseModel> GetSellerById(string sellerId)
    {
        var seller = await _sellerRepository.GetByIdAsync(sellerId);
        if (seller == null)
        {
            return new SellerResponseModel
            {
                Message = "Seller doesn't exist",
                Status = false
            };
        }

        return new SellerResponseModel()
        {
            Status = true,
            Data = new SellerDTO()
            {
                Username = seller.User.Username,
                CommunityName = seller.Community.Name
            },
            Message = "Seller has been returned successfully",
        };
    }
    
    public async Task<SellersResponseModel> GetSellersByCommunityAsync(string communityId)
    {
        var sellers = await _sellerRepository.GetByCommunityAsync(communityId);

        return new SellersResponseModel()
        {
            Status = true,
            Data = sellers.Select(s => new SellerDTO()
            {
                Username = s.User.Username,
                CommunityName = s.Community.Name
            }).ToList()
        };
        
    }
    public async Task<BaseResponse> DeleteSellerAsync(string sellerId)
    {
        var seller = await _sellerRepository.GetByIdAsync(sellerId);
        if (seller == null)
        {
            return new BaseResponse { Status = false, Message = "Seller not found" };
        }

        await _sellerRepository.DeleteAsync(seller);

        return new BaseResponse { Status = true, Message = "Seller deleted successfully" };
    }
    
    public async Task<BaseResponse> ApproveSellerAsync(string sellerId)
    {
        var seller = await _sellerRepository.GetByIdAsync(sellerId);
        if (seller == null) return new BaseResponse { Status = false, Message = "Seller not found" };

        seller.Status = SellerStatus.Approved;
        await _sellerRepository.UpdateAsync(seller);

        return new BaseResponse { Status = true, Message = "Seller approved successfully" };
    }
    
    public async Task<BaseResponse> RejectSellerAsync(string sellerId)
    {
        var seller = await _sellerRepository.GetByIdAsync(sellerId);
        if (seller == null) return new BaseResponse { Status = false, Message = "Seller not found" };

        seller.Status = SellerStatus.Rejected;
        await _sellerRepository.UpdateAsync(seller);

        return new BaseResponse { Status = true, Message = "Seller rejected successfully" };
    }
}