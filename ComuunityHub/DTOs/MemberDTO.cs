namespace ComuunityHub.DTOs;

public record MemberDTO
{
    public string Username { get; init; }
    public string Email { get; init; }
    public DateTime JoinedAt { get; init; }
}