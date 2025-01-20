using MathWinner.Library.Models;

namespace MathWinner.Tests.Matrix
{
    public class MatrixTests
    {
        [Fact]
        public void Constructor_ShouldInitializeMatrix()
        {
            var input = new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            };

            var matrix = new Matrix<int>(input);

            Assert.Equal(2, matrix.N);
            Assert.Equal(2, matrix.M);
        }

        [Fact]
        public void IsSquare_ShouldReturnTrue_ForSquareMatrix()
        {
            var input = new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            };
            var matrix = new Matrix<int>(input);

            var result = matrix.IsSquare();

            Assert.True(result);
        }

        [Fact]
        public void IsSquare_ShouldReturnFalse_ForNonSquareMatrix()
        {
            var input = new int[][]
            {
                new[] { 1, 2, 3 },
                new[] { 4, 5, 6 }
            };
            var matrix = new Matrix<int>(input);

            var result = matrix.IsSquare();

            Assert.False(result);
        }

        [Fact]
        public void Transposition_ShouldReturnTransposedMatrix()
        {
            var input = new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            };
            var expected = new int[][]
            {
                new[] { 1, 3 },
                new[] { 2, 4 }
            };
            var matrix = new Matrix<int>(input);

            var transposedMatrix = matrix.Transposition();

            Assert.Equal(expected, transposedMatrix.ToArray());
        }

        [Fact]
        public void Get_ShouldReturnCorrectElement()
        {
            var input = new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            };
            var matrix = new Matrix<int>(input);

            var element = matrix.Get(1, 1);

            Assert.Equal(4, element);
        }

        [Fact]
        public void CanBeMultiplied_ShouldReturnTrue_WhenDimensionsAreCompatible()
        {
            var matrix1 = new Matrix<int>(new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            });
            var matrix2 = new Matrix<int>(new int[][]
            {
                new[] { 5, 6, 7 },
                new[] { 8, 9, 10 }
            });

            var result = matrix1.CanBeMultiplied(matrix2);

            Assert.True(result);
        }

        [Fact]
        public void CanBeMultiplied_ShouldReturnFalse_WhenDimensionsAreNotCompatible()
        {
            var matrix1 = new Matrix<int>(new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            });
            var matrix2 = new Matrix<int>(new int[][]
            {
                new[] { 5, 6 },
                new[] { 7, 8 },
                new[] { 9, 10 }
            });

            var result = matrix1.CanBeMultiplied(matrix2);

            Assert.False(result);
        }

        [Fact]
        public void Identity_ShouldReturnIdentityMatrix()
        {
            var identityMatrix = Matrix<int>.Identity(3);

            var expected = new int[][]
            {
                new[] { 1, 0, 0 },
                new[] { 0, 1, 0 },
                new[] { 0, 0, 1 }
            };

            Assert.Equal(expected, identityMatrix.ToArray());
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForEqualMatrices()
        {
            var matrix1 = new Matrix<int>(new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            });
            var matrix2 = new Matrix<int>(new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            });

            var result = matrix1.Equals(matrix2);

            Assert.True(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForUnequalMatrices()
        {
            var matrix1 = new Matrix<int>(new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            });
            var matrix2 = new Matrix<int>(new int[][]
            {
                new[] { 5, 6 },
                new[] { 7, 8 }
            });

            var result = matrix1.Equals(matrix2);

            Assert.False(result);
        }

        [Fact]
        public void ToArray_ShouldReturnPrimitiveArrayRepresentation()
        {
            var input = new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 }
            };
            var matrix = new Matrix<int>(input);

            var result = matrix.ToArray();

            Assert.Equal(input, result);
        }
    }
}
