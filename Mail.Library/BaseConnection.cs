using System.Collections.Generic;

namespace Mail.Library
{
	public abstract class BaseConnection : IValidatable
	{
		protected List<string> ValidationMessages = new List<string>();

		/// <summary>
		/// Mail server port
		/// </summary>
		public int Port { get; set; }

		/// <summary>
		/// Username for connection
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Password for connection
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Mail server address
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		/// Mail protocol type
		/// </summary>
		public MailProtocol Protocol { get; protected internal set; }

		/// <summary>
		/// Secure connection type
		/// </summary>
		public SecureType Security { get; set; }

		/// <summary>
		/// Validation mail connection property
		/// </summary>
		/// <returns><code>true</code> when no error configuration and vice versa</returns>
		public virtual Validation Validate()
		{
			ValidationMessages.Clear();
			ValidatePort();
			ValidateCredential();
			ValidateServer();
			return new Validation(ValidationMessages);
		}

		public virtual Validation Validate(string prefix) => Validate();

		private void ValidateServer()
		{
			if (string.IsNullOrEmpty(Server)) ValidationMessages.Add("Server address is required");
		}

		private void ValidateCredential()
		{
			if ((string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password)) ||
				(!string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password)))
				ValidationMessages.Add(
					"Username & password can't be blank together. " +
					"If mail server doesn't require credential, then clear both username and password");
		}

		protected void ValidatePort()
		{
			if (Port < 1) ValidationMessages.Add("Port can't be zero or negative");
		}
	}
}
