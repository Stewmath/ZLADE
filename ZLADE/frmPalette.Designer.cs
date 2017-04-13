namespace ZLADE
{
	partial class frmPalette
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
			this.pPallete = new System.Windows.Forms.PictureBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.hR = new System.Windows.Forms.HScrollBar();
			this.hG = new System.Windows.Forms.HScrollBar();
			this.label2 = new System.Windows.Forms.Label();
			this.hB = new System.Windows.Forms.HScrollBar();
			this.label3 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.nIndex = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pPallete)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nIndex)).BeginInit();
			this.SuspendLayout();
			// 
			// pPallete
			// 
			this.pPallete.Location = new System.Drawing.Point(12, 19);
			this.pPallete.Name = "pPallete";
			this.pPallete.Size = new System.Drawing.Size(129, 129);
			this.pPallete.TabIndex = 0;
			this.pPallete.TabStop = false;
			this.pPallete.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pPallete_MouseDown);
			this.pPallete.Paint += new System.Windows.Forms.PaintEventHandler(this.pPallete_Paint);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pPallete);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(154, 164);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Palette";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(172, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(18, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "R:";
			// 
			// hR
			// 
			this.hR.LargeChange = 2;
			this.hR.Location = new System.Drawing.Point(193, 26);
			this.hR.Maximum = 32;
			this.hR.Name = "hR";
			this.hR.Size = new System.Drawing.Size(80, 17);
			this.hR.TabIndex = 4;
			this.hR.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hR_Scroll);
			// 
			// hG
			// 
			this.hG.LargeChange = 2;
			this.hG.Location = new System.Drawing.Point(193, 55);
			this.hG.Maximum = 32;
			this.hG.Name = "hG";
			this.hG.Size = new System.Drawing.Size(80, 17);
			this.hG.TabIndex = 6;
			this.hG.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(172, 57);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(18, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "G:";
			// 
			// hB
			// 
			this.hB.LargeChange = 2;
			this.hB.Location = new System.Drawing.Point(193, 83);
			this.hB.Maximum = 32;
			this.hB.Name = "hB";
			this.hB.Size = new System.Drawing.Size(80, 17);
			this.hB.TabIndex = 8;
			this.hB.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hB_Scroll);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(172, 85);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(17, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "B:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(175, 147);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(98, 29);
			this.button1.TabIndex = 9;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(172, 111);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(36, 13);
			this.label4.TabIndex = 10;
			this.label4.Text = "Index:";
			// 
			// nIndex
			// 
			this.nIndex.Location = new System.Drawing.Point(214, 109);
			this.nIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nIndex.Name = "nIndex";
			this.nIndex.Size = new System.Drawing.Size(59, 20);
			this.nIndex.TabIndex = 11;
			this.nIndex.ValueChanged += new System.EventHandler(this.nIndex_ValueChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(173, 131);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(92, 13);
			this.label5.TabIndex = 12;
			this.label5.Text = "Address: 0x85520";
			// 
			// frmPalette
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 191);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.nIndex);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.hB);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.hG);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.hR);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmPalette";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Palette Editor";
			this.Load += new System.EventHandler(this.frmPalette_Load);
			((System.ComponentModel.ISupportInitialize)(this.pPallete)).EndInit();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nIndex)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pPallete;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.HScrollBar hR;
		private System.Windows.Forms.HScrollBar hG;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.HScrollBar hB;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		public System.Windows.Forms.NumericUpDown nIndex;
	}
}