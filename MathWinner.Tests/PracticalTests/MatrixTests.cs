using MathWinner.Library.Models;
using System;
using Xunit;

namespace MathWinner.Tests
{
    public class MatrixTests
    {
        [Fact]
        public void TestMatrixOperations()
        {
            var A = new MatrixBuilder<int>(5, 4).Matrix;
            var B = new MatrixBuilder<int>(9, 9).Matrix;
            var C = new MatrixBuilder<int>(4, 5).Matrix;

            var At = A.Transposition();
            var Bt = B.Transposition();
            var Ct = C.Transposition();

            // 1
            Assert.True(C.CanBeAdded(At), "1");
            // 2
            Assert.False(C.CanBeMultiplied(B), "2");
            // 3
            Assert.True(B.CanBePowerd(), "3");
            // 4
            Assert.True(A.CanBeMultiplied(C), "4");
            Assert.True(C.CanBeMultiplied(A), "4");
            // 5
            Assert.False(C.CanBePowerd(), "5");
            // 6
            Assert.True(!Ct.CanBeMultiplied(A), "6");
            // 7
            Assert.False(B.CanBeMultiplied(A), "7");
            // 8
            Assert.False(A.CanBeMultiplied(Bt), "8");
            // 9
            Assert.False(Matrix<int>.Identity(7).CanBeMultiplied(A), "9");
        }
    }
}
