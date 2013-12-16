using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections;
using PixelMEDIA.PixelCore;
using PixelMEDIA.PixelCore.Helpers;
using PixelMEDIA.PixelWeb.ViewModels;
using PixelMEDIA.PixelWeb.Helpers;
using System.Net;
using System.IO;

namespace PixelMEDIA.PixelWeb.Components
{
	/// <summary>
	/// A base class for all controllers, with added convenience functions.
	/// </summary>
	public abstract class PixelController : System.Web.Mvc.Controller
	{
		private const string CONTEXT_MESSAGES = "MESSAGES";
		private const string CONTEXT_ERRORS = "ERRORS";

		/// <summary>
		/// Returns true if the application is in debugging mode.
		/// </summary>
		public bool IsDebugMode { get { return this.HttpContext.IsDebuggingEnabled; } }

		/// <summary>
		/// Returns a SelectListItem with the value and text set to the provided value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public SelectListItem MakeSelectListItem(string value) { return MakeSelectListItem(value, value); }

		/// <summary>
		/// Returns a SelectListItem with the provided value and text.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public SelectListItem MakeSelectListItem(string value, string text) { return new SelectListItem() { Value = value, Text = text }; }

		/// <summary>
		/// Renders the error page and passes it the ErrorModel.
		/// </summary>
		/// <param name="err"></param>
		/// <returns></returns>
		public ActionResult Error(ErrorModel err)
		{
			return View("Error", err);
		}

		/// <summary>
		/// Global catch-all for uncaught errors.
		/// </summary>
		/// <param name="filterContext"></param>
		protected override void OnException(ExceptionContext filterContext)
		{
			try
			{
				var exception = filterContext.Exception; // Server.GetLastError() doesn't work properly here

				Logger.Fatal(exception);

				if (!this.IsDebugMode)
				{
					var errorModel = new ErrorModel(exception);
					filterContext.ExceptionHandled = true;
					filterContext.Result = this.Error(errorModel);
				}
			}
			catch (Exception ex)
			{
				ex.ToString();
			}

			base.OnException(filterContext);
		}

		/// <summary>
		/// Extracts a list of messages from a dictionary.
		/// </summary>
		/// <param name="dictionary"></param>
		/// <param name="collectionKey"></param>
		/// <returns></returns>
		private List<string> GetMessageCollection(IDictionary<string, object> dictionary, string collectionKey)
		{
			if (dictionary.ContainsKey(collectionKey))
			{
				return dictionary[collectionKey] as List<string>;
			}
			var messageCollection = new List<string>();
			dictionary.Add(collectionKey, messageCollection);
			return messageCollection;
		}

		private List<String> _currentErrors = null;

		/// <summary>
		/// Get a list of all current errors for the view.
		/// </summary>
		public List<String> CurrentErrors
		{
			get
			{
				return _currentErrors = _currentErrors ?? GetMessageCollection(this.ViewData, CONTEXT_ERRORS);
			}
		}

		private List<String> _currentMessages = null;

		/// <summary>
		/// Get a list of all current messages for the view.
		/// </summary>
		public List<String> CurrentMessages
		{
			get
			{
				return _currentMessages = _currentMessages ?? GetMessageCollection(this.ViewData, CONTEXT_MESSAGES);
			}
		}

		private List<String> _redirectErrors = null;

		/// <summary>
		/// Get a list of all errors included when redirecting to this page.
		/// </summary>
		public List<String> RedirectErrors
		{
			get
			{
				return _redirectErrors = _redirectErrors ?? GetMessageCollection(this.TempData, CONTEXT_ERRORS);
			}
		}

		private List<String> _redirectMessages = null;

		/// <summary>
		/// Get a list of all messages included when redirecting to this page.
		/// </summary>
		public List<String> RedirectMessages
		{
			get
			{
				return _redirectMessages = _redirectMessages ?? GetMessageCollection(this.TempData, CONTEXT_MESSAGES);
			}
		}

		/// <summary>
		/// Initialize the controller.
		/// </summary>
		/// <param name="requestContext"></param>
		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
            var ctx = requestContext.HttpContext;
            string configGuid = SettingsHelper.Get("accessGuid");
            string cookieGuidValue = HttpHelper.GetCookie("accessGuid");
            string requestGuidValue = ctx.Request["accessGuid"];

            Guid accessGuid = Guid.Empty;
            Guid cookieGuid = Guid.Empty;
            Guid requestGuid = Guid.Empty;

            // Check For Configuration
            if (!string.IsNullOrEmpty(configGuid))
            {
                Guid.TryParse(configGuid, out accessGuid);
                Guid.TryParse(cookieGuidValue, out cookieGuid);

                // Verify Cookie
                if (accessGuid.Equals(cookieGuid))
                {
                    base.Initialize(requestContext);
                    return;
                }

                // Check For Request Value
                if (!string.IsNullOrEmpty(requestGuidValue))
                {
                    Guid.TryParse(requestGuidValue, out requestGuid);

                    if (accessGuid.Equals(requestGuid))
                    {
                        HttpHelper.SetCookie("accessGuid", requestContext.HttpContext.Request["accessGuid"]);
                        base.Initialize(requestContext);
                        return;
                    }
                }

                ctx.Response.Status = "403 Forbidden";
                ctx.Response.ContentType = "text/html";
                ctx.Response.Write("403 Access Forbidden");
                ctx.Response.Flush();
                ctx.Response.Close();
                return;
            }
            base.Initialize(requestContext);
		}

		/// <summary>
		/// Add an error to the current view.
		/// </summary>
		/// <param name="s"></param>
		public void AddError(string s)
		{
			if (!String.IsNullOrWhiteSpace(s)) { this.CurrentErrors.Add(s); }
		}

		/// <summary>
		/// Add an error to the current view.
		/// </summary>
		/// <param name="ex"></param>
		public void AddError(Exception ex) { AddError(ex.Message); }

		/// <summary>
		/// Add an error to the current view.
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public bool AddError(Status status) { AddError(status.Message); return status.Success; }

		/// <summary>
		/// Add a list of errors to the current view.
		/// </summary>
		/// <param name="errors"></param>
		public void AddErrors(IEnumerable<string> errors) { foreach (var s in errors) { AddError(s); } }

		/// <summary>
		/// Add a message to the current view.
		/// </summary>
		/// <param name="s"></param>
		public void AddMessage(string s)
		{
			if (!String.IsNullOrWhiteSpace(s)) { this.CurrentMessages.Add(s); }
		}

		/// <summary>
		/// Add a message to the current view.
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public bool AddMessage(Status status) { AddMessage(status.Message); return status.Success; }

		/// <summary>
		/// Add a list of messages to the current view.
		/// </summary>
		/// <param name="messages"></param>
		public void AddMessages(IEnumerable<string> messages) { foreach (var s in messages) { AddMessage(s); } }

		/// <summary>
		/// Adds an error message or success message to the appropriate collection and returns true if the status is successful.
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public bool AddRedirectMessageOrError(Status status)
		{
			if (status.Success)
			{
				this.RedirectMessages.Add(status.Message);
			}
			else
			{
				this.RedirectErrors.Add(status.Message);
			}
			return status.Success;
		}

		/// <summary>
		/// Redirect to a URL with a redirect message or error included.
		/// </summary>
		/// <param name="status"></param>
		/// <param name="url"></param>
		/// <returns></returns>
		public ActionResult RedirectWithStatus(Status status, string url)
		{
			AddRedirectMessageOrError(status);
			return Redirect(url);
		}

		/// <summary>
		/// Redirect to an action with a redirect message or error included.
		/// </summary>
		/// <param name="status"></param>
		/// <param name="actionName"></param>
		/// <returns></returns>
		public ActionResult RedirectToActionWithStatus(Status status, string actionName)
		{
			AddRedirectMessageOrError(status);
			return RedirectToAction(actionName);
		}

		/// <summary>
		/// Redirect to a URL constructed from the given format and args.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public ActionResult RedirectFormat(string format, params object[] args)
		{
			return Redirect(String.Format(format, args));
		}

		/// <summary>
		/// Returns a CSV with the given name, built from the provided collection,
		/// and including the specified field names in that order. If no field names are specified, all fields are included.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fileDownloadName"></param>
		/// <param name="collection"></param>
		/// <param name="fieldNames"></param>
		/// <returns></returns>
		public FileContentResult Csv<T>(string fileDownloadName, IEnumerable<T> collection, params string[] fieldNames)
		{
			var csv = ExportHelper.GetCsv(collection, fieldNames);
			return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", fileDownloadName);
		}

        /// <summary>
        /// Return a JSON result built from a Status object.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
		public JsonResult JsonStatus(Status status, HttpStatusCode statusCode)
		{
			if (!status.Success)
			{
				Response.StatusCode = (int)statusCode;
			}
			return Json(status);
		}

		/// <summary>
		/// Returns the status code as json, with a status code of 500 if there is an error.
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public JsonResult JsonStatus(Status status)
		{
			return JsonStatus(status, HttpStatusCode.InternalServerError);
		}

        /// <summary>
        /// Creates a Json representation of the object passed in as a parameter.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="data"></param>
        /// <returns></returns>
		public ActionResult Jsonp(string callback, object data)
		{
            var result = JsonpText(callback, data, false);
			return Content(result);
		}

        /// <summary>
        /// Creates an 'unescaped' text/html representation of the object passed in as a parameter.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public MvcHtmlString JsonpText(string callback, object data)
        {
            return MvcHtmlString.Create(JsonpText(callback, data, true));
        }

        /// <summary>
        /// Creates an 'unescaped' text/html representation of the object passed in as a parameter.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public MvcHtmlString JsonText(object data)
        {
            return MvcHtmlString.Create(JsonText(data, true));
        }

        private string JsonpText(string callback, object data, bool embedded)
        {
            var result = String.Format("{0}({1});", callback, JsonText(data, embedded));
            return result;
        }

        private string JsonText(object data, bool embedded)
        {
            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            var jsonText = jsonResult.Capture(this.ControllerContext);
            if (embedded)
            {
                this.ControllerContext.HttpContext.Response.ContentType = "text/html";
            }
            return jsonText;
        }


		/// <summary>
        /// Renders an MVC view to a string.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="viewName"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		public string RenderViewToString<T>(string viewName, T model)
		{
			this.ViewData.Model = model;
			try
			{
				using (StringWriter sw = new StringWriter())
				{
					ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
					ViewContext viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
					viewResult.View.Render(viewContext, sw);
					return sw.ToString();
				}
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
		}
	}
}
