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

#pragma warning disable SA1615 // Element return value should be documented
#pragma warning disable SA1201 // Elements should appear in the correct order

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        [InlineData("Oevasy AB-C D0-1")]
        public async Task ProcGen_Valid(string name)
        {
            StarSystem system = await EdtsApi.FindSystem(name: name);
            Assert.Equal(expected: name, actual: system.Name);
        }

        /// <summary>
        /// Pulls data from the EDTS API for known-bad systems. This should
        /// throw an ArgumentException.
        /// </summary>
        /// <param name="name">The system name.</param>
        [Theory]
        [InlineData("Ysaveo YG-S D0")]
        [InlineData("Oevasy SG-Y")]
        [InlineData("Oevasy SG-Y D")]
        [InlineData("Oevasy SG-Y 0")]
        public async Task ProcGen_Invalid(string name)
        {
            ArgumentException e = await Assert.ThrowsAsync<ArgumentException>(() => EdtsApi.FindSystem(name: name));
            Assert.Equal(expected: "name", actual: e.ParamName);
        }

        /// <summary>
        /// Pulls some systems from the EDTS API and checks if they still have
        /// the previously known coordinates.
        /// </summary>
        /// <param name="name">The system name.</param>
        /// <param name="coords">The expected coordinates.</param>
        [Theory]
        [MemberData(nameof(Systems))]
        public async Task ProcGen_Coordinates(string name, Location coords)
        {
            StarSystem system = await EdtsApi.FindSystem(name: name);
            Assert.Equal(expected: coords.X, actual: system.Coordinates.X);
            Assert.Equal(expected: coords.Y, actual: system.Coordinates.Y);
            Assert.Equal(expected: coords.Z, actual: system.Coordinates.Z);
            Assert.Equal(expected: coords.Precision, actual: system.Coordinates.Precision);
        }

        /// <summary>
        /// Gets a list of systems and known coordinates for the ProcGen_Coordinates test.
        /// </summary>
        public static IEnumerable<object[]> Systems =>
            new List<object[]>
            {
                new object[] { "Oevasy SG-Y D0", new Location(x: -1465, y: 15, z: 65615, precision: 40) },
                new object[] { "Lysoorb AA-A b0", new Location(x: -55, y: -15, z: 6625, precision: 10) },
                new object[] { "Dryau Aowsy AB-C D1-234", new Location(x: 775, y: 1615, z: 18255, precision: 40) },
                new object[] { "Dryau Aowsy DC-B A4-321", new Location(x: 1170, y: 400, z: 18180, precision: 5) },
            };
    }
}
