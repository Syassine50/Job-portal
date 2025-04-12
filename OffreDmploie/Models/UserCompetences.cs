namespace OffreDmploie.Models
{
    public class UserCompetences
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }

        public int CompetenceId { get; set; }
        public Competence? Competence { get; set; }
    }
}
