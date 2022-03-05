using System.Drawing;

namespace PaletteSwapper
{
    public class BitmapFile
    {
        private Bitmap bitmap;
        private string filename;

        public BitmapFile(Bitmap bitmap, string filename)
        {
            this.bitmap = bitmap;
            this.filename = filename;
        }

        public Bitmap Bitmap { get => bitmap; set => bitmap = value; }
        public string Filename { get => filename; set => filename = value; }
    }
}