using ColorMine.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaletteSwapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap paletteBitmap = BitmapHandler.Load("palette/endesga64.png");
            List<Color> paletteColors = ColorHandler.GetBitmapColors(paletteBitmap);
            Console.WriteLine("Palette Colors loaded: " + paletteColors.Count);

            List<BitmapFile> bitmapFiles = BitmapHandler.LoadAll("in");
            Console.WriteLine("Images loaded.");
            foreach (BitmapFile bitmapFile in bitmapFiles)
            {
                Console.WriteLine("Swap Colors: " + bitmapFile.Filename);
                Bitmap bitmap = ColorHandler.SwapColors(bitmapFile.Bitmap, paletteColors);
                BitmapHandler.Save(bitmap, bitmapFile.Filename);
                Console.WriteLine("Save image to out.");
            }
        }
    }
}
