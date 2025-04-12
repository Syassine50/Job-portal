using Microsoft.AspNetCore.Identity;

namespace OffreDmploie.Models
{
    public class Competence
    {
        public int Id { get; set; }

        public string Nomcompetences { get; set; } = null!;

        public int IdDomaine { get; set; }

        public virtual Domaine? Domaine { get; set; } = null!;

        public virtual ICollection<User> IdUsers { get; set; } = new List<User>();

        public virtual ICollection<UserCompetences> UserCompetences { get; set; } = new List<UserCompetences>();
    }
}
