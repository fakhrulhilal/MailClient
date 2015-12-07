using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mail.Library;
using netDumbster.smtp;
using NUnit.Framework;

namespace Mail.Plugin.MailKit.Test
{
    [TestFixture]
    class MailKitClientTest
    {
        private SimpleSmtpServer server;
        private IMailSender client;
        private SendConnection connection;
        private readonly List<Attachment> files = new List<Attachment>();

        [TestFixtureSetUp]
        public void SetUp()
        {
            server = SimpleSmtpServer.Start(Default.Port);
            client = new MailKitClient();
            connection = new SendConnection(Default.ServerHost, server.Port);
        }

        [SetUp]
        public void TestSetUp()
        {
            files.Clear();
        }

        [TestFixtureTearDown]
        public void Finish()
        {
            server.Stop();
        }

        [TearDown]
        public void TestFinish()
        {
            foreach (var file in files.Where(f => !string.IsNullOrEmpty(f.Path) && File.Exists(f.Path)))
            {
                try
                {
                    File.Delete(file.Path);
                }
                catch (IOException)
                { }
            }
        }

        [Test]
        [TestCaseSource(nameof(SendingSource), Category = "MailSender_MailKit")]
        public bool SendTest(Message message)
        {
            string errorMessage;
            if (client.Send(connection, message, out errorMessage))
            {
                Assert.That(server.ReceivedEmailCount, Is.GreaterThanOrEqualTo(1));
                var lastMessage = server.ReceivedEmail.LastMessage();
                Assert.IsNotNull(lastMessage);
                Assert.That(lastMessage.Body(), Is.EqualTo(message.Body));
                Assert.That(lastMessage.Subject(), Is.EqualTo(message.Subject));
                Assert.That(lastMessage.FromAddress.Address, Is.EqualTo(message.Sender.Email));
                return true;
            }

            Assert.That(errorMessage, Is.Not.Null.Or.Empty);
            return false;
        }

        private void GenerateAttachments()
        {
            var random = new Random();
            for (int i = 0, total = random.Next(1, 5); i < total; i++)
            {
                string tempFileName = Path.GetTempFileName();
                File.WriteAllText(tempFileName, Default.Text);
                files.Add(new Attachment
                {
                    Path = tempFileName,
                    MimeType = "text/plain"
                });
            }
        }

        IEnumerable SendingSource
        {
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
                files.ForEach(file => message.Attachments.Add(file));
                yield return new TestCaseData(message).SetName("BuiltIn_WithAttachments").Returns(true);
            }
        }

        private static class Default
        {
            public static string ServerHost = "localhost";
            public const int Port = 2525;
            public static readonly Address Sender = new Address("sender@domain.com");
            public static readonly string Email = "email@domain.com";
            public static readonly string Subject = "Default subject";
            public static readonly string Body = "Default body";
            public static string Text = "Dummy test";
        }
    }
}
