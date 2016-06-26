# ultricalendar

## Architektura

Zasadniczo system składałby się z aplikacji Web, relacyjnej bazy danych oraz usługi Windows Service, która realizowała by wysyłkę maili z powiadomieniami.

### Aplikacja webowa

To, jak aplikacja mogłaby być podzielona na warstwy jest widoczne w kodzie, tzn. mamy, idąc od dołu:

- domenę (Ultricalendar.Domain)
- warstwę dostępu do danych (Ultricalendar.DataAccess)
- warstwę aplikacji (Ultricalendar.Application.*)
- warstwę prezentacyjną. To jest element, którego nie ma w kodzie, ale tutaj byłby oczywiście jakiś projekt ASP.NET MVC/WebAPI w ramach którego dodatkowo byłby wstrzykiwane implementacje niższych warstw przez kontener IoC.

### Baza danych

Baza danych relacyjna. Serie i eventy byłyby zapisywane w relacji 1 do wielu z NULLowalnym kluczem obcym, aby obsłużyć również zdarzenia nie będące serią. Szczegóły ustawień rekurencji mogłybybyć zapisane dla uproszczenia w jednej dedykowanej kolumnie XML. Przy czym data z początkiem i opcjonanym końcem serii powinna być zapisywana w odrębnych kolumnach, co pozwoliłoby na szybkie wyszukiwanie serii, które znajdują się w danym zakresie (w szczególności zakresie dat w widoku, ktory ustawił użytkownik).

### Windows Service

Windows serice miałby za zadanie wysyłać maile o przypomnieniach w odpowiednim czasie. Najprościej byłoby użyć do tego dedykowanej biblioteki jak Quartz.NET. Serwis powinien być notyfikowany o zmianach w kalendarzu, za pomocą asynchronicznych komunikatów, np. przez MSMQ.

## Challenges

### Zapis zdarzeń i serii

Przyjąłem tutaj podejście, że seria zostaje zapisana w momencie utworzenia w bazie, natomiast żadne z jej zdarzeń nie jest na początku fizycznie zapisywane. Przy odświeżeniu interfejsu zdarzenia, a dokładnie ich daty, są generowane na bierząco. To upraszcza problem nieskończonej seri, która w kodzie jest zrealizowana jako IEnumerable, po którym możemy sobie iterować w nieskończoność. Dopiero w momencie przesunięcia daty pojednczego zdarzenia z serii, miałoby być ono zapisywane wraz z informacją z kiedy do kiedy dane zdarzenie było przesunięte. Przesunięte zdarzenia powinny być załadowane z bazy wraz serią (słownik ```Series._shifts```) aby można byłoby je odpowiednio wyświetlić. Przyjąłem założenie, że ten słownik będzie na tyle mały (w końcu przesunięcia zaplanowanych zdarzeń to sytuacje wyjątkowe), że nie trzeba go wstępnie filtrować i spokojnie można iterować po nim podczas pobierania danych na widok kalendarze. 

### Zmiany w całej serii

Tutaj sprawa nie jest zbyt skomplikowana, ponieważ każda zmiana w serii powoduje, że:

- Albo cała seria jest nadpisywana i wtedy przesunięte zdarzenia są kasowane

- Jeśli seria jest modyfikowana od pewnego zdarzenia "w środku" i dla wszystkich kolejnych, to powoduje to, że de facto seria jest dzielona na dwie - tak działa to w Google. Wtedy wystarczy zmodyfikować starą serię "przycinając" ją do ostatniego niemodyfikowanego dnia i tworząc całkiem nową serię na nowych ustawieniach. Seria mogła by mieć do tego specjalną metodę ```Split()```, która zwracałaby nowo utworzoną serię.
Nie trzeba się tutaj też martwić późniejszymi zmianami w całej serii w przypadku przesunięc pojedynczych zdarzeń, bo każda taka zmiana powoduje, że pojedyncze przesunięcie jest nadpisywane. 


## Czego nie zrobiłem:

### Testów serwisu:

Testy można byłoby oczywiście wprowadzić, ale kod serwisu jest na tyle trywialny, że wolałem się skupić na testach algorytmów, które są realizowane w domenie. Zaimplementowałem ostatecznie testy dlatego, że po prostu pisałem je przed implementacją, tak mi było szybciej ją napisać. Warto byłoby pokryć również testami metodę ```Series.GetEvents()```.

### Wszystich typów rekurencji

Chociaż to nie było wymagane, dodałem część algorytmów dla innych strategi powtarzania zdarzeń. Zasadniczo model został pomyślany tak, aby móc obsłużyć dowolny rodzaj rekurencji. 

## Uwagi

Klasa Recurrence i EndCondition jest inspirowana Discriminated Unions w F#, które razem z operatorem match dają bardzo elastyczne możliwości modelowania różnych przypadków w kodzie. W C# Disriminated unions dają możliwość wyjścia poza polimorfizm, gdy jego użycie nie jest możliwe (przykład: użycie biblioteki ```OneOf```).