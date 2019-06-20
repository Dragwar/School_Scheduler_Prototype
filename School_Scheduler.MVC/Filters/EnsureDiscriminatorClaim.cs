using System;
using System.Linq;
using System.Web.Mvc;
using School_Scheduler.MVC.Helpers;

namespace School_Scheduler.MVC.Filters
{
    /// <summary>
    /// This isn't a authorize filter but this will just check the current <see cref="Models.ApplicationUser"/>'s <see cref="Discriminator"/>.
    /// <para />
    /// If the <see cref="Models.ApplicationUser"/> doesn't have the any of the specified <see cref="Discriminator"/>(s)
    /// then the <see cref="Models.ApplicationUser"/> will be redirected to "Error" page
    /// </summary>
    public sealed class EnsureDiscriminatorClaim : ActionFilterAttribute
    {
        /// <summary>
        /// All allowed <see cref="Discriminator"/> types
        /// </summary>
        public Discriminator[] EsuredDiscriminators { get; }

        /// <summary>
        /// To use this action filter, you must use another constructor
        /// </summary>
        [Obsolete("You must provide a " + nameof(Discriminator) + " to ensure", true)]
        public EnsureDiscriminatorClaim() : base()
        {
        }

        /// <summary>
        /// Ensure the <see cref="Discriminator"/>(s) is present on the current <see cref="Models.ApplicationUser"/>
        /// </summary>
        /// <param name="discriminatorsToEnsureUserHasAny">The <see cref="Discriminator"/>(s) to ensure the current <see cref="Models.ApplicationUser"/> is any of them</param>
        public EnsureDiscriminatorClaim(params Discriminator[] discriminatorsToEnsureUserHasAny) : base()
        {
            EsuredDiscriminators = discriminatorsToEnsureUserHasAny ?? throw new ArgumentNullException(nameof(discriminatorsToEnsureUserHasAny));
        }

        /// <summary>
        /// Determines if the <see cref="Models.ApplicationUser"/> has the specified <see cref="EsuredDiscriminators"/>
        /// </summary>
        /// <param name="filterContext">The <see cref="ActionExecutingContext"/> <paramref name="filterContext"/></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Discriminator currentUserDiscriminator = filterContext.HttpContext.User.Identity.GetUserDiscriminator();

            if (!EsuredDiscriminators.Any(discriminator => discriminator == currentUserDiscriminator))
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "Error"
                };
                ((ViewResult)filterContext.Result).TempData[nameof(EnsureDiscriminatorClaim)] = $"You must be the following user type(s): {string.Join(", ", EsuredDiscriminators)}";
                return;
            }


            // Not needed
            base.OnActionExecuting(filterContext);
        }
    }
}