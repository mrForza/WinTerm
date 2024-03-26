using System.Drawing;

namespace WinTerminal.Editor.ImageEditor
{
    internal class BitmapToAsciiConverter
    {
        private char[] _asciiTable = new char[] { '.', ',', ':', '+', '*', '?', '%', 'S', '#', '@', };
        private readonly Bitmap _bitmap;

        public BitmapToAsciiConverter(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public char[][] GetAsciiPicture()
        {
            char[][] result = new char[_bitmap.Height][];

            for (int i = 0; i < _bitmap.Height; i++)
            {
                result[i] = new char[_bitmap.Width];

                for (int j = 0; j < _bitmap.Width; j++)
                {
                    int mapIndex = (int)Map(_bitmap.GetPixel(j, i).R, 0, 255, 0, _asciiTable.Length - 1);
                    result[i][j] = _asciiTable[mapIndex];
                }
            }

            return result;
        }

        private float Map(float valueToMap, float start1, float stop1, float start2, float stop2)
        {
            return (valueToMap - start1) / (stop1 - start1) * (stop2 - start2) + start2;
        }
    }
}
