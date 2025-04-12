using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OffreDmploie.Models
{
    public class Candidature
    {
        public int Id { get; set; }

        public string DescriptionCondidature { get; set; } = null!;

        public  DateOnly? Datedesoumis { get; set; } = DateOnly.FromDateTime( DateTime.Today);

        public string Statusdecondidature { get; set; } = "En cours"; //[En cours,Acceptée , Rejetée]
        public byte[]? ResumeFile { get; set; }

        public string? ResumeFileName { get; set; }

        public string? ResumeContentType { get; set; }
        [NotMapped]
        public IFormFile? Resume { get; set; }

        public int IdJob { get; set; }

        public string? IdUser { get; set; } 

        public virtual Job? Job { get; set; } = null!;

        public virtual User? User { get; set; } = null!;
    }
}
