using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ZLADE
{
	class IndoorTilesetLoader
	{
		BinaryReader reader = null;
		int dungeonId = 0;
		int mapId = 0;
		bool sideview = false;
		bool special = false;

		byte[, ,] mapTiles1 = new byte[16 * 1, 8, 8];       // 0x00 - 0x0F
		byte[, ,] mapTiles2 = new byte[16 * 1, 8, 8];       // 0x10 - 0x1F
		byte[, ,] mapTiles3 = new byte[16 * 6, 8, 8];       // 0x20 - 0x7F
		byte[, ,] mapTiles4 = new byte[16 * 1, 8, 8];       // 0xF0 - 0xFF
		byte[, ,] mapTiles5 = new byte[4, 8, 8];

		public int GetPalOffset()
		{
			if (!sideview)
			{
				int specOffset = getOffsetPointer(0x8518E);
				for (int i = 0; i < readByte(0x8518C); ++i)
				{
					if (readByte(specOffset) == dungeonId && (dungeonId < 9 || readByte(specOffset + 1) == mapId))
					{
						int off = getOffsetPointer(0x851B5);
						return getOffsetPointer(off + (readByte(specOffset + 3) & 0x3F) * 2);
					}
					else
						specOffset += 4;
				}
			}

			if (dungeonId == 0xFF)
			{
				return 0x867D0;
			}
			if (dungeonId < 0x09)
			{
				return getOffsetPointer(0x84401 + (dungeonId * 2));
			}
			else
			{
				int off = getOffsetPointer(0x84413 + (dungeonId - 0xA) * 2);
				int pal = readByte(off + mapId);
				return getOffsetPointer(0x8443F + pal * 2);
			}

		}

		int getInt16(byte b1, byte b2)
		{
			return b1 + (b2 << 8);
		}

		int getOffsetPointer(int offset)
		{
			return getPointerAddress((byte)(offset / 0x4000), readByte(offset), readByte(offset + 1));
		}

		int getOffsetPointer(int offset, int bank)
		{
			return getPointerAddress((byte)bank, readByte(offset), readByte(offset + 1));
		}

		public void ReadMapPal()
		{
			int offset = GetPalOffset();
			ReadPal(offset, ref mapPal, 0, 8);
			int add = (int)getPointerAddress(0x84000 / 0x4000, readByte(0x841E5), readByte(0x841E6));
			ReadPal(0x841E5, ref mapPal, 8, 6);
			ReadPal(offset + 0x40, ref mapPal, 14, 2);
		}

		public void ReadPal(int offset, ref Color[,] target, int start, int count)
		{
			for (int i = 0; i < count; ++i)
			{
				for (int j = 0; j < 4; ++j)
				{
					target[i + start, j] = GetColor(offset);
					offset = offset + 2;
				}
			}
		}
		public Color GetColor(int offset)
		{
			int value = readByte(offset) + (readByte(offset + 1) << 8);
			UInt16 color2B = (UInt16)value;
			int red = (color2B & 31) << 3;
			color2B >>= 5;
			int green = (color2B & 31) << 3;
			color2B >>= 5;
			int blue = (color2B & 31) << 3;
			return Color.FromArgb(red, green, blue);
		}

		public void ReadTiles(int width, int height, int offset, ref byte[, ,] target)
		{
			ReadTiles(width, height, 0, offset, ref target);
		}
		public void ReadTiles(int width, int height, int yOff, int offset, ref byte[, ,] target)
		{
			int curOffset = offset;
			for (int j = 0; j < height; ++j)
			{
				if (j + yOff > 15)
					yOff -= 16;
				for (int i = 0; i < width; ++i)
				{
					for (int y2 = 0; y2 < 8; ++y2)
					{
						byte byte1 = readByte(curOffset++);
						byte byte2 = readByte(curOffset++);
						for (int x2 = 7; x2 >= 0; --x2)
						{
							target[i + (j + yOff) * width, x2, y2] = (byte)((byte1 & 1) + (byte2 & 1) * 2);
							byte1 >>= 1;
							byte2 >>= 1;
						}
					}
				}
			}
		}

		public byte readByte(int address)
		{
			reader.BaseStream.Position = address;
			return reader.ReadByte();
		}
		public byte[] readBytes(int address, int count)
		{
			reader.BaseStream.Position = address;
			return reader.ReadBytes(count);
		}

		public int GetBlockFlipOffset()
		{
			if (dungeonId == 0xFF)
				return 0x22 * 0x4000 + 0x6000;

			if (dungeonId < 0x09)
			{
				return getOffsetPointer(0x6A076 + dungeonId * 2, 0x23);
			}
			else
			{
				return getOffsetPointer(0x6A276 + dungeonId * 2, 0x24);
			}
		}

		public int getPointerAddress(byte bank, byte byte1, byte byte2)
		{
			byte2 -= 0x40;
			string b1 = byte1.ToString("X");
			if (b1.Length == 1)
				b1 = "0" + b1;
			string s = byte2.ToString("X") + b1;
			int l1 = Convert.ToInt32(s, 16);
			int l = (bank * 0x4000) + l1;
			string s1 = l.ToString("X");
			return l;
		}

		public int GetBlockTileOffset()
		{
			if (dungeonId == 0xFF || (dungeonId == 0x10 && mapId == 0xB5))
				return 0x20760;
			else
				return 0x203B0;
		}

		private int GetMapOffset()
		{
			if (special)
			{
				if (dungeonId == 0x1F && mapId == 0xF5)
					return 0xA * 0x4000 + 0x7855 - 0x4000;
			}

			if (dungeonId == 0xFF)
			{
				getOffsetPointer(0x2BB77 + mapId * 2);
			}

			int bank;
			if (dungeonId < 6 || dungeonId >= 0x1A)
				bank = 0xA;
			else
				bank = 0xB;
			return getOffsetPointer(bank * 0x4000 + mapId * 2);
		}

		public void ReadMapTiles()
		{
			if (dungeonId == 0xFF)
			{
				//0x00-0x0F
				ReadTiles(16, 1, 0xB5000 + readByte(OffsetLoader.activeOffset.iTileLoc + mapId) * 0x100, ref mapTiles1);

				//0x10-0x1F
				ReadTiles(16, 1, 0xD6000, ref mapTiles2);

				//0x20-0x7F
				ReadTiles(16, 6, 0x34000, ref mapTiles3);
				ReadTiles(16, 2, readByte(0x805C9) * 0x100 + 0x2C * 0x4000, ref mapTiles3);

				//0x6C-6F
				//TODO: check if correct offset
				ReadTiles(4, 1, 0xB2D00, ref mapTiles5);

				//0xF0-0xFF
				ReadTiles(16, 1, 0xD6100, ref mapTiles4);

				return;
			}

			if (!sideview)
			{
				//0x00-0x0F
				ReadTiles(16, 1, 0xB5000 + readByte(((dungeonId < 6 || dungeonId >= 0x1A) ? OffsetLoader.activeOffset.iTileLoc2 : OffsetLoader.activeOffset.iTileLoc2 + 0x100) + mapId) * 0x100, ref mapTiles1);

				//0x10-0x1F
				//TODO: check when to use 0xC instead of 0x2C
				int offset = readByte(0x80589 + dungeonId) * 0x100 + 0x2C * 0x4000;
				ReadTiles(16, 1, offset, ref mapTiles2);

				//0x20-0x7F
				ReadTiles(16, 6, 0x34000, ref mapTiles3);
				ReadTiles(16, 2, readByte(0x805A9 + dungeonId) * 0x100 + 0x2C * 0x4000, ref mapTiles3);
			}
			else
			{

				int off;
				if ((dungeonId < 0x0A || mapId == 0xE9) && dungeonId != 0x06)
					off = 0xB7800;
				else
					off = 0xB7000;

				ReadTiles(16, 1, off, ref mapTiles1);
				ReadTiles(16, 1, off + 0x100, ref mapTiles2);
				ReadTiles(16, 6, off + 0x200, ref mapTiles3);
			}

			//0x6C-6F
			int anim = readByte(GetMapOffset());
			int routOff = getFirstPointer(readByte(0x1BD5 + anim * 2), readByte(0x1BD5 + 1 + anim * 2));
			while (readByte(routOff) != 0x26)
			{
				routOff += 1;
			}
			ReadTiles(4, 1, 0xB0000 + readByte(routOff + 1) * 0x100 - 0x4000, ref mapTiles5);

			//0xF0-0xFF
			//TODO: check when to use 0x11 instead of 0x31
			int offset2 = readByte(0x805CA + dungeonId) * 0x100 + 0x31 * 0x4000;
			ReadTiles(16, 1, offset2, ref mapTiles4);
		}

		public void ReadMapTiles(int animation)
		{
			if (dungeonId == 0xFF)
			{
				//0x00-0x0F
				ReadTiles(16, 1, 0xB5000 + readByte(0x830B7 + mapId) * 0x100, ref mapTiles1);

				//0x10-0x1F
				ReadTiles(16, 1, 0xD6000, ref mapTiles2);

				//0x20-0x7F
				ReadTiles(16, 6, 0x34000, ref mapTiles3);
				ReadTiles(16, 2, readByte(0x805C9) * 0x100 + 0x2C * 0x4000, ref mapTiles3);

				//0x6C-6F
				//TODO: check if correct offset
				ReadTiles(4, 1, 0xB2D00, ref mapTiles5);

				//0xF0-0xFF
				ReadTiles(16, 1, 0xD6100, ref mapTiles4);

				return;
			}

			if (!sideview)
			{
				//0x00-0x0F
				ReadTiles(16, 1, 0xB5000 + readByte(((dungeonId < 6 || dungeonId >= 0x1A) ? 0x82EB3 : 0x82FB3) + mapId) * 0x100, ref mapTiles1);

				//0x10-0x1F
				//TODO: check when to use 0xC instead of 0x2C
				int offset = readByte(0x80589 + dungeonId) * 0x100 + 0x2C * 0x4000;
				ReadTiles(16, 1, offset, ref mapTiles2);

				//0x20-0x7F
				ReadTiles(16, 6, 0x34000, ref mapTiles3);
				ReadTiles(16, 2, readByte(0x805A9 + dungeonId) * 0x100 + 0x2C * 0x4000, ref mapTiles3);
			}
			else
			{

				int off;
				if ((dungeonId < 0x0A || mapId == 0xE9) && dungeonId != 0x06)
					off = 0xB7800;
				else
					off = 0xB7000;

				ReadTiles(16, 1, off, ref mapTiles1);
				ReadTiles(16, 1, off + 0x100, ref mapTiles2);
				ReadTiles(16, 6, off + 0x200, ref mapTiles3);
			}

			//0x6C-6F
			int anim = animation;//readByte(GetMapOffset());
			int routOff = getFirstPointer(readByte(OffsetLoader.activeOffset.tileAnim + anim * 2), readByte(OffsetLoader.activeOffset.tileAnim + 1 + anim * 2));
			while (readByte(routOff) != 0x26)
			{
				routOff += 1;
			}
			byte b = readByte(routOff + 1);
			ReadTiles(4, 1, 0xB0000 + b * 0x100 - 0x4000, ref mapTiles5);

			//0xF0-0xFF
			//TODO: check when to use 0x11 instead of 0x31
			int offset2 = readByte(0x805CA + dungeonId) * 0x100 + 0x31 * 0x4000;
			ReadTiles(16, 1, offset2, ref mapTiles4);
		}

		public int getFirstPointer(byte b1, byte b2)
		{
			return b1 + b2 * 0x100;
		}

		protected struct Block
		{
			public byte[] tile;
			public byte[] flipPal;
		}
		protected Block[] blockdata = new Block[256];
		public Color[,] mapPal = new Color[16, 4];

		public void ReadBlockData()
		{
			int offset = GetBlockFlipOffset();
			int tileOffset = GetBlockTileOffset();
			for (int i = 0; i < 256; ++i)
			{
				blockdata[i].tile = new byte[4];
				blockdata[i].flipPal = new byte[4];
				for (int j = 0; j < 4; ++j)
				{
					blockdata[i].tile[j] = readByte(tileOffset++);
					blockdata[i].flipPal[j] = readByte(offset++);
				}
			}
		}

		public void DrawBlockData()
		{
			for (int i = 0; i < 256; ++i)
			{
				for (int x1 = 0; x1 < 2; ++x1)
				{
					for (int y1 = 0; y1 < 2; ++y1)
					{
						byte tile = blockdata[i].tile[y1 * 2 + x1];
						byte pal = (byte)(blockdata[i].flipPal[y1 * 2 + x1] & 0xF);
						byte flip = (byte)(blockdata[i].flipPal[y1 * 2 + x1] & 0xF0);
						int col = i % 16;
						int line = i / 16;
						for (int y2 = 0; y2 < 8; ++y2)
						{
							for (int x2 = 0; x2 < 8; ++x2)
							{
								int sourceX = x2;
								int sourceY = y2;
								if ((flip & 0x20) != 0)
									sourceX = 7 - sourceX;
								if ((flip & 0x40) != 0)
									sourceY = 7 - sourceY;
								if (tile < 0x10)
									blockBmp.SetPixel(col * 16 + x1 * 8 + x2, line * 16 + y1 * 8 + y2, mapPal[pal, mapTiles1[tile, sourceX, sourceY]]);
								else if (tile < 0x20)
									blockBmp.SetPixel(col * 16 + x1 * 8 + x2, line * 16 + y1 * 8 + y2, mapPal[pal, mapTiles2[tile - 0x10, sourceX, sourceY]]);
								else if (tile < 0x80)
								{
									if (tile < 0x6C || tile > 0x6F)
										blockBmp.SetPixel(col * 16 + x1 * 8 + x2, line * 16 + y1 * 8 + y2, mapPal[pal, mapTiles3[tile - 0x20, sourceX, sourceY]]);
									else
										blockBmp.SetPixel(col * 16 + x1 * 8 + x2, line * 16 + y1 * 8 + y2, mapPal[pal, mapTiles5[tile - 0x6C, sourceX, sourceY]]);
								}
								else
									blockBmp.SetPixel(col * 16 + x1 * 8 + x2, line * 16 + y1 * 8 + y2, mapPal[pal, mapTiles4[tile & 0xF, sourceX, sourceY]]);
							}
						}
					}
				}
			}
		}

		public void loadMap(BinaryReader b, int dungeon, int map, bool spec, bool side)
		{
			reader = b;
			dungeonId = dungeon;
			mapId = map;
			special = spec;
			sideview = side;
			ReadMapPal();
			ReadBlockData();
			ReadMapTiles();
			DrawBlockData();
		}

		public void loadMap(BinaryReader b, int dungeon, int map, bool spec, bool side, int animation)
		{
			reader = b;
			dungeonId = dungeon;
			mapId = map;
			special = spec;
			sideview = side;
			ReadMapPal();
			ReadBlockData();
			ReadMapTiles(animation);
			DrawBlockData();
		}

		public Bitmap blockBmp = new Bitmap(256, 256);
	}
}