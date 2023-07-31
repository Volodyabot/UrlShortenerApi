using UrlShortener.DTOs;
using UrlShortener.Models;

namespace UrlShortener.Interfaces
{
    public interface IDescriptionRepository
    {
        Task<Description> CreateDescriptionAsync(string value);
        Task<Description> GetDescriptionByIdAsync(int id);
        Task<Description> UpdateDescriptionAsync(DescriptionUpdateDto updateDto);
        Task DeleteDescriptionAsync(int id);
    }
}