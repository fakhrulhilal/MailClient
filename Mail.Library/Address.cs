using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mail.Library
{
	/// <summary>
	/// Email address
	/// </summary>
	public class Address : IValidatable
	{
		private readonly string _fullRegex = $@"(?<display>[a-z0-9,\.\-_ ]+)\s*\<{MailRegex}>";
		private readonly bool _isOriginalAddressValid = true;
		private readonly string _originalAddress;
		private const string MailRegex = @"(?<mail>(?<username>[a-z0-9\._]+)@(?<domain>[a-z0-9\-\._]+\.\w+))";
		public string Display { get; set; }

		/// <summary>
		/// Email address using username@domain.com format
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Username in <see cref="Email" />
		/// </summary>
		public string Username
		{
			get
			{
				if (string.IsNullOrEmpty(Email)) return null;
				var match = Regex.Match(Email, MailRegex, RegexOptions.IgnoreCase);
				return match.Success
					? match.Groups["username"].Value
					: null;
			}
		}

		/// <summary>
		/// Domain in <see cref="Email" />
		/// </summary>
		public string Domain
		{
			get
			{
				if (string.IsNullOrEmpty(Email)) return null;
				var match = Regex.Match(Email, MailRegex, RegexOptions.IgnoreCase);
				return match.Success
					? match.Groups["domain"].Value
					: null;
			}
		}

		public string FullAddress
		{
			get
			{
				if (!string.IsNullOrEmpty(Display) && !string.IsNullOrEmpty(Email)) return $"{Display} <{Email}>";
				if (!string.IsNullOrEmpty(Email)) return Email;
				return string.Empty;
			}
		}

		/// <summary>
		/// Initialize email address
		/// </summary>
		/// <param name="address">Full email address, ex: Display Name &lt;username@domain.com&gt; or username@domain.com</param>
		public Address(string address)
		{
			if (!string.IsNullOrEmpty(address))
			{
				_originalAddress = address;
				string email, display;
				ParseDestination(address, out email, out display);
				if (!string.IsNullOrEmpty(email)) Email = email;
				if (!string.IsNullOrEmpty(display)) Display = display;
				_isOriginalAddressValid = !string.IsNullOrEmpty(email);
			}
		}

		/// <summary>
		/// Initialize empty address
		/// </summary>
		public Address() : this(null)
		{
		}

		/// <summary>
		/// Validate email address
		/// </summary>
		/// <returns>Validation result</returns>
		public Validation Validate() => Validate(null);

		/// <summary>
		/// Validate email address with adding prefix in error validation
		/// </summary>
		/// <param name="prefix">Prefix to be added on each error validation message</param>
		/// <returns>Validation result</returns>
		public Validation Validate(string prefix)
		{
			var messages = new List<string>();
			if (string.IsNullOrEmpty(Email))
				messages.Add(StringHelper.Format("Email address is required", prefix));
			else if (!Regex.IsMatch(Email, MailRegex, RegexOptions.IgnoreCase))
				messages.Add(StringHelper.Format("Email is not valid format", prefix));
			else if (!_isOriginalAddressValid)
				messages.Add($"{_originalAddress} is not valid address");
			return new Validation(messages);
		}

		/// <summary>
		/// Give full email address representation
		/// </summary>
		/// <returns></returns>
		public override string ToString() => FullAddress;

		private void ParseDestination(string emailFormat, out string address, out string display)
		{
			address = null;
			display = null;
			if (string.IsNullOrEmpty(emailFormat)) return;
			emailFormat = emailFormat.Trim();
			var match = Regex.Match(emailFormat, _fullRegex, RegexOptions.IgnoreCase);
			if (match.Success)
			{
				address = match.Groups["mail"].Value.Trim();
				display = match.Groups["display"].Value.Trim();
				return;
			}

			match = Regex.Match(emailFormat, MailRegex, RegexOptions.IgnoreCase);
			if (match.Success) address = match.Groups["mail"].Value.Trim();
		}
	}
}