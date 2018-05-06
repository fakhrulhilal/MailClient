using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Mail;

namespace Mail.Library
{
    /// <summary>
    /// Email client using builtin .NET mail client
    /// </summary>
	[Export(typeof(IMailSender))]
	[ExportMetadata("Name", "BuiltIn")]
	public class MicrosoftClient : IMailSender, IPluginMetadata, IDisposable
	{
		private SmtpClient _client;

		public string Name => "BuiltIn";
		public string Version => "0.1.0";
		public IEnumerable<string> AssemblyDependency => new string[0];

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="connection">connection to SMTP server</param>
        /// <param name="message">email message</param>
        /// <param name="errorMessage">error message if any</param>
        /// <returns><code>false</code> when fails (see <paramref name="errorMessage"/> for more details) and vice versa</returns>
        public bool Send(SendConnection connection, Message message, out string errorMessage)
		{
			errorMessage = string.Empty;
			if (connection == null) throw new ArgumentNullException(nameof(connection));
			if (message == null) throw new ArgumentNullException(nameof(message));
	        var validation = message.Validate();
	        if (!validation.IsValid)
	        {
		        errorMessage = string.Join("\n", validation.Messages);
		        return false;
	        }

			using (_client = new SmtpClient(connection.Server, connection.Port))
			using (var mail = ((IComposable<MailMessage>)message).Compose())
			{
				try
				{
					//enable SSL untuk semua jenis security, baik SSL maupun TLS
					_client.EnableSsl = connection.Security != SecureType.None;

					// login using specified username and password
					if (!string.IsNullOrEmpty(connection.Username) && !string.IsNullOrEmpty(connection.Password))
					{
						_client.UseDefaultCredentials = false;
						_client.Credentials = new NetworkCredential(connection.Username, connection.Password);
					}

					_client.Send(mail);
					return true;
				}
				catch (SmtpException exception)
				{
					errorMessage = exception.GetLeafException();
					return false;
				}
			}
		}

		void IDisposable.Dispose()
		{
			if (_client != null)
			{
				_client.Dispose();
				_client = null;
			}

			GC.SuppressFinalize(this);
		}
	}
}
