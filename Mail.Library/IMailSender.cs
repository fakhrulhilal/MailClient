namespace Mail.Library
{
	/// <summary>
	/// Contract for sending email
	/// </summary>
	public interface IMailSender
	{
		/// <summary>
		/// Send email using SMTP server
		/// </summary>
		/// <param name="connection">connection property to mail server</param>
		/// <param name="message">email to be sent</param>
		/// <param name="errorMessage">error message from server if any</param>
		/// <returns><code>false</code> when fail (see <see cref="errorMessage" /> for detail) and vice versa</returns>
		bool Send(SendConnection connection, Message message, out string errorMessage);
	}
}