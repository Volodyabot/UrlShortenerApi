using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class Description
    {
        [Key]
        public int Id { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
        public required string Value { get; set; }

    }
}