using Aspose.ThreeD;
using Aspose.ThreeD.Entities;
using Aspose.ThreeD.Utilities;
using System.Collections.Generic;
using System.IO;
using SystemHalf;

namespace PrimeWPF
{
    public class PMDL : Tag
    {
        public uint Unknown { get; set; }
        public uint Count { get; set; }
        public Vector4[] UnknownVector4s { get; set; }
        public Vector4[] UnknownVector4s2 { get; set; }
        public uint PZero { get; set; }
        public uint PZero2 { get; set; }
        public uint[] UnknownValues { get; set; }
        public List<MeshInfo> MeshInfos { get; set; }
        public uint MeshInfosCount { get; set; }
        public uint MeshInfoOffsetsOffset { get; set; }
        public List<uint> MeshInfoOffsets { get; set; }
        public uint VertexCount { get; private set; }
        public uint VertexBufferSize { get; private set; }
        public uint VertexBufferOffset { get; private set; }
        public uint PZero3 { get; private set; }
        public uint FaceCount { get; private set; }
        public uint FaceOffset { get; private set; }
        public uint PZero4 { get; private set; }
        public uint UnknownCount { get; private set; }
        public uint UnknownOffset { get; private set; }
        public Mesh[] MeshData { get; set; }
        public uint BoneCount { get; private set; }
        public uint BoneDataOffset { get; private set; }
        public uint BoneNamesOffset { get; private set; }
        public uint BoneParentsOffset { get; private set; }
        public uint ChunkCount { get; private set; }
        public uint ChunkOffset { get; private set; }
        public List<byte[]> Chunks { get; private set; }
        public List<List<Vector4>> Normals { get; private set; } = new();
        public List<List<Vector4>> Uvs { get; private set; } = new();

        ~PMDL()
        {
            MeshInfos.Clear();
            MeshInfoOffsets.Clear();
        }

        public PMDL() : base()
        {
            Unknown = TRB._f.ReadUInt32();
            Count = TRB._f.ReadUInt32();
            var headerPos = TRB._f.BaseStream.Position;
            TRB._f.BaseStream.Seek(TRB._f.ReadUInt32() + TRB.sections[1].SectionOffset, SeekOrigin.Begin);
            UnknownVector4s = new Vector4[] { ReadHelper.ReadVector4(TRB._f), ReadHelper.ReadVector4(TRB._f) };
            MeshInfosCount = TRB._f.ReadUInt32();
            MeshInfoOffsetsOffset = TRB._f.ReadUInt32();
            //ReadHelper.SeekToOffset(TRB._f.ReadUInt32() + TRB.sections[1].SectionOffset);
            //UnknownVector4s2 = new Vector4[] { ReadHelper.ReadVector4(), ReadHelper.ReadVector4() };
            //ReadHelper.ReturnToOrginalPosition();
            //TRB._f.BaseStream.Seek(TRB._f.ReadUInt32() + TRB.sections[1].SectionOffset, SeekOrigin.Begin);
            //ChunkCount = TRB._f.ReadUInt32();
            //ChunkOffset = TRB._f.ReadUInt32();
            //TRB._f.BaseStream.Seek(ChunkOffset + TRB.sections[1].SectionOffset, SeekOrigin.Begin);
            //Chunks = new List<byte[]>((int)ChunkCount);
            //Chunks.ForEach(x => x = TRB._f.ReadBytes(400)); //Chunks are 400 bytes long but no clue what they are for.. 
            //TRB._f.BaseStream.Seek(TRB._f.ReadUInt32() + TRB.sections[1].SectionOffset, SeekOrigin.Begin);
            //UnknownValues = new uint[] { TRB._f.ReadUInt32(), TRB._f.ReadUInt32() };
            TRB._f.BaseStream.Seek(MeshInfoOffsetsOffset + TRB.sections[1].SectionOffset, SeekOrigin.Begin);
            MeshInfoOffsets = new List<uint>();
            for (int i = 0; i < MeshInfosCount; i++)
            {
                MeshInfoOffsets.Add(TRB._f.ReadUInt32());
            }
            MeshInfos = new List<MeshInfo>();
            for (int i = 0; i < MeshInfosCount; i++)
            {
                TRB._f.BaseStream.Seek(MeshInfoOffsets[i] + TRB.sections[1].SectionOffset, SeekOrigin.Begin);
                MeshInfos.Add(new MeshInfo());
            }
            // Continue reader the pmdl header
            TRB._f.BaseStream.Seek(headerPos+4, SeekOrigin.Begin);
            PZero = TRB._f.ReadUInt32();
            PZero2 = TRB._f.ReadUInt32();
            ReadHelper.SeekToOffset(TRB._f, TRB._f.ReadUInt32() + TRB.sections[1].SectionOffset);
            BoneCount = TRB._f.ReadUInt32();
            BoneDataOffset = TRB._f.ReadUInt32();
            BoneNamesOffset = TRB._f.ReadUInt32();
            BoneParentsOffset = TRB._f.ReadUInt32();
            ReadHelper.ReturnToOrginalPosition(TRB._f);
            VertexCount = TRB._f.ReadUInt32();
            VertexBufferSize = TRB._f.ReadUInt32();
            VertexBufferOffset = TRB._f.ReadUInt32();
            PZero3 = TRB._f.ReadUInt32();
            FaceCount = TRB._f.ReadUInt32();
            FaceOffset = TRB._f.ReadUInt32();
            PZero4 = TRB._f.ReadUInt32();
            UnknownCount = TRB._f.ReadUInt32();
            UnknownOffset = TRB._f.ReadUInt32();
            ReadModelData();
        }

        private void ReadModelData()
        {
            var scene = new Scene();
            MeshData = new Mesh[MeshInfosCount];
            uint normalUVStart;
            uint normalUVEnd;
            List<uint> normalUVSize = new List<uint>();
            //This is code from BL2ModelConverter pretty dirty!
            if (MeshInfosCount > 1)
            {
                int remember = 0;
                for (int a = 0; a + 1 < MeshInfosCount; a++)
                {
                    normalUVStart = MeshInfos[a].NormalUVOffset;
                    normalUVEnd = MeshInfos[a + 1].NormalUVOffset;
                    normalUVSize.Add(normalUVEnd - normalUVStart);
                    remember = a;
                }
                normalUVStart = MeshInfos[remember + 1].NormalUVOffset;
                normalUVEnd = MeshInfos[remember + 1].FaceOffset;
                normalUVSize.Add(normalUVEnd - normalUVStart);
            }
            else if (MeshInfosCount == 1)
            {
                normalUVStart = MeshInfos[0].NormalUVOffset;
                normalUVEnd = MeshInfos[0].FaceOffset;
                normalUVSize.Add(normalUVEnd - normalUVStart);
            }
            // ------------------------------------------------------------------
            for (int i = 0; i < MeshInfosCount; i++)
            {
                var normals = new List<Vector4>();
                var uvs = new List<Vector4>();
                MeshData[i] = new Mesh();
                TRB._f.BaseStream.Seek(VertexBufferOffset + MeshInfos[i].VertexOffsetRelative + TRB.sections[2].SectionOffset, SeekOrigin.Begin);
                for (int j = 0; j < MeshInfos[i].VertexCount; j++)
                {
                    MeshData[i].ControlPoints.Add(new Vector4(Half.ToHalf(TRB._f.ReadUInt16()), Half.ToHalf(TRB._f.ReadUInt16()), Half.ToHalf(TRB._f.ReadUInt16()), Half.ToHalf(TRB._f.ReadUInt16())));
                }
                TRB._f.BaseStream.Seek(VertexBufferOffset + MeshInfos[i].NormalUVOffset + TRB.sections[2].SectionOffset, SeekOrigin.Begin);
                uint stride = normalUVSize[i] / MeshInfos[i].VertexCount;
                for (int j = 0; j < MeshInfos[i].VertexCount; j++)
                {
                    switch (stride)
                    {
                        case 16:
                            normals.Add(new Vector4(TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle()));
                            uvs.Add(new Vector4(Half.ToHalf(TRB._f.ReadUInt16()), -Half.ToHalf(TRB._f.ReadUInt16()), 1));
                            break;
                        case 20:
                            normals.Add(new Vector4(TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle()));
                            uvs.Add(new Vector4(Half.ToHalf(TRB._f.ReadUInt16()), -Half.ToHalf(TRB._f.ReadUInt16()), 1));
                            TRB._f.BaseStream.Seek(4, SeekOrigin.Current);
                            break;
                        case 24:
                            normals.Add(new Vector4(TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle()));
                            uvs.Add(new Vector4(Half.ToHalf(TRB._f.ReadUInt16()), -Half.ToHalf(TRB._f.ReadUInt16()), 1));
                            TRB._f.BaseStream.Seek(8, SeekOrigin.Current);
                            break;
                        case 28:
                            normals.Add(new Vector4(TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle()));
                            uvs.Add(new Vector4(Half.ToHalf(TRB._f.ReadUInt16()), -Half.ToHalf(TRB._f.ReadUInt16()), 1));
                            TRB._f.BaseStream.Seek(12, SeekOrigin.Current);
                            break;
                        case 32:
                            normals.Add(new Vector4(TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle()));
                            TRB._f.BaseStream.Seek(4, SeekOrigin.Current);
                            uvs.Add(new Vector4(Half.ToHalf(TRB._f.ReadUInt16()), -Half.ToHalf(TRB._f.ReadUInt16()), 1));
                            TRB._f.BaseStream.Seek(12, SeekOrigin.Current);
                            break;
                        default:
                            TRB._f.BaseStream.Seek(stride, SeekOrigin.Current);
                            break;
                    }

                }
                Normals.Add(normals);
                Uvs.Add(uvs);
                TRB._f.BaseStream.Seek(VertexBufferOffset + MeshInfos[i].FaceOffset + MeshInfos[i].PreviousFaceCount*2 + TRB.sections[2].SectionOffset, SeekOrigin.Begin);
                for (int j = 0; j < MeshInfos[i].FaceCount / 3; j++)
                {
                    MeshData[i].CreatePolygon(new int[] { TRB._f.ReadUInt16(), TRB._f.ReadUInt16(), TRB._f.ReadUInt16() });
                }
            }
        }
    }
}
