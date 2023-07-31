using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.DTOs;
using UrlShortener.Interfaces;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlRepository _urlRepository;

        public UrlController(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost("ShortenUrl")]
        public async Task<ActionResult<Url>> ShortenUrl(string longUrl)
        {
            try
            {
                var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Random random = new Random();

                string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                string baseUrl = "https://localhost:7102/Url/";

                string result = baseUrl;

                if (!Uri.TryCreate(longUrl, UriKind.Absolute, out Uri uriResult))
                {
                    return BadRequest("Url is not valid!");
                }

                var foundUrl = await _urlRepository.FindUrlAsync(longUrl);

                if (foundUrl != null)
                {
                    return BadRequest("Url already exists!");
                }

                for (int i = 0; i < 5; i++)
                {
                    result += characters[random.Next(characters.Length)];
                }

                var createdUrl = await _urlRepository.CreateAsync(longUrl, result, userName!);

                if (createdUrl == null)
                {
                    return BadRequest("Not created");
                }

                return Ok(createdUrl);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetAllUrls")]
        public async Task<ActionResult<IEnumerable<Url>>> GetAllUrls()
        {
            var urlList = await _urlRepository.GetAllAsync();

            return Ok(urlList);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete("Delete/{guid}")]
        public async Task<IActionResult> DeleteUrlByGuid(Guid guid)
        {
            try
            {
                var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                var url = _urlRepository.GetByGuidAsync(guid);

                if (userRole != "Admin" && url.Result.ShortUrlInfo.CreatedBy != userName)
                {
                    return BadRequest("Not allowed");
                }

                var isDeleted = await _urlRepository.Delete(guid);

                if (!isDeleted)
                {
                    return NotFound("object not found");
                }

                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Get/{guid}")]
        public async Task<ActionResult<Url>> GetByGuid(Guid guid)
        {
            var url = await _urlRepository.GetByGuidAsync(guid);

            if (url == null)
            {
                return NotFound("url not found");
            }

            return url;
        }

        [HttpGet("{shortUrlCode}")]
        public async Task<IActionResult> RedirectUrl(string shortUrlCode)
        {
            string baseUrl = "https://localhost:7102/Url/";

            try
            {
                var url = await _urlRepository.FindUrlByShortUrlAsync(baseUrl + shortUrlCode);
                if (url == null)
                {
                    return BadRequest($"didnt find [{baseUrl + shortUrlCode}]");
                }
                return Redirect(url.LongUrl);
            }
            catch (System.Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

    }
}