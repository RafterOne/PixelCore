using System;

namespace PixelMEDIA.PixelCore
{
	/// <summary>
	/// A status message with a typed Data property for additional details.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Status<T> : Status
	{
        /// <summary>
        /// The object related to the status message.
        /// </summary>
		public T Data { get; protected set; }

        /// <summary>
        /// Creates a new typed status message.
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful or not.</param>
        /// <param name="message">The message returned from the operation.</param>
        /// <param name="data">The object related to the status message.</param>
		public Status(bool success, String message, T data)
			: base(success, message)
		{
			this.Data = data;
		}

        /// <summary>
        /// Creates a new typed status message, with a default value for the Data property.
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful or not.</param>
        /// <param name="message">The message returned from the operation.</param>
		public Status(bool success, String message)
			: base(success, message)
		{
			this.Data = default(T);
		}
	}

	/// <summary>
	/// A simple status message.
	/// </summary>
	public class Status
	{
        /// <summary>
        /// Indicates whether the operation was successful or not.
        /// </summary>
		public bool Success { get; protected set; }
        /// <summary>
        /// The message returned from the operation.
        /// </summary>
		public String Message { get; protected set; }

        /// <summary>
        /// Create a new Status.
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful or not.</param>
        /// <param name="message">The message returned from the operation.</param>
		public Status(bool success, String message)
		{
			this.Success = success;
			this.Message = message;
		}

		/// <summary>
		/// Get a version of this status message with a typed Data property.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <returns></returns>
		public Status<T> WithData<T>(T data)
		{
			return new Status<T>(this.Success, this.Message, data);
		}

		/// <summary>
		/// Get a fail status with the given message.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static Status Fail(String message)
		{
			return new Status(false, message);
		}

        /// <summary>
        /// Creates a new failed Status from an exception.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>A faile Status with the message from the exception.</returns>
		public static Status Fail(Exception ex)
		{
			return new Status(false, ex.Message);
		}

		/// <summary>
		/// Get a fail status with the formatted message.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static Status FailFormat(String message, params Object[] args)
		{
			return new Status(false, String.Format(message, args));
		}

		/// <summary>
		/// Get a success status with the given message.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static Status Succeed(String message)
		{
			return new Status(true, message);
		}

		/// <summary>
		/// Get a success status with the formatted message.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static Status SucceedFormat(String message, params Object[] args)
		{
			return new Status(true, String.Format(message, args));
		}

		/// <summary>
		/// Get a general success status.
		/// </summary>
		/// <returns></returns>
		public static Status Succeed()
		{
			return new Status(true, String.Empty);
		}

		/// <summary>
		/// Get a success status with the message and typed Data property.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="message"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static Status<T> Succeed<T>(String message, T data)
		{
			return new Status<T>(true, message, data);
		}

		/// <summary>
		/// Get a fail status with the message and typed Data property.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="message"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static Status<T> Fail<T>(String message, T data)
		{
			return new Status<T>(false, message, data);
		}

		/// <summary>
		/// Get a fail status with a message and a default Data property.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="message"></param>
		/// <returns></returns>
		public static Status<T> Fail<T>(String message)
		{
			return new Status<T>(false, message);
		}

		/// <summary>
		/// Allows the Status object to be automatically cast into boolean.
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public static implicit operator bool(Status status)
		{
			return status.Success;
		}
	}
}
