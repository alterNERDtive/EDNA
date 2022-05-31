// <copyright file="EdtsApi.cs" company="alterNERDtive">
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
        /// Gets or sets the system’s name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the system’s coordinates.
        /// </summary>
        public Coordinates Position { get; set; }

        /// <summary>
        /// Gets or sets the system’s positional uncertainty in light years.
        /// </summary>
        public decimal Uncertainty { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Really? For this?")]
        public struct Coordinates
        {
            public decimal X { get; set; }

            public decimal Y { get; set; }

            public decimal Z { get; set; }
        }
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
            RestResponse<ApiResult> response = await ApiClient.ExecuteAsync<ApiResult>(new RestRequest($"system_position/{name}"));
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new ArgumentException(message: $"“{name}” is not a valid proc gen system name.", paramName: nameof(name));
            }

            return response.Data.Result!.Value;
        }

        private struct ApiResult
        {
            public StarSystem? Result { get; set; }
        }
    }
}