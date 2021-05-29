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
    using ICICILombard.TeamsApp.ChampApp.Provider;
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
        private readonly int taskModuleHeight = 520;

        /// <summary>
        /// Task module width.
        /// </summary>
        private readonly int taskModuleWidth = 700;

        /// <summary>
        /// Task module height.
        /// </summary>
        //private readonly string noTeamTaskModuleHeight = "small";

        /// <summary>
        /// Task module width.
        /// </summary>
        //private readonly string noTeamTaskModuleWidth = "small";
               
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

        private readonly IApplicationDetailProvider applicationDetailProvider;

        private readonly IGraphServiceClientProvider graphServiceClientProvider;
        
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
            IGraphServiceClientHelper graphServiceClientHelper,
            IApplicationDetailProvider applicationDetailProvider,
            IGraphServiceClientProvider graphServiceClientProvider
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
            this.applicationDetailProvider = applicationDetailProvider;
            this.graphServiceClientProvider = graphServiceClientProvider;
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
                //var welcomeCardImageUrl = new Uri(new Uri(this.appBaseUrl), "/images/welcome.png");
                //await turnContext.SendActivityAsync(MessageFactory.Attachment(WelcomeCard.GetWelcomeCardAttachment(welcomeCardImageUrl)), cancellationToken);
                await Task.Delay(1);
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
            //string memberName;
            try
            {
                // Check if your app is installed by fetching member information.
                var memberList = await TeamsInfo.GetMembersAsync(turnContext,cancellationToken);
                
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
                                Title = "App Installation",
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
            string filePath =System.IO.Directory.GetCurrentDirectory() + @"\Cards\JSONCards\"+ fileName + "";
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
                try
                {
                    if (action.Data.ToString().Contains("justInTimeInstall"))
                    {
                        return await OnFetchTaskAsync(turnContext, action, cancellationToken);
                    }
                   
                }
                catch(Exception ex)
                {
                    ex.ToString();
                }
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

                        var awardDetails = badgeDetails.award;

                        //Check if award is inserted or not

                        var memberlist = await TeamsInfo.GetMembersAsync(turnContext, cancellationToken);
                        List<TeamsAwardRecipent> awardedrecipentdetails = new List<TeamsAwardRecipent>();
                        foreach (var award_recipent in awardRecipients)
                        {
                            var teamMember = memberlist.Where(x => x.UserPrincipalName.ToLower() == award_recipent.email.ToLower()).FirstOrDefault();
                            if (teamMember != null)
                            {
                                TeamsAwardRecipent _t = new TeamsAwardRecipent();
                                _t.UserId = teamMember.Id;
                                _t.UPN = teamMember.Name;
                                awardedrecipentdetails.Add(_t);
                                //break;
                            }
                        }

                        //Adding at mentioned to Sender
                        var teamMemberAwardedBy = memberlist.Where(x => x.UserPrincipalName.ToLower() == badgeDetails.awardedByEmail.ToLower()).FirstOrDefault();
                        if (teamMemberAwardedBy != null)
                        {
                            TeamsAwardRecipent _t = new TeamsAwardRecipent();
                            _t.UserId = teamMemberAwardedBy.Id;
                            _t.UPN = teamMemberAwardedBy.Name; ;
                            awardedrecipentdetails.Add(_t);
                            //break;
                        }

                        Uri url = new Uri(turnContext.Activity.ServiceUrl);
                        var client = new ConnectorClient(url, this.microsoftAppCredentials.MicrosoftAppId, this.microsoftAppCredentials.MicrosoftAppPassword);


                        /*************Send AdaptiveCard*************/

                        List<UserProfile> _receipentuser = new List<UserProfile>();

                        var graphClient = await this.graphServiceClientProvider.GetGraphClientApplication();
                        foreach (var mem in awardRecipients)
                        {
                            var _userprofile = await this.graphServiceClientHelper.GetUserProfileAsync(graphClient, mem.email);
                            _userprofile.PhotoUrl = (_userprofile.PhotoUrl == null || _userprofile.PhotoUrl == "") ? this.appBaseUrl + "/images/userImage.png" : _userprofile.PhotoUrl;

                            //Get User Manager
                            Microsoft.Graph.User lineManger = await this.graphServiceClientHelper.GetUserManagerAsync(graphClient, mem.email);
                            if (lineManger != null)
                            {
                                _userprofile.ReporitngManagerUPN = lineManger.UserPrincipalName;
                                _userprofile.ReportingManagerEmail = lineManger.Mail;
                                _userprofile.ReportingManagerName = lineManger.DisplayName;
                            }
                            _receipentuser.Add(_userprofile);
                        }

                        //For Receipent Reporting Manager
                        for(var i=0;i< awardDetails.Recipents.Count; i++)
                        {
                           var recUser= _receipentuser.Where(x => x.UserPrincipalName.ToLower() == awardDetails.Recipents[i].RecipentEmail.ToLower() || x.Mail.ToLower() == awardDetails.Recipents[i].RecipentEmail.ToLower()).FirstOrDefault();
                            if (recUser != null)
                            {
                                awardDetails.Recipents[i].RecipentEmail = recUser.Mail;
                                awardDetails.Recipents[i].RecipentUPN = recUser.UserPrincipalName;
                                awardDetails.Recipents[i].Department = recUser.Department;
                                awardDetails.Recipents[i].Designation = recUser.JobTitle;

                                awardDetails.Recipents[i].ReporitngManagerUPN = recUser.ReporitngManagerUPN;
                                awardDetails.Recipents[i].ReportingManagerEmail = recUser.ReportingManagerEmail;
                                awardDetails.Recipents[i].ReportingManagerName = recUser.ReportingManagerName;
                            }
                        }

                        UserProfile _senderuser = new UserProfile();
                        _senderuser = await this.graphServiceClientHelper.GetUserProfileAsync(graphClient, badgeDetails.awardedByEmail);
                        awardDetails.Department = _senderuser.Department;
                        awardDetails.Designation = _senderuser.JobTitle;
                        awardDetails.AwardedByUPN = _senderuser.UserPrincipalName;
                        //Get User Manager
                        Microsoft.Graph.User lineMangerSender = await this.graphServiceClientHelper.GetUserManagerAsync(graphClient, badgeDetails.awardedByEmail);
                        if (lineMangerSender != null)
                        {
                            _senderuser.ReporitngManagerUPN = lineMangerSender.UserPrincipalName;
                            _senderuser.ReportingManagerEmail = lineMangerSender.Mail;
                            _senderuser.ReportingManagerName = lineMangerSender.DisplayName;

                            awardDetails.ReporitngManagerUPN = lineMangerSender.UserPrincipalName;
                            awardDetails.ReportingManagerEmail = lineMangerSender.Mail;
                            awardDetails.ReportingManagerName = lineMangerSender.DisplayName;
                        }
                        _senderuser.PhotoUrl = (_senderuser.PhotoUrl == null || _senderuser.PhotoUrl == "") ? this.appBaseUrl + "/images/userImage.png" : _senderuser.PhotoUrl;


                        //Save Information to Database
                        
                        var response = await this.applicationDetailProvider.InsertAward(awardDetails).ConfigureAwait(false);
                        if (response.SuccessFlag == 1)
                        {
                            var message = Activity.CreateMessageActivity();
                            var awardCardAttachment = AwardCard.GetBadgeAttachment(badgeDetails, _receipentuser, _senderuser, awardedrecipentdetails, this.appBaseUrl);
                            message.Attachments.Add(awardCardAttachment);

                            await turnContext.SendActivityAsync((Activity)message, cancellationToken);
                        }

                        /************************************/

                        return default;


                    default:
                        return await Task.FromResult(new MessagingExtensionActionResponse());
                }

                
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
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