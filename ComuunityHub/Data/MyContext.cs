using ComuunityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ComuunityHub.Data;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options)  : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Community> Communities { get; set; }
    public DbSet<CommunityMember> CommunityMembers { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Employer> Employers { get; set; }
    public DbSet<JobSeeker> JobSeekers { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Provider> ServiceProviders { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Booking> Bookings { get; set; }
}