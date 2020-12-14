namespace PrimeWPF
{
    class TagInfo
    {
        public string Name { get; set; }
        public uint Offset { get; set; }
        public uint Flag { get; set; }
        public string FullName { get; set; }

        public TagInfo()
        {
            Name = new string(TRB._f.ReadChars(4));
            Offset = TRB._f.ReadUInt32();
            Flag = TRB._f.ReadUInt32();
            FullName = ReadHelper.ReadStringFromOffset(TRB.sections[0].SectionOffset + TRB._f.ReadUInt32());
        }
    }
}
