using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Mvc;

namespace School_Scheduler.MVC.Helpers
{
    /// <summary>
    /// Html helper extensions
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Returns a usable HTML input[type="datetime-local"] value or JS Date string value
        /// </summary>
        /// <param name="dateTime">The DateTime to convert</param>
        /// <returns></returns>
        public static string ToHtmlInputDateTimeValueString(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd HH:mm:ss").Replace(' ', 'T');

        /// <summary>
        /// Returns a usable HTML input[type="date"] value or JS Date string value
        /// </summary>
        /// <param name="date">The DateTime to convert (Time will be ignored)</param>
        /// <returns></returns>
        public static string ToHtmlInputDateValueString(this DateTime date) => date.ToString("yyyy-MM-dd");

        /// <summary>
        /// Delegate to select a member of a object (<typeparamref name="TModel"/>) and return the member's value (<typeparamref name="TMember"/>)
        /// </summary>
        /// <typeparam name="TModel">The object type that has a member</typeparam>
        /// <typeparam name="TMember">The member's type</typeparam>
        /// <param name="model">The object to get the member from</param>
        /// <returns>(<typeparamref name="TMember"/>) member value</returns>
        public delegate TMember SelectMemeber<TModel, TMember>(TModel model) where TModel : class;

        /// <summary>
        /// Returns a input tag (input[type="datetime-local"] tag) and you can provide a style dictionary and any classNames
        /// </summary>
        /// <typeparam name="TModel">Model of the Current View</typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">lambda expression to choose DateTime property off of the Model</param>
        /// <param name="initialDateTime">initialDateTime value for the input tag</param>
        /// <param name="style">inline style for the generated input tag</param>
        /// <param name="classNames"></param>
        /// <returns>input[type="datetime-local"] tag</returns>
        public static MvcHtmlString DateTimeFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<SelectMemeber<TModel, DateTime>> expression,
            [Optional] DateTime? initialDateTime,
            [Optional] Dictionary<string, string[]> style,
            params string[] classNames)
            where TModel : class
        {
            // Get Model DateTime property name (dev has to use a lambda express to select the DateTime property)
            string modelPropertyDateTimeName = (expression.Body as MemberExpression)?.Member.Name ?? throw new ArgumentException(
                $"Failed to get DateTime property Name with existing {nameof(expression)}. ({expression.ToString()})",
                nameof(expression));

            // build style string and skip styles that are empty/null
            string styleString = style.BuildStyleString();

            // make C# DateTime usable for the input tag (html5 input tag doesn't know how to interpret a C# DateTime string)
            string initialDateTimeString = initialDateTime.HasValue ? initialDateTime.Value.ToHtmlInputDateTimeValueString() : null;
            string classString = string.Join(" ", classNames ?? new[] { "form-control" });

            string resultString = $"<input class='{classString}' style='{styleString}' type='datetime-local' name='{modelPropertyDateTimeName}' value='{initialDateTimeString}' />";

            return MvcHtmlString.Create(resultString);
        }
        #region DateTimeFor<TModel> Overloads
        public static MvcHtmlString DateTimeFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<SelectMemeber<TModel, DateTime?>> expression,
            [Optional] DateTime? initialDateTime,
            [Optional] Dictionary<string, string[]> style,
            params string[] classNames)
            where TModel : class
        {
            // Get Model DateTime property name (dev has to use a lambda express to select the DateTime property)
            string modelPropertyDateTimeName = (expression.Body as MemberExpression)?.Member.Name ?? throw new ArgumentException(
                $"Failed to get DateTime property Name with existing {nameof(expression)}. ({expression.ToString()})",
                nameof(expression));

            // build style string and skip styles that are empty/null
            string styleString = style.BuildStyleString();

            // make C# DateTime usable for the input tag (html5 input tag doesn't know how to interpret a C# DateTime string)
            string initialDateTimeString = initialDateTime.HasValue ? initialDateTime.Value.ToHtmlInputDateTimeValueString() : null;
            string classString = string.Join(" ", classNames ?? new[] { "form-control" });

            string resultString = $"<input class='{classString}' style='{styleString}' type='datetime-local' name='{modelPropertyDateTimeName}' value='{initialDateTimeString}' />";

            return MvcHtmlString.Create(resultString);
        }
        #endregion

        /// <summary>
        /// Returns a input tag (input[type="date"] tag) and you can provide a style dictionary and any classNames
        /// </summary>
        /// <typeparam name="TModel">Model of the Current View</typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">lambda expression to choose DateTime property off of the Model</param>
        /// <param name="initialDate">initialDate value for the input tag</param>
        /// <param name="style">inline style for the generated input tag</param>
        /// <param name="classNames"></param>
        /// <returns>input[type="date"] tag</returns>
        public static MvcHtmlString DateFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<SelectMemeber<TModel, DateTime>> expression,
            [Optional] DateTime? initialDate,
            [Optional] Dictionary<string, string[]> style,
            params string[] classNames)
            where TModel : class
        {
            // Get Model DateTime property name (dev has to use a lambda express to select the DateTime property)
            string modelPropertyDateTimeName = (expression.Body as MemberExpression)?.Member.Name ?? throw new ArgumentException(
                $"Failed to get DateTime property Name with existing {nameof(expression)}. ({expression.ToString()})",
                nameof(expression));

            // build style string and skip styles that are empty/null
            string styleString = style.BuildStyleString();

            // make C# DateTime usable for the input tag (html5 input tag doesn't know how to interpret a C# DateTime string)
            string initialDateString = initialDate.HasValue ? initialDate.Value.Date.ToHtmlInputDateValueString() : null;
            string classString = string.Join(" ", classNames ?? new[] { "form-control" });

            string resultString = $"<input class='{classString}' style='{styleString}' type='date' name='{modelPropertyDateTimeName}' value='{initialDateString}' />";

            return MvcHtmlString.Create(resultString);
        }
        #region DateFor<TModel> Overloads
        public static MvcHtmlString DateFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<SelectMemeber<TModel, DateTime?>> expression,
            [Optional] DateTime? initialDateTime,
            [Optional] Dictionary<string, string[]> style,
            params string[] classNames)
            where TModel : class
        {
            // Get Model DateTime property name (dev has to use a lambda express to select the DateTime property)
            string modelPropertyDateTimeName = (expression.Body as MemberExpression)?.Member.Name ?? throw new ArgumentException(
                $"Failed to get DateTime property Name with existing {nameof(expression)}. ({expression.ToString()})",
                nameof(expression));

            // build style string and skip styles that are empty/null
            string styleString = style.BuildStyleString();

            // make C# DateTime usable for the input tag (html5 input tag doesn't know how to interpret a C# DateTime string)
            string initialDateTimeString = initialDateTime.HasValue ? initialDateTime.Value.Date.ToHtmlInputDateTimeValueString() : null;
            string classString = string.Join(" ", classNames ?? new[] { "form-control" });

            string resultString = $"<input class='{classString}' style='{styleString}' type='date' name='{modelPropertyDateTimeName}' value='{initialDateTimeString}' />";

            return MvcHtmlString.Create(resultString);
        } 
        #endregion

        /// <summary>
        /// Converts a dictionary that represents CSS styles into a usable string
        /// </summary>
        /// <param name="style">Dictionary that represents CSS styles ("{CSS style property}":string, "{CSS style property values}": string[])</param>
        /// <returns>Dictionary that represents CSS styles to a string</returns>
        private static string BuildStyleString(this Dictionary<string, string[]> style)
        {
            if (style == null)
            {
                return null;
            }

            // Linq version
            //return string.Join("", style.Keys
            //    .Where(key => !(string.IsNullOrWhiteSpace(key) || style[key].Any(string.IsNullOrWhiteSpace)))
            //    .Select(key => $"{key}: {string.Join(" ", style[key])}; "))
            //    .Trim();

            // foreach loop version
            StringBuilder sb = new StringBuilder(style.Keys.Count);
            foreach (string key in style.Keys)
            {
                if (string.IsNullOrWhiteSpace(key) || style[key].Any(string.IsNullOrWhiteSpace))
                {
                    continue;
                }
                string currentStyle = $"{key}: {string.Join(" ", style[key])}; ";
                sb.Append(currentStyle);
            }
            return sb.ToString().Trim();

        }

        /// <summary>
        /// Returns decimal string containing '$', '-', "2 decimal places"
        /// </summary>
        /// <param name="numberToFormat">The number to be formatted in a currency format</param>
        /// <param name="cultureInfoOverride">provide culture to override default [Optional]</param>
        /// <returns>decimal string containing '$', '-', "2 decimal places"</returns>
        public static string ToCurrencyFormattedString(this decimal numberToFormat, [Optional] CultureInfo cultureInfoOverride)
        {
            CultureInfo culture = cultureInfoOverride ?? new CultureInfo("en-US")
            {
                NumberFormat = new NumberFormatInfo
                {
                    CurrencySymbol = "$",
                    CurrencyDecimalSeparator = ".",
                    CurrencyDecimalDigits = 2,
                    CurrencyNegativePattern = 1,
                },
            };
            return string.Format(culture, "{0:C}", numberToFormat);
        }
    }
}