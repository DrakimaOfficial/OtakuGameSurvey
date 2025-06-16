using CoreMvc.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CoreMvc.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly GameSurveyDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SurveyService(GameSurveyDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Genre>> GetGenresAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task SubmitResponseAsync(int genreId, int preferenceScore)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User must be logged in to submit responses.");
            }

            var response = new SurveyResponse
            {
                UserId = userId,
                GenreId = genreId,
                PreferenceScore = preferenceScore,
                SubmittedAt = DateTime.UtcNow
            };

            _context.SurveyResponses.Add(response);
            await _context.SaveChangesAsync();
        }

        public async Task<Genre> GetTopGenreAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            return await _context.SurveyResponses
                .Where(r => r.UserId == userId)
                .Include(r => r.Genre)
                .GroupBy(r => r.Genre)
                .Select(g => new
                {
                    Genre = g.Key,
                    AverageScore = g.Average(r => r.PreferenceScore)
                })
                .OrderByDescending(x => x.AverageScore)
                .Select(x => x.Genre)
                .FirstOrDefaultAsync();
        }
    }
}