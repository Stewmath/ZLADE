namespace ZLADE
{
	partial class frmEvent
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cEvent = new System.Windows.Forms.ComboBox();
			this.cTrigger = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.cTrigger);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.cEvent);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(262, 96);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Event";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Event:";
			// 
			// cEvent
			// 
			this.cEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cEvent.FormattingEnabled = true;
			this.cEvent.Items.AddRange(new object[] {
            "00 - None",
            "02 - Open Shutters",
            "04 - Defeats All Enemies",
            "06 - Chest Appears (X: 8, Y: 2)",
            "08 - Key Drops (X: 2, Y: 3)",
            "0A - Stairs Appears (X: 8, Y: 1)",
            "0C - Miniboss Death Event",
            "0E - Unknown"});
			this.cEvent.Location = new System.Drawing.Point(50, 13);
			this.cEvent.Name = "cEvent";
			this.cEvent.Size = new System.Drawing.Size(206, 21);
			this.cEvent.TabIndex = 1;
			// 
			// cTrigger
			// 
			this.cTrigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cTrigger.FormattingEnabled = true;
			this.cTrigger.Items.AddRange(new object[] {
            "00 - None",
            "01 - Defeat All Enemies",
            "02 - Push a Block",
            "03 - Push a Trigger",
            "04 - Unknown",
            "05 - Light All Torches",
            "06 - L2 Nightmare Key Puzzle",
            "07 - Push Two Blocks Together",
            "08 - Kill Special Enemies",
            "09 - L4 Tile Puzzle",
            "0A - Defeat Boss 4 or 7",
            "0B - One-Way Shutter (Throw to Open)",
            "0C - Throw Horse Heads at Wall",
            "0D - Smash Chest Open",
            "0E - \"Fill in the holes with the rock that rolls\"",
            "0F - Fire Arrow at Statue"});
			this.cTrigger.Location = new System.Drawing.Point(50, 40);
			this.cTrigger.Name = "cTrigger";
			this.cTrigger.Size = new System.Drawing.Size(206, 21);
			this.cTrigger.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Trigger:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(85, 67);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// frmEvent
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(286, 114);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmEvent";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Event Editor";
			this.Load += new System.EventHandler(this.frmEvent_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cEvent;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cTrigger;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
	}
}