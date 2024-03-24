using PathOfVision.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Path = System.IO.Path;
using Rectangle = System.Drawing.Rectangle;

namespace PathOfVision
{
    public class CutIcons
    {
        GetScreenArea GetScreenArea = new();
        FindBorder GetBorder = new FindBorder();


      

        public Dictionary<Bitmap, Vector2> GetIcons(Bitmap image, Dictionary<int, Vector2> iconAndCenter, bool needCreateImage = false)
        {
            Dictionary<Bitmap, Vector2> icons = new();

            if (iconAndCenter == null)
            {
                Debug.WriteLine("Не могу получить иконки из цельного изображения.");
                return icons;

            }

            for (int i = 0; i < iconAndCenter.Count; i++)
            {
                int iconWidth = 63;
                int iconHeight = 63;

                int centerX = (int)iconAndCenter[i + 1].X;
                int centerY = (int)iconAndCenter[i + 1].Y;

                int x = centerX - iconWidth / 2;
                int y = centerY - iconHeight / 2;

                // Ensure the cropping coordinates are within the image bounds
                x = Math.Max(0, x);
                y = Math.Max(0, y);
                int width = Math.Min(iconWidth, image.Width - x);
                int height = Math.Min(iconHeight, image.Height - y);

                Rectangle cropRect = new Rectangle(x, y, width, height);
                Bitmap icon = new Bitmap(cropRect.Width, cropRect.Height);

                icons.Add(icon, iconAndCenter[i + 1]);

                // Save the cropped icon image
                if (needCreateImage)
                {
                    using (Graphics g = Graphics.FromImage(icon))
                    {
                        g.DrawImage(image, new Rectangle(0, 0, icon.Width, icon.Height), cropRect, GraphicsUnit.Pixel);
                    }
                    Extensions.SaveImage(icon, "icon" + i);

                }
            }




            return icons;
        }

        public Dictionary<string, string> CutIcon(Bitmap screenArea, string iconName, int xScreenPos, int yScreenPos, bool isNumberIcon = true, bool needSaveIcon = true)
        {
            int iconWidth = 63;
            int iconHeight = 63;

            if (isNumberIcon)
            {
                iconHeight += 20; // Увеличиваем высоту на 10 пикселей, если иконка с числом
            }
            yScreenPos += 10;

            int x = xScreenPos - iconWidth / 2;
            int y = yScreenPos - iconHeight / 2;

            // Ensure the cropping coordinates are within the image bounds
            x = Math.Max(0, x);
            y = Math.Max(0, y);
            int width = Math.Min(iconWidth, screenArea.Width - x)+1;
            int height = Math.Min(iconHeight, screenArea.Height - y);

            Rectangle cropRect = new Rectangle(x, y, width, height);
            Bitmap icon = new Bitmap(cropRect.Width, cropRect.Height);

            // Копируем часть изображения из screenArea в новый Bitmap
            using (Graphics g = Graphics.FromImage(icon))
            {
                g.DrawImage(screenArea, 0, 0, cropRect, GraphicsUnit.Pixel);
            }

            // Сохраняем изображение, если требуется
            string imagePath = string.Empty;
            string fileName = "";
            if (needSaveIcon)
            {

                // Получаем текущую директорию
                string directoryPath = Directory.GetCurrentDirectory();
                fileName = $"icon_{DateTime.Now:yyyyMMddHHmmssfff}.png";

                // Сохраняем изображение в текущей директории
                imagePath = Path.Combine($"{directoryPath}/Icon/TempIcons", fileName);

                Extensions.SaveImage(icon, imagePath);
            }

            // Возвращаем путь к сохраненному файлу
            Dictionary<string, string> pathAndName = new();
            pathAndName.Add($"{imagePath}.png", fileName);
            return pathAndName;
        }


        public void CutFullIcon(Bitmap gameScreen, Dictionary<int, Vector2> iconsPositions)
        {
            Debug.WriteLine($"Вырезаю иконку.");

            // Bitmap image = new Bitmap(imageCopy);
            int iconWidth = 63;
            int iconHeight = 63;
            int i = 0;
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Icon", "AllCuttedIcon");
            foreach (var iconPos in iconsPositions)
            {
                i++;
                int centerX = (int)iconPos.Value.X + 31; // Позиция X центра иконки
                int centerY = (int)iconPos.Value.Y; // Позиция Y центра иконки

                // Определяем координаты левого верхнего угла области, которую нам нужно вырезать
                int startX = centerX - (iconWidth / 2);
                int startY = centerY - (iconHeight / 2);

                // Создаем прямоугольную область для вырезания
                Rectangle sourceRectangle = new Rectangle(startX, startY, iconWidth, iconHeight);
                Bitmap icon = new Bitmap(sourceRectangle.Width, sourceRectangle.Height);

                using (Graphics g = Graphics.FromImage(icon))
                {
                    g.DrawImage(gameScreen, 0, 0, sourceRectangle, GraphicsUnit.Pixel);
                }


                bool isNewIcon = false;

                if (!IsDuplicateIcon(icon))
                {

                    Extensions.SaveImage(icon, "s", directoryPath, true);
                    GlobalData.cutedIcons.Add(icon);
                }

                //using (Bitmap croppedImage = gameScreen.Clone(sourceRectangle, gameScreen.PixelFormat))
                //{
                    
                //}

            }
        }

        public bool IsDuplicateIcon(Bitmap image)
        {
            Bitmap decreasedImage = DecreaseIconSize(image);
           
            foreach (Bitmap savedImage in GlobalData.cutedIcons)
            {

                Bitmap decreasedSavedImage = DecreaseIconSize(savedImage);
                
                if (ImageComparer_Hash.CompareMiniIconForCutIcon(decreasedImage, decreasedSavedImage))
                //if (ImageComparer_Hash.CompareMiniIcon(image, savedImage))
                {
                    // Found a duplicate
                    return true;
                }
            }
            return false; // No duplicates found
        }



        public Bitmap DecreaseIconSize(Bitmap image)
        {

            int centerX = image.Width / 2;
            int centerY = image.Height / 2;

            int startX = Math.Max(0, centerX - 20);
            int startY = Math.Max(0, centerY - 20);
            int width = Math.Min(image.Width - startX, 31);  // Ensure the cropped region doesn't exceed the image boundaries
            int height = Math.Min(image.Height - startY, 31);

            Rectangle sourceRectangle = new Rectangle(startX, startY, width, height);

            Bitmap croppedImage = image.Clone(sourceRectangle, image.PixelFormat);
            return croppedImage;
        }



        public Dictionary<Vector2, Bitmap> GetIconsFromGameScreen(Bitmap gameScreen, Dictionary<int, Vector2> iconsPositions)
        {
            Dictionary<Vector2, Bitmap> icons = new Dictionary<Vector2, Bitmap>();

            int iconWidth = 63;
            int iconHeight = 63;
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Icon", "AllCuttedIcon");

            foreach (var iconPos in iconsPositions)
            {
                int centerX = (int)iconPos.Value.X + 31; // Позиция X центра иконки
                int centerY = (int)iconPos.Value.Y; // Позиция Y центра иконки

                // Определяем координаты левого верхнего угла области, которую нам нужно вырезать
                int startX = centerX - (iconWidth / 2);
                int startY = centerY - (iconHeight / 2);

                // Создаем прямоугольную область для вырезания
                Rectangle sourceRectangle = new Rectangle(startX, startY, iconWidth, iconHeight);

                // Вырезаем иконку из игрового экрана
                Bitmap icon = new Bitmap(sourceRectangle.Width, sourceRectangle.Height);
                using (Graphics g = Graphics.FromImage(icon))
                {
                    g.DrawImage(gameScreen, 0, 0, sourceRectangle, GraphicsUnit.Pixel);
                }

                // Сохраняем центр иконки и соответствующую ему иконку
                Vector2 center = new Vector2(centerX, centerY);
                if (!icons.ContainsKey(center))
                {
                    icons.Add(center, icon);

                }
                else
                {
                    Debug.WriteLine("яебу ошибка");
                    //icons[center] = icon;
                }
            }

            return icons;
        }





    }
}
