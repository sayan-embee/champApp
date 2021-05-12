using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICILombard.TeamsApp.ChampApp.Models
{
    public class ViswasBehaviour
    {
        /// <summary>
        /// Gets or sets name of viswas behaviour.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets id of viswas behaviour.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
