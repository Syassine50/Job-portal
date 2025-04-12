namespace OffreDmploie.Models
{
    public class Domaine
    {
        public int id { get; set; }

        public string Nomdomaine { get; set; } = null!;

        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
        public virtual ICollection<Competence> Competences { get; set; } = new List<Competence>();
    }
}
