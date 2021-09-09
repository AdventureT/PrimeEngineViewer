using Aspose.ThreeD.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWPF
{
    public class enti
    {
        public enum PropertyType
        {
            INT, //OrientationType, VehicleType maybe enums? OnPointTrigger has FF FF FF FF?? INT
            UINT,
            FLOAT,
            BOOL,
            TEXTOFFSET,
            VECTOR4,
            Unknown2,
            Unknown3,
            Unknown4,
            OFFSET
        }

        public List<EntityInfo> EntityInfos { get; set; } = new();

        public class EntityInfo 
        {
            public string EntityName { get; set; }
            public ushort PropertiesCount { get; set; }
            public ushort Flag { get; set; }
            public uint PropertiesOffset { get; set; }
            public uint MatrixOffset { get; set; }
            public uint PositionOffset { get; set; }
            public uint Unk2 { get; set; }
            public uint Unk3 { get; set; }
            public uint ValuesOffset { get; set; }
            public List<EntityProperty> Properties { get; set; }
            public Vector3 Position { get; set; }
            public Vector3 Scale { get; set; }
            public Vector3 Rotation { get; set; }
            public uint PositionPos { get; set; }
            public EntityInfo(string entityName, ushort propertiesCount, ushort flag, uint propertiesOffset, uint matrixOffset, uint positionOffset, uint unk2, uint unk3, uint valuesOffset)
            {
                EntityName = entityName;
                PropertiesCount = propertiesCount;
                Flag = flag;
                PropertiesOffset = propertiesOffset;
                MatrixOffset = matrixOffset;
                PositionOffset = positionOffset;
                Unk2 = unk2;
                Unk3 = unk3;
                ValuesOffset = valuesOffset;
                Properties = new();
            }
        } ;

        public class EntityProperty
        {
            public string Name { get; set; }
            public PropertyType Type { get; set; }
            public object Value { get; set; }
            public uint Position { get; set; }
            public EntityProperty(string name, PropertyType type, object value)
            {
                Name = name;
                Type = type;
                Value = value;
                Position = (uint)TRB._f.BaseStream.Position - 12;
            }
            public override string ToString()
            {
                return $"Name: {Name}, Type: {Type}, Value: {Value}";
            }
        }        
        public enti()
        {
            var entityInfoOffset = TRB._f.ReadUInt32();
            var entityInfoCount = TRB._f.ReadUInt32();
            TRB._f.BaseStream.Seek(entityInfoOffset + TRB.sections[1].SectionOffset, SeekOrigin.Begin);
            for (int i = 0; i < entityInfoCount; i++)
            {
                EntityInfos.Add( new EntityInfo(ReadHelper.ReadStringFromOffset(TRB._f, TRB._f.ReadUInt32() + TRB.sections[0].SectionOffset), TRB._f.ReadUInt16(),
                    TRB._f.ReadUInt16(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32(), TRB._f.ReadUInt32()));
            }

            for (int i = 0; i < EntityInfos.Count; i++)
            {
                TRB._f.BaseStream.Seek(EntityInfos[i].PropertiesOffset + TRB.sections[1].SectionOffset, SeekOrigin.Begin);
                for (int j = 0; j < EntityInfos[i].PropertiesCount; j++)
                {
                    var entName = ReadHelper.ReadStringFromOffset(TRB._f, TRB._f.ReadUInt32() + TRB.sections[0].SectionOffset);
                    var propType = (PropertyType)TRB._f.ReadUInt32();
                    switch (propType)
                    {
                        case PropertyType.INT:
                            EntityInfos[i].Properties.Add(new EntityProperty(entName, propType, TRB._f.ReadInt32()));
                            break;
                        case PropertyType.UINT:
                            EntityInfos[i].Properties.Add(new EntityProperty(entName, propType, TRB._f.ReadInt32()));
                            break;
                        case PropertyType.FLOAT:
                            EntityInfos[i].Properties.Add(new EntityProperty(entName, propType, TRB._f.ReadSingle()));
                            break;
                        case PropertyType.BOOL:
                            EntityInfos[i].Properties.Add(new EntityProperty(entName, propType, TRB._f.ReadBoolean()));
                            TRB._f.BaseStream.Seek(3, SeekOrigin.Current);
                            break;
                        case PropertyType.TEXTOFFSET:
                            var offset = TRB._f.ReadUInt32();
                            if (offset == 0) EntityInfos[i].Properties.Add(new EntityProperty(entName, propType, "N/A"));
                            else EntityInfos[i].Properties.Add(new EntityProperty(entName, propType, ReadHelper.ReadStringFromOffset(TRB._f, offset + TRB.sections[0].SectionOffset)));
                            break;
                        case PropertyType.VECTOR4:
                            EntityInfos[i].Properties.Add(new EntityProperty(entName, propType, ReadHelper.ReadVector4FromOffset(TRB._f, TRB._f.ReadUInt32())));
                            break;
                        case PropertyType.Unknown2:
                            break;
                        case PropertyType.Unknown3:
                            break;
                        case PropertyType.Unknown4:
                            break;
                        case PropertyType.OFFSET:
                            EntityInfos[i].Properties.Add(new EntityProperty(entName, propType, TRB._f.ReadInt32()));
                            break;
                        default:
                            EntityInfos[i].Properties.Add(new EntityProperty(entName, propType, TRB._f.ReadInt32()));
                            break;
                    }
                }
                TRB._f.BaseStream.Seek(EntityInfos[i].MatrixOffset + TRB.sections[1].SectionOffset, SeekOrigin.Begin);
                EntityInfos[i].PositionPos = (uint)TRB._f.BaseStream.Position;
                Matrix4 mat = new(TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle(), 
                    TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle(), 
                    TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle(), 
                    TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle(), TRB._f.ReadSingle());
                mat.Decompose(out Vector3 pos, out Vector3 scale, out Quaternion rot);
                EntityInfos[i].Position = pos;
                EntityInfos[i].Scale = scale;
                EntityInfos[i].Rotation = rot.EulerAngles();
            }
        }
    }
}
