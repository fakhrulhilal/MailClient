using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Mail.Library;
using NUnit.Framework;

namespace Mail.Plugin.MailKit.Test
{
	[TestFixture]
	class MailKitExportTest
	{
		[Test]
		public void WhenNullThenItThrowsArgumentNullException()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => _client.SaveAsEml(null));

			Assert.That(exception.ParamName, Is.EqualTo("message"));
		}

		[Test]
		public void WhenCreatedThenItShouldHaveValidVersion()
		{
			var message = CreateValidMessage();

			string eml = _client.SaveAsEml(message);

			Assert.That(GetField("MIME-Version", eml), Does.Match(@"\d+\.\d+"));
		}

		[Test]
		public void WhenCreatedThenItShouldHaveUniqueId()
		{
			var message = CreateValidMessage();

			string eml = _client.SaveAsEml(message);
		
			Assert.That(GetField("Message-Id", eml).Trim('<', '>'), Is.Not.Null.And.Not.Empty);
		}

		[Test]
		public void WhenCreatedThenItShouldUseNowAsDate()
		{
			var lastTime = DateTime.Now;
			var message = CreateValidMessage();

			string eml = _client.SaveAsEml(message);

			string dateString = GetField("Date", eml);
			var createdDate = DateTime.Parse(dateString);
			Assert.That(createdDate, Is.GreaterThanOrEqualTo(lastTime.StripMiliSeconds()));
		}

		[Test]
		public void WhenSenderSpecifiedThenItShouldHaveIt()
		{
			var message = CreateValidMessage();
			const string address = "sender@local.test";
			message.Sender = new Address(address);

			string eml = _client.SaveAsEml(message);

			Assert.That(GetField("From", eml), Is.EqualTo($"\"{address}\" <{address}>"));
		}

		[Test]
		public void WhenDestinationSpecifiedThenItShouldHaveIt()
		{
			var message = CreateValidMessage();
			const string address = "dest@local.test";
			message.To = new AddressCollection(address);

			string eml = _client.SaveAsEml(message);

			Assert.That(GetField("To", eml), Is.EqualTo($"\"{address}\" <{address}>"));
		}

		[Test]
		public void WhenSubjectSpecifiedThenItShouldHaveIt()
		{
			var message = CreateValidMessage();
			message.Subject = "mail subject";

			string eml = _client.SaveAsEml(message);

			Assert.That(GetField("Subject", eml), Is.EqualTo(message.Subject));
		}

		[Test]
		public void WhenNoAttachmentThenTypeIsHtml()
		{
			var message = CreateValidMessage();

			string eml = _client.SaveAsEml(message);

			Assert.That(GetField("Content-Type", eml), Is.EqualTo("text/html; charset=utf-8").IgnoreCase);
		}

		[Test]
		public void WhenContainingAttachmentThenTypeIsMultipart()
		{
			var message = CreateValidMessage();
			message.Attachments.Add(CreateDummyText());

			string eml = _client.SaveAsEml(message);

			Assert.That(GetField("Content-Type", eml), Does.StartWith("multipart/mixed;").IgnoreCase);
		}

		#region non test

		private readonly MailKitClient _client = new MailKitClient();
		private readonly List<Action> _cleanUpActions = new List<Action>();

		[SetUp]
		public void Setup() => _cleanUpActions.Clear();

		[TearDown]
		public void CleanUp() => _cleanUpActions.ForEach(action => action());

		private Message CreateValidMessage() => new Message
		{
			Sender = new Address("source@test.local"),
			To = new AddressCollection("dest@test.local"),
			Body = "content test",
			Subject = "subject test"
		};

		private string GetField(string key, string eml)
		{
			var match = Regex.Match(eml, $@"(?<field>{key}):\s*(?<value>[^\n]+)",
				RegexOptions.Multiline | RegexOptions.IgnoreCase);
			return match.Success ? match.Groups["value"].Value.Trim() : null;
		}

		private Attachment CreateDummyText()
		{
			string path = Path.GetTempFileName();
			File.WriteAllText(path, "hello world");
			_cleanUpActions.Add(() => File.Delete(path));
			return new Attachment
			{
				MimeType = "text/plain",
				Path = path
			};
		}

		#endregion
	}
}
