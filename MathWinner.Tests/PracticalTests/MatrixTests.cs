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
            var A = new MatrixBuilder<int>(7, 8).Matrix;
            var B = new MatrixBuilder<int>(9, 9).Matrix;
            var C = new MatrixBuilder<int>(7, 8).Matrix;

            var At = A.Transposition();
            var Bt = B.Transposition();
            var Ct = C.Transposition();

            // 1
            Assert.False(A.CanBeMultiplied(Bt), "1");
            // 2
            var D_n8 = new MatrixBuilder<int>(9, 8).Matrix;
            Assert.True(D_n8.CanBeMultiplied(At), "2");
            // 3
            Assert.False(C.CanBePowerd(), "3");
            // 4
            Assert.False(B.CanBeMultiplied(A), "4");
            // 5
            Assert.True(B.CanBePowerd(), "5");
            // 6
            var D_n9 = new MatrixBuilder<int>(9, 9).Matrix;
            Assert.True(D_n9.CanBeSubtracted(B), "6");
            // 7
            Assert.True(C.CanBeAdded(A), "7");
            // 8
            Assert.True(Ct.CanBeMultiplied(A), "8");
            // 9
            var D_n7 = new MatrixBuilder<int>(9, 7).Matrix;
            Assert.False(C.CanBeMultiplied(D_n7), "9");
        }

        [Fact]
        public void TestMatrixOperations_1()
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

        [Fact]
        public void TestMatrixOperations_2()
        {
            var A = new MatrixBuilder<int>(4, 2, false)
                .AddRow(-6, -5)
                .AddRow(3, 3)
                .AddRow(2, 5)
                .AddRow(-6, -2)
                .Build();

            var x = 0;
            var B = new MatrixBuilder<int>(3, 3, false)
                .AddRow(7, 5, 1)
                .AddRow(7, 9, 3)
                .AddRow(x, 3, 1)
                .Build();

            Assert.Equal(30, B.Determinant);

            var C = new MatrixBuilder<int>(4, 2, false)
                .AddRow(8, 0)
                .AddRow(1, 3)
                .AddRow(7, 5)
                .AddRow(7, 7)
                .Build();

            Assert.False(C.CanBeMultiplied(A));

            var Ct = C.Transposition();
            Console.WriteLine();
        }
    }
}
