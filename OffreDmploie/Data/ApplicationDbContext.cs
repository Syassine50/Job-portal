using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OffreDmploie.Models;

namespace OffreDmploie.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Job> Jobs { get; set; }
    public DbSet<Candidature> Candidatures { get; set; }
    public DbSet<Competence> Competences { get; set; }
    public DbSet<Domaine> Domaines { get; set; }
    public DbSet<UserCompetences> UserCompetences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Job Entity Configuration
        modelBuilder.Entity<Job>()
            .HasKey(j => j.Id);

        modelBuilder.Entity<Job>()
            .HasOne(j => j.Domaine)
            .WithMany(d => d.Jobs)
            .HasForeignKey(j => j.IdDomaine)
            .OnDelete(DeleteBehavior.Restrict);
        // ✅ Éviter la suppression en cascade

        modelBuilder.Entity<Job>()
            .HasOne(j => j.User)
            .WithMany(u => u.Jobs)
            .HasForeignKey(j => j.IdUser)
            .OnDelete(DeleteBehavior.Restrict); // ✅ Éviter la suppression en cascade

        // Candidature Entity Configuration
        modelBuilder.Entity<Candidature>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Candidature>()
            .HasOne(c => c.Job)
            .WithMany(j => j.Candidatures)
            .HasForeignKey(c => c.IdJob)
            .OnDelete(DeleteBehavior.Restrict); // ✅ Éviter la suppression en cascade

        modelBuilder.Entity<Candidature>()
            .HasOne(c => c.User)
            .WithMany(u => u.Candidatures)
            .HasForeignKey(c => c.IdUser)
            .OnDelete(DeleteBehavior.Restrict); // ✅ Éviter la suppression en cascade

        // Competence Entity Configuration
        modelBuilder.Entity<Competence>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Competence>()
            .HasOne(c => c.Domaine)
            .WithMany(j => j.Competences)
            .HasForeignKey(c => c.IdDomaine)
            .OnDelete(DeleteBehavior.Restrict); // ✅ Éviter la suppression en cascade

        // UserCompetence (Many-to-Many) Configuration
        modelBuilder.Entity<UserCompetences>()
            .HasKey(uc => new { uc.UserId, uc.CompetenceId });

        modelBuilder.Entity<UserCompetences>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserCompetences)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Restrict); // ✅ Éviter la suppression en cascade

        modelBuilder.Entity<UserCompetences>()
            .HasOne(uc => uc.Competence)
            .WithMany(u => u.UserCompetences)
            .HasForeignKey(uc => uc.CompetenceId)
            .OnDelete(DeleteBehavior.Restrict); // ✅ Éviter la suppression en cascade

        // Domaine Entity Configuration
        modelBuilder.Entity<Domaine>()
            .HasKey(d => d.id);

        modelBuilder.Entity<Domaine>()
            .Property(d => d.Nomdomaine)
            .IsRequired()
            .HasMaxLength(255);
    }
}