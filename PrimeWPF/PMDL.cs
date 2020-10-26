using Aspose.ThreeD.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PrimeWPF
{
    class PMDL : Tag
    {
        public uint Unknown { get; set; }
        public uint Count { get; set; }
        public Vector4 Vector4Offset { get; set; }
        public PMDL() : base()
        {
            Unknown = TRB._f.ReadUInt32();
            Count = TRB._f.ReadUInt32();
            Vector4Offset = ReadHelper.ReadVector4FromOffset(TRB._f.ReadUInt32() + TRB.sections[1].SectionOffset);
            Console.WriteLine();
        }
    }
}
