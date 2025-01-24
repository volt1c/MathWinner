using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.Write("Podaj bazę (np. 3, 10 itp.): ");
        int numberBase = int.Parse(Console.ReadLine());

        Console.Write("Ile wierszy liczb chcesz dodać? ");
        int rowCount = int.Parse(Console.ReadLine());

        // Wczytujemy wiersze (składniki)
        string[] rows = new string[rowCount];
        for (int i = 0; i < rowCount; i++)
        {
            Console.Write($"Podaj wiersz nr {i + 1}: ");
            rows[i] = Console.ReadLine().Trim();
        }

        // Wczytujemy wiersz wyniku
        Console.Write("Podaj wiersz wyniku: ");
        string result = Console.ReadLine().Trim();

        // Rozwiązujemy zagadkę
        List<Solution> solutions = SolvePuzzle(rows, result, numberBase);

        if (solutions.Count == 0)
        {
            Console.WriteLine("Brak rozwiązań!");
        }
        else
        {
            Console.WriteLine($"\nZnaleziono {solutions.Count} poprawne rozwiązanie(-a):\n");
            foreach (var sol in solutions)
            {
                // Wypisujemy w czytelnej formie
                sol.PrintSolution();
                Console.WriteLine();
            }
        }

        Console.WriteLine("Naciśnij Enter, aby zakończyć.");
        Console.ReadLine();
    }

    /// <summary>
    /// Główna metoda rozwiązująca zagadkę.
    /// Zwraca listę wszystkich możliwych rozwiązań (z kompletnym przypisaniem znaków „?”).
    /// </summary>
    static List<Solution> SolvePuzzle(string[] rows, string result, int numberBase)
    {
        // Najpierw ustalamy maksymalną długość (kolumnę) spośród wszystkich wierszy i wyniku
        int maxLen = result.Length;
        foreach (var row in rows)
        {
            if (row.Length > maxLen) maxLen = row.Length;
        }

        // Odczytujemy wszystkie ciągi w "odwróconej" postaci (prawa cyfra – indeks 0)
        // i przechowujemy w tablicy, gdzie -1 będzie oznaczać "?"
        // a każda inna liczba [0..numberBase-1] oznacza konkretną cyfrę
        List<int[]> rowDigits = new List<int[]>();
        foreach (var row in rows)
            rowDigits.Add(ParseRow(row, maxLen, numberBase));

        int[] resultDigits = ParseRow(result, maxLen, numberBase);

        // Lista wszystkich pozycji w całym układzie, w których występuje „?”.
        // Każdą taką pozycję będziemy wypełniać liczbą z [0..numberBase-1].
        // Pozycja określona przez: (czy to któryś ze składników, czy wynik) + index w tym wierszu.
        List<UnknownPosition> unknowns = new List<UnknownPosition>();

        // Zebranie nieznanych cyfr w składnikach
        for (int r = 0; r < rows.Length; r++)
        {
            for (int col = 0; col < maxLen; col++)
            {
                if (rowDigits[r][col] == -1) // znak „?”
                {
                    unknowns.Add(new UnknownPosition
                    {
                        IsResult = false,
                        RowIndex = r,
                        ColumnIndex = col
                    });
                }
            }
        }
        // Zebranie nieznanych cyfr w wyniku
        for (int col = 0; col < maxLen; col++)
        {
            if (resultDigits[col] == -1)
            {
                unknowns.Add(new UnknownPosition
                {
                    IsResult = true,
                    RowIndex = 0,     // Dla wyniku nie ma znaczenia
                    ColumnIndex = col
                });
            }
        }

        // Przygotowujemy listę rozwiązań
        List<Solution> solutions = new List<Solution>();

        // Funkcja rekurencyjna do backtrackingu
        void Backtrack(int idx)
        {
            // Jeżeli wszystkie „?” zostały przypisane, sprawdzamy czy suma się zgadza
            if (idx == unknowns.Count)
            {
                if (CheckSum(rowDigits, resultDigits, numberBase))
                {
                    // Tworzymy obiekt z rozwiniętymi wierszami (już bez „?”)
                    solutions.Add(new Solution
                    {
                        Rows = ConvertToStrings(rowDigits, numberBase),
                        Result = ConvertToString(resultDigits, numberBase)
                    });
                }
                return;
            }

            // W przeciwnym razie – przypisujemy bieżącemu „?” każdą możliwą cyfrę i rekurujemy
            var upos = unknowns[idx];
            for (int digit = 0; digit < numberBase; digit++)
            {
                if (!upos.IsResult)
                    rowDigits[upos.RowIndex][upos.ColumnIndex] = digit;
                else
                    resultDigits[upos.ColumnIndex] = digit;

                Backtrack(idx + 1);

                // Przywracamy znak „?” (choć można tego nie robić, bo i tak go nadpiszemy w następnym przebiegu)
                if (!upos.IsResult)
                    rowDigits[upos.RowIndex][upos.ColumnIndex] = -1;
                else
                    resultDigits[upos.ColumnIndex] = -1;
            }
        }

        // Uruchamiamy backtracking
        Backtrack(0);

        return solutions;
    }

    /// <summary>
    /// Sprawdzenie, czy zdefiniowane w rowDigits + resultDigits równanie 
    /// (suma wierszy = wynik) jest poprawne w danej bazie.
    /// </summary>
    static bool CheckSum(List<int[]> rowDigits, int[] resultDigits, int numberBase)
    {
        int maxLen = resultDigits.Length;
        int carry = 0;

        // Sumujemy kolumnami, od 0 do maxLen-1
        for (int col = 0; col < maxLen; col++)
        {
            int columnSum = carry;
            // Dodajemy cyfry ze wszystkich wierszy
            foreach (var rd in rowDigits)
            {
                columnSum += rd[col];
            }

            int expected = resultDigits[col];
            // Sprawdzamy w bazie
            if (columnSum % numberBase != expected)
                return false;

            carry = columnSum / numberBase;
        }

        // Na końcu nie powinno być przeniesienia, jeśli wszystkie liczby mają
        // maksymalną kolumnę = maxLen. Jeśli akceptujemy przeniesienie poza wynik,
        // należałoby to inaczej interpretować. W standardowych zagadkach – 0 oznacza brak.
        return (carry == 0);
    }

    /// <summary>
    /// Konwertuje wczytany ciąg (np. "2?102") na tablicę cyfr (odwróconą),
    /// gdzie -1 oznacza znak „?” i normalna liczba [0..baza-1] oznacza znaną cyfrę.
    /// Wynik ma długość = maxLen (reszta wypełniona zerami).
    /// </summary>
    static int[] ParseRow(string row, int maxLen, int numberBase)
    {
        var digits = new int[maxLen];
        for (int i = 0; i < maxLen; i++)
            digits[i] = 0; // wypełnienie wstępne

        // Wypełniamy od końca (row[row.Length-1] to najmniej znacząca cyfra)
        int pos = 0;
        for (int i = row.Length - 1; i >= 0; i--)
        {
            char c = row[i];
            if (c == '?')
            {
                digits[pos] = -1; // znak zapytania
            }
            else
            {
                // Parsujemy cyfrę w bazie <= 36 (dla uproszczenia: 0..9, A..Z)
                int val = ParseDigit(c);
                if (val >= numberBase)
                {
                    // Niepoprawny znak dla danej bazy!
                    // W realnym kodzie można zgłosić wyjątek.
                    // Tutaj przyjmijmy, że to błąd – zwracamy cokolwiek.
                    digits[pos] = -1;
                }
                else
                {
                    digits[pos] = val;
                }
            }
            pos++;
        }
        return digits;
    }

    /// <summary>
    /// Zamienia jedną cyfrę w postaci char (0..9 lub A..Z) na liczbę w [0..35].
    /// </summary>
    static int ParseDigit(char c)
    {
        if (char.IsDigit(c)) return (c - '0');
        // Gdyby ktoś chciał obsługiwać bazy > 10:
        if (c >= 'A' && c <= 'Z') return (c - 'A') + 10;
        if (c >= 'a' && c <= 'z') return (c - 'a') + 10;
        // Nieznany znak
        return -1;
    }

    /// <summary>
    /// Zamienia tablicę cyfr (odwróconą) na napis (od lewej do prawej),
    /// używając standardowego zapisu 0..9, A..Z dla cyfr >= 10.
    /// </summary>
    static string ConvertToString(int[] digits, int numberBase)
    {
        // Znajdź pozycję pierwszej niezerowej cyfry od końca (aby przyciąć wiodące zera)
        int idx = digits.Length - 1;
        while (idx > 0 && digits[idx] == 0)
            idx--;

        // Składamy string od idx do 0
        List<char> list = new List<char>();
        for (int i = idx; i >= 0; i--)
        {
            list.Add(DigitToChar(digits[i]));
        }

        return new string(list.ToArray());
    }

    /// <summary>
    /// Konwertuje wszystkie wiersze (każdy w postaci tablicy cyfr odwróconej) na teksty.
    /// </summary>
    static string[] ConvertToStrings(List<int[]> rows, int numberBase)
    {
        string[] result = new string[rows.Count];
        for (int i = 0; i < rows.Count; i++)
        {
            result[i] = ConvertToString(rows[i], numberBase);
        }
        return result;
    }

    /// <summary>
    /// Zamienia cyfrę [0..35] na char 0..9,A..Z.
    /// </summary>
    static char DigitToChar(int d)
    {
        if (d < 10) return (char)('0' + d);
        return (char)('A' + (d - 10));
    }
}

/// <summary>
/// Reprezentuje jedną znalezioną solucję (kompletne wypełnienie wszystkich znaków „?”).
/// </summary>
class Solution
{
    public string[] Rows;
    public string Result;

    /// <summary>
    /// Wypisuje rozwiązanie w postaci:
    ///    wiersz1
    ///  + wiersz2
    ///  + ...
    ///  ----------
    ///    result
    /// </summary>
    public void PrintSolution()
    {
        // Zależnie od potrzeb można ładniej formatować:
        for (int i = 0; i < Rows.Length; i++)
        {
            Console.WriteLine("  " + Rows[i]);
        }
        Console.WriteLine("----------");
        Console.WriteLine("  " + Result);
    }
}

/// <summary>
/// Pozycja, w której mamy „?” – w którym wierszu, kolumnie i czy to wiersz wyniku.
/// </summary>
struct UnknownPosition
{
    public bool IsResult;
    public int RowIndex;
    public int ColumnIndex;
}