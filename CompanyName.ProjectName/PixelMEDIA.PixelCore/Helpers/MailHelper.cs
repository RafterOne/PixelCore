using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace PixelMEDIA.PixelCore.Helpers
{
	/// <summary>
	/// Helper library for email functions.
	/// </summary>
	public static class MailHelper
	{
		private const string APP_SETTINGS_SMTP_SERVER = "SmtpServer";
        private const string APP_SETTINGS_FROM_ADDRESS_DISPLAY_NAME = "FromEmailDisplayName";
        private const string APP_SETTINGS_FROM_ADDRESS = "FromEmailAddress";
        private const string APP_SETTINGS_EMAIL_CC = "EmailCC";

		/// <summary>
		/// Send an HTML email to the indicated recipients.
		/// </summary>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="recipients"></param>
		/// <returns></returns>
		public static Status SendHtmlEmail(String subject, String body, params string[] recipients)
		{
			return SendEmail(subject, body, true, recipients);
		}

		/// <summary>
		/// Send a text email to the indicated recipients.
		/// </summary>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="recipients"></param>
		/// <returns></returns>
		public static Status SendTextEmail(String subject, String body, params string[] recipients)
		{
			return SendEmail(subject, body, false, recipients);
		}

		private static MailAddress FromAddress
		{
			get
			{
				var fromAddress = SettingsHelper.Get(APP_SETTINGS_FROM_ADDRESS);
				var fromDisplay = SettingsHelper.Get(APP_SETTINGS_FROM_ADDRESS_DISPLAY_NAME);

				return new MailAddress(fromAddress, fromDisplay);
			}
		}

        private static MailAddress EmailCC
        {
            get
            {
                return new MailAddress(SettingsHelper.Get(APP_SETTINGS_EMAIL_CC));
            }
        }

		private static string SmtpServerAddress
		{
			get
			{
				return SettingsHelper.Get(APP_SETTINGS_SMTP_SERVER);
			}
		}

		/// <summary>
		/// Attempts to create a MailAddress from the given string.
		/// </summary>
		/// <param name="email"></param>
		/// <param name="mailAddress"></param>
		/// <returns></returns>
		public static bool TryLoadEmailAddress(string email, out MailAddress mailAddress)
		{
			try
			{
				mailAddress = new MailAddress(email);
				return true;
			}
			catch (Exception)
			{
				mailAddress = null;
				return false;
			}
		}

		/// <summary>
		/// Checks whether an email address is valid.
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public static bool IsValidEmailAddress(string email)
		{
			MailAddress mailAddress;
			return TryLoadEmailAddress(email, out mailAddress);
		}

		/// <summary>
		/// Sends an email to the indicated recipients.
		/// </summary>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="isHtml"></param>
		/// <param name="recipients"></param>
		/// <returns></returns>
		private static Status SendEmail(String subject, String body, bool isHtml, params string[] recipients)
		{
			int successCount = 0;

			using (SmtpClient smtp = new SmtpClient(SmtpServerAddress))
			{
				foreach (var recipient in recipients)
				{
					MailAddress recipientAddress;

					if (!TryLoadEmailAddress(recipient, out recipientAddress))
					{
						Logger.Warn("Skipping invalid email recipient: {0}", recipient);
						continue;
					}

					MailMessage mail = new MailMessage(FromAddress, recipientAddress);

					mail.Subject = subject;
					mail.IsBodyHtml = isHtml;
					mail.Body = body;

                    mail.CC.Add(EmailCC);

					try
					{
						smtp.Send(mail);
					}
					catch (Exception ex)
					{
						Logger.Error(ex);
					}

				}
			}
			Logger.Info(@"Sent {0}/{1} emails for ""{2}""", successCount, recipients.Length, subject);

			return Status.SucceedFormat("Sent {0}/{1} emails", successCount, recipients.Length);
		}

	}
}
