using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static PathOfVision.FindBorder;

namespace PathOfVision
{
    public class FindIcon
    {
        public GetScreenArea screenArea = new();
        public FindBorder findBorder = new();
        public Bitmap screen;
        public Dictionary<int, Vector2> iconAndPos = new();

        public CutIcons cutIcons = new CutIcons();

        public List<Bitmap> targetIcons = new();

        public Dictionary<string, Dictionary<ExtensionsData.Side, List<int>>> nameAndPixels = new();

        public List<Bitmap> cropedIcons = new();



        public FindIcon(Dictionary<string, Dictionary<ExtensionsData.Side, List<int>>> targetIcons)
        {
            nameAndPixels = targetIcons;
        }

        public void Get()
        {
            //screen = screenArea.GetScreen(GetScreenArea.ScreenAreaVariant.BuffAreaTop);
            //Extensions.SaveImage(screen, "ScreenArea");

            iconAndPos = findBorder.GetCenterPositionIcon(screen);

            ModifiyImage(screen);

           // cropedIcons=cutIcons.GetIcons(screen,iconAndPos);
        }

        public void ModifiyImage(Bitmap image)
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



            Extensions.SaveImage(copyImage, "ScreenArea_CenterIcons");

        }

        public void CheckScreen()
        {
            for (int i = 0; i < iconAndPos.Count; i++)
            {

            }
        }
    }
}
