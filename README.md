# C-warcaby-STM32F429I-DISCO
Gra warcaby dla dwóch graczy napisana w języku C++ na ekran dotykowy płytki STM32F429I-DISCO.

Aby uruchomić projekt należy wgrać plik initflash.hex na STMa za pomocą np: STM32 ST-LINK Utility. 
W grę należy grać we dwie osoby, wykonując ruchy na zmiane, poprzez klikanie na ekranie swojego pionka, a następnie wybieranie jego nowej pozycji z wyświetlonych możliwych. Aby zrezygnować z ruchu danym pionkiem i wykonać go innym, należy wcisnąć inny przycisk niż zielony (czyli wybrać inny pionek gracza).

Aby skompilować projekt lub wprowadzać w nim własne zmiany należy mieć zainstalowanego TouchGFXa - http://touchgfx.com/try-touchgfx/evaluation/. Następnie należy stworzyć nowy projekt przy użyciu TouchGFX Designera. Po utworzeniu go wystarczy podmienić plik assets w folderze projektu, a następnie uruchomić przez Visual Studio jego symulacje, którą można znaleźć pod ścieżką: simulator/msvs/application.sln. Tutaj wystarczy zlokalizować pliki odpowiadające nazwami plikom udostępnionym w projekcie i podmienić ich kod. To wszystko. Teraz możemy skompilować nasz program i pokaże nam się jego symulacja.

Dodany krótki film pokazujący działanie gry na symulatorze. Partia tam ukazana nie jest nagrana od początku, by w jak najkrótszym czasie zademonstrować działanie królowek.
