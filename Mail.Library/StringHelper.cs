namespace Mail.Library
{
	/// <summary>
	/// Collection of string helper
	/// </summary>
	internal class StringHelper
	{
		/// <summary>
		/// Format message with additional prefix
		/// </summary>
		/// <param name="message">Actual message</param>
		/// <param name="prefix">Additional prefix to be added</param>
		/// <returns>Original <paramref name="message"/> if <paramref name="prefix"/> is empty/<code>null</code> or combination of <paramref name="prefix"/> and <paramref name="message"/></returns>
		internal static string Format(string message, string prefix)
		{
			return string.IsNullOrEmpty(prefix) ? message : $"{prefix} {message}";
		}
	}
}
