using System;

namespace Mail.Library.Configuration
{
	/// <summary>
	/// Section
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class IniSectionAttribute : Attribute
	{
		/// <summary>
		/// Section name.
		/// Valid characters: alphabet, numeric, underscore, space.
		/// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Additional comment
        /// </summary>
	    public string Comment { get; set; }

		/// <summary>
		/// Initialize section attribute
		/// </summary>
		/// <param name="name">Section name, valid characters: alphabet, numeric, underscore, space.</param>
		public IniSectionAttribute(string name)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
			Name = name;
		}
	}
}
