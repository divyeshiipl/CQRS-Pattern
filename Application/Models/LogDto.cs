namespace Application.Models;

public class LogDto
{
    public string? Message { get; set; }
    public string? IPAddress { get; set; }
    public string? Request { get; set; }
    public string? Response { get; set; }
    public string? Action { get; set; }
    public string? RequestURL { get; set; }
    public int? StatusCode { get; set; }
    public string? Verb { get; set; }
    public string? Matchine { get; set; }
}
