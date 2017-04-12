namespace ZLADE
{
	partial class frmMinimap
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMinimap));
			this.pMap = new System.Windows.Forms.PictureBox();
			this.pTiles = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lSelected = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.pMap)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pTiles)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// pMap
			// 
			this.pMap.Location = new System.Drawing.Point(6, 13);
			this.pMap.Name = "pMap";
			this.pMap.Size = new System.Drawing.Size(128, 128);
			this.pMap.TabIndex = 0;
			this.pMap.TabStop = false;
			this.pMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pMap_MouseDown);
			this.pMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pMap_Paint);
			// 
			// pTiles
			// 
			this.pTiles.Image = ((System.Drawing.Image)(resources.GetObject("pTiles.Image")));
			this.pTiles.Location = new System.Drawing.Point(6, 16);
			this.pTiles.Name = "pTiles";
			this.pTiles.Size = new System.Drawing.Size(64, 16);
			this.pTiles.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pTiles.TabIndex = 1;
			this.pTiles.TabStop = false;
			this.pTiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pTiles_MouseDown);
			this.pTiles.Paint += new System.Windows.Forms.PaintEventHandler(this.pTiles_Paint);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(161, 124);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(50, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lSelected);
			this.groupBox1.Controls.Add(this.pTiles);
			this.groupBox1.Location = new System.Drawing.Point(12, 152);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(140, 41);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Icons";
			// 
			// lSelected
			// 
			this.lSelected.AutoSize = true;
			this.lSelected.Location = new System.Drawing.Point(76, 16);
			this.lSelected.Name = "lSelected";
			this.lSelected.Size = new System.Drawing.Size(61, 13);
			this.lSelected.TabIndex = 2;
			this.lSelected.Text = "Selected: 0";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.pMap);
			this.groupBox2.Location = new System.Drawing.Point(12, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(140, 147);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Minimap";
			// 
			// button3
			// 
			this.button3.BackColor = System.Drawing.SystemColors.Control;
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button3.Location = new System.Drawing.Point(161, 41);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(49, 23);
			this.button3.TabIndex = 11;
			this.button3.Text = "Set";
			this.button3.UseVisualStyleBackColor = false;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Location = new System.Drawing.Point(161, 12);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(50, 23);
			this.button2.TabIndex = 10;
			this.button2.Text = "Pointer";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(158, 67);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "Room:";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Hexadecimal = true;
			this.numericUpDown1.Location = new System.Drawing.Point(161, 83);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(49, 20);
			this.numericUpDown1.TabIndex = 13;
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// frmMinimap
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(215, 196);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmMinimap";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Minimap Editor";
			this.Load += new System.EventHandler(this.frmMinimap_Load);
			((System.ComponentModel.ISupportInitialize)(this.pMap)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pTiles)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pMap;
		private System.Windows.Forms.PictureBox pTiles;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lSelected;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
	}
}