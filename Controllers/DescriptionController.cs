using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.DTOs;
using UrlShortener.Interfaces;

namespace UrlShortener.Controllers
{
    [Route("[controller]")]
    public class DescriptionController : ControllerBase
    {
        private readonly IDescriptionRepository _descriptionRepository;

        public DescriptionController(IDescriptionRepository descriptionRepository)
        {
            _descriptionRepository = descriptionRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateDescription")]
        public async Task<IActionResult> CreateDescription([FromBody] string value)
        {
            try
            {
                var description = await _descriptionRepository.CreateDescriptionAsync(value);
                return Ok(description);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDescription/{id}")]
        public async Task<IActionResult> GetDescriptionById(int id)
        {
            try
            {
                var description = await _descriptionRepository.GetDescriptionByIdAsync(id);
                if (description == null)
                {
                    return NotFound();
                }

                return Ok(description);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateDescription")]
        public async Task<IActionResult> UpdateDescription(DescriptionUpdateDto updateDto)
        {
            try
            {
                var updatedDescription = await _descriptionRepository.UpdateDescriptionAsync(updateDto);
                if (updatedDescription == null)
                {
                    return NotFound();
                }

                return Ok(updatedDescription);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteDescription/{id}")]
        public async Task<IActionResult> DeleteDescription(int id)
        {
            try
            {
                var existingDescription = await _descriptionRepository.GetDescriptionByIdAsync(id);
                if (existingDescription == null)
                {
                    return NotFound();
                }

                await _descriptionRepository.DeleteDescriptionAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}