using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PathOfVision.SampleIconData;

namespace PathOfVision.Data
{
    public class GlobalData
    {
        private static readonly object lockObject = new object();
        private static List<IconData> iconDataList = new List<IconData>();
        public static List<GameIconState> searchableIcons = new List<GameIconState>();

        public static List<Bitmap> cutedIcons = new();

        public static Bitmap screenArea;

        public static List<IconData> IconDataList
        {
            get
            {
                lock (lockObject)
                {
                    return iconDataList;
                }
            }
            set
            {
                lock (lockObject)
                {
                    iconDataList = value;
                    TransferDataToList();
                }
            }
        }

        public static void TransferDataToList()
        {
            LoadIconFromFolder();

            searchableIcons.Clear();
            foreach (var icon in iconDataList)
            {
                if (icon.isSearcheabled)
                {
                    GameIconState createdIcon = new GameIconState();
                    createdIcon.isDetected = false;
                    createdIcon.isIconWichNumbers = icon.isIconWichNumbers;
                    createdIcon.isHalfComapre = icon.isNeedHalfCompare;
                    createdIcon.createdIconPositionX = icon.x;
                    createdIcon.createdIconPositionY = icon.y;
                    createdIcon.sampleIcon = icon.sampleImage;
                    createdIcon.iconName = icon.iconName;
                    createdIcon.imagePath = icon.imagePath;

                    createdIcon.height = icon.height;
                    createdIcon.width = icon.width;

                    createdIcon.opacity = icon.opacity;

                    createdIcon.argbDifferences = icon.argbDifferences;

                    createdIcon.isTopCheck = icon.isTopCheck;
                    createdIcon.isBotCheck = icon.isBotCheck;
                    createdIcon.isLeftCheck = icon.isLeftCheck;
                    createdIcon.isRightCheck = icon.isRightCheck;

                    createdIcon.isShowAllResults = icon.isShowAllResults;

                    searchableIcons.Add(createdIcon);
                }
            }
        }

        public static void LoadIconFromFolder()
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Icon", "AllCuttedIcon");

            // Проверяем существует ли папка
            if (Directory.Exists(directoryPath))
            {
                // Получаем список файлов в папке
                string[] files = Directory.GetFiles(directoryPath);

                // Проходим по каждому файлу
                foreach (string file in files)
                {
                    try
                    {
                        // Загружаем изображение из файла
                        Bitmap icon = new Bitmap(file);

                        // Добавляем иконку в список
                        cutedIcons.Add(icon);
                    }
                    catch (Exception ex)
                    {
                        // Обрабатываем ошибку загрузки файла
                        Console.WriteLine($"Ошибка при загрузке файла {file}: {ex.Message}");
                    }
                }
            }
            else
            {
                // Выводим сообщение об ошибке, если папка не существует
                Console.WriteLine($"Папка {directoryPath} не существует");
            }
        }



        public class GameIconState
        {
            public bool isDetected, isIconWichNumbers, isHalfComapre;
            public string iconName;
            public string imagePath;
            public int gameIconPositionX, gameIconPositionY;
            public int createdIconPositionX, createdIconPositionY;
            public Bitmap sampleIcon;

            public int height,width;
            public int opacity;
            public int argbDifferences;

            public bool isTopCheck;
            public bool isBotCheck;
            public bool isLeftCheck;
            public bool isRightCheck;

            public bool isShowAllResults;
        }



        public static void SaveDataIconList()
        {
            string jsonPath = "Icon\\IconData.json";
            string jsonData = JsonConvert.SerializeObject(iconDataList, Formatting.Indented);
            Extensions.SaveJson(jsonPath, jsonData);
        }

    }
}
