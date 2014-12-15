using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Message = Dashinginfo.Practices.Mail.Pop3.Mime.Message;
using Dashinginfo.Practices.Mail.Pop3;
using Dashinginfo.Practices.Mail.Pop3.Common.Logging;
using Dashinginfo.Practices.Mail.Pop3.Exceptions;
using Dashinginfo.Practices.Mail.Pop3.Mime;
using Dashinginfo.Practices.Mail.Pop3.Mime.Header;
using Dashinginfo.Practices.Mail;

namespace MailTest.TestApplication
{
    /// <summary>
    /// This class is a form which makes it possible to download all messages
    /// from a pop3 mailbox in a simply way.
    /// </summary>
    public class TestForm : Form
    {
        private readonly Dictionary<int, Message> messages = new Dictionary<int, Message>();
        private readonly IMailReceive pop3Client;
        private Button connectAndRetrieveButton;
        private Button uidlButton;
        private Panel attachmentPanel;
        private ContextMenu contextMenuMessages;
        private DataGrid gridHeaders;
        private Label labelServerAddress;
        private Label labelServerPort;
        private Label labelAttachments;
        private Label labelMessageBody;
        private Label labelMessageNumber;
        private Label labelTotalMessages;
        private Label labelPassword;
        private Label labelUsername;
        private TreeView listAttachments;
        private TreeView listMessages;
        private MenuItem menuDeleteMessage;
        private MenuItem menuViewSource;
        private Panel panelTop;
        private Panel panelProperties;
        private Panel panelMiddle;
        private Panel panelMessageBody;
        private Panel panelMessagesView;
        private SaveFileDialog saveFile;
        private TextBox loginTextBox;
        private TextBox messageTextBox;
        private TextBox popServerTextBox;
        private TextBox passwordTextBox;
        private TextBox portTextBox;
        private TextBox totalMessagesTextBox;
        private ProgressBar progressBar;
        private Button btnSend;
        private Button btnSendMail;
        private CheckBox useSslCheckBox;

        private TestForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            
            //
            // User defined stuff here 
            //
            //MailConfiguration m = new MailConfiguration();
            // This is how you would override the default logger type
            // Here we want to log to a file
            DefaultLogger.SetLog(new FileLogger());

            // Enable file logging and include verbose information
            FileLogger.Enabled = true;
            FileLogger.Verbose = true;
           // MailConfiguration mailConf = new MailConfiguration();
            //pop3Client = MailReceiveFactory.GetMailReceiver(mailConf.ServerConfiguration[0].ReceiveConfiguration.ReceiveAssembly, mailConf.ServerConfiguration[0].ReceiveConfiguration.ReceiveType);

            // This is only for faster debugging purposes
            // We will try to load in default values for the hostname, port, ssl, username and password from a file
            string myDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string file = Path.Combine(myDocs, "MailLogin.txt");
            if (File.Exists(file))
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(file)))
                {
                    // This describes how the Dashinginfo.Practices.MailLogin.txt file should look like
                    popServerTextBox.Text = reader.ReadLine(); // Hostname
                    portTextBox.Text = reader.ReadLine(); // Port
                    useSslCheckBox.Checked = bool.Parse(reader.ReadLine() ?? "true"); // Whether to use SSL or not
                    loginTextBox.Text = reader.ReadLine(); // Username
                    passwordTextBox.Text = reader.ReadLine(); // Password
                }
            }
        }

        #region Windows Form Designer generated code
        /// <summary>
        ///   Required method for Designer support - do not modify
        ///   the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnSendMail = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.useSslCheckBox = new System.Windows.Forms.CheckBox();
            this.uidlButton = new System.Windows.Forms.Button();
            this.totalMessagesTextBox = new System.Windows.Forms.TextBox();
            this.labelTotalMessages = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.loginTextBox = new System.Windows.Forms.TextBox();
            this.connectAndRetrieveButton = new System.Windows.Forms.Button();
            this.labelServerPort = new System.Windows.Forms.Label();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.labelServerAddress = new System.Windows.Forms.Label();
            this.popServerTextBox = new System.Windows.Forms.TextBox();
            this.panelProperties = new System.Windows.Forms.Panel();
            this.gridHeaders = new System.Windows.Forms.DataGrid();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.panelMessageBody = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.labelMessageBody = new System.Windows.Forms.Label();
            this.panelMessagesView = new System.Windows.Forms.Panel();
            this.listMessages = new System.Windows.Forms.TreeView();
            this.contextMenuMessages = new System.Windows.Forms.ContextMenu();
            this.menuDeleteMessage = new System.Windows.Forms.MenuItem();
            this.menuViewSource = new System.Windows.Forms.MenuItem();
            this.labelMessageNumber = new System.Windows.Forms.Label();
            this.attachmentPanel = new System.Windows.Forms.Panel();
            this.listAttachments = new System.Windows.Forms.TreeView();
            this.labelAttachments = new System.Windows.Forms.Label();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.panelTop.SuspendLayout();
            this.panelProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridHeaders)).BeginInit();
            this.panelMiddle.SuspendLayout();
            this.panelMessageBody.SuspendLayout();
            this.panelMessagesView.SuspendLayout();
            this.attachmentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnSendMail);
            this.panelTop.Controls.Add(this.btnSend);
            this.panelTop.Controls.Add(this.useSslCheckBox);
            this.panelTop.Controls.Add(this.uidlButton);
            this.panelTop.Controls.Add(this.totalMessagesTextBox);
            this.panelTop.Controls.Add(this.labelTotalMessages);
            this.panelTop.Controls.Add(this.labelPassword);
            this.panelTop.Controls.Add(this.passwordTextBox);
            this.panelTop.Controls.Add(this.labelUsername);
            this.panelTop.Controls.Add(this.loginTextBox);
            this.panelTop.Controls.Add(this.connectAndRetrieveButton);
            this.panelTop.Controls.Add(this.labelServerPort);
            this.panelTop.Controls.Add(this.portTextBox);
            this.panelTop.Controls.Add(this.labelServerAddress);
            this.panelTop.Controls.Add(this.popServerTextBox);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(865, 69);
            this.panelTop.TabIndex = 0;
            // 
            // btnSendMail
            // 
            this.btnSendMail.Location = new System.Drawing.Point(911, 23);
            this.btnSendMail.Name = "btnSendMail";
            this.btnSendMail.Size = new System.Drawing.Size(90, 24);
            this.btnSendMail.TabIndex = 11;
            this.btnSendMail.Text = "SmptSendMail";
            this.btnSendMail.UseVisualStyleBackColor = true;
            this.btnSendMail.Click += new System.EventHandler(this.btnSendMail_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(806, 0);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(90, 67);
            this.btnSend.TabIndex = 10;
            this.btnSend.Text = "SendMail";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // useSslCheckBox
            // 
            this.useSslCheckBox.AutoSize = true;
            this.useSslCheckBox.Checked = true;
            this.useSslCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useSslCheckBox.Location = new System.Drawing.Point(23, 41);
            this.useSslCheckBox.Name = "useSslCheckBox";
            this.useSslCheckBox.Size = new System.Drawing.Size(66, 16);
            this.useSslCheckBox.TabIndex = 4;
            this.useSslCheckBox.Text = "Use SSL";
            this.useSslCheckBox.UseVisualStyleBackColor = true;
            // 
            // uidlButton
            // 
            this.uidlButton.Enabled = false;
            this.uidlButton.Location = new System.Drawing.Point(552, 45);
            this.uidlButton.Name = "uidlButton";
            this.uidlButton.Size = new System.Drawing.Size(98, 23);
            this.uidlButton.TabIndex = 6;
            this.uidlButton.Text = "UIDL";
            this.uidlButton.Click += new System.EventHandler(this.UidlButtonClick);
            // 
            // totalMessagesTextBox
            // 
            this.totalMessagesTextBox.Location = new System.Drawing.Point(664, 32);
            this.totalMessagesTextBox.Name = "totalMessagesTextBox";
            this.totalMessagesTextBox.Size = new System.Drawing.Size(120, 21);
            this.totalMessagesTextBox.TabIndex = 7;
            // 
            // labelTotalMessages
            // 
            this.labelTotalMessages.Location = new System.Drawing.Point(664, 8);
            this.labelTotalMessages.Name = "labelTotalMessages";
            this.labelTotalMessages.Size = new System.Drawing.Size(120, 24);
            this.labelTotalMessages.TabIndex = 9;
            this.labelTotalMessages.Text = "Total Messages";
            // 
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(317, 39);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(77, 25);
            this.labelPassword.TabIndex = 8;
            this.labelPassword.Text = "Password";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(394, 39);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(153, 21);
            this.passwordTextBox.TabIndex = 2;
            // 
            // labelUsername
            // 
            this.labelUsername.Location = new System.Drawing.Point(317, 5);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(77, 25);
            this.labelUsername.TabIndex = 6;
            this.labelUsername.Text = "Username";
            // 
            // loginTextBox
            // 
            this.loginTextBox.Location = new System.Drawing.Point(394, 5);
            this.loginTextBox.Name = "loginTextBox";
            this.loginTextBox.Size = new System.Drawing.Size(153, 21);
            this.loginTextBox.TabIndex = 1;
            // 
            // connectAndRetrieveButton
            // 
            this.connectAndRetrieveButton.Location = new System.Drawing.Point(552, 0);
            this.connectAndRetrieveButton.Name = "connectAndRetrieveButton";
            this.connectAndRetrieveButton.Size = new System.Drawing.Size(98, 42);
            this.connectAndRetrieveButton.TabIndex = 5;
            this.connectAndRetrieveButton.Text = "Connect and Retreive";
            this.connectAndRetrieveButton.Click += new System.EventHandler(this.ConnectAndRetrieveButtonClick);
            // 
            // labelServerPort
            // 
            this.labelServerPort.Location = new System.Drawing.Point(116, 42);
            this.labelServerPort.Name = "labelServerPort";
            this.labelServerPort.Size = new System.Drawing.Size(38, 25);
            this.labelServerPort.TabIndex = 3;
            this.labelServerPort.Text = "Port";
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(154, 42);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(153, 21);
            this.portTextBox.TabIndex = 3;
            this.portTextBox.Text = "110";
            // 
            // labelServerAddress
            // 
            this.labelServerAddress.Location = new System.Drawing.Point(19, 9);
            this.labelServerAddress.Name = "labelServerAddress";
            this.labelServerAddress.Size = new System.Drawing.Size(135, 24);
            this.labelServerAddress.TabIndex = 1;
            this.labelServerAddress.Text = "POP Server Address";
            // 
            // popServerTextBox
            // 
            this.popServerTextBox.Location = new System.Drawing.Point(154, 9);
            this.popServerTextBox.Name = "popServerTextBox";
            this.popServerTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.popServerTextBox.Size = new System.Drawing.Size(153, 21);
            this.popServerTextBox.TabIndex = 0;
            // 
            // panelProperties
            // 
            this.panelProperties.Controls.Add(this.gridHeaders);
            this.panelProperties.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelProperties.Location = new System.Drawing.Point(0, 246);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Size = new System.Drawing.Size(865, 198);
            this.panelProperties.TabIndex = 1;
            // 
            // gridHeaders
            // 
            this.gridHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridHeaders.DataMember = "";
            this.gridHeaders.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.gridHeaders.Location = new System.Drawing.Point(0, 0);
            this.gridHeaders.Name = "gridHeaders";
            this.gridHeaders.PreferredColumnWidth = 400;
            this.gridHeaders.ReadOnly = true;
            this.gridHeaders.Size = new System.Drawing.Size(865, 202);
            this.gridHeaders.TabIndex = 3;
            // 
            // panelMiddle
            // 
            this.panelMiddle.Controls.Add(this.panelMessageBody);
            this.panelMiddle.Controls.Add(this.panelMessagesView);
            this.panelMiddle.Controls.Add(this.attachmentPanel);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(0, 69);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(865, 177);
            this.panelMiddle.TabIndex = 2;
            // 
            // panelMessageBody
            // 
            this.panelMessageBody.Controls.Add(this.progressBar);
            this.panelMessageBody.Controls.Add(this.messageTextBox);
            this.panelMessageBody.Controls.Add(this.labelMessageBody);
            this.panelMessageBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMessageBody.Location = new System.Drawing.Point(337, 0);
            this.panelMessageBody.Name = "panelMessageBody";
            this.panelMessageBody.Size = new System.Drawing.Size(277, 177);
            this.panelMessageBody.TabIndex = 6;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(8, 151);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(258, 13);
            this.progressBar.TabIndex = 10;
            // 
            // messageTextBox
            // 
            this.messageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.messageTextBox.Location = new System.Drawing.Point(8, 24);
            this.messageTextBox.MaxLength = 999999999;
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.messageTextBox.Size = new System.Drawing.Size(258, 121);
            this.messageTextBox.TabIndex = 9;
            // 
            // labelMessageBody
            // 
            this.labelMessageBody.Location = new System.Drawing.Point(10, 9);
            this.labelMessageBody.Name = "labelMessageBody";
            this.labelMessageBody.Size = new System.Drawing.Size(163, 17);
            this.labelMessageBody.TabIndex = 5;
            this.labelMessageBody.Text = "Message Body";
            // 
            // panelMessagesView
            // 
            this.panelMessagesView.Controls.Add(this.listMessages);
            this.panelMessagesView.Controls.Add(this.labelMessageNumber);
            this.panelMessagesView.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMessagesView.Location = new System.Drawing.Point(0, 0);
            this.panelMessagesView.Name = "panelMessagesView";
            this.panelMessagesView.Size = new System.Drawing.Size(337, 177);
            this.panelMessagesView.TabIndex = 5;
            // 
            // listMessages
            // 
            this.listMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listMessages.ContextMenu = this.contextMenuMessages;
            this.listMessages.Location = new System.Drawing.Point(10, 26);
            this.listMessages.Name = "listMessages";
            this.listMessages.Size = new System.Drawing.Size(319, 138);
            this.listMessages.TabIndex = 8;
            this.listMessages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ListMessagesMessageSelected);
            // 
            // contextMenuMessages
            // 
            this.contextMenuMessages.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuDeleteMessage,
            this.menuViewSource});
            // 
            // menuDeleteMessage
            // 
            this.menuDeleteMessage.Index = 0;
            this.menuDeleteMessage.Text = "Delete Mail";
            this.menuDeleteMessage.Click += new System.EventHandler(this.MenuDeleteMessageClick);
            // 
            // menuViewSource
            // 
            this.menuViewSource.Index = 1;
            this.menuViewSource.Text = "View source";
            this.menuViewSource.Click += new System.EventHandler(this.MenuViewSourceClick);
            // 
            // labelMessageNumber
            // 
            this.labelMessageNumber.Location = new System.Drawing.Point(10, 9);
            this.labelMessageNumber.Name = "labelMessageNumber";
            this.labelMessageNumber.Size = new System.Drawing.Size(163, 17);
            this.labelMessageNumber.TabIndex = 1;
            this.labelMessageNumber.Text = "Messages";
            // 
            // attachmentPanel
            // 
            this.attachmentPanel.Controls.Add(this.listAttachments);
            this.attachmentPanel.Controls.Add(this.labelAttachments);
            this.attachmentPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.attachmentPanel.Location = new System.Drawing.Point(614, 0);
            this.attachmentPanel.Name = "attachmentPanel";
            this.attachmentPanel.Size = new System.Drawing.Size(251, 177);
            this.attachmentPanel.TabIndex = 4;
            this.attachmentPanel.Visible = false;
            // 
            // listAttachments
            // 
            this.listAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listAttachments.Location = new System.Drawing.Point(10, 26);
            this.listAttachments.Name = "listAttachments";
            this.listAttachments.ShowLines = false;
            this.listAttachments.ShowRootLines = false;
            this.listAttachments.Size = new System.Drawing.Size(230, 138);
            this.listAttachments.TabIndex = 10;
            this.listAttachments.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ListAttachmentsAttachmentSelected);
            // 
            // labelAttachments
            // 
            this.labelAttachments.Location = new System.Drawing.Point(14, 9);
            this.labelAttachments.Name = "labelAttachments";
            this.labelAttachments.Size = new System.Drawing.Size(164, 17);
            this.labelAttachments.TabIndex = 3;
            this.labelAttachments.Text = "Attachments";
            // 
            // saveFile
            // 
            this.saveFile.Title = "Save Attachment";
            // 
            // TestForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(865, 444);
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.panelProperties);
            this.Controls.Add(this.panelTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashinginfo.Practices.Mail.NET Test Application";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridHeaders)).EndInit();
            this.panelMiddle.ResumeLayout(false);
            this.panelMessageBody.ResumeLayout(false);
            this.panelMessageBody.PerformLayout();
            this.panelMessagesView.ResumeLayout(false);
            this.attachmentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        ///   The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.Run(new TestForm());
        }

        private void ReceiveMails()
        {
            // Disable buttons while working
            connectAndRetrieveButton.Enabled = false;
            uidlButton.Enabled = false;
            progressBar.Value = 0;

            try
            {
                if (pop3Client.Connected)
                    pop3Client.Disconnect();
                pop3Client.Connect(popServerTextBox.Text, int.Parse(portTextBox.Text), useSslCheckBox.Checked);
                pop3Client.Authenticate(loginTextBox.Text, passwordTextBox.Text);
                int count = pop3Client.GetMessageCount();
                totalMessagesTextBox.Text = count.ToString();
                messageTextBox.Text = "";
                messages.Clear();
                listMessages.Nodes.Clear();
                listAttachments.Nodes.Clear();

                int success = 0;
                int fail = 0;
                for (int i = count; i >= 1; i -= 1)
                {
                    // Check if the form is closed while we are working. If so, abort
                    if (IsDisposed)
                        return;

                    // Refresh the form while fetching emails
                    // This will fix the "Application is not responding" problem
                    Application.DoEvents();

                    try
                    {
                        Message message = pop3Client.GetMessage(i);

                        // Add the message to the dictionary from the messageNumber to the Message
                        messages.Add(i, message);

                        // Create a TreeNode tree that mimics the Message hierarchy
                        TreeNode node = new TreeNodeBuilder().VisitMessage(message);

                        // Set the Tag property to the messageNumber
                        // We can use this to find the Message again later
                        node.Tag = i;

                        // Show the built node in our list of messages
                        listMessages.Nodes.Add(node);

                        success++;
                    }
                    catch (Exception e)
                    {
                        DefaultLogger.Log.LogError(
                            "TestForm: Message fetching failed: " + e.Message + "\r\n" +
                            "Stack trace:\r\n" +
                            e.StackTrace);
                        fail++;
                    }

                    progressBar.Value = (int)(((double)(count - i) / count) * 100);
                }

                MessageBox.Show(this, "Mail received!\nSuccesses: " + success + "\nFailed: " + fail, "Message fetching done");

                if (fail > 0)
                {
                    MessageBox.Show(this,
                                    "Since some of the emails were not parsed correctly (exceptions were thrown)\r\n" +
                                    "please consider sending your log file to the developer for fixing.\r\n" +
                                    "If you are able to include any extra information, please do so.",
                                    "Help improve Dashinginfo.Practices.Mail!");
                }
            }
            catch (InvalidLoginException)
            {
                MessageBox.Show(this, "The server did not accept the user credentials!", "POP3 Server Authentication");
            }
            catch (PopServerNotFoundException)
            {
                MessageBox.Show(this, "The server could not be found", "POP3 Retrieval");
            }
            catch (PopServerLockedException)
            {
                MessageBox.Show(this, "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", "POP3 Account Locked");
            }
            catch (LoginDelayException)
            {
                MessageBox.Show(this, "Login not allowed. Server enforces delay between logins. Have you connected recently?", "POP3 Account Login Delay");
            }
            catch (Exception e)
            {
                MessageBox.Show(this, "Error occurred retrieving mail. " + e.Message, "POP3 Retrieval");
            }
            finally
            {
                // Enable the buttons again
                connectAndRetrieveButton.Enabled = true;
                uidlButton.Enabled = true;
                progressBar.Value = 100;
            }
        }

        private void ConnectAndRetrieveButtonClick(object sender, EventArgs e)
        {
            ReceiveMails();
        }

        private void ListMessagesMessageSelected(object sender, TreeViewEventArgs e)
        {
            // Fetch out the selected message
            Message message = messages[GetMessageNumberFromSelectedNode(listMessages.SelectedNode)];

            // If the selected node contains a MessagePart and we can display the contents - display them
            if (listMessages.SelectedNode.Tag is MessagePart)
            {
                MessagePart selectedMessagePart = (MessagePart)listMessages.SelectedNode.Tag;
                if (selectedMessagePart.IsText)
                {
                    // We can show text MessageParts
                    messageTextBox.Text = selectedMessagePart.GetBodyAsText();
                }
                else
                {
                    // We are not able to show non-text MessageParts (MultiPart messages, images, pdf's ...)
                    messageTextBox.Text = "<<Dashinginfo.Practices.Mail>> Cannot show this part of the email. It is not text <<Dashinginfo.Practices.Mail>>";
                }
            }
            else
            {
                // If the selected node is not a subnode and therefore does not
                // have a MessagePart in it's Tag property, we genericly find some content to show

                // Find the first text/plain version
                MessagePart plainTextPart = message.FindFirstPlainTextVersion();
                if (plainTextPart != null)
                {
                    // The message had a text/plain version - show that one
                    messageTextBox.Text = plainTextPart.GetBodyAsText();
                }
                else
                {
                    // Try to find a body to show in some of the other text versions
                    List<MessagePart> textVersions = message.FindAllTextVersions();
                    if (textVersions.Count >= 1)
                        messageTextBox.Text = textVersions[0].GetBodyAsText();
                    else
                        messageTextBox.Text = "<<Dashinginfo.Practices.Mail>> Cannot find a text version body in this message to show <<Dashinginfo.Practices.Mail>>";
                }
            }

            // Clear the attachment list from any previus shown attachments
            listAttachments.Nodes.Clear();

            // Build up the attachment list
            List<MessagePart> attachments = message.FindAllAttachments();
            foreach (MessagePart attachment in attachments)
            {
                // Add the attachment to the list of attachments
                TreeNode addedNode = listAttachments.Nodes.Add((attachment.FileName));

                // Keep a reference to the attachment in the Tag property
                addedNode.Tag = attachment;
            }

            // Only show that attachmentPanel if there is attachments in the message
            bool hadAttachments = attachments.Count > 0;
            attachmentPanel.Visible = hadAttachments;

            // Generate header table
            DataSet dataSet = new DataSet();
            DataTable table = dataSet.Tables.Add("Headers");
            table.Columns.Add("Header");
            table.Columns.Add("Value");

            DataRowCollection rows = table.Rows;

            // Add all known headers
            rows.Add(new object[] { "Content-Description", message.Headers.ContentDescription });
            rows.Add(new object[] { "Content-Id", message.Headers.ContentId });
            foreach (string keyword in message.Headers.Keywords) rows.Add(new object[] { "Keyword", keyword });
            foreach (RfcMailAddress dispositionNotificationTo in message.Headers.DispositionNotificationTo) rows.Add(new object[] { "Disposition-Notification-To", dispositionNotificationTo });
            foreach (Received received in message.Headers.Received) rows.Add(new object[] { "Received", received.Raw });
            rows.Add(new object[] { "Importance", message.Headers.Importance });
            rows.Add(new object[] { "Content-Transfer-Encoding", message.Headers.ContentTransferEncoding });
            foreach (RfcMailAddress cc in message.Headers.Cc) rows.Add(new object[] { "Cc", cc });
            foreach (RfcMailAddress bcc in message.Headers.Bcc) rows.Add(new object[] { "Bcc", bcc });
            foreach (RfcMailAddress to in message.Headers.To) rows.Add(new object[] { "To", to });
            rows.Add(new object[] { "From", message.Headers.From });
            rows.Add(new object[] { "Reply-To", message.Headers.ReplyTo });
            foreach (string inReplyTo in message.Headers.InReplyTo) rows.Add(new object[] { "In-Reply-To", inReplyTo });
            foreach (string reference in message.Headers.References) rows.Add(new object[] { "References", reference });
            rows.Add(new object[] { "Sender", message.Headers.Sender });
            rows.Add(new object[] { "Content-Type", message.Headers.ContentType });
            rows.Add(new object[] { "Content-Disposition", message.Headers.ContentDisposition });
            rows.Add(new object[] { "Date", message.Headers.Date });
            rows.Add(new object[] { "Date", message.Headers.DateSent });
            rows.Add(new object[] { "Message-Id", message.Headers.MessageId });
            rows.Add(new object[] { "Mime-Version", message.Headers.MimeVersion });
            rows.Add(new object[] { "Return-Path", message.Headers.ReturnPath });
            rows.Add(new object[] { "Subject", message.Headers.Subject });

            // Add all unknown headers
            foreach (string key in message.Headers.UnknownHeaders)
            {
                string[] values = message.Headers.UnknownHeaders.GetValues(key);
                if (values != null)
                    foreach (string value in values)
                    {
                        rows.Add(new object[] { key, value });
                    }
            }

            // Now set the headers displayed on the GUI to the header table we just generated
            gridHeaders.DataMember = table.TableName;
            gridHeaders.DataSource = dataSet;
        }

        /// <summary>
        /// Finds the MessageNumber of a Message given a <see cref="TreeNode"/> to search in.
        /// The root of this <see cref="TreeNode"/> should have the Tag property set to a int, which
        /// points into the <see cref="messages"/> dictionary.
        /// </summary>
        /// <param name="node">The <see cref="TreeNode"/> to look in. Cannot be <see langword="null"/>.</param>
        /// <returns>The found int</returns>
        private static int GetMessageNumberFromSelectedNode(TreeNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            // Check if we are at the root, by seeing if it has the Tag property set to an int
            if (node.Tag is int)
            {
                return (int)node.Tag;
            }

            // Otherwise we are not at the root, move up the tree
            return GetMessageNumberFromSelectedNode(node.Parent);
        }

        private void ListAttachmentsAttachmentSelected(object sender, TreeViewEventArgs args)
        {
            // Fetch the attachment part which is currently selected
            MessagePart attachment = (MessagePart)listAttachments.SelectedNode.Tag;

            if (attachment != null)
            {
                saveFile.FileName = attachment.FileName;
                DialogResult result = saveFile.ShowDialog();
                if (result != DialogResult.OK)
                    return;

                // Now we want to save the attachment
                FileInfo file = new FileInfo(saveFile.FileName);

                // Check if the file already exists
                if (file.Exists)
                {
                    // User was asked when he chose the file, if he wanted to overwrite it
                    // Therefore, when we get to here, it is okay to delete the file
                    file.Delete();
                }

                // Lets try to save to the file
                try
                {
                    attachment.Save(file);

                    MessageBox.Show(this, "Attachment saved successfully");
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, "Attachment saving failed. Exception message: " + e.Message);
                }
            }
            else
            {
                MessageBox.Show(this, "Attachment object was null!");
            }
        }

        private void MenuDeleteMessageClick(object sender, EventArgs e)
        {
            if (listMessages.SelectedNode != null)
            {
                DialogResult drRet = MessageBox.Show(this, "Are you sure to delete the email?", "Delete email", MessageBoxButtons.YesNo);
                if (drRet == DialogResult.Yes)
                {
                    int messageNumber = GetMessageNumberFromSelectedNode(listMessages.SelectedNode);
                    pop3Client.DeleteMessage(messageNumber);

                    listMessages.Nodes[messageNumber].Remove();

                    drRet = MessageBox.Show(this, "Do you want to receive email again (this will commit your changes)?", "Receive email", MessageBoxButtons.YesNo);
                    if (drRet == DialogResult.Yes)
                        ReceiveMails();
                }
            }
        }

        private void UidlButtonClick(object sender, EventArgs e)
        {
            List<string> uids = pop3Client.GetMessageUids();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UIDL:");
            stringBuilder.Append("\r\n");
            foreach (string uid in uids)
            {
                stringBuilder.Append(uid);
                stringBuilder.Append("\r\n");
            }

            messageTextBox.Text = stringBuilder.ToString();
        }

        private void MenuViewSourceClick(object sender, EventArgs e)
        {

            if (listMessages.SelectedNode != null)
            {
                int messageNumber = GetMessageNumberFromSelectedNode(listMessages.SelectedNode);
                Message m = messages[messageNumber];

                // We do not know the encoding of the full message - and the parts could be differently
                // encoded. Therefore we take a choice of simply using US-ASCII encoding on the raw bytes
                // to get the source code for the message. Any bytes not in th US-ASCII encoding, will then be
                // turned into question marks "?"
                ShowSourceForm sourceForm = new ShowSourceForm(Encoding.ASCII.GetString(m.RawMessage));
                sourceForm.ShowDialog();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendForm sf = new SendForm();
            sf.Show();
        }
        MailConfiguration mailConf;
        private void btnSendMail_Click(object sender, EventArgs e)
        {
            try
            {
                //OutMailDB.WriteLog("OnStart", "����ɹ�!");
                mailConf = MailConfiguration.Load("MailConfig.xml");
                SendSmptMail();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Try:MailConfiguration:::" + ex.ToString());
            }
        }
        private void SendSmptMail()
        {
            foreach (ServerConf rc in mailConf.ServerConfiguration)
            {
                //using (Dashinginfo.Practices.Library.Database.SqlDatabase db = new Dashinginfo.Practices.Library.Database.SqlDatabase(rc.ConnectionString))
                //{
                    try
                    {
                        SendMail(rc);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Try:SendMail:::" + ex.ToString());
                    }
                //}
            }
        }
        private void SendMail(ServerConf sc)
        {
            // ע��������Ҫ����߼�������������
            // ����ʧ�ܺ����⴦��ÿʧ��һ�μ�¼���һ�η���ʱ�䣬Ȼ��ʧ������ʱ������һ�η���ʱ��������㷨���£�
            //�����ʹ���*mailConf.RestFailRetryBase*mailConf.FailRetryIntervalPow��
            if (sc.SendConfiguration.Enabled)
            {
                MailSmtpClient msc = new MailSmtpClient(sc.SendConfiguration.SmtpServer, sc.SendConfiguration.MailAccount, sc.SendConfiguration.MailPassword);
                DataSet ds = OutMailDB.GetMailByStatus(OutMailStatus.UnSend, mailConf.FailRetryIntervalPow, mailConf.FailMostNum, sc.Provider.ConnectionString);//���ݵ�ǰ�����ݿ�����  �鿴δ�����ʼ�����Ϣ                 
                List<string> senderIDs = OutMailDB.GetSenderIDsByDataTable(ds.Tables[0]);
                if (sc.SendConfiguration.LogConfiguration.Enabled)
                {
                    OutMailDB.WriteLog("SendMail", "����SendMail,Ҫ����" + ds.Tables[0].Rows.Count + "���ʼ�", sc.SendConfiguration.LogConfiguration.Url);
                    OutMailDB.WriteLog("SendMail", "Ҫ�����ʼ�SendMailIDs:" + string.Join(",", senderIDs), sc.SendConfiguration.LogConfiguration.Url);
                    OutMailDB.WriteLog("SendMail", "���ݿ����Ӵ�Ϊ:" + sc.Provider.ConnectionString, sc.SendConfiguration.LogConfiguration.Url);
                }
                foreach (string strID in senderIDs)
                {
                    DataRow[] drs = ds.Tables[0].Select("MailID=" + strID);
                    Dictionary<string, Stream> dictemp = new Dictionary<string, Stream>();
                    try
                    {
                        if (sc.SendConfiguration.LogConfiguration.Enabled)
                            OutMailDB.WriteLog("SendMail", "Ҫ�����ʼ�SendMailID:" + strID + ";ToUser:" + string.Join(",", drs[0]["To"].ToString().Split(';')), sc.SendConfiguration.LogConfiguration.Url);
                        string[] mailToAddress = null;
                        string[] mailCC = null;
                        string mailSender = string.Empty;
                        string mailFrom = string.Empty;
                        if (!string.IsNullOrEmpty(drs[0]["To"].ToString()))
                        {
                            mailToAddress = drs[0]["To"].ToString().Split(';');
                        }
                        else
                        {
                            mailToAddress = new string[] { };
                        }
                        if (!string.IsNullOrEmpty(drs[0]["CC"].ToString()))
                        {
                            mailCC = drs[0]["CC"].ToString().Split(';');
                        }
                        else
                        {
                            mailCC = new string[] { };
                        }
                        if (!string.IsNullOrEmpty(drs[0]["Sender"].ToString()))
                        {
                            mailSender = drs[0]["Sender"].ToString();
                        }
                        else
                        {
                            mailSender = sc.SendConfiguration.DefaultSender;
                        }
                        if (!string.IsNullOrEmpty(drs[0]["From"].ToString()))
                        {
                            mailFrom = drs[0]["From"].ToString();
                        }
                        else
                        {
                            mailFrom = sc.SendConfiguration.MailAccount;
                        }
                        msc.SendMailMessage(mailSender,
                        mailFrom,
                        null,
                        mailToAddress,
                        mailCC,
                        null,
                        drs[0]["Subject"].ToString(),
                        drs[0]["Body"].ToString(),
                        OutMailDB.GetAttachMents(drs),
                        true);
                        if (sc.SendConfiguration.LogConfiguration.Enabled)
                            OutMailDB.WriteLog("SendMailMessageOnTime", "this.SmtpClient.Send Success", sc.SendConfiguration.LogConfiguration.Url);
                        //�ɹ�������rc.ConnectionString���޸�
                        OutMailDB.UpdataMailStatus((int)drs[0]["MailID"], OutMailStatus.Success, sc.Provider.ConnectionString);
                    }
                    catch (Exception ex)
                    {
                        //OutMailDB.UpdateSendErrorMessage((int)drs[0]["MailID"], sc.Provider.ConnectionString, ex.Message);
                        MessageBox.Show(ex.Message);
                        if (sc.SendConfiguration.LogConfiguration.Enabled)
                            OutMailDB.WriteLog("SendMail#####Exception", "Ҫ�����ʼ�SendMailID:" + strID + ";Ex:" + ex.Message, sc.SendConfiguration.LogConfiguration.Url);
                        if (int.Parse(drs[0]["SendCount"].ToString()) >= mailConf.FailMostNum - 1)
                        {
                            //ʧ�ܣ�����rc.ConnectionString���޸�
                            OutMailDB.UpdataMailStatus((int)drs[0]["MailID"], OutMailStatus.Fail, sc.Provider.ConnectionString);
                            //����rc.ConnectionStringдLog
                            //LogManager.LogError("�ʼ�����ʧ��", ex.Message, drs[0]["MailToAddress"].ToString(), drs[0]["MailToAddress"].ToString(), Configuration.LogCategoryID, Configuration.LogCategoryName);
                        }
                        //ʧ�� ����rc.ConnectionString���޸�
                        OutMailDB.UpdateSendCount((int)drs[0]["MailID"], sc.Provider.ConnectionString);
                    }

                }
            }
        }
        
    }
}