namespace PrimeWPF
{
    class Section
    {
        public uint Unknown { get; set; }
        public string TextOffset { get; set; }
        public uint Uk2 { get; set; }
        public uint Uk3 { get; set; }
        public uint SectionSize { get; set; }
        public uint SectionSize2 { get; set; }
        public uint SectionOffset { get; set; }
        public uint Uk4 { get; set; }
        public uint Uk5 { get; set; }
        public uint Zero { get; set; }
        public uint ReversedTagCount { get; set; }
        public uint ReversedTagsOffset { get; set; }

        public Section()
        {
            Unknown = TRB._f.ReadUInt32();
            var tempTextOffset = TRB._f.ReadUInt32();
            Uk2 = TRB._f.ReadUInt32();
            Uk3 = TRB._f.ReadUInt32();
            SectionSize = TRB._f.ReadUInt32();
            SectionSize2 = TRB._f.ReadUInt32();
            SectionOffset = TRB._f.ReadUInt32();
            Uk4 = TRB._f.ReadUInt32();
            Uk5 = TRB._f.ReadUInt32();
            Zero = TRB._f.ReadUInt32();
            ReversedTagCount = TRB._f.ReadUInt32();
            ReversedTagsOffset = TRB._f.ReadUInt32();
            TextOffset = ReadHelper.ReadStringFromOffset(TRB._f, tempTextOffset + SectionOffset);
        }

        public Section(uint textOffset)
        {
            Unknown = TRB._f.ReadUInt32();
            TextOffset = ReadHelper.ReadStringFromOffset(TRB._f, TRB._f.ReadUInt32() + textOffset);
            Uk2 = TRB._f.ReadUInt32();
            Uk3 = TRB._f.ReadUInt32();
            SectionSize = TRB._f.ReadUInt32();
            SectionSize2 = TRB._f.ReadUInt32();
            SectionOffset = TRB._f.ReadUInt32();
            Uk4 = TRB._f.ReadUInt32();
            Uk5 = TRB._f.ReadUInt32();
            Zero = TRB._f.ReadUInt32();
            ReversedTagCount = TRB._f.ReadUInt32();
            ReversedTagsOffset = TRB._f.ReadUInt32();
        }
    }
}
