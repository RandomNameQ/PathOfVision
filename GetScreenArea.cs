using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Numerics;
using System.Windows;
using static PathOfVision.MainWindow;

namespace PathOfVision
{
    public class GetScreenArea
    {

        public ScreenAreaSettings screenAreaSettings;


        public enum ScreenAreaVariant
        {
            FirstIcon, BuffAreaTop, BuffAreaBottom, Custom
        }

        public Bitmap GetScreen()
        {
           
            return CaptureScreenshot();
        }
        

        public Bitmap CaptureScreenshot()
        {
            // Создаем Bitmap для сохранения изображения
            Bitmap screenshot = new Bitmap(screenAreaSettings.screenWidth, screenAreaSettings.screenHeight);

            // Получаем графику из Bitmap
            using (Graphics graphics = Graphics.FromImage(screenshot))
            {
                // Копируем содержимое экрана в Bitmap, используя координаты и размеры ScreenArea
                graphics.CopyFromScreen(screenAreaSettings.distanceFromLeft, screenAreaSettings.disatnceFromTop, 0, 0, new System.Drawing.Size(screenAreaSettings.screenWidth, screenAreaSettings.screenHeight), CopyPixelOperation.SourceCopy);
            }

            return screenshot;
          
        }

        public void UpdateSettings(ScreenAreaSettings screenAreaSettings)
        {
            this.screenAreaSettings = screenAreaSettings;
        }
       
    }




}
