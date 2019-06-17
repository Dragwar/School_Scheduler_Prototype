using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using School_Scheduler.MVC.Models.Domain;

namespace School_Scheduler.MVC.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Represents the <see cref="string"/> Name of a <see cref="ApplicationUser"/>
        /// </summary>
        public string Name { get; set; }

        public bool IsInstructor { get; set; } = false;



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<SchoolProgram> SchoolPrograms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<Student> Students { get; set; }


        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create() => new ApplicationDbContext();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new InstructorConfig());
            modelBuilder.Configurations.Add(new SchoolProgramConfig());
            modelBuilder.Configurations.Add(new CourseConfig());
            modelBuilder.Configurations.Add(new ClassRoomConfig());
            modelBuilder.Configurations.Add(new StudentConfig());
        }
    }
}