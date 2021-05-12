using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICICILombard.TeamsApp.ChampApp.Models
{
    public class UserProfile
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [JsonProperty("mail")]
        public string Mail { get; set; }

        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty("officeLocation")]
        public string OfficeLocation { get; set; }

        [JsonProperty("preferredLanguage")]
        public string PreferredLanguage { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("photoUrl")]
        public string PhotoUrl { get; set; }
    }

    public class TeamMemberInfo
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("Email")]
        public string Mail { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        
    }
}
