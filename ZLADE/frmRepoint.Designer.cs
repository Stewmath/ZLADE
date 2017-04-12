namespace ZLADE
{
	partial class frmRepoint
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.nAddress = new System.Windows.Forms.NumericUpDown();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.cCopy = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.nAddress)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(150, 86);
			this.label1.TabIndex = 0;
			this.label1.Text = "Repointing is used to allow you extend rooms, and add objects that you couldn\'t a" +
				"dd with the space you\'re given.\r\nNote: This requires saving.";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 86);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Address:";
			// 
			// nAddress
			// 
			this.nAddress.Hexadecimal = true;
			this.nAddress.Location = new System.Drawing.Point(66, 84);
			this.nAddress.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.nAddress.Name = "nAddress";
			this.nAddress.Size = new System.Drawing.Size(96, 20);
			this.nAddress.TabIndex = 2;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(87, 132);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(12, 132);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 4;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// cCopy
			// 
			this.cCopy.AutoSize = true;
			this.cCopy.Checked = true;
			this.cCopy.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cCopy.Location = new System.Drawing.Point(12, 110);
			this.cCopy.Name = "cCopy";
			this.cCopy.Size = new System.Drawing.Size(141, 17);
			this.cCopy.TabIndex = 5;
			this.cCopy.Text = "Copy Existing Data Over";
			this.cCopy.UseVisualStyleBackColor = true;
			// 
			// frmRepoint
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(174, 167);
			this.Controls.Add(this.cCopy);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.nAddress);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmRepoint";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Room Repointer";
			this.Load += new System.EventHandler(this.frmRepoint_Load);
			((System.ComponentModel.ISupportInitialize)(this.nAddress)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		public System.Windows.Forms.NumericUpDown nAddress;
		private System.Windows.Forms.CheckBox cCopy;
	}
}