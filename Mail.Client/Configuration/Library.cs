using Mail.Library.Configuration;

namespace Mail.Client.Configuration
{
	[IniSection("library")]
	internal class Library
	{
		[IniKey("sender", "BuiltIn", Comment = "The file Mail.Plugin.[name].dll must exist within this directory")]
		public string Sender { get; set; }

		[IniKey("reader", Comment = "The file Mail.Plugin.[name].dll must exist within this directory")]
		public string Reader { get; set; }

		[IniKey("exporter", Comment = "The file Mail.Plugin.[name].dll must exist within this directory")]
		public string Exporter { get; set; }
	}
}
