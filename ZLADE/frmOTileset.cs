using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZLADE
{
	public partial class frmOTileset : Form
	{
		Form1 frm;
		MapLoader loader;
		int id = 0;
		public frmOTileset(Form1 f, MapLoader m, int idd)
		{
			InitializeComponent();
			frm = f;
			loader = m;
			id = idd;
		}

		private byte[] getPointer(long address)
		{
			string a = address.ToString("X");
			string b1 = a.Substring(a.Length - 2, 2);
			string b2 = a.Substring(a.Length - 4, 2);
			int byte1 = Convert.ToInt32(b1, 16);
			int byte2 = Convert.ToInt32(b2, 16);
			while (byte2 > 0x7F)
				byte2 -= 0x40;
			while (byte2 < 0x40)
				byte2 += 0x40;
			byte[] b = { (byte)byte1, (byte)byte2 };
			return b;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			loader.writer = new System.IO.BinaryWriter(System.IO.File.Open(loader.fname, System.IO.FileMode.Open));
			loader.writer.BaseStream.Position = 0x842EF + id;
			loader.writer.Write((byte)nPalette.Value);
			int line = id / 32;
			int loc = 0x82E73 + (line * 8 + (id % 16) / 2);
			loader.writer.BaseStream.Position = loc;
			loader.writer.Write((byte)nPrimary.Value);
			byte[] pointer = getPointer((long)nAddress.Value);
			byte bank = (byte)(nAddress.Value / 0x4000);
			loader.writer.BaseStream.Position = 0x6A476 + id;
			loader.writer.Write(bank);
			loader.writer.BaseStream.Position = 0x69E76 + (id * 2);
			loader.writer.Write(pointer);
			loader.writer.Close();
			this.Close();
			frm.pOMap.Invalidate();
		}

		private void nPalette_ValueChanged(object sender, EventArgs e)
		{
			loader.openRom();
			frm.tloader.readMap(loader.reader, id, (int)nPrimary.Value, (int)nPalette.Value, (int)nAddress.Value);
			pTileset.Image = frm.tloader.blockBmp;
			loader.closeRom();
		}

		private void nPrimary_ValueChanged(object sender, EventArgs e)
		{
			loader.openRom();
			frm.tloader.readMap(loader.reader, id, (int)nPrimary.Value, (int)nPalette.Value, (int)nAddress.Value);
			pTileset.Image = frm.tloader.blockBmp;
			loader.closeRom();
		}

		private void nAddress_ValueChanged(object sender, EventArgs e)
		{
			loader.openRom();
			frm.tloader.readMap(loader.reader, id, (int)nPrimary.Value, (int)nPalette.Value, (int)nAddress.Value);
			pTileset.Image = frm.tloader.blockBmp;
			loader.closeRom();
		}
	}
}
