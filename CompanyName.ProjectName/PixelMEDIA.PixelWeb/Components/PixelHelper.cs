using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.Web;
using System.Web.Helpers;
using System.Globalization;
using System.ComponentModel;
using PixelMEDIA.PixelCore.Helpers;
using System.Web.Mvc.Ajax;


namespace PixelMEDIA.PixelWeb.Components
{
    /// <summary>
    /// Put any custom helper functions in this guy, so we don't make the general Html helper too noisy.
    /// </summary>
    public class PixelHelper
    {
        /// <summary>
        /// The current view page.
        /// </summary>
        protected WebViewPage ViewPage { get; set; }

        /// <summary>
        /// The current Html Helper.
        /// </summary>
        protected HtmlHelper Html { get { return this.ViewPage.Html; } }

        /// <summary>
        /// The current Ajax Helper.
        /// </summary>
        protected AjaxHelper Ajax { get { return this.ViewPage.Ajax; } }

        /// <summary>
        /// The current ViewDataDictionary
        /// </summary>
        public ViewDataDictionary ViewData { get; private set; }

        /// <summary>
        /// The current ViewContext
        /// </summary>
        public ViewContext ViewContext { get; private set; }

        /// <summary>
        /// The current ViewBag
        /// </summary>
        public dynamic ViewBag { get { return this.ViewPage.ViewBag; } }

        /// <summary>
        /// The current Controller as a PixelController
        /// </summary>
        private PixelController PixelController { get { return (this.ViewContext.Controller as PixelController); } }

        /// <summary>
        /// Returns all messages for the current view.
        /// </summary>
        public IEnumerable<String> Messages { get { return this.PixelController.CurrentMessages.Concat(this.PixelController.RedirectMessages); } }

        /// <summary>
        /// Returns all errors for the current view.
        /// </summary>
        public IEnumerable<String> Errors { get { return this.PixelController.CurrentErrors.Concat(this.PixelController.RedirectErrors); } }

        /// <summary>
        /// Returns true if there are messages to display.
        /// </summary>
        public bool HasMessages { get { return this.PixelController.CurrentMessages.Count + this.PixelController.RedirectMessages.Count > 0; } }

        /// <summary>
        /// Returns true if there are errors to display.
        /// </summary>
        public bool HasErrors { get { return this.PixelController.CurrentErrors.Count + this.PixelController.RedirectErrors.Count > 0; } }

        /// <summary>
        /// Returns true if the application is in debugging mode.
        /// </summary>
        public bool IsDebugMode { get { return this.PixelController.IsDebugMode; } }

        /// <summary>
        /// The current RouteValueDictionary.
        /// </summary>
        public RouteValueDictionary RouteValues { get { return this.PixelController.RouteData.Values; } }

        /// <summary>
        /// Gets a route value from the route values collection.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetRouteValue(string key)
        {
            return this.RouteValues[key];
        }

        /// <summary>
        /// Creates a new PixelHelper.
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="viewPage"></param>
        public PixelHelper(ViewContext viewContext, WebViewPage viewPage)
            : this(viewContext, viewPage, RouteTable.Routes)
        {
        }

        /// <summary>
        /// Creates a new PixelHelper.
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="viewPage"></param>
        /// <param name="routeCollection"></param>
        public PixelHelper(ViewContext viewContext, WebViewPage viewPage, RouteCollection routeCollection)
        {
            ViewContext = viewContext;
            ViewData = new ViewDataDictionary(viewPage.ViewData);
            this.ViewPage = viewPage;
        }

        /// <summary>
        /// Generates a basic post back form for the current controller and action, with the class name applied to it.
        /// </summary>
        /// <param name="cssClassName"></param>
        /// <returns></returns>
        public MvcForm BeginFormWithClassName(string cssClassName)
        {
            string controllerName = (string)this.RouteValues["controller"];
            string actionName = (string)this.RouteValues["action"];
            return this.Html.BeginForm(actionName, controllerName, FormMethod.Post, new { @class = cssClassName });
        }

        /// <summary>
        /// Create a new AJAX form.
        /// </summary>
        /// <param name="ajaxOptions"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcForm BeginAjaxForm(AjaxOptions ajaxOptions, object htmlAttributes)
        {
            string actionName = (string)this.RouteValues["action"];

            var attrs = new RouteValueDictionary(htmlAttributes);

            return this.Ajax.BeginForm(actionName, this.RouteValues, ajaxOptions, attrs);
        }

        /// <summary>
        /// Generates a basic post back form for the current controller and action, with the class name applied to it.
        /// </summary>
        /// <param name="cssClassName"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="method"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public MvcForm BeginFormWithClassName(string cssClassName, string actionName, string controllerName, FormMethod method, object routeValues)
        {
            return this.Html.BeginForm(actionName, controllerName, routeValues, method, new { @class = cssClassName });
        }

        /// <summary>
        /// Create a form that will post to a page with a hash.
        /// </summary>
        /// <param name="cssClassName"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public MvcForm BeginFormWithClassNameAndHash(string cssClassName, string hash)
        {
            string controllerName = (string)this.RouteValues["controller"];
            string actionName = (string)this.RouteValues["action"];
            string formAction = UrlHelper.GenerateUrl(null, actionName, controllerName, this.RouteValues, this.Html.RouteCollection, this.Html.ViewContext.RequestContext, true);

            return this.Html.BeginForm(actionName, controllerName, FormMethod.Post, new { @class = cssClassName, action = formAction + "#" + hash });
        }

        /// <summary>
        /// Gets the name of the current action.
        /// </summary>
        public string ActionName
        {
            get { return (string)this.RouteValues["action"]; }
        }

        /// <summary>
        /// Gets the name of the current controller.
        /// </summary>
        public string ControllerName
        {
            get { return (string)this.RouteValues["controller"]; }
        }

        /// <summary>
        /// Gets the name of the current area, or returns "" if there is no area.
        /// </summary>
        public string AreaName
        {
            get
            {
                var area = this.ViewContext.RouteData.DataTokens["area"];
                return area == null ? "" : Convert.ToString(area);
            }
        }

        /// <summary>
        /// Render a validation summary with a specific class applied.
        /// </summary>
        /// <param name="cssClassName"></param>
        /// <returns></returns>
        public MvcHtmlString GetValidationSummarySection(string cssClassName)
        {
            var valid = this.Html.ValidationSummary(false);
            return new MvcHtmlString(string.Format(@"<div class=""{0}"">{1}</div>", cssClassName, valid));
        }

        /// <summary>
        /// Render a validation summary with a specific class applied.
        /// </summary>
        /// <param name="cssClassName"></param>
        /// <param name="propErrors"></param>
        /// <returns></returns>
        public MvcHtmlString GetValidationSummarySection(string cssClassName, bool propErrors)
        {
            var valid = this.Html.ValidationSummary(propErrors);
            return new MvcHtmlString(string.Format(@"<div class=""{0}"">{1}</div>", cssClassName, valid));
        }

        /// <summary>
        /// Renders a label with the provided HTML attributes. 
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="labelText"></param>
        /// <param name="hookValidation"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString LabelFor(string controlName, string labelText, bool hookValidation, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("label");
            builder.MergeAttributes(htmlAttributes);
            builder.SetInnerText(labelText);

            return new MvcHtmlString(builder.ToString());
        }


        /// <summary>
        /// Returns a link. Will set the selectedClass if the specified action, controller, and area match the current page.
        /// </summary>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="selectedClass"></param>
        /// <returns></returns>
        public MvcHtmlString NavigationLink(string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, string selectedClass = "selected")
        {
            var routeValueDictionary = new RouteValueDictionary(routeValues);
            var htmlAttributeDictionary = new RouteValueDictionary(htmlAttributes);
            var classStub = htmlAttributeDictionary.ContainsKey("class") ? htmlAttributeDictionary["class"] + " " : "";
            var areaName = routeValueDictionary.ContainsKey("Area") ? Convert.ToString(routeValueDictionary["Area"]) : "";

            if (IsCurrentPage(actionName, controllerName, areaName))
            {
                htmlAttributeDictionary["class"] = classStub + selectedClass;
            }

            return this.Html.ActionLink(linkText, actionName, controllerName, routeValueDictionary, htmlAttributeDictionary);
        }

        /// <summary>
        /// Returns a sort link (for column headers).
        /// </summary>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="thisSortColumn"></param>
        /// <param name="currentSortColumn"></param>
        /// <param name="currentSortDirection"></param>
        /// <param name="ascendingClass"></param>
        /// <param name="descendingClass"></param>
        /// <returns></returns>
        public MvcHtmlString SortLink(string linkText, string actionName, object routeValues, object htmlAttributes, object thisSortColumn, object currentSortColumn, ListSortDirection currentSortDirection, string ascendingClass = "sort-asc", string descendingClass = "sort-desc")
        {
            var routeValueDictionary = new RouteValueDictionary(routeValues);
            var htmlAttributeDictionary = new RouteValueDictionary(htmlAttributes);

            routeValueDictionary["sort"] = thisSortColumn;

            if (Object.Equals(thisSortColumn, currentSortColumn))
            {
                switch (currentSortDirection)
                {
                    case ListSortDirection.Ascending:
                        routeValueDictionary["sortDir"] = ListSortDirection.Descending;
                        htmlAttributeDictionary["class"] = ascendingClass;
                        break;

                    case ListSortDirection.Descending:
                        routeValueDictionary["sortDir"] = ListSortDirection.Ascending;
                        htmlAttributeDictionary["class"] = descendingClass;
                        break;
                }
            }
            else
            {
                routeValueDictionary["sortDir"] = ListSortDirection.Ascending;
            }

            return Html.ActionLink(linkText, actionName, routeValueDictionary, htmlAttributeDictionary);
        }

        /// <summary>
        /// Returns true if the specified action, controller, and area match the current page.
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public bool IsCurrentPage(string actionName, string controllerName, string areaName = "")
        {
            if (String.Equals(this.ActionName, actionName, StringComparison.CurrentCultureIgnoreCase)
                && String.Equals(this.ControllerName, controllerName, StringComparison.CurrentCultureIgnoreCase))
            {
                if (areaName != null ? String.Equals(this.AreaName, areaName, StringComparison.CurrentCultureIgnoreCase) : true)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Truncate a string greater than the given length and add ellipsis (...). Otherwise, return the string.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string Ellipsize(string s, int length)
        {
            return StringHelper.Ellipsize(s, length);
        }

        /// <summary>
        /// Renders an object as html attribute key/value pairs.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public MvcHtmlString Attrs(object attributes)
        {
            var attrDict = new RouteValueDictionary(attributes);
            return new MvcHtmlString(GetAttributesString(attrDict));
        }

        /// <summary>
        /// Renders the specified attributes if condition is true.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public MvcHtmlString AttrsIf(bool condition, object attributes)
        {
            return condition ? Attrs(attributes) : new MvcHtmlString(" ");
        }

        /// <summary>
        /// Renders the first set of attributes if condition is true, otherwise renders the second set of attributes.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="attributesIfTrue"></param>
        /// <param name="attributesIfFalse"></param>
        /// <returns></returns>
        public MvcHtmlString AttrsIf(bool condition, object attributesIfTrue, object attributesIfFalse)
        {
            return condition ? Attrs(attributesIfTrue) : Attrs(attributesIfFalse);
        }

        /// <summary>
        /// Returns a disabled attribute if the condition is true.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public MvcHtmlString DisabledAttrIf(bool condition)
        {
            return AttrsIf(condition, new { disabled = "disabled" });
        }

        /// <summary>
        /// Returns a checked attribute if the condition is true.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public MvcHtmlString CheckedAttrIf(bool condition)
        {
            return AttrsIf(condition, new { @checked = "checked" });
        }

        /// <summary>
        /// Returns a selected attribute if the condition is true.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public MvcHtmlString SelectedAttrIf(bool condition)
        {
            return AttrsIf(condition, new { selected = "selected" });
        }

        /// <summary>
        /// Returns a class attribute if the condition is true.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public MvcHtmlString ClassIf(bool condition, string className)
        {
            return AttrsIf(condition, new { @class = className });
        }

        private const string _attributeFormat = @" {0}=""{1}""";

        private string GetAttributesString(RouteValueDictionary attributes)
        {
            //Repurposed from MVC's TagBuilder
            StringBuilder sb = new StringBuilder();
            foreach (var attribute in attributes)
            {
                string key = attribute.Key;
                string value = HttpUtility.HtmlAttributeEncode(attribute.Value.ToString());
                sb.AppendFormat(CultureInfo.InvariantCulture, _attributeFormat, key, value);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns the first string if the condition is true, otherwise returns the other.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="text"></param>
        /// <param name="elseText"></param>
        /// <returns></returns>
        public MvcHtmlString StringIf(bool condition, string text, string elseText)
        {
            return new MvcHtmlString(condition ? text : elseText);
        }

        /// <summary>
        /// Returns the string if the condition is true, otherwise returns an empty string.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public MvcHtmlString StringIf(bool condition, string text)
        {
            return StringIf(condition, text, String.Empty);
        }

        /// <summary>
        /// Get the absolute url of the provided relative url string.
        /// </summary>
        /// <param name="urlString"></param>
        /// <returns></returns>
        public MvcHtmlString GetAbsoluteUrl(string urlString)
        {
            var url = this.ViewPage.Url.Content(urlString);

            return new MvcHtmlString(new System.Uri(this.ViewPage.Request.Url, url).AbsoluteUri);
        }


        private static MvcHtmlString MvcHtmlStringConcat(params MvcHtmlString[] items)
        {
            //By Miguel Vitorino: http://stackoverflow.com/a/4360017
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                sb.Append(item.ToHtmlString());
            }
            return MvcHtmlString.Create(sb.ToString());
        }

    }

    /// <summary>
    /// Extensions to the HTMLHelper class.
    /// </summary>
    public static class PixelHtmlExtensions
    {
        /// <summary>
        /// Create a label with the given text and HTML attributes.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="labelText"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes)
        {
            return LabelFor(html, expression, labelText, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Create a label with the given text and HTML attributes.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="labelText"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, IDictionary<string, object> htmlAttributes)
        {
            //Adapted from Imran Baloch: http://weblogs.asp.net/imranbaloch/archive/2010/07/03/asp-net-mvc-labelfor-helper-with-htmlattributes.aspx
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            TagBuilder tag = new TagBuilder("label");
            tag.MergeAttributes(htmlAttributes);
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(labelText);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }


    }


}