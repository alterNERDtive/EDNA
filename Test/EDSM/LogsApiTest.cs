// <copyright file="LogsApiTest.cs" company="alterNERDtive">
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using alterNERDtive.Edna.Edsm;
using Xunit;

namespace Test.Edsm
{
    /// <summary>
    /// Tests correctly getting data from EDSM’s Log API.
    /// </summary>
    public class LogsApiTest
    {
        /// <summary>
        /// Tests getting data correctly for a known CMDR.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task KnownCmdrTestAsync()
        {
            ApiCmdr cmdr = await LogsApi.FindCmdr("IHaveFuelYouDont");
            Assert.Equal(expected: 100, actual: cmdr.MsgNum);
            Assert.Equal(expected: "OK", actual: cmdr.Msg);
            Assert.Equal(expected: "Dromi", actual: cmdr.System);
            Assert.Equal(expected: false, actual: cmdr.FirstDiscover);
            _ = DateTime.Parse(cmdr.Date);
            Assert.Equal(expected: 38324688UL, actual: cmdr.SystemId);
            Assert.Equal(expected: 1213084977515UL, actual: cmdr.SystemId64);
            Assert.Equal(expected: new Coordinates { X = 25.40625, Y = -31.0625, Z = 41.625 }, actual: cmdr.Coordinates);
            Assert.IsType<bool>(cmdr.IsDocked);
            Assert.IsType<int>(cmdr.ShipId);
            Assert.Equal(expected: "Anaconda", actual: cmdr.ShipType);
            Assert.Null(cmdr.ShipFuel);
            _ = DateTime.Parse(cmdr.DateLastActivity);
            Assert.Equal(expected: "https://www.edsm.net/en/user/profile/id/86423/cmdr/IHaveFuelYouDont", actual: cmdr.Url);
        }

        /// <summary>
        /// Tests correctly getting an ArgumentException for wrong API key.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task WrongApiKeyTestAsync()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogsApi.FindCmdr("IHaveFuelYouDont", "sometext"));
        }

        /// <summary>
        /// Tests correctly getting an AccessViolationException for hidden CMDRs.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HiddenCmdrTestAsync()
        {
            await Assert.ThrowsAsync<AccessViolationException>(() => LogsApi.FindCmdr("Hojothefool"));
        }

        /// <summary>
        /// Tests correctly getting an Argumentexception for nonexistent CMDRs.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task NonexistentCmdrTestAsync()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogsApi.FindCmdr("IHaveFuelYouDoButDontExistLOL"));
        }
    }
}
