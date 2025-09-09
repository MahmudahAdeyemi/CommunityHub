namespace ComuunityHub.Models;

public class Post
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string CommunityId { get; set; }
    public Community Community { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Reaction> Reactions { get; set; }
}