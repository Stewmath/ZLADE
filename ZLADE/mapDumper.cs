using System;
using System.Collections.Generic;
using System.IO;

namespace ZLADE
{
	public class mapDumper
	{
		public byte[] objectData;
		public int byteCount = 0;
		public byte animIndex = 0;
		public byte borderTileIndex = 0;
		public byte floorTileIndex = 0;
		public void writeFile(string filename)
		{
			BinaryWriter w = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate));
			byte animindex = animIndex;
			string template = borderTileIndex.ToString("X") + floorTileIndex.ToString("X");
			byte temp = (byte)Convert.ToInt32(template, 16);
			byte[] ttt = { animindex, temp };
			//w.Write(byteCount);
			w.Write(ttt);
			w.Write(objectData);
			w.Close();
		}

		public byte[] loadData(string filename)
		{
			BinaryReader r = new BinaryReader(File.Open(filename, FileMode.Open));
			//int count = (int)r.ReadByte();
			byte[] temp = r.ReadBytes((int)r.BaseStream.Length);
			r.Close();
			return temp;
		}
	}
}
