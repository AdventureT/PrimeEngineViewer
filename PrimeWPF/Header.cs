using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Serilog;

namespace PrimeWPF
{
    class Header
    {
        public string Signature { get; set; }
        public uint Version { get; set; }
        public ushort Flag1 { get; set; }
        public ushort Flag2 { get; set; }
        public uint DataInfoCount { get; set; }
        public uint DataInfoSize { get; set; }
        public uint TagCount { get; set; }
        public uint TagSize { get; set; }
        public uint RelocationDataOffset { get; set; }
        public uint RelocationDataSize { get; set; }

        public Header(BinaryReader f)
        {
            Signature = new string(f.ReadChars(4));
            Version = f.ReadUInt32();
            Flag1 = f.ReadUInt16();
            Flag2 = f.ReadUInt16();
            DataInfoCount = f.ReadUInt32();
            DataInfoSize = f.ReadUInt32();
            TagCount = f.ReadUInt32();
            TagSize = f.ReadUInt32();
            RelocationDataOffset = f.ReadUInt32();
            RelocationDataSize = f.ReadUInt32();
            Log.Information(Signature);
        }
    }
}
