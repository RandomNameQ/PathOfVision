Path of Vision - это приложение, которое обнаруживает иконки, которые вы выбрали, и отображает их в нужном месте экрана, с определенным размером и прозрачностью. Приложение также поддерживает работу с геймпадами, если к ПК подключен геймпад.

# [English description](#english-description) 💡

<br>

### [Discrod](https://discord.gg/2CMYfUBMsq)
### [Redit](https://www.reddit.com/r/pathofexile/comments/1bmn5ut/tool_qol_detect_icons_application/?xpromo_edp=enabled)

<br>

https://github.com/RandomNameQ/PathOfVision/assets/125605136/4e10c7f0-b680-4446-b83b-45da8f64e877


<br>


# 💡 Как запустить?
1. Выберите иконки во вкладке "Icons". Если стоит галочка, значит иконка выбрана.
![image](https://github.com/RandomNameQ/PathOfVision/assets/125605136/83864abb-4000-469d-b6af-2ad53740e5ad)
2. Нажмите кнопку "Icon Positioning" и перемещайте иконки или изменяйте их размер.
3. Настройки сохраняются через кнопку "Save settings". Когда кнопка становится зеленой, это означает, что настройки сохранены.
4. Во вкладке "Main" настройте область сканирования в "Screen Area Settings".
Область сканирования определяет ширину и высоту области, в которой происходит сканирование.
Чтобы сохранить настройки, нажмите "Save settings". Кнопка "Make test screen..." делает скриншот, а "Open folder" открывает папку с программой.
7. Когда все настроено, нажмите "Start scan".
8. Если нужно изменить настройки отображения для иконок, остановите сканирование, внесите изменения и начните сканирование снова.

# 🛠️Функциональность
1. Приложение каждые 0.1 секунды делает скриншот области экрана.
2. Находит иконки в этой области.
3. Сверяет найденные иконки с иконками из программы. Если программа находит одинаковые иконки, то отображает их на экране.

# 🐞Известные проблемы
-Программу нужно удалять через диспетчер задач, иначе она не завершит работу.
-Иконки иногда мигают.
-Некоторые иконки не находятся.
-Некоторые иконки заменяются другими.

Фатальная проблема
Поешка генерирует разные иконки. В Icon\AllCuttedIcon можно увидеть примеры. Пое рендерит иконки разными способами, что приводит к сдвигам внутри изображений (наверно я хз).

# 💡 FAQ.
1. У меня нет иконок, где взять?
-Включи галочку "cut icons" в main и нажми "start scan", теперь программа будет сохранять все уникальные иконки в папке "Icon\AllCuttedIcon".
2. Как добавить новую иконку?
Получи иконки, переименуй (пробелы не ставь) и положи иконку в Icon\Buffs. Запусти программу, выключи ее и снова запусти (в первый запуск создается место для иконки, но во второй она добавляется в список).
3. Исчезла иконка.
-Сбрось позицию к X200 и Y200 - может иконка ушла за пределы экрана. Сделай opacity по центру - может она на 100% прозрачна.
4. Хочу, чтобы был звук, когда иконка находится или исчезает.
-Включи галку appearance sound и выбери файл - теперь проигрывается звук, когда иконка появляется. Disappearance sound для звука при исчезновении.

5. Иконка не отображается или отображается другая иконка.
Иконка не отображается или отображается другая иконка.

Алгоритм, используемый для обнаружения "одинаковых" картинок:

Берется верхняя\нижняя\левая\правая линия пикселей у иконки из игры, сохраняется сумма всех цветов.
Затем берется выбранная нами иконка, у нее собирается сумма цветов, и теперь мы сравниваем две суммы цветов. Если разница между цифрами не больше 10%, то значит, мы нашли одинаковые картинки.

В таких условиях некоторые иконки не находятся, а иногда находятся не те картинки. Чтобы исправить это, вам необходимо изменить подход к обнаружению картинки.

Например, если вы увеличите параметр ArgbDiffrence во вкладке с 10% до 20%, то иконка может чаще находиться, и\или другие иконки вместо искомой могут быть найдены. Если уменьшите значение с 10% до 5%, то это может решить проблему с тем, что находятся другие иконки.

По умолчанию программа сравнивает верхнюю и нижнюю стороны пикселей. Вы можете изменить это, выбрав либо все 4 стороны (что увеличит шанс того, что будет найдена иконка или другая), либо уменьшить количество, либо выбрать другую зону.

Например, если вы ищете MoltenShell, а программа находит Onslaught, то проблема заключается в том, что у обеих иконок верхняя часть похожа по цветам. В этом случае укажите, чтобы программа искала цвета в той стороне, где иконки не одинаковые.

# English-description

Path of Vision is an application that detects the icons you have selected and displays them in the desired location on the screen, with a specific size and transparency. The application also supports gamepads if a gamepad is connected to the PC.

<br>


# 💡 How to launch?
1. Select icons in the "Icons" tab. If there is a checkmark, then the icon is selected.
![image](https://github.com/RandomNameQ/PathOfVision/assets/125605136/83864abb-4000-469d-b6af-2ad53740e5ad)
2. Click the "Icon Positioning" button and move the icons or change their size.
3. Settings are saved using the "Save settings" button. When the button turns green, it means the settings have been saved.
4. In the "Main" tab, configure the scanning area in "Screen Area Settings".
The scanning area determines the width and height of the area in which scanning occurs.
To save the settings, click "Save settings". The "Make test screen..." button takes a screenshot, and "Open folder" opens the folder with the program.
7. When everything is configured, click "Start scan".
8. If you need to change the display settings for icons, stop scanning, make changes, and start scanning again.

# 🛠️Functionality
1. The application takes a screenshot of an area of the screen every 0.1 seconds.
2. Finds icons in this area.
3. Compares the found icons with the icons from the program. If the program finds identical icons, it displays them on the screen.

# 🐞Known issues
-The program must be uninstalled through the task manager, otherwise it will not terminate.
-Icons sometimes blink.
-Some icons are not located.
-Some icons are replaced by others.

Fatal problem
Poeshka generates different icons. You can see examples in Icon\AllCuttedIcon. Poe renders icons in different ways, which leads to shifts inside the images (I guess I don't know).

# 💡 FAQ.
1. I don’t have icons, where can I get them?
-Check the "cut icons" checkbox in main and click "start scan", now the program will save all unique icons in the "Icon\AllCuttedIcon" folder.
2. How to add a new icon?
Get the icons, rename them (don’t put spaces) and put the icon in Icon\Buffs. Run the program, turn it off and run it again (on the first run, a place for the icon is created, but on the second it is added to the list).
3. The icon has disappeared.
-Reset the position to X200 and Y200 - maybe the icon has gone off the screen. Make the opacity in the center - maybe it is 100% transparent.
5. The icon is not displayed or another icon is displayed.
The icon is not displayed or a different icon is displayed.

Algorithm used to detect "identical" pictures:

The top\bottom\left\right line of pixels from the icon from the game is taken and the sum of all colors is saved.
Then the icon we selected is taken, its color sum is collected, and now we compare the two color sums. If the difference between the numbers is no more than 10%, then we have found the same pictures.

In such conditions, some icons are not found, and sometimes the wrong pictures are found. To fix this, you need to change your approach to image detection.

For example, if you increase the ArgbDiffrence parameter in the tab from 10% to 20%, then the icon may be found more often, and/or other icons may be found instead of the one you are looking for. If you reduce the value from 10% to 5%, this may solve the problem with other icons being located.

By default, the program compares the top and bottom sides of pixels. You can change this by either selecting all 4 sides (which will increase the chance of an icon or another being found), or reducing the number, or choosing a different zone.

For example, if you search for MoltenShell and the program finds Onslaught, the problem is that both icons have similar top colors. In this case, tell the program to look for colors on the side where the icons are not the same.
