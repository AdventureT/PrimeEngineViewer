using Aspose.ThreeD.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWPF
{
    public class PCOL : Tag
    {
        public uint CollisionCount { get; set; }
        public uint CollsionInfo { get; set; }
        public List<CollisionInfo> CollsionInfos { get; set; } = new();
        public byte[] CollisionFile { get; set; }
        public uint CollisionFileSize { get; set; }
        public List<List<Vector4>> Vertices { get; set; } = new();
        public List<List<uint>> Faces { get; set; } = new();

        public PCOL()
        {
            CollisionCount = TRB._f.ReadUInt32();
            CollsionInfo = TRB._f.ReadUInt32();
            var collsionFileOffset = TRB._f.ReadUInt32();
            CollisionFileSize = TRB._f.ReadUInt32();
            CollisionFile = ReadHelper.ReadFromOffset(CollisionFileSize, collsionFileOffset + TRB.sections[1].SectionOffset);
            TRB._f.BaseStream.Seek(CollsionInfo + TRB.sections[1].SectionOffset, System.IO.SeekOrigin.Begin);
            for (int i = 0; i < CollisionCount; i++)
            {
                CollsionInfos.Add(new CollisionInfo());
            }
            for (int i = 0; i < CollisionCount; i++)
            {
                TRB._f.BaseStream.Seek(CollsionInfos[i].VertexOffset + TRB.sections[1].SectionOffset, System.IO.SeekOrigin.Begin);
                Vertices.Add(new());
                for (int j = 0; j < CollsionInfos[i].VertexCount; j++)
                {

                    Vertices[i].Add(new Vector4(TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle()));
                }
                TRB._f.BaseStream.Seek(CollsionInfos[i].FaceOffset + TRB.sections[1].SectionOffset, System.IO.SeekOrigin.Begin);
                Faces.Add(new());
                for (int j = 0; j < CollsionInfos[i].FaceCount; j++)
                {

                    Faces[i].AddRange(new uint[] { TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32() });
                }
            }
        }
    }
}
