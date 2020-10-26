using System.IO;
using System.Text;

namespace PrimeWPF
{
    class ReadHelper
    {
        public static string ReadString()
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

        public static string ReadStringFromOffset(uint offset)
        {
            var pos = TRB._f.BaseStream.Position;
            TRB._f.BaseStream.Seek(offset, SeekOrigin.Begin);
            var str = ReadString();
            TRB._f.BaseStream.Seek(pos, SeekOrigin.Begin);
            return str;
        }
    }
}
