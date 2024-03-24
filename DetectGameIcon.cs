using PathOfVision.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows;
using Application = System.Windows.Application;

namespace PathOfVision
{
    /*
     * вводные данные:
     * по дефолту мы начинаем поиск слева сверху и идем > право+низ.
     * по дефолту ширина и высота иконки 63 пикселя
     * 
     * 
     * _____________________________________________
     * дполнение: 
     * не все иконки 63 на 63
     * не все типы иконок имеют одинаковый цвет границы
     * ______________________________________________
     * 
     * план такой находим пиксель, который отвечает за границу иконки
     * 
     * если находим, то проверяем пиксель ниже, если такой же цвет, то продолжаем проверять нижние пиксели. 
     * если их количество 30+, то мы нашли левую границу иконки
     * теперь мы перемещаемся на 63 пиксели и если цвет пикселя такой же, то прояврем пиксель снизу, если количество успешных сравнений 30+ то мы нашли првавую часть иконки.
     * 
     * когда нашли левую и правучю часть иконок - получаем центр иконки и берем 100 пикселйе снизу и сверху - это изображения мы будем использовать для сравнения иконок
     * 
     * допустим картинка высотой 300 пикселей, тогда мы должны разделить картинку на 3 части, 1 часть от 0 до 80 пикселей, вторая часть от 80 пикселей до 180 и так далее. 
     * раздение требуется, чтобы находить одну иконку за другой, а не сразу все и по несколько раз.
     * 
     * поэто
     */
    public class DetectGameIcon
    {

        public Bitmap buffBitmap;
        public Bitmap debuffBitmap;
        public Bitmap moltenShelBitmap;
        public Bitmap flask1Bitmap;
        public Bitmap flask2Bitmap;

        public Dictionary<int, Vector2> icons = new();

        public int defaultWidthHeightIconSize = 63;

        public int buffLeftRightBorderPixelColor = -11255757;
        public int defbuffLeftRightBorder = -11255757;

        public int firstIconLineCenter;
        public int secondIconLineCenter;
        public int threeIconLineCenter;

        // Это лево низ
        //public int maxValue = -10506721;
        //public int minValue = -10999999;

        // это лево право низ вверх
        // public int maxValue = -10006721;
        //public int minValue = -10999999;


        // это лево право и низ
        public int maxValue = -10206721;
        public int minValue = -11255757;

        public int left = -10797777;
        public int right = -10468555;
        public int top = -10139334;

        public int flask1Right = -10468555;
        public int flask1Left = -10139334;
        public int flaskTrue = -10731986;




        // это находит вверхню часть иконки
        //public int maxValue = -10006721;
        //public int minValue = -10206721;



        public List<int> iconPixels = new();
        public List<Color> iconColorsPixels = new();

        public void CreatePixelsIconList()
        {
            iconPixels.Add(left);
            iconPixels.Add(flaskTrue);


            // это молтеншел в позиции 5+ потому что цвет границы другой, пиксель взять из скриншота
            Color customColor = Color.FromArgb(255, 91, 61, 47);
            iconPixels.Add(customColor.ToArgb());

            iconColorsPixels.Add(customColor);


            customColor = Color.FromArgb(255, 72, 54, 43);
            iconColorsPixels.Add(customColor);


            iconPixels.Add(customColor.ToArgb());
        }

        Dictionary<ExtensionsData.Side, List<int>> sideAndPixels = new();



        public GetScreenArea screenArea = new();
        public CutIcons cutIcons = new();
        public FindBorder findBorder = new();


        public Bitmap gameAreaScreen;


        public int startScanHeightPosition;
        public int countIconLines;
        public bool isBorderFounded;
        public int countCheckWithoutDetectBorder;
        public int modifyHeight;


        public bool isCutIcon;


        public int widthX, heightY;
        public Dictionary<Vector2, Bitmap> currenVisibleIcons = new();

        public DetectGameIcon()
        {
            CreatePixelsIconList();
        }

        public void AllThings(bool isCutIcon)
        {
            this.isCutIcon = isCutIcon;
            // LoadFrames();
            // TryFindBorderColor();

            GetGameScreen();
            ReadScreen();
            if (isCutIcon)
            {
                cutIcons.CutFullIcon(gameAreaScreen, icons);
            }
            currenVisibleIcons = cutIcons.GetIconsFromGameScreen(gameAreaScreen, icons);
            CompareHash();
            //Application.Current.MainWindow.
            var mainWindow = App.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.CountDetectedIcons.Content = $"Detected {icons.Count} icons";
            }
            else
            {
                // Handle the case where the main window is not set
            }
            //Debug.WriteLine($"Нашел {icons.Count} иконок, detectGame");

        }
        public void LoadFrames()
        {
            string directoryPath = Directory.GetCurrentDirectory();

            buffBitmap = new Bitmap(Path.Combine(directoryPath, "Images", "Frames", "Buff.png"));
            debuffBitmap = new Bitmap(Path.Combine(directoryPath, "Images", "Frames", "Debuff.png"));
            moltenShelBitmap = new Bitmap(Path.Combine(directoryPath, "Images", "Frames", "MoltenShell.png"));
            flask1Bitmap = new Bitmap(Path.Combine(directoryPath, "Images", "Frames", "flask1.png"));
            flask2Bitmap = new Bitmap(Path.Combine(directoryPath, "Images", "Frames", "flask2.png"));
        }

        public void TryFindBorderColor()
        {
            Bitmap border = new(flask1Bitmap);

            int centerX = border.Width / 2;
            int centerY = border.Height / 2;

            int checkedPixels = 25;

            List<int> pixelsRight = new List<int>();
            for (int i = 0; i < 60; i++)
            {
                Color pixelColor = border.GetPixel(border.Width - 1, 0 + i);
                pixelsRight.Add(pixelColor.ToArgb());
                border.SetPixel(border.Width - 1, 0 + i, Color.White);
            }

            List<int> pixelsLeftt = new List<int>();
            for (int i = 0; i < 60; i++)
            {
                Color pixelColor = border.GetPixel(0 + i, 0);
                pixelsLeftt.Add(pixelColor.ToArgb());
                border.SetPixel(0 + i, 0, Color.White);
            }

            Extensions.SaveImage(border, $"test");

            // Получаем 5 пикселей слева направо по центру высоты
            sideAndPixels[ExtensionsData.Side.Left] = GetVerticalPixels(border, border.Width - 1, centerY, checkedPixels, true);


            // Получаем 5 пикселей справа налево по центру высоты
            sideAndPixels[ExtensionsData.Side.Right] = GetVerticalPixels(border, 0, centerY, checkedPixels, false);


            // Получаем 5 пикселей сверху вниз по центру ширины
            sideAndPixels[ExtensionsData.Side.Top] = GetHorizontalPixels(border, centerX, 0, checkedPixels, true);


            // Получаем 5 пикселей снизу вверх по центру ширины
            sideAndPixels[ExtensionsData.Side.Bottom] = GetHorizontalPixels(border, centerX, border.Height - 1, checkedPixels, false);

        }

        private List<int> GetVerticalPixels(Bitmap bitmapCopy, int startX, int startY, int count, bool isRightToLeft)
        {
            Bitmap bitmap = new(bitmapCopy);
            List<int> pixels = new List<int>();

            int step = isRightToLeft ? -1 : 1;

            for (int i = 0; i < count; i++)
            {
                int x = startX + i * step;
                Color pixelColor = bitmap.GetPixel(x, startY);
                pixels.Add(pixelColor.ToArgb());
                bitmap.SetPixel(x, startY, Color.White);
            }

            Extensions.SaveImage(bitmap, $"{startY}{startX}");

            return pixels;
        }

        private List<int> GetVerticalPixels_Test(Bitmap bitmapCopy, int startX, int startY, int count, bool isRightToLeft)
        {
            Bitmap bitmap = new(bitmapCopy);
            List<int> pixels = new List<int>();

            int step = isRightToLeft ? 1 : -1;

            for (int i = 0; i < count; i++)
            {
                int y = startY + i * step;
                Color pixelColor = bitmap.GetPixel(startX, y);
                pixels.Add(pixelColor.ToArgb());
                bitmap.SetPixel(startX, y, Color.White);
            }

            Extensions.SaveImage(bitmap, $"{startY}{startX}");

            return pixels;
        }
        private List<int> GetHorizontalPixels(Bitmap bitmapCopy, int startX, int startY, int count, bool isTopToBottom)
        {
            Bitmap bitmap = new(bitmapCopy);
            List<int> pixels = new List<int>();

            int step = isTopToBottom ? 1 : -1;

            for (int i = 0; i < count; i++)
            {
                int y = startY + i * step;
                Color pixelColor = bitmap.GetPixel(startX, y);
                pixels.Add(pixelColor.ToArgb());
                bitmap.SetPixel(startX, y, Color.White);
            }

            Extensions.SaveImage(bitmap, $"{startY}{startX}");

            return pixels;
        }

        public void GetGameScreen()
        {
            gameAreaScreen = screenArea.GetScreen();
            GlobalData.screenArea = gameAreaScreen;
            Extensions.SaveImage(gameAreaScreen, "ScreenArea");
        }

        
        public void ReadScreen()
        {
            countIconLines = 0;
            modifyHeight = 0;
            ReadWidth();
            //ColorBorderTest();


            //Extensions.SaveImage(img, "area");
        }

        public void ReadWidth()
        {
            for (widthX = 0; widthX < gameAreaScreen.Width; widthX++)
            {
                //if (isBorderFounded)
                //{
                //    countCheckWithoutDetectBorder++;
                //    if (countCheckWithoutDetectBorder > 20)
                //    {
                //        countIconLines++;
                //        modifyHeight = 90 * countIconLines;
                //        countCheckWithoutDetectBorder = 0;
                //        widthX = 0;
                //        isBorderFounded = false;
                //    }
                //}
                ReadHeight();

            }
        }
        public void ReadHeight()
        {
            int countChekedPixels = 0;
            for (heightY = modifyHeight; heightY < gameAreaScreen.Height; heightY++)
            {
                //if (countChekedPixels > 20) return;
                //countChekedPixels++;


                Color pixelColor = gameAreaScreen.GetPixel(widthX, heightY);
                int pixelArgb = pixelColor.ToArgb();



                for (int i = 0; i < iconColorsPixels.Count; i++)
                {
                    if (Extensions.CompareArgbColor(iconColorsPixels[i], pixelColor))
                    {
                        if (IsBorder(this.widthX, this.heightY, pixelColor))
                        {
                            countCheckWithoutDetectBorder = 0;
                            this.widthX += 65;
                            if (this.widthX + 1 >= gameAreaScreen.Width)
                            {
                                countIconLines++;
                                modifyHeight = 90 * countIconLines;
                                widthX = 0;
                                return;
                            }
                            isBorderFounded = true;
                            // если достиг максимума, то значит первую линию иконок просканировал, теперь надо сканировать второй ряд
                        }
                    }


                    else
                    {

                        if (this.widthX + 1 >= gameAreaScreen.Width)
                        {
                            countIconLines++;
                            modifyHeight = 90 * countIconLines;
                            widthX = 0;
                            return;
                        }
                    }
                }

                ////нашел границу иконки
                //for (global::System.Int32 i = 0; i < iconPixels.Count; i++)
                //{
                //    if (pixelArgb == iconPixels[i])
                //    {
                //        if (IsBorder(this.widthX, this.heightY, pixelColor))
                //        {
                //            countCheckWithoutDetectBorder = 0;
                //            this.widthX += 65;
                //            if (this.widthX + 1 >= gameAreaScreen.Width)
                //            {
                //                countIconLines++;
                //                modifyHeight = 90 * countIconLines;
                //                widthX = 0;
                //                return;
                //            }
                //            isBorderFounded = true;
                //            // если достиг максимума, то значит первую линию иконок просканировал, теперь надо сканировать второй ряд
                //        }
                //    }

                //    //if (Extensions.IsColorInRange(minValue, maxValue, pixelColor))
                //    //{

                //    //    if (IsBorder(this.widthX, this.heightY, pixelArgb))
                //    //    {
                //    //        countCheckWithoutDetectBorder = 0;
                //    //        this.widthX += 65;
                //    //        if (this.widthX + 1 >= gameAreaScreen.Width)
                //    //        {
                //    //            countIconLines++;
                //    //            modifyHeight = 90 * countIconLines;
                //    //            widthX = 0;
                //    //            return;
                //    //        }
                //    //        isBorderFounded = true;
                //    //        // если достиг максимума, то значит первую линию иконок просканировал, теперь надо сканировать второй ряд
                //    //    }

                //    //}
                //    else
                //    {

                //        if (this.widthX + 1 >= gameAreaScreen.Width)
                //        {
                //            countIconLines++;
                //            modifyHeight = 90 * countIconLines;
                //            widthX = 0;
                //            return;
                //        }
                //    }
            }



        }


        // тут провряем является ли текущий найденный пиксель границой иконки
        // так как сканирую изображения слева сверху, то искать буду пиксели снизу, а не сверху
        public bool IsBorder1(int widthX, int heightY, Color pixelColorOut)
        {

            int check = 0;
            List<Vector2> pos = new();

            int pixelArgb = pixelColorOut.ToArgb();
            for (global::System.Int32 y = 0; y < 60; y++)
            {
                if (heightY + y >= gameAreaScreen.Height)
                {
                    break;
                }
                Color pixelColor = gameAreaScreen.GetPixel(widthX, heightY + y);

                int currentPixel = pixelColor.ToArgb();

                if (pixelArgb != currentPixel)
                {
                    break;
                }
                bool isRightSideExists = false;
                // чекаем правую сторону иконки, чтобы увеличить точность
                for (global::System.Int32 i = 0; i < 4; i++)
                {
                    if (widthX + 61 + i < gameAreaScreen.Width)
                    {


                        //if (Extensions.IsColorInRange(minValue, maxValue, pixelColor))
                        if (pixelArgb == left)
                        {
                            isRightSideExists = true;
                            // break;
                        }
                    }

                }

                if (!isRightSideExists)
                {
                    //  break;
                }

                Vector2 newPos;
                newPos.X = widthX;
                newPos.Y = heightY + y;
                pos.Add(newPos);
                check++;

            }

            // если нашел левую сторону иконки 
            if (check >= 50)
            {

                Vector2 center = CalculateCenter(pos);

                // добавлям номер кионки и позицию левой стороны в центре иконки
                if (icons.Count == 0)
                {

                    icons.Add(0, center);
                }
                else
                {
                    int maxKey = icons.Keys.Max();
                    int newKey = maxKey + 1;
                    icons.Add(newKey, center);
                }

                return true;
            }
            return false;
        }

        public bool IsBorder(int widthX, int heightY, Color pixelColorOut)
        {

            int check = 0;
            List<Vector2> pos = new();

            int pixelArgb = pixelColorOut.ToArgb();
            for (global::System.Int32 y = 0; y < 60; y++)
            {
                if (heightY + y >= gameAreaScreen.Height)
                {
                    break;
                }
                Color pixelColor = gameAreaScreen.GetPixel(widthX, heightY + y);

                int currentPixel = pixelColor.ToArgb();

                if (pixelArgb != currentPixel)
                {
                    break;
                }
                bool isRightSideExists = false;
                // чекаем правую сторону иконки, чтобы увеличить точность
                for (global::System.Int32 i = 0; i < 4; i++)
                {
                    if (widthX + 61 + i < gameAreaScreen.Width)
                    {


                        //if (Extensions.IsColorInRange(minValue, maxValue, pixelColor))
                        if (pixelArgb == left)
                        {
                            isRightSideExists = true;
                            // break;
                        }
                    }

                }

                if (!isRightSideExists)
                {
                    //  break;
                }

                Vector2 newPos;
                newPos.X = widthX;
                newPos.Y = heightY + y;
                pos.Add(newPos);
                check++;

            }

            // если нашел левую сторону иконки 
            if (check >= 50)
            {

                Vector2 center = CalculateCenter(pos);

                // добавлям номер кионки и позицию левой стороны в центре иконки
                if (icons.Count == 0)
                {

                    icons.Add(0, center);
                }
                else
                {
                    int maxKey = icons.Keys.Max();
                    int newKey = maxKey + 1;
                    icons.Add(newKey, center);
                }

                return true;
            }
            return false;
        }
        private Vector2 CalculateCenter(List<Vector2> positions)
        {
            float sumX = 0;
            float sumY = 0;

            foreach (Vector2 pos in positions)
            {
                sumX += pos.X;
                sumY += pos.Y;
            }

            float centerX = sumX / positions.Count;
            float centerY = sumY / positions.Count;

            return new Vector2(centerX, centerY);
        }
        public void ColorBorderTest()
        {
            for (global::System.Int32 i = 0; i < icons.Count; i++)
            {
                for (global::System.Int32 g = 0; g < 63; g++)
                {
                    gameAreaScreen.SetPixel((int)icons[i].X + g, (int)icons[i].Y, Color.FromArgb(255, 255, 255, 255));
                }
            }
        }





        public void CompareHash()
        {

            foreach (var icon in GlobalData.searchableIcons)
            {
                icon.isDetected = false;
            }

            foreach (var icon in currenVisibleIcons)
            {
                foreach (var entry in currenVisibleIcons)
                {
                    Bitmap currentGameIcon = entry.Value;
                    Vector2 currentPosition = entry.Key;

                    CheckImage_Hash(currentGameIcon, currentPosition);
                }
            }

            UpdateDataInThreads();

        }
        public void CheckImage_Hash(Bitmap currentGameIcon, Vector2 currentPosition)
        {
            for (int j = 0; j < GlobalData.searchableIcons.Count; j++)
            {
                currentGameIcon = cutIcons.DecreaseIconSize(currentGameIcon);
                // Extensions.SaveImage(currentGameIcon,"gameIcon");


                var comparedIcon = cutIcons.DecreaseIconSize(GlobalData.searchableIcons[j].sampleIcon);
                //  Extensions.SaveImage(comparedIcon, "target"+j);

                if (ImageComparer_Hash.CompareTest(currentGameIcon, comparedIcon, GlobalData.searchableIcons[j]))
                {
                    // Debug.WriteLine($"Нашел {GlobalData.searchableIcons[j].iconName}");
                    GlobalData.searchableIcons[j].isDetected = true;

                    GlobalData.searchableIcons[j].gameIconPositionX = (int)currentPosition.X;
                    GlobalData.searchableIcons[j].gameIconPositionY = (int)currentPosition.Y;

                    if (GlobalData.searchableIcons[j].isShowAllResults)
                    {

                    }
                    else
                    {
                        return;
                    }
                }



                //if (GlobalData.searchableIcons[j].isHalfComapre)
                //{
                //    if (ImageComparer_Hash.AreImagesSimilar_HalfCompare(currentGameIcon, GlobalData.searchableIcons[j].sampleIcon))
                //    {
                //        GlobalData.searchableIcons[j].isDetected = true;

                //        GlobalData.searchableIcons[j].gameIconPositionX = (int)currentPosition.X;
                //        GlobalData.searchableIcons[j].gameIconPositionY = (int)currentPosition.Y;


                //        return;
                //    }
                //}
                //else
                //{
                //    if (ImageComparer_Hash.AreImagesSimilar(currentGameIcon, GlobalData.searchableIcons[j].sampleIcon))
                //    {
                //        GlobalData.searchableIcons[j].isDetected = true;

                //        GlobalData.searchableIcons[j].gameIconPositionX = (int)currentPosition.X;
                //        GlobalData.searchableIcons[j].gameIconPositionY = (int)currentPosition.Y;


                //        return;
                //    }
                //}

            }
        }

        public void UpdateDataInThreads()
        {
            // Получаем доступ к экземпляру createThread_DisplayIcon из класса App
            App app = (App)System.Windows.Application.Current;
            var createThread_DisplayIcon = app.createThread_DisplayIcon;

            if (createThread_DisplayIcon == null) return;
            createThread_DisplayIcon.ChangeDataInThread(GlobalData.searchableIcons);
        }
    }
}


