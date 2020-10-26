using System.IO;
using System.Text;

namespace PrimeWPF
{
    class ReadHelper
    {
        public static string ReadString(BinaryReader f)
        {
            var sb = new StringBuilder();
            while (true) 
            {
                var newByte = f.ReadByte();
                if (newByte == 0) break;
                sb.Append((char)newByte);
            }
            return sb.ToString();
        }

        public static string ReadStringFromOffset(BinaryReader f, uint offset)
        {
            var pos = f.BaseStream.Position;
            f.BaseStream.Seek(offset, SeekOrigin.Begin);
            var str = ReadString(f);
            f.BaseStream.Seek(pos, SeekOrigin.Begin);
            return str;
        }
    }
}
