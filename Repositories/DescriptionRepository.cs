using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.DTOs;
using UrlShortener.Interfaces;
using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public class DescriptionRepository : IDescriptionRepository
    {
        private readonly DataContext _dataContext;

        public DescriptionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Description> CreateDescriptionAsync(string value)
        {
            var description = new Description
            {
                UpdateDate = DateTime.UtcNow,
                Value = value
            };

            var result = _dataContext.Descriptions.Add(description);

            await _dataContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Description> GetDescriptionByIdAsync(int id)
        {
            var result = await _dataContext.Descriptions.FirstOrDefaultAsync(d => d.Id == id);

            if (result == null)
            {
                throw new Exception("Description not found.");
            }
            return result;
        }

        public async Task<Description> UpdateDescriptionAsync(DescriptionUpdateDto updateDto)
        {
            var existingDescription = await _dataContext.Descriptions.FirstOrDefaultAsync(d => d.Id == updateDto.Id); ;
            if (existingDescription == null)
            {
                throw new Exception("Description not found.");
            }

            existingDescription.UpdateDate = DateTime.UtcNow;
            existingDescription.Value = updateDto.Value;

            await _dataContext.SaveChangesAsync();

            return existingDescription;
        }

        public async Task DeleteDescriptionAsync(int id)
        {
            var description = await _dataContext.Descriptions.FirstOrDefaultAsync(d => d.Id == id);
            if (description != null)
            {
                _dataContext.Descriptions.Remove(description);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}