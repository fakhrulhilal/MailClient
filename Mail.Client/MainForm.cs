using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Mail.Client.Configuration;
using Mail.Library;
using Mail.Library.Configuration;
using MailMessage = Mail.Library.Message;

namespace Mail.Client
{
	public partial class MainForm : Form
	{
		private readonly IMailSender mailSender;
		private readonly IMailReader mailReader;
	    private readonly IniConfiguration configuration;

	    private string MailFrom
	    {
	        get { return txtAddress.Text; }
	        set { txtAddress.Text = value; }
	    }

	    private string To
	    {
	        get { return txtTo.Text; }
	        set { txtTo.Text = value; }
	    }

	    private string Cc
	    {
	        get { return txtCarbonCopy.Text; }
	        set { txtCarbonCopy.Text = value; }
	    }

	    private string Bcc
	    {
	        get { return txtBlindCarbonCopy.Text; }
	        set { txtBlindCarbonCopy.Text = value; }
	    }

	    private string Subject
	    {
	        get { return txtSubject.Text; }
	        set { txtSubject.Text = value; }
	    }

	    private string Message
	    {
	        get { return txtMessage.Text; }
	        set { txtMessage.Text = value; }
	    }

	    private string Username
		{
		    get { return txtUsername.Text; }
		    set { txtUsername.Text = value; }
		}

	    private string Password
	    {
	        get { return txtPassword.Text; }
	        set { txtPassword.Text = value; }
	    }

	    private string Server
	    {
	        get { return txtServer.Text; }
	        set { txtServer.Text = value; }
	    }

	    private int Port
		{
			get { return Convert.ToInt32(nudPort.Value); }
			set { nudPort.Value = value; }
		}

	    private bool UseSecureConnection
	    {
            get { return chkSecure.Checked; }
            set { chkSecure.Checked = value; }
	    }

		private void btnSend_Click(object sender, EventArgs e)
		{
			var connection = BuildSendConnection();
			var connectionValidation = connection.Validate();
			var mail = BuildMessage();
			var mailValidation = mail.Validate();
			var messages = new List<string>();
			if (!connectionValidation.IsValid) messages.AddRange(connectionValidation.Messages);
			if (!mailValidation.IsValid) messages.AddRange(mailValidation.Messages);
			if (messages.Any())
			{
				MessageBox.Show("There're errors:\n- " + string.Join("\n- ", messages), "Errors!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Cursor = Cursors.WaitCursor;
			string errorMessage;
			bool success = mailSender.Send(connection, mail, out errorMessage);
			if (success)
				MessageBox.Show("Messsage sent to " + string.Join(",", mail.To.Select(destination => destination.Email)), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
				MessageBox.Show($"Failed sending message because of {errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Cursor = DefaultCursor;
		}

		public MainForm(IMailSender mailSender, IMailReader mailReader, IniConfiguration configuration)
		{
		    this.configuration = configuration;
		    this.mailSender = mailSender;
			this.mailReader = mailReader;
			InitializeComponent();
			listAttachment.ItemSelectionChanged += listAttachment_SelectedIndexChanged;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			listAttachment.Columns.Add(new ColumnHeader
			{
				Name = "Path",
				Width = listAttachment.Width - 4
			});
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
		    var sendingConfig = configuration.GetConfig<SendingConnection>();
		    Port = sendingConfig.Port;
		    UseSecureConnection = sendingConfig.UseSecureConnection;
		    Username = sendingConfig.LogonUsername;
		    Password = sendingConfig.LogonPassword;
		    Server = sendingConfig.ServerAddress;
		    var sendingTemplateConfig = configuration.GetConfig<SendingTemplate>();
		    MailFrom = sendingTemplateConfig.From;
		    To = sendingTemplateConfig.To;
		    Cc = sendingTemplateConfig.Cc;
		    Bcc = sendingTemplateConfig.Bcc;
		    Subject = sendingTemplateConfig.Subject;
		    Message = sendingTemplateConfig.Body;
		}

		private void btnAddAttachment_Click(object sender, EventArgs e)
		{
			using (var dialog = new OpenFileDialog())
			{
				dialog.Filter = "Any | *.*";
				dialog.Multiselect = true;
				dialog.CheckFileExists = true;
				if (dialog.ShowDialog() != DialogResult.OK ||
					dialog.FileNames == null ||
					dialog.FileNames.Length < 1)
					return;
				Array.ForEach(
					dialog.FileNames.Where(file => !listAttachment.Items.ContainsKey(file)).ToArray(),
					file => listAttachment.Items.Add(new ListViewItem
					{
						Name = file,
						Text = System.IO.Path.GetFileName(file),
						ToolTipText = file
					}));
			}
		}

		private void listAttachment_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listAttachment.SelectedItems.Count > 0)
				btnDeleteAttachment.Enabled = true;
		}

		private void btnDeleteAttachment_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listAttachment.Items)
			{
				if (item.Selected)
					listAttachment.Items.Remove(item);
			}

			btnDeleteAttachment.Enabled = false;
		}

		private MailMessage BuildMessage()
		{
			var mail = new MailMessage
			{
				UseHtml = true,
				Body = Message,
				Subject = Subject,
				Sender = new Address(MailFrom),
				To = new AddressCollection(To)
			};
			if (Cc != null) mail.Cc = new AddressCollection(Cc);
			if (Bcc != null) mail.Bcc = new AddressCollection(Bcc);
			foreach (ListViewItem item in listAttachment.Items)
			{
				mail.Attachments.Add(new Attachment { Path = item.Name });
			}

			return mail;
		}

		private SendConnection BuildSendConnection()
		{
			return new SendConnection(Server, Port)
			{
				Username = Username,
				Password = Password,
				Security = !UseSecureConnection ? SecureType.None : SecureType.Default
			};
		}

        private void btnSave_Click(object sender, EventArgs e)
        {
            var connectionConfig = new SendingConnection
            {
                ServerAddress = Server,
                LogonUsername = Username,
                LogonPassword = Password,
                Port = Port,
                UseSecureConnection = UseSecureConnection
            };
            configuration.SetConfig(connectionConfig);
            var templateConfig = new SendingTemplate
            {
                Subject = Subject,
                From = MailFrom,
                To = To,
                Cc = Cc,
                Bcc = Bcc
            };
            configuration.SetConfig(templateConfig);
            try
            {
                configuration.Write();
                MessageBox.Show($"Successfully saved configuration to '{configuration.Path}'", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show($"Can't save configuration to '{configuration.Path}'", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
