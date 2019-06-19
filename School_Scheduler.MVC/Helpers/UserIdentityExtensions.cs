using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;
using School_Scheduler.MVC.Models;
using School_Scheduler.MVC.Models.Domain;

namespace School_Scheduler.MVC.Helpers
{
    /// <summary>
    /// User Identity Custom Helpers and Extensions
    /// </summary>
    public static class UserIdentityExtensions
    {
        /// <summary>
        /// Get the <see cref="Discriminator"/> of the <paramref name="user"/>
        /// </summary>
        /// <param name="user">The user you want the <see cref="Discriminator"/> of</param>
        /// <returns><see cref="Discriminator"/> result</returns>
        public static Discriminator GetUserDiscriminator(this ApplicationUser user)
        {
            switch (user)
            {
                case Instructor _: return Discriminator.Instructor;
                case Student _: return Discriminator.Student;
                case ApplicationUser _: return Discriminator.ApplicationUser;
                default: return Discriminator.Null;
            }
        }

        /// <summary>
        /// Get the current user's <see cref="Discriminator"/> via <paramref name="identity"/>
        /// </summary>
        /// <param name="identity">The identity of the current user (<see cref="IIdentity"/> User.Identity.GetUserDiscriminator())</param>
        /// <returns><see cref="Discriminator"/> result</returns>
        public static Discriminator GetUserDiscriminator(this IIdentity identity)
        {
            Claim claim = ((ClaimsIdentity)identity).FindFirst(nameof(Discriminator));
            if (claim != null && Enum.TryParse(claim.Value, out Discriminator result))
            {
                return result;
            }
            return Discriminator.Null;
        }

        /// <summary>
        /// Returns a new <see cref="Claim"/> that represents the <paramref name="user"/>'s <see cref="Discriminator"/>
        /// (doesn't add claim to <paramref name="user"/>)
        /// </summary>
        /// <param name="user">User that will be used to get his/her <see cref="Discriminator"/> for creating a <see cref="Discriminator"/> <see cref="Claim"/></param>
        /// <returns><see cref="Claim"/> that represents the <paramref name="user"/>'s <see cref="Discriminator"/></returns>
        public static Claim CreateDiscriminatorClaim(this ApplicationUser user)
        {
            Claim discriminatorClaim;
            switch (user)
            {
                case Instructor _: discriminatorClaim = new Claim(nameof(Discriminator), nameof(Discriminator.Instructor)); break;
                case Student _: discriminatorClaim = new Claim(nameof(Discriminator), nameof(Discriminator.Student)); break;
                case ApplicationUser _: discriminatorClaim = new Claim(nameof(Discriminator), nameof(Discriminator.ApplicationUser)); break;
                default: discriminatorClaim = new Claim(nameof(Discriminator), nameof(Discriminator.Null)); break;
            }
            return discriminatorClaim;
        }

        /// <summary>
        /// Queries the Database to retrieve the Current User,
        /// then the user will be compared via the <see langword="is"/> keyword to the <typeparamref name="TAppUserType"/>
        /// </summary>
        /// <typeparam name="TAppUserType"><see cref="ApplicationUser"/> or any derived <see langword="class"/> of <see cref="ApplicationUser"/></typeparam>
        /// <returns><see cref="ApplicationUser"/> <see langword="is"/> <typeparamref name="TAppUserType"/></returns>
        public static bool IsCurrentUserType<TAppUserType>()
            where TAppUserType : ApplicationUser
        {
            string currentUserId = HttpContext.Current.User.Identity.GetUserId();
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return false;
            }
            ApplicationUser currentUser;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                currentUser = db.Users.FirstOrDefault(user => user.Id == currentUserId);
            }
            return currentUser?.IsType<TAppUserType>() ?? false;
        }

        /// <summary>
        /// Very simple method that is just returns the result of this: <paramref name="userToCheck"/> <see langword="is"/> <typeparamref name="TAppUserType"/>
        /// </summary>
        /// <typeparam name="TAppUserType">The type to check against the <paramref name="userToCheck"/>'s type</typeparam>
        /// <param name="userToCheck">The <paramref name="userToCheck"/> is to check if his/her's type matches <typeparamref name="TAppUserType"/></param>
        /// <returns><paramref name="userToCheck"/> <see langword="is"/> <typeparamref name="TAppUserType"/></returns>
        public static bool IsType<TAppUserType>(this ApplicationUser userToCheck)
            where TAppUserType : ApplicationUser => userToCheck is TAppUserType;

        /// <summary>
        /// Returns <paramref name="userToCheck"/> <see langword="is"/> <typeparamref name="TAppUserType"/> and outs the <paramref name="appUser"/> result if this method returned <see langword="true"/>
        /// </summary>
        /// <typeparam name="TAppUserType">The type to check against the <paramref name="userToCheck"/>'s type</typeparam>
        /// <param name="userToCheck">The <paramref name="userToCheck"/> is to check if his/her's type matches <typeparamref name="TAppUserType"/></param>
        /// <param name="appUser">The casted version of <paramref name="userToCheck"/> as <typeparamref name="TAppUserType"/> (only when return is <see langword="true"/>)</param>
        /// <returns><paramref name="userToCheck"/> <see langword="is"/> <typeparamref name="TAppUserType"/></returns>
        public static bool IsType<TAppUserType>(this ApplicationUser userToCheck, out TAppUserType appUser)
            where TAppUserType : ApplicationUser
        {
            if (userToCheck is TAppUserType user)
            {
                appUser = user;
                return true;
            }

            appUser = null;
            return false;
        }
    }
}