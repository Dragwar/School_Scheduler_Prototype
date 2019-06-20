using System.Web.Mvc;

namespace School_Scheduler.MVC.Helpers
{
    /// <summary>
    /// Miscellaneous Extensions/Methods
    /// </summary>
    public static class MiscExt
    {
        /// <summary>
        /// Gets the name of controller (Removes "Controller" at the end of the FullName)
        /// </summary>
        /// <example>
        /// var test = new TestController();
        /// var fullName = nameof(TestController); // "TestController"
        /// var controllerName =  MiscExt.GetName/<TestController/>(); // "Test"
        /// </example>
        /// <typeparam name="TController">Has to be or inherit from <see cref="System.Web.Mvc.Controller"/></typeparam>
        /// <returns>returns the name of the controller</returns>
        public static string GetName<TController>() where TController : Controller
        {
            string name = typeof(TController).Name;
            if (name.EndsWith(nameof(Controller)))
            {
                return name.Substring(0, name.LastIndexOf(nameof(Controller)));
            }
            return name;
        }
    }
}