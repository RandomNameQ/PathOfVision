using MaterialDesignThemes.Wpf;
using PathOfVision.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static MaterialDesignThemes.Wpf.Theme;
using static PathOfVision.SampleIconData;
using Button = System.Windows.Controls.Button;
using CheckBox = System.Windows.Controls.CheckBox;
using ListBoxItem = System.Windows.Controls.ListBoxItem;
using TextBox = System.Windows.Controls.TextBox;

namespace PathOfVision.CreateUI
{
    public class CreateIconList
    {
        public ListBox listBox;
        public ListBoxItem listBoxItem_Original;
        public Image image_Original;
        public CheckBox checkBox_Original;

        public Button SaveSettingsButton;
        public Button IconPositioning;

        public Label IconName;
        public TextBox ArgbDiffrence;


        public CheckBox IconWithName;
        public CheckBox IconWithNumber;
        public Slider IconOpacity;
        public TextBox IconWidthSize;
        public TextBox IconHeightSize;
        public TextBox ScreenPositionX;
        public TextBox ScreenPositionY;
        public CheckBox AppearanceSound;
        public CheckBox DissapearanceSound;

        public CheckBox IsTopCheck;
        public CheckBox IsBotCheck;
        public CheckBox IsLeftCheck;
        public CheckBox IsRightCheck;
        public CheckBox IsShowAllResult;

        public string DissapearanceSound_Path;
        public string AppearanceSoundd_Path;
        public Button DissapearanceSound_Click;
        public Button AppearanceSoundd_Click;



        public CreateIconList()
        {

            listBox = UIHelper.FindChild<ListBox>(Application.Current.MainWindow, "Tab_Buff_ListBox");
            listBoxItem_Original = UIHelper.FindChild<ListBoxItem>(Application.Current.MainWindow, "Tab_Buff_ListBox_Element");
            image_Original = UIHelper.FindChild<Image>(Application.Current.MainWindow, "Tab_Buff_Image");
            checkBox_Original = UIHelper.FindChild<CheckBox>(Application.Current.MainWindow, "Tab_Buff_CheckBox");

            SaveSettingsButton = UIHelper.FindChild<Button>(Application.Current.MainWindow, "SaveSettingsButton");
            IconPositioning = UIHelper.FindChild<Button>(Application.Current.MainWindow, "IconPositioning");

            IconName = UIHelper.FindChild<Label>(Application.Current.MainWindow, "IconName");
            ArgbDiffrence = UIHelper.FindChild<TextBox>(Application.Current.MainWindow, "argbDiffrence");


            IconWithName = UIHelper.FindChild<CheckBox>(Application.Current.MainWindow, "IconWithName");
            IconWithNumber = UIHelper.FindChild<CheckBox>(Application.Current.MainWindow, "IconWithNumber");

            IconOpacity = UIHelper.FindChild<Slider>(Application.Current.MainWindow, "IconOpacity");

            IconWidthSize = UIHelper.FindChild<TextBox>(Application.Current.MainWindow, "IconWidthSize");
            IconHeightSize = UIHelper.FindChild<TextBox>(Application.Current.MainWindow, "IconHeightSize");

            ScreenPositionX = UIHelper.FindChild<TextBox>(Application.Current.MainWindow, "ScreenPositionX");
            ScreenPositionY = UIHelper.FindChild<TextBox>(Application.Current.MainWindow, "ScreenPositionY");

            AppearanceSound = UIHelper.FindChild<CheckBox>(Application.Current.MainWindow, "AppearanceSound");
            DissapearanceSound = UIHelper.FindChild<CheckBox>(Application.Current.MainWindow, "DissapearanceSound");

            IsTopCheck = UIHelper.FindChild<CheckBox>(Application.Current.MainWindow, "IsTopCheck");
            IsBotCheck = UIHelper.FindChild<CheckBox>(Application.Current.MainWindow, "IsBotCheck");
            IsLeftCheck = UIHelper.FindChild<CheckBox>(Application.Current.MainWindow, "IsLeftCheck");
            IsRightCheck = UIHelper.FindChild<CheckBox>(Application.Current.MainWindow, "IsRightCheck");
            IsShowAllResult = UIHelper.FindChild<CheckBox>(Application.Current.MainWindow, "IsShowAllResult");

            AppearanceSoundd_Click = UIHelper.FindChild<Button>(Application.Current.MainWindow, "AppearanceSoundClick");
            DissapearanceSound_Click = UIHelper.FindChild<Button>(Application.Current.MainWindow, "DissapearanceSoundClick");

        }




        public void CreateListBox_Buff()
        {
            SaveSettingsButton.Click += SaveSettingsButton_Click;
            IconPositioning.Click += SetPositionForIcons;

            AppearanceSoundd_Click.Click += AppearanceSound_ClickEvent;
            DissapearanceSound_Click.Click += DissapearanceSound_ClickEvent;

            foreach (SampleIconData.IconData iconData in GlobalData.IconDataList)
            {
                Create_ListBoxItem(iconData);
            }
            // скрываем элемент который служил настройками, чтобы не занимал место
            listBoxItem_Original.Visibility = Visibility.Collapsed;
        }

        public void Create_ListBoxItem(SampleIconData.IconData iconData)
        {

            ListBoxItem listBoxItem = new ListBoxItem();
            Image image = new();
            CheckBox checkBox = new CheckBox();

            UIHelper.CopySettings(checkBox_Original, checkBox);
            UIHelper.CopySettings(image_Original, image);
            UIHelper.CopySettings(listBoxItem_Original, listBoxItem);

            string currentDirectory = Directory.GetCurrentDirectory();


            var imagePath = $"{currentDirectory}/{iconData.imagePath}";
            image.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));

            checkBox.Content = iconData.iconName;
            checkBox.IsChecked = iconData.isSearcheabled;

            image.Tag = iconData.iconName;

            if (iconData.isSearcheabled)
            {
                SolidColorBrush greenBrush = new SolidColorBrush(Colors.Green);
                checkBox.Background = greenBrush;
            }

            // Добавление обработчика события для клика по CheckBox
            checkBox.Checked += CheckBox_Checked;
            checkBox.Unchecked += CheckBox_Unchecked;

            listBoxItem.Content = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children = { image, checkBox }
            };
            listBox.Items.Add(listBoxItem);
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox != null)
            {
                SolidColorBrush greenBrush = new SolidColorBrush(Colors.Green);
                checkBox.Background = greenBrush;
                SaveSettingsButton.Background = new SolidColorBrush(Colors.White);

                RestoreIconSettings((string)checkBox.Content);
                ChangeSearchable(true);

            }
        }

        // Обработчик события для события Unchecked
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                SolidColorBrush greenBrush = new SolidColorBrush(Colors.White);
                checkBox.Background = greenBrush;

                SaveSettingsButton.Background = new SolidColorBrush(Colors.White);
                RestoreIconSettings((string)checkBox.Content);
                ChangeSearchable(false);


            }
        }


        public void RestoreIconSettings(string iconName)
        {
            var iconData = Extensions.GetIconData(iconName);
            IconName.Content = $"Name: {iconData.iconName}";
            ArgbDiffrence.Text = iconData.argbDifferences.ToString();

            IconWithName.IsChecked = iconData.isIconWichNumbers;
            IconWithNumber.IsChecked = iconData.isNeedHalfCompare;

            IconOpacity.Value = iconData.opacity;

            IconWidthSize.Text = iconData.width.ToString();
            IconHeightSize.Text = iconData.height.ToString();

            ScreenPositionX.Text = iconData.x.ToString();
            ScreenPositionY.Text = iconData.y.ToString();

            AppearanceSound.IsChecked = iconData.isSoundIfDetected;
            DissapearanceSound.IsChecked = iconData.isSoundIfOver;

            IsTopCheck.IsChecked = iconData.isTopCheck;
            IsBotCheck.IsChecked = iconData.isBotCheck;
            IsLeftCheck.IsChecked = iconData.isLeftCheck;
            IsRightCheck.IsChecked = iconData.isRightCheck;

            IsShowAllResult.IsChecked = iconData.isShowAllResults;

        }

        private void DissapearanceSound_ClickEvent(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Установка фильтра для расширения файла и расширение файла по умолчанию
            dlg.DefaultExt = ".wav";
            dlg.Filter = "Wave Files (*.wav)|*.wav";

            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            // Отображение диалогового окна выбора файла вызовом метода ShowDialog
            Nullable<bool> result = dlg.ShowDialog();

            // Получение выбранного имени файла и отображение его в TextBox
            if (result == true)
            {
                // Открываем документ
                string filename = dlg.FileName;
                // textBox1.Text = filename;
                DissapearanceSound_Path = filename;

            }
        }


        private void AppearanceSound_ClickEvent(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Установка фильтра для расширения файла и расширение файла по умолчанию
            dlg.DefaultExt = ".wav";
            dlg.Filter = "Wave Files (*.wav)|*.wav";

            // Установка начальной директории на текущую директорию
            dlg.InitialDirectory = Directory.GetCurrentDirectory();

            // Отображение диалогового окна выбора файла вызовом метода ShowDialog
            Nullable<bool> result = dlg.ShowDialog();

            // Получение выбранного имени файла и отображение его в TextBox
            if (result == true)
            {
                // Открываем документ
                string filename = dlg.FileName;
                AppearanceSoundd_Path = filename;
            }
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush greenBrush = new SolidColorBrush(Colors.Green);
            SaveSettingsButton.Background = greenBrush;
            SaveSettingsIcon();
        }





        public void SaveSettingsIcon()
        {
            if (IconName.Content == null || IconName.Content.ToString() == "" || IconName.Content.ToString().Length < 6)
            {
                return;
            }
            var iconName = IconName.Content.ToString().Substring(6); // Удаляем подстроку "Name: "
            var iconData = Extensions.GetIconData(iconName);


            iconData.isIconWichNumbers = IconWithName.IsChecked ?? false;
            iconData.isNeedHalfCompare = IconWithNumber.IsChecked ?? false;

            iconData.opacity = (int)IconOpacity.Value;


            if (int.TryParse(ArgbDiffrence.Text, out int argb))
            {
                iconData.argbDifferences = argb;
            }

            if (int.TryParse(IconWidthSize.Text, out int iconWidth))
            {
                iconData.width = iconWidth;
            }
            if (int.TryParse(IconHeightSize.Text, out int iconHeight))
            {
                iconData.height = iconHeight;
            }



            if (int.TryParse(ScreenPositionX.Text, out int x))
            {
                iconData.x = x;
            }
            if (int.TryParse(ScreenPositionY.Text, out int y))
            {
                iconData.y = y;
            }

            iconData.isSoundIfDetected = AppearanceSound.IsChecked ?? false;
            iconData.isSoundIfOver = DissapearanceSound.IsChecked ?? false;

            iconData.isTopCheck = IsTopCheck.IsChecked ?? false;
            iconData.isBotCheck = IsBotCheck.IsChecked ?? false;
            iconData.isLeftCheck = IsLeftCheck.IsChecked ?? false;
            iconData.isRightCheck = IsRightCheck.IsChecked ?? false;

            iconData.isShowAllResults = IsShowAllResult.IsChecked ?? false;


            iconData.soundDetectedPath = AppearanceSoundd_Path;
            iconData.soundOverPath = DissapearanceSound_Path;




            for (int i = 0; i < GlobalData.IconDataList.Count; i++)
            {
                if (GlobalData.IconDataList[i].iconName == iconName)
                {
                    GlobalData.IconDataList[i] = iconData;
                    GlobalData.SaveDataIconList();
                    break;
                }
            }

        }

        public void ChangeSearchable(bool isSearchabled)
        {
            if (IconName.Content == null || IconName.Content.ToString() == "" || IconName.Content.ToString().Length < 6)
            {
                return;
            }
            var iconName = IconName.Content.ToString().Substring(6); // Удаляем подстроку "Name: "
            var iconData = Extensions.GetIconData(iconName);

            iconData.isSearcheabled = isSearchabled;

            for (int i = 0; i < GlobalData.IconDataList.Count; i++)
            {
                if (GlobalData.IconDataList[i].iconName == iconName)
                {
                    GlobalData.IconDataList[i] = iconData;
                    GlobalData.SaveDataIconList();
                    break;
                }
            }
        }

        public bool isThreadsForShowIconRunning;
        public CreateThread_DisplayIcon createThread_DisplayIcon = new();

        public void SetPositionForIcons(object sender, RoutedEventArgs e)
        {
            if (isThreadsForShowIconRunning)
            {
                createThread_DisplayIcon.StopThreads();
            }
            else
            {
                createThread_DisplayIcon.CreateThreads(true);
            }

            isThreadsForShowIconRunning = !isThreadsForShowIconRunning;
        }

    }








}

