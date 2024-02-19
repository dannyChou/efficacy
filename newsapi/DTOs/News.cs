namespace newsapi.DTOs;

public class News
{
    public int NewsId { get; set; }

    public string? Content { get; set; }

    public DateTime CreateTimeStamp { get; set; }
}
