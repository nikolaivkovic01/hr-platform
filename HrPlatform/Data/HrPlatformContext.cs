using Microsoft.EntityFrameworkCore;
using HrPlatform.Models;


namespace HrPlatform.Data
{
    public class HrPlatformContext : DbContext
    {
        public HrPlatformContext(DbContextOptions<HrPlatformContext> options) : base(options) { } 
        
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<CandidateSkill> CandidatesSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CandidateSkill>()
                .HasKey(cs => new {cs.CandidateId, cs.SkillId});


            modelBuilder.Entity<CandidateSkill>()
                .HasOne(cs => cs.Candidate)
                .WithMany(c => c.CandidateSkills)
                .HasForeignKey(cs => cs.CandidateId);

            modelBuilder.Entity<CandidateSkill>()
                .HasOne(cs => cs.Skill)
                .WithMany(s => s.CandidateSkills)
                .HasForeignKey(cs => cs.SkillId);

            modelBuilder.Entity<Candidate>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Skill>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<Skill>().HasData(
                
                new Skill { Id = 1, Name = "Java Programming"},
                new Skill { Id = 2, Name = "C# programming"},
                new Skill { Id = 3, Name = "Database design"},
                new Skill { Id = 4, Name = "English" },
                new Skill { Id = 5, Name = "Russian"},
                new Skill { Id = 6, Name = "German"}
                  
                );

            modelBuilder.Entity<Candidate>().HasData(
                
                new Candidate
                {
                    Id = 1,
                    Name = "Nikola Ivkovic",
                    DateOfBirth = new DateOnly(2001,6,21),
                    ContactNumber = "+381641408818",
                    Email = "nikola.ivkovic.contact@gmail.com"
                },
                new Candidate
                {
                    Id = 2,
                    Name = "Marko Markovic",
                    DateOfBirth = new DateOnly(1998,5,14),
                    ContactNumber = "+381612649973",
                    Email = "marko.markovic.contact@gmail.com"
                }
                
                );

            modelBuilder.Entity<CandidateSkill>().HasData(
                
                new CandidateSkill { CandidateId = 1, SkillId = 2},
                new CandidateSkill { CandidateId = 1, SkillId = 3},
                new CandidateSkill { CandidateId = 1, SkillId = 4},
                new CandidateSkill { CandidateId = 2, SkillId = 1},
                new CandidateSkill { CandidateId = 2, SkillId = 4}
                
                );

            
        }
    }
}
