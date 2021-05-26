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
    using System;

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

        public static Attachment GetBadgeAttachment(BadgeList _badgeList, UserProfile recipent, UserProfile sender, string baseurl)
        {

            string filePath = Directory.GetCurrentDirectory() + @"\Cards\JSONCards\TestCard.json";
            _badgeList.awardRecipients[0].photoUrl = recipent.PhotoUrl;
            _badgeList.awardedByPhotoUrl = sender.PhotoUrl;
            //_badgeList.UPN = recipent.UserPrincipalName;

            //_badgeList.badge[0].badgeImage = "https://5222dce4dcd1.ngrok.io/images/gold.png"; //_badgeList.badge[0].badgeImage;
            var adaptiveCardJsons = File.ReadAllText(Path.Combine(filePath));
            AdaptiveCardTemplate template = new AdaptiveCardTemplate(adaptiveCardJsons);
            //string cardJsons = template.Expand(_badgeList);
            string cardJsons= "{" +
 " \"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\"," +
  "\"type\": \"AdaptiveCard\"," +
  "\"version\": \"1.2\"," +
  "\"body\": [" +
    "{" +
      "\"type\": \"ColumnSet\"," +
     " \"columns\": [" +
        "{" +
          "\"type\": \"Column\"," +
          "\"items\": [" +
            "{" +
              "\"type\": \"Image\"," +
              "\"style\": \"Person\"," +
              "\"url\": \""+sender.PhotoUrl+"\"," +
              "\"size\": \"Small\"," +
              "\"horizontalAlignment\": \"Right\"" +
            "}" +
          "]," +
          "\"width\": 40" +
        "}," +
        "{" +
          "\"type\": \"Column\"," +
          "\"items\": [" +
            "{" +
              "\"type\": \"TextBlock\"," +
              "\"weight\": \"Bolder\"," +
              "\"text\": \""+_badgeList.awardedByName+"\"," +
              "\"wrap\": true" +
            "}" +
          "]," +
          "\"width\": 60," +
          "\"verticalContentAlignment\": \"Center\"" +
        "}" +
      "]," +
      "\"horizontalAlignment\": \"Center\"" +
    "}," +
    "{" +
      "\"type\": \"TextBlock\"," +
      "\"size\": \"Medium\"," +
      "\"weight\": \"Lighter\"," +
      "\"text\": \"Sent applause to\"," +
      "\"wrap\": true," +
      "\"horizontalAlignment\": \"Center\"" +
    "}," +
    "{" +
      "\"type\": \"ColumnSet\"," +
      "\"columns\": [" +
        "{" +
          "\"type\": \"Column\"," +
          "\"items\": [" +
            "{" +
              "\"type\": \"Image\"," +
              "\"style\": \"Person\"," +
              "\"url\": \""+ recipent.PhotoUrl + "\"," +
              "\"size\": \"Small\"," +
              "\"horizontalAlignment\": \"Right\"" +
            "}" +
          "]," +
          "\"width\": 40" +
        "}," +
        "{" +
          "\"type\": \"Column\"," +
          "\"items\": [" +
            "{" +
              "\"type\": \"TextBlock\"," +
              "\"weight\": \"Bolder\"," +
              "\"text\": \""+_badgeList.awardRecipients[0].name+"\"," +
              "\"wrap\": true" +
            "}" +
          "]," +
          "\"width\": 60," +
          "\"verticalContentAlignment\": \"Center\"" +
        "}" +
      "]," +
      "\"horizontalAlignment\": \"Center\"" +
    "}," +
    "{" +
      "\"type\": \"Image\"," +
      "\"url\": \""+_badgeList.badge[0].badgeImage+"\"," +
      "\"horizontalAlignment\": \"Center\"," +
      "\"spacing\": \"Large\"" +
    "}," +
    "{" +
      "\"type\": \"TextBlock\"," +
      "\"text\": \"Vishvas  Behaviours\"," +
      "\"wrap\": true," +
      "\"horizontalAlignment\": \"Center\"" +
    "}," +
    "{" +
      "\"type\": \"TextBlock\"," +
      "\"text\": \""+_badgeList.behaviour+"\"," +
      "\"wrap\": true," +
      "\"horizontalAlignment\": \"Center\"," +
      "\"spacing\": \"None\"," +
      "\"color\": \"Accent\"" +
    "}," +
    "{" +
      "\"type\": \"TextBlock\"," +
      "\"text\": \"Reason for applause\"," +
      "\"wrap\": true," +
      "\"horizontalAlignment\": \"Center\"" +
    "}," +
    "{" +
      "\"type\": \"TextBlock\","+
      "\"text\": \""+_badgeList.reason+"\","+
      "\"wrap\": true,"+
      "\"horizontalAlignment\": \"Center\"," +
      "\"spacing\": \"None\"," +
      "\"color\": \"Accent\"" +
    "}," +
    "{" +
      "\"type\": \"TextBlock\"," +
      "\"text\": \"<at>"+_badgeList.UPN+"</at>\"" +
    "}," +
    "{" +
      "\"type\": \"TextBlock\"," +
      "\"text\": \"<at>" + _badgeList.UPN + "</at>\"" +
    "}" +
  "]," +
  "\"msteams\": {" +
    "\"entities\": [" +
      "{" +
        "\"type\": \"mention\"," +
        "\"text\": \"<at>"+_badgeList.UPN+"</at>\"," +
        "\"mentioned\": {" +
          "\"id\": \""+_badgeList.awardrecipentUserId+"\"," +
          "\"name\": \""+_badgeList.UPN+"\"" +
        "}" +
      "}," +
       "{" +
        "\"type\": \"mention\"," +
        "\"text\": \"<at>" + _badgeList.UPN + "</at>\"," +
        "\"mentioned\": {" +
          "\"id\": \"" + _badgeList.awardrecipentUserId + "\"," +
          "\"name\": \"" + _badgeList.UPN + "\"" +
        "}" +
      "}" +
    "]" +
  "}" +
"}";
            var adaptiveCardAttachments = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardJsons),
            };
            return adaptiveCardAttachments;
        }
        public static Attachment GetBadgeAttachment1(BadgeList _badgeList, List<UserProfile> recipent, UserProfile sender, List<TeamsAwardRecipent> awardedrecipentdetails, string baseurl)
        {
            try
            {


                string cardJsons = "{" +
 " \"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\"," +
  "\"type\": \"AdaptiveCard\"," +
  "\"version\": \"1.2\"," +
  "\"body\": [" +
    "{" +
      "\"type\": \"ColumnSet\"," +
     " \"columns\": [" +
        "{" +
          "\"type\": \"Column\"," +
          "\"items\": [" +
            "{" +
              "\"type\": \"Image\"," +
              "\"style\": \"Person\"," +
              "\"url\": \"" + sender.PhotoUrl + "\"," +
              "\"size\": \"Small\"," +
              "\"horizontalAlignment\": \"Right\"" +
            "}" +
          "]," +
          "\"width\": 40" +
        "}," +
        "{" +
          "\"type\": \"Column\"," +
          "\"items\": [" +
            "{" +
              "\"type\": \"TextBlock\"," +
              "\"weight\": \"Bolder\"," +
              "\"text\": \"" + _badgeList.awardedByName + "\"," +
              "\"wrap\": true" +
            "}" +
          "]," +
          "\"width\": 60," +
          "\"verticalContentAlignment\": \"Center\"" +
        "}" +
      "]," +
      "\"horizontalAlignment\": \"Center\"" +
    "}," +
    "{" +
      "\"type\": \"TextBlock\"," +
      "\"size\": \"Medium\"," +
      "\"weight\": \"Lighter\"," +
      "\"text\": \"Sent applause to\"," +
      "\"wrap\": true," +
      "\"horizontalAlignment\": \"Center\"" +
      "},";
                
                foreach (var rep in recipent) {

                    cardJsons += "{" +
                      "\"type\": \"ColumnSet\"," +
                      "\"columns\": [" +
                        "{" +
                          "\"type\": \"Column\"," +
                          "\"items\": [" +
                            "{" +
                              "\"type\": \"Image\"," +
                              "\"style\": \"Person\"," +
                              "\"url\": \"" + rep.PhotoUrl + "\"," +
                              "\"size\": \"Small\"," +
                              "\"horizontalAlignment\": \"Right\"" +
                            "}" +
                          "]," +
                          "\"width\": 40" +
                        "}," +
                        "{" +
                          "\"type\": \"Column\"," +
                          "\"items\": [" +
                            "{" +
                              "\"type\": \"TextBlock\"," +
                              "\"weight\": \"Bolder\"," +
                              "\"text\": \"" + rep.DisplayName + "\"," +
                              "\"wrap\": true" +
                            "}" +
                          "]," +
                          "\"width\": 60," +
                          "\"verticalContentAlignment\": \"Center\"" +
                        "}" +
                      "]," +
                      "\"horizontalAlignment\": \"Center\""+
                    "},";
                    }

                cardJsons += "{" +
                  "\"type\": \"Image\"," +
                  "\"url\": \"" + _badgeList.badge[0].badgeImage + "\"," +
                  "\"horizontalAlignment\": \"Center\"," +
                  "\"spacing\": \"Large\"" +
                "},";
                if (_badgeList.behaviour != "")
                {
                    cardJsons += "{" +
                      "\"type\": \"TextBlock\"," +
                      "\"text\": \"Vishvas  Behaviours\"," +
                      "\"wrap\": true," +
                      "\"horizontalAlignment\": \"Center\"" +
                    "}," +
                    "{" +
                      "\"type\": \"TextBlock\"," +
                      "\"text\": \"" + _badgeList.behaviour + "\"," +
                      "\"wrap\": true," +
                      "\"horizontalAlignment\": \"Center\"," +
                      "\"spacing\": \"None\"," +
                      "\"color\": \"Accent\"" +
                    "},";
                }
                cardJsons+="{" +
                  "\"type\": \"TextBlock\"," +
                  "\"text\": \"Reason for applause\"," +
                  "\"wrap\": true," +
                  "\"horizontalAlignment\": \"Center\"" +
                "}," +
                "{" +
                  "\"type\": \"TextBlock\"," +
                  "\"text\": \"" + _badgeList.reason + "\"," +
                  "\"wrap\": true," +
                  "\"horizontalAlignment\": \"Center\"," +
                  "\"spacing\": \"None\"," +
                  "\"color\": \"Accent\"" +
                "}";

                foreach (var recp in awardedrecipentdetails) {

                    cardJsons += ",{" +
                      "\"type\": \"TextBlock\"," +
                      "\"text\": \"<at>" +recp.UPN + "</at>\"" +
                    "}";
                }

                cardJsons += "]," +
                "\"msteams\": {" +
                  "\"entities\": [";
                int j = awardedrecipentdetails.Count;
                int i = 1;
                foreach (var recp in awardedrecipentdetails)
                {
                    cardJsons += "{" +
                      "\"type\": \"mention\"," +
                      "\"text\": \"<at>" + recp.UPN + "</at>\"," +
                      "\"mentioned\": {" +
                        "\"id\": \"" +recp.UserId + "\"," +
                        "\"name\": \"" + recp.UPN + "\"" +
                      "}" +
                    "}";
                    if (j > 1)
                    {
                        if(i<j)
                        {
                            cardJsons += ",";
                        }
                    }
                    i++;
                }
                     
    cardJsons += "]" +
  "}" +
"}";
                var adaptiveCardAttachments = new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = JsonConvert.DeserializeObject(cardJsons),
                };
                return adaptiveCardAttachments;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

    }
}
