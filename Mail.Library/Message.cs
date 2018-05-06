using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Mail.Library
{
	/// <summary>
	/// Email message
	/// </summary>
	public class Message : IValidatable, IComposable<MailMessage>
	{
		public Message()
		{
			UseHtml = true;
			Attachments = new List<Attachment>();
		}

		/// <summary>
		/// Email body
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// Determine whether <see cref="Body"/> uses HTML format or not
		/// </summary>
		public bool UseHtml { get; set; }

		/// <summary>
		/// Email subject
		/// </summary>
		public string Subject { get; set; }

		/// <summary>
		/// Email sender (FROM)
		/// </summary>
		public Address Sender { get; set; }

		/// <summary>
		/// Email attachments
		/// </summary>
		public List<Attachment> Attachments { get; private set; }

		/// <summary>
		/// Email To recipient
		/// </summary>
		public AddressCollection To { get; set; }

		/// <summary>
		/// Email Carbon Copy recipient
		/// </summary>
		public AddressCollection Cc { get; set; }

		/// <summary>
		/// Email Blind Carbon Copy recipient
		/// </summary>
		public AddressCollection Bcc { get; set; }

		/// <summary>
		/// Validate email message
		/// </summary>
		/// <returns>Validation result</returns>
		public Validation Validate()
		{
			var message = new List<string>();
			Validation validation;
			if (string.IsNullOrEmpty(Sender?.Email)) message.Add("Sender/from is required");
			if (string.IsNullOrEmpty(Body)) message.Add("Message body is required");
			if (To == null || To.Count < 1)
				message.Add("Please specifiy at least 1 destination To address");
			else
			{
				validation = To.Validate();
				if (!validation.IsValid) message.AddRange(validation.Messages);
			}
			Cc = Cc ?? new AddressCollection();
			validation = Cc.Validate("CC:");
			if (!validation.IsValid) message.AddRange(validation.Messages);
			Bcc = Bcc ?? new AddressCollection();
			validation = Bcc.Validate("BCC:");
			if (!validation.IsValid) message.AddRange(validation.Messages);

			return new Validation(message);
		}
		
		/// <summary>
		/// Validate email message
		/// </summary>
		/// <returns>Validation result</returns>
		public Validation Validate(string prefix) => Validate();

		MailMessage IComposable<MailMessage>.Compose()
		{
			var mail = new MailMessage
			{
				From = !string.IsNullOrEmpty(Sender.Display)
					? new MailAddress(Sender.Email, Sender.Display)
					: new MailAddress(Sender.Email),
				Subject = Subject,
				Body = Body,
				IsBodyHtml = UseHtml
			};
			Action<MailAddressCollection, string, string> add = (collection, email, display) =>
				collection.Add(!string.IsNullOrEmpty(display)
					? new MailAddress(email, display)
					: new MailAddress(email));
			To = To ?? new AddressCollection();
			Cc = Cc ?? new AddressCollection();
			Bcc = Bcc ?? new AddressCollection();
			To.ForEach(dst => add(mail.To, dst.Email, dst.Display));
			Cc.ForEach(dst => add(mail.CC, dst.Email, dst.Display));
			Bcc.ForEach(dst => add(mail.Bcc, dst.Email, dst.Display));
			Attachments.ForEach(attachment => mail.Attachments.Add(new System.Net.Mail.Attachment(attachment.Path)));
			return mail;
		}
	}
}
