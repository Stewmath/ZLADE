using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZLADE
{
	public partial class frmEventPos : Form
	{
		public byte chestX = 0;
		public byte chestY = 0;
		public byte stairsX = 0;
		public byte stairsY = 0;
		public byte keyX = 0;
		public byte keyY = 0;
		public Image imgT1;
		MapLoader m;
		int sel = 0;
		int mouse = 0;

		public frmEventPos(Image i, MapLoader l)
		{
			InitializeComponent();
			imgT1 = i;
			m = l;
			chestX = m.chestX;
			chestY = m.chestY;
			stairsX = m.stairsX;
			stairsY = m.stairsY;
			keyX = m.keyX;
			keyY = m.keyY;
		}

		private void frmEventPos_Load(object sender, EventArgs e)
		{
			
		}

		Bitmap getTile(int id)
		{
			char[] c = id.ToString("X").ToCharArray();
			int y = 0;
			int x = 0;
			if (c.Length > 1)
			{
				y = Convert.ToInt32(c[0].ToString(), 16);
				x = Convert.ToInt32(c[1].ToString(), 16);
			}
			else
			{
				x = Convert.ToInt32(c[0].ToString(), 16);
			}

			Bitmap b = new Bitmap(16, 16);
			Graphics g = Graphics.FromImage(b);
			Rectangle r = new Rectangle(0, 0, 16, 16);
			g.DrawImage(imgT1, r, x * 16, y * 16, 16, 16, GraphicsUnit.Pixel);
			return b;
		}

		private void pMap_Paint(object sender, PaintEventArgs e)
		{
			for (int x = 0; x < 10; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					e.Graphics.DrawImage(getTile(0xD), x * 16, y * 16);
				}
			}
			e.Graphics.DrawImage(getTile(0xA0), chestX, chestY);
			e.Graphics.DrawImage(getTile(0xBE), stairsX, stairsY);
			//e.Graphics.DrawImage(getTile(0xF), keyX, keyY);
			e.Graphics.FillRectangle(Brushes.Black, keyX, keyY, 4, 16);
			if (sel == 0)
				e.Graphics.DrawRectangle(Pens.Red, chestX, chestY, 16, 16);
			else if(sel == 1)
				e.Graphics.DrawRectangle(Pens.Red, stairsX, stairsY, 16, 16);
			else if(sel == 2)
				e.Graphics.DrawRectangle(Pens.Red, keyX, keyY, 16, 16);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			m.chestX = chestX;
			m.chestY = chestY;
			m.stairsX = stairsX;
			m.stairsY = stairsY;
			m.keyX = keyX;
			m.keyY = keyY;
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			mouse = 0;
			button1.BackColor = SystemColors.ButtonHighlight;
			button2.BackColor = SystemColors.Control;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			mouse = 1;
			button2.BackColor = SystemColors.ButtonHighlight;
			button1.BackColor = SystemColors.Control;
		}

		private void pMap_MouseDown(object sender, MouseEventArgs e)
		{
			int x = (e.X / 16) * 16;
			int y = (e.Y / 16) * 16;
			int x2 = (e.X / 8) * 8;
			int y2 = (e.Y / 8) * 8;
			if (mouse == 0)
			{
				if (x == (keyX / 16) * 16 && y == (keyY / 16) * 16)
					sel = 2;
				else if (x == (stairsX / 16) * 16 && y == (stairsY / 16) * 16)
					sel = 1;
				else if (x == (chestX / 16) * 16 && y == (chestY / 16) * 16)
					sel = 0;
			}
			else
			{
				if (sel == 0)
				{
					chestX = (byte)x2;
					chestY = (byte)y2;
				}
				else if (sel == 1)
				{
					stairsX = (byte)x2;
					stairsY = (byte)y2;
				}
				else if (sel == 2)
				{
					keyX = (byte)x2;
					keyY = (byte)y2;
				}
			}
			pMap.Invalidate();
		}
	}
}
