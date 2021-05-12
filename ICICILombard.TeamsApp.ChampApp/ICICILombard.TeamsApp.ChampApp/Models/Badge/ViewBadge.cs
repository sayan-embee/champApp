// <copyright file="ViewBadge.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace ICICILombard.TeamsApp.ChampApp.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Class containing information of awarded badge.
    /// </summary>
    public class ViewBadge
    {
        /// <summary>
        /// Gets or sets name of user who awarded badge.
        /// </summary>
        public string AwardedBy { get; set; }

        /// <summary>
        /// Gets or sets name of badge to be awarded.
        /// </summary>
        public string BadgeName { get; set; }

        /// <summary>
        /// Gets or sets base64 encoded string of an image that represents the badge.
        /// </summary>
        public string ImageUri { get; set; }
//
        /// <summary>
        /// Gets or sets narrative that describes the achievement.
        /// </summary>
        public string Narrative { get; set; }

        /// <summary>
        /// Gets or sets list of users who received or will receive the award.
        /// </summary>
        public List<string> AwardRecipients { get; set; }

        /// <summary>
        /// Gets or sets from where task module is invoked.
        /// </summary>
        public string CommandContext { get; set; }
        
    }

    public class BadgeList
    {
        public List<awardedTo> awardRecipients { get; set; }
        public List<badgeDetails> badge { get; set; }
        public string behaviour { get; set; }
        public string reason { get; set; }
        public string awardedByName { get; set; }
        public string awardedByEmail { get; set; }
        public string awardedByUserId { get; set; }
        public string awardedByPhotoUrl { get; set; }
        public string UPN { get; set; }
        public string awardrecipentUserId { get; set; }
    }


    public class awardedTo
    {
        public string photoUrl { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string userId { get; set; }
    }

    public class badgeDetails
    {
        public string badgeName { get; set; }
        public string badgeId { get; set; }
        public string badgeImage { get; set; }
    }
}