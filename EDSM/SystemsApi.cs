// <copyright file="SystemsApi.cs" company="alterNERDtive">
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
using System.Threading.Tasks;
using RestSharp;

namespace Edsm
{
    /// <summary>
    /// A star system in the galaxy of Elite Dangerous, as returned from EDSM’s
    /// “Systems” API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Either this or wrong class/struct order 🤷")]
    public struct ApiSystem
    {
        /// <summary>
        /// Gets or sets the distance to the reference system. Only used by the
        /// Sphere and Cube endpoints of the Systems API.
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Gets or sets the body count of the star system. Only used by the
        /// Sphere and Cube endpoints of the Systems API.
        /// </summary>
        public int? BodyCount { get; set; }

        /// <summary>
        /// Gets or sets the name of the star system.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the EDSM ID of the star system.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID64 of the star system.
        /// </summary>
        public ulong Id64 { get; set; }

        /// <summary>
        /// Gets or sets the location of the star system.
        /// </summary>
        public Coordinates? Coords { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the coordinates of the star
        /// system are “locked”.
        ///
        /// FIXXME: I _think_ that means they have been confirmed by multiple
        /// people, but since it’s undocumented I am not sure.
        /// </summary>
        public bool CoordsLocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the star system requires
        /// acquisition of a permit.
        /// </summary>
        public bool RequirePermit { get; set; }

        /// <summary>
        /// Gets or sets the name of the permit required to visit the star system.
        /// </summary>
        public string? PermitName { get; set; }

        /// <summary>
        /// Gets or sets general information about the star system.
        /// </summary>
        public SystemInformation? Information { get; set; }

        /// <summary>
        /// Gets or sets information about the primary star of the star system.
        /// </summary>
        public PrimaryStarInformation? PrimaryStar { get; set; }

        /// <summary>
        /// Information about the primary star of a star system.
        /// </summary>
        public struct PrimaryStarInformation
        {
            /// <summary>
            /// Gets or sets the star’s name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the star’s star type (full text version).
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether or not the star is able
            /// to be scooped for fuel.
            /// </summary>
            public bool IsScoopable { get; set; }
        }

        /// <summary>
        /// General Information about a star system.
        /// </summary>
        public struct SystemInformation
        {
            /// <summary>
            /// Gets or sets the star system’s allegiance.
            ///
            /// Can currently be Federation, Empire, Alliance, Independent, Thargoid.
            /// </summary>
            public string? Allegiance { get; set; }

            /// <summary>
            /// Gets or sets the star system’s government type.
            /// </summary>
            public string? Government { get; set; }

            /// <summary>
            /// Gets or sets the star system’s current controlling faction.
            /// </summary>
            public string? Faction { get; set; }

            /// <summary>
            /// Gets or sets the star system’s current controlling faction’s state.
            /// </summary>
            public string? FactionState { get; set; }

            /// <summary>
            /// Gets or sets the star system’s current population.
            /// </summary>
            public ulong? Population { get; set; }

            /// <summary>
            /// Gets or sets the star system’s current security state.
            /// </summary>
            public string? Security { get; set; }

            /// <summary>
            /// Gets or sets the star system’s primary economy.
            /// </summary>
            public string? Economy { get; set; }

            /// <summary>
            /// Gets or sets the star system’s secondary economy.
            /// </summary>
            public string? SecondEconomy { get; set; }

            /// <summary>
            /// Gets or sets the star system’s reserve level.
            /// </summary>
            public string? Reserve { get; set; }
        }
    }

    /// <summary>
    /// The SystemsApi class is used to pull information about a single or
    /// multiple star systems from EDSM’s “Systems” API.
    ///
    /// See https://www.edsm.net/en/api-v1.
    /// </summary>
    public class SystemsApi
    {
        private static readonly Uri ApiUrl = new Uri("https://www.edsm.net/en/api-v1");
        private static readonly RestClient ApiClient = new RestClient(ApiUrl)
            .AddDefaultQueryParameter("showId", "1")
            .AddDefaultQueryParameter("showCoordinates", "1")
            .AddDefaultQueryParameter("showPermit", "1")
            .AddDefaultQueryParameter("showInformation", "1")
            .AddDefaultQueryParameter("showPrimaryStar", "1");

        /// <summary>
        /// Retrieves a single star system by name.
        /// </summary>
        /// <param name="name">The system name.</param>
        /// <returns>The star system.</returns>
        public static async Task<ApiSystem> FindSystem(string name)
        {
            RestResponse<ApiSystem> response = await ApiClient.ExecuteAsync<ApiSystem>(
                new RestRequest("system").AddQueryParameter("systemName", name));
            try
            {
                CheckResponseStatus(response);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"System “{name}” not found.", nameof(name));
            }

            return response.Data;
        }

        /// <summary>
        /// Retrieves multiple star systems by partial name.
        /// </summary>
        /// <param name="name">The partial name.</param>
        /// <returns>A list of star systems beginning with the partial name.</returns>
        public static async Task<IEnumerable<ApiSystem>> FindSystems(string name)
        {
            RestResponse<IEnumerable<ApiSystem>> response = await ApiClient.ExecuteAsync<IEnumerable<ApiSystem>>(
                new RestRequest("systems").AddQueryParameter("systemName", name));
            try
            {
                CheckResponseStatus(response);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"No system found for partial name “{name}”.", nameof(name));
            }

            return response.Data;
        }

        /// <summary>
        /// Retrieves the star systems to a given set of system names.
        /// </summary>
        /// <param name="names">The star systems in question.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<IEnumerable<ApiSystem>> FindSystems(IEnumerable<string> names)
        {
            RestRequest request = new RestRequest("systems");
            foreach (string name in names)
            {
                request.AddQueryParameter("systemName[]", name);
            }

            RestResponse<IEnumerable<ApiSystem>> response = await ApiClient.ExecuteAsync<IEnumerable<ApiSystem>>(request);
            try
            {
                CheckResponseStatus(response);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"No systems found for “{string.Join(", ", names)}”.", nameof(names));
            }

            return response.Data;
        }

        /// <summary>
        /// Retrieves all star systems contained in a sphere around a given star
        /// system.
        /// </summary>
        /// <param name="name">The name of the star system.</param>
        /// <param name="minRadius">The minimum radius within which star systems are
        /// excluded from the result set.</param>
        /// <param name="radius">The radius of the search sphere.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<IEnumerable<ApiSystem>> FindSystemsSphere(string name, int minRadius = 0, int radius = 50)
        {
            int max = 100;
            if (radius > max)
            {
                throw new ArgumentException($"Radius cannot exceed {max} ly.", nameof(radius));
            }

            RestResponse<IEnumerable<ApiSystem>> response = await ApiClient.ExecuteAsync<IEnumerable<ApiSystem>>(
                new RestRequest("sphere-systems")
                .AddQueryParameter("systemName", name)
                .AddQueryParameter("minRadius", minRadius)
                .AddQueryParameter("radius", radius));
            try
            {
                CheckResponseStatus(response);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"No systems found within {radius} ly of “{name}”.");
            }

            return response.Data;
        }

        /// <summary>
        /// Retrieves all star systems contained in a sphere around given coordinates.
        /// </summary>
        /// <param name="coords">The coordinates in question.</param>
        /// <param name="minRadius">The minimum radius within which star systems
        /// are excluded from the result set.</param>
        /// <param name="radius">The radius of the search sphere.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<IEnumerable<ApiSystem>> FindSystemsSphere(Coordinates coords, int minRadius = 0, int radius = 50)
        {
            int max = 100;
            if (radius > max)
            {
                throw new ArgumentException($"Radius cannot exceed {max} ly.", nameof(radius));
            }

            RestResponse<IEnumerable<ApiSystem>> response = await ApiClient.ExecuteAsync<IEnumerable<ApiSystem>>(
                new RestRequest("sphere-systems")
                .AddQueryParameter("x", coords.X)
                .AddQueryParameter("y", coords.Y)
                .AddQueryParameter("z", coords.Z)
                .AddQueryParameter("minRadius", minRadius)
                .AddQueryParameter("radius", radius));
            try
            {
                CheckResponseStatus(response);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"No systems found within {radius} ly of {coords}.");
            }

            return response.Data!;
        }

        /// <summary>
        /// Retrivese all star systems contained in a cube centered on a given
        /// star system.
        /// </summary>
        /// <param name="name">The name of the system.</param>
        /// <param name="boundarySize">The boundary size for the cube.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<IEnumerable<ApiSystem>> FindSystemsCube(string name, int boundarySize = 100)
        {
            int max = 200;
            if (boundarySize > max)
            {
                throw new ArgumentException($"Boundary size cannot exceed {max} ly.", nameof(boundarySize));
            }

            RestResponse<IEnumerable<ApiSystem>> response = await ApiClient.ExecuteAsync<IEnumerable<ApiSystem>>(
                new RestRequest("cube-systems")
                .AddQueryParameter("systemName", name)
                .AddQueryParameter("size", boundarySize));
            try
            {
                CheckResponseStatus(response);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"No systems found in a cube of {boundarySize} ly boundary size around “{name}”.");
            }

            return response.Data!;
        }

        /// <summary>
        /// Retrieves all star systems contained in a cube centered on a given
        /// set of coordinates.
        /// </summary>
        /// <param name="coords">The coordinates in question.</param>
        /// <param name="boundarySize">The boundary size for the cube.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<IEnumerable<ApiSystem>> FindSystemsCube(Coordinates coords, int boundarySize = 100)
        {
            int max = 200;
            if (boundarySize > max)
            {
                throw new ArgumentException($"Boundary size cannot exceed {max} ly.", nameof(boundarySize));
            }

            RestResponse<IEnumerable<ApiSystem>> response = await ApiClient.ExecuteAsync<IEnumerable<ApiSystem>>(
                new RestRequest("cube-systems")
                .AddQueryParameter("x", coords.X)
                .AddQueryParameter("y", coords.Y)
                .AddQueryParameter("z", coords.Z)
                .AddQueryParameter("size", boundarySize));
            try
            {
                CheckResponseStatus(response);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"No systems found in a cube of {boundarySize} ly boundary size around {coords}.");
            }

            return response.Data;
        }

        private static void CheckResponseStatus(RestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                if (response.ErrorException is System.Text.Json.JsonException && response.Content!.Equals("[]"))
                {
                    throw new ArgumentException();
                }
                else
                {
                    throw response.ErrorException!;
                }
            }
        }
    }
}