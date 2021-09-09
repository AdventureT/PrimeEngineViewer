using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWPF
{
    class ReversedTagInfo
    {
        public uint Offset { get; set; }
        public string FullName { get; set; }
        public int Hash { get; set; }
        public string Name { get; set; }

        public ReversedTagInfo()
        {
            Offset = TRB._f.ReadUInt32();
            FullName = ReadHelper.ReadStringFromOffset(TRB._f, TRB.sections[0].SectionOffset + TRB._f.ReadUInt32());
            Hash = TRB._f.ReadInt32();
            Name = new string(TRB._f.ReadChars(4));
        }
    }
}
