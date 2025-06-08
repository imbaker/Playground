namespace Playground.Models;

public record Response<T>
{
    public required string Status { get; set; }
    public int Code { get; set; }
    public required string Locale { get; set; }
    public required string Seed { get; set; }
    public int Total { get; set; }
    public required List<T> Data { get; set; }
}