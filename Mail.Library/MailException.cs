﻿using System;

namespace Mail.Library
{
	/// <summary>
	/// General exception for mail operation
	/// </summary>
	public class MailException : Exception
	{
		/// <summary>
		/// Initialize exception with message only
		/// </summary>
		/// <param name="message">Exception message</param>
		public MailException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initialize exception with message and inner exception
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="innerException">Inner exception</param>
		public MailException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}