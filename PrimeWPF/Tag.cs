using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PrimeWPF
{
    class Tag
    {
        public string Name { get; set; }
        public uint Offset { get; set; }
        public uint Flag { get; set; }
        public string FullName { get; set; }

        public Tag(BinaryReader f)
        {
            Name = new string(f.ReadChars(4));
            Offset = f.ReadUInt32();
            Flag = f.ReadUInt32();
            FullName = ReadHelper.ReadStringFromOffset(f, TRB.sections[0].SectionOffset + f.ReadUInt32());
            Log.Information($"Tag name: {FullName}");
        }
    }
}
