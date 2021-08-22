// <copyright file="LocationTest.cs" company="alterNERDtive">
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
    /// Tests all things Location.
    /// </summary>
    public class LocationTest
    {
        /// <summary>
        /// Tests if the Location at 0,0,0 gets properly detected as at itself.
        /// </summary>
        [Fact]
        public void ZeroLocationEquals()
        {
            Assert.Equal(new Location(0, 0, 0), new Location(0, 0, 0));
            Assert.Equal(new Location(0, 0, 0), new Location(0, 0, 0, 0));
        }

        /// <summary>
        /// Tests if Locations are detected as not equal when just the precision
        /// differs.
        /// </summary>
        [Fact]
        public void NotEqualIfPrecisionDifferent()
        {
            Assert.NotEqual(new Location(0, 0, 0), new Location(0, 0, 0, 1));
            Assert.NotEqual(new Location(0, 0, 0, 0), new Location(0, 0, 0, 1));

            Assert.NotEqual(new Location(1, 2, 3), new Location(1, 2, 3, 1));
            Assert.NotEqual(new Location(1, 2, 3, 0), new Location(1, 2, 3, 1));

            Assert.NotEqual(new Location(1.1, 2.2, 3.3), new Location(1.1, 2.2, 3.3, 1));
            Assert.NotEqual(new Location(1.1, 2.2, 3.3, 0), new Location(1.1, 2.2, 3.3, 1));
        }

        /// <summary>
        /// Tests if two Locations with the same coordinates AND some
        /// SAME imprecision are correctly not considered equal.
        /// </summary>
        [Fact]
        public void NotEqualIfSameImprecision()
        {
            Assert.NotEqual(new Location(0, 0, 0, 5), new Location(0, 0, 0, 5));
            Assert.NotEqual(new Location(1, 2, 3, 5), new Location(1, 2, 3, 5));
        }
    }
}
