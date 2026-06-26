using Microsoft.EntityFrameworkCore;

namespace BACKEND.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //Put tables into Database
        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Technology> Tecnologies { get; set; }
        public DbSet<ProjectTechnology> ProjectTecnologies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ContactMessage> ContactMessage { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectTechnology>()
                .HasKey(pt => new { pt.ProjectId, pt.TechnologyId });

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name).IsUnique();

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name).IsUnique();
            
            modelBuilder.Entity<RefreshToken>()
                .HasKey(rt => rt.TokenId);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.Token).IsUnique();


            modelBuilder.Entity<Technology>()
                .HasIndex(t => t.Name).IsUnique();
        }
    }
}
