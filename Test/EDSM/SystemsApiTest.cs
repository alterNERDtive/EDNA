// <copyright file="SystemsApiTest.cs" company="alterNERDtive">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Edsm;

namespace Test.EDSM
{
    /// <summary>
    /// Tests getting correct System data EDSM’s Systems API.
    /// </summary>
    public class SystemsApiTest
    {
        /// <summary>
        /// Tests getting correct System data from the System endpoint of EDSM’s
        /// Systems API.
        /// </summary>
        public class SystemEndpointTest
        {
            /// <summary>
            /// Tests whether the Sol system can be retrieved correctly.
            /// </summary>
            [Fact]
            public async Task SolData()
            {
                Edsm.ApiSystem sol = await Edsm.SystemsApi.FindSystem(name: "Sol");

                Assert.Equal("Sol", sol.Name);
                Assert.Equal<int>(27, sol.Id);
                Assert.Equal<ulong>(10477373803, sol.Id64);

                Edsm.Coordinates coords = sol.Coords.Value;
                Assert.Equal<double>(0, coords.X);
                Assert.Equal<double>(0, coords.Y);
                Assert.Equal<double>(0, coords.Z);

                Edsm.ApiSystem.SystemInformation systemInformation = sol.Information.Value;
                Assert.Equal("Federation", systemInformation.Allegiance);
                Assert.Equal("Democracy", systemInformation.Government);
                Assert.Equal("Mother Gaia", systemInformation.Faction);
                Assert.NotNull(systemInformation.FactionState);
                Assert.Equal<ulong>(22780919531, systemInformation.Population.Value);
                Assert.Equal("High", systemInformation.Security);
                Assert.Equal("Refinery", systemInformation.Economy);
                Assert.Equal("Service", systemInformation.SecondEconomy);
                Assert.Equal("Common", systemInformation.Reserve);

                Edsm.ApiSystem.PrimaryStarInformation starInformation = sol.PrimaryStar.Value;
                Assert.Equal("G (White-Yellow) Star", starInformation.Type);
                Assert.Equal("Sol", starInformation.Name);
                Assert.True(starInformation.IsScoopable);
            }

            /// <summary>
            /// Tests whether an invalid system name / system not in EDSM throws
            /// the proper ArgumentException.
            /// </summary>
            [Fact]
            public async Task InvalidSystemName()
            {
                await Assert.ThrowsAsync<ArgumentException>(() => Edsm.SystemsApi.FindSystem(name: "Soll"));
            }
        }

        /// <summary>
        /// Tests getting correct System data from the Systems endpoint of
        /// EDSM’s Systems API.
        /// </summary>
        public class SystemsEndpointTest
        {
            /// <summary>
            /// Tests whether a partial system search for “Beagle Po” correctly
            /// retrieves (only) the Beagle Point system.
            /// </summary>
            [Fact]
            public async Task PartialBeaglePoint()
            {
                List<Edsm.ApiSystem> systems = (await Edsm.SystemsApi.FindSystems(name: "Beagle Po")).ToList();
                Assert.Single(systems);
                Assert.Equal("Beagle Point", systems.First().Name);
            }

            /// <summary>
            /// Tests retrieving Sol and Beagle, with a random invalid system in
            /// there that the API will silently ignore.
            /// </summary>
            [Fact]
            public async Task SolAndBeaglePoint()
            {
                List<Edsm.ApiSystem> systems = (await Edsm.SystemsApi.FindSystems(names: new[] { "Sol", "Beagle Point", "Random Invalid Name" })).ToList();
                Assert.Equal<int>(2, systems.Count);
                systems.Find(x => x.Name == "Sol");
                systems.Find(x => x.Name == "Beagle Point");
            }
        }

        /// <summary>
        /// Tests getting correct star system data from the Sphere-systems
        /// endpoint of EDSM’s Systems API.
        /// </summary>
        public class SphereSystemsEndpointTest
        {
            /// <summary>
            /// Tests whether a sphere search between 9 and 10 ly around Sol
            /// correctly returns the 3 systems in that section of the sphere.
            /// </summary>
            [Fact]
            public async Task SphereAroundSol()
            {
                List<Edsm.ApiSystem> systems = (await Edsm.SystemsApi.FindSystemsSphere(name: "Sol", minRadius: 9, radius: 10)).ToList();
                Assert.Equal<int>(3, systems.Count);
                systems.Find(x => x.Name == "Duamta" && x.Distance == 9.88 && x.BodyCount == 12);
                systems.Find(x => x.Name == "Ross 154" && x.Distance == 9.69 && x.BodyCount == 9);
                systems.Find(x => x.Name == "Yin Sector CL-Y d127" && x.Distance == 9.86 && x.BodyCount == 2);
            }

            /// <summary>
            /// Tests whether a sphere between of 30 ly around (1000,1000,1000)
            /// correctly returns the star systems in that sphere.
            /// </summary>
            [Fact]
            public async Task SphereAround1000()
            {
                List<Edsm.ApiSystem> systems =
                    (await Edsm.SystemsApi.FindSystemsSphere(new Edsm.Coordinates { X = 1000, Y = 1000, Z = 1000 }, minRadius: 20, radius: 30)).ToList();
                Assert.Single(systems);
                Assert.Equal("Praea Euq ZK-M d8-3", systems.First().Name);

                systems = (await Edsm.SystemsApi.FindSystemsSphere(new Edsm.Coordinates { X = 1000, Y = 1000, Z = 1000 }, radius: 30)).ToList();
                Assert.Equal(2, systems.Count);
                systems.Find(x => x.Name == "Praea Euq ZK-M d8-3");
                systems.Find(x => x.Name == "Praea Euq AH-X c17-0");
            }
        }

        /// <summary>
        /// Test getting correct star system data from the Cube-systems endpoint
        /// of EDSM’s Systems API.
        /// </summary>
        public class CubeSystemsEndpointTest
        {
            /// <summary>
            /// Tests whether a cube search of boundary size 10 ly around Sol
            /// correctly returns the 3 systems is that cube.
            /// </summary>
            [Fact]
            public async Task CubeAroundSol()
            {
                List<Edsm.ApiSystem> systems = (await Edsm.SystemsApi.FindSystemsCube(name: "Sol", boundarySize: 10)).ToList();
                Assert.Equal<int>(3, systems.Count);
                systems.Find(x => x.Name == "Barnard's Star" && x.Distance == 5.95 && x.BodyCount == 16);
                systems.Find(x => x.Name == "Sol" && x.Distance == 0 && x.BodyCount == 40);
                systems.Find(x => x.Name == "Alpha Centauri" && x.Distance == 4.38 && x.BodyCount == 9);
            }

            /// <summary>
            /// Tests whether a cube of boundary size 42 centered around
            /// (1000,1000,1000) correctly returns the star systems in that cube.
            /// </summary>
            [Fact]
            public async Task CubeAround1000()
            {
                List<Edsm.ApiSystem> systems =
                    (await Edsm.SystemsApi.FindSystemsCube(new Edsm.Coordinates { X = 1000, Y = 1000, Z = 1000 }, boundarySize: 42)).ToList();
                Assert.Equal(2, systems.Count);
                systems.Find(x => x.Name == "Praea Euq ZK-M d8-3");
                systems.Find(x => x.Name == "Praea Euq AH-X c17-0");
            }
        }
    }
}
