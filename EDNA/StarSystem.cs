// <copyright file="StarSystem.cs" company="alterNERDtive">
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

namespace alterNERDtive.Edna
{
    /// <summary>
    /// A star system in the galaxy of Elite Dangerous.
    /// </summary>
    public class StarSystem : Locatable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StarSystem"/> class.
        /// </summary>
        /// <param name="name">The system’s name.</param>
        private StarSystem(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StarSystem"/> class.
        /// </summary>
        /// <param name="id64">The system’s id64.</param>
        private StarSystem(ulong id64)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the system’s name. Mostly but not necessarily unique.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the systems’s id64, which _should_ be unique.
        /// </summary>
        public ulong Id64 { get; private set; }

        /// <summary>
        /// Gets the system’s Location in the galaxy.
        /// </summary>
        public new Location Coordinates { get => throw new NotImplementedException(); }

        /// <summary>
        /// Finds a star system by name.
        ///
        /// System names are not necessarily unique.
        /// </summary>
        /// <param name="name">The system’s name.</param>
        /// <returns>The matching system.</returns>
        public static StarSystem Find(string name)
        {
            return new StarSystem(name); // FIXXME: singleton, caching, …
        }

        /// <summary>
        /// Finds a star system by id64.
        /// </summary>
        /// <param name="id64">The system’s id64</param>
        /// <returns>The matching system.</returns>
        public static StarSystem Find(ulong id64)
        {
            return new StarSystem(id64); // FIXXME: singleton, caching, …
        }

        /// <summary>
        /// Finds outdated stations in the system.
        /// </summary>
        /// <param name="minimumAge">The minimum age for data to be considered outdated.</param>
        /// <param name="count">The maximum count of stations to list.</param>
        /// <returns>A list of outdated stations.</returns>
        public List<Station> FindOutdatedStations(TimeSpan minimumAge, int count)
        {
            throw new NotImplementedException();
        }
    }
}
