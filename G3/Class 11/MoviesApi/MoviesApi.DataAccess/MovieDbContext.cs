using Microsoft.EntityFrameworkCore;
using MoviesApi.Domain.Models;

namespace MoviesApi.DataAccess
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasOne(x => x.User)
                .WithMany(x => x.Movies)
                .HasForeignKey(x => x.UserId);

            //modelBuilder.Entity<User>()
            //    .HasMany(x => x.Movies)
            //    .WithOne(x => x.User)
            //    .HasForeignKey(x => x.UserId);
            //    

            modelBuilder.Entity<Movie>()
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Movie>()
                .Property(x => x.Year)
                .IsRequired();

            modelBuilder.Entity<Movie>()
                .Property(x => x.Genre)
                .IsRequired();

            modelBuilder.Entity<Movie>()
                .Property(x => x.Description)
                .HasMaxLength(250);

            modelBuilder.Entity<User>()
                .Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(x => x.Password)
                .IsRequired();

            modelBuilder.Entity<User>()
               .Property(x => x.Firstname)
               .IsRequired()
               .HasMaxLength(150);

            modelBuilder.Entity<User>()
              .Property(x => x.Lastname)
              .IsRequired()
              .HasMaxLength(150);

            base.OnModelCreating(modelBuilder);
        }
    }
}
