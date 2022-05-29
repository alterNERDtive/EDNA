// <copyright file="CommanderTest.cs" company="alterNERDtive">
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

using System;

using alterNERDtive.Edna;
using Xunit;

namespace Test.Edna
{
    /// <summary>
    /// Tests getting CMDR data correctly from the EDSM API.
    /// </summary>
    public class CommanderTest
    {
        /// <summary>
        /// Tests getting data correctly for a known CMDR.
        /// </summary>
        [Fact]
        public void KnownCmdrTest()
        {
            Assert.Throws<NotImplementedException>(() => Commander.Find("IHaveFuelYouDont", "sometext"));
            Commander cmdr = Commander.Find("IHaveFuelYouDont");
            Assert.Equal(expected: new Coordinates(x: 25.40625, y: -31.0625, z: 41.625), actual: cmdr.Coordinates);
            Assert.Equal(expected: "https://www.edsm.net/en/user/profile/id/86423/cmdr/IHaveFuelYouDont", actual: cmdr.EdsmProfileUrl);
            Assert.IsType<DateTime>(cmdr.LastActiveAt);
            Assert.Equal(expected: "IHaveFuelYouDont", actual: cmdr.Name);
            Assert.Equal(expected: "Dromi", actual: cmdr.StarSystem!.Name);
        }

        /// <summary>
        /// Tests correctly getting a CmdrHiddenException for hidden CMDRs.
        /// </summary>
        [Fact]
        public void HiddenCmdrTest()
        {
            Assert.Throws<CommanderHiddenException>(() => Commander.Find("Hojothefool"));
        }

        /// <summary>
        /// Tests correctly getting a CmdrNotFoundException for nonexistent CMDRs.
        /// </summary>
        [Fact]
        public void NonexistentCmdrTest()
        {
            Assert.Throws<CommanderNotFoundException>(() => Commander.Find("IHaveFuelYouDoButDontExistLOL"));
        }
    }
}
