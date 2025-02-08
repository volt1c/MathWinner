using MathWinner.Library.Models;

/// find 'x' value for determinant 30
/// determinant - wyznacznik

Matrix<int> B;
for (int x = -10; x <= 10; x++)
{
    B = new MatrixBuilder<int>(3, 3, false)
    .AddRow(7, 5, 1)
    .AddRow(7, 9, 3)
    .AddRow(x, 2, 1)
    .Build();

    if (B.Determinant == 30)
    {
        Console.WriteLine("--------");
        Console.WriteLine("> " + x);
        Console.WriteLine("--------");

    }

}