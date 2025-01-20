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
        public int N { get => throw new NotImplementedException(); }
        public int M { get => throw new NotImplementedException(); }

        public Matrix(T[][] primitiveMatrix) { throw new NotImplementedException(); }

        public T Determinant { get => throw new NotImplementedException(); }

        public Matrix<T> Transposition() { throw new NotImplementedException(); }

        public T Get(int n, int m) { throw new NotImplementedException(); }

        public bool CanBeMultiplied(Matrix<T> other) { throw new NotImplementedException(); }
        public bool CanBeMultiplied(int n, int m) { throw new NotImplementedException(); }

        public bool IsSquare() => N == M;
        public bool CanBePowerd() => IsSquare();

        public bool CanBeAdded(Matrix<T> other) { throw new NotImplementedException(); }
        public bool CanBeAdded(int n, int m) { throw new NotImplementedException(); }

        public bool CanBeSubtracted(Matrix<T> other) { throw new NotImplementedException(); }
        public bool CanBeSubtracted(int n, int m) { throw new NotImplementedException(); }

        public bool CanBeDevided(Matrix<T> other) { throw new NotImplementedException(); }

        public static Matrix<T> Identity(int n) { throw new NotImplementedException(); }
        public static Matrix<T> FromNumber(T value) { throw new NotImplementedException(); }

        public T[][] ToArray() { throw new NotImplementedException(); }
        public List<List<T>> ToList() { throw new NotImplementedException(); }

        public bool Equals(Matrix<T>? other)
        {
            throw new NotImplementedException();
        }
    }
}
