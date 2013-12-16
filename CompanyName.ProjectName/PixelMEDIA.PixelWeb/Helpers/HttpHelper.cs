using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using PixelMEDIA.PixelCore.Helpers;
using System.IO;
using PixelMEDIA.PixelCore;
using System.Web.Mvc;

namespace PixelMEDIA.PixelWeb.Helpers
{
	/// <summary>
	/// Helper library for web functions.
	/// </summary>
	public static class HttpHelper
	{
		private const string APP_SETTINGS_LOGIN_URL = "LoginUrl";

        /// <summary>
        /// Combine url segments (similar to Path.Combine)
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
		public static string UrlCombine(params string[] parts)
		{
			Uri uri = null;

			foreach (var part in parts)
			{
				uri = (uri != null) ? new Uri(uri, part) : new Uri(part);
			}
			return uri.ToString();
		}

		/// <summary>
		/// Redirect to a given URL.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="url"></param>
		public static void Redirect(HttpContext context, string url)
		{
			if (context.Response.IsRequestBeingRedirected)
				return;

			if (context.Response.IsClientConnected)
			{
				context.Response.Redirect(url, true);
			}
			else
			{
				context.Response.End();
			}
		}

		/// <summary>
		/// Redirect to a given URL.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="url"></param>
		public static void Redirect(HttpContextBase context, string url)
		{
			Redirect(context.ApplicationInstance.Context, url);
		}

		/// <summary>
		/// Redirect to a given URL.
		/// </summary>
		/// <param name="url"></param>
		public static void Redirect(string url)
		{
			Redirect(HttpContext.Current, url);
		}

		/// <summary>
		/// Redirect to the login page (using APP_SETTINGS_LOGIN_URL).
		/// </summary>
		/// <param name="context"></param>
		public static void RedirectToLogin(HttpContext context)
		{
			Redirect(context, SettingsHelper.Get(APP_SETTINGS_LOGIN_URL, "/"));
		}

		/// <summary>
		/// Redirect to the login page (using APP_SETTINGS_LOGIN_URL).
		/// </summary>
		/// <param name="context"></param>
		public static void RedirectToLogin(HttpContextBase context)
		{
			RedirectToLogin(context.ApplicationInstance.Context);
		}

		/// <summary>
		/// Redirect to the login page (using APP_SETTINGS_LOGIN_URL).
		/// </summary>
		public static void RedirectToLogin()
		{
			RedirectToLogin(HttpContext.Current);
		}

		/// <summary>
		/// Sets a cookie under the specified path.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="path"></param>
		public static void SetCookie(string key, string value, string path)
		{
			var ck = HttpContext.Current.Request.Cookies[key] == null
						? new HttpCookie(key, value)
						: HttpContext.Current.Response.Cookies[key];
			ck.Value = value;
			ck.Path = path;
			ck.Expires = DateTime.MaxValue;
			HttpContext.Current.Response.Cookies.Add(ck);
			return;
		}

		/// <summary>
		/// Sets a cookie at the root level.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void SetCookie(string key, string value)
		{
			SetCookie(key, value, "/");
		}

		/// <summary>
		/// Gets a cookie from the request.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetCookie(string key)
		{
			var ck = HttpContext.Current.Request.Cookies[key];
			return ck != null ? ck.Value : null;
		}

		/// <summary>
		/// Get the current user's IP address.
		/// </summary>
		public static string ClientIp
		{
			get { return HttpContext.Current.Request.UserHostAddress; }
		}

		/// <summary>
		/// Get the protocol, domain, and port for the current request.
		/// </summary>
		public static string ServerDomain
		{
			get
			{
				var url = HttpContext.Current.Request.Url;
				return url.GetLeftPart(UriPartial.Authority);
			}
		}

		/// <summary>
		/// Get an array of bytes from an uploaded file.
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static byte[] GetBytesFromUploadedFile(HttpPostedFileBase file)
		{
			MemoryStream target = new MemoryStream();
			file.InputStream.CopyTo(target);
			return target.ToArray();
		}


		/// <summary>
		/// Saves an uploaded file to the specified location
		/// </summary>
		/// <param name="fileNameBase"></param>
		/// <param name="saveDirectory"></param>
		/// <param name="file"></param>
		/// <param name="allowedExtensions"></param>
		/// <returns></returns>
		public static Status UploadFile(HttpPostedFileBase file, string saveDirectory, object fileNameBase, params string[] allowedExtensions)
		{
			if (file.ContentLength > 0)
			{
				var ext = Path.GetExtension(file.FileName).ToLower();

				if (allowedExtensions.Contains(ext))
				{
					var path = Path.Combine(saveDirectory, String.Format("{0}{1}", fileNameBase, ext));
					file.SaveAs(path);
					return Status.Succeed();
				}
				else
				{
					return Status.Fail("Invalid file extension.");
				}
			}

			return Status.Fail("Uploaded file was empty.");
		}

		/// <summary>
		/// Captures the text from an ActionResult.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="controllerContext"></param>
		/// <returns></returns>
		public static string Capture(this ActionResult result, ControllerContext controllerContext)
		{
			// Credit:Dmytrii Nagirniak
			// http://approache.com/blog/render-any-aspnet-mvc-actionresult-to/
			using (var it = new ResponseCapture(controllerContext.RequestContext.HttpContext.Response))
			{
				result.ExecuteResult(controllerContext);
				return it.ToString();
			}
		}
	}

	/// <summary>
	/// Used to capture the text from an ActionResult
	/// </summary>
	internal class ResponseCapture : IDisposable
	{
		// Credit:Dmytrii Nagirniak
		// http://approache.com/blog/render-any-aspnet-mvc-actionresult-to/
		private readonly HttpResponseBase _response;
		private readonly TextWriter _originalWriter;
		private StringWriter _localWriter;

		public ResponseCapture(HttpResponseBase response)
		{
			_response = response;
			_originalWriter = response.Output;
			_localWriter = new StringWriter();
			_response.Output = _localWriter;
		}

		public override string ToString()
		{
			_localWriter.Flush();
			return _localWriter.ToString();
		}

		public void Dispose()
		{
			if (_localWriter != null)
			{
				_localWriter.Dispose();
				_localWriter = null;
				_response.Output = _originalWriter;
			}
		}
	}
}
