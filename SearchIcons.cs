using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using PathOfVision.Data;

namespace PathOfVision
{
    public class SearchIcons
    {

       
        public GetScreenArea screenArea = new();
        public CutIcons cutIcons = new();
        public FindBorder findBorder = new();


        public Bitmap gameAreaScreen;
        public Dictionary<Bitmap, Vector2> currentVisibleIconInGame = new();

        public enum ImageComparerVariant
        {
            Hash,
            Pixels,
        }
        public ImageComparerVariant imageComparerVariant = ImageComparerVariant.Hash;

        public void GetGameIcons()
        {
            GetGameScreen();
            ConvertScreenToMultiIcons(findBorder.GetCenterPositionIcon(gameAreaScreen));
            CompareIcons();
        }
        public void GetGameScreen()
        {
            //gameAreaScreen = screenArea.GetScreen(GetScreenArea.ScreenAreaVariant.BuffAreaTop);
            //GlobalData.screenArea = gameAreaScreen;
            //Extensions.SaveImage(gameAreaScreen, "ScreenArea");
        }
        public void ConvertScreenToMultiIcons(Dictionary<int, Vector2> iconPosition)
        {
            currentVisibleIconInGame = cutIcons.GetIcons(gameAreaScreen, iconPosition, true);
        }
        public void CompareIcons()
        {
            CompareHash();
            switch (imageComparerVariant)
            {
                case ImageComparerVariant.Hash:

                    break;
                case ImageComparerVariant.Pixels:
                    break;
            }
        }

        public void CompareHash()
        {
            Debug.WriteLine($"Проверяю");

            foreach (var icon in GlobalData.searchableIcons)
            {
                icon.isDetected = false;
            }

            foreach (var entry in currentVisibleIconInGame)
            {
                Bitmap currentGameIcon = entry.Key;
                Vector2 currentPosition = entry.Value;

                CheckImage_Hash(currentGameIcon,currentPosition);
            }

            UpdateDataInThreads();

        }
        public void CheckImage_Hash(Bitmap currentGameIcon, Vector2 currentPosition)
        {
            for (int j = 0; j < GlobalData.searchableIcons.Count; j++)
            {
                if (GlobalData.searchableIcons[j].isHalfComapre)
                {
                    if (ImageComparer_Hash.AreImagesSimilar_HalfCompare(currentGameIcon, GlobalData.searchableIcons[j].sampleIcon))
                    {
                        GlobalData.searchableIcons[j].isDetected = true;

                        GlobalData.searchableIcons[j].gameIconPositionX = (int)currentPosition.X;
                        GlobalData.searchableIcons[j].gameIconPositionY = (int)currentPosition.Y;

                        Debug.WriteLine($"Нашел {currentPosition} {GlobalData.searchableIcons[j].iconName}");

                        return;
                    }
                }
                else
                {
                    if (ImageComparer_Hash.AreImagesSimilar(currentGameIcon, GlobalData.searchableIcons[j].sampleIcon))
                    {
                        GlobalData.searchableIcons[j].isDetected = true;

                        GlobalData.searchableIcons[j].gameIconPositionX = (int)currentPosition.X;
                        GlobalData.searchableIcons[j].gameIconPositionY = (int)currentPosition.Y;

                        Debug.WriteLine($"Нашел {currentPosition} {GlobalData.searchableIcons[j].iconName}");

                        return;
                    }
                }
                
            }
        }

        public void UpdateDataInThreads()
        {
            // Получаем доступ к экземпляру createThread_DisplayIcon из класса App
            App app = (App)Application.Current;
            var createThread_DisplayIcon = app.createThread_DisplayIcon;

            if (createThread_DisplayIcon == null) return;


            createThread_DisplayIcon.ChangeDataInThread(GlobalData.searchableIcons);
        }
    }

    
}

