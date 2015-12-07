using Mail.Library.Configuration;

namespace Mail.Client.Configuration
{
	[IniSection("outgoing connection")]
	internal class SendingConnection
	{
        [IniKey("port", 25)]
		public int Port { get; set; }

        [IniKey("secure", Comment = "Determine whether to use SSL/TLS connection or not (true or false)")]
	    public bool UseSecureConnection { get; set; }

        [IniKey("server")]
	    public string ServerAddress { get; set; }

        [IniKey("username")]
	    public string LogonUsername { get; set; }

        [IniKey("password")]
	    public string LogonPassword { get; set; }
    }
}
