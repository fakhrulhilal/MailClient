using Mail.Library.Configuration;
using NUnit.Framework;

namespace Mail.Library.Test.Configuration
{
	[TestFixture]
	internal class IniLineParserTest
	{
		[Test]
		[TestCase("", "", false, Category = "INILineParser", TestName = "Comment_Empty")]
		[TestCase("; hello world", "hello world", true, Category = "INILineParser", TestName = "Comment_Normal")]
		[TestCase(" ; hello world", "hello world", true, Category = "INILineParser", TestName = "Comment_PreceededSpace")]
		public void CommentTest(string line, string parsed, bool expected)
		{
			IniComment iniLine;
			if (expected)
			{
				Assert.That(IniLineParser.ParseComment(line, out iniLine), Is.True);
				Assert.That(iniLine, Is.Not.Null);
				Assert.That(iniLine.Comment, Is.EqualTo(parsed));
			}
			else
			{
				Assert.That(IniLineParser.ParseComment(line, out iniLine), Is.False);
				Assert.That(iniLine, Is.Null);
			}
		}

		[Test]
		[TestCase("key = value", "key", "value", null, true, Category = "INILineParser", TestName = "Pair_Valid")]
		[TestCase("key_123 = whatever value is it ok whether to use number (ex: 123) or alpha", "key_123",
			"whatever value is it ok whether to use number (ex: 123) or alpha", null, true, Category = "INILineParser",
			TestName = "Pair_ComplexValue")]
		[TestCase("", "", "", null, false, Category = "INILineParser", TestName = "Pair_EmptyString")]
		[TestCase("key=value", "key", "value", null, true, Category = "INILineParser", TestName = "Pair_NotContainingSpace")]
		[TestCase("  key = value", "key", "value", null, true, Category = "INILineParser",
			TestName = "Pair_ContainingAndPreceededSpace")]
		[TestCase("  key&_1 = value", "", "", null, false, Category = "INILineParser", TestName = "Pair_Invalid")]
		[TestCase("; key = value", "", "", null, false, Category = "INILineParser", TestName = "Pair_Commented")]
		[TestCase("key; = value", "", "", null, false, Category = "INILineParser", TestName = "Pair_CommentedInMiddle")]
		[TestCase("key = value ; additional comment", "key", "value", "additional comment", true, Category = "INILineParser",
			TestName = "Pair_ContainingComment")]
		[TestCase("key \t= value ; additional comment", "key", "value", "additional comment", true,
			Category = "INILineParser", TestName = "Pair_ContainingWhitespaceAndComment")]
		public void PairTest(string line, string key, string value, string comment, bool expected)
		{
			IniPair iniLine;
			if (expected)
			{
				Assert.That(IniLineParser.ParseKeyValuePair(line, out iniLine), Is.True);
				Assert.That(iniLine, Is.Not.Null);
				Assert.That(iniLine.Key, Is.EqualTo(key));
				Assert.That(iniLine.Value, Is.EqualTo(value));
				if (comment != null) Assert.That(iniLine.Comment, Is.EqualTo(comment));
			}
			else
			{
				Assert.That(IniLineParser.ParseKeyValuePair(line, out iniLine), Is.False);
				Assert.That(iniLine, Is.Null);
			}
		}

		[Test]
		[TestCase("[]", "", null, false, Category = "INILineParser", TestName = "Section_Empty")]
		[TestCase("", "", null, false, Category = "INILineParser", TestName = "Section_EmptyString")]
		[TestCase("[section]", "section", null, true, Category = "INILineParser", TestName = "Section_Normal")]
		[TestCase("[   section ]", "section", null, true, Category = "INILineParser", TestName = "Section_ContainingSpace")]
		[TestCase("  [   section ]", "section", null, true, Category = "INILineParser",
			TestName = "Section_ContainingAndPreceededSpace")]
		[TestCase("  section", "", null, false, Category = "INILineParser", TestName = "Section_Invalid")]
		[TestCase(";[section]", "", null, false, Category = "INILineParser", TestName = "Section_Commented")]
		[TestCase("[secti;on]", "", null, false, Category = "INILineParser", TestName = "Section_CommentedInMiddle")]
		[TestCase("[section] ; additional comment", "section", "additional comment", true, Category = "INILineParser",
			TestName = "Section_ContainingComment")]
		public void SectionTest(string line, string parsed, string comment, bool expected)
		{
			IniSection iniLine;
			if (expected)
			{
				Assert.That(IniLineParser.ParseSection(line, out iniLine), Is.True);
				Assert.That(iniLine, Is.Not.Null);
				Assert.That(iniLine.Name, Is.EqualTo(parsed));
				if (comment != null) Assert.That(iniLine.Comment, Is.EqualTo(comment));
			}
			else
			{
				Assert.That(IniLineParser.ParseSection(line, out iniLine), Is.False);
				Assert.That(iniLine, Is.Null);
			}
		}
	}
}