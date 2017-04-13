namespace ZLADE
{
	partial class frmOMinimapEditor
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
			this.pMap = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.nTile = new System.Windows.Forms.NumericUpDown();
			this.pTile = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.nPal = new System.Windows.Forms.NumericUpDown();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pMap)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nTile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pTile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nPal)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pMap);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(271, 288);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Minimap";
			// 
			// pMap
			// 
			this.pMap.Location = new System.Drawing.Point(6, 19);
			this.pMap.Name = "pMap";
			this.pMap.Size = new System.Drawing.Size(256, 256);
			this.pMap.TabIndex = 0;
			this.pMap.TabStop = false;
			this.pMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pMap_MouseDown);
			this.pMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pMap_MouseDown);
			this.pMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pMap_Paint);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(289, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Tile:";
			// 
			// nTile
			// 
			this.nTile.Hexadecimal = true;
			this.nTile.Location = new System.Drawing.Point(322, 20);
			this.nTile.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nTile.Name = "nTile";
			this.nTile.Size = new System.Drawing.Size(57, 20);
			this.nTile.TabIndex = 2;
			this.nTile.ValueChanged += new System.EventHandler(this.nTile_ValueChanged);
			// 
			// pTile
			// 
			this.pTile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pTile.Location = new System.Drawing.Point(385, 34);
			this.pTile.Name = "pTile";
			this.pTile.Size = new System.Drawing.Size(16, 16);
			this.pTile.TabIndex = 3;
			this.pTile.TabStop = false;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(291, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(25, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Pal:";
			// 
			// nPal
			// 
			this.nPal.Hexadecimal = true;
			this.nPal.Location = new System.Drawing.Point(322, 46);
			this.nPal.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
			this.nPal.Name = "nPal";
			this.nPal.Size = new System.Drawing.Size(57, 20);
			this.nPal.TabIndex = 5;
			this.nPal.ValueChanged += new System.EventHandler(this.nPal_ValueChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(289, 277);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(111, 23);
			this.button1.TabIndex = 6;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// frmOMinimapEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(412, 309);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.nPal);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.pTile);
			this.Controls.Add(this.nTile);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmOMinimapEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Overworld Minimap Editor";
			this.Load += new System.EventHandler(this.frmOMinimapEditor_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pMap)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nTile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pTile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nPal)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox pMap;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nTile;
		private System.Windows.Forms.PictureBox pTile;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown nPal;
		private System.Windows.Forms.Button button1;
	}
}