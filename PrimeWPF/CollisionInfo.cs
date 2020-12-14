using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWPF
{
    public class CollisionInfo
    {
        public uint VertexOffset { get; set; }
        public uint FaceOffset { get; set; }
        public uint UnknownOffset { get; set; }
        public uint BoundingBoxOffset { get; set; }
        public uint VertexCount { get; set; }
        public uint FaceCount { get; set; }
        public uint UnknownCount { get; set; }
        public uint BoundingBoxCount { get; set; }

        public CollisionInfo()
        {
            VertexOffset = TRB._f.ReadUInt32();
            FaceOffset = TRB._f.ReadUInt32();
            UnknownOffset = TRB._f.ReadUInt32();
            BoundingBoxOffset = TRB._f.ReadUInt32();
            VertexCount = TRB._f.ReadUInt32();
            FaceCount = TRB._f.ReadUInt32();
            UnknownCount = TRB._f.ReadUInt32();
            BoundingBoxCount = TRB._f.ReadUInt32();
        }
    }
}
