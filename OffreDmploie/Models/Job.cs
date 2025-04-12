using Microsoft.AspNetCore.Identity;

namespace OffreDmploie.Models
{
    public class Job
    {
        public int Id { get; set; }

        public string NomDeLoffre { get; set; } = null!;
        public string Descriptiondeloffre { get; set; } = null!;

        public DateOnly? Datecreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public DateOnly Datefin { get; set; }

        public string Lieu { get; set; } = null!;

        public string Typedeloffre { get; set; } = null!;

        public string Typedecontrat { get; set; } = null!;

        public string? Statusdeloffre { get; set; } = "Ouvert"; //[Ouvert , Fermé]

        public int IdDomaine { get; set; }

        public string? IdUser { get; set; } = null!;

        public virtual ICollection<Candidature> Candidatures { get; set; } = new List<Candidature>();

        public virtual ICollection<Competence> Competences { get; set; } = new List<Competence>();

        public virtual Domaine? Domaine { get; set; } = null!;

        public virtual User? User { get; set; } = null!;
    }
}
