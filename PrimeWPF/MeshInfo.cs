using System;
using System.Collections.Generic;
using System.Text;

namespace PrimeWPF
{
    public class MeshInfo
    {
        public uint CurrentPMDLOffset { get; set; }
        public ushort[] UK { get; set; }
        public uint[] UK2 { get; set; }
        public uint VertexCount { get; set; }
        public uint UK3 { get; set; }
        public uint PreviousFaceCount { get; set; }
        public uint FaceCount { get; set; }
        public uint[] UK4 { get; set; }
        public uint VertexOffsetRelative { get; set; }
        public uint NormalUVOffset { get; set; }
        public uint FaceOffset { get; set; }
        public uint[] SameSizeorOffset { get; set; }

        public MeshInfo()
        {
            CurrentPMDLOffset = TRB._f.ReadUInt32();
            UK = new ushort[] { TRB._f.ReadUInt16(), TRB._f.ReadUInt16() };
            UK2 = new uint[] { TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32() };
            VertexCount = TRB._f.ReadUInt32();
            UK3 = TRB._f.ReadUInt32();
            PreviousFaceCount = TRB._f.ReadUInt32();
            FaceCount = TRB._f.ReadUInt32();
            UK4 = new uint[] { TRB._f.ReadUInt32(), TRB._f.ReadUInt32() };
            VertexOffsetRelative = TRB._f.ReadUInt32();
            NormalUVOffset = TRB._f.ReadUInt32();
            FaceOffset = TRB._f.ReadUInt32();
            SameSizeorOffset = new uint[] { TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32() };
        }
    }
}
