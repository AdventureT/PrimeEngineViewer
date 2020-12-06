using Serilog;

namespace PrimeWPF
{
    class Header
    {
        public string Signature { get; set; }
        public uint Version { get; set; }
        public ushort Flag1 { get; set; }
        public ushort Flag2 { get; set; }
        public uint DataInfoCount { get; set; }
        public uint DataInfoSize { get; set; }
        public uint TagCount { get; set; }
        public uint TagSize { get; set; }
        public uint RelocationDataOffset { get; set; }
        public uint RelocationDataSize { get; set; }

        public Header()
        {
            Signature = new string(TRB._f.ReadChars(4));
            Version = TRB._f.ReadUInt32();
            Flag1 = TRB._f.ReadUInt16();
            Flag2 = TRB._f.ReadUInt16();
            DataInfoCount = TRB._f.ReadUInt32();
            DataInfoSize = TRB._f.ReadUInt32();
            TagCount = TRB._f.ReadUInt32();
            TagSize = TRB._f.ReadUInt32();
            RelocationDataOffset = TRB._f.ReadUInt32();
            RelocationDataSize = TRB._f.ReadUInt32();
            Log.Information(Signature);
        }
    }
}
