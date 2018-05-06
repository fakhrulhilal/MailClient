using System.ComponentModel;
using System.Windows.Forms;

namespace Mail.Client
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDeleteAttachment = new System.Windows.Forms.Button();
            this.btnAddAttachment = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.listAttachment = new System.Windows.Forms.ListView();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBlindCarbonCopy = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtCarbonCopy = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSecure = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnDeleteAttachment);
            this.groupBox1.Controls.Add(this.btnAddAttachment);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.listAttachment);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtBlindCarbonCopy);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtCarbonCopy);
            this.groupBox1.Controls.Add(this.txtMessage);
            this.groupBox1.Controls.Add(this.btnSend);
            this.groupBox1.Controls.Add(this.txtSubject);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtTo);
            this.groupBox1.Location = new System.Drawing.Point(9, 13);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(330, 344);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Email Message";
            // 
            // btnDeleteAttachment
            // 
            this.btnDeleteAttachment.Enabled = false;
            this.btnDeleteAttachment.Location = new System.Drawing.Point(40, 135);
            this.btnDeleteAttachment.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDeleteAttachment.Name = "btnDeleteAttachment";
            this.btnDeleteAttachment.Size = new System.Drawing.Size(22, 19);
            this.btnDeleteAttachment.TabIndex = 36;
            this.btnDeleteAttachment.Text = "-";
            this.btnDeleteAttachment.UseVisualStyleBackColor = true;
            this.btnDeleteAttachment.Click += new System.EventHandler(this.btnDeleteAttachment_Click);
            // 
            // btnAddAttachment
            // 
            this.btnAddAttachment.Location = new System.Drawing.Point(14, 135);
            this.btnAddAttachment.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAddAttachment.Name = "btnAddAttachment";
            this.btnAddAttachment.Size = new System.Drawing.Size(22, 19);
            this.btnAddAttachment.TabIndex = 4;
            this.btnAddAttachment.Text = "+";
            this.btnAddAttachment.UseVisualStyleBackColor = true;
            this.btnAddAttachment.Click += new System.EventHandler(this.btnAddAttachment_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 118);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(66, 13);
            this.label12.TabIndex = 34;
            this.label12.Text = "Attachments";
            // 
            // listAttachment
            // 
            this.listAttachment.AllowDrop = true;
            this.listAttachment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listAttachment.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listAttachment.HideSelection = false;
            this.listAttachment.Location = new System.Drawing.Point(106, 118);
            this.listAttachment.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listAttachment.Name = "listAttachment";
            this.listAttachment.Size = new System.Drawing.Size(207, 94);
            this.listAttachment.TabIndex = 33;
            this.listAttachment.UseCompatibleStateImageBehavior = false;
            this.listAttachment.View = System.Windows.Forms.View.Details;
            this.listAttachment.SelectedIndexChanged += new System.EventHandler(this.listAttachment_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 74);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(28, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "BCC";
            // 
            // txtBlindCarbonCopy
            // 
            this.txtBlindCarbonCopy.Location = new System.Drawing.Point(106, 70);
            this.txtBlindCarbonCopy.Name = "txtBlindCarbonCopy";
            this.txtBlindCarbonCopy.Size = new System.Drawing.Size(207, 20);
            this.txtBlindCarbonCopy.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(21, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "CC";
            // 
            // txtCarbonCopy
            // 
            this.txtCarbonCopy.Location = new System.Drawing.Point(106, 45);
            this.txtCarbonCopy.Name = "txtCarbonCopy";
            this.txtCarbonCopy.Size = new System.Drawing.Size(207, 20);
            this.txtCarbonCopy.TabIndex = 1;
            // 
            // txtMessage
            // 
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMessage.EnableAutoDragDrop = true;
            this.txtMessage.Location = new System.Drawing.Point(106, 216);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(207, 86);
            this.txtMessage.TabIndex = 5;
            this.txtMessage.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(237, 307);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "&Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(106, 94);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(207, 20);
            this.txtSubject.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 216);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Message";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Subject";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "To";
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(106, 17);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(207, 20);
            this.txtTo.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkSecure);
            this.groupBox2.Controls.Add(this.txtAddress);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.nudPort);
            this.groupBox2.Controls.Add(this.txtServer);
            this.groupBox2.Controls.Add(this.txtPassword);
            this.groupBox2.Controls.Add(this.txtUsername);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(350, 13);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(289, 185);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Connection";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(91, 21);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(189, 20);
            this.txtAddress.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Mail From";
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(91, 120);
            this.nudPort.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(121, 20);
            this.nudPort.TabIndex = 4;
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(91, 95);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(189, 20);
            this.txtServer.TabIndex = 3;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(91, 70);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(189, 20);
            this.txtPassword.TabIndex = 2;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(91, 44);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(189, 20);
            this.txtUsername.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "SSL/TLS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Username";
            // 
            // chkSecure
            // 
            this.chkSecure.AutoSize = true;
            this.chkSecure.Location = new System.Drawing.Point(91, 150);
            this.chkSecure.Name = "chkSecure";
            this.chkSecure.Size = new System.Drawing.Size(44, 17);
            this.chkSecure.TabIndex = 5;
            this.chkSecure.Text = "Yes";
            this.chkSecure.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(156, 307);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "S&ave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 369);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Tiny Mail Client";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
		private GroupBox groupBox1;
		private Label label11;
		private TextBox txtBlindCarbonCopy;
		private Label label10;
		private TextBox txtCarbonCopy;
		private RichTextBox txtMessage;
		private Button btnSend;
		private TextBox txtSubject;
		private Label label8;
		private Label label7;
		private Label label6;
		private TextBox txtTo;
		private Button btnDeleteAttachment;
		private Button btnAddAttachment;
		private Label label12;
		private ListView listAttachment;
		private GroupBox groupBox2;
		private TextBox txtAddress;
		private Label label9;
		private NumericUpDown nudPort;
		private TextBox txtServer;
		private TextBox txtPassword;
		private TextBox txtUsername;
		private Label label5;
		private Label label4;
		private Label label3;
		private Label label2;
		private Label label1;
        private CheckBox chkSecure;
        private Button btnSave;
    }
}

