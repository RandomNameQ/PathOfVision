using Newtonsoft.Json;
using PathOfVision.CreateUI;
using PathOfVision.Data;
using PathOfVision.UIModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using static MaterialDesignThemes.Wpf.Theme;
using static PathOfVision.MainWindow;
using static PathOfVision.SampleIconData;
using Image = System.Windows.Controls.Image;

namespace PathOfVision
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int test;
        public string jsonPath = "Icon\\Settings.json";
        public ScreenAreaSettings screenAreaSettings = new();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Ваш код здесь будет выполнен после загрузки UI
            // Например, добавление элементов или выполнение других операций

            CreateUI.CreateUI createUI = new();
            createUI.CreateIconList();
            LoadSettings();
            CountDetectedIcons.Content = "How much icon detected";
        }
        
       

        private void OpenDiscordPage_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://discord.gg/2CMYfUBMsq",
                UseShellExecute = true
            });
        }
        private void OpenGitHubPage_Click(object sender, RoutedEventArgs e)
        {
           
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/RandomNameQ/PathOfVision",
                UseShellExecute = true
            });
           
        }
        public void LoadSettings()
        {
            try
            {
                if (File.Exists(jsonPath))
                {
                    string jsonData = File.ReadAllText(jsonPath);
                    var settings = JsonConvert.DeserializeObject<ScreenAreaSettings>(jsonData);

                    screenWidth.Text = settings.screenWidth.ToString();
                    screenHeight.Text = settings.screenHeight.ToString();
                    distanceFromLeft.Text = settings.distanceFromLeft.ToString();
                    disatnceFromTop.Text = settings.disatnceFromTop.ToString();

                    screenAreaSettings = settings;
                }
                else
                {
                    screenWidth.Text = "800";
                    screenHeight.Text = "200";
                    distanceFromLeft.Text = "0";
                    disatnceFromTop.Text = "0";
                    
                    SaveScreenArea();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading JSON: {ex.Message}");
            }
            ((App)Application.Current).detectGameIcon.screenArea.UpdateSettings(screenAreaSettings);

        }

        private void GetIcon_Click(object sender, RoutedEventArgs e)
        {
            GetIconExample getIconExample = new GetIconExample();
            getIconExample.GetIcon();
        }



       

        private void StartScan_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).StartTimer();
            ((App)Application.Current).StartScan();
        }

        private void StopScan_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).StopTimer();
            ((App)Application.Current).StopScan();

        }
        private void IsCutIcon_Checked(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).isCutIcon = true;
        }

        private void IsCutIcon_Unchecked(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).isCutIcon = false;
        }


        public string DissapearanceSound_Path;
        public string AppearanceSoundd_Path;

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            // Use Process.Start to open the File Explorer with the current directory
            Process.Start("explorer.exe", currentDirectory);
        }


        private void SaveScreenSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveScreenArea();
        }

        public void SaveScreenArea()
        {
            if (int.TryParse(screenWidth.Text, out int width))
                screenAreaSettings.screenWidth = width;

            if (int.TryParse(screenHeight.Text, out int height))
                screenAreaSettings.screenHeight = height;

            if (int.TryParse(distanceFromLeft.Text, out int distanceLeft))
                screenAreaSettings.distanceFromLeft = distanceLeft;

            if (int.TryParse(disatnceFromTop.Text, out int distanceRight))
                screenAreaSettings.disatnceFromTop = distanceRight;


            string jsonData = JsonConvert.SerializeObject(screenAreaSettings, Formatting.Indented);
            Extensions.SaveJson(jsonPath, jsonData);

            LoadSettings();
        }
        public class ScreenAreaSettings
        {
            public int screenWidth;
            public int screenHeight;
            public int distanceFromLeft;
            public int disatnceFromTop;
        }

        private void TestScreen_Click(object sender, RoutedEventArgs e)
        {
            LoadSettings();
            GetScreenArea screenArea = new();
            screenArea.UpdateSettings(screenAreaSettings);
            var image = screenArea.GetScreen();
            Extensions.SaveImage(image,"Test_Screenshot");
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }


        private void LoadJson_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveJson_Click(object sender, RoutedEventArgs e)
        {

        }


        private async void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            // Ваш код для обработки закрытия приложения
            //((App)Application.Current).StopTimer();

            //// Дождитесь 0.3 секунды
            //await Task.Delay(300);

            //// Вызовите метод StopScan() после задержки
            //((App)Application.Current).StopScan();

            //// Установите e.Cancel в false
            //e.Cancel = false;
        }


    }
}
