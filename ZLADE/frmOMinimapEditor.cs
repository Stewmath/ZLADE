using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZLADE
{
	public partial class frmOMinimapEditor : Form
	{
		Form1 frm;
		MapLoader m;
		public frmOMinimapEditor(Form1 f, MapLoader l)
		{
			InitializeComponent();
			frm = f;
			m = l;
		}

		private void frmOMinimapEditor_Load(object sender, EventArgs e)
		{

		}

		private void pMap_MouseDown(object sender, MouseEventArgs e)
		{
			int i = (e.X / 16) + ((e.Y / 16) * 16);
			if (i > 255 || i < 0)
				return;
			if (e.Button == MouseButtons.Left)
			{
				m.oMinimapTiles[i].gID = (byte)nTile.Value;
				m.oMinimapTiles[i].pID = (byte)nPal.Value;
				Bitmap b = getTileImage((int)nTile.Value, (int)nPal.Value);
				Bitmap from = (Bitmap)pMap.Image;
				Graphics g = Graphics.FromImage(from);
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.DrawImage(b, (e.X / 16) * 8, (e.Y / 16) * 8, 8, 8);
				pMap.Image = from;
			}
			else if (e.Button == MouseButtons.Right)
			{
				nTile.Value = m.oMinimapTiles[i].gID;
				nPal.Value = m.oMinimapTiles[i].pID;
			}
		}

		public void setOMinimapImage()
		{
			Bitmap b = new Bitmap(128, 128);

			for (int tY = 0; tY < 16; tY++)
			{
				for (int tX = 0; tX < 16; tX++)
				{
					int i = tX + (tY * 16);
					for (int y = 0; y < 8; y++)
					{
						for (int x = 0; x < 8; x++)
						{
							int si = x + (y * 8);
							int t = m.oMinimapTiles[i].gID;
							int p = m.oMinimapTiles[i].pID;
							if (t < 0x70)
								b.SetPixel((tX * 8) + x, (tY * 8) + y, m.oMinimapPalette[p, m.oMinimapGraphics[t, si]]);
							else
								b.SetPixel((tX * 8) + x, (tY * 8) + y, m.oMinimapPalette[p, m.oMinimapGraphics2[t & 0xF, si]]);
						}
					}
				}
			}
			pMap.Image = b;
		}

		Bitmap getTileImage(int index, int pal)
		{
			Bitmap b = new Bitmap(8, 8);
			for (int y = 0; y < 8; y++)
			{
				for (int x = 0; x < 8; x++)
				{
					int si = x + (y * 8);
					int t = index;
					int p = pal;
					if (t < 0x70)
						b.SetPixel(x, y, m.oMinimapPalette[p, m.oMinimapGraphics[t, si]]);
					else
						b.SetPixel(x, y, m.oMinimapPalette[p, m.oMinimapGraphics2[t & 0xF, si]]);
				}
			}
			return b;
		}

		private void pMap_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			if (pMap.Image == null)
				setOMinimapImage();
			e.Graphics.DrawImage(pMap.Image, 0, 0, 256, 256);
		}

		private void nTile_ValueChanged(object sender, EventArgs e)
		{
			Bitmap b = new Bitmap(16, 16);
			Graphics g = Graphics.FromImage(b);
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			g.DrawImage(getTileImage((int)nTile.Value, (int)nPal.Value), 0, 0, 16, 16);
			pTile.Image = b;
		}

		private void nPal_ValueChanged(object sender, EventArgs e)
		{
			Bitmap b = new Bitmap(16, 16);
			Graphics g = Graphics.FromImage(b);
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			g.DrawImage(getTileImage((int)nTile.Value, (int)nPal.Value), 0, 0, 16, 16);
			pTile.Image = b;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			frm.pOMinimap.Image = pMap.Image;
			this.Close();
		}
	}
}
