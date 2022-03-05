using ColorMine.ColorSpaces;
using ColorMine.ColorSpaces.Comparisons;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;

namespace PaletteSwapper
{
    public class ColorHandler
    {
        public static List<Color> GetBitmapColors(Bitmap bitmap)
        {
            List<Color> colors = new List<Color>();
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color color = Color.FromArgb(255, bitmap.GetPixel(i, j));
                    if (!colors.Contains(color))
                    {
                        colors.Add(color);
                    }
                }
            }
            return colors;
        }

        public static Bitmap SwapColors(Bitmap bitmap, List<Color> targetColors)
        {
            List<Color> imageColors = GetBitmapColors(bitmap);
            Dictionary<Color, Color> colorMap = GetColorMap(imageColors, targetColors);
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color actualColor = bitmap.GetPixel(i, j);
                    if (colorMap.ContainsKey(actualColor))
                    {
                        newBitmap.SetPixel(i, j, Color.FromArgb(actualColor.A, colorMap[actualColor]));
                    }
                }
            }
            return newBitmap;
        }

        public static Bitmap ReplaceColors(Bitmap bitmap, List<Color> inputColors, List<Color> targetColors)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color actualColor = bitmap.GetPixel(i, j);
                    Color color = Color.FromArgb(actualColor.A, actualColor);
                    int index = inputColors.IndexOf(color);
                    if (index > -1)
                    {
                        newBitmap.SetPixel(i, j, targetColors[index]);
                    }
                }
            }
            return newBitmap;
        }

        private static Dictionary<Color, Color> GetColorMap(List<Color> imageColors, List<Color> paletteColors)
        {
            List<Rgb> imageRgb = ConvertToRgb(imageColors);
            List<Rgb> paletteRgb = ConvertToRgb(paletteColors);
            Dictionary<Rgb, Rgb> rgbMap = new Dictionary<Rgb, Rgb>();
            foreach (Rgb rgb in imageRgb)
            {
                List<Rgb> similarRgbs = GetSimilarRgbs(rgb, paletteRgb);
                Rgb match = similarRgbs.First();
                rgbMap.Add(rgb, match);
            }
            return ConvertToColorMap(rgbMap);
        }

        private static Dictionary<Color, Color> ConvertToColorMap(Dictionary<Rgb, Rgb> rgbMap)
        {
            // TODO: convert rgb dictionary to color dictionary
            Dictionary<Color, Color> colorMap = new Dictionary<Color, Color>();
            foreach (Rgb rgb in rgbMap.Keys)
            {
                Color color = Color.FromArgb((int)rgb.R, (int)rgb.G, (int)rgb.B);
                Color match = Color.FromArgb((int)rgbMap[rgb].R, (int)rgbMap[rgb].G, (int)rgbMap[rgb].B);
                colorMap.Add(color, match);
            }
            return colorMap;
        }

        private static List<Rgb> GetSimilarRgbs(Rgb rgb, List<Rgb> paletteRgb)
        {
            SortedDictionary<double, Rgb> similarRgbs = new SortedDictionary<double, Rgb>();
            foreach (Rgb match in paletteRgb)
            {
                double similarity = rgb.Compare(match, new CieDe2000Comparison());
                similarRgbs.Add(similarity, match);
            }
            return similarRgbs.Values.ToList();
        }

        private static List<Rgb> ConvertToRgb(List<Color> colors)
        {
            List<Rgb> rgbs = new List<Rgb>();
            foreach (Color color in colors)
            {
                Rgb rgb = new Rgb { R = color.R, G = color.G, B = color.B };
                rgbs.Add(rgb);
            }
            return rgbs;
        }
    }
}