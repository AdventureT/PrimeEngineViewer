using Serilog;
using System;
using System.IO;

namespace PrimeWPF
{
    class Section
    {
        public uint Unknown { get; set; }
        public string TextOffset { get; set; }
        public uint Uk2 { get; set; }
        public uint Uk3 { get; set; }
        public uint SectionSize { get; set; }
        public uint SectionSize2 { get; set; }
        public uint SectionOffset { get; set; }
        public uint Uk4 { get; set; }
        public uint[] Padding { get; set; }

        public Section()
        {
            Unknown = TRB._f.ReadUInt32();
            var tempTextOffset = TRB._f.ReadUInt32();
            Uk2 = TRB._f.ReadUInt32();
            Uk3 = TRB._f.ReadUInt32();
            SectionSize = TRB._f.ReadUInt32();
            SectionSize2 = TRB._f.ReadUInt32();
            SectionOffset = TRB._f.ReadUInt32();
            Uk4 = TRB._f.ReadUInt32();
            Padding = new uint[4] { TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32() };
            TextOffset = ReadHelper.ReadStringFromOffset(tempTextOffset + SectionOffset);
            Log.Information($"Section {TextOffset} has a size of {SectionSize}");
        }

        public Section(uint textOffset)
        {
            Unknown = TRB._f.ReadUInt32();
            TextOffset = ReadHelper.ReadStringFromOffset(TRB._f.ReadUInt32() + textOffset);
            Uk2 = TRB._f.ReadUInt32();
            Uk3 = TRB._f.ReadUInt32();
            SectionSize = TRB._f.ReadUInt32();
            SectionSize2 = TRB._f.ReadUInt32();
            SectionOffset = TRB._f.ReadUInt32();
            Uk4 = TRB._f.ReadUInt32();
            Padding = new uint[4] { TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32() };
            Log.Information($"Section {TextOffset} has a size of {SectionSize}");
        }
    }
}
