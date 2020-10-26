using System;
using System.Collections.Generic;
using System.Text;

namespace PrimeWPF
{
    class Tag
    {
        public string Name { get; set; }
        public uint Zero { get; set; }
        public ushort RelocationDataCount { get; set; }
        public ushort Size { get; set; }
        public uint Zero2 { get; set; }
        public string FullName { get; set; }
        public uint Hash { get; set; }
        public uint Zero3 { get; set; }
        public uint Zero4 { get; set; }

        public Tag()
        {
            Name = new string(TRB._f.ReadChars(4));
            Zero = TRB._f.ReadUInt32();
            RelocationDataCount = TRB._f.ReadUInt16();
            Size = TRB._f.ReadUInt16();
            Zero2 = TRB._f.ReadUInt32();
            FullName = ReadHelper.ReadStringFromOffset(TRB._f.ReadUInt32() + TRB.sections[0].SectionOffset);
            Hash = TRB._f.ReadUInt32();
            Zero3 = TRB._f.ReadUInt32();
            Zero4 = TRB._f.ReadUInt32();
        }
    }
}
