using Aspose.ThreeD.Utilities;
using System;
using System.IO;
using System.Text;

namespace PrimeWPF
{
    class ReadHelper
    {
        public static long _lastPos = -1;
        public static string ReadString()
        {
            var sb = new StringBuilder();
            while (true) 
            {
                var newByte = TRB._f.ReadByte();
                if (newByte == 0) break;
                sb.Append((char)newByte);
            }
            return sb.ToString();
        }

        public static string ReadUnicodeString()
        {
            var sb = new StringBuilder();
            while (true)
            {
                var newByte = TRB._f.ReadByte();
                var newByte2 = TRB._f.ReadByte();
                if (newByte == 0 && newByte2 == 0) break;
                var convertedChar = Encoding.Unicode.GetString(new byte[] { newByte, newByte2 });
                sb.Append(convertedChar);
            }
            return sb.ToString();
        }

        public static string ReadStringFromOffset(uint offset)
        {
            var pos = TRB._f.BaseStream.Position;
            TRB._f.BaseStream.Seek(offset, SeekOrigin.Begin);
            string str;
            var intCheck = TRB._f.ReadUInt16();
            if (intCheck <= 0xFF)
            {
                TRB._f.BaseStream.Seek(-2, SeekOrigin.Current);
                str = ReadUnicodeString();
            }
            else
            {
                TRB._f.BaseStream.Seek(-2, SeekOrigin.Current);
                str = ReadString();
            }
            
            TRB._f.BaseStream.Seek(pos, SeekOrigin.Begin);
            return str;
        }

        public static byte[] ReadFromOffset(uint bytesToRead, uint offset)
        {
            var pos = TRB._f.BaseStream.Position;
            TRB._f.BaseStream.Seek(offset, SeekOrigin.Begin);
            var buffer = new byte[bytesToRead];
            var bytesRead = TRB._f.Read(buffer);
            TRB._f.BaseStream.Seek(pos, SeekOrigin.Begin);
            return buffer;
        }

        public static long SeekToOffset(uint offset)
        {
            _lastPos = TRB._f.BaseStream.Position;
            TRB._f.BaseStream.Seek(offset, SeekOrigin.Begin);
            return _lastPos;
        }

        public static void ReturnToOrginalPosition()
        {
            if (_lastPos != -1) TRB._f.BaseStream.Seek(_lastPos, SeekOrigin.Begin);
        }

        public static Vector4 ReadVector4() => new Vector4(TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle());
        public static Vector3 ReadVector3() => new Vector3(TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle());

        public static Vector4 ReadVector4FromOffset(uint offset)
        {
            var pos = TRB._f.BaseStream.Position;
            TRB._f.BaseStream.Seek(offset, SeekOrigin.Begin);
            var result = ReadVector4();
            TRB._f.BaseStream.Seek(pos, SeekOrigin.Begin);
            return result;
        }

        public static Vector3 ReadVector3FromOffset(uint offset)
        {
            var pos = TRB._f.BaseStream.Position;
            TRB._f.BaseStream.Seek(offset, SeekOrigin.Begin);
            var result = ReadVector3();
            TRB._f.BaseStream.Seek(pos, SeekOrigin.Begin);
            return result;
        }
    }
}
