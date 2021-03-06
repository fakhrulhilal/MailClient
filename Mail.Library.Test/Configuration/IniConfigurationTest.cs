﻿using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Mail.Library.Configuration;
using NUnit.Framework;

namespace Mail.Library.Test.Configuration
{
	[TestFixture]
	[Category("INIConfiguration")]
	internal class IniConfigurationTest
	{
		[OneTimeSetUp]
		public void Setup()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			string path = Path.Combine(OriginalAssemblyLocation, @"Configuration\config.ini");
			_configuration = new IniConfiguration(path, "General");
			_configuration.Parse();
			_blankConfiguration = new IniConfiguration();
		}

		[OneTimeTearDown]
		public void Finish()
		{
			if (string.IsNullOrEmpty(_tempPath)) return;
			try
			{
				File.Delete(_tempPath);
			}
			catch (IOException)
			{
			}
		}

		[Test]
		public void DumpTest()
		{
			var file = new IniConfiguration(_configuration.Path);
			file.Parse();
			var config1 = file.GetConfig<Metadata>();
			config1.NotFound3 = 100;
			config1.NotFound2 = false;
			config1.NotFound4 = 250m;
			file.SetConfig(config1);
			file.SetConfig("New Section", "new key", "new value");
			file.Write(_tempPath, true);
			file.Path = _tempPath;
			file.Parse();

			config1 = file.GetConfig<Metadata>();
			Assert.That(config1.NotFound2, Is.EqualTo(false));
			Assert.That(config1.NotFound3, Is.EqualTo(100));
			Assert.That(config1.NotFound4, Is.EqualTo(250m));
			Assert.That(file.GetConfig<string>("new section", "new key"), Is.EqualTo("new value"));

			Assert.That(file.GetConfig<string>("mailer", "sender"), Is.EqualTo("MailKit"));
			Assert.That(file.GetConfig<string>("mailer", "reader"), Is.EqualTo("builtin"));
		}

		[Test]
		public void GetWithMetadataTest()
		{
			var config1 = _configuration.GetConfig<Metadata>();
			Assert.That(config1.Position, Is.EqualTo(2.3));
			Assert.That(config1.NotFound, Is.True);
			Assert.That(config1.NotFound2, Is.True);
			Assert.That(config1.NotFound3, Is.EqualTo(default(int?)));
			Assert.That(config1.NotFound4, Is.EqualTo(default(decimal)));

			var config2 = _configuration.GetConfig<Library>();
			Assert.That(config2.Sender, Is.EqualTo("MailKit"));
			Assert.That(config2.Reader, Is.EqualTo("builtin"));
			Assert.That(config2.Dummy, Is.EqualTo(null));

			var config3 = _blankConfiguration.GetConfig<Metadata>();
			Assert.That(config3.Position, Is.EqualTo(-1d));
			Assert.That(config3.NotFound, Is.True);
			Assert.That(config3.NotFound2, Is.True);
			Assert.That(config3.NotFound3, Is.EqualTo(default(int?)));
			Assert.That(config3.NotFound4, Is.EqualTo(default(decimal)));

			var config4 = _blankConfiguration.GetConfig<Library>();
			Assert.That(config4.Sender, Is.EqualTo("default"));
			Assert.That(config4.Reader, Is.EqualTo(default(string)));
		}

		[Test]
		public void GetWithStringTest()
		{
			Assert.That(_configuration.GetConfig<string>("Key1"), Is.EqualTo("value1"));
			Assert.That(_configuration.GetConfig<int>("Key2"), Is.EqualTo(12));
			Assert.That(_configuration.GetConfig<string>("Key3"), Is.EqualTo("another 3"));
			Assert.That(_configuration.GetConfig<int>("key4"), Is.EqualTo(4));

			Assert.That(_configuration.GetConfig<string>("mailer", "sender"), Is.EqualTo("MailKit"));
			Assert.That(_configuration.GetConfig<string>("mailer", "reader"), Is.EqualTo("builtin"));
			Assert.That(_configuration.GetConfig<decimal>("another section", "position"), Is.EqualTo(2.3m));
			Assert.That(_configuration.GetConfig<string>("another section", "hello"), Is.EqualTo("world"));
			Assert.That(_configuration.GetConfig<string>("another section", "foo"), Is.EqualTo("bar"));
		}

		[Test]
		public void SetWithMetadataTest()
		{
			var file = new IniConfiguration(_configuration.Path);
			var config1 = new Metadata();
			IniConfiguration.SetDefault(config1);
			config1.Position = 50;
			config1.NotFound = false;
			file.SetConfig(config1);
			var compare1 = file.GetConfig<Metadata>();
			Assert.That(compare1.Position, Is.EqualTo(config1.Position));
			Assert.That(compare1.NotFound, Is.EqualTo(config1.NotFound));
			Assert.That(compare1.NotFound2, Is.EqualTo(true));
			Assert.That(compare1.NotFound3, Is.EqualTo(default(int?)));
			Assert.That(compare1.NotFound4, Is.EqualTo(default(decimal)));
		}

		[Test]
		public void SetWithStringTest()
		{
			var file = new IniConfiguration(_configuration.Path);
			file.Parse();
			file.SetConfig("key1", "value1_1");
			file.SetConfig("Key3", "33");
			Assert.That(file.GetConfig<string>("Key1"), Is.EqualTo("value1_1"));
			Assert.That(file.GetConfig<int>("Key2"), Is.EqualTo(12));
			Assert.That(file.GetConfig<int>("Key3"), Is.EqualTo(33));
			Assert.That(file.GetConfig<int>("key4"), Is.EqualTo(4));
		}

		#region non test

		private IniConfiguration _configuration;
		private IniConfiguration _blankConfiguration;
		private readonly string _tempPath = Path.GetTempFileName();

		private string OriginalAssemblyLocation
		{
			get
			{
				string original = typeof(IniConfigurationTest).Assembly.CodeBase;
				string path = Regex.Replace(original, @"^file:/+", string.Empty, RegexOptions.IgnoreCase);
				path = path.Replace('/', '\\');
				return Path.GetDirectoryName(path);
			}
		}

		[IniSection("Another Section")]
		public class Metadata
		{
			[IniKey("position", -1d)]
			public double Position { get; set; }

			[IniKey("unknown1", true)]
			public bool NotFound { get; set; }

			[IniKey("unknown2", true)]
			public bool? NotFound2 { get; set; }

			[IniKey("unknown3")]
			public int? NotFound3 { get; set; }

			public decimal NotFound4 { get; set; }
		}

		[IniSection("mailer")]
		public class Library
		{
			[IniKey("sender", "default")]
			public string Sender { get; set; }

			[IniKey("reader")]
			public string Reader { get; set; }

			public string Dummy { get; set; }
		}

		#endregion
	}
}