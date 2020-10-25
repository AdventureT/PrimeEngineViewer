using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PrimeWPF
{
    class ReadHelper
    {
        public static string ReadString()
        {
            var c = '\0';
            var result = String.Empty;
            while ((c = TRB._f.ReadChar()) != '\0')
            {
                result += c;
            }
            return result;
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
