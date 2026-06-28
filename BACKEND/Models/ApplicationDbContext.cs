using Microsoft.EntityFrameworkCore;

namespace BACKEND.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //Put tables into Database
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Technology> Tecnologies { get; set; }
        public virtual DbSet<ProjectTechnology> ProjectTecnologies { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<ContactMessage> ContactMessage { get; set; }
        public virtual DbSet<BlackList> BlackList { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>().HasData(new Role { RoleId = new Guid("11111111-1111-1111-1111-111111111111"), Name = "Admin", IsActive = true, Order = 1 });
            modelBuilder.Entity<Role>().HasData(new Role { RoleId = new Guid("22222222-2222-2222-2222-222222222222"), Name = "User", IsActive = true, Order = 2 });
            modelBuilder.Entity<Role>().HasData(new Role { RoleId = new Guid("33333333-3333-3333-3333-333333333333"), Name = "Viewer", IsActive = true, Order= 3 });

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

            modelBuilder.Entity<Project>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<BlackList>(entity =>
            {
                entity.Property(e => e.ExpirationDate)
                .HasColumnType("timestamp with time zone");
                entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
                entity.ToTable("BlackList");

                entity.HasOne(b => b.User)
                  .WithMany()
                  .HasForeignKey(b => b.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            });

        }
    }
}
