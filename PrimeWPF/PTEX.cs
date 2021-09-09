using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PrimeWPF
{
    public class PTEX : Tag
    {
        public uint Width { get; set; }
        public uint Height { get; set; }
        public int Unknown { get; set; }
        public uint DDSOffset { get; set; }
        public uint DDSSize { get; set; }
        public Bitmap Image { get; set; }
        public byte[] RawImage { get; set; }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public static BitmapSource LoadBitmap(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }

        public PTEX() : base()
        {
            TRB._f.BaseStream.Seek(56, SeekOrigin.Current);
            Width = TRB._f.ReadUInt32();
            Height = TRB._f.ReadUInt32();
            Unknown = TRB._f.ReadInt32();
            DDSOffset = TRB._f.ReadUInt32();
            DDSSize = TRB._f.ReadUInt32();


            RawImage = ReadHelper.ReadFromOffset(TRB._f, DDSSize, DDSOffset + TRB.sections.Where(x => x.TextOffset == "texmem").ToArray()[0].SectionOffset);
            DDSImage img = new DDSImage(RawImage);
            Image = img.BitmapImage;
        }
    }
}
