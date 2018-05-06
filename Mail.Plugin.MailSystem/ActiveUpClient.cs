using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using ActiveUp.Net.Mail;
using ActiveUp.Net.Security;
using Mail.Library;
using Address = ActiveUp.Net.Mail.Address;
using AddressCollection = ActiveUp.Net.Mail.AddressCollection;
using EmailMessage = Mail.Library.Message;
using Message = ActiveUp.Net.Mail.Message;

namespace Mail.Plugin.MailSystem
{
	[Export(typeof(IMailSender))]
	[ExportMetadata("Name", "MailSystem")]
	[ExportMetadata("Version", "0.1.0")]
	public class ActiveUpClient : IMailSender, IDisposable
	{
		private SmtpClient _client;

		public void Dispose()
		{
			if (_client != null)
			{
				if (_client.IsConnected) _client.Disconnect();
				_client = null;
			}

			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Send email message
		/// </summary>
		/// <param name="connection">connection property to mail server</param>
		/// <param name="email">email to be sent</param>
		/// <param name="errorMessage">error message when sending if any</param>
		/// <returns><code>true</code> when success and vice versa</returns>
		public bool Send(SendConnection connection, EmailMessage email, out string errorMessage)
		{
			_client = new SmtpClient();
			errorMessage = string.Empty;
			try
			{
				var mail = ConvertMessage(email);
				mail.CheckBuiltMimePartTree();
				var securityProtocol = SslProtocols.None;
				if (connection.Security == SecureType.None)
				{
					_client.Connect(connection.Server, connection.Port);
				}
				else
				{
					if ((connection.Security & SecureType.SslV2) == SecureType.SslV2) securityProtocol |= SslProtocols.Ssl2;
					if ((connection.Security & SecureType.SslV3) == SecureType.SslV3) securityProtocol |= SslProtocols.Ssl3;
					if ((connection.Security & SecureType.TlsV1) == SecureType.TlsV1) securityProtocol |= SslProtocols.Tls;
					if ((connection.Security & SecureType.TlsV11) == SecureType.TlsV11) securityProtocol |= SslProtocols.Tls11;
					if ((connection.Security & SecureType.TlsV12) == SecureType.TlsV12) securityProtocol |= SslProtocols.Tls12;
					_client.Connect(connection.Server, connection.Port);
					var sslStream = new SslStream(_client.GetStream(), true, (sender, certificate, chain, errors) => true);
					sslStream.Write(Encoding.UTF32.GetBytes("STLS\r\n"), 0, 6);
					sslStream.AuthenticateAsClient(connection.Server, null, securityProtocol, false);
					var sslHandShake =
						new SslHandShake(connection.Server, securityProtocol, (sender, certificate, chain, errors) => true);
					_client.ConnectSsl(connection.Server, connection.Port, sslHandShake);
				}

				string hostname = Dns.GetHostName();
				try
				{
					_client.Ehlo(hostname);
				}
				catch (SmtpException)
				{
					_client.Helo(hostname);
				}

				if (!string.IsNullOrEmpty(connection.Username) && !string.IsNullOrEmpty(connection.Password))
					_client.Authenticate(connection.Username, connection.Password, SaslMechanism.Login);
				if (!string.IsNullOrEmpty(mail.From.Email)) _client.MailFrom(mail.From.Email);
				_client.RcptTo(mail.To);
				if (mail.Cc.Any()) _client.RcptTo(mail.Cc);
				if (mail.Bcc.Any()) _client.RcptTo(mail.Bcc);
				_client.Data(mail.ToMimeString());
				_client.Disconnect();
				return true;
			}
			catch (Exception exception)
				when (exception is SmtpException ||
					  exception is SocketException ||
					  exception is IOException)
			{
				errorMessage = exception.GetLeafException();
				return false;
			}
		}

		private Message ConvertMessage(EmailMessage message)
		{
			var mail = new Message
			{
				IsHtml = message.UseHtml,
				Subject = message.Subject,
				From = !string.IsNullOrEmpty(message.Sender.Display)
					? new Address(message.Sender.Email, message.Sender.Display)
					: new Address(message.Sender.Email)
			};
			if (message.UseHtml)
				mail.BodyHtml = new MimeBody(BodyFormat.Html) { Text = message.Body };
			else
				mail.BodyText = new MimeBody(BodyFormat.Text) { Text = message.Body };

			void Add(AddressCollection collection, string email, string display)
			{
				if (string.IsNullOrEmpty(display))
					collection.Add(email);
				else
					collection.Add(email, display);
			}

			message.To.ForEach(dst => Add(mail.To, dst.Email, dst.Display));
			message.Cc.ForEach(dst => Add(mail.Cc, dst.Email, dst.Display));
			message.Bcc.ForEach(dst => Add(mail.Bcc, dst.Email, dst.Display));
			message.Attachments.ForEach(attachment =>
				mail.Attachments.Add(File.ReadAllBytes(attachment.Path), attachment.Filename));
			return mail;
		}
	}
}