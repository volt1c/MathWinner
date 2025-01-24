using MathWinner.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MathWinner.Tests.PracticalTests
{
    public class PointTransformerTests
    {
        [Fact]
        public void TestTransformations()
        {
            PointTransformer point = new PointTransformer(0, -6);

            point.ReflectOverLineXEquals(9);
            point.Rotate(47);
            point.ReflectOverOrigin();
            point.Translate(1, MathF.Exp(MathF.PI));

            double cutX = Math.Truncate(point.Position.X * 100) / 100;
            Assert.Equal(-15.66, cutX);
        }
    }

}
