using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace newsapi.EFs;

[Table("News1")]
public class News1
{
    [Key]
    public int NewsId { get; set; }

    public string? Content { get; set; }

    public DateTime CreateTimeStamp { get; set; }
}
