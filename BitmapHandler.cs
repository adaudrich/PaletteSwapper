using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace PaletteSwapper
{
    public class BitmapHandler
    {
        public static List<BitmapFile> LoadAll(string path)
        {
            List<BitmapFile> bitmaps = new List<BitmapFile>();
            string[] files = Directory.GetFiles(path);
            foreach (string filename in files)
            {
                Bitmap bitmap = (Bitmap)Image.FromFile(filename);
                BitmapFile bitmapFile = new BitmapFile(bitmap, filename);
                bitmaps.Add(bitmapFile);
            }
            return bitmaps;
        }

        public static Bitmap Load(string path)
        {
            return (Bitmap)Image.FromFile(path);
        }

        public static void Save(Bitmap bitmap, string filename)
        {
            string[] parts = filename.Split('\\');
            bitmap.Save("out\\" + parts[1]);
        }

        public static void SavePalette(List<Color> palette)
        {
            Bitmap bitmap = new Bitmap(palette.Count, 1);
            for (int i = 0; i < bitmap.Width; i++)
            {
                bitmap.SetPixel(i, 0, palette[i]);
            }
            bitmap.Save("out\\palette_input.png");
        }
    }
}