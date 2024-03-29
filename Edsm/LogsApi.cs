﻿// <copyright file="LogsApi.cs" company="alterNERDtive">
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

namespace alterNERDtive.Edna.Edsm
{
    /// <summary>
    /// A Commander in the galaxy of Elite Dangerous, as returned from EDSM’s
    /// “Logs” API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Either this or wrong class/struct order 🤷")]
    public struct ApiCmdr
    {
        /// <summary>
        /// Gets or sets EDSM’s internal message code for the request; see
        /// https://www.edsm.net/en/api-logs-v1 for details.
        /// </summary>
        public int MsgNum { get; set; }

        /// <summary>
        /// Gets or sets the API’s status message; see
        /// https://www.edsm.net/en/api-logs-v1 for details.
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// Gets or sets the commander’s current system. Will be “null” if the
        /// commander’s flight log is hidden.
        /// </summary>
        public string? System { get; set; }

        /// <summary>
        /// Gets or sets whether the commander has first discovered their
        /// current system.
        /// </summary>
        public bool? FirstDiscover { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the commander’s jump into the
        /// current system. Will be null if the commander’s flight log
        /// timestamps are hidden.
        /// </summary>
        public string? Date { get; set; }

        /// <summary>
        /// Gets or sets EDSM’s internal ID of the commander’s current system.
        /// </summary>
        public ulong? SystemId { get; set; }

        /// <summary>
        /// Gets or sets the ID64 of the commander’s current system.
        /// </summary>
        public ulong? SystemId64 { get; set; }

        /// <summary>
        /// Gets or sets the coordinates of the commander’s current system. Will
        /// be null if the commander’s flight log is hidden.
        /// </summary>
        public Coordinates? Coordinates { get; set; }

        /// <summary>
        /// Gets or sets whether the commander is currently docked to a station.
        /// </summary>
        public bool? IsDocked { get; set; }

        /// <summary>
        /// Gets or sets the station the commander is currently docked at.
        /// </summary>
        public string Station { get; set; }

        /// <summary>
        /// Gets or sets EDSM’s internal ID of the station the commander is
        /// currently docked at.
        /// </summary>
        public int? StationId { get; set; }

        /// <summary>
        /// Gets or sets the date time of docking at the commander’s currently
        /// docked station.
        /// </summary>
        public string? DateDocked { get; set; }

        /// <summary>
        /// Gets or sets the ship ID of the commander’s current ship. That is
        /// the slot ID of the ship, not the “ID” chosen in livery.
        /// </summary>
        public int? ShipId { get; set; }

        /// <summary>
        /// Gets or sets the ship type of the commander’s current ship.
        /// </summary>
        public string ShipType { get; set; }

        /// <summary>
        /// Gets or sets the fuel status of the commander’s current ship. Seems
        /// to always be “null”.
        /// </summary>
        public object ShipFuel { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the commander’s last recorded
        /// activity. Will be “null” if the commander’s flight log is hidden.
        /// </summary>
        public string? DateLastActivity { get; set; }

        /// <summary>
        /// Gets or sets the commander’s EDSM profile URL. Will be “null” if the
        /// commander’s profile page is hidden.
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// Pulls data about CMDRs from the EDSM Logs API.
    /// </summary>
    public class LogsApi
    {
        private static readonly Uri ApiUrl = new Uri("https://www.edsm.net/api-logs-v1");
        private static readonly RestClient ApiClient = new RestClient(ApiUrl);

        /// <summary>
        /// Pulls data about a single CMDR from the EDSM Logs API.
        /// </summary>
        /// <param name="name">The CMDR’s name.</param>
        /// <param name="apiKey">The CMDR’s EDSM API key.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<ApiCmdr> FindCmdr(string name, string? apiKey = null)
        {
            RestRequest request = new RestRequest("get-position")
                .AddQueryParameter("commanderName", name)
                .AddQueryParameter("showId", 1)
                .AddQueryParameter("showCoordinates", 1);

            if (apiKey != null)
            {
                request.AddQueryParameter("apiKey", apiKey);
            }

            ApiCmdr response = await ApiClient.GetAsync<ApiCmdr>(request);

            if (response.MsgNum == 203)
            {
                throw new ArgumentException($"Cmdr not found{(apiKey == null ? string.Empty : " and/or invalid API key")}.", name);
            }
            else if (response.MsgNum == 100 && response.System == null && response.FirstDiscover == null && response.Date == null)
            {
                throw new AccessViolationException($"Cmdr “{name}” has not made their profile and/or flight log public.");
            }

            return response;
        }
    }
}
