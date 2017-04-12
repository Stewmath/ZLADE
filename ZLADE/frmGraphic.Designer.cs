namespace ZLADE
{
	partial class frmGraphic
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.pGraphics = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cGraphics = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.cboZoom = new System.Windows.Forms.ComboBox();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pGraphics)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.Color.Gray;
			this.panel1.Controls.Add(this.pGraphics);
			this.panel1.Location = new System.Drawing.Point(0, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(512, 512);
			this.panel1.TabIndex = 0;
			// 
			// pGraphics
			// 
			this.pGraphics.BackColor = System.Drawing.Color.Gray;
			this.pGraphics.Location = new System.Drawing.Point(0, 0);
			this.pGraphics.Name = "pGraphics";
			this.pGraphics.Size = new System.Drawing.Size(512, 512);
			this.pGraphics.TabIndex = 0;
			this.pGraphics.TabStop = false;
			this.pGraphics.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pGraphics_MouseDown);
			this.pGraphics.Paint += new System.Windows.Forms.PaintEventHandler(this.pGraphics_Paint);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(518, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Graphics:";
			// 
			// cGraphics
			// 
			this.cGraphics.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cGraphics.FormattingEnabled = true;
			this.cGraphics.Items.AddRange(new object[] {
            "The Legend of Zelda",
            "Windfish",
            "Instruments"});
			this.cGraphics.Location = new System.Drawing.Point(521, 28);
			this.cGraphics.Name = "cGraphics";
			this.cGraphics.Size = new System.Drawing.Size(147, 21);
			this.cGraphics.TabIndex = 2;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(593, 55);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "Load";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(518, 84);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(37, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Zoom:";
			// 
			// cboZoom
			// 
			this.cboZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboZoom.FormattingEnabled = true;
			this.cboZoom.Items.AddRange(new object[] {
            "100%",
            "200%",
            "400%",
            "800%",
            "1600%"});
			this.cboZoom.Location = new System.Drawing.Point(518, 100);
			this.cboZoom.Name = "cboZoom";
			this.cboZoom.Size = new System.Drawing.Size(147, 21);
			this.cboZoom.TabIndex = 5;
			this.cboZoom.SelectedIndexChanged += new System.EventHandler(this.cboZoom_SelectedIndexChanged);
			// 
			// frmGraphic
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(680, 536);
			this.Controls.Add(this.cboZoom);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.cGraphics);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmGraphic";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Graphics Viewer";
			this.Load += new System.EventHandler(this.frmGraphic_Load);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pGraphics)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pGraphics;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cGraphics;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cboZoom;
	}
}