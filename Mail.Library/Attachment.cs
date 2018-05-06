using System.Collections.Generic;

namespace Mail.Library
{
	/// <summary>
	/// Email attachment
	/// </summary>
	public class Attachment : IValidatable
	{
		/// <summary>
		/// Fullpath of attachment. Required for sending email.
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// File name of attachment
		/// </summary>
		public string Filename => !string.IsNullOrEmpty(Path) ? System.IO.Path.GetFileName(Path) : null;

		/// <summary>
		/// MIME type
		/// </summary>
		public string MimeType { get; set; }

		/// <summary>
		/// Validate attachment
		/// </summary>
		/// <returns>Validation result</returns>
		public Validation Validate()
		{
			var message = new List<string>();
			if (string.IsNullOrEmpty(Path)) message.Add("Path is required");
			return new Validation(message);
		}

		/// <summary>
		/// Validate attachment
		/// </summary>
		/// <returns>Validation result</returns>
		public Validation Validate(string prefix) => Validate();
	}
}