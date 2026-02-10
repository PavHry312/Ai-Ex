# Projekt Zaliczeniowy: Gra karciana "Dureń" (Durak)

### Opis projektu
Projekt stanowi kompletną, konsolową implementację popularnej gry karcianej "Dureń". Aplikacja została napisana w języku C# i demonstruje zastosowanie programowania obiektowego (OOP) oraz złożonych algorytmów decyzyjnych.

### Kluczowe funkcjonalności i zaawansowane aspekty techniczne:
* **Logika Rozgrywki:** Implementacja pełnego zestawu zasad, w tym mechaniki atutu (trump), obrony oraz możliwości dorzucania kart o tym samym nominale.
* **Sztuczna Inteligencja (AI):** Przeciwnik komputerowy podejmuje decyzje w oparciu o analizę stanu gry, dążąc do minimalizacji strat (wykorzystanie kart o najniższych nominałach do obrony).
* **Zarządzanie stanem i kolekcjami:** Wykorzystanie list generycznych (`List<T>`) oraz zaawansowanych zapytań **LINQ** do filtrowania i wyszukiwania kart na stole oraz w rękach graczy.
* **Odporność na błędy (Error Handling):** Zastosowanie metody `int.TryParse` oraz walidacji danych wejściowych, co zapobiega awariom programu przy błędnym wprowadzeniu danych przez użytkownika.
* **Architektura:** Podział logiczny na klasy (Card, Program) zapewniający czytelność i łatwość rozbudowy kodu.

### Technologie:
* Język: C#
* Platforma: .NET 6.0 / 8.0
* Narzędzia: Visual Studio 2022

### Instrukcja uruchomienia:
1. Sklonuj repozytorium lub pobierz pliki źródłowe.
2. Otwórz plik rozwiązania `.sln` w środowisku Visual Studio.
3. Upewnij się, że kodowanie konsoli wspiera znaki Unicode (program automatycznie ustawia `OutputEncoding = Encoding.UTF8`).
4. Naciśnij klawisz **F5**, aby skompilować i uruchomić grę.

**Zasady sterowania:**
* Wybieraj karty, wpisując ich indeks (numer w nawiasie) widoczny na ekranie.
* Wpisz `-1`, aby zakończyć atak lub zebrać karty ze stołu.
