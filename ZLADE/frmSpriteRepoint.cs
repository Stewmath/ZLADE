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
		public bool indoor = false;
		Form1 frm;
		public frmSpriteRepoint(MapLoader l, Form1 f)
		{
			InitializeComponent();
			m = l;
			nAddress.Maximum = 0x100000;
			frm = f;
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
			while (byte2 > 0x7F)
				byte2 -= 0x40;
			while (byte2 < 0x40)
				byte2 += 0x40;
			byte[] b = { (byte)byte1, (byte)byte2 };
			return b;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (level != -1)
			{
				if (!indoor)
				{
					m.spriteLocation = (long)nAddress.Value;
					m.spritePointer = getPointer(m.spriteLocation);
					m.writeSpritePointer(level, room);
					if (cCopy.Checked)
					{
						m.saveRoomData();
					}
					m.openRom();
					m.sprites = new List<Sprite>();
					m.readSprites(level, room + (level > 6 ? 0x100 : 0));
					m.closeRom();
					frm.pMap.Invalidate();
				}
				else
				{
					m.iSpriteLocation = (long)nAddress.Value;
					byte[] p = getPointer((long)nAddress.Value);
					m.writeISpritePointer(level, room, p[0], p[1]);
					if (cCopy.Checked)
					{
						m.saveIRoomData();
					}
					frm.loadIndoorRoom(level, room);
					frm.pIMap.Invalidate();
				}
			}
			else
			{
				m.oSpriteLocation = (long)nAddress.Value;
				byte[] pp = getPointer((long)nAddress.Value);
				m.writeOSpritePointer(pp[0], pp[1]);
				if (cCopy.Checked)
				{
					m.saveOverworld();
				}
				frm.loadOverworldMap(room);
				frm.pOMap.Invalidate();
			}
			this.Close();
		}

		private void frmSpriteRepoint_Load(object sender, EventArgs e)
		{

		}
	}
}
