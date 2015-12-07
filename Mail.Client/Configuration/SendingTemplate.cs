using Mail.Library.Configuration;

namespace Mail.Client.Configuration
{
    [IniSection("outgoing template", Comment = "Can use 'Display Name <email@domain.com>' for full format.")]
    internal class SendingTemplate
    {
        public string From { get; set; }

        [IniKey("to")]
        public string To { get; set; }

        [IniKey("cc")]
        public string Cc { get; set; }

        [IniKey("bcc")]
        public string Bcc { get; set; }

        public string Subject { get; set; }

        [IniKey("message", Comment = "Can use full HTML format if necessary")]
        public string Body { get; set; }
    }
}
