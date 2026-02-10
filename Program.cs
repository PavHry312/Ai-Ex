using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GraDurak
{
    // Klasa reprezentująca kartę do gry
    public class Karta
    {
        public string Kolor { get; set; } // Kolor karty (Maść)
        public int Ranga { get; set; }    // Wartość karty (6-14, gdzie 14 to As)
        public string Nazwa => GetNazwaRangi() + Kolor;

        private string GetNazwaRangi() => Ranga switch
        {
            11 => "J",
            12 => "Q",
            13 => "K",
            14 => "A",
            _ => Ranga.ToString()
        };
    }

    class Program
    {
        static string[] kolory = { "♠", "♣", "♥", "♦" };
        static List<Karta> talia = new List<Karta>();
        static List<Karta> rekaGracza = new List<Karta>();
        static List<Karta> rekaBota = new List<Karta>();
        static string kolorAtutowy; // Kozyr
        static Random rng = new Random();

        static void Main(string[] args)
        {
            // Ustawienie kodowania konsoli dla wyświetlania symboli kart
            Console.OutputEncoding = Encoding.UTF8;
            InicjalizacjaGry();

            bool turaGracza = true;

            while (rekaGracza.Count > 0 && rekaBota.Count > 0)
            {
                bool obronaNieudana;
                if (turaGracza)
                    obronaNieudana = AtakGracza(); // Gracz atakuje, Bot się broni
                else
                    obronaNieudana = AtakBota();    // Bot atakuje, Gracz się broni

                UzupelnijKarty();

                // Jeśli obrońca zebrał karty, traci turę
                if (!obronaNieudana)
                {
                    turaGracza = !turaGracza;
                }
                else
                {
                    Console.WriteLine("\nObrońca zebrał karty. Tura zostaje pominięta!");
                    Thread.Sleep(2000);
                }
            }

            Console.Clear();
            if (rekaGracza.Count == 0) Console.WriteLine("GRATULACJE! Wygrałeś!");
            else Console.WriteLine("KONIEC GRY! Bot okazał się lepszy.");
            Console.ReadKey();
        }

        static void InicjalizacjaGry()
        {
            // Tworzenie talii 36 kart
            foreach (var k in kolory)
                for (int r = 6; r <= 14; r++)
                    talia.Add(new Karta { Kolor = k, Ranga = r });

            // Tasowanie talii (Algorytm Fisher-Yates)
            talia = talia.OrderBy(x => rng.Next()).ToList();
            kolorAtutowy = talia.Last().Kolor;

            // Rozdawanie po 6 kart
            for (int i = 0; i < 6; i++)
            {
                rekaGracza.Add(DobierzKarte());
                rekaBota.Add(DobierzKarte());
            }
        }

        static Karta DobierzKarte()
        {
            if (talia.Count == 0) return null;
            var k = talia[0];
            talia.RemoveAt(0);
            return k;
        }

        // Bezpieczne pobieranie liczby od użytkownika (Walidacja danych)
        static int PobierzLiczbe(int min, int max, bool dozwoloneWyjscie)
        {
            int wynik;
            while (true)
            {
                string wejscie = Console.ReadLine();
                if (int.TryParse(wejscie, out wynik))
                {
                    if ((dozwoloneWyjscie && wynik == -1) || (wynik >= min && wynik <= max))
                        return wynik;
                }
                Console.Write($"Błąd! Wpisz liczbę od {min} do {max} {(dozwoloneWyjscie ? "lub -1" : "")}: ");
            }
        }

        static bool AtakGracza()
        {
            List<Karta> stol = new List<Karta>();
            bool zebrane = false;

            while (rekaGracza.Count > 0)
            {
                Console.Clear();
                Console.WriteLine($"=== TWOJA TURA (ATAK) | ATUT: {kolorAtutowy} ===");
                Console.WriteLine("Na stole: " + (stol.Count == 0 ? "Pusto" : string.Join(", ", stol.Select(k => k.Nazwa))));
                Console.WriteLine("Twoje karty: " + string.Join(", ", rekaGracza.Select((k, i) => $"[{i}] {k.Nazwa}")));
                Console.Write(stol.Count == 0 ? "Wybierz kartę do ataku: " : "Dorzuć kartę o tym samym nominale (lub -1 aby zakończyć): ");

                int idx = PobierzLiczbe(0, rekaGracza.Count - 1, stol.Count > 0);
                if (idx == -1) break;

                Karta kartaAtaku = rekaGracza[idx];

                // Mechanika dorzucania kart
                if (stol.Count > 0 && !stol.Any(k => k.Ranga == kartaAtaku.Ranga))
                {
                    Console.WriteLine("Nie możesz dorzucić tej karty!");
                    Thread.Sleep(1000);
                    continue;
                }

                rekaGracza.RemoveAt(idx);
                stol.Add(kartaAtaku);

                // Logika obrony Bota (Sztuczna Inteligencja)
                Karta obrona = rekaBota.Where(k => CzyBije(kartaAtaku, k)).OrderBy(k => k.Ranga).FirstOrDefault();
                if (obrona != null)
                {
                    Console.WriteLine($"Bot broni się kartą: {obrona.Nazwa}");
                    stol.Add(obrona);
                    rekaBota.Remove(obrona);
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine("Bot nie może się bronić i zabiera karty!");
                    rekaBota.AddRange(stol);
                    zebrane = true;
                    break;
                }
                if (rekaBota.Count == 0) break;
            }
            Thread.Sleep(1500);
            return zebrane;
        }

        static bool AtakBota()
        {
            List<Karta> stol = new List<Karta>();
            bool zebrane = false;

            while (rekaBota.Count > 0)
            {
                // Wybór karty przez Bota
                Karta kartaAtaku = (stol.Count == 0)
                    ? rekaBota.OrderBy(k => k.Ranga).First()
                    : rekaBota.FirstOrDefault(k => stol.Any(s => s.Ranga == k.Ranga));

                if (kartaAtaku == null) break;

                rekaBota.Remove(kartaAtaku);
                stol.Add(kartaAtaku);

                Console.Clear();
                Console.WriteLine($"=== TURA BOTA (ATAK) | ATUT: {kolorAtutowy} ===");
                Console.WriteLine("Na stole: " + string.Join(", ", stol.Select(k => k.Nazwa)));
                Console.WriteLine("Twoje karty: " + string.Join(", ", rekaGracza.Select((k, i) => $"[{i}] {k.Nazwa}")));
                Console.Write("Broń się (wybierz indeks) lub wpisz -1 aby zabrać karty: ");

                int idx = PobierzLiczbe(0, rekaGracza.Count - 1, true);

                if (idx == -1 || !CzyBije(kartaAtaku, rekaGracza[idx]))
                {
                    Console.WriteLine("Zabierasz karty!");
                    rekaGracza.AddRange(stol);
                    zebrane = true;
                    break;
                }
                else
                {
                    Console.WriteLine($"Bronisz się kartą: {rekaGracza[idx].Nazwa}");
                    stol.Add(rekaGracza[idx]);
                    rekaGracza.RemoveAt(idx);
                }
                if (rekaGracza.Count == 0) break;
                Thread.Sleep(1000);
            }
            return zebrane;
        }

        // Logika porównywania kart
        static bool CzyBije(Karta atak, Karta obrona)
        {
            if (atak.Kolor == obrona.Kolor) return obrona.Ranga > atak.Ranga;
            if (obrona.Kolor == kolorAtutowy) return true;
            return false;
        }

        static void UzupelnijKarty()
        {
            while (rekaGracza.Count < 6 && talia.Count > 0) rekaGracza.Add(DobierzKarte());
            while (rekaBota.Count < 6 && talia.Count > 0) rekaBota.Add(DobierzKarte());
        }
    }
}