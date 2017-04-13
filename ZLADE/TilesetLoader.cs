using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ZLADE
{
	public class TilesetLoader
	{
		byte[, ,] mapTiles1 = new byte[16 * 2, 8, 8];       // 0x00 - 0x1F
		byte[, ,] mapTiles2 = new byte[16 * 6, 8, 8];       // 0x20 - 0x7F
		byte[, ,] mapTiles3 = new byte[4, 8, 8];            // 0x6C - 0x6F
		byte[, ,] mapTiles4 = new byte[16 * 1, 8, 8];       // 0xF0 - 0xFF
		public int lastPal1 = 0;
		public int palAddress = 0;
		public int lastPrimary = 0;

		protected struct Block
		{
			public byte[] tile;
			public byte[] flipPal;
		}
		protected Block[] blockdata = new Block[256];

		public BinaryReader reader;
		public TilesetLoader()
		{
			
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

		public int GetBlockFlipOffset(int mapId)
		{
			int add = (int)getPointerAddress(readByte(0x6A476 + mapId), readByte(0x69E76 + (mapId * 2)), readByte(0x69E76 + 1 + (mapId * 2)));
			palAddress = add;
			return add;
		}

		public int GetBlockTileOffset()
		{
			return 0x6AB1D;
		}

		public int GetPalOffset(int mapId)
		{
			int pal = readByte(0x842EF + mapId);
			lastPal1 = pal;
			return (int)getPointerAddress(0x21, readByte(0x842B1 + (pal * 2)), readByte(0x842B2 + (pal * 2)));
		}
		public long getPointerAddress(byte bank, byte byte1, byte byte2)
		{
			byte2 -= 0x40;
			string b1 = byte1.ToString("X");
			if (b1.Length == 1)
				b1 = "0" + b1;
			string s = byte2.ToString("X") + b1;
			int l1 = Convert.ToInt32(s, 16);
			long l = (bank * 0x4000) + l1;
			string s1 = l.ToString("X");
			return l;
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

		public int getFirstPointer(byte b1, byte b2)
		{
			return b1 + b2 * 0x100;
		}

		public void ReadMapTiles(int mapId)
		{
			int line = mapId / 32;
			byte p = readByte(OffsetLoader.activeOffset.tile1Loc + (line * 8 + (mapId % 16) / 2));
			lastPrimary = p;
			ReadTiles(16, 2, 0xBC000 + p * 0x100, ref mapTiles1);
			ReadTiles(16, 6, 0xB1200, ref mapTiles2);
			byte pv1 = readByte(0x24000 + (mapId * 2));
			byte pv2 = readByte(0x24001 + (mapId * 2));
			long l = getPointerAddress(0x24000 / 0x4000, pv1, pv2);
			if (mapId > 0x7F)
			{
				int temp = (int)l - 0x24000;
				l = 0x68000 + temp;
			}
			byte anim = readByte((int)l);
			byte b1 = (byte)(readByte(OffsetLoader.activeOffset.tileAnim + (anim * 2)));
			byte b2 = (byte)(readByte(OffsetLoader.activeOffset.tileAnim + (anim * 2) + 1));
			int offset = getFirstPointer(b1, b2);
			byte b = readByte(offset + 1);
			offset = 0x2C000 + b * 0x100;

			ReadTiles(4, 1, offset, ref mapTiles3);
			ReadTiles(16, 1, 0xB0F00, ref mapTiles4);
		}

		public void ReadMapTiles(int mapId, int animation)
		{
			int line = mapId / 32;
			byte p = readByte(0x82E73 + (line * 8 + (mapId % 16) / 2));
			lastPrimary = p;
			ReadTiles(16, 2, 0xBC000 + p * 0x100, ref mapTiles1);
			ReadTiles(16, 6, 0xB1200, ref mapTiles2);
			byte pv1 = readByte(0x24000 + (mapId * 2));
			byte pv2 = readByte(0x24001 + (mapId * 2));
			byte anim = (byte)animation;
			byte b1 = (byte)(readByte(0x1BD5 + (anim * 2)));
			byte b2 = (byte)(readByte(0x1BD5 + (anim * 2) + 1));
			int offset = getFirstPointer(b1, b2);
			byte b = readByte(offset + 1);
			offset = 0x2C000 + b * 0x100;

			ReadTiles(4, 1, offset, ref mapTiles3);
			ReadTiles(16, 1, 0xB0F00, ref mapTiles4);
		}

		public void ReadMapTiles(int mapId, int pal, int primary)
		{
			int line = mapId / 32;
			ReadTiles(16, 2, 0xBC000 + primary * 0x100, ref mapTiles1);
			lastPrimary = primary;
			ReadTiles(16, 6, 0xB1200, ref mapTiles2);
			byte pv1 = readByte(0x24000 + (mapId * 2));
			byte pv2 = readByte(0x24001 + (mapId * 2));
			long l = getPointerAddress(0x24000 / 0x4000, pv1, pv2);
			if (mapId > 0x7F)
			{
				int temp = (int)l - 0x24000;
				l = 0x68000 + temp;
			}
			byte anim = readByte((int)l);
			byte b1 = (byte)(readByte(0x1BD5 + (anim * 2)));
			byte b2 = (byte)(readByte(0x1BD5 + (anim * 2) + 1));
			int offset = getFirstPointer(b1, b2);
			byte b = readByte(offset + 1);
			offset = 0x2C000 + b * 0x100;

			ReadTiles(4, 1, offset, ref mapTiles3);
			ReadTiles(16, 1, 0xB0F00, ref mapTiles4);
		}

		public void readMap(BinaryReader b, int map)
		{
			reader = b;
			ReadMapTiles(map);
			ReadMapPal(map);
			ReadBlockData(map);
			DrawBlockData();
		}

		public void readMap(BinaryReader b, int map, int anim)
		{
			reader = b;
			ReadMapTiles(map, anim);
			ReadMapPal(map);
			ReadBlockData(map);
			DrawBlockData();
		}

		public void readMap(BinaryReader b, int map, int primary, int pal, int padd)
		{
			reader = b;
			ReadMapTiles(map, pal, primary);
			ReadMapPal(map, pal);
			ReadBlockData(map, padd);
			DrawBlockData();
		}

		public Color[,] mapPal = new Color[16, 4];

		public void ReadPal(int offset, ref Color[,] target, int start, int count)
		{
			for (int i = 0; i < count; ++i)
			{
				for (int j = 0; j < 4; ++j)
				{
					string v = readByte(offset + 1).ToString("X") + readByte(offset).ToString("X");
					int value = Convert.ToInt32(v, 16);
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
		public void ReadMapPal(int mapId)
		{
			int offset = GetPalOffset(mapId);
			ReadPal(offset, ref mapPal, 0, 8);
			int add = (int)getPointerAddress(0x84000 / 0x4000, readByte(0x841E5), readByte(0x841E5 + 1));
			ReadPal(add, ref mapPal, 8, 6);
			ReadPal(offset + 0x40, ref mapPal, 14, 2);
		}
		public void ReadMapPal(int mapId, int pal)
		{
			int offset = (int)getPointerAddress(0x21, readByte(0x842B1 + (pal * 2)), readByte(0x842B2 + (pal * 2)));//GetPalOffset(mapId);
			ReadPal(offset, ref mapPal, 0, 8);
			int add = (int)getPointerAddress(0x84000 / 0x4000, readByte(0x841E5), readByte(0x841E5 + 1));
			ReadPal(add, ref mapPal, 8, 6);
			ReadPal(offset + 0x40, ref mapPal, 14, 2);
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
								if (tile < 0x20)
									blockBmp.SetPixel(col * 16 + x1 * 8 + x2, line * 16 + y1 * 8 + y2, mapPal[pal, mapTiles1[tile, sourceX, sourceY]]);
								else if (tile < 0x80)
									if (tile < 0x6C || tile > 0x6F)
										blockBmp.SetPixel(col * 16 + x1 * 8 + x2, line * 16 + y1 * 8 + y2, mapPal[pal, mapTiles2[tile - 0x20, sourceX, sourceY]]);
									else
										blockBmp.SetPixel(col * 16 + x1 * 8 + x2, line * 16 + y1 * 8 + y2, mapPal[pal, mapTiles3[tile - 0x6C, sourceX, sourceY]]);
								else
									blockBmp.SetPixel(col * 16 + x1 * 8 + x2, line * 16 + y1 * 8 + y2, mapPal[pal, mapTiles4[tile & 0xF, sourceX, sourceY]]);
							}
						}
					}
				}
			}
		}

		public void ReadBlockData(int mapId)
		{
			int offset = GetBlockFlipOffset(mapId);
			palAddress = offset;
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

		public void ReadBlockData(int mapId, int add)
		{
			palAddress = add;
			int offset = add;
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

		public Bitmap blockBmp = new Bitmap(256, 256);
	}
}