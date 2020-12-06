using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWPF
{
    public class LocaleStrings
    {
        public uint StringCount { get; set; }
        public List<string> Strings { get; set; } = new();

        public LocaleStrings()
        {
            StringCount = TRB._f.ReadUInt32();
            TRB._f.BaseStream.Seek(TRB.sections[1].SectionOffset, System.IO.SeekOrigin.Begin);
            for (int i = 0; i < StringCount; i++)
            {
                Strings.Add(ReadHelper.ReadStringFromOffset(TRB._f.ReadUInt32() + TRB.sections[1].SectionOffset));
            }
        }
    }
}
