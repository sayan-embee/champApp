// <copyright file="AwardCard.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace ICICILombard.TeamsApp.ChampApp.Cards
{
    using System.Collections.Generic;
    using AdaptiveCards;
    using Microsoft.Bot.Schema;
    using ICICILombard.TeamsApp.ChampApp.Models;
    using ICICILombard.TeamsApp.ChampApp.Resources;
    using System.IO;
    using AdaptiveCards.Templating;
    using Newtonsoft.Json;

    /// <summary>
    /// Class having method to create card sent after awarding a badge.
    /// </summary>
    public class AwardCard
    {
        /// <summary>
        /// Get adaptive card attachment when badge is awarded to user.
        /// </summary>
        /// <param name="viewBadge">Instance of class containing information of awarded badge.</param>
        /// <returns>An attachment containing award confirmation card.</returns>
        public static Attachment GetAwardBadgeAttachment(ViewBadge viewBadge)
        {
            AdaptiveCard card = new AdaptiveCard("1.2")
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveColumnSet
                    {
                        Columns = new List<AdaptiveColumn>
                        {
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Auto,
                                VerticalContentAlignment = AdaptiveVerticalContentAlignment.Center,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Size = AdaptiveTextSize.Large,
                                        Wrap = true,
                                        Text = viewBadge.BadgeName,
                                        Weight = AdaptiveTextWeight.Bolder,
                                        HorizontalAlignment = AdaptiveHorizontalAlignment.Left,
                                        Height = AdaptiveHeight.Auto,
                                    },
                                },
                            },
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Stretch,
                                VerticalContentAlignment = AdaptiveVerticalContentAlignment.Center,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveImage
                                    {
                                        Url = new System.Uri(viewBadge.ImageUri),
                                        Size = AdaptiveImageSize.Medium,
                                        HorizontalAlignment = AdaptiveHorizontalAlignment.Right,
                                        Height = AdaptiveHeight.Auto,
                                    },
                                },
                            },
                        },
                    },
                    new AdaptiveColumnSet
                    {
                        Columns = new List<AdaptiveColumn>
                        {
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Auto,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Size = AdaptiveTextSize.Small,
                                        Wrap = true,
                                        Text = string.Format(Strings.AwardedTo, viewBadge.AwardedBy),
                                        Weight = AdaptiveTextWeight.Default,
                                    },
                                },
                            },
                        },
                    },
                    new AdaptiveColumnSet
                    {
                        Columns = new List<AdaptiveColumn>
                        {
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Auto,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Size = AdaptiveTextSize.Small,
                                        Wrap = true,
                                        Text = $"{string.Join(", ", viewBadge.AwardRecipients)}",
                                        Weight = AdaptiveTextWeight.Bolder,
                                    },
                                },
                            },
                        },
                    },
                    new AdaptiveColumnSet
                    {
                        Columns = new List<AdaptiveColumn>
                        {
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Auto,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Size = AdaptiveTextSize.Small,
                                        Wrap = true,
                                        Text = viewBadge.Narrative,
                                        Weight = AdaptiveTextWeight.Default,
                                    },
                                },
                            },
                        },
                    },
                },
            };

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card,
            };

            return adaptiveCardAttachment;
        }

        public static Attachment GetBadgeAttachment(BadgeList _badgeList, UserProfile recipent, UserProfile sender,string baseurl)
        {

            string filePath = Directory.GetCurrentDirectory() + @"\Cards\JSONCards\TestCard.json";
            _badgeList.awardRecipients[0].photoUrl = recipent.PhotoUrl;
            _badgeList.awardedByPhotoUrl = sender.PhotoUrl;
            //_badgeList.UPN = recipent.UserPrincipalName;
            
            //_badgeList.badge[0].badgeImage = "https://5222dce4dcd1.ngrok.io/images/gold.png"; //_badgeList.badge[0].badgeImage;
            var adaptiveCardJsons = File.ReadAllText(Path.Combine(filePath));
            AdaptiveCardTemplate template = new AdaptiveCardTemplate(adaptiveCardJsons);
            string cardJsons = template.Expand(_badgeList);
            var adaptiveCardAttachments = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardJsons),
            };
            return adaptiveCardAttachments;
        }
    }
}
