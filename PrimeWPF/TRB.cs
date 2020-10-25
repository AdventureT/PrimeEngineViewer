using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Serilog;

namespace PrimeWPF
{
    class TRB
    {
        private BinaryReader _f;
        private Header _header;
        public static List<Section> sections = new List<Section>();
        public static List<Tag> tags = new List<Tag>();

        public TRB(string fileName)
        {
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
                if (sections.Count == 0) { sections.Add(new Section(_f)); } else { sections.Add(new Section(_f, sections[0].SectionOffset)); }
            }
            for (int i = 0; i < _header.TagCount; i++)
            {
                tags.Add(new Tag(_f));
            }
        }
    }
}
