using System.Security.Authentication;

namespace Mail.Library
{
	/// <summary>
	/// Secure connection type
	/// </summary>
	[System.Flags]
	public enum SecureType
    {
		/// <summary>
		/// Without secure connection
		/// </summary>
		None = SslProtocols.None,

		/// <summary>
		/// SSL v2.0
		/// </summary>
		SslV2 = SslProtocols.Ssl2,

		/// <summary>
		/// SSL v3.0
		/// </summary>
		SslV3 = SslProtocols.Ssl3,

		/// <summary>
		/// TLS v1.0
		/// </summary>
		TlsV1 = SslProtocols.Tls,

		/// <summary>
		/// TLS v1.1
		/// </summary>
		TlsV11 = SslProtocols.Tls11,

		/// <summary>
		/// TLS v1.2
		/// </summary>
		TlsV12 = SslProtocols.Tls12,

		/// <summary>
		/// Using SSL v3, TLS v1.0 &amp; TLS v1.2
		/// </summary>
		Default = TlsV1 | TlsV11 | TlsV12
	}
}
