// <copyright file="Stations.cs" company="alterNERDtive">
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

#nullable enable

using System;
using System.Collections.Generic;

namespace alterNERDtive.Edna
{
    /// <summary>
    /// Provides access to functions that deal with several stations at once.
    /// </summary>
    public class Stations
    {
        /// <summary>
        /// Finds outdated stations. A station is outdated if it has not had its
        /// data updated in a given time span.
        /// </summary>
        /// <param name="minimumAge">The minimum age to be considered outdated.</param>
        /// <param name="count">The maximum count of outdated stations to return.</param>
        /// <returns>A list of outdated stations, sorted by age, descending.</returns>
        public static List<Station> FindOutdatedStations(TimeSpan minimumAge, int count)
        {
            throw new NotImplementedException();
        }
    }
}
