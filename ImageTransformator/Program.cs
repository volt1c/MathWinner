using System;

class Program
{
    // Struktura do przechowywania punktów (współrzędne x, y)
    public struct Punkt
    {
        public double X;
        public double Y;

        public Punkt(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    // Funkcja obliczająca macierz przekształcenia
    public static double[,] ObliczMacierzPrzeksztalcenia(Punkt[] starePunkty, Punkt[] nowePunkty)
    {
        // Zakładamy, że mamy tylko translację (można rozszerzyć na inne operacje)
        double deltaX = nowePunkty[0].X - starePunkty[0].X;
        double deltaY = nowePunkty[0].Y - starePunkty[0].Y;

        // Macierz przekształcenia 3x3 (w celu uwzględnienia translacji)
        double[,] macierzPrzeksztalcenia = new double[3, 3];

        // Ustawienie elementów macierzy (operacja translacji)
        macierzPrzeksztalcenia[0, 0] = 1;
        macierzPrzeksztalcenia[0, 1] = 0;
        macierzPrzeksztalcenia[0, 2] = deltaX;

        macierzPrzeksztalcenia[1, 0] = 0;
        macierzPrzeksztalcenia[1, 1] = 1;
        macierzPrzeksztalcenia[1, 2] = deltaY;

        macierzPrzeksztalcenia[2, 0] = 0;
        macierzPrzeksztalcenia[2, 1] = 0;
        macierzPrzeksztalcenia[2, 2] = 1;

        return macierzPrzeksztalcenia;
    }

    // Funkcja do wyświetlania macierzy
    public static void WyswietlMacierz(double[,] macierz)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(macierz[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }

    // Funkcja główna
    static void Main(string[] args)
    {
        // Zczytanie punktów wejściowych
        Punkt[] starePunkty = new Punkt[4];
        Punkt[] nowePunkty = new Punkt[4];

        Console.WriteLine("Podaj współrzędne 4 punktów (kąty kwadratu) w układzie współrzędnych:");

        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine($"Podaj punkt {i + 1} (x, y) starego kwadratu:");
            string[] dane = Console.ReadLine().Split(',');
            double x = Convert.ToDouble(dane[0]);
            double y = Convert.ToDouble(dane[1]);
            starePunkty[i] = new Punkt(x, y);
        }

        Console.WriteLine("\nPodaj współrzędne 4 punktów (kąty kwadratu) w nowym układzie współrzędnych:");

        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine($"Podaj punkt {i + 1} (x, y) nowego kwadratu:");
            string[] dane = Console.ReadLine().Split(',');
            double x = Convert.ToDouble(dane[0]);
            double y = Convert.ToDouble(dane[1]);
            nowePunkty[i] = new Punkt(x, y);
        }

        // Obliczanie macierzy przekształcenia
        double[,] macierz = ObliczMacierzPrzeksztalcenia(starePunkty, nowePunkty);

        // Wyświetlanie macierzy
        Console.WriteLine("\nMacierz przekształcenia:");
        WyswietlMacierz(macierz);
    }
}
