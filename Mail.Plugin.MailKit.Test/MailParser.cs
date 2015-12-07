using System;
using System.Linq;
using netDumbster.smtp;

namespace Mail.Plugin.MailKit.Test
{
	internal static class MailParser
	{
		internal static SmtpMessage LastMessage(this SmtpMessage[] messages)
		{
			if (messages == null || messages.Length < 1) throw new ArgumentNullException(nameof(messages));
			return 
				(from message in messages
				let date = System.DateTime.Parse(message.Headers["Date"])
				orderby date descending
				select message).FirstOrDefault();
		}

		internal static string Body(this SmtpMessage message)
		{
			if (message == null) throw new ArgumentNullException(nameof(message));
			return message.MessageParts?[0]?.BodyData;
		}

		internal static string Subject(this SmtpMessage message)
		{
			if (message == null) throw new ArgumentNullException(nameof(message));
			return message.Headers?["Subject"];
		}
	}
}
