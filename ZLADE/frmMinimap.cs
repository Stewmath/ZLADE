using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZLADE
{
	public partial class frmMinimap : Form
	{
		MapLoader loader;
		public byte[] minimapData = new byte[64];
		int selected = 0;
		Form1 form1;
		int mouse = 0;
		int iconselected = 0;
		Point mPoint = new Point();

		public Color MinimapColor
		{
			get
			{
				return Color.FromArgb(40, 48, 80);
			}
		}
		public Color SquareColor
		{
			get
			{
				return Color.FromArgb(255, 255, 173);
			}
		}

		public frmMinimap(MapLoader m, Form1 f)
		{
			InitializeComponent();
			loader = m;
			minimapData = m.minimap;
			form1 = f;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			loader.minimap = minimapData;
			form1.pMinimap.Invalidate();
			this.Close();
		}

		public Bitmap getMinimapIcon(int x, int y)
		{
			Bitmap i = new Bitmap(8, 8);
			Graphics g = Graphics.FromImage(i);
			Rectangle r = new Rectangle(0, 0, 8, 8);
			g.DrawImage(pTiles.Image, r, x, y, 8, 8, GraphicsUnit.Pixel);
			return i;
		}

		void drawMinimap(Graphics g)
		{
			int x = 0;
			int y = 0;
			if (loader == null)
				return;
			for (int i = 0; i < 64; i++)
			{
				byte b = minimapData[i];
				if (b == 0x7D) //Empty
					g.FillRectangle(new SolidBrush(MinimapColor), (x * 16), (y * 16), 16, 16);
				if (b == 0xEF) //Room
					g.DrawImage(getMinimapIcon(0, 0), x * 16, y * 16, 16, 16);
				if (b == 0xEE) //Boss
					g.DrawImage(getMinimapIcon(16, 0), x * 16, y * 16, 16, 16);
				if (b == 0xED) //Chest
					g.DrawImage(getMinimapIcon(8, 0), x * 16, y * 16, 16, 16);
				x++;
				if (x == 8)
				{
					x = 0;
					y++;
				}
			}
		}

		private void pMap_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			drawMinimap(e.Graphics);
			if(mouse == 0)
			{
				int x = (mPoint.X / 16) * 16;
				int y = (mPoint.Y / 16) * 16;
				e.Graphics.DrawRectangle(Pens.Red, x, y, 16, 16);
			}
		}

		private void pTiles_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(Pens.Red, selected * 16, 0, 16, 16);
		}

		private void pTiles_MouseDown(object sender, MouseEventArgs e)
		{
			selected = e.X / 16;
			lSelected.Text = "Selected: " + selected;
			pTiles.Invalidate();
		}

		private void pMap_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.X >= 128 || e.Y >= 128 || loader == null)
				return;
			int i = 0;
			int x = e.X / 16;
			int y = e.Y / 16;
			i = x + (y * 8);
			if (mouse == 1)
				minimapData[i] = (byte)getSelected();
			else
			{
				iconselected = i;
				numericUpDown1.Value = (decimal)loader.realMinimap[i];
			}
			mPoint = new Point(e.X, e.Y);
			pMap.Invalidate();
		}

		int getSelected()
		{
			int i = 0x7D;
			if (selected == 0)
				i = 0xEF;
			if (selected == 2)
				i = 0xEE;
			if (selected == 1)
				i = 0xED;
			return i;
		}

		private void frmMinimap_Load(object sender, EventArgs e)
		{

		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			loader.realMinimap[iconselected] = (byte)numericUpDown1.Value;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			mouse = 0;
			button3.BackColor = SystemColors.Control;
			button2.BackColor = SystemColors.ButtonHighlight;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			mouse = 1;
			button2.BackColor = SystemColors.Control;
			button3.BackColor = SystemColors.ButtonHighlight;
		}
	}
}
