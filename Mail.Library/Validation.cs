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
		/// All error validation messages
		/// </summary>
		public IEnumerable<string> Messages { get; }

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
			Messages = messages ?? throw new ArgumentNullException(nameof(messages));
		}

		/// <inheritdoc />
		public bool IsValid => Messages == null || !Messages.Any();
	}
}