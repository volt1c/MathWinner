using MathWinner.Library.Models;

namespace MathWinner.Tests.Matrix
{
    public class MatrixTests
    {
        [Fact]
        public void ShouldCreateIdentityMatrix()
        {
            var identityMatrix = Matrix<int>.Identity(4);

            var expected = new int[][]
            {
                new [] { 1, 0, 0, 0 },
                new [] { 0, 1, 0, 0 },
                new [] { 0, 0, 1, 0 },
                new [] { 0, 0, 0, 1 },
            };

            for (int n = 0; n < expected.Length; n++)
            {
                for (int m = 0; m < expected[n].Length; m++)
                {
                    Assert.Equal(expected[n][m], identityMatrix.Get(n, m));
                }
            }
        }


    }
}