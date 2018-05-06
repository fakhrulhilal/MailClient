using System;

namespace Mail.Library
{
	/// <summary>
	/// Connection for sending email
	/// </summary>
	public class SendConnection : BaseConnection
	{
		/// <summary>
		/// Initialize sending connection
		/// </summary>
		/// <param name="server">Mail server address</param>
		public SendConnection(string server) : this(server, SecureType.None)
		{
		}

		/// <summary>
		/// Initialize sending connection
		/// </summary>
		/// <param name="server">Mail server address</param>
		/// <param name="port">Mail server port</param>
		public SendConnection(string server, int port) : this(server)
		{
			if (port < 1) throw new ArgumentException("Port can't be zero or negative", nameof(port));
			Port = port;
		}

		/// <summary>
		/// Initialize sending connection
		/// </summary>
		/// <param name="server">Mail server address</param>
		/// <param name="security">Mail server security type</param>
		public SendConnection(string server, SecureType security)
		{
			if (string.IsNullOrEmpty(server)) throw new ArgumentNullException(nameof(server));
			Server = server;
			Protocol = MailProtocol.Smtp;
			Security = security;
			switch (security)
			{
				case SecureType.SslV2:
				case SecureType.SslV3:
					Port = 465;
					break;
				case SecureType.TlsV1:
				case SecureType.TlsV11:
				case SecureType.TlsV12:
					Port = 587;
					break;
				default:
					Port = 25;
					break;
			}
		}
	}
}