using Newtonsoft.Json;
using PathOfVision.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace PathOfVision
{
    public class SampleIconData
    {
        public string iconPath = "Icon\\Buffs";
        public string jsonPath = "Icon\\IconData.json";
        public List<IconData> iconDataList = new();







        public struct IconData
        {
            public string iconName, imagePath;
            public bool isSearcheabled, isDiscovered, isIconWichNumbers;
            public bool isSoundIfDetected, isSoundIfOver;
            public bool isNeedHalfCompare;
            public string soundDetectedPath, soundOverPath;
            public Dictionary<ExtensionsData.Side, List<int>> pixels;
            public int x, y;
            public int opacity;
            public int width, height;
            public int argbDifferences;
            [JsonIgnore]
            public Bitmap sampleImage, destinationImage;

            public bool isTopCheck,isBotCheck,isLeftCheck,isRightCheck;
            public bool isShowAllResults;
        }


        public void CreateJson()
        {

            if (!File.Exists(jsonPath))
            {
                iconDataList.Clear();
                var images = LoadIconsFromFolder();

                foreach (var fileName in images.Keys)
                {
                    Bitmap image = images[fileName];
                    IconData iconData = new IconData();

                    iconData.iconName = fileName;
                    iconData.imagePath = Path.Combine(iconPath, fileName + ".png");
                    iconData.isSearcheabled = false;
                    iconData.isDiscovered = false;
                    iconData.isIconWichNumbers = false;

                    iconData.x = 200;
                    iconData.y = 200;
                    iconData.argbDifferences = 10;

                    iconData.isTopCheck = true;
                    iconData.isBotCheck = true;
                    iconData.isLeftCheck = false;
                    iconData.isRightCheck = false;
                    iconData.isShowAllResults = false;

                    iconData.soundDetectedPath = "";
                    iconData.soundOverPath = "";

                    // Uncomment the line below if you have a method to get pixel data
                    // iconData.pixels = Extensions.GetPixelData(image);

                    iconDataList.Add(iconData);

                }
                SaveJson();
            }
            else
            {
                CheckIfHaveNewData();
                LoadJson();
            }


        }

        public Dictionary<string, Bitmap> LoadIconsFromFolder()
        {
            return Extensions.LoadIconFromFolder(iconPath);
        }
        public void CheckIfHaveNewData()
        {
            // Загружаем текущие данные из JSON
            string jsonData = File.ReadAllText(jsonPath);
            var oldData = JsonConvert.DeserializeObject<List<IconData>>(jsonData);

            // Загружаем новые данные из папки
            var newImages = LoadIconsFromFolder();

            // Проверяем, есть ли новые изображения, которые еще не добавлены в JSON
            foreach (var fileName in newImages.Keys)
            {
                bool isNewData = true;
                foreach (var oldIcon in oldData)
                {
                    if (oldIcon.iconName == fileName)
                    {
                        isNewData = false;
                        break;
                    }
                }

                if (isNewData)
                {
                    // Если обнаружено новое изображение, создаем новый объект IconData и добавляем его в список
                    Bitmap image = newImages[fileName];
                    IconData iconData = new IconData();

                    iconData.iconName = fileName;
                    iconData.imagePath = Path.Combine(iconPath, fileName + ".png");
                    iconData.isSearcheabled = true;
                    iconData.isDiscovered = false;
                    iconData.isIconWichNumbers = false;
                    iconData.x = 0;
                    iconData.y = 0;
                    iconData.isTopCheck = true;
                    iconData.isBotCheck = true;
                    iconData.argbDifferences = 10;
                    iconData.x = 200;
                    iconData.y = 300;
                    iconData.isSearcheabled = false;

                    // Добавляем новый объект IconData в список
                    oldData.Add(iconData);
                }
            }

            // Сохраняем обновленные данные в JSON
            string updatedJson = JsonConvert.SerializeObject(oldData, Formatting.Indented);
            File.WriteAllText(jsonPath, updatedJson);
        }

        public void SaveJson()
        {
            string jsonData = JsonConvert.SerializeObject(iconDataList, Formatting.Indented);
            Extensions.SaveJson(jsonPath, jsonData);
        }

        public void LoadJson()
        {
            try
            {
                if (File.Exists(jsonPath))
                {
                    string jsonData = File.ReadAllText(jsonPath);
                    iconDataList = JsonConvert.DeserializeObject<List<IconData>>(jsonData);

                    // Load Bitmap for each IconData
                    for (int i = 0; i < iconDataList.Count; i++)
                    {
                        var iconData = iconDataList[i];

                        if (iconData.height<=10)
                        {
                            iconData.height = 100;
                        }
                        if (iconData.width <= 10)
                        {
                            iconData.width = 100;

                        }

                        if (iconData.opacity==0)
                        {
                            iconData.opacity = 100;
                        }
                        if (iconData.argbDifferences==0)
                        {
                            iconData.argbDifferences = 10;
                        }
                        //iconData.height = 100;
                        //iconData.width = 100;

                        if (File.Exists(iconData.imagePath))
                        {
                            iconData.sampleImage = new Bitmap(iconData.imagePath);
                        }
                        else
                        {
                            Console.WriteLine($"Image file not found: {iconData.imagePath}");
                        }

                        // Заменяем элемент в списке обновленным значением
                        iconDataList[i] = iconData;
                    }
                    GlobalData.IconDataList = iconDataList;
                }
                else
                {
                    Console.WriteLine("JSON file does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading JSON: {ex.Message}");
            }


        }

        public void UpdateData(IconData updatedIconData)
        {
            for (int i = 0; i < iconDataList.Count; i++)
            {
                var item = iconDataList[i];
                if (item.iconName == updatedIconData.iconName)
                {
                    iconDataList[i] = updatedIconData;
                    break; // Выход из цикла после обновления данных
                }
            }

            string jsonData = JsonConvert.SerializeObject(iconDataList, Formatting.Indented);
            Extensions.SaveJson(jsonPath, jsonData);
            LoadJson(); // Загрузка данных после сохранения
        }

    }
}
