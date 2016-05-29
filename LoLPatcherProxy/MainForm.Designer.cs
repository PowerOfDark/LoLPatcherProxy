namespace LoLPatcherProxy
{
    partial class MainForm
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
            this.RealmComboBox = new System.Windows.Forms.ComboBox();
            this.RegionComboBox = new System.Windows.Forms.ComboBox();
            this.AirClientComboBox = new System.Windows.Forms.ComboBox();
            this.GameClientComboBox = new System.Windows.Forms.ComboBox();
            this.GameClientVersionLabel = new System.Windows.Forms.Label();
            this.AirClientVersionLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.AIRClientGroupBox = new System.Windows.Forms.GroupBox();
            this.GameClientGroupBox = new System.Windows.Forms.GroupBox();
            this.IgnoreAIRCheckbox = new System.Windows.Forms.CheckBox();
            this.AIRClientGroupBox.SuspendLayout();
            this.GameClientGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // RealmComboBox
            // 
            this.RealmComboBox.FormattingEnabled = true;
            this.RealmComboBox.Items.AddRange(new object[] {
            "live",
            "pbe"});
            this.RealmComboBox.Location = new System.Drawing.Point(12, 12);
            this.RealmComboBox.Name = "RealmComboBox";
            this.RealmComboBox.Size = new System.Drawing.Size(121, 21);
            this.RealmComboBox.TabIndex = 0;
            this.RealmComboBox.Text = "Realm";
            this.RealmComboBox.SelectedIndexChanged += new System.EventHandler(this.RealmComboBox_SelectedIndexChanged);
            // 
            // RegionComboBox
            // 
            this.RegionComboBox.Enabled = false;
            this.RegionComboBox.FormattingEnabled = true;
            this.RegionComboBox.Items.AddRange(new object[] {
            "NA",
            "EUW",
            "EUNE",
            "LA1",
            "LA2",
            "BR",
            "TR",
            "OC1",
            "RU"});
            this.RegionComboBox.Location = new System.Drawing.Point(12, 39);
            this.RegionComboBox.Name = "RegionComboBox";
            this.RegionComboBox.Size = new System.Drawing.Size(121, 21);
            this.RegionComboBox.TabIndex = 1;
            this.RegionComboBox.Text = "Region";
            this.RegionComboBox.SelectedIndexChanged += new System.EventHandler(this.RegionComboBox_SelectedIndexChanged);
            // 
            // AirClientComboBox
            // 
            this.AirClientComboBox.Enabled = false;
            this.AirClientComboBox.FormattingEnabled = true;
            this.AirClientComboBox.Location = new System.Drawing.Point(6, 14);
            this.AirClientComboBox.Name = "AirClientComboBox";
            this.AirClientComboBox.Size = new System.Drawing.Size(121, 21);
            this.AirClientComboBox.TabIndex = 2;
            this.AirClientComboBox.Text = "AIR client version";
            this.AirClientComboBox.SelectedIndexChanged += new System.EventHandler(this.AirClientComboBox_SelectedIndexChanged);
            // 
            // GameClientComboBox
            // 
            this.GameClientComboBox.Enabled = false;
            this.GameClientComboBox.FormattingEnabled = true;
            this.GameClientComboBox.Location = new System.Drawing.Point(6, 14);
            this.GameClientComboBox.Name = "GameClientComboBox";
            this.GameClientComboBox.Size = new System.Drawing.Size(121, 21);
            this.GameClientComboBox.TabIndex = 3;
            this.GameClientComboBox.Text = "Game client version";
            this.GameClientComboBox.SelectedIndexChanged += new System.EventHandler(this.GameClientComboBox_SelectedIndexChanged);
            // 
            // GameClientVersionLabel
            // 
            this.GameClientVersionLabel.AutoSize = true;
            this.GameClientVersionLabel.Location = new System.Drawing.Point(133, 17);
            this.GameClientVersionLabel.Name = "GameClientVersionLabel";
            this.GameClientVersionLabel.Size = new System.Drawing.Size(78, 13);
            this.GameClientVersionLabel.TabIndex = 4;
            this.GameClientVersionLabel.Text = "Loading data...";
            this.GameClientVersionLabel.Visible = false;
            // 
            // AirClientVersionLabel
            // 
            this.AirClientVersionLabel.AutoSize = true;
            this.AirClientVersionLabel.Location = new System.Drawing.Point(133, 17);
            this.AirClientVersionLabel.Name = "AirClientVersionLabel";
            this.AirClientVersionLabel.Size = new System.Drawing.Size(78, 13);
            this.AirClientVersionLabel.TabIndex = 5;
            this.AirClientVersionLabel.Text = "Loading data...";
            this.AirClientVersionLabel.Visible = false;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(9, 177);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // AIRClientGroupBox
            // 
            this.AIRClientGroupBox.Controls.Add(this.IgnoreAIRCheckbox);
            this.AIRClientGroupBox.Controls.Add(this.AirClientComboBox);
            this.AIRClientGroupBox.Controls.Add(this.AirClientVersionLabel);
            this.AIRClientGroupBox.Location = new System.Drawing.Point(9, 79);
            this.AIRClientGroupBox.Name = "AIRClientGroupBox";
            this.AIRClientGroupBox.Size = new System.Drawing.Size(444, 43);
            this.AIRClientGroupBox.TabIndex = 8;
            this.AIRClientGroupBox.TabStop = false;
            this.AIRClientGroupBox.Text = "AIR Client";
            // 
            // GameClientGroupBox
            // 
            this.GameClientGroupBox.Controls.Add(this.GameClientComboBox);
            this.GameClientGroupBox.Controls.Add(this.GameClientVersionLabel);
            this.GameClientGroupBox.Location = new System.Drawing.Point(9, 128);
            this.GameClientGroupBox.Name = "GameClientGroupBox";
            this.GameClientGroupBox.Size = new System.Drawing.Size(444, 43);
            this.GameClientGroupBox.TabIndex = 9;
            this.GameClientGroupBox.TabStop = false;
            this.GameClientGroupBox.Text = "Game Client";
            // 
            // IgnoreAIRCheckbox
            // 
            this.IgnoreAIRCheckbox.AutoSize = true;
            this.IgnoreAIRCheckbox.Dock = System.Windows.Forms.DockStyle.Right;
            this.IgnoreAIRCheckbox.Location = new System.Drawing.Point(291, 16);
            this.IgnoreAIRCheckbox.Name = "IgnoreAIRCheckbox";
            this.IgnoreAIRCheckbox.Size = new System.Drawing.Size(150, 24);
            this.IgnoreAIRCheckbox.TabIndex = 6;
            this.IgnoreAIRCheckbox.Text = "Don\'t download AIR Client";
            this.IgnoreAIRCheckbox.UseVisualStyleBackColor = true;
            this.IgnoreAIRCheckbox.CheckedChanged += new System.EventHandler(this.IgnoreAIRCheckbox_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 209);
            this.ControlBox = false;
            this.Controls.Add(this.GameClientGroupBox);
            this.Controls.Add(this.AIRClientGroupBox);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.RegionComboBox);
            this.Controls.Add(this.RealmComboBox);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.AIRClientGroupBox.ResumeLayout(false);
            this.AIRClientGroupBox.PerformLayout();
            this.GameClientGroupBox.ResumeLayout(false);
            this.GameClientGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox RealmComboBox;
        private System.Windows.Forms.ComboBox RegionComboBox;
        private System.Windows.Forms.ComboBox AirClientComboBox;
        private System.Windows.Forms.ComboBox GameClientComboBox;
        private System.Windows.Forms.Label GameClientVersionLabel;
        private System.Windows.Forms.Label AirClientVersionLabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.GroupBox AIRClientGroupBox;
        private System.Windows.Forms.GroupBox GameClientGroupBox;
        private System.Windows.Forms.CheckBox IgnoreAIRCheckbox;
    }
}