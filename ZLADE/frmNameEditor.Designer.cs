namespace ZLADE
{
	partial class frmNameEditor
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
			this.tText = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cDungeon = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.cDungeon);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.tText);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(278, 103);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Dungeon Names";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(31, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Text:";
			// 
			// tText
			// 
			this.tText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tText.Location = new System.Drawing.Point(43, 19);
			this.tText.Name = "tText";
			this.tText.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.tText.Size = new System.Drawing.Size(229, 20);
			this.tText.TabIndex = 1;
			this.tText.TextChanged += new System.EventHandler(this.tText_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(54, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Dungeon:";
			// 
			// cDungeon
			// 
			this.cDungeon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cDungeon.FormattingEnabled = true;
			this.cDungeon.Items.AddRange(new object[] {
            "Level 1",
            "Level 2",
            "Level 3",
            "Level 4",
            "Level 5",
            "Level 6",
            "Level 7",
            "Level 8",
            "Wind Fish\'s Egg"});
			this.cDungeon.Location = new System.Drawing.Point(66, 45);
			this.cDungeon.Name = "cDungeon";
			this.cDungeon.Size = new System.Drawing.Size(206, 21);
			this.cDungeon.TabIndex = 3;
			this.cDungeon.SelectedIndexChanged += new System.EventHandler(this.cDungeon_SelectedIndexChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(197, 72);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// frmNameEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(302, 127);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmNameEditor";
			this.ShowIcon = false;
			this.Text = "Dungeon Name Editor";
			this.Load += new System.EventHandler(this.frmNameEditor_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cDungeon;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tText;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
	}
}