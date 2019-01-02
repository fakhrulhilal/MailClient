using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Mail.Library;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Mail.Plugin.MailKit
{
	/// <summary>
	/// Email client using MailKit library
	/// </summary>
	[Export(typeof(IMailSender))]
	[Export(typeof(IMailExport))]
	[ExportMetadata("Name", "MailKit")]
	public class MailKitClient : IMailSender, IMailExport, IPluginMetadata, IDisposable
	{
		private readonly Type[] _authenticationExceptions =
		{
			typeof(NotSupportedException), typeof(AuthenticationException),
			typeof(SaslException), typeof(IOException),
			typeof(SmtpCommandException), typeof(SmtpProtocolException)
		};

		private readonly Type[] _connectionExceptions =
		{
			typeof(NotSupportedException), typeof(IOException),
			typeof(SmtpCommandException), typeof(SmtpProtocolException)
		};

		private readonly Type[] _sendExceptions =
		{
			typeof(InvalidOperationException), typeof(NotSupportedException),
			typeof(IOException), typeof(SmtpCommandException), typeof(SmtpProtocolException)
		};

		private SmtpClient _client;

		void IDisposable.Dispose()
		{
			if (_client != null)
			{
				if (_client.IsConnected) _client.Disconnect(true);
				_client.Dispose();
				_client = null;
			}

			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Send email
		/// </summary>
		/// <param name="connection">connection to SMTP server</param>
		/// <param name="message">email message</param>
		/// <param name="errorMessage">error message if any</param>
		/// <returns><code>false</code> when fails (see <paramref name="errorMessage" /> for more details) and vice versa</returns>
		public bool Send(SendConnection connection, Message message, out string errorMessage)
		{
			if (connection == null) throw new ArgumentNullException(nameof(connection));
			if (message == null) throw new ArgumentNullException(nameof(message));
			var validation = connection.Validate();
			if (!validation.IsValid)
			{
				errorMessage = string.Join("\n", validation.Messages);
				return false;
			}

			validation = message.Validate();
			if (!validation.IsValid)
			{
				errorMessage = string.Join("\n", validation.Messages);
				return false;
			}

			errorMessage = null;
			using (_client = new SmtpClient())
			{
				_client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
				try
				{
					_client.Connect(
						connection.Server,
						connection.Port,
						connection.Security == SecureType.None ? SecureSocketOptions.None : SecureSocketOptions.Auto);
				}
				catch (InvalidOperationException)
				{
				}
				catch (Exception exception) when (_connectionExceptions.Any(ex => ex.IsAssignableFrom(exception.GetType())))
				{
					errorMessage =
						$"Can't establish connection to {connection.Server}:{connection.Port} because of: {exception.GetLeafException()}";
					return false;
				}

				if (!string.IsNullOrEmpty(connection.Username) &&
					!string.IsNullOrEmpty(connection.Password))
					try
					{
						_client.Authenticate(connection.Username, connection.Password);
					}
					catch (InvalidOperationException)
					{
					}
					catch (Exception exception)
						when (_authenticationExceptions.Any(ex => ex.IsAssignableFrom(exception.GetType())))
					{
						errorMessage =
							$"Can't authenticate to {connection.Server}:{connection.Port} using username '{connection.Username}' because of: {exception.GetLeafException()}";
						return false;
					}

				try
				{
					_client.Send(message.Convert());
				}
				catch (Exception exception) when (_sendExceptions.Any(ex => ex.IsAssignableFrom(exception.GetType())))
				{
					errorMessage = $"Can't send message because of: {exception.GetLeafException()}";
					return false;
				}
			}

			return true;
		}

		public string Name => "MailKit";

		/// <inheritdoc />
		public string SaveAsEml(Message message) =>
			message?.Convert().ToString() ?? throw new ArgumentNullException(nameof(message));
	}
}