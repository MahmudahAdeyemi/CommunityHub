namespace ComuunityHub.DTOs;

public record CategoryDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<CategoryDTO> SubCategories { get; set; } = [];
}