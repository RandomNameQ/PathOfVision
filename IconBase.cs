using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PathOfVision
{
    public class IconBase
    {
      public enum ImageSkillType
        {
            Buff,
            Debuff,
            Minion,
            Brand
        }

        public ImageSkillType imageSkillType;

        public string name;


        public enum FrameSides
        {
            Left,Right,Top,Down
        }

        public Dictionary<FrameSides, List<int>> framesPixels = new();
        public Dictionary<int, List<int>> iconPixels = new();

        public Dictionary<FrameSides, List<int>> BuffFrame = new();
        public Dictionary<FrameSides, List<int>> DebuffFrame = new();

       

        public IconBase()
        {
            Debug.WriteLine("asdas123");
            Console.WriteLine("12312");
            LoadImage("Images/Frames/Buff.png", BuffFrame);
        }

        private void LoadImage(string imagePath, Dictionary<FrameSides, List<int>> frameDictionary)
        {
            // Load the image
            Bitmap image = new Bitmap(imagePath);

            // Iterate through the sides and add pixels to the corresponding list
            foreach (FrameSides side in Enum.GetValues(typeof(FrameSides)))
            {
                List<int> pixels = GetPixelsFromSide(image, side);
                frameDictionary.Add(side, pixels);
            }
        }

        private List<int> GetPixelsFromSide(Bitmap image, FrameSides side)
        {
            List<int> pixels = new List<int>();

            // Adjust these values based on the specific side you want to extract pixels from
            int startX = 0, startY = 0, width = 10, height = image.Height;

            switch (side)
            {
                case FrameSides.Top:
                    // Set the parameters for the top side
                    break;
                case FrameSides.Right:
                    // Set the parameters for the right side
                    break;
                case FrameSides.Down:
                    // Set the parameters for the bottom side
                    break;
                case FrameSides.Left:
                    // Set the parameters for the left side
                    break;
                default:
                    break;
            }

            // Extract pixels from the specified side
            for (int y = startY; y < startY + height; y++)
            {
                for (int x = startX; x < startX + width; x++)
                {
                    pixels.Add(image.GetPixel(x, y).ToArgb());
                }
            }

            return pixels;
        }
    }

}
