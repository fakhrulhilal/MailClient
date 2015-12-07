using System;
using System.Collections.Generic;
using System.Linq;

namespace Mail.Library
{
	/// <summary>
	/// Validation result
	/// </summary>
	public class Validation : IValidateResult
	{
		/// <summary>
		/// Initialize validation result
		/// </summary>
		public Validation()
		{
			Messages = new List<string>();
		}

		/// <summary>
		/// Initialize validation result
		/// </summary>
		/// <param name="messages">All error validation messages</param>
		public Validation(IEnumerable<string> messages)
		{
			if (messages == null) throw new ArgumentNullException(nameof(messages));
			Messages = messages;
		}

		/// <summary>
		/// Validation status: <code>true</code> when no error and vice versa
		/// </summary>
		public bool IsValid => Messages == null || !Messages.Any();

		/// <summary>
		/// All error validation messages
		/// </summary>
		public IEnumerable<string> Messages { get; private set; }
	}
}
