using CoreMvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreMvc.Services
{
    public interface ISurveyService
    {
        Task<List<Genre>> GetGenresAsync();
        Task SubmitResponseAsync(int genreId, int preferenceScore);
        Task<Genre> GetTopGenreAsync();
    }
}