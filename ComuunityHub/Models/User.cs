namespace ComuunityHub.Models;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Password { get; set; }
    public Roles Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ICollection<CommunityMember> Members { get; set; }
    public ICollection<Post> Posts { get; set; }
    public ICollection<Product> Products { get; set; }
    public ICollection<Job> Jobs { get; set; }
    public ICollection<Service> Services { get; set; }
}