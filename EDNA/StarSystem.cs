// <copyright file="StarSystem.cs" company="alterNERDtive">
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
using System.Threading.Tasks;

namespace alterNERDtive.Edna
{
    /// <summary>
    /// A star system in the galaxy of Elite Dangerous.
    /// </summary>
    public class StarSystem : Locatable
    {
        private static readonly Dictionary<string, StarSystem> NameCache = new Dictionary<string, StarSystem>();
        private static readonly Dictionary<ulong, StarSystem> Id64Cache = new Dictionary<ulong, StarSystem>();
        private Coordinates? coordinates = null;

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
        public new Coordinates Coordinates
        {
            get
            {
                return this.GetCoordinatesAsync().Result;
            }
        }

        /// <summary>
        /// Gets the system’s stations.
        /// </summary>
        public Station[] Stations { get => throw new NotImplementedException(); }

        /// <summary>
        /// Gets the systems Location in the galaxy, asynchronously. If you want
        /// blocking access, use the Property instead.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<Coordinates> GetCoordinatesAsync()
        {
            if (this.coordinates == null)
            {
                // query EDSM
                // query spansh
                // query EDTS
                this.coordinates = new Coordinates(await Edts.EdtsApi.FindSystem(this.Name));
            }

            return this.coordinates.Value;
        }

        /// <summary>
        /// Finds a star system by name.
        ///
        /// System names are not necessarily unique.
        /// </summary>
        /// <param name="name">The system’s name.</param>
        /// <returns>The matching system.</returns>
        public static StarSystem Find(string name)
        {
            if (NameCache.ContainsKey(name))
            {
                return NameCache[name];
            }
            else
            {
                StarSystem system = new StarSystem(name);
                NameCache[name] = system;
                return system;
            }
        }

        /// <summary>
        /// Finds a star system by id64.
        /// </summary>
        /// <param name="id64">The system’s id64.</param>
        /// <returns>The matching system.</returns>
        public static StarSystem Find(ulong id64)
        {
            if (Id64Cache.ContainsKey(id64))
            {
                return Id64Cache[id64];
            }
            else
            {
                StarSystem system = new StarSystem(id64);
                Id64Cache[id64] = system;
                return system;
            }
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
