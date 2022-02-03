// <copyright file="DistanceTest.cs" company="alterNERDtive">
// Copyright 2021 alterNERDtive.
//
// This file is part of EDNA.
//
// EDNA is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// EDNA is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with EDNA.  If not, see &lt;https://www.gnu.org/licenses/&gt;.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using alterNERDtive.Edna;
using Xunit;

namespace Test.Edna
{
    /// <summary>
    /// Tests all thisg Distance.
    /// </summary>
    public class DistanceTest
    {
        /// <summary>
        /// Tests if the calculations are correct for some known distances.
        /// </summary>
        [Fact]
        public void SomeExampleDistances()
        {
            Distance distance;

            distance = new Location(0, 0, 0).DistanceTo(new Location(10, 10, 10));
            Assert.Equal(expected: Math.Round(17.3205080756888, 4), actual: Math.Round(distance.Value, 4));
            Assert.Equal(expected: 0, actual: distance.Precision);
        }

        /// <summary>
        /// Tests if the calculations are correct for some known distances  with
        /// precision >0.
        /// </summary>
        [Fact]
        public void SomeExampleDistancesWithImprecision()
        {
            throw new NotImplementedException();
        }
    }
}
