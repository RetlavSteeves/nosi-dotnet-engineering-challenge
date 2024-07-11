using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ContentDb
{
   [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
   [Key]
    public Guid Id { get; set;}
    public string? Title { get; set; }
    public string? SubTitle { get; set;}
    public string? Description { get; set;}
    public string? ImageUrl { get; set;}
    public int Duration { get; set;}
    public DateTime StartTime { get; set;}
    public DateTime EndTime { get; set;}
    public IEnumerable<string>? GenreList { get; set;}
}
