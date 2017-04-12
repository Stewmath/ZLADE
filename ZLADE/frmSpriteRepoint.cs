using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZLADE
{
	public partial class frmSpriteRepoint : Form
	{
		MapLoader m;
		public int room = 0;
		public int level = 1;
		public frmSpriteRepoint(MapLoader l)
		{
			InitializeComponent();
			m = l;
			nAddress.Maximum = 0x100000;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private byte[] getPointer(long address)
		{
			string a = address.ToString("X");
			string b1 = a.Substring(a.Length - 2, 2);
			string b2 = a.Substring(a.Length - 4, 2);
			int byte1 = Convert.ToInt32(b1, 16);
			int byte2 = Convert.ToInt32(b2, 16);
			if (byte2 >= 0x40)
				byte2 -= 0x40;
			byte[] b = { (byte)byte1, (byte)byte2 };
			return b;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			m.spriteLocation = (long)nAddress.Value;
			m.spritePointer = getPointer(m.spriteLocation);
			m.writeSpritePointer(level, room);
			if (cCopy.Checked)
			{
				m.saveRoomData();
			}
			this.Close();
		}

		private void frmSpriteRepoint_Load(object sender, EventArgs e)
		{

		}
	}
}
