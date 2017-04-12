using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ZLADE
{
	public partial class frmGraphic : Form
	{
		public byte[,] data = new byte[256, 64];
		MapLoader m;
		public int tileCount = 0;
		public Color[] palette = { Color.Black, Color.DarkGray, Color.LightGray, Color.White };
		BinaryReader reader;
		public int paletteIndex = 0;
		bool drawGrid = true;

		public frmGraphic(MapLoader mm)
		{
			InitializeComponent();
			m = mm;
		}

		private void frmGraphic_Load(object sender, EventArgs e)
		{
			cGraphics.SelectedIndex = 0;
			cboZoom.SelectedIndex = 3;
			readData(112, 0xBC900);
			pGraphics.Invalidate();
		}

		public void readData(int tilecount, long address)
		{
			tileCount = tilecount;
			m.openRom();
			reader = m.reader;
			reader.BaseStream.Position = address;
			
			for (int ind = 0; ind < tilecount; ind++)
			{
				int last = 0;
				byte[] by = reader.ReadBytes(16);
				string[] bincodes = new string[2];
				for (int i = 0; i < 16; i++)
				{
					bincodes[0] = Convert.ToString(by[i], 2);
					bincodes[1] = Convert.ToString(by[i + 1], 2);
					while (bincodes[0].Length < 8)
						bincodes[0] = "0" + bincodes[0];
					while (bincodes[1].Length < 8)
						bincodes[1] = "0" + bincodes[1];
					for (int k = 0; k < 8; k++)
					{
						string s1 = bincodes[0].Substring(k, 1);
						string s2 = bincodes[1].Substring(k, 1);
						string t = s1 + s2;
						int value = (int)Convert.ToInt64(t, 2);
						data[ind, last] = (byte)value;
						last++;
					}

					i++;
				}
			}
			m.closeRom();
		}

		private void pGraphics_Paint(object sender, PaintEventArgs e)
		{
			if (reader == null)
				return;
			int xx = cboZoom.SelectedIndex;
			int s = 8;
			if (xx == 0)
				s = 1;
			if (xx == 1)
				s = 2;
			if (xx == 2)
				s = 4;
			if (xx == 3)
				s = 8;
			if (xx == 4)
				s = 16;

			int width = (tileCount < 16 ? (tileCount) * s * 8 : 16 * s * 8);
			int vtiles = tileCount / 16;
			int height = (vtiles < 17 ? (vtiles) * s * 8 : vtiles * s * 8);
			pGraphics.Width = width;
			pGraphics.Height = height;

			int lx = 0;
			int ly = 0;
			for (int i = 0; i < tileCount; i++)
			{
				int x = 0;
				int y = 0;
				for (int k = 0; k < 64; k++)
				{
					SolidBrush b = new SolidBrush(palette[data[i, k]]);
					e.Graphics.FillRectangle(b, (x * s) + (lx * (s * 8)), (y * s) + (ly * (s * 8)), s, s);
					x++;
					if (x == 8)
					{
						x = 0;
						y++;
					}
				}
				lx++;
				if(lx == 16)
				{
					lx = 0;
					ly++;
				}
			}

			if (drawGrid)
			{
				for (int i = 0; i < 16; i++)
					e.Graphics.DrawLine(Pens.Red, i * s * 8, 0, i * s * 8, pGraphics.Height);
				for (int k = 0; k < pGraphics.Height / s; k++)
					e.Graphics.DrawLine(Pens.Red, 0, k * s * 8, pGraphics.Width, k * s * 8);
			}
		}

		private void cboZoom_SelectedIndexChanged(object sender, EventArgs e)
		{
			pGraphics.Invalidate();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			int i = cGraphics.SelectedIndex;
			if (i == 0)
			{
				readData(112, 0xBC900);
				pGraphics.Invalidate();
			}
			else if (i == 1)
			{
				readData(128, 0xCD000);
				pGraphics.Invalidate();
			}
			else if (i == 2)
			{
				readData(32, 0xb0d00);
				pGraphics.Invalidate();
			}
		}

		private void pGraphics_MouseDown(object sender, MouseEventArgs e)
		{
			int xx = cboZoom.SelectedIndex;
			int s = 8;
			if (xx == 0)
				s = 1;
			if (xx == 1)
				s = 2;
			if (xx == 2)
				s = 4;
			if (xx == 3)
				s = 8;
			if (xx == 4)
				s = 16;
			SolidBrush b = new SolidBrush(palette[paletteIndex]);
			int y = (e.Y / s) * s;
			int x = (e.X / s) * s;
			pGraphics.CreateGraphics().FillRectangle(b, x, y, s, s);
			if (drawGrid)
			{
				for (int i = 0; i < 16; i++)
					pGraphics.CreateGraphics().DrawLine(Pens.Red, i * s * 8, 0, i * s * 8, pGraphics.Height);
				for (int j = 0; j < pGraphics.Height / s; j++)
					pGraphics.CreateGraphics().DrawLine(Pens.Red, 0, j * s * 8, pGraphics.Width, j * s * 8);
			}
			int index = 0;
			int k = s * 8;
			x /= k;
			y /= k;
			index = x + (y * (pGraphics.Width / k));
			int tx = ((e.X / s) * s);
			int x2 = (e.X % (s * 8)) / s;
			int y2 = (e.Y % (s * 8)) / s;
			int index2 = x2 + (y2 * 8);
			data[index, index2] = (byte)paletteIndex;
		}
	}
}
