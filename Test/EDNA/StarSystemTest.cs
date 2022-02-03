// <copyright file="StarSystemTest.cs" company="alterNERDtive">
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

using alterNERDtive.Edna;
using Xunit;

namespace Test.Edna
{
    /// <summary>
    /// Tests all things Starsystem.
    /// </summary>
    public class StarSystemTest
    {
        /// <summary>
        /// Tests if Sol really rests at 0,0,0.
        /// </summary>
        [Fact]
        public void SolAtZero()
        {
            Distance distance;

            distance = StarSystem.Find("Sol").DistanceTo(new Location(0, 0, 0));
            Assert.Equal(expected: 0, actual: distance.Value);
            Assert.Equal(expected: 0, actual: distance.Precision);

            distance = StarSystem.Find("Sol").DistanceTo(new Location(0, 0, 0, 0));
            Assert.Equal(expected: 0, actual: distance.Value);
            Assert.Equal(expected: 0, actual: distance.Precision);
        }
    }
}
