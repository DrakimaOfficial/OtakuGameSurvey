using CoreMvc.Models;
using CoreMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;

        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        [HttpGet("Genres")]
        public async Task<ActionResult<List<Genre>>> GetGenres()
        {
            var genres = await _surveyService.GetGenresAsync();
            return Ok(genres);
        }

        [HttpPost("Submit")]
        public async Task<IActionResult> SubmitResponse([FromBody] ResponseDto responseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _surveyService.SubmitResponseAsync(responseDto.GenreId, responseDto.PreferenceScore);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("TopGenre")]
        public async Task<ActionResult<Genre>> GetTopGenre()
        {
            var topGenre = await _surveyService.GetTopGenreAsync();
            if (topGenre == null)
            {
                return NotFound();
            }
            return Ok(topGenre);
        }
    }

    public class ResponseDto
    {
        public int GenreId { get; set; }
        public int PreferenceScore { get; set; }
    }
}