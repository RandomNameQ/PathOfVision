using PathOfVision.Data;
using PathOfVision.UIModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Windows.Input;
using WindowsInput;

namespace PathOfVision
{
    public partial class App : Application
    {

       
        private DispatcherTimer timer;
        SearchIcons searchIcons = new SearchIcons();

        public DetectGameIcon detectGameIcon = new DetectGameIcon();
        public bool isScanned;


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SampleIconData sampleIconData = new SampleIconData();
            sampleIconData.CreateJson();
        }

        public CreateThread_DisplayIcon createThread_DisplayIcon = new();

        public void StartScan()
        {

            if (isScanned)
            {
                return;
            }
            isScanned = true;
            //RepeatLeftClick();

            string currentDirectory = Directory.GetCurrentDirectory();
            string audioFilePath = $"{currentDirectory}/Sound/StartBleeding.wav";



            SampleIconData sampleIconData1 = new SampleIconData();
            sampleIconData1.LoadJson();
            createThread_DisplayIcon.CreateThreads();
            string directoryPath = Directory.GetCurrentDirectory();
            Extensions.DeleteFilesInFolder($"{directoryPath}/Icon/TempIcons");
        }

        public void StopScan()
        {
            createThread_DisplayIcon.StopThreads();
        }


        public void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.3);
            timer.Tick += Timer_Tick;

            timer.Start();
        }

        public void StopTimer()
        {
            isScanned = false;
            if (timer != null)
            {
                timer.Stop();
            }
            StopScan();
        }

        public bool isCutIcon;
        private void Timer_Tick(object sender, EventArgs e)
        {
            detectGameIcon.AllThings(isCutIcon);
            detectGameIcon.icons.Clear();

            // searchIcons.GetGameIcons();
        }

        public void RepeatLeftClick()
        {
            // Создаем новый объект DispatcherTimer
            DispatcherTimer timer = new DispatcherTimer();

            // Устанавливаем интервал повторения в 1 секунду
            timer.Interval = TimeSpan.FromSeconds(1);

            // Обработчик события Tick, который будет вызываться каждую секунду
            timer.Tick += (sender, e) =>
            {
                // Выполняем клик левой кнопкой мыши
                // Замените этот код на ваш собственный код для выполнения клика левой кнопкой мыши
                // В примере используется синтаксис Windows Input Simulator для генерации события нажатия левой кнопки мыши
                InputSimulator inputSimulator = new InputSimulator();
                inputSimulator.Mouse.LeftButtonClick();
            };

            // Запускаем таймер
            timer.Start();
        }



    }
}
