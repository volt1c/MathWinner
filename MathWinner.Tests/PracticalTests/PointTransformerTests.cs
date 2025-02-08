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
        public void TestTransformations_1()
        {
            PointTransformer point = new PointTransformer(0, -6);

            point.ReflectOverLineXEquals(9);
            point.Rotate(47);
            point.ReflectOverOrigin();
            point.Translate(1, MathF.Exp(MathF.PI));

            double floorX = Math.Floor(point.Position.X * 100) / 100;
            Assert.Equal(-15.66, floorX);
        }

        [Fact]
        public void TestTransformations_2()
        {
            PointTransformer point = new PointTransformer(-9, -7);

            point.Rotate(296);
            point.Translate(7, 9);
            point.ReflectOverLineXEquals(0);

            double floorX = Math.Floor(point.Position.X * 100) / 100;
            Assert.Equal(3.23, floorX);
        }
    }

}
