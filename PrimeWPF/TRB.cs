﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
        public static List<ReversedTagInfo> reversedTagInfos = new List<ReversedTagInfo>();
        public List<object> _items = new List<object>();

        public TRB(string fileName)
        {
            _fileName = fileName;
            Log.Logger = new LoggerConfiguration().WriteTo.File("log.txt").CreateLogger();
             _f = new BinaryReader(File.Open(fileName, FileMode.Open, FileAccess.Read));
            Read();
        }

        ~TRB()
        {
            _f.Close();
            Log.CloseAndFlush();
        }

        private void Read()
        {
            _header = new Header();
            _f.BaseStream.Seek(0x5C, SeekOrigin.Current); //Skip padding
            for (int i = 0; i < _header.DataInfoCount; i++)
            {
                if (sections.Count == 0) { sections.Add(new Section()); } else { sections.Add(new Section(sections[0].SectionOffset)); }
            }
            for (int i = 0; i < _header.TagCount; i++)
            {
                tagInfos.Add(new TagInfo());
            }
            for (int i = 0; i < sections[1].ReversedTagCount; i++)
            {
                reversedTagInfos.Add(new ReversedTagInfo());
            }
            var dataSectionOffset = sections.First(x => x.TextOffset == ".data").SectionOffset;
            for (int i = 0; i < tagInfos.Count; i++)
            {
                _f.BaseStream.Seek(tagInfos[i].Offset + dataSectionOffset, SeekOrigin.Begin);
                switch (tagInfos[i].Name)
                {
                    case "BTEC":
                        //_items.Add();
                        break;
                    case "PMDL":
                        _items.Add(new PMDL());
                        break;
                    case "PTEX":
                        _items.Add(new PTEX());
                        break;
                    case "PCOL":
                        _items.Add(new PCOL());
                        break;
                    case "enti":
                        _items.Add(new enti());
                        break;
                    case "\0\0\0\0":
                        switch (tagInfos[i].FullName)
                        {
                            case "LocaleStrings":
                                _items.Add(new LocaleStrings());
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            _f.Close();
            ReadHelper._lastPos = 0;
            sections.Clear();
            tagInfos.Clear();
        }
    }
}
