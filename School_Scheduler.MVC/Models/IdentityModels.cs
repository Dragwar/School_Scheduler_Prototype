using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using School_Scheduler.MVC.Models.Domain;
using static School_Scheduler.MVC.Helpers.UserIdentityExtensions;

namespace School_Scheduler.MVC.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        // Ok I'll commit what i have now and comment out the things I was trying (code that doesn't work yet)
        // then when I'm done pushing mine maybe push yours or make a pull request? 
        // Or you stash your changes then fetch mine then compare that way? 
        // Or you can even live share your code as well?
        // Or more ways as well.
        // 
        /// <summary>
        /// Represents the <see cref="string"/> Name of a <see cref="ApplicationUser"/>
        /// </summary>
        public string Name { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            Claim discriminatorClaim;
            switch (this)
            {
                case Instructor _: discriminatorClaim = new Claim("Discriminator", nameof(Discriminator.Instructor)); break;
                case Student _: discriminatorClaim = new Claim("Discriminator", nameof(Discriminator.Student)); break;
                case ApplicationUser _: discriminatorClaim = new Claim("Discriminator", nameof(Discriminator.ApplicationUser)); break;
                default: discriminatorClaim = new Claim("Discriminator", nameof(Discriminator.Null)); break;
            }

            userIdentity.AddClaim(discriminatorClaim);

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public DbSet<Instructor> Instructors { get; set; }
        public DbSet<SchoolProgram> SchoolPrograms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        //public DbSet<Student> Students { get; set; }


        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create() => new ApplicationDbContext();
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine();
                sb.AppendLine();
                foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().FullName, eve.Entry.State));
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                            ve.PropertyName,
                            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                            ve.ErrorMessage));
                    }
                }
                sb.AppendLine();

                bool isDebug = false;
                Debug.Assert(isDebug = true);
                if (!isDebug && !Debugger.IsAttached)
                {
                    Debugger.Launch();
                }
                throw;
            }
        }


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