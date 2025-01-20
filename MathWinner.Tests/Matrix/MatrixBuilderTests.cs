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
        public void ShouldBuildSimpleMatrix()
        {
            var builder = new MatrixBuilder<int>();

            builder = builder.SetM(3)
                .AddRow(1, 2, 3)
                .AddRow(3, 4, 5)
                .AddRow(6, 7, 9);

           var matrix = builder.Build();

            Assert.NotNull(matrix);

            Assert.Equal(1, matrix.Get(0, 0));
            Assert.Equal(2, matrix.Get(0, 1));
            Assert.Equal(3, matrix.Get(0, 2));
            Assert.Equal(4, matrix.Get(1, 0));
            Assert.Equal(5, matrix.Get(1, 1));
            Assert.Equal(6, matrix.Get(1, 2));
            Assert.Equal(7, matrix.Get(2, 0));
            Assert.Equal(8, matrix.Get(2, 1));
            Assert.Equal(9, matrix.Get(2, 2));
        }
    }
}
