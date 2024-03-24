using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathOfVision
{
    public class TargetIcons
    {
        public List<Bitmap> images = new();

        public Dictionary<string, Dictionary<ExtensionsData.Side, List<int>>> nameAndPixels = new();
        public string imageName;

        public int countSaveblePixels = 10;

        public FindBorder findBorder = new();
        
        public TargetIcons()
        {
            string originalImagePath = "Icon/Buffs/ConcecratedGround.png";
            Bitmap originalImage = new Bitmap(originalImagePath);
            imageName = Path.GetFileNameWithoutExtension(originalImagePath);
            images.Add(originalImage);
            GetCenter();


            for (int i = 0; i < images.Count; i++)
            {
                foreach (ExtensionsData.Side side in Enum.GetValues(typeof(ExtensionsData.Side)))
                {
                    GetSidePixels(side, originalImage, GetCenterImage(images[i]));
                }
            }
            var iconPos = findBorder.GetCenterPositionIcon(images[0]);
            ModifiyImage(images[0], iconPos);
        }

        public Dictionary<string, Dictionary<ExtensionsData.Side, List<int>>> GetTargetIcons()
        {
            return nameAndPixels;
        }

        public void AddAllImages()
        {
            // получаем файлы из папки и добавляекм
        }

        public Vector2 GetCenterImage(Bitmap image)
        {
            int centerX = image.Width / 2;
            int centerY = image.Height / 2;

            return new Vector2(centerX, centerY);
        }



        public void GetSidePixels(ExtensionsData.Side side, Bitmap image, Vector2 center)
        {

            Dictionary<ExtensionsData.Side, List<int>> sideAndPixels = new();
            List<int> pixels = new();

            int x = (int)center.X;
            int y = (int)center.Y;

            int xOffset = 0;
            int yOffset = 0;

            switch (side)
            {
                case ExtensionsData.Side.Left:
                    xOffset = -1;
                    break;
                case ExtensionsData.Side.Right:
                    xOffset = 1;
                    break;
                case ExtensionsData.Side.Top:
                    yOffset = 1;
                    break;
                case ExtensionsData.Side.Bottom:
                    yOffset = -1;
                    break;
            }


            for (int i = 0; i < countSaveblePixels; i++)
            {
                Color pixelColor = image.GetPixel(x + xOffset * i, y + yOffset * i);

                pixels.Add(pixelColor.ToArgb());
            }

            sideAndPixels.Add(side, pixels);

            if (!nameAndPixels.ContainsKey(imageName))
            {
                nameAndPixels.Add(imageName, sideAndPixels);
            }
            else
            {
                nameAndPixels[imageName].Add(side, pixels);
            }


        }

        public void GetCenter()
        {
            FindBorder findBorder = new();
            // var borders = findBorder.GetPosition(images[0]);
        }

        public void ModifiyImage(Bitmap image, Dictionary<int, Vector2> iconAndPos)
        {
            Bitmap copyImage = new Bitmap(image);

            for (int i = 0; i < iconAndPos.Count; i++)
            {
                var x = (int)iconAndPos[i + 1].X;
                var y = (int)iconAndPos[i + 1].Y;

                for (int j = 0; j < 10; j++)
                {
                    // top
                    copyImage.SetPixel(x, y - j, Color.FromArgb(255, 255, 255, 255));
                    //bot
                    copyImage.SetPixel(x, y + j, Color.FromArgb(255, 255, 255, 255));

                    //right
                    copyImage.SetPixel(x + j, y, Color.FromArgb(255, 255, 255, 255));
                    //left
                    copyImage.SetPixel(x - j, y, Color.FromArgb(255, 255, 255, 255));


                }


            }

            Extensions.SaveImage(copyImage, "TEST_ConcecratedGround");


        }
    }
}
