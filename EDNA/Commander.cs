// <copyright file="Commander.cs" company="alterNERDtive">
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
    /// A CMDR in the galaxy of Elite Dangerous.
    /// </summary>
    public class Commander : Locatable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Commander"/> class.
        /// </summary>
        /// <param name="name">The CMDR’s name.</param>
        public Commander(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the CMDR’s name.
        /// </summary>
        public string Name { get; }

        public string EdsmProfileUrl { get; private set; }

        public DateTime LastActiveAt { get; private set; }

        public StarSystem StarSystem { get; private set; }

        public new Coordinates Coordinates { get; private set; }
    }
}
