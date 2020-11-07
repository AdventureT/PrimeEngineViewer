using Aspose.ThreeD;
using Aspose.ThreeD.Entities;
using Aspose.ThreeD.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media.Media3D;
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
        public uint[] NextPMDLOffset { get; set; }
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

        ~PMDL()
        {
            MeshInfos.Clear();
            MeshInfoOffsets.Clear();
        }

        public PMDL() : base()
        {
            Unknown = TRB._f.ReadUInt32();
            Count = TRB._f.ReadUInt32();
            var headerPos = ReadHelper.SeekToOffset(TRB._f.ReadUInt32() + TRB.sections[1].SectionOffset);
            UnknownVector4s = new Vector4[] { ReadHelper.ReadVector4(), ReadHelper.ReadVector4() };
            MeshInfosCount = TRB._f.ReadUInt32();
            MeshInfoOffsetsOffset = TRB._f.ReadUInt32();
            ReadHelper.SeekToOffset(TRB._f.ReadUInt32() + TRB.sections[1].SectionOffset);
            UnknownVector4s2 = new Vector4[] { ReadHelper.ReadVector4(), ReadHelper.ReadVector4() };
            ReadHelper.ReturnToOrginalPosition();
            TRB._f.BaseStream.Seek(TRB._f.ReadUInt32() + TRB.sections[1].SectionOffset, SeekOrigin.Begin);
            UnknownValues = new uint[] { TRB._f.ReadUInt32(), TRB._f.ReadUInt32() };
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
            TRB._f.BaseStream.Seek(headerPos, SeekOrigin.Begin);
            PZero = TRB._f.ReadUInt32();
            PZero2 = TRB._f.ReadUInt32();
            ReadHelper.SeekToOffset(TRB._f.ReadUInt32() + TRB.sections[1].SectionOffset);
            NextPMDLOffset = new uint[] { TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32()};
            ReadHelper.ReturnToOrginalPosition();
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
            for (int i = 0; i < MeshInfosCount; i++)
            {
                var meshNode = new Node("Mesh");
                MeshData[i] = new Mesh();
                meshNode.Entity = MeshData[i];
                scene.RootNode.AddChildNode(meshNode);
                TRB._f.BaseStream.Seek(VertexBufferOffset + MeshInfos[i].VertexOffsetRelative + TRB.sections[2].SectionOffset, SeekOrigin.Begin);
                for (int j = 0; j < MeshInfos[i].VertexCount; j++)
                {
                    MeshData[i].ControlPoints.Add(new Vector4(Half.ToHalf(TRB._f.ReadUInt16()), Half.ToHalf(TRB._f.ReadUInt16()), Half.ToHalf(TRB._f.ReadUInt16()), Half.ToHalf(TRB._f.ReadUInt16())));
                }
                TRB._f.BaseStream.Seek(VertexBufferOffset + MeshInfos[i].NormalUVOffset + TRB.sections[2].SectionOffset, SeekOrigin.Begin);
                //VertexElementNormal elementNormal = mesh.CreateElement(VertexElementType.Normal, MappingMode.ControlPoint, ReferenceMode.Direct) as VertexElementNormal;
                //for (int j = 0; j < MeshInfos[i].VertexCount; j++)
                //{
                //    elementNormal.Data.Add(new Vector4(TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle()));
                //}
                TRB._f.BaseStream.Seek(VertexBufferOffset + MeshInfos[i].FaceOffset + MeshInfos[i].PreviousFaceCount*2 + TRB.sections[2].SectionOffset, SeekOrigin.Begin);
                for (int j = 0; j < MeshInfos[i].FaceCount / 3; j++)
                {
                    MeshData[i].CreatePolygon(new int[] { TRB._f.ReadUInt16(), TRB._f.ReadUInt16(), TRB._f.ReadUInt16() });
                }
                meshNode.Transform.Rotation = Aspose.ThreeD.Utilities.Quaternion.FromRotation(new Vector3(0, 1, 0), new Vector3(180,0, 0));
            }
        }
    }
}
