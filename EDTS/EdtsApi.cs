﻿// <copyright file="EdtsApi.cs" company="alterNERDtive">
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
using System.Net.Http;
using System.Threading.Tasks;

using RestSharp;

namespace alterNERDtive.Edna.Edts
{
    /// <summary>
    /// A star system in the galaxy of Elite Dangerous. Or rather, what EDTS
    /// knows about a star system.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Either this or wrong class/struct order 🤷")]
    public struct StarSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StarSystem"/> struct.
        /// </summary>
        /// <param name="name">The system’s name.</param>
        /// <param name="coordinates">The system’s coordinates.</param>
        public StarSystem(string name, Location coordinates)
        {
            (this.Name, this.Coordinates) = (name, coordinates);
        }

        /// <summary>
        /// Gets the system’s name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the system’s coordinates.
        /// </summary>
        public Location Coordinates { get; }
    }

    /// <summary>
    /// A location in the galaxy, represented by coordinates and a value for
    /// precision (all in ly).
    /// </summary>
    public struct Location
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="precision">The available precision.</param>
        public Location(int x, int y, int z, int precision)
        {
            (this.X, this.Y, this.Z, this.Precision) = (x, y, z, precision);
        }

        /// <summary>
        /// Gets the x coordinate.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the y coordinate.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Gets the z coordinate.
        /// </summary>
        public int Z { get; }

        /// <summary>
        /// Gets the precision to which the location can be calculated.
        /// </summary>
        public int Precision { get; private set; }
    }

    /// <summary>
    /// The EdtsApi class is used to query the publicly hosted EDTS API at
    /// http://edts.thargoid.space for data on unknown procedurally generated
    /// systems.
    /// </summary>
    public class EdtsApi
    {
        private static readonly string ApiUrl = "http://edts.thargoid.space/api/v1";
        private static readonly RestClient ApiClient = new RestClient(ApiUrl);

        /// <summary>
        /// Pulls system data for a given procedurally generated system name.
        /// </summary>
        /// <param name="name">A procedurally generated system name.</param>
        /// <returns>The system with calculated coordinates.</returns>
        public static async Task<StarSystem> FindSystem(string name)
        {
            try
            {
                ApiResult response = await ApiClient.GetAsync<ApiResult>(new RestRequest($"system_position/{name}"));

                ApiResult.ApiSystem result = response.Result!.Value;

                return new StarSystem(
                    name: name,
                    coordinates: new Location(
                        x: (int)result.Position.X,
                        y: (int)result.Position.Y,
                        z: (int)result.Position.Z,
                        precision: (int)result.Uncertainty));
            }
            catch (HttpRequestException e)
            {
                // EDTS API gives a 400 status code (and an empty result) if the
                // system name is not valid.
                if (e.Message.Equals("Request failed with status code BadRequest"))
                {
                    throw new ArgumentException(message: $"“{name}” is not a valid proc gen system name.", paramName: nameof(name));
                }
                else
                {
                    throw;
                }
            }
        }

        private struct ApiResult
        {
            public ApiSystem? Result { get; set; }

            public struct ApiSystem
            {
                public string Name { get; set; }

                public ApiLocation Position { get; set; }

                public decimal Uncertainty { get; set; }

                public struct ApiLocation
                {
                    public decimal X { get; set; }

                    public decimal Y { get; set; }

                    public decimal Z { get; set; }
                }
            }
        }
    }
}