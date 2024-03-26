using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTerminal.Editor.ImageEditor
{
    internal static class Picture
    {
        internal static void ToGrayScale(this Bitmap bitmap)
        {
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color pixel = bitmap.GetPixel(i, j);
                    int avg = (pixel.R + pixel.G + pixel.B) / 3;
                    bitmap.SetPixel(i, j, Color.FromArgb(pixel.A, avg, avg, avg));
                }
            }
        }
    }
}
