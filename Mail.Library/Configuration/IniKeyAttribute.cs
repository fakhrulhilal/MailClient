using System;

namespace Mail.Library.Configuration
{
	/// <summary>
	/// Config key attribute
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class IniKeyAttribute : Attribute
	{
		/// <summary>
		/// Config key name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Default value for this key-value pair
		/// </summary>
		public object DefaultValue { get; set; }

        /// <summary>
        /// Additional comment
        /// </summary>
	    public string Comment { get; set; }

		internal bool HasDefaultValue { get; private set; }

		/// <summary>
		/// Initialize key attribute
		/// </summary>
		/// <param name="name">Config key name</param>
		public IniKeyAttribute(string name)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
			Name = name;
			HasDefaultValue = false;
		}

		/// <summary>
		/// Initialize key attribute
		/// </summary>
		/// <param name="name">Config key name</param>
		/// <param name="defaultValue">Default value for this key-value pair</param>
		public IniKeyAttribute(string name, object defaultValue) : this(name)
		{
			DefaultValue = defaultValue;
			HasDefaultValue = true;
		}
	}
}
