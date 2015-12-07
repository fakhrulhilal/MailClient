using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mail.Library.Configuration
{
	public abstract class IniBase
	{
		/// <summary>
		/// INI line string
		/// </summary>
		protected string Sentence;

		/// <summary>
		/// Line position, start from 1
		/// </summary>
		public int Position { get; set; }

		/// <summary>
		/// Additional comment in line
		/// </summary>
		public string Comment { get; set; }
	}

	/// <summary>
	/// INI section
	/// </summary>
	public class IniSection : IniBase
	{
		/// <summary>
		/// Section name.
		/// Valid characters: alphabet, numeric, underscore, space.
		/// </summary>
		public string Name { get; set; }

		public List<IniPair> Pairs { get; private set; }

		/// <summary>
		/// Initialize new section
		/// </summary>
		/// <param name="name">Section name, valid characters: alphabet, numeric, underscore, space.</param>
		public IniSection(string name)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
			Name = name.Trim();
			Pairs = new List<IniPair>();
		}

		public override string ToString()
		{
			return !string.IsNullOrEmpty(Comment)
				? $"[{Name}] ; {Comment}"
				: $"[{Name}]";
		}
	}

	/// <summary>
	/// Config key-value pair
	/// </summary>
	public class IniPair : IniBase
	{
		/// <summary>
		/// Config key name
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Config value
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Initialize new key-value pair
		/// </summary>
		/// <param name="key">Key name</param>
		/// <param name="value">Value</param>
		public IniPair(string key, string value)
		{
			if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
			if (value == null) throw new ArgumentNullException(nameof(value));
			Key = key.Trim();
			Value = value.Trim();
		}

		public override string ToString()
		{
			return !string.IsNullOrEmpty(Comment)
				? $"{Key} = {Value} ; {Comment}"
				: $"{Key} = {Value}";
		}
	}

	/// <summary>
	/// INI full line comment, preceeded by ';' character
	/// </summary>
	public class IniComment : IniBase
	{
		/// <summary>
		/// Initialize new comment
		/// </summary>
		/// <param name="comment">Comment description</param>
		public IniComment(string comment)
		{
			if (string.IsNullOrEmpty(comment)) throw new ArgumentNullException(nameof(comment));
			Comment = comment.Trim();
		}

		public override string ToString()
		{
			return $"; {Comment}";
		}
	}

	/// <summary>
	/// INI line parser
	/// </summary>
	public class IniLineParser
	{
		internal static Regex SectionRegex = new Regex(@"^\s*\[\s*(?<name>[\w\x20]+)\s*\]\s*(;\s*(?<comment>[^\n]*))?$");
		internal static Regex PairRegex = new Regex(@"^\s*(?<name>[\w\x20]+)\s*=\s*(?<value>[^;\n]*)\s*(;\s*(?<comment>[^\n]*))?");
		internal static Regex CommentRegex = new Regex(@"^\s*;\s*(?<comment>[^\n]*)");

		/// <summary>
		/// Try parse line of section
		/// </summary>
		/// <param name="line">INI line</param>
		/// <param name="section">INI section if <paramref name="line"/> is valid</param>
		/// <returns><code>true</code> when <paramref name="line"/> is valid section</returns>
		public static bool ParseSection(string line, out IniSection section)
		{
		    if (string.IsNullOrEmpty(line))
		    {
		        section = null;
		        return false;
		    }
			Match match = SectionRegex.Match(line);
			if (match.Success)
			{
				section = new IniSection(match.Groups["name"].Value.Trim());
				if (match.Groups["comment"].Success) section.Comment = match.Groups["comment"].Value.Trim();
				return true;
			}

			section = null;
			return false;
		}

		/// <summary>
		/// Try parse line of key-value pair
		/// </summary>
		/// <param name="line">INI line</param>
		/// <param name="keyValuePair">INI key-value pair if <paramref name="line"/> is valid</param>
		/// <returns><code>true</code> when <paramref name="line"/> is valid key-value pair</returns>
		public static bool ParseKeyValuePair(string line, out IniPair keyValuePair)
		{
		    if (string.IsNullOrEmpty(line))
		    {
		        keyValuePair = null;
		        return false;
		    }
			Match match = PairRegex.Match(line);
			if (match.Success)
			{
				keyValuePair = new IniPair(match.Groups["name"].Value.Trim(), match.Groups["value"].Value.Trim());
				if (match.Groups["comment"].Success) keyValuePair.Comment = match.Groups["comment"].Value.Trim();
				return true;
			}

			keyValuePair = null;
			return false;
		}

		/// <summary>
		/// Try parse line of comment
		/// </summary>
		/// <param name="line">INI line</param>
		/// <param name="comment">INI comment if <paramref name="line"/> is valid</param>
		/// <returns><code>true</code> when <paramref name="line"/> is valid comment</returns>
		public static bool ParseComment(string line, out IniComment comment)
		{
		    if (string.IsNullOrEmpty(line))
		    {
		        comment = null;
		        return false;
		    }
			Match match = CommentRegex.Match(line);
			if (match.Success)
			{
				comment = new IniComment(match.Groups["comment"].Value.Trim());
				return true;
			}

			comment = null;
			return false;
		}
	}
}
