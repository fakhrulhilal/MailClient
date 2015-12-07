using System;
using System.Collections.Generic;
using System.Linq;

namespace Mail.Library
{
	/// <summary>
	/// Collection of email address split by separator character
	/// </summary>
	public class AddressCollection : List<Address>, IValidatable
	{
		/// <summary>
		/// Initialize collection of email address by string
		/// </summary>
		/// <param name="addresses">Collection of email address split by ';' as separator</param>
		public AddressCollection(string addresses)
		{
			if (addresses == null) throw new ArgumentNullException(nameof(addresses));
			ParseMultipleAddress(addresses).ForEach(Add);
		}

		public AddressCollection()
		{
		}

		private List<Address> ParseMultipleAddress(string addresses, char separator = ';')
		{
			if (string.IsNullOrEmpty(addresses)) return new List<Address>();
			var splitted = addresses.Split(separator);
			var output = new List<Address>();
			splitted.ToList().ForEach(address => output.Add(new Address(address.Trim())));
			return output;
		}

		/// <summary>
		/// Validate collection of email address
		/// </summary>
		/// <returns>Validation result</returns>
		public Validation Validate() => Validate(null);

		/// <summary>
		/// Validate collection of email address with adding prefix in error validation
		/// </summary>
		/// <param name="prefix">Prefix to be added on each error validation message</param>
		/// <returns>Validation result</returns>
		public Validation Validate(string prefix)
		{
			if (Count < 1) return new Validation();
			var validations = this.Select(address => address.Validate(prefix)).ToList();
			var messages = new List<string>();
			foreach (var validation in validations.Where(val => val.Messages.Any()))
			{
				messages.AddRange(validation.Messages);
			}
			return new Validation(messages);
		}
	}
}
