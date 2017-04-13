namespace ZLADE
{
	partial class frmOTileset
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
			this.pTileset = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.nPalette = new System.Windows.Forms.NumericUpDown();
			this.nPrimary = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.nAddress = new System.Windows.Forms.NumericUpDown();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pTileset)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nPalette)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nPrimary)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nAddress)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pTileset);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(268, 276);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Tileset";
			// 
			// pTileset
			// 
			this.pTileset.BackColor = System.Drawing.Color.Transparent;
			this.pTileset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pTileset.Location = new System.Drawing.Point(6, 14);
			this.pTileset.Name = "pTileset";
			this.pTileset.Size = new System.Drawing.Size(256, 256);
			this.pTileset.TabIndex = 0;
			this.pTileset.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(286, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Palette Index:";
			// 
			// nPalette
			// 
			this.nPalette.Hexadecimal = true;
			this.nPalette.Location = new System.Drawing.Point(373, 24);
			this.nPalette.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nPalette.Name = "nPalette";
			this.nPalette.Size = new System.Drawing.Size(62, 20);
			this.nPalette.TabIndex = 2;
			this.nPalette.ValueChanged += new System.EventHandler(this.nPalette_ValueChanged);
			// 
			// nPrimary
			// 
			this.nPrimary.Hexadecimal = true;
			this.nPrimary.Location = new System.Drawing.Point(373, 50);
			this.nPrimary.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nPrimary.Name = "nPrimary";
			this.nPrimary.Size = new System.Drawing.Size(62, 20);
			this.nPrimary.TabIndex = 4;
			this.nPrimary.ValueChanged += new System.EventHandler(this.nPrimary_ValueChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(286, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(73, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Primary Index:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(358, 256);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.nAddress);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Location = new System.Drawing.Point(289, 76);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(146, 43);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Secondary Palette";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Address:";
			// 
			// nAddress
			// 
			this.nAddress.Hexadecimal = true;
			this.nAddress.Location = new System.Drawing.Point(57, 14);
			this.nAddress.Maximum = new decimal(new int[] {
            786432,
            0,
            0,
            0});
			this.nAddress.Minimum = new decimal(new int[] {
            524288,
            0,
            0,
            0});
			this.nAddress.Name = "nAddress";
			this.nAddress.Size = new System.Drawing.Size(83, 20);
			this.nAddress.TabIndex = 1;
			this.nAddress.Value = new decimal(new int[] {
            524288,
            0,
            0,
            0});
			this.nAddress.ValueChanged += new System.EventHandler(this.nAddress_ValueChanged);
			// 
			// frmOTileset
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(445, 291);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.nPrimary);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.nPalette);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmOTileset";
			this.ShowIcon = false;
			this.Text = "Overworld Tileset Editor";
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pTileset)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nPalette)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nPrimary)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nAddress)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		public System.Windows.Forms.NumericUpDown nPalette;
		public System.Windows.Forms.NumericUpDown nPrimary;
		public System.Windows.Forms.PictureBox pTileset;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.NumericUpDown nAddress;
	}
}