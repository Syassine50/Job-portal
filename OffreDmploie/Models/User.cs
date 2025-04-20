using Microsoft.AspNetCore.Identity;

namespace OffreDmploie.Models
{
    public class User : IdentityUser
    {

        public string? Nometprenom {  get; set; }
        public string? Nomdentreprise { get; set; }
        public virtual ICollection<Candidature> Candidatures { get; set; } = new List<Candidature>();

        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

        public virtual ICollection<UserCompetences> UserCompetences { get; set; } = new List<UserCompetences>();
    }
}
