using PathOfVision.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static PathOfVision.ExtensionsData;
using static System.Net.Mime.MediaTypeNames;

namespace PathOfVision
{
    public class ImageComparer_Hash
    {
        public static bool AreImagesSimilar(Bitmap image1, Bitmap image2)
        {
            List<bool> hash1 = GetHash(image1);
            List<bool> hash2 = GetHash(image2);


            int equalElements = hash1.Zip(hash2, (i, j) => i == j).Count(eq => eq);

            if (equalElements >= 250)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public static bool AreImagesSimilar_HalfCompare(Bitmap image1, Bitmap image2)
        {
            // Получаем высоту изображения и определяем количество пикселей, которые нужно оставить для сравнения (оставляем только верхние 70%)
            int compareHeight = (int)(image1.Height * 0.7);
            int totalPixels = image1.Width * compareHeight;

            // Обрезаем изображения до верхних 70%
            Bitmap croppedImage1 = CropImage(image1, 0, 0, image1.Width, compareHeight);
            Bitmap croppedImage2 = CropImage(image2, 0, 0, image2.Width, compareHeight);


            // Получаем хэши для обрезанных изображений
            List<bool> hash1 = GetHash(croppedImage1);
            List<bool> hash2 = GetHash(croppedImage2);

            // Сравниваем хэши
            int equalElements = hash1.Zip(hash2, (i, j) => i == j).Count(eq => eq);

            // Определяем пороговое значение для считывания изображения как похожего
            int threshold = (int)(totalPixels * 0.5); // Устанавливаем пороговое значение в 50%

            // Возвращаем результат сравнения
            return equalElements >= 240;
        }

        private static Bitmap CropImage(Bitmap image, int x, int y, int width, int height)
        {
            Rectangle cropRect = new Rectangle(x, y, width, height);
            return image.Clone(cropRect, image.PixelFormat);
        }

        private static List<bool> GetImageHash(Bitmap image)
        {
            // Resize the image to 16x16 pixels
            Bitmap resizedImage = new Bitmap(image, new Size(16, 16));


            List<bool> hash = new List<bool>();

            // Reduce colors to black/white
            for (int y = 0; y < resizedImage.Height; y++)
            {
                for (int x = 0; x < resizedImage.Width; x++)
                {
                    Color pixelColor = resizedImage.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);

                    hash.Add(grayValue > 128); // Convert to black/white (true/false)
                }
            }

            return hash;
        }



        public static string GetHashString(Bitmap image)
        {
            List<bool> hash = GetImageHash(image);

            // Convert the boolean hash to a byte array
            byte[] bytes = new byte[hash.Count / 8];
            for (int i = 0; i < hash.Count; i++)
            {
                if (hash[i])
                {
                    bytes[i / 8] |= (byte)(1 << (7 - (i % 8)));
                }
            }

            // Compute the MD5 hash of the byte array
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "");
            }
        }


        public static bool CompareMiniIconForCutIcon(Bitmap image1, Bitmap image2)
        {



            List<bool> hash1 = GetHash(image1);
            List<bool> hash2 = GetHash(image2);

            var equalCount = hash2.Count * 0.85f;
            int countAccactableFails = (int)equalCount - hash2.Count;
            //  int equalElements = hash1.Zip(hash2, (i, j) => i == j).Count(eq => eq);

            for (int i = 0; i < hash1.Count; i++)
            {
                if (hash1[i] != hash2[i])
                {
                    return false; // Hash lists are not equal
                }
            }
            return true;


        }

        public static bool CompareMiniIconForGameCompare(Bitmap image1, Bitmap image2)
        {
            var wid = Math.Min(image1.Width, image2.Width);

            List<bool> hash1 = GetHash(image1);
            List<bool> hash2 = GetHash(image2);

            var equalCount = hash2.Count * 0.8f;
            int countAccactableFails = hash2.Count - (int)equalCount;
            int equalElements = hash1.Zip(hash2, (i, j) => i == j).Count(eq => eq);

            int countMiss = 0;

            for (int i = 0; i < hash1.Count; i++)
            {
                if (hash1[i] != hash2[i])
                {
                    countAccactableFails--;
                    if (countAccactableFails == 0)
                    {
                        return false;
                    }
                }
            }
            return true;


        }
        public static bool CompareMiniIconForGameCompare1(Bitmap image1, Bitmap image2)
        {
            int mismatchThreshold = (int)(0.99f * image1.Width * image1.Height); // Порог несоответствия - 35%
            int mismatchCount = 0;

            for (int y = 0; y < image1.Height; y++)
            {
                for (int x = 0; x < image1.Width; x++)
                {
                    Color pixel1 = image1.GetPixel(x, y);
                    Color pixel2 = image2.GetPixel(x, y);

                    // Сравниваем цвета пикселей
                    if (pixel1 != pixel2)
                    {
                        mismatchCount++;
                    }

                    // Если количество несоответствий превышает порог - изображения не совпадают
                    if (mismatchCount > mismatchThreshold)
                    {
                        return false;
                    }
                }
            }

            // Если количество несоответствий не превысило порог - изображения считаются совпадающими
            return true;
        }

        public static List<bool> GetHash(Bitmap bmpSource)
        {
            List<bool> lResult = new List<bool>();
            //create new image with 16x16 pixel
            Bitmap bmpMin = new Bitmap(bmpSource, new Size(16, 16));

            for (int j = 0; j < bmpMin.Height; j++)
            {
                for (int i = 0; i < bmpMin.Width; i++)
                {
                    //reduce colors to true / false                
                    lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);
                }
            }
            return lResult;
        }
        public static Bitmap DecreaseSize(Bitmap bmpSource)
        {
            Bitmap bmpMin = new Bitmap(bmpSource, new Size(16, 16));
            return bmpMin;
        }

        public static bool CompareTest1(Bitmap image1, Bitmap image2)
        {
            image1 = DecreaseSize(new Bitmap(image1));
            image2 = DecreaseSize(new Bitmap(image2));

            List<int> pixels1 = new List<int>();
            List<int> pixels2 = new List<int>();

            int countFail = 0;


            for (int i = 0; i < image1.Width; i++)
            {
                int pixelColor1 = image1.GetPixel(0 + i, 0).ToArgb();
                int pixelColor2 = image2.GetPixel(0 + i, 0).ToArgb();


                pixels1.Add(pixelColor1);
                pixels2.Add(pixelColor2);

                if (pixelColor1 != pixelColor2)
                {
                    countFail++;
                    if (countFail > 4)
                    {
                        return false;
                    }
                }

            }
            return true;
        }
        public struct ColorDifference
        {
            public int RedDifference { get; set; }
            public int GreenDifference { get; set; }
            public int BlueDifference { get; set; }
        }

        public static bool CompareTest1(Bitmap image1, Bitmap image2, int argbDiffrence)
        {
            image1 = DecreaseSize(new Bitmap(image1));
            image2 = DecreaseSize(new Bitmap(image2));

            int maxDifference = argbDiffrence; // Максимальная разница в значениях RGB для приемлемого сходства

            List<Color> pixels1 = new List<Color>();
            List<Color> pixels2 = new List<Color>();
            List<ColorDifference> differences = new List<ColorDifference>();

            for (int i = 0; i < image1.Width; i++)
            {
                Color pixelColor1 = image1.GetPixel(i, 0);
                Color pixelColor2 = image2.GetPixel(i, 0);
                pixels1.Add(pixelColor1);
                pixels2.Add(pixelColor2);

                // Проверяем разницу между значениями RGB для каждого пикселя
                int diffRed = Math.Abs(pixelColor1.R - pixelColor2.R);
                int diffGreen = Math.Abs(pixelColor1.G - pixelColor2.G);
                int diffBlue = Math.Abs(pixelColor1.B - pixelColor2.B);

                // Добавляем различия в список
                differences.Add(new ColorDifference
                {
                    RedDifference = diffRed,
                    GreenDifference = diffGreen,
                    BlueDifference = diffBlue
                });

                // Если хотя бы одна разница превышает максимальную допустимую, возвращаем false
                if (diffRed > maxDifference || diffGreen > maxDifference || diffBlue > maxDifference)
                {
                    return false;
                }
            }




            return true;
        }

        public static ExtensionsData.Side side;

        public static bool CompareTest(Bitmap image1, Bitmap image2, GlobalData.GameIconState iconSettings)
        {
            if (iconSettings.isTopCheck)
            {
                side = Side.Top;
                if (ColorCompare(image1, image2, iconSettings, side))
                {
                    return true;
                }
                
            }
            if (iconSettings.isBotCheck)
            {
                side = Side.Bottom;
                if (ColorCompare(image1, image2, iconSettings, side))
                {
                    return true;
                }

            }
            if (iconSettings.isLeftCheck)
            {
                side = Side.Left;
                if (ColorCompare(image1, image2, iconSettings, side))
                {
                    return true;
                }

            }
            if (iconSettings.isRightCheck)
            {
                side = Side.Right;
                if (ColorCompare(image1, image2, iconSettings, side))
                {
                    return true;
                }

            }





            return false;
        }






        public static bool ColorCompare(Bitmap image1, Bitmap image2, GlobalData.GameIconState iconSettings, ExtensionsData.Side side)
        {
            float minusDiffrence = 1f - iconSettings.argbDifferences / 100f;
            float plusDiffrence = 1f + iconSettings.argbDifferences / 100f;

            int totalPixels = image1.Width - 5;

            int avgRed1 = 0, avgBlue1 = 0, avgGreen1 = 0;
            int avgRed2 = 0, avgBlue2 = 0, avgGreen2 = 0;



            for (int i = 5; i < totalPixels; i++)
            {
                Color pixelColor1Top = default;
                Color pixelColor2Top = default;

                switch (side)
                {
                    case ExtensionsData.Side.Left:
                        pixelColor1Top = image1.GetPixel(5 + 1, 5 + i);
                        pixelColor2Top = image2.GetPixel(5, 5 + i);
                        break;
                    case ExtensionsData.Side.Right:
                        pixelColor1Top = image1.GetPixel(image1.Width - 5 + 1, 5 + i);
                        pixelColor2Top = image2.GetPixel(image1.Width - 5, 5 + i);
                        break;
                    case ExtensionsData.Side.Top:
                        pixelColor1Top = image1.GetPixel(i + 1, 0);
                        pixelColor2Top = image2.GetPixel(i, 0);
                        break;
                    case ExtensionsData.Side.Bottom:
                        pixelColor1Top = image1.GetPixel(i + 1, image1.Height - 1);
                        pixelColor2Top = image2.GetPixel(i, image1.Height - 1);
                        break;
                }



                avgRed1 += pixelColor1Top.R;
                avgBlue1 += pixelColor1Top.B;
                avgGreen1 += pixelColor1Top.G;

                avgRed2 += pixelColor2Top.R;
                avgBlue2 += pixelColor2Top.B;
                avgGreen2 += pixelColor2Top.G;
            }
            int countOk = 0;
            if (avgRed2 >= avgRed1 * minusDiffrence && avgRed2 <= avgRed1 * plusDiffrence)
            {
                countOk++;
            }
            if (avgBlue2 >= avgBlue1 * minusDiffrence && avgBlue2 <= avgBlue1 * plusDiffrence)
            {
                countOk++;

            }
            if (avgGreen2 >= avgGreen1 * minusDiffrence && avgGreen2 <= avgGreen1 * plusDiffrence)
            {
                countOk++;

            }

            if (countOk == 3)
            {
                return true;
            }

            avgRed1 = 0; avgBlue1 = 0; avgGreen1 = 0;
            avgRed2 = 0; avgBlue2 = 0; avgGreen2 = 0;
            countOk = 0;

            for (int i = 5; i < totalPixels; i++)
            {
                Color pixelColor1Top = default;
                Color pixelColor2Top = default;

                switch (side)
                {
                    case ExtensionsData.Side.Left:
                        pixelColor1Top = image1.GetPixel(5, 5 + i);
                        pixelColor2Top = image2.GetPixel(5, 5 + i);
                        break;
                    case ExtensionsData.Side.Right:
                        pixelColor1Top = image1.GetPixel(image1.Width - 5, 5 + i);
                        pixelColor2Top = image2.GetPixel(image1.Width - 5, 5 + i);
                        break;
                    case ExtensionsData.Side.Top:
                        pixelColor1Top = image1.GetPixel(i, 0);
                        pixelColor2Top = image2.GetPixel(i, 0);
                        break;
                    case ExtensionsData.Side.Bottom:
                        pixelColor1Top = image1.GetPixel(i, image1.Height - 1);
                        pixelColor2Top = image2.GetPixel(i, image1.Height - 1);
                        break;
                }



                avgRed1 += pixelColor1Top.R;
                avgBlue1 += pixelColor1Top.B;
                avgGreen1 += pixelColor1Top.G;

                avgRed2 += pixelColor2Top.R;
                avgBlue2 += pixelColor2Top.B;
                avgGreen2 += pixelColor2Top.G;
            }
            if (avgRed2 >= avgRed1 * minusDiffrence && avgRed2 <= avgRed1 * plusDiffrence)
            {
                countOk++;
            }
            if (avgBlue2 >= avgBlue1 * minusDiffrence && avgBlue2 <= avgBlue1 * plusDiffrence)
            {
                countOk++;

            }
            if (avgGreen2 >= avgGreen1 * minusDiffrence && avgGreen2 <= avgGreen1 * plusDiffrence)
            {
                countOk++;

            }

            if (countOk == 3)
            {
                return true;
            }

            return false;
        }






    }
}
