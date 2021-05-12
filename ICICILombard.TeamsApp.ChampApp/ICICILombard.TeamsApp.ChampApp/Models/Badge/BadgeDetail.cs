// <copyright file="BadgeDetail.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace ICICILombard.TeamsApp.ChampApp.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Class contains details of badges earned in Badgr.
    /// </summary>
    public class BadgeDetail
    {
        

        /// <summary>
        /// Gets or sets name of the badge earned.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets image URL of badge.
        /// </summary>
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets short description of the badge.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or set unique id of the badge
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or set color of the badge
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }
    }
}
