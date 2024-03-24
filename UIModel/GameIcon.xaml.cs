using PathOfVision.CreateUI;
using PathOfVision.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static PathOfVision.SampleIconData;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;
using Point = System.Windows.Point;

namespace PathOfVision.UIModel
{
    /// <summary>
    /// Interaction logic for GameIcon.xaml
    /// </summary>
    public partial class GameIcon : Window
    {




        private bool isDragging = false;
        public Border borderIconGame;
        public Window windowIconGame;
        //public Image image => UIHelper.FindChild<Image>(Application.Current.MainWindow, "border");
        public bool isClickThrouIcon;
        public GameIcon(bool isClickThrouIcon)
        {
            this.isClickThrouIcon = isClickThrouIcon;
            InitializeComponent();
            FindData();

            this.Loaded += MainWindow_Loaded;
        }
      

        protected override void OnSourceInitialized(EventArgs e)
        {
            if (isClickThrouIcon)
            {
                base.OnSourceInitialized(e);
                var hwnd = new WindowInteropHelper(this).Handle;
                WindowsServices.SetWindowExTransparent(hwnd);
            }
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            windowIconGame.Visibility = Visibility.Collapsed;
        }

        public void ChangeIconImage(Dictionary<string, string> pathAndName)
        {
            // Проверяем, что в словаре есть данные
            if (pathAndName == null || pathAndName.Count == 0)
            {
                Console.WriteLine("The pathAndName dictionary is empty or null.");
                return;
            }

            // Получаем путь к изображению из словаря
            string iconPath = pathAndName.FirstOrDefault().Key; // Получаем первое значение из словаря

            // Проверяем, что путь к изображению не пустой
            if (string.IsNullOrEmpty(iconPath))
            {
                Console.WriteLine("The iconPath is null or empty.");
                return;
            }

            // Создаем объект BitmapImage из пути к изображению
            BitmapImage bitmapImage = new BitmapImage(new Uri(iconPath, UriKind.RelativeOrAbsolute));

            // Устанавливаем изображение как фон элемента управления Border
            borderIconGame.Background = new ImageBrush(bitmapImage);
        }

        private void BorderGameIcon_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Ваша логика обработки клика на элементе
        }


        public void FindData()
        {
            borderIconGame = (Border)WindowIconGame.FindName("BorderGameIcon");
            windowIconGame = (Window)WindowIconGame.FindName("WindowIconGame");
        }

      

       

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();

                var movedObject = e.OriginalSource as FrameworkElement;
                if (movedObject != null)
                {
                    SaveNewPosition(movedObject);
                }
            }
        }

        public void SaveNewPosition(FrameworkElement movedObject)
        {
            string iconName = movedObject.Tag.ToString();
            var iconData = Extensions.GetIconData(iconName);

            var positions = Extensions.GetAbsolutePlacement(movedObject);



            iconData.x = (int)positions.X;
            iconData.y = (int)positions.Y;

            iconData.width = (int)movedObject.ActualWidth;
            iconData.height = (int)movedObject.ActualHeight;

            for (int i = 0; i < GlobalData.IconDataList.Count; i++)
            {
                if (GlobalData.IconDataList[i].iconName == iconName)
                {
                    // Обновляем позицию иконки
                    GlobalData.IconDataList[i] = iconData;

                    // Сохраняем обновленные данные
                    GlobalData.SaveDataIconList();
                    break;
                }
            }
        }

        public void ApplySettingsToIcon(GlobalData.GameIconState gameIcon, bool isIconPositioning = false)
        {

            string currentDirectory = Directory.GetCurrentDirectory();
            var imagePath = $"{currentDirectory}/{gameIcon.imagePath}";

            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));

            borderIconGame.Background = new ImageBrush(bitmapImage);
            borderIconGame.Tag = gameIcon.iconName;
            SetPosition(gameIcon.createdIconPositionX, gameIcon.createdIconPositionY);

          
            var parentWindow = Window.GetWindow(borderIconGame);

            parentWindow.Height = gameIcon.height;
            parentWindow.Width = gameIcon.width;

            parentWindow.Opacity = gameIcon.opacity / 100.0;
            //parentWindow.Opacity = 20 / 100.0;


            if (isIconPositioning)
            {
                parentWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
                parentWindow.IsHitTestVisible = true;

                //parentWindow.Opacity = 1;
            }

        }

        public void SetPosition(double x, double y)
        {
            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                // Получаем координаты в контексте главного окна
                Point relativePos = new Point(x, y);

                // Устанавливаем позицию окна относительно главного окна
                Left = relativePos.X;
                Top = relativePos.Y;
            }
        }





    }




}
