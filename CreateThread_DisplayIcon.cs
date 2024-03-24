using PathOfVision.Data;
using PathOfVision.UIModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Timers;
using System.Windows.Documents;

namespace PathOfVision
{
    public class CreateThread_DisplayIcon
    {
        public List<IconThread> threads = new List<IconThread>();

        public void CreateThreads(bool iconPositioning = false)
        {
            for (int i = 0; i < GlobalData.IconDataList.Count; i++)
            {
                if (!GlobalData.IconDataList[i].isSearcheabled)
                {
                    continue;
                }
                IconThread iconThread = new IconThread(iconPositioning);
                iconThread.IconSettings(GlobalData.IconDataList[i]);
                threads.Add(iconThread);
            }
        }

        public void StopThreads()
        {
            foreach (var thread in threads)
            {
                thread.StopThread();
            }
            threads.Clear();
        }

        public void ChangeDataInThread(List<GlobalData.GameIconState> iconsData)
        {
            foreach (var iconThread in threads)
            {

                for (global::System.Int32 i = 0; i < iconsData.Count; i++)
                {
                    if (iconThread.displayedIcon != null && iconThread.displayedIcon.iconName == iconsData[i].iconName)
                    {
                        iconThread.displayedIcon = iconsData[i];
                    }
                }

            }
        }

        public class IconThread
        {
            public GlobalData.GameIconState displayedIcon;
            public SampleIconData.IconData iconData;
            public CutIcons cutIcons = new();

            public GameIcon gameIcon;

            private System.Timers.Timer timer;

            public bool alwaysShowIcon;
            private bool shouldStop = false;

            public IconThread(bool iconPositioning = false)
            {
                alwaysShowIcon = iconPositioning;
            }

            public void StopThread()
            {
                gameIcon.Dispatcher.Invoke(() =>
                {
                    gameIcon.windowIconGame.Visibility = System.Windows.Visibility.Hidden;
                });
                // Установка флага для завершения работы потока
                shouldStop = true;
            }

            public void IconSettings(SampleIconData.IconData iconData)
            {
                this.iconData = iconData;

                displayedIcon = new GlobalData.GameIconState()
                {
                    isIconWichNumbers = iconData.isIconWichNumbers,
                    iconName = iconData.iconName,
                    createdIconPositionX = iconData.x,
                    createdIconPositionY = iconData.y,
                    sampleIcon = iconData.sampleImage,
                    imagePath = iconData.imagePath,

                    height = iconData.height,
                    width = iconData.width,

                    opacity = iconData.opacity,

                    argbDifferences = iconData.argbDifferences,

                    isTopCheck = iconData.isTopCheck,
                    isBotCheck = iconData.isBotCheck,
                    isLeftCheck = iconData.isLeftCheck,
                    isRightCheck = iconData.isRightCheck,

                    isShowAllResults = iconData.isShowAllResults,

                };

                gameIcon = new GameIcon(!alwaysShowIcon);

                gameIcon.ApplySettingsToIcon(displayedIcon, alwaysShowIcon);

                gameIcon.Show();
                if (alwaysShowIcon)
                {
                    ShowIcons();
                }
                else
                {
                    StartTimer();

                }

            }

            public void StartTimer()
            {
                timer = new System.Timers.Timer(100);
                timer.Elapsed += OnTimedEvent;
                timer.AutoReset = true; // Устанавливаем таймер на автоматическое повторение
                timer.Enabled = true;
            }
            private void OnTimedEvent(object source, ElapsedEventArgs e)
            {
                if (shouldStop)
                {
                    // Остановка выполнения цикла таймера
                    timer.Stop();
                    return;
                }

                IsIconDetected();


            }

            public SoundPlayer sp = new SoundPlayer();

            public bool isIconDetected;
            public int countDetected;
            public bool isFirstAuidioPlay;

            public void IsIconDetected()
            {
               
                bool needHide = true;

                if (displayedIcon.isDetected)
                {
                    gameIcon.Dispatcher.Invoke(() =>
                    {
                        if (displayedIcon.isIconWichNumbers)
                        {
                            CutIcon();
                        }
                        gameIcon.windowIconGame.Visibility = System.Windows.Visibility.Visible;
                        needHide = false;

                        if (iconData.isSoundIfDetected && !isFirstAuidioPlay)
                        {
                            sp.SoundLocation = iconData.soundDetectedPath;
                            sp.Load();
                            sp.Play();
                            isIconDetected = true;
                            isFirstAuidioPlay = true;


                        }
                    });
                }
                else
                {
                    gameIcon.Dispatcher.Invoke(() =>
                    {
                        if (!isIconDetected)
                        {
                            countDetected = 0;
                        }
                        if (needHide)
                        {
                            gameIcon.windowIconGame.Visibility = System.Windows.Visibility.Collapsed;
                            if (iconData.isSoundIfOver && isIconDetected)
                            {
                                sp.SoundLocation = iconData.soundOverPath;
                                sp.Load();
                                sp.Play();
                                isIconDetected = false;
                                isFirstAuidioPlay = false;
                            }
                        }

                    });
                }
            }

            public void CutIcon()
            {
                var pathAndIcon = cutIcons.CutIcon(GlobalData.screenArea, displayedIcon.iconName, displayedIcon.gameIconPositionX, displayedIcon.gameIconPositionY, displayedIcon.isIconWichNumbers);
                gameIcon.ChangeIconImage(pathAndIcon);
            }


            public void ShowIcons()
            {

                gameIcon.Dispatcher.Invoke(() =>
                {
                    gameIcon.windowIconGame.Visibility = System.Windows.Visibility.Visible;
                });

            }

        }

    }


}

