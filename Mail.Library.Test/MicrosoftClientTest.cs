using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using netDumbster.smtp;
using NUnit.Framework;

namespace Mail.Library.Test
{
	[TestFixture]
	internal class MicrosoftClientTest
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			_server = SimpleSmtpServer.Start(2525);
			_client = new MicrosoftClient();
			_connection = new SendConnection("localhost", _server.Port);
		}

		[SetUp]
		public void TestSetUp()
		{
			_files.Clear();
		}

		[TestFixtureTearDown]
		public void Finish()
		{
			_server.Stop();
		}

		[TearDown]
		public void TestFinish()
		{
			foreach (var file in _files.Where(f => !string.IsNullOrEmpty(f.Path) && File.Exists(f.Path)))
				try
				{
					File.Delete(file.Path);
				}
				catch (IOException)
				{
				}
		}

		[Test]
		[TestCaseSource(nameof(SendingSource), Category = "MailSender")]
		public bool SendTest(Message message)
		{
			if (_client.Send(_connection, message, out string errorMessage))
			{
				Assert.That(_server.ReceivedEmailCount, Is.GreaterThanOrEqualTo(1));
				var lastMessage = _server.ReceivedEmail.LastMessage();
				Assert.IsNotNull(lastMessage);
				Assert.That(lastMessage.Body(), Is.EqualTo(message.Body));
				Assert.That(lastMessage.Subject(), Is.EqualTo(message.Subject));
				Assert.That(lastMessage.FromAddress.Address, Is.EqualTo(message.Sender.Email));
				return true;
			}

			Assert.That(errorMessage, Is.Not.Null.Or.Empty);
			return false;
		}

		#region non test

		private SimpleSmtpServer _server;
		private IMailSender _client;
		private SendConnection _connection;
		private readonly List<Attachment> _files = new List<Attachment>();

		private void GenerateAttachments()
		{
			var random = new Random();
			for (int i = 0, total = random.Next(1, 5); i < total; i++)
			{
				string tempFileName = Path.GetTempFileName();
				File.WriteAllText(tempFileName, Default.Text);
				_files.Add(new Attachment
				{
					Path = tempFileName,
					MimeType = "text/plain"
				});
			}
		}

		private IEnumerable SendingSource
		{
			// ReSharper disable once UnusedMember.Local - called by NUnit
			get
			{
				yield return new TestCaseData(new Message
				{
					Body = Default.Body,
					Subject = Default.Subject,
					Sender = Default.Sender,
					To = new AddressCollection(Default.Email)
				}).SetName("BuiltIn_MinimumMessage").Returns(true);
				yield return new TestCaseData(new Message
				{
					Body = Default.Body,
					Subject = Default.Subject,
					Sender = Default.Sender
				}).SetName("BuiltIn_WithoutDestination").Returns(false);
				yield return new TestCaseData(new Message
				{
					Subject = Default.Subject,
					Sender = Default.Sender,
					To = new AddressCollection(Default.Email)
				}).SetName("BuiltIn_WithoutBody").Returns(false);
				yield return new TestCaseData(new Message
				{
					Body = string.Empty,
					Subject = Default.Subject,
					Sender = Default.Sender,
					To = new AddressCollection(Default.Email)
				}).SetName("BuiltIn_EmptyBody").Returns(false);
				yield return new TestCaseData(new Message
				{
					Body = Default.Body,
					Subject = Default.Subject,
					To = new AddressCollection(Default.Email)
				}).SetName("BuiltIn_WithoutSender").Returns(false);
				GenerateAttachments();
				var message = new Message
				{
					Body = Default.Body,
					Subject = Default.Subject,
					Sender = Default.Sender,
					To = new AddressCollection(Default.Email)
				};
				_files.ForEach(file => message.Attachments.Add(file));
				yield return new TestCaseData(message).SetName("BuiltIn_WithAttachments").Returns(true);
			}
		}

		private static class Default
		{
			public static readonly Address Sender = new Address("sender@domain.com");
			public static readonly string Email = "email@domain.com";
			public static readonly string Subject = "Default subject";
			public static readonly string Body = "Default body";
			public static readonly string Text = "Dummy test";
		}

		#endregion
	}
}