using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MathWinner.Library.Models
{
    public class Matrix<T> :
        IEquatable<Matrix<T>>

        where T : INumber<T>
    {
        private readonly T[][] _matrix;
        public int N;
        public int M;

        public Matrix(T[][] primitiveMatrix)
        {
            if (primitiveMatrix == null || primitiveMatrix.Length == 0)
            {
                throw new ArgumentException("Wymiary macierzy muszą być większe od 0.");
            }
            _matrix = primitiveMatrix;

            N = primitiveMatrix.Length;
            M = primitiveMatrix[0].Length;
        }

        public T Determinant
        {
            get
            {
                if (!IsSquare())
                {
                    throw new InvalidOperationException("Nie można policzyć wyznacznika dla niekwadratowych macierzy.");
                }
                return CalculateDeterminant(_matrix);
            }
        }

        private T CalculateDeterminant(T[][] matrix)
        {
            int size = matrix.Length;

            if (size == 1)
            {
                return matrix[0][0];
            }

            if (size == 2)
            {
                return matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];
            }

            T determinant = T.Zero;
            for (int i = 0; i < size; i++)
            {
                T[][] minor = GetMinor(matrix, 0, i);
                determinant += (i % 2 == 0 ? T.One : -T.One) * matrix[0][i] * CalculateDeterminant(minor);
            }

            return determinant;
        }

        private T[][] GetMinor(T[][] matrix, int row, int column)
        {
            int size = matrix.Length;
            T[][] minor = new T[size - 1][];
            for (int i = 0; i < size - 1; i++)
            {
                minor[i] = new T[size - 1];
            }

            for (int i = 0, mi = 0; i < size; i++)
            {
                if (i == row) continue;
                for (int j = 0, mj = 0; j < size; j++)
                {
                    if (j == column) continue;
                    minor[mi][mj] = matrix[i][j];
                    mj++;
                }
                mi++;
            }

            return minor;
        }

        public Matrix<T> Transposition()
        {
            T[][] transposedMatrix = new T[M][];
            for (int i = 0; i < M; i++)
            {
                transposedMatrix[i] = new T[N];
                for (int j = 0; j < N; j++)
                {
                    transposedMatrix[i][j] = _matrix[j][i];
                }
            }
            return new Matrix<T>(transposedMatrix);
        }

        public T Get(int n, int m)
        {
            if (n < 0 || n >= N || m < 0 || m >= M)
            {
                throw new IndexOutOfRangeException("Indeks wykracza poza zakres macierzy.");
            }
            return _matrix[n][m];
        }

        public bool CanBeMultiplied(Matrix<T> other) => M == other.N;

        public bool CanBeMultiplied(int n, int m) => M == n;

        public bool IsSquare() => N == M;
        public bool CanBePowerd() => IsSquare();

        public bool CanBeAdded(Matrix<T> other)
        {
            return N == other.N && M == other.M;
        }
        public bool CanBeAdded(int n, int m)
        {
            return N == n && M == m;
        }

        public bool CanBeSubtracted(Matrix<T> other) => CanBeAdded(other);

        public bool CanBeSubtracted(int n, int m) => CanBeAdded(n, m);

        public bool is1x1() => N == 1 && M == 1;

        public static Matrix<T> Identity(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentException("Wymiary macierzy muszą być większe od 0.");
            }

            T[][] identityMatrix = new T[n][];
            for (int i = 0; i < n; i++)
            {
                identityMatrix[i] = new T[n];
                for (int j = 0; j < n; j++)
                {
                    identityMatrix[i][j] = i == j ? T.One : T.Zero;
                }
            }

            return new Matrix<T>(identityMatrix);
        }
        public static Matrix<T> FromNumber(T value)
        {
            return new Matrix<T>([[value]]);
        }

        public T[][] ToArray() 
        {
            T[][] array = new T[N][];
            for (int i = 0; i < N; i++) 
            {
                array[i] = new T[M];
                for (int j = 0; j < M; j++) 
                {
                    array[i][j] = _matrix[i][j];
                }
            }
            return array;
        }
        public List<List<T>> ToList()
        {
            List<List<T>> list = new List<List<T>>(N);
            for (int i = 0; i < N; i++)
            {
                List<T> row = new List<T>(M);
                for (int j = 0; j < M; j++)
                {
                    row.Add(_matrix[i][j]);
                }
                list.Add(row);
            }
            return list;
        }

        public bool Equals(Matrix<T>? other)
        {
            if (other == null || N != other.N || M != other.M)
            {
                return false;
            }

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if (!_matrix[i][j].Equals(other._matrix[i][j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
