using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace newsapi.EFs;

[Table("News2")]
public class News2
{
    [Key]
    public int NewsId { get; set; }

    public string? Content { get; set; }

    public DateTime CreateTimeStamp { get; set; }
}
