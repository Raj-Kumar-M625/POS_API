using Microsoft.EntityFrameworkCore;
using Comments.API.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Comment = Comments.API.Data.Models.Comments;

namespace Comments.API.Data
{
    public class CommentsContext : IdentityDbContext<User, Role, int,
    UserClaim, UserRole, UserLogin,
    RoleClaim, UserToken>
    {
        public CommentsContext(DbContextOptions<CommentsContext> options):base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<CommentHierarchy> CommentHierarchy { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Identity table modifications
            ModifyIdentityTables(modelBuilder);
            //Seeding
            SeedRoles(modelBuilder);

            //modelBuilder.Entity<EmployeeTask>()
            // .HasOne(et => et.Employee)
            // .WithMany()
            // .HasForeignKey(et => et.EmployeeId);

            //modelBuilder.Entity<TeamEmployee>()
            //    .HasOne(te => te.Employee)
            //    .WithMany()
            //    .HasForeignKey(te => te.EmployeeId);

        }
        private void ModifyIdentityTables(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasIndex(u => u.PhoneNumber).IsUnique();

                // Each User can have many UserClaims
                entity.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                entity.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                entity.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                entity.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                // Each Role can have many entries in the UserRole join table
                entity.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                entity.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            builder.Entity<UserRole>(entity => { entity.ToTable("UserRole"); });
            builder.Entity<UserClaim>(entity => { entity.ToTable("UserClaim"); });
            builder.Entity<UserLogin>(entity => { entity.ToTable("UserLogin"); });
            builder.Entity<UserToken>(entity => { entity.ToTable("UserToken"); });
            builder.Entity<RoleClaim>(entity => { entity.ToTable("RoleClaim"); });

        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "SuperAdmin",
                    ConcurrencyStamp = "1",
                    NormalizedName = "SuperAdmin"
                },
                new Role
                {
                    Id = 2,
                    Name = "Admin",
                    ConcurrencyStamp = "2",
                    NormalizedName = "Admin"
                },
                new Role
                {
                    Id = 3,
                    Name = "Employee",
                    ConcurrencyStamp = "3",
                    NormalizedName = "Employee"
                },
                new Role
                {
                    Id = 4,
                    Name = "TeamLead",
                    ConcurrencyStamp = "4",
                    NormalizedName = "TeamLead"
                }
            );
        }

    }
}
