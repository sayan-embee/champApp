// <copyright file="BadgeBot.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace ICICILombard.TeamsApp.ChampApp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Teams;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ICICILombard.TeamsApp.ChampApp.Cards;
    using ICICILombard.TeamsApp.ChampApp.Helpers;
    using ICICILombard.TeamsApp.ChampApp.Models;
    using ICICILombard.TeamsApp.ChampApp.Resources;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Microsoft.Bot.Connector;

    /// <summary>
    /// Implements the core logic of the Badges bot.
    /// </summary>
    public class ChampBot : TeamsActivityHandler
    {
        /// <summary>
        /// Messaging extension authentication type.
        /// </summary>
        private const string MessagingExtensionAuthType = "auth";

        /// <summary>
        /// Application base Uri.
        /// </summary>
        private readonly string appBaseUrl;

        /// <summary>
        /// OAuth 2.0 bot connection name.
        /// </summary>
        private readonly string connectionName;

        /// <summary>
        /// Application Insights instrumentation key needed for initialization of logger in client application.
        /// </summary>
        private readonly string appInsightsInstrumentationKey;

        /// <summary>
        /// Unique identifier of Microsoft Azure Active Directory in which application is installed.
        /// </summary>
        private readonly string tenantId;

        /// <summary>
        /// Task module height.
        /// </summary>
        private readonly int taskModuleHeight = 500;

        /// <summary>
        /// Task module width.
        /// </summary>
        private readonly int taskModuleWidth = 600;

        /// <summary>
        /// Task module height.
        /// </summary>
        private readonly string noTeamTaskModuleHeight = "small";

        /// <summary>
        /// Task module width.
        /// </summary>
        private readonly string noTeamTaskModuleWidth = "small";

        /// <summary>
        /// Sends logs to the Application Insights service.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Generating custom JWT token and retrieving Badgr access token for user.
        /// </summary>
        private readonly ITokenHelper tokenHelper;

        /// <summary>
        /// Represents a set of key/value application configuration properties for Badges bot.
        /// </summary>
        private readonly BotSettings configurationSettings;


        /// <summary>
        /// Open badges bot adapter.
        /// </summary>
        private readonly BotFrameworkAdapter botAdapter;


        /// <summary>
        /// Microsoft app credentials.
        /// </summary>
        private readonly MicrosoftAppCredentials microsoftAppCredentials;

        private readonly IGraphServiceClientHelper graphServiceClientHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeBot"/> class.
        /// </summary>
        /// <param name="tokenHelper">Generating custom JWT token and retrieving Badgr access token for user.</param>
        /// <param name="badgeUserHelper">Instance of badge user helper.</param>
        /// <param name="optionsAccessor">A set of key/value application configuration properties for Badges bot.</param>
        /// <param name="badgeApiAppSettings">Represents a set of key/value application configuration properties for Badges bot.</param>
        /// <param name="oAuthSettings">Represents a set of key/value application configuration properties for OAuth connection.</param>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="botAdapter">Open badges bot adapter.</param>
        /// <param name="badgrIssuerHelper">Helper to get issuer group entity ID.</param>
        /// <param name="microsoftAppCredentials">Azure Bot channel registered application credentials.</param>
        public ChampBot(
            ITokenHelper tokenHelper,
            IOptionsMonitor<BotSettings> optionsAccessor,
            IOptionsMonitor<OAuthSettings> oAuthSettings,
            ILogger<ChampBot> logger,
            BotFrameworkAdapter botAdapter,
            MicrosoftAppCredentials microsoftAppCredentials,
            IGraphServiceClientHelper graphServiceClientHelper
            )
        {
            this.configurationSettings = optionsAccessor.CurrentValue;
            this.appBaseUrl = this.configurationSettings.AppBaseUri;
            this.connectionName = oAuthSettings.CurrentValue.ConnectionName;
            this.appInsightsInstrumentationKey = this.configurationSettings.AppInsightsInstrumentationKey;
            this.tenantId = this.configurationSettings.TenantId;
            this.microsoftAppCredentials = microsoftAppCredentials;


            this.logger = logger;
            this.tokenHelper = tokenHelper;

            this.botAdapter = botAdapter;
            this.graphServiceClientHelper = graphServiceClientHelper;
        }

        /// <summary>
        /// Method will be invoked on each bot turn.
        /// </summary>
        /// <param name="turnContext">Provides context for a turn of a bot.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            if (!this.IsActivityFromExpectedTenant(turnContext))
            {
                this.logger.LogInformation($"Unexpected tenant Id {turnContext.Activity.Conversation.TenantId}", SeverityLevel.Warning);
                await turnContext.SendActivityAsync(activity: MessageFactory.Text(Strings.InvalidTenant));
            }
            else
            {
                // Get the current culture info to use in resource files
                string locale = turnContext.Activity.Entities?.Where(entity => entity.Type == "clientInfo").First().Properties["locale"].ToString();

                if (!string.IsNullOrEmpty(locale))
                {
                    CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(locale);
                }

                await base.OnTurnAsync(turnContext, cancellationToken);
            }
        }

        /// <summary>
        /// Overriding to send welcome card once Bot/ME is installed in team.
        /// </summary>
        /// <param name="membersAdded">A list of all the members added to the conversation, as described by the conversation update activity.</param>
        /// <param name="turnContext">Provides context for a turn of a bot.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Welcome card  when bot is added first time by user.</returns>
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;
            this.logger.LogInformation($"conversationType: {activity.Conversation?.ConversationType}, MemberCount: {membersAdded?.Count}");

            if (membersAdded.Where(member => member.Id == activity.Recipient.Id).FirstOrDefault() != null)
            {
                this.logger.LogInformation($"Bot added {activity.Conversation.Id}");
                var welcomeCardImageUrl = new Uri(new Uri(this.appBaseUrl), "/images/welcome.png");
                //await turnContext.SendActivityAsync(MessageFactory.Attachment(WelcomeCard.GetWelcomeCardAttachment(welcomeCardImageUrl)), cancellationToken);
                return;
            }
        }

        /// <summary>
        /// Method overridden to show task module response.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="action">Messaging extension action commands.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionFetchTaskAsync(
            ITurnContext<IInvokeActivity> turnContext,
            MessagingExtensionAction action,
            CancellationToken cancellationToken)
        {

            // we are handling two cases within try/catch block 
            //if the bot is installed it will create adaptive card attachment and show card with input fields
            string memberName;
            try
            {
                // Check if your app is installed by fetching member information.
                var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
                var memberList = await TeamsInfo.GetMembersAsync(turnContext, cancellationToken);
                memberName = member.Name;

                return await OnFetchTaskAsync(turnContext, action, cancellationToken);
            }
            catch (ErrorResponseException ex)
            {
                if (ex.Body.Error.Code == "BotNotInConversationRoster")
                {
                    return new MessagingExtensionActionResponse
                    {
                        Task = new TaskModuleContinueResponse
                        {
                            Value = new TaskModuleTaskInfo
                            {
                                Card = GetAdaptiveCardAttachmentFromFile("justintimeinstallation.json"),
                                Height = 200,
                                Width = 400,
                                Title = "Adaptive Card - App Installation",
                            },
                        },
                    };
                }
                return await OnFetchTaskAsync(turnContext, action, cancellationToken);

            }


        }

        protected async Task<MessagingExtensionActionResponse> OnFetchTaskAsync(
            ITurnContext<IInvokeActivity> turnContext,
            MessagingExtensionAction action,
            CancellationToken cancellationToken)
        {
            try
            {
                var activity = turnContext.Activity;
                var activityState = ((JObject)activity.Value).GetValue("state")?.ToString();

                // Check for Badgr token.
                var userBadgrToken = await (turnContext.Adapter as IUserTokenProvider).GetUserTokenAsync(turnContext, this.connectionName, activityState, cancellationToken);
                if (userBadgrToken == null)
                {
                    // Token is not present in bot framework. Create sign in link and send sign in card to user.
                    return await this.CreateSignInCardAsync(turnContext, cancellationToken, Strings.SignInButtonText);
                }
                return await this.CreateSignInSuccessResponse(turnContext, action.Context.Theme);
            }
            catch (UnauthorizedAccessException ex)
            {
                // If token for Badgr expires, sign out user from bot and send sign in card.
                this.logger.LogError(ex, ex.Message);
                return await this.SignOutUserFromBotAsync(turnContext, cancellationToken);
            }

        }


        /// Returns adaptive card attachment which allows Just In Time installation of app. 
        private static Attachment GetAdaptiveCardAttachmentFromFile(string fileName)
        {
            string filePath = System.IO.Directory.GetCurrentDirectory() + @"\Cards\JSONCards\" + fileName + "";
            var adaptiveCardJson = System.IO.File.ReadAllText(filePath);

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }
        /// <summary>
        /// Method overridden to send card in team after awarding badge.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="action">Messaging extension action commands.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// 
        protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionSubmitActionAsync(
            ITurnContext<IInvokeActivity> turnContext,
            MessagingExtensionAction action,
            CancellationToken cancellationToken)
        {
            try
            {
                ViewMsTeam objViewMsTeam = null;
                try
                {
                    objViewMsTeam = JsonConvert.DeserializeObject<ViewMsTeam>(((JObject)action.Data).GetValue("msteams")?.ToString());
                    if (objViewMsTeam.justInTimeInstall == true)
                    {
                        return await OnFetchTaskAsync(turnContext, action, cancellationToken);
                    }
                }
                catch (Exception ex)
                {

                }
                var activityState = ((JObject)turnContext.Activity.Value).GetValue("msteams")?.ToString();
                switch (action.CommandId)
                {
                    // These commandIds are defined in the Teams App Manifest.
                    case "badge":
                        List<string> AwardedToEmails = new List<string>();
                        var JSONString = action.Data.ToString();
                        var activity = turnContext.Activity as Activity;
                        var badgeDetails = JsonConvert.DeserializeObject<BadgeList>(JSONString);
                        var awardRecipients = badgeDetails.awardRecipients.ToList();
                        string awardedByName = badgeDetails.awardedByName;

                        var memberlist = await TeamsInfo.GetMembersAsync(turnContext, cancellationToken);
                        foreach (var teamMember in memberlist)
                        {
                            if (teamMember.Email.ToLower() == awardRecipients[0].email.ToLower())
                            {
                                badgeDetails.awardrecipentUserId = teamMember.Id;
                                badgeDetails.UPN = teamMember.Name;
                                break;
                            }
                        }

                        List<TeamsAwardRecipent> awardedrecipentdetails = new List<TeamsAwardRecipent>();
                        var memberlist1 = await TeamsInfo.GetMembersAsync(turnContext, cancellationToken);
                        foreach (var teamMember in memberlist1)
                        {
                            if (teamMember.Email != null)
                            {


                                foreach (var award_recipent in awardRecipients)
                                {
                                    if (teamMember.Email.ToLower() == award_recipent.email.ToLower())
                                    {
                                        //badgeDetails.awardrecipentUserId = teamMember.Id;
                                        //badgeDetails.UPN = teamMember.Name;
                                        TeamsAwardRecipent _t = new TeamsAwardRecipent();
                                        _t.UserId = teamMember.Id;
                                        _t.UPN = teamMember.Name; ;
                                        awardedrecipentdetails.Add(_t);
                                        break;
                                    }
                                }
                            }
                        }

                        Uri url = new Uri(turnContext.Activity.ServiceUrl);
                        var client = new ConnectorClient(url, this.microsoftAppCredentials.MicrosoftAppId, this.microsoftAppCredentials.MicrosoftAppPassword);


                        /*var mentionText = new StringBuilder();
                        var entities = new List<Entity>();
                        var mentions = new List<Mention>();

                        foreach (var member in awardRecipients)
                        {
                            var mention = new Mention
                            {
                                Mentioned = new ChannelAccount()
                                {
                                    Id = member.userId,
                                    Name = member.name,
                                },
                                Text = $"<at>{XmlConvert.EncodeName(member.name)}</at>",
                                //Text = $"<at>{XmlConvert.EncodeName("Workflow Approver")}</at>",
                            };
                            mentions.Add(mention);
                            entities.Add(mention);
                            mentionText.Append(mention.Text).Append(", ");
                        }

                        var awardedBymention = new Mention
                        {
                            Mentioned = new ChannelAccount()
                            {
                                Id = turnContext.Activity.From.Id,
                                Name = awardedByName

                            },
                            Text = $"<at>{XmlConvert.EncodeName(awardedByName)}</at>",
                        };
                        entities.Add(awardedBymention);
                        string strtext = string.Format(Strings.MentionText, mentionText.ToString(), awardedBymention.Text);
                        //var notificationActivity = MessageFactory.Text(string.Format(Strings.MentionText, mentionText.ToString(), awardedBymention.Text));
                        var notificationActivity = MessageFactory.Text(strtext);
                        notificationActivity.Entities = entities;*/
                        /*************Send AdaptiveCard*************/
                        //UserProfile _receipentuser = new UserProfile();
                        //_receipentuser = await this.graphServiceClientHelper.GetUserProfileAsync(awardRecipients[0].email);
                        //UserProfile _senderuser = new UserProfile();
                        //_senderuser = await this.graphServiceClientHelper.GetUserProfileAsync(badgeDetails.awardedByEmail);
                        //var awardCardAttachment = AwardCard.GetBadgeAttachment(badgeDetails, _receipentuser, _senderuser, this.appBaseUrl);
                        //var message = Activity.CreateMessageActivity();
                        //message.Attachments.Add(awardCardAttachment);
                        //await turnContext.SendActivityAsync((Activity)message, cancellationToken);
                        /************************************/

                        List<UserProfile> _receipentuser = new List<UserProfile>();
                        foreach (var mem in awardRecipients)
                        {
                            var _userprofile = await this.graphServiceClientHelper.GetUserProfileAsync(mem.email);
                            _receipentuser.Add(_userprofile);
                        }
                        UserProfile _senderuser = new UserProfile();
                        _senderuser = await this.graphServiceClientHelper.GetUserProfileAsync(badgeDetails.awardedByEmail);
                        var awardCardAttachment = AwardCard.GetBadgeAttachment1(badgeDetails, _receipentuser, _senderuser, awardedrecipentdetails, this.appBaseUrl);
                        var message = Activity.CreateMessageActivity();
                        message.Attachments.Add(awardCardAttachment);
                        await turnContext.SendActivityAsync((Activity)message, cancellationToken);

                        //await turnContext.SendActivityAsync(notificationActivity, cancellationToken);
                        return default;


                    default:
                        return await Task.FromResult(new MessagingExtensionActionResponse());
                }


            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private MessagingExtensionActionResponse CreateCardCommand(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action)
        {
            // The user has chosen to create a card by choosing the 'Create Card' context menu command.  
            //var createCardData = ((JObject)action.Data).ToObject<CreateCardData>();

            var card = new HeroCard
            {
                Title = "Title",
                Subtitle = "Subtitle",
                Text = "Hi",
            };

            var attachments = new List<MessagingExtensionAttachment>();
            attachments.Add(new MessagingExtensionAttachment
            {
                Content = card,
                ContentType = HeroCard.ContentType,
                Preview = card.ToAttachment(),
            });

            return new MessagingExtensionActionResponse
            {
                ComposeExtension = new MessagingExtensionResult
                {
                    AttachmentLayout = "list",
                    Type = "result",
                    Attachments = attachments,
                },
            };
        }
        //protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionSubmitActionAsync(
        //    ITurnContext<IInvokeActivity> turnContext,
        //    MessagingExtensionAction action,
        //    CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        List<string> AwardedToEmails = new List<string>();
        //        var JSONString = action.Data.ToString();//.Substring(0, (action.Data.ToString().Length) - 1);
        //        var activity = turnContext.Activity as Activity;
        //        var badgeDetails = JsonConvert.DeserializeObject<BadgeList>(JSONString);
        //        string str = "";
        //        var awardRecipients = badgeDetails.awardRecipients.ToList();
        //        var awardedBy = badgeDetails.awardedBy;

        //        //string userid = turnContext.Activity.From.Id;
        //        //var member = await TeamsInfo.GetMembersAsync(turnContext, cancellationToken);
        //        //string AccountName = "", AccountEmail = "", AccountId = "";
        //        //foreach (var teamMember in member)
        //        //{
        //        //  AccountEmail = teamMember.Email;
        //        //    AccountName = teamMember.Name;
        //        //    AccountId = teamMember.Id;
        //        //    if (AccountId == userid)
        //        //    {
        //        //        break;
        //        //    }
        //        //}
        //        // Get team members in Team.
        //        /// var teamsDetails = turnContext.Activity.TeamsGetTeamInfo();
        //        //var channelMembers = await TeamsInfo.GetTeamMembersAsync(turnContext, teamsDetails.Id, cancellationToken);
        //        //var channelMembers = await TeamsInfo.GetMemberAsync(turnContext,badgeDetails.awardedBy,cancellationToken);

        //        //for (var recipientCount = 0; recipientCount < badgeDetails.AwardRecipients.Count; recipientCount++)
        //        //{
        //        //var memberemail = badgeDetails.awardRecipients[recipientCount];
        //        //AwardedToEmails.Add(memberemail.email);
        //        // badgeDetails.AwardRecipients[recipientCount].name = channelMembers.where(member => member.email.tostring() == memberemail.email.tostring()).select(member => member.name).firstordefault();
        //        //}

        //        //badgeDetails.awardedBy = channelMembers.where(member => member.id == badgeDetails.awardedBy).select(member => member.email).firstordefault();

        //        AwardedToEmails.Add("approver@esplspd.onmicrosoft.com");


        //        //string awardedByEmail = badgeDetails.awardedBy;
        //        string AwardedByEmail = "initiator@esplspd.onmicrosoft.com";
        //        var channelData = activity.GetChannelData<TeamsChannelData>();

        //        var conversationParameters = new ConversationParameters
        //        {
        //            Activity = (Activity)MessageFactory.Attachment(AwardCard.GetBadgeAttachment(badgeDetails)),
        //            //Bot = activity.Recipient,
        //            IsGroup = true,
        //            ChannelData = channelData,
        //            TenantId = channelData.Tenant.Id,
        //        };
        //        ///////////////////////////////////////////////////////////
        //        var message = Activity.CreateMessageActivity();
        //        message.Text = "Hello World";

        //        // Get activity for mentioning members who are awarded with badge.
        //        //var mentionActivity = await this.GetMentionActivityAsync(AwardedToEmails, AwardedByEmail, turnContext, cancellationToken);
        //        await this.botAdapter.CreateConversationAsync(
        //            "msteams",
        //            turnContext.Activity.ServiceUrl,
        //            this.microsoftAppCredentials,
        //            conversationParameters,
        //            async (newTurnContext, newCancellationToken) =>
        //            {
        //                await this.botAdapter.ContinueConversationAsync(
        //                    this.microsoftAppCredentials.MicrosoftAppId,
        //                    newTurnContext.Activity.GetConversationReference(),
        //                    async (conversationTurnContext, conversationCancellationToken) =>
        //                    {
        //                       // mentionActivity.ApplyConversationReference(conversationTurnContext.Activity.GetConversationReference());
        //                        await conversationTurnContext.SendActivityAsync(message, conversationCancellationToken);
        //                    },
        //                    newCancellationToken);
        //            },
        //            cancellationToken).ConfigureAwait(false);
        //        //var channelData1 = turnContext.Activity.GetChannelData<TeamsChannelData>();
        //        //var message = Activity.CreateMessageActivity();
        //        //message.Text = "Hello World";
        //        //var conversationParameters1 = new ConversationParameters
        //        //{
        //        //    IsGroup = true,
        //        //    ChannelData = new TeamsChannelData
        //        //    {
        //        //        Channel = new ChannelInfo(channelData1.Channel.Id),
        //        //    },
        //        //    Activity = (Activity)message
        //        //};
        //        //MicrosoftAppCredentials.TrustServiceUrl(activity.ServiceUrl); 
        //        //var connectorClient = new ConnectorClient(new Uri(activity.ServiceUrl)); 
        //        //var response = await connectorClient.Conversations.CreateConversationAsync(conversationParameters1); 

        //        return default;
        //    }
        //    catch(Exception ex)
        //    {
        //        return null;
        //    }
        //}

        private async Task<MessagingExtensionActionResponse> ValidateUserEmailId(ITurnContext<IInvokeActivity> turnContext, TokenResponse userBadgrToken)
        {
            var activity = turnContext.Activity;

            //Check the conversation type in which the messaging extension is opened e.g. (channel,personal,groupChat)



            // Get team members in Team.
            var teamsDetails = activity.TeamsGetTeamInfo();
            if (teamsDetails?.Id == null)
            {
                this.logger.LogError("Team ID is empty for user " + activity.From.AadObjectId);
                return new MessagingExtensionActionResponse
                {
                    Task = new TaskModuleContinueResponse
                    {
                        Value = new TaskModuleTaskInfo()
                        {
                            Card = TeamNotFoundCard.GetAttachment(),
                            Height = this.noTeamTaskModuleHeight,
                            Width = this.noTeamTaskModuleWidth,
                        },
                    },
                };
            }

            IEnumerable<TeamsChannelAccount> channelMembers = new List<TeamsChannelAccount>();

            try
            {
                channelMembers = await TeamsInfo.GetTeamMembersAsync(turnContext, teamsDetails.Id);
            }
            catch (ErrorResponseException ex)
            {
                // If bot is not part of team roster, 'Forbidden' status code is returned while fetching team members
                if (ex.Response?.StatusCode == HttpStatusCode.Forbidden)
                {
                    this.logger.LogError("Bot is not part of team roaster", ex);
                    return new MessagingExtensionActionResponse
                    {
                        Task = new TaskModuleContinueResponse
                        {
                            Value = new TaskModuleTaskInfo()
                            {
                                Card = TeamNotFoundCard.GetAttachment(),
                                Height = this.noTeamTaskModuleHeight,
                                Width = this.noTeamTaskModuleWidth,
                            },
                        },
                    };
                }
            }

            // Get user email ID.
            var userTeamsEmailId = channelMembers.First(member => member.AadObjectId == activity.From.AadObjectId).Email;
            return null;
        }

        /// <summary>
        /// Methods mentions user in respective channel of which they are part after grouping.
        /// </summary>
        /// <param name="awardedToEmails">List of email ID to whom badge is awarded.</param>
        /// <param name="awardedByEmail">Email ID of member who awarded badge.</param>
        /// <param name="turnContext">Provides context for a turn of a bot.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that sends notification in newly created channel and mention its members.</returns>
        private async Task<Activity> GetMentionActivityAsync(List<string> awardedToEmails, string awardedByEmail, ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {
                var mentionText = new StringBuilder();
                var entities = new List<Entity>();
                var mentions = new List<Mention>();
                var teamsDetails = turnContext.Activity.TeamsGetTeamInfo();
                var channelMembers = await TeamsInfo.GetTeamMembersAsync(turnContext, teamsDetails.Id, cancellationToken);

                var awardedToMemberDetails = channelMembers.Where(member => awardedToEmails.Contains(member.Email)).Select(member => new ChannelAccount { Id = member.Id, Name = member.Name });
                var awardedByMemberDetails = channelMembers.Where(member => member.Email == awardedByEmail).Select(member => new ChannelAccount { Id = member.Id, Name = member.Name }).FirstOrDefault();

                foreach (var member in awardedToMemberDetails)
                {
                    var mention = new Mention
                    {
                        Mentioned = new ChannelAccount()
                        {
                            Id = member.Id,
                            Name = member.Name,
                        },
                        Text = $"<at>{XmlConvert.EncodeName(member.Name)}</at>",
                    };
                    mentions.Add(mention);
                    entities.Add(mention);
                    mentionText.Append(mention.Text).Append(", ");
                }

                var awardedBymention = new Mention
                {
                    Mentioned = new ChannelAccount()
                    {
                        Id = awardedByMemberDetails.Id,
                        Name = awardedByMemberDetails.Name,
                    },
                    Text = $"<at>{XmlConvert.EncodeName(awardedByMemberDetails.Name)}</at>",
                };
                entities.Add(awardedBymention);

                var notificationActivity = MessageFactory.Text(string.Format(Strings.MentionText, mentionText.ToString(), awardedBymention.Text));
                notificationActivity.Entities = entities;
                return notificationActivity;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error while mentioning channel member in respective channels.");
                return null;
            }
        }

        /// <summary>
        /// Verify if the tenant id in the message is the same tenant id used when application was configured.
        /// </summary>
        /// <param name="turnContext">Provides context for a turn of a bot.</param>
        /// <returns>A boolean, true if tenant provided is expected tenant.</returns>
        private bool IsActivityFromExpectedTenant(ITurnContext turnContext)
        {
            return turnContext.Activity.Conversation.TenantId.Equals(this.tenantId, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Method creates sign in card response.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <param name="signInText">Text to be displayed on sign in card.</param>
        /// <returns>Returns sign in card response.</returns>
        private async Task<MessagingExtensionActionResponse> CreateSignInCardAsync(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken, string signInText)
        {
            var signInLink = await this.botAdapter.GetOauthSignInLinkAsync(turnContext, this.connectionName, cancellationToken);

            return new MessagingExtensionActionResponse
            {
                ComposeExtension = new MessagingExtensionResult
                {
                    Type = MessagingExtensionAuthType,
                    SuggestedActions = new MessagingExtensionSuggestedAction
                    {
                        Actions = new List<CardAction>
                            {
                                new CardAction
                                {
                                    Type = ActionTypes.OpenUrl,
                                    Value = signInLink,
                                    Title = signInText,
                                },
                            },
                    },
                },
            };
        }

        /// <summary>
        /// Method to sign out user from bot.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Returns sign in card in response after logging out user from bot.</returns>
        private async Task<MessagingExtensionActionResponse> SignOutUserFromBotAsync(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            await this.botAdapter.SignOutUserAsync(turnContext, this.connectionName, userId: null, cancellationToken);
            return await this.CreateSignInCardAsync(turnContext, cancellationToken, Strings.InvalidAccountText);
        }

        /// <summary>
        /// Creates response after user is validated after successful sign in.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="theme">Microsoft Teams theme name set by user.</param>
        /// <returns>Returns task module response where user is redirected after successful sign in.</returns>
        private async Task<MessagingExtensionActionResponse> CreateSignInSuccessResponse(ITurnContext<IInvokeActivity> turnContext, string theme)
        {
            // Get context from where task module is invoked.
            var commandContext = ((JObject)turnContext.Activity.Value).GetValue("commandContext")?.ToString();

            // Generate custom JWT token to authenticate in app API controller.
            var customAPIAuthenticationToken = this.tokenHelper.GenerateInternalAPIToken(turnContext.Activity.ServiceUrl, turnContext.Activity.From.Id, jwtExpiryMinutes: 60);

            // Check for Badgr token.
            var userBadgrToken = await (turnContext.Adapter as IUserTokenProvider).GetUserTokenAsync(turnContext, this.connectionName, null, CancellationToken.None);
            if (userBadgrToken == null)
            {
                // Token is not present in bot framework. Create sign in link and send sign in card to user.
                return await this.CreateSignInCardAsync(turnContext, CancellationToken.None, Strings.SignInButtonText);
            }

            var entitiyId = "praise";// await this.badgrIssuerHelper.GetIssuerEntityId();
            var activity = turnContext.Activity;

            if (activity.ChannelId == null)
            {
                return new MessagingExtensionActionResponse
                {
                    Task = new TaskModuleContinueResponse
                    {
                        Value = new TaskModuleTaskInfo()
                        {
                            Url = $"{this.appBaseUrl}/badge?token={customAPIAuthenticationToken}&telemetry={this.appInsightsInstrumentationKey}&entityId={entitiyId}&theme={theme}&commandContext={commandContext}",
                            Height = this.taskModuleHeight,
                            Width = this.taskModuleWidth,
                            Title = Strings.TaskModuleTitle,
                        },
                    },
                };
            }
            else
            {
                return new MessagingExtensionActionResponse
                {
                    Task = new TaskModuleContinueResponse
                    {
                        Value = new TaskModuleTaskInfo()
                        {
                            Url = $"{this.appBaseUrl}/badge?token={customAPIAuthenticationToken}&telemetry={this.appInsightsInstrumentationKey}&entityId={entitiyId}&theme={theme}&commandContext={commandContext}",

                            Height = this.taskModuleHeight,
                            Width = this.taskModuleWidth,
                            Title = Strings.TaskModuleTitle,
                        },
                    },
                };
            }
        }
    }
}