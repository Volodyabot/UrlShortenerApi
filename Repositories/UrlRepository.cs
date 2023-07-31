using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Interfaces;
using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly DataContext _dataContext;
        public UrlRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Url> CreateAsync(string longUrl, string shortUrl, string createdBy)
        {
            Url url = new Url
            {
                LongUrl = longUrl,
                ShortUrl = shortUrl,

                ShortUrlInfo = new ShortUrlInfo()
                {
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now
                }
            };

            var createdUrl = _dataContext.Add<Url>(url);

            await _dataContext.SaveChangesAsync();

            return createdUrl.Entity;
        }

        public async Task<bool> Delete(Guid guid)
        {
            var exisistingUrl = await _dataContext.Urls.FirstOrDefaultAsync(u => u.Id == guid);

            if (exisistingUrl == null)
            {
                return false;
            }

            _dataContext.Urls.Remove(exisistingUrl);

            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Url>> GetAllAsync()
        {
            return await _dataContext.Urls
            .Include(url => url.ShortUrlInfo)
            .ToListAsync();
        }

        public async Task<Url> GetByGuidAsync(Guid guid)
        {
            var exisistingUrl = await _dataContext.Urls
            .Include(url => url.ShortUrlInfo)
            .FirstOrDefaultAsync(u => u.Id == guid);

            return exisistingUrl;
        }

        public async Task<bool> UpdateByGuidAsync(Guid guid, Url url)
        {
            var exisistingUrl = await _dataContext.Urls.FirstOrDefaultAsync(u => u.Id == guid);

            if (exisistingUrl == null)
            {
                return false;
            }

            _dataContext.Entry(exisistingUrl).State = EntityState.Modified;

            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<Url> FindUrlAsync(string longUrl)
        {
            var exisistingUrl = await _dataContext.Urls
            .Include(url => url.ShortUrlInfo)
            .FirstOrDefaultAsync(u => u.LongUrl == longUrl);

            return exisistingUrl;
        }

        public async Task<Url> FindUrlByShortUrlAsync(string shortUrl)
        {
            var exisistingUrl = await _dataContext.Urls
            .Include(url => url.ShortUrlInfo)
            .FirstOrDefaultAsync(u => u.ShortUrl == shortUrl);

            return exisistingUrl;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

    }
}