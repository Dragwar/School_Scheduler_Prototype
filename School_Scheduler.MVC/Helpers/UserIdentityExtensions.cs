using System;
using System.Security.Claims;
using System.Security.Principal;

namespace School_Scheduler.MVC.Helpers
{
    public static class UserIdentityExtensions
    {
        public enum Discriminator
        {
            Null,
            ApplicationUser,
            Instructor,
            Student
        }
        public static Discriminator GetDiscriminator(this IIdentity identity)
        {
            Claim claim = ((ClaimsIdentity)identity).FindFirst("Discriminator");
            if (Enum.TryParse(claim.Value, out Discriminator result))
            {
                return result;
            }

            return Discriminator.Null; /*(claim != null) ? claim.Value : string.Empty;*/
        }
    }
}