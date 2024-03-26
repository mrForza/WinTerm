using System.Drawing;

namespace WinTerminal.Editor.ImageEditor
{
    internal class ImageDisplayer
    {
        internal static void DisplayImage(string name)
        {       
            Console.Clear();
            Bitmap bitmap = new Bitmap($"{Terminal.Path}/{name}");
            bitmap = ResizeBitmap(bitmap);
            bitmap.ToGrayScale();

            BitmapToAsciiConverter converter = new BitmapToAsciiConverter(bitmap);
            char[][] mainArray = converter.GetAsciiPicture();

            for (int i = 0; i < mainArray.Length; i++)
            {
                for (int j = 0; j < mainArray[i].Length; j++)
                {
                    Console.Write(mainArray[i][j]);
                }
                Console.WriteLine();
            }

            Console.SetCursorPosition(0, 0);
                
        }
        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            int maxWidth = 120;
            float newHeight = bitmap.Height / 1.5f * maxWidth / bitmap.Width;

            if (bitmap.Width > maxWidth || bitmap.Height > newHeight)
            {
                bitmap = new Bitmap(bitmap, new Size(maxWidth, (int)newHeight));
            }

            return bitmap;
        }
    }
}
