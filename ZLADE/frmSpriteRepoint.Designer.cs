namespace ZLADE
{
	partial class frmSpriteRepoint
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
			this.cCopy = new System.Windows.Forms.CheckBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.nAddress = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nAddress)).BeginInit();
			this.SuspendLayout();
			// 
			// cCopy
			// 
			this.cCopy.AutoSize = true;
			this.cCopy.Checked = true;
			this.cCopy.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cCopy.Location = new System.Drawing.Point(12, 33);
			this.cCopy.Name = "cCopy";
			this.cCopy.Size = new System.Drawing.Size(141, 17);
			this.cCopy.TabIndex = 10;
			this.cCopy.Text = "Copy Existing Data Over";
			this.cCopy.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(12, 55);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 9;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(87, 55);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 8;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// nAddress
			// 
			this.nAddress.Hexadecimal = true;
			this.nAddress.Location = new System.Drawing.Point(66, 7);
			this.nAddress.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.nAddress.Name = "nAddress";
			this.nAddress.Size = new System.Drawing.Size(96, 20);
			this.nAddress.TabIndex = 7;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Address:";
			// 
			// frmSpriteRepoint
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(178, 84);
			this.Controls.Add(this.cCopy);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.nAddress);
			this.Controls.Add(this.label2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmSpriteRepoint";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sprite Repointer";
			this.Load += new System.EventHandler(this.frmSpriteRepoint_Load);
			((System.ComponentModel.ISupportInitialize)(this.nAddress)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox cCopy;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		public System.Windows.Forms.NumericUpDown nAddress;
		private System.Windows.Forms.Label label2;
	}
}