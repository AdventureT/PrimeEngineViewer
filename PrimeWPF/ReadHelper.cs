using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PrimeWPF
{
    class ReadHelper
    {
        public static string ReadString(BinaryReader f)
        {
            var c = '\0';
            var result = String.Empty;
            while ((c = f.ReadChar()) != '\0')
            {
                result += c;
            }
            return result;
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
