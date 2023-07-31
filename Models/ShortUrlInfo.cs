using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class ShortUrlInfo
    {
        [Key]
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid UrlId { get; set; }
    }
}