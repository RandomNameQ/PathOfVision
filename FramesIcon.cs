using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace PathOfVision
{
    public class FramesIcon
    {
        public enum FrameSides
        {
            Left, Right, Top, Down
        }

        public enum FrameType
        {
            Buff, Debuff, Minion, Brand, Another
        }


        public static Dictionary<FrameSides, List<int>> BuffFrame = new();
        public static Dictionary<FrameSides, List<int>> DebuffFrame = new();

        public int defaultColorFrame = -11255757;
        public string defaultCountFrame;



        // left -11255757, right -11255757, top -9350847, down -10006721

        // left -15268350, right -10468555, top -13692149, down -10929105

        public FramesIcon()
        {
            GetImage();
        }

        public void GetImage()
        {
           // string originalImagePath = "Images/Frames/Buff.png";
            string originalImagePath = "Images/Frames/BuffTest.png";
            string outputPath = "Modified_Buff.png";

            // Копируем изображение
            Bitmap originalImage = new Bitmap(originalImagePath);
            Bitmap copiedImage = new Bitmap(originalImage);

            // Модифицируем изображение
            ModifyImage(copiedImage);

            foreach (FrameSides side in Enum.GetValues(typeof(FrameSides)))
            {
               SaveFramePixels(originalImage,side);

            }




            // Сохраняем измененное изображение
            copiedImage.Save(outputPath, ImageFormat.Png);
        }

        public void SaveFramePixels(Bitmap image, FrameSides side)
        {
            int yOffset = 0;
            int xOffset = 0;


            switch (side)
            {
                case FrameSides.Left:
                    xOffset = 1;
                    break;
                case FrameSides.Right:
                    xOffset = image.Width - 1;
                    break;
                case FrameSides.Top:
                    yOffset = 0;
                    break;
                case FrameSides.Down:
                    yOffset = image.Height - 2;
                    break;
            }

            List<int> sidePixels = new List<int>();

            if (side == FrameSides.Down || side == FrameSides.Top)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, yOffset);
                    sidePixels.Add(pixelColor.ToArgb());
                }
            }
            else
            {
                for (int x = 0; x < image.Height; x++)
                {
                    Color pixelColor = image.GetPixel(xOffset, x);
                    sidePixels.Add(pixelColor.ToArgb());
                }
            }

            BuffFrame.Add(side, sidePixels);
        }

        private void ModifyImage(Bitmap image)
        {
            
            // Заменяем верхний ряд пикселей на белый цвет
            for (int x = 0; x < image.Width; x++)
            {
              
                // bot
                image.SetPixel(image.Width/2-10+x, image.Height-2, Color.FromArgb(255, 255, 255, 255));
                //top
                image.SetPixel(image.Width/2-10+x, 0, Color.FromArgb(255, 255, 255, 255));
                if (20==x)
                {
                    break;
                }
            }

            for (int x = 0; x < image.Height; x++)
            {
                // right
                image.SetPixel(image.Width - 1, image.Height / 2 - 10 + x, Color.FromArgb(255, 255, 255, 255));
                image.SetPixel(1, image.Height / 2 - 10 + x, Color.FromArgb(255, 255, 255, 255));
                if (20 == x)
                {
                    break;
                }
            }
        }


        public static Dictionary<FrameSides, List<int>> GetFrame(FrameType frameType)
        {
            return BuffFrame;
        }

        
    }
}
