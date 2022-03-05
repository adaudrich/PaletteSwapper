using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PaletteSwapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Select task:");
            Console.WriteLine("- Extract Palette from input files (1)");
            Console.WriteLine("- Auto-Swap colors in input files by target palette (2)");
            Console.WriteLine("- Replace colors in input files by color map (3)");
            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    ExtractPalette();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    AutoSwapColorPalette();
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    ReplaceColors();
                    break;
                default:
                    break;
            }
            Console.ReadKey();

        }

        private static void ReplaceColors()
        {
            Bitmap inputPalette = BitmapHandler.Load("palette/palette_input.png");
            List<Color> inputColors = ColorHandler.GetBitmapColors(inputPalette);

            Bitmap targetPalette = BitmapHandler.Load("palette/palette_target.png");
            List<Color> targetColors = ColorHandler.GetBitmapColors(targetPalette);

            Console.WriteLine("Input & Target Palette Colors loaded.");
            List<BitmapFile> bitmapFiles = BitmapHandler.LoadAll("in");
            Console.WriteLine("Images loaded.");
            foreach (BitmapFile bitmapFile in bitmapFiles)
            {
                Console.WriteLine("Replace Colors: " + bitmapFile.Filename);
                Bitmap bitmap = ColorHandler.ReplaceColors(bitmapFile.Bitmap, inputColors, targetColors);
                BitmapHandler.Save(bitmap, bitmapFile.Filename);
                Console.WriteLine("Save image to out.");
            }
        }

        private static void AutoSwapColorPalette()
        {
            //Bitmap targetPalette = BitmapHandler.Load("palette/endesga64.png");
            Bitmap targetPalette = BitmapHandler.Load("palette/duel256.png");
            List<Color> targetColors = ColorHandler.GetBitmapColors(targetPalette);
            Console.WriteLine("Palette Colors loaded: " + targetColors.Count);

            List<BitmapFile> bitmapFiles = BitmapHandler.LoadAll("in");
            Console.WriteLine("Images loaded.");
            foreach (BitmapFile bitmapFile in bitmapFiles)
            {
                Console.WriteLine("Auto-Swap Colors: " + bitmapFile.Filename);
                Bitmap bitmap = ColorHandler.SwapColors(bitmapFile.Bitmap, targetColors);
                BitmapHandler.Save(bitmap, bitmapFile.Filename);
                Console.WriteLine("Save image to out.");
            }
        }

        private static void ExtractPalette()
        {
            List<BitmapFile> bitmapFiles = BitmapHandler.LoadAll("in");
            Console.WriteLine("Images loaded.");
            List<Color> colorPalette = new List<Color>();
            foreach (BitmapFile bitmapFile in bitmapFiles)
            {
                Console.WriteLine("Extract Colors: " + bitmapFile.Filename);
                List<Color> colors = ColorHandler.GetBitmapColors(bitmapFile.Bitmap);
                foreach (Color color in colors)
                {
                    if (!colorPalette.Contains(color))
                    {
                        colorPalette.Add(color);
                    }
                }
            }
            List<Color> orderedColorPalette = colorPalette.OrderBy(color => color.GetHue()).ToList();
            BitmapHandler.SavePalette(orderedColorPalette);
            Console.WriteLine("Save extracted palette to out.");
        }
    }
}
