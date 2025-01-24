using System;
using System.Globalization;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("PROGRAM ROZWIAZUJACY nieliniowo: A * A^-1 = I DLA MACIERZY 3x3");
        Console.WriteLine("Podaj macierz A (3x3) – znane elementy lub '?' dla nieznanych:");
        double[,] A = ReadMatrixFromUser();

        Console.WriteLine("\nPodaj macierz A^-1 (3x3) – znane elementy lub '?' dla nieznanych:");
        double[,] Ainv = ReadMatrixFromUser();

        // Zbuduj wektor niewiadomych x[] z tych pól, gdzie jest double.NaN
        var unknowns = new System.Collections.Generic.List<(bool isA, int i, int j)>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (double.IsNaN(A[i, j]))
                    unknowns.Add((true, i, j));
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (double.IsNaN(Ainv[i, j]))
                    unknowns.Add((false, i, j));
            }
        }

        int m = unknowns.Count; // liczba niewiadomych

        if (m == 0)
        {
            Console.WriteLine("Brak niewiadomych – sprawdzamy tylko czy A*A^-1 == I.");
            double[,] product = Multiply(A, Ainv);
            if (IsIdentity(product))
                Console.WriteLine("Jest OK – to macierze odwrotne.");
            else
                Console.WriteLine("Niestety A*A^-1 != I.");
            return;
        }

        // Przygotowanie wektora startowego – np. same 1.0 (albo 0.0)
        double[] x0 = new double[m];
        for (int i = 0; i < m; i++)
            x0[i] = 1.0; // start

        Console.WriteLine($"Liczba niewiadomych: {m}. Startowa probna wartosc = 1.0 dla kazdej.");

        // Uruchamiamy metodę Newtona
        bool success = SolveNonlinearSystem(A, Ainv, unknowns, ref x0);

        if (!success)
        {
            Console.WriteLine("\nMetoda Newtona nie znalazla rozwiazania (brak zbieznosci?)\n");
        }
        else
        {
            // Wstaw do macierzy
            for (int k = 0; k < m; k++)
            {
                var (isA, i, j) = unknowns[k];
                if (isA) A[i, j] = x0[k];
                else Ainv[i, j] = x0[k];
            }

            Console.WriteLine("\n=== OTRZYMANE ROZWIAZANIE ===");
            Console.WriteLine("Macierz A (uzupelniona):");
            PrintMatrix(A);

            Console.WriteLine("Macierz A^-1 (uzupelniona):");
            PrintMatrix(Ainv);

            double[,] product = Multiply(A, Ainv);
            Console.WriteLine("Iloczyn A * A^-1 (powinien byc macierza I):");
            PrintMatrix(product);

            if (IsIdentity(product, 1e-5))
                Console.WriteLine("Wyglada OK (w granicach bledu).");
            else
                Console.WriteLine("NIE jest macierza jednostkowa! Mozliwe bledy obliczen lub brak faktycznego rozwiazania.");
        }
    }

    /// <summary>
    /// Metoda Newtona do układu 9 równań nieliniowych (A*A^-1=I).
    /// unknowns – indeksy w (A/Ainv) które są w wektorze x[].
    /// x – wektor (start, a potem wynik).
    /// Zwraca true, jeśli udało się zejść do ||f(x)|| < tolerance.
    /// </summary>
    static bool SolveNonlinearSystem(double[,] A, double[,] Ainv,
        System.Collections.Generic.List<(bool isA, int i, int j)> unknowns,
        ref double[] x)
    {
        double tolerance = 1e-7;
        int maxIter = 100;  // liczba iteracji
        double alpha = 1.0; // ew. współczynnik tłumienia kroków

        for (int iter = 0; iter < maxIter; iter++)
        {
            // 1. Obliczamy f(x) – wektor 9-elementowy
            double[] fx = ComputeResidual(A, Ainv, unknowns, x);

            // 2. Sprawdzamy normę ||f(x)||
            double norm = 0.0;
            for (int i = 0; i < fx.Length; i++)
                norm += fx[i] * fx[i];
            norm = Math.Sqrt(norm);

            Console.WriteLine($"Iteracja {iter}, ||f(x)|| = {norm:E6}");

            if (norm < tolerance)
            {
                // Sukces – mamy dość mały błąd
                return true;
            }

            // 3. Liczymy Jacobiego J(x) ~ df/dx (9x m) – tu m = unknowns.Count
            double[,] J = ApproxJacobian(A, Ainv, unknowns, x, fx);

            // 4. Rozwiązujemy (J^T J) delta = -J^T f metodą Gaussa (lub cokolwiek)
            //    – to jest wariant "Newtona Levenberga–Marquardta" / "Gaussa-Newtona"
            //    ale zrobimy proste Jx = -f. Uwaga: standardowo Newton to J delta = -f,
            //    ale jeśli 9 != m, to musimy się troszkę ratować. Możemy zrobić:
            //         J wymiary: 9 x m
            //      => J^T wymiary: m x 9
            //      => J^T * J wymiary: m x m
            //         J^T * f wymiary: m
            //      => Rozwiązać (J^T J) * delta = - J^T * f
            double[,] JT = Transpose(J);
            double[,] JTJ = MultiplyMatrix(JT, J);    // (m x 9) * (9 x m) => (m x m)
            double[] JTf = MultiplyVector(JT, fx);    // (m x 9) * (9) => (m)
            double[] negJTf = new double[JTf.Length];
            for (int i = 0; i < JTf.Length; i++)
                negJTf[i] = -JTf[i];

            // Rozwiązujemy (JTJ) * delta = (-JTf)
            double[] delta = new double[x.Length];
            if (!SolveGauss(JTJ, negJTf, delta))
            {
                Console.WriteLine("Nie udalo sie rozwiazac rownania J^T J * delta = -J^T f (macierz osobliwa?).");
                return false;
            }

            // 5. Aktualizacja x <- x + alpha * delta (alpha=1 – standard Newtona)
            for (int i = 0; i < x.Length; i++)
                x[i] += alpha * delta[i];
        }

        // Jeśli tu dotarliśmy, nie zbliżyliśmy się do ||f|| < tolerance
        return false;
    }

    /// <summary>
    /// Oblicza rezydua f(x). Mamy 9 równań: (A*Ainv)[i,j] - delta_{ij} = 0, i=0..2, j=0..2
    /// Zwraca tablicę 9 liczb: f_0..f_8.
    /// </summary>
    static double[] ComputeResidual(double[,] A, double[,] Ainv,
        System.Collections.Generic.List<(bool isA, int i, int j)> unknowns,
        double[] x)
    {
        // Wstaw x do (kopii) A i Ainv
        double[,] Atemp = (double[,])A.Clone();
        double[,] Ainvtemp = (double[,])Ainv.Clone();
        for (int k = 0; k < x.Length; k++)
        {
            var (isA, i, j) = unknowns[k];
            if (isA) Atemp[i, j] = x[k];
            else Ainvtemp[i, j] = x[k];
        }

        // Policz (Atemp * Ainvtemp)[i,j] - delta_ij
        // spakuj w tablicę 9-elementową w kolejności (i=0..2, j=0..2)
        double[] f = new double[9];
        int idx = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                double sum = 0.0;
                for (int k = 0; k < 3; k++)
                    sum += Atemp[i, k] * Ainvtemp[k, j];

                double delta = (i == j) ? 1.0 : 0.0;
                f[idx] = sum - delta;
                idx++;
            }
        }
        return f;
    }

    /// <summary>
    /// Aproksymuje macierz Jacobiego J (9 x m) metodą różnicową:
    /// J_{r,c} = d f_r / d x_c.
    /// f_r to r-ty element wektora f(x) (z computeResidual).
    /// x_c to c-ta niewiadoma.
    /// </summary>
    static double[,] ApproxJacobian(double[,] A, double[,] Ainv,
        System.Collections.Generic.List<(bool isA, int i, int j)> unknowns,
        double[] x, double[] fx)
    {
        int nEq = 9;      // 9 równań
        int mVar = x.Length;
        double[,] J = new double[nEq, mVar];

        double eps = 1e-6;
        // Dla każdej zmiennej c
        for (int c = 0; c < mVar; c++)
        {
            double oldVal = x[c];

            // x_c + eps
            x[c] = oldVal + eps;
            double[] fxPlus = ComputeResidual(A, Ainv, unknowns, x);

            // cofać x[c]
            x[c] = oldVal;

            for (int r = 0; r < nEq; r++)
            {
                J[r, c] = (fxPlus[r] - fx[r]) / eps;
            }
        }

        return J;
    }

    /// <summary>
    /// Rozwiązuje układ równań (mat xVector = rhs) metodą Gaussa:
    ///  mat - macierz (m x m),
    ///  rhs - wektor (m).
    /// Wynik w solution (m).
    /// Zwraca true/false, czy się udało.
    /// </summary>
    static bool SolveGauss(double[,] mat, double[] rhs, double[] solution)
    {
        int n = mat.GetLength(0);
        // Tworzymy rozszerzoną macierz
        double[,] ext = new double[n, n + 1];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
                ext[i, j] = mat[i, j];
            ext[i, n] = rhs[i];
        }

        // Eliminacja
        for (int i = 0; i < n; i++)
        {
            // Szukanie pivotu
            double maxVal = Math.Abs(ext[i, i]);
            int pivot = i;
            for (int r = i + 1; r < n; r++)
            {
                double val = Math.Abs(ext[r, i]);
                if (val > maxVal)
                {
                    maxVal = val;
                    pivot = r;
                }
            }
            // Jeśli pivot == 0 -> brak
            if (Math.Abs(maxVal) < 1e-14)
            {
                return false;
            }
            // Zamiana wierszy
            if (pivot != i)
            {
                for (int c = i; c < n + 1; c++)
                {
                    double tmp = ext[i, c];
                    ext[i, c] = ext[pivot, c];
                    ext[pivot, c] = tmp;
                }
            }
            // Normalizacja
            double diag = ext[i, i];
            for (int c = i; c < n + 1; c++)
                ext[i, c] /= diag;

            // Zerowanie poniżej
            for (int r = i + 1; r < n; r++)
            {
                double factor = ext[r, i];
                for (int c = i; c < n + 1; c++)
                    ext[r, c] -= factor * ext[i, c];
            }
        }

        // Back-substitution
        for (int i = n - 1; i >= 0; i--)
        {
            double s = ext[i, n];
            for (int c = i + 1; c < n; c++)
                s -= ext[i, c] * solution[c];
            solution[i] = s;
        }
        return true;
    }

    // --- Funkcje pomocnicze (macierzowe) ---
    static double[,] Multiply(double[,] A, double[,] B)
    {
        double[,] C = new double[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                double sum = 0;
                for (int k = 0; k < 3; k++)
                {
                    sum += A[i, k] * B[k, j];
                }
                C[i, j] = sum;
            }
        }
        return C;
    }
    static bool IsIdentity(double[,] M, double eps = 1e-7)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                double expected = (i == j) ? 1.0 : 0.0;
                if (Math.Abs(M[i, j] - expected) > eps)
                    return false;
            }
        }
        return true;
    }
    static double[,] Transpose(double[,] M)
    {
        int rows = M.GetLength(0);
        int cols = M.GetLength(1);
        double[,] T = new double[cols, rows];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                T[j, i] = M[i, j];
        return T;
    }
    static double[,] MultiplyMatrix(double[,] A, double[,] B)
    {
        int Ar = A.GetLength(0), Ac = A.GetLength(1);
        int Br = B.GetLength(0), Bc = B.GetLength(1);
        if (Ac != Br) throw new Exception("Zle wymiary do mnozenia");
        double[,] C = new double[Ar, Bc];
        for (int i = 0; i < Ar; i++)
        {
            for (int j = 0; j < Bc; j++)
            {
                double sum = 0;
                for (int k = 0; k < Ac; k++)
                    sum += A[i, k] * B[k, j];
                C[i, j] = sum;
            }
        }
        return C;
    }
    static double[] MultiplyVector(double[,] A, double[] v)
    {
        int n = A.GetLength(0);
        int m = A.GetLength(1);
        if (m != v.Length) throw new Exception("Zle wymiary do mnozenia Axv");
        double[] w = new double[n];
        for (int i = 0; i < n; i++)
        {
            double sum = 0;
            for (int j = 0; j < m; j++)
                sum += A[i, j] * v[j];
            w[i] = sum;
        }
        return w;
    }

    /// <summary>
    /// Wczytuje 3x3 z konsoli. "?" -> double.NaN, liczba -> double.
    /// </summary>
    static double[,] ReadMatrixFromUser()
    {
        double[,] M = new double[3, 3];
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"Wiersz {i + 1} (3 wartosci, oddzielone spacjami):");
            var line = Console.ReadLine();
            if (line == null) throw new Exception("Brak danych");
            var parts = line.Split(' ', '\t');
            if (parts.Length < 3) throw new Exception("Za malo wartosci w wierszu!");
            for (int j = 0; j < 3; j++)
            {
                if (parts[j] == "?" || parts[j] == "?," || parts[j].Contains("?"))
                {
                    M[i, j] = double.NaN;
                }
                else
                {
                    if (double.TryParse(parts[j], NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                        M[i, j] = val;
                    else
                        M[i, j] = double.NaN;
                }
            }
        }
        return M;
    }

    /// <summary>
    /// Proste wypisanie macierzy 3x3.
    /// </summary>
    static void PrintMatrix(double[,] M)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
                Console.Write($"{M[i, j],8:F3} ");
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}