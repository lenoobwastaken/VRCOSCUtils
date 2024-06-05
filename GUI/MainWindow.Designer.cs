
namespace VRChatify
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.presenceToggle = new System.Windows.Forms.CheckBox();
            this.OSCToggle = new System.Windows.Forms.CheckBox();
            this.oscMessageLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PresenceUpdateButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.presenceDetails = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.SessionHolder = new System.Windows.Forms.GroupBox();
            this.ForceUpdateSessions = new System.Windows.Forms.Button();
            this.Sessions = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ClanTagLabel = new System.Windows.Forms.Label();
            this.ClanTag = new System.Windows.Forms.TextBox();
            this.DebugLogging = new System.Windows.Forms.CheckBox();
            this.PlaceHolder = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Console = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.PlaceHolder.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.Console.SuspendLayout();
            this.SuspendLayout();
            // 
            // presenceToggle
            // 
            this.presenceToggle.AutoSize = true;
            this.presenceToggle.BackColor = System.Drawing.Color.Transparent;
            this.presenceToggle.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.presenceToggle.Location = new System.Drawing.Point(9, 19);
            this.presenceToggle.Name = "presenceToggle";
            this.presenceToggle.Size = new System.Drawing.Size(59, 17);
            this.presenceToggle.TabIndex = 0;
            this.presenceToggle.Text = "Enable";
            this.presenceToggle.UseVisualStyleBackColor = false;
            this.presenceToggle.CheckedChanged += new System.EventHandler(this.PresenceToggle_CheckedChanged);
            // 
            // OSCToggle
            // 
            this.OSCToggle.AutoSize = true;
            this.OSCToggle.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.OSCToggle.Location = new System.Drawing.Point(9, 19);
            this.OSCToggle.Name = "OSCToggle";
            this.OSCToggle.Size = new System.Drawing.Size(122, 17);
            this.OSCToggle.TabIndex = 2;
            this.OSCToggle.Text = "OSC Message Send";
            this.OSCToggle.UseVisualStyleBackColor = true;
            this.OSCToggle.CheckedChanged += new System.EventHandler(this.OSCToggle_CheckedChanged);
            // 
            // oscMessageLabel
            // 
            this.oscMessageLabel.AutoSize = true;
            this.oscMessageLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.oscMessageLabel.Location = new System.Drawing.Point(7, 39);
            this.oscMessageLabel.Name = "oscMessageLabel";
            this.oscMessageLabel.Size = new System.Drawing.Size(70, 13);
            this.oscMessageLabel.TabIndex = 3;
            this.oscMessageLabel.Text = "OSC Toggles";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox7);
            this.groupBox1.Controls.Add(this.checkBox5);
            this.groupBox1.Controls.Add(this.checkBox4);
            this.groupBox1.Controls.Add(this.checkBox3);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.oscMessageLabel);
            this.groupBox1.Controls.Add(this.OSCToggle);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 122);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OSC Settings";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.checkBox5.Location = new System.Drawing.Point(81, 78);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(78, 17);
            this.checkBox5.TabIndex = 8;
            this.checkBox5.Text = "Local Time";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.checkBox4.Location = new System.Drawing.Point(9, 99);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(62, 17);
            this.checkBox4.TabIndex = 7;
            this.checkBox4.Text = "Clantag";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.checkBox3.Location = new System.Drawing.Point(81, 55);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(46, 17);
            this.checkBox3.TabIndex = 6;
            this.checkBox3.Text = "FPS";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.checkBox2.Location = new System.Drawing.Point(8, 76);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(67, 17);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.Text = "PC Stats";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.checkBox1.Location = new System.Drawing.Point(9, 55);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(58, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Spotify";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.PresenceUpdateButton);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.presenceDetails);
            this.groupBox2.Controls.Add(this.presenceToggle);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox2.Location = new System.Drawing.Point(12, 133);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(274, 115);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Discord Presence";
            // 
            // PresenceUpdateButton
            // 
            this.PresenceUpdateButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.PresenceUpdateButton.Location = new System.Drawing.Point(6, 81);
            this.PresenceUpdateButton.Name = "PresenceUpdateButton";
            this.PresenceUpdateButton.Size = new System.Drawing.Size(97, 23);
            this.PresenceUpdateButton.TabIndex = 3;
            this.PresenceUpdateButton.Text = "Update";
            this.PresenceUpdateButton.UseVisualStyleBackColor = true;
            this.PresenceUpdateButton.Click += new System.EventHandler(this.PresenceUpdateButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Presence Text";
            // 
            // presenceDetails
            // 
            this.presenceDetails.Location = new System.Drawing.Point(6, 55);
            this.presenceDetails.Name = "presenceDetails";
            this.presenceDetails.Size = new System.Drawing.Size(100, 20);
            this.presenceDetails.TabIndex = 1;
            this.presenceDetails.Text = "Using VRchatify";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SessionHolder);
            this.groupBox3.Controls.Add(this.ForceUpdateSessions);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox3.Location = new System.Drawing.Point(292, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 115);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Session List";
            // 
            // SessionHolder
            // 
            this.SessionHolder.Location = new System.Drawing.Point(6, 19);
            this.SessionHolder.Name = "SessionHolder";
            this.SessionHolder.Size = new System.Drawing.Size(187, 61);
            this.SessionHolder.TabIndex = 2;
            this.SessionHolder.TabStop = false;
            // 
            // ForceUpdateSessions
            // 
            this.ForceUpdateSessions.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ForceUpdateSessions.Location = new System.Drawing.Point(6, 86);
            this.ForceUpdateSessions.Name = "ForceUpdateSessions";
            this.ForceUpdateSessions.Size = new System.Drawing.Size(187, 23);
            this.ForceUpdateSessions.TabIndex = 1;
            this.ForceUpdateSessions.Text = "Force Update Sessions";
            this.ForceUpdateSessions.UseVisualStyleBackColor = true;
            this.ForceUpdateSessions.Click += new System.EventHandler(this.ForceUpdateSessions_Click);
            // 
            // Sessions
            // 
            this.Sessions.FormattingEnabled = true;
            this.Sessions.Location = new System.Drawing.Point(6, 19);
            this.Sessions.Name = "Sessions";
            this.Sessions.Size = new System.Drawing.Size(187, 147);
            this.Sessions.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ClanTagLabel);
            this.groupBox4.Controls.Add(this.ClanTag);
            this.groupBox4.Controls.Add(this.DebugLogging);
            this.groupBox4.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox4.Location = new System.Drawing.Point(12, 254);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(274, 100);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Settings";
            // 
            // ClanTagLabel
            // 
            this.ClanTagLabel.AutoSize = true;
            this.ClanTagLabel.Location = new System.Drawing.Point(9, 40);
            this.ClanTagLabel.Name = "ClanTagLabel";
            this.ClanTagLabel.Size = new System.Drawing.Size(50, 13);
            this.ClanTagLabel.TabIndex = 5;
            this.ClanTagLabel.Text = "Clan Tag";
            // 
            // ClanTag
            // 
            this.ClanTag.Location = new System.Drawing.Point(9, 56);
            this.ClanTag.Name = "ClanTag";
            this.ClanTag.Size = new System.Drawing.Size(122, 20);
            this.ClanTag.TabIndex = 4;
            this.ClanTag.Text = "VRChatify";
            this.ClanTag.TextChanged += new System.EventHandler(this.ClanTag_TextChanged);
            // 
            // DebugLogging
            // 
            this.DebugLogging.AutoSize = true;
            this.DebugLogging.Location = new System.Drawing.Point(9, 20);
            this.DebugLogging.Name = "DebugLogging";
            this.DebugLogging.Size = new System.Drawing.Size(99, 17);
            this.DebugLogging.TabIndex = 0;
            this.DebugLogging.Text = "Debug Logging";
            this.DebugLogging.UseVisualStyleBackColor = true;
            this.DebugLogging.CheckedChanged += new System.EventHandler(this.DebugLogging_CheckedChanged);
            // 
            // PlaceHolder
            // 
            this.PlaceHolder.Controls.Add(this.richTextBox2);
            this.PlaceHolder.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.PlaceHolder.Location = new System.Drawing.Point(499, 13);
            this.PlaceHolder.Name = "PlaceHolder";
            this.PlaceHolder.Size = new System.Drawing.Size(269, 341);
            this.PlaceHolder.TabIndex = 9;
            this.PlaceHolder.TabStop = false;
            this.PlaceHolder.Text = "Custom Variables";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.richTextBox1);
            this.groupBox5.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox5.Location = new System.Drawing.Point(292, 134);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 173);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Custom Text";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.Location = new System.Drawing.Point(6, 16);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(188, 151);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // Console
            // 
            this.Console.Controls.Add(this.label2);
            this.Console.ForeColor = System.Drawing.Color.White;
            this.Console.Location = new System.Drawing.Point(12, 360);
            this.Console.Name = "Console";
            this.Console.Size = new System.Drawing.Size(756, 174);
            this.Console.TabIndex = 11;
            this.Console.TabStop = false;
            this.Console.Text = "Console";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 0;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBox6.Location = new System.Drawing.Point(356, 312);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(59, 17);
            this.checkBox6.TabIndex = 12;
            this.checkBox6.Text = "Enable";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(298, 331);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Load Config";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(397, 331);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Save Config";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.SystemColors.ControlText;
            this.richTextBox2.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBox2.Location = new System.Drawing.Point(7, 20);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(262, 321);
            this.richTextBox2.TabIndex = 0;
            this.richTextBox2.Text = "{SPOTIFY}\n\n{TIMESTAMP}\n\n{FPS}\n\n{TIME}\n\n{CLANTAG}\n\n{CPU}\n\n{GPU}\n\n{RAM}\n\n{TABBED}\n\n" +
    "\n\n";
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.checkBox7.Location = new System.Drawing.Point(81, 101);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(63, 17);
            this.checkBox7.TabIndex = 9;
            this.checkBox7.Text = "Tabbed";
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(780, 549);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox6);
            this.Controls.Add(this.Console);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.PlaceHolder);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "VRChatify";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.PlaceHolder.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.Console.ResumeLayout(false);
            this.Console.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox oscMessage;

        private System.Windows.Forms.CheckBox presenceToggle;
        private System.Windows.Forms.CheckBox OSCToggle;
        private System.Windows.Forms.Label oscMessageLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox presenceDetails;
        private System.Windows.Forms.Button PresenceUpdateButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button ForceUpdateSessions;
        private System.Windows.Forms.ListBox Sessions;
        private System.Windows.Forms.GroupBox SessionHolder;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox DebugLogging;
        private System.Windows.Forms.Label ClanTagLabel;
        private System.Windows.Forms.TextBox ClanTag;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox PlaceHolder;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox Console;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.CheckBox checkBox7;
    }
}
