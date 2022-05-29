// <copyright file="LocationTest.cs" company="alterNERDtive">
// Copyright 2021–2022 alterNERDtive.
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
        [InlineData("Oevasy SG-Y d0")]
        [InlineData("Oevasy AB-C d1-2")]
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
        [InlineData("Ysaveo YG-S d0")]
        [InlineData("Oevasy SG-Y")]
        [InlineData("Oevasy SG-Y d")]
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
        /// <param name="testSystem">The expected system/coordinates.</param>
        [Theory]
        [MemberData(nameof(Systems))]
        public async Task ProcGen_Coordinates(StarSystem testSystem)
        {
            StarSystem system = await EdtsApi.FindSystem(name: testSystem.Name);
            Assert.Equal(expected: testSystem.Position.X, actual: system.Position.X);
            Assert.Equal(expected: testSystem.Position.Y, actual: system.Position.Y);
            Assert.Equal(expected: testSystem.Position.Z, actual: system.Position.Z);
            Assert.Equal(expected: testSystem.Uncertainty, actual: system.Uncertainty);
        }

        /// <summary>
        /// Gets a list of systems and known coordinates for the ProcGen_Coordinates test.
        /// </summary>
        public static IEnumerable<object[]> Systems =>
            new List<object[]>
            {
                new object[] { new StarSystem { Name = "Oevasy SG-Y D0", Position = new StarSystem.Coordinates { X = -1465, Y = 15, Z = 65615 }, Uncertainty = 40 } },
                new object[] { new StarSystem { Name = "Lysoorb AA-A b0", Position = new StarSystem.Coordinates { X = -55, Y = -15, Z = 6625 }, Uncertainty = 10 } },
                new object[] { new StarSystem { Name = "Dryau Aowsy AB-C D1-234", Position = new StarSystem.Coordinates { X = 775, Y = 1615, Z = 18255 }, Uncertainty = 40 } },
                new object[] { new StarSystem { Name = "Dryau Aowsy DC-B A4-321", Position = new StarSystem.Coordinates { X = 1170, Y = 400, Z = 18180 }, Uncertainty = 5 } },
            };
    }
}
