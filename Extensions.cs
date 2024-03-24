using Newtonsoft.Json;
using PathOfVision.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static PathOfVision.SampleIconData;

namespace PathOfVision
{
    public static class Extensions
    {
        public static void SaveImage(Bitmap imageOut, string name = "default", string path = "", bool isRandomName = false)
        {
            Bitmap image = new Bitmap(imageOut);

            if (isRandomName)
            {
                // Generate a random file name using Guid
                string randomName = $"{name}_{Guid.NewGuid()}.png";
                string fullPath = Path.Combine(path, randomName);
                image.Save(fullPath, ImageFormat.Png);
            }
            else
            {
                string fullPath = Path.Combine(path, $"{name}.png");
                image.Save(fullPath, ImageFormat.Png);
            }
        }

        public static void DeleteFilesInFolder(string folderPath)
        {
            try
            {
                // Получаем список всех файлов в указанной папке
                string[] files = Directory.GetFiles(folderPath);

                // Удаляем каждый файл
                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (IOException ex)
                    {
                        // Если файл не удалось удалить из-за ошибки ввода-вывода (например, файл занят другим процессом),
                        // то выводим сообщение об ошибке и переходим к следующему файлу
                        Console.WriteLine($"Failed to delete file {file}: {ex.Message}");
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        // Если нет разрешения на удаление файла, то выводим сообщение об ошибке и переходим к следующему файлу
                        Console.WriteLine($"No permission to delete file {file}: {ex.Message}");
                    }
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                // Если указанная папка не существует, выводим сообщение об ошибке
                Console.WriteLine($"Folder not found: {ex.Message}");
            }
        }

        public static bool IsColorInRange(int minValue, int maxValue, Color color)
        {
            int targetColorValue = color.ToArgb();
            return targetColorValue >= minValue && targetColorValue <= maxValue;
        }
        public static bool IsColorInRange(int minValue, int MaxValue, int targetNumber)
        {
            return false;
        }


        public static Dictionary<string, Bitmap> LoadIconFromFolder(string folderPath)
        {
            Dictionary<string, Bitmap> icons = new Dictionary<string, Bitmap>();

            if (Directory.Exists(folderPath))
            {
                string[] imageFiles = Directory.GetFiles(folderPath, "*.png"); // Change the file extension as needed

                foreach (string imagePath in imageFiles)
                {
                    try
                    {
                        string fileName = Path.GetFileNameWithoutExtension(imagePath);
                        Bitmap icon = new Bitmap(imagePath);
                        icons.Add(fileName, icon);
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions, e.g., if the file is not a valid image
                        Console.WriteLine($"Error loading image: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Folder does not exist.");
            }

            return icons;
        }


        public static void SaveJson(string path, string jsonData)
        {
            File.WriteAllText(path, jsonData);
        }

        public static IconData GetIconData(string iconName, bool needSaveNewData = false)
        {
            foreach (var icon in GlobalData.IconDataList)
            {
                if (needSaveNewData)
                {
                    if (icon.iconName == iconName)
                    {
                        return icon;
                    }
                }
                else
                {
                    if (icon.iconName == iconName)
                    {
                        return icon;
                    }
                }

            }
            return default;
        }

        public static Rect GetAbsolutePlacement1(this FrameworkElement element, bool relativeToScreen = false)
        {
            var absolutePos = element.PointToScreen(new System.Windows.Point(0, 0));
            if (relativeToScreen)
            {
                return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
            }
            var posMW = Application.Current.MainWindow.PointToScreen(new System.Windows.Point(0, 0));
            absolutePos = new System.Windows.Point(absolutePos.X - posMW.X, absolutePos.Y - posMW.Y);
            return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
        }
        public static Rect GetAbsolutePlacement(this FrameworkElement element, bool relativeToScreen = false)
        {
            var absolutePos = element.PointToScreen(new System.Windows.Point(0, 0));
            if (relativeToScreen)
            {
                return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
            }
            var posMW = Application.Current.MainWindow.PointToScreen(new System.Windows.Point(0, 0));
            absolutePos = new System.Windows.Point(absolutePos.X, absolutePos.Y);
            return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
        }


        public static void SaveNewIcon(Bitmap icon)
        {
            string directoryPath = Directory.GetCurrentDirectory();
            string path = $"{directoryPath}\\Icon\\AllCuttedIcon";

            string randomName = "";
            if (true)
            {
                SaveImage(icon, randomName, path);
            }

        }

        public static bool DifferenceBetweenColor(int sourceNumber, int numberToCheck, int difference)
        {
            int lowerBound = sourceNumber - difference;
            int upperBound = sourceNumber + difference;

            if (numberToCheck >= lowerBound && numberToCheck <= upperBound)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool CompareArgbColor(Color mainColor, Color checkColor)
        {
            int redDifference = Math.Abs(mainColor.R - checkColor.R);
            int greenDifference = Math.Abs(mainColor.G - checkColor.G);
            int blueDifference = Math.Abs(mainColor.B - checkColor.B);

            int red = 5;
            int blue = 5;
            int green = 5;
            if (mainColor.R > mainColor.G && mainColor.R > mainColor.B)
            {
                red = 10;
            }
            else if (mainColor.G > mainColor.R && mainColor.G > mainColor.B)
            {
                green = 10;
            }
            else if (mainColor.B > mainColor.R && mainColor.B > mainColor.G)
            {
                blue = 10;
            }


            if (redDifference <= red && greenDifference <= green && blueDifference <= blue)
            {
                return true;

            }
            return false;
        }



    }
}
