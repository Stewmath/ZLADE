using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ZLADE
{
	public partial class frmRepoint : Form
	{
		MapLoader loader;
		public int room = 0;
		public int level = 1;
		public frmRepoint(MapLoader m)
		{
			InitializeComponent();
			nAddress.Maximum = 0x100000;
			loader = m;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void frmRepoint_Load(object sender, EventArgs e)
		{

		}

		private byte[] getPointer(long address)
		{
			string a = address.ToString("X");
			string b1 = a.Substring(a.Length - 2, 2);
			string b2 = a.Substring(a.Length - 4, 2);
			int byte1 = Convert.ToInt32(b1, 16);
			int byte2 = Convert.ToInt32(b2, 16);
			if(byte2 >= 0x40)
				byte2 -= 0x40;
			byte[] b = { (byte)byte1, (byte)byte2 };
			return b;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			loader.dataLocation = (long)nAddress.Value;
			loader.pointerValues = getPointer(loader.dataLocation);
			loader.writePointer(level, room);
			if (cCopy.Checked)
			{
				loader.saveRoomData();
			}
			this.Close();
		}
	}
}
