Path of Vision - это приложение, которое обнаруживает иконки, которые вы выбрали, и отображает их в нужном месте экрана, с определенным размером и прозрачностью. Приложение также поддерживает работу с геймпадами, если к ПК подключен геймпад.

### [Markdown - Link](#💡-English-description)

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

# 💡 English description

