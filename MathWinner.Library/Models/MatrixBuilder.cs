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
        public Matrix<T> Matrix => Build();

        public MatrixBuilder() { throw new NotImplementedException(); }
        public MatrixBuilder(int n, int m) { throw new NotImplementedException(); }

        public MatrixBuilder<T> SetN(int n) { throw new NotImplementedException(); }
        public MatrixBuilder<T> SetM(int m) { throw new NotImplementedException(); }

        public MatrixBuilder<T> AddRow(params T[] args) { throw new NotImplementedException(); }
        public MatrixBuilder<T> AddRow(List<T> row) { throw new NotImplementedException(); }

        public MatrixBuilder<T> Set(int n, int m, T value) { throw new NotImplementedException(); }

        public Matrix<T> Build() { throw new NotImplementedException(); }
    }
}
