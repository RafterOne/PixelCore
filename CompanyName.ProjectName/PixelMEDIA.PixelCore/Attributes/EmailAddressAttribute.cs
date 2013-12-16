using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using PixelMEDIA.PixelCore.Helpers;

namespace PixelMEDIA.PixelCore.Attributes
{
	/// <summary>
	/// Validates an email address field on a model.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class EmailAddressAttribute : DataTypeAttribute
	{
        /// <summary>
        /// Create a new EmailAddressAttribute.
        /// </summary>
		public EmailAddressAttribute()
			: base(DataType.EmailAddress)
		{
			this.ErrorMessage = "Please enter a valid email address.";
		}

		/// <summary>
		/// Returns true if this property is a valid e-mail address.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool IsValid(object value)
		{
			return (MailHelper.IsValidEmailAddress(Convert.ToString(value)));
		}
	}
}
