using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MathWinner.Library.Models
{
    public class MatrixBuilder<T> where T : INumber<T>
    {
        private List<List<T>> _rows = new List<List<T>>();
        private int _n = 1;
        private int _m = 1;

        public Matrix<T> Matrix => Build();

        public MatrixBuilder() : this(1, 1) {
            _rows.Add(new List<T> { T.Zero });
        }

        public MatrixBuilder(int n, int m) 
        { 
            if (n < 1 || m < 1)
            {
                throw new ArgumentException("Wymiary macierzy muszą być większe od 0.");
            }
            _n = n;
            _m = m;

            for (int i = 0; i < _n; i++)
            {
                List<T> row = new List<T>();
                for (int j = 0; j < _m; j++)
                {
                    row.Add(item: default);
                }
                _rows.Add(row);
            }
        }

        public MatrixBuilder<T> SetN(int n) 
        { 
            if (n < 1)
            {
                throw new ArgumentException("Liczba wierszy musi być większa od 0.");
            }
            _n = n;
            _rows.AddRange(Enumerable.Repeat(new List<T>(), n - _rows.Count));
            return this;
        }

        public MatrixBuilder<T> SetM(int m)
        {
            if (m < 1)
            {
                throw new ArgumentException("Liczba kolumn musi być większa od 0.");
            }
            _m = m;
            _rows.ForEach(row => row.AddRange(Enumerable.Repeat(T.Zero, m - row.Count)));
            return this;
        }

        public MatrixBuilder<T> AddRow(params T[] args) 
        {
            if (args.Length != _m)
                throw new ArgumentException("Długość wiersza musi odpowiadać liczbie kolumn.");

            if (_rows.Count >= _n)
                throw new InvalidOperationException("Przekroczono maksymalną liczbę wierszy.");

            _rows.Add(args.ToList());
            return this;
        }

        public MatrixBuilder<T> AddRow(List<T> row) 
        { 
            return AddRow(row.ToArray());
        }

        public MatrixBuilder<T> Set(int n, int m, T value) {
            if (n < 0 || n >= _n || m < 0 || m >= _m)
            {
                throw new IndexOutOfRangeException();
            }

            // Add rows if necessary
            while (_rows.Count <= n)
            {
                _rows.Add(Enumerable.Repeat(T.Zero, _m).ToList());
            }

            _rows[n][m] = value;
            return this;
        }

        public Matrix<T> Build() 
        {
            if (_rows.Count < _n)
            {
                throw new InvalidOperationException("Nie wszystkie wiersze zostały uzupełnione.");
            }

            var primitiveMatrix = _rows.Select(row => row.ToArray()).ToArray();
            return new Matrix<T>(primitiveMatrix);
        }
    }
}
