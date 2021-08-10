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
using alterNERDtive.Edna.Edts;
using Xunit;

namespace Test.Edts
{
    /// <summary>
    /// Tests getting the correct location data from the EDTS API.
    /// </summary>
    public class LocationTest
    {
        /// <summary>
        /// Pulls data from the EDTS API for known-good systems. This should not
        /// throw an exception.
        /// </summary>
        /// <param name="name">The system name.</param>
        [Theory]
        [InlineData("Oevasy SG-Y D0")]
        public void ProcGen_Valid(string name)
        {
            StarSystem system = EdtsApi.FindSystem(name: name);
            Assert.Equal(expected: "Oevasy SG-Y D0", actual: system.Name);
        }

        /// <summary>
        /// Pulls data from the EDTS API for known-bad systems. This should
        /// throw an ArgumentException.
        /// </summary>
        /// <param name="name">The system name.</param>
        [Theory]
        [InlineData("Ysaveo YG-S D0")]
        public void ProcGen_Invalid(string name)
        {
            ArgumentException e = Assert.Throws<ArgumentException>(() => EdtsApi.FindSystem(name: name));
            Assert.Equal(expected: "name", actual: e.ParamName);
        }
    }
}
