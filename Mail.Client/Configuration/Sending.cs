using Mail.Library.Configuration;

namespace Mail.Client.Configuration
{
	[IniSection("outgoing")]
	internal class Sending
	{
		public int Port { get; set; }

		[IniKey("secure", SecurityType.None)]
		public SecurityType Security { get; set; }
	}
}
