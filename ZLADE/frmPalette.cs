using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZLADE
{
	public partial class frmPalette : Form
	{
		MapLoader m;
		Color[] colors = new Color[32];
		int selected = 0;

		public frmPalette(MapLoader l)
		{
			InitializeComponent();
			m = l;
		}

		int getHexValue(int red, int green, int blue)
		{
			double r = Math.Floor(red * 8.25);
			double g = Math.Floor(green * 8.25);
			double b = Math.Floor(blue * 8.25);

			r = Math.Floor((r + 1) / 8.25);
			g = Math.Floor((g + 1) / 8.25);
			b = Math.Floor((b + 1) / 8.25);

			g *= 2;
			b *= 4;

			b *= 256;
			g *= 16;

			double d = Math.Floor(r + g + b);
			int i = Convert.ToInt32(d);

			return i;
		}

		private void frmPalette_Load(object sender, EventArgs e)
		{

		}

		void setColors()
		{
			for (int i = 0; i < 32; i++)
			{
				colors[i] = getColorFromHex(m.paletteColors[i]);
			}
		}

		public static Color getColorFromHex(int color)
		{
			int r = color & 31;
			int g = (color / 32) & 31;
			int b = (color / 1024) & 31;

			return Color.FromArgb(r * 8, g * 8, b * 8);
		}

		private void pPallete_Paint(object sender, PaintEventArgs e)
		{
			setColors();
			int x = 0;
			int y = 0;
			for (int i = 0; i < 32; i++)
			{
				SolidBrush s = new SolidBrush(colors[i]);
				e.Graphics.FillRectangle(s, x * 32, y * 16, 32, 16);
				x++;
				if (x == 4)
				{
					x = 0;
					y++;
				}
			}
			for (int i = 0; i < 5; i++)
			{
				e.Graphics.DrawLine(Pens.Black, i * 32, 0, i * 32, pPallete.Height);
			}
			for (int k = 0; k < 9; k++)
			{
				e.Graphics.DrawLine(Pens.Black, 0, k * 16, pPallete.Width, k * 16);
			}
			int yy = selected / 4;
			int xx = selected % 4;
			e.Graphics.DrawRectangle(Pens.Red, xx * 32, yy * 16, 32, 16);
		}

		private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
		{
			m.paletteColors[selected] = getHexValue(hR.Value, hG.Value, hB.Value);
			pPallete.Invalidate();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void pPallete_MouseDown(object sender, MouseEventArgs e)
		{
			int x = (e.X / 32) % 4;
			int y = (e.Y / 16) % 8;
			selected = x + (y * 4);
			hR.Value = colors[selected].R / 8;
			hG.Value = colors[selected].G / 8;
			hB.Value = colors[selected].B / 8;
			pPallete.Invalidate();
		}

		private void hR_Scroll(object sender, ScrollEventArgs e)
		{
			m.paletteColors[selected] = getHexValue(hR.Value, hG.Value, hB.Value);
			pPallete.Invalidate();
		}

		private void hB_Scroll(object sender, ScrollEventArgs e)
		{
			m.paletteColors[selected] = getHexValue(hR.Value, hG.Value, hB.Value);
			pPallete.Invalidate();
		}

		private void nIndex_ValueChanged(object sender, EventArgs e)
		{
			m.openRom();
			m.loadPalette(m.lastLevel, (int)nIndex.Value+1);
			label5.Text = "Address: 0x" + (0x85520 + ((int)nIndex.Value * 32)).ToString("X");
			setColors();
			pPallete.Invalidate();
			m.closeRom();
		}
	}
}
