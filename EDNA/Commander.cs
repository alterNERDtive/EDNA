// <copyright file="Commander.cs" company="alterNERDtive">
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

namespace alterNERDtive.Edna
{
    /// <summary>
    /// A CMDR in the galaxy of Elite Dangerous.
    /// </summary>
    public class Commander : Locatable
    {
        /// Initializes a new instance of the <see cref="Commander"/> class.
        /// </summary>
        /// <param name="name">The CMDR’s name.</param>
        /// <param name="apiKey">The CMDR’s EDSM API key.</param>
        private Commander(string name, string? edsmProfileUrl, DateTime? lastActiveAt, StarSystem? starsystem, Coordinates? coordinates)
            => (this.Name, this.EdsmProfileUrl, this.LastActiveAt, this.StarSystem, this.Coordinates) = (name, edsmProfileUrl, lastActiveAt, starsystem, coordinates);

        /// <summary>
        /// Gets the CMDR’s name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the CMDR’s EDSM profile URL.
        /// </summary>
        public string? EdsmProfileUrl { get; }

        /// <summary>
        /// Gets the CMDR’s date of last activity.
        /// </summary>
        public DateTime? LastActiveAt { get; }

        /// <summary>
        /// Gets the CMDR’s current star system.
        /// </summary>
        public StarSystem? StarSystem { get; }

        /// <summary>
        /// Gets the CMDR’s current coordinates.
        /// </summary>
        public new Coordinates? Coordinates { get; }

        /// <summary>
        /// Finds a CMDR by name. Optionally takes an EDSM API key to access a
        /// private profile.
        /// </summary>
        /// <param name="name">The CMDR’s name.</param>
        /// <param name="apiKey">The CMDR’s EDSM API key.</param>
        /// <returns>The CMDR.</returns>
        public static Commander Find(string name, string? apiKey = null)
        {
            return FindAsync(name, apiKey).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static async Task<Commander> FindAsync(string name, string? apiKey = null)
        {
            if (apiKey != null)
            {
                throw new NotImplementedException();
            }

            try
            {
                Edsm.ApiCmdr cmdr = await Edsm.LogsApi.FindCmdr(name);

                return new Commander(name, cmdr.Url, DateTime.Parse(cmdr.DateLastActivity), StarSystem.Find(cmdr.SystemId64!.Value), new Coordinates(cmdr.Coordinates!.Value));
            }
            catch (ArgumentException e)
            {
                throw new CommanderNotFoundException(e.Message, e);
            }
            catch (AccessViolationException e)
            {
                throw new CommanderHiddenException(e.Message, e);
            }
        }
    }
}
