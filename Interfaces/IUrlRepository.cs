using UrlShortener.Models;

namespace UrlShortener.Interfaces
{
    public interface IUrlRepository
    {
        Task<IEnumerable<Url>> GetAllAsync();
        Task<Url> GetByGuidAsync(Guid guid);
        Task<Url> CreateAsync(string longUrl, string shortUrl, string createdBy);
        Task<bool> UpdateByGuidAsync(Guid guid, Url url);
        Task<bool> Delete(Guid guid);
        Task<bool> SaveAllAsync();
        Task<Url> FindUrlAsync(string longUrl);
        Task<Url> FindUrlByShortUrlAsync(string shortUrl);
    }
}