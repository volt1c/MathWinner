using MathWinner.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathWinner.Tests.Matrix
{
    public class MatrixBuilderTests
    {
        [Fact]
        public void Constructor_ShouldInitializeMinimalMatrix()
        {
            var builder = new MatrixBuilder<int>();

            Assert.NotNull(builder);

            var matrix = builder.Build();

            Assert.Equal(1, matrix.N);
            Assert.Equal(1, matrix.M);
        }

        [Fact]
        public void Constructor_ShouldInitializeWithDimensions()
        {
            var builder = new MatrixBuilder<int>(2, 3);

            builder.AddRow(1, 2, 3);
            builder.AddRow(4, 5, 6);

            var matrix = builder.Matrix;
            Assert.Equal(2, matrix.N);
            Assert.Equal(3, matrix.M);

            Assert.Equal(1, matrix.Get(0, 0));
        }

        [Fact]
        public void SetN_ShouldUpdateRowCount()
        {
            var builder = new MatrixBuilder<int>();

            builder.SetN(4);
            var matrix = builder.Matrix;

            Assert.Equal(4, matrix.N);
        }

        [Fact]
        public void SetM_ShouldUpdateColumnCount()
        {
            var builder = new MatrixBuilder<int>();

            builder.SetM(5);
            var matrix = builder.Matrix;

            Assert.Equal(5, matrix.M);
        }

        [Fact]
        public void AddRow_ShouldAddRowToMatrix_WithArrayInput()
        {
            var builder = new MatrixBuilder<int>(2, 2);

            builder.AddRow(1, 2);
            builder.AddRow(3, 4);
            var matrix = builder.Matrix;

            var expected = new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            };
            Assert.Equal(expected, matrix.ToArray());
        }

        [Fact]
        public void AddRow_ShouldAddRowToMatrix_WithListInput()
        {
            var builder = new MatrixBuilder<int>(2, 2);

            builder.AddRow(new List<int> { 1, 2 });
            builder.AddRow(new List<int> { 3, 4 });
            var matrix = builder.Matrix;

            var expected = new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            };
            Assert.Equal(expected, matrix.ToArray());
        }

        [Fact]
        public void Set_ShouldUpdateElementAtSpecifiedPosition()
        {
            var builder = new MatrixBuilder<int>(2, 2);

            builder.AddRow(1, 2);
            builder.AddRow(3, 4);
            builder.Set(1, 1, 69);
            var matrix = builder.Matrix;

            Assert.Equal(69, matrix.Get(1, 1));
        }

        [Fact]
        public void Build_ShouldReturnMatrixWithAddedRows()
        {
            var builder = new MatrixBuilder<int>(2, 2);

            builder.AddRow(1, 2);
            builder.AddRow(3, 4);
            var matrix = builder.Build();

            var expected = new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            };

            Assert.Equal(expected, matrix.ToArray());
        }

        [Fact]
        public void Build_ShouldThrowException_WhenMatrixIsIncomplete()
        {
            var builder = new MatrixBuilder<int>(2, 2);

            builder.AddRow(1, 2);

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void AddRow_ShouldThrowException_WhenRowLengthDoesNotMatchColumnCount()
        {
            var builder = new MatrixBuilder<int>(2, 3);

            Assert.Throws<ArgumentException>(() => builder.AddRow(1, 2)); // Row length is less than 3
        }

        [Fact]
        public void Set_ShouldThrowException_WhenPositionIsOutOfBounds()
        {
            var builder = new MatrixBuilder<int>(2, 2);

            Assert.Throws<IndexOutOfRangeException>(() => builder.Set(2, 2, 420)); // Out of bounds for 2x2 matrix
        }
    }
}
