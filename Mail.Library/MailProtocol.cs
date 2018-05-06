namespace Mail.Library
{
	/// <summary>
	/// Email protocol
	/// </summary>
	public enum MailProtocol
	{
		/// <summary>
		/// Simple Mail Transport Protocol for sending email
		/// </summary>
		Smtp,

		/// <summary>
		/// Internet Message Access Protocol for reading email
		/// </summary>
		Imap,

		/// <summary>
		/// Post Office Protocol for reading email
		/// </summary>
		Pop
	}
}