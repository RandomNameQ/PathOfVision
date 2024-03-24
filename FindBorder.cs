using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http.Headers;
using System.Numerics;

namespace PathOfVision
{
    public class FindBorder
    {
        public Bitmap screen;
        public int maxValue = -10006721;
        public int minValue = -10999999;

        public int iconWidth = 63;

        public Dictionary<int, Vector2> numberAndPosBorder = new();
        public int countIcon;







        public Vector2 findedXYFramePos;

        public Dictionary<int, Vector2> GetCenterPositionIcon(Bitmap imageOut)
        {
            Bitmap image =new Bitmap(imageOut);

            bool isBordedFinded = false;
            Dictionary<int, Vector2> positions = new();
            int countFinedBorders = 0;
            // нам надо получить середину иконки,высота иконки 63, 31 = по середине
            int yOffset = 31;

            int heightMidPos = yOffset;

            for (int x = 0; x < image.Width; x++)
            {
                var pixelColor = image.GetPixel(x, heightMidPos);

                // если пиксель есть в спектре и если мы не проверяли текущую позицию ширины. Если проверяли позицию 12:6 то 12:8 не проверяем
                if (Extensions.IsColorInRange(minValue, maxValue, pixelColor))
                {
                    // если нашли границу иконки
                    if (IsBorder(x, heightMidPos, image, pixelColor.ToArgb()))
                    {
                        isBordedFinded = true;

                        countFinedBorders++;
                        Vector2 centerIcon = new Vector2(x + 31, heightMidPos);
                        positions.Add(countFinedBorders, centerIcon);

                        x += iconWidth + 5;
                    }
                }
            }
            if (isBordedFinded)
            {
                return positions;

            }
            else
            {
                return default;
            }
        }

        public bool IsBorder(int x, int y, Bitmap image, int checkedPixel)
        {

            // тут мы нашли цвет пикселя, который означает рамки иконки
            // теперь мы хотим проверить от текущей позиции +10вверх и -10 вниз пиксели.
            // Если все эти пиксели имеют одинаковый цвет, то мы нашли левую сторону иконки.


            for (int i = 0; i < 10; i++)
            {
                Color pixelColor = image.GetPixel(x, y + i);
                if (checkedPixel != pixelColor.ToArgb())
                {
                    return false;
                }

            }

            for (int i = 0; i < 10; i++)
            {
                Color pixelColor = image.GetPixel(x, y - i);

                if (checkedPixel != pixelColor.ToArgb())
                {
                    return false;
                }

            }
            findedXYFramePos.X = x;
            findedXYFramePos.Y = y;
            ColorBorder(image);
            return true;
        }

        public void ColorBorder(Bitmap image)
        {

            try
            {
                for (int i = 0; i < 10; i++)
                {
                    image.SetPixel((int)findedXYFramePos.X + iconWidth - 1, (int)findedXYFramePos.Y + i, Color.FromArgb(255, 255, 255, 255));
                    image.SetPixel((int)findedXYFramePos.X, (int)findedXYFramePos.Y + i, Color.FromArgb(255, 255, 255, 255));
                }
                for (int i = 0; i < 10; i++)
                {
                    image.SetPixel((int)findedXYFramePos.X + iconWidth - 1, (int)findedXYFramePos.Y - i, Color.FromArgb(255, 255, 255, 255));
                    image.SetPixel((int)findedXYFramePos.X, (int)findedXYFramePos.Y - i, Color.FromArgb(255, 255, 255, 255));
                }

                Extensions.SaveImage(image, "TEST_ColorBorder");
            }
            catch (Exception ex)
            {
                // Обработка исключения
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            // красим пиксели для визуализации

        }









    }
}
