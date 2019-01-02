namespace Mail.Library
{
	/// <summary>
	/// Export mail message to certain format
	/// </summary>
	public interface IMailExport
	{
		/// <summary>
		/// Save as EML file
		/// </summary>
		/// <param name="message">mail message</param>
		/// <returns>string in EML format</returns>
		string SaveAsEml(Message message);
	}
}
