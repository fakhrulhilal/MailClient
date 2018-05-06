using System;
using System.IO;
using Mail.Library;
using MimeKit;

namespace Mail.Plugin.MailKit
{
	/// <summary>
	/// Collection of extension helper
	/// </summary>
	internal static class ConvertionExtension
	{
		/// <summary>
		/// Convert email address
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		internal static MailboxAddress Convert(this Address address)
		{
			if (address == null) throw new ArgumentNullException(nameof(address));
			return new MailboxAddress(
				!string.IsNullOrEmpty(address.Display) ? address.Display : address.Email,
				address.Email);
		}

		/// <summary>
		/// Convert email message
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		internal static MimeMessage Convert(this Message message)
		{
			if (message == null) throw new ArgumentNullException(nameof(message));
			var output = new MimeMessage
			{
				Subject = message.Subject,
				Sender = message.Sender.Convert()
			};
			var body = new BodyBuilder();
			if (message.UseHtml)
				body.HtmlBody = message.Body;
			else
				body.TextBody = message.Body;
			foreach (var attachment in message.Attachments)
				body.Attachments.Add(attachment.Filename, File.OpenRead(attachment.Path));
			output.Body = body.ToMessageBody();
			output.From.Add(output.Sender);
			message.To.ForEach(destination => output.To.Add(destination.Convert()));
			message.Cc.ForEach(destination => output.Cc.Add(destination.Convert()));
			message.Bcc.ForEach(destination => output.Bcc.Add(destination.Convert()));
			return output;
		}
	}
}