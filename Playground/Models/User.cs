namespace Playground.Models;

public record User
{
    public required int Id { get; init; }
    public string? Uuid { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public string? Email { get; set; }
    public required string Ip { get; set; }
    public required string MacAddress { get; set; }
    public required string Website { get; set; }
    public required string Image { get; set; }
}