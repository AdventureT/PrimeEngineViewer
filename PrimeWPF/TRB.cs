using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Serilog;

namespace PrimeWPF
{
    class TRB
    {
        public static BinaryReader _f;
        public static string _fileName;
        private Header _header;
        public static List<Section> sections = new List<Section>();
        public static List<TagInfo> tagInfos = new List<TagInfo>();

        public TRB(string fileName)
        {
            _fileName = fileName;
            Log.Logger = new LoggerConfiguration().WriteTo.File("log.txt").CreateLogger();
            _f = new BinaryReader(File.Open(fileName, FileMode.Open));
            Read();
        }

        ~TRB()
        {
            _f.Close();
            Log.CloseAndFlush();
        }

        private void Read()
        {
            _header = new Header(_f);
            _f.BaseStream.Seek(0x5C, SeekOrigin.Current); //Skip padding
            for (int i = 0; i < _header.DataInfoCount; i++)
            {
                if (sections.Count == 0) { sections.Add(new Section()); } else { sections.Add(new Section(sections[0].SectionOffset)); }
            }
            for (int i = 0; i < _header.TagCount; i++)
            {
                tagInfos.Add(new TagInfo());
            }
            var dataSectionOffset = sections.Where(x => x.TextOffset == ".data").First().SectionOffset;
            for (int i = 0; i < tagInfos.Count; i++)
            {
                _f.BaseStream.Seek(tagInfos[i].Offset + dataSectionOffset, SeekOrigin.Begin);
                switch (tagInfos[i].Name)
                {
                    case "PMDL":
                        var pmdl = new PMDL();
                        break;
                    default:
                        if (tagInfos[i].Name.First() == 'P')
                        {
                            var unknownTag = new Tag();
                            Log.Information($"Unknown Tag found: {unknownTag.Name} filename is {unknownTag.FullName}");
                        }
                        else
                        {
                            Log.Information($"Unknown Tag found: {tagInfos[i].Name} filename is {tagInfos[i].FullName}");
                        }
                        break;
                }
            }
        }
    }
}
