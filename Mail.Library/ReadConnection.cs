using System;

namespace Mail.Library
{
	/// <summary>
	/// Connection for reading email
	/// </summary>
	public class ReadConnection : BaseConnection
	{
		/// <summary>
		/// Initialize reading connection
		/// </summary>
		/// <param name="protocol">Mail protocol type to be used</param>
		/// <param name="server">Mail server address</param>
		public ReadConnection(MailProtocol protocol, string server)
		{
			if (protocol == MailProtocol.Smtp) throw new ArgumentException("Can't use SMTP for reading", nameof(protocol));
			Protocol = protocol;
			if (string.IsNullOrEmpty(server)) throw new ArgumentNullException(nameof(server));
			Server = server;
		}

		/// <summary>
		/// Initialize reading connection
		/// </summary>
		/// <param name="server">Mail server address</param>
		public ReadConnection(string server) : this(MailProtocol.Imap, server)
		{
		}

		/// <summary>
		/// Initialize reading connection
		/// </summary>
		/// <param name="protocol">Mail protocol type to be used</param>
		/// <param name="server">Mail server address</param>
		/// <param name="port">Mail server port</param>
		public ReadConnection(MailProtocol protocol, string server, int port) : this(protocol, server)
		{
			if (port < 1) throw new ArgumentException("Port can't be zero or negative", nameof(port));
			Port = port;
		}

		/// <summary>
		/// Initialize reading connection
		/// </summary>
		/// <param name="protocol">Mail protocol type to be used</param>
		/// <param name="server">Mail server address</param>
		/// <param name="security">Mail server security type</param>
		public ReadConnection(MailProtocol protocol, string server, SecureType security) : this(protocol, server)
		{
			Security = security;
			switch (security)
			{
				case SecureType.SslV2:
				case SecureType.SslV3:
				case SecureType.TlsV1:
				case SecureType.TlsV11:
				case SecureType.TlsV12:
					Port = protocol == MailProtocol.Imap ? 993 : 995;
					break;
				default:
					Port = protocol == MailProtocol.Imap ? 143 : 110;
					break;
			}
		}
	}
}