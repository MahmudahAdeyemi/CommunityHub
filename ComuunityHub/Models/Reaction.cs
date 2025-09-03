namespace ComuunityHub.Models;

public class Reaction
{
    public string Id { get; set; }
    public string PostId { get; set; }
    public string UserId { get; set; }
    public ReactionType Type { get; set; }
    public Post Post { get; set; }
    public User User { get; set; }
}