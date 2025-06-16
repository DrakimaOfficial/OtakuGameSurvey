namespace CoreMvc.Models
{
    public class SurveyResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Changed to string for IdentityUser.Id
        public int GenreId { get; set; }
        public int PreferenceScore { get; set; }
        public DateTime SubmittedAt { get; set; }

        public Genre Genre { get; set; }
    }
}