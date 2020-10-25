using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public Section(BinaryReader f)
        {
            Unknown = f.ReadUInt32();
            var tempTextOffset = f.ReadUInt32();
            Uk2 = f.ReadUInt32();
            Uk3 = f.ReadUInt32();
            SectionSize = f.ReadUInt32();
            SectionSize2 = f.ReadUInt32();
            SectionOffset = f.ReadUInt32();
            Uk4 = f.ReadUInt32();
            Padding = new uint[4] { f.ReadUInt32(), f.ReadUInt32(), f.ReadUInt32(), f.ReadUInt32() };
            TextOffset = ReadHelper.ReadStringFromOffset(f, tempTextOffset + SectionOffset);
            Log.Information($"Section {TextOffset} has a size of {SectionSize}");
        }

        public Section(BinaryReader f, uint textOffset)
        {
            Unknown = f.ReadUInt32();
            TextOffset = ReadHelper.ReadStringFromOffset(f, f.ReadUInt32() + textOffset);
            Uk2 = f.ReadUInt32();
            Uk3 = f.ReadUInt32();
            SectionSize = f.ReadUInt32();
            SectionSize2 = f.ReadUInt32();
            SectionOffset = f.ReadUInt32();
            Uk4 = f.ReadUInt32();
            Padding = new uint[4] { f.ReadUInt32(), f.ReadUInt32(), f.ReadUInt32(), f.ReadUInt32() };
            Log.Information($"Section {TextOffset} has a size of {SectionSize}");
        }
    }
}
