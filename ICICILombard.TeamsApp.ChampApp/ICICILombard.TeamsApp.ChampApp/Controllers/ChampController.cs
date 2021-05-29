// <copyright file="BadgeController.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace ICICILombard.TeamsApp.ChampApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Teams;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ICICILombard.TeamsApp.ChampApp.Helpers;
    using ICICILombard.TeamsApp.ChampApp.Models;
    using Error = ICICILombard.TeamsApp.ChampApp.Models.Error;
    using Microsoft.Graph;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using System.Net.Http.Headers;
    using System.Net.Http;
    using Newtonsoft.Json;
    using ICICILombard.TeamsApp.ChampApp.Provider;


    /// <summary>
    /// Controller to handle Badge API operations.
    /// </summary>
    [Route("api/champ-app")]
    [ApiController]
    //[Authorize]
    public class ChampController : ControllerBase
    {
        /// <summary>
        /// Microsoft Azure Key Vault base Uri.
        /// </summary>
        private readonly string keyVaultBaseUrl;

        private readonly string appBaseUrl;

        /// <summary>
        /// Microsoft app ID.
        /// </summary>
        private readonly string appId;


        /// <summary>
        /// TenantId.
        /// </summary>
        private readonly string tenantId;

        /// <summary>
        /// Bot adapter to get context.
        /// </summary>
        private readonly BotFrameworkAdapter botAdapter;

        /// <summary>
        /// Generating custom JWT token and retrieving Badgr access token for user.
        /// </summary>
        private readonly ITokenHelper tokenHelper;


        /// <summary>
        /// Sends logs to the Application Insights service.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Represents a set of key/value super user configuration for Badges bot.
        /// </summary>
        private readonly SuperUserSettings superUserSettings;

        /// <summary>
        /// Instance of key vault helper to retrieve key vault secrets.
        /// </summary>
        private readonly IKeyVaultHelper keyVaultHelper;


        private readonly IGraphServiceClientHelper graphServiceClientHelper;
        private readonly IApplicationDetailProvider applicationDetailProvider;
        private readonly BotSettings configurationSettings;
        private readonly IGraphServiceClientProvider graphServiceClientProvider;
        /// <summary>
        /// Initializes a new instance of the <see cref="ChampController"/> class.
        /// </summary>
        /// <param name="botAdapter">Open badges bot adapter.</param>
        /// <param name="badgeUserHelper">Instance of badge user helper.</param>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="tokenHelper">Generating custom JWT token and retrieving Badgr access token for user.</param>
        /// <param name="microsoftAppCredentials">App credentials for Bot/ME.</param>
        /// <param name="superUserSettings">A set of key/value super user configuration for Badges app.</param>
        /// <param name="keyVaultHelper">Instance of key vault helper to retrieve secrets from Microsoft Azure Key Vault.</param>
        /// <param name="badgeIssuerHelper">Instance of badge Issuer helper.</param>
        /// <param name="badgrApiHelper">Helper to handle errors and get user details.</param>
        /// <param name="badgrUserHelper">Helper to get user details from Badgr.</param>
        public ChampController(
            BotFrameworkAdapter botAdapter,
            ILogger<ChampController> logger,
            ITokenHelper tokenHelper,
            MicrosoftAppCredentials microsoftAppCredentials,
            IOptionsMonitor<SuperUserSettings> superUserSettings,
            IKeyVaultHelper keyVaultHelper,
            IGraphServiceClientHelper graphServiceClientHelper,
            IOptionsMonitor<BotSettings> optionsAccessor,
            IApplicationDetailProvider applicationDetailProvider,
            IGraphServiceClientProvider graphServiceClientProvider
            )
        {
            this.configurationSettings = optionsAccessor.CurrentValue;
            this.logger = logger;
            this.tokenHelper = tokenHelper;
            this.botAdapter = botAdapter;
            this.appId = microsoftAppCredentials.MicrosoftAppId;


            this.superUserSettings = superUserSettings.CurrentValue;
            this.keyVaultBaseUrl = this.superUserSettings.BaseUrl;
            

            this.keyVaultHelper = keyVaultHelper;
            this.graphServiceClientHelper = graphServiceClientHelper;
            this.appBaseUrl = this.configurationSettings.AppBaseUri;
            this.tenantId = this.configurationSettings.TenantId;
            this.applicationDetailProvider = applicationDetailProvider;
            this.graphServiceClientProvider = graphServiceClientProvider;
        }

      
        [Route("teammembers")]
        public async Task<IActionResult> GetTeamMembersAsync([FromHeader] string authorization, string teamId)
        {
            try
            {
                if (teamId == null)
                {
                    return this.BadRequest(new { message = "Team ID cannot be empty." });
                }

                var userClaims = this.GetUserClaims();

                IEnumerable<TeamsChannelAccount> teamsChannelAccounts = new List<TeamsChannelAccount>();
                var conversationReference = new ConversationReference
                {
                    ChannelId = teamId,
                    ServiceUrl = userClaims.ServiceUrl,
                   
                };
              
                await this.botAdapter.ContinueConversationAsync(
                    this.appId,
                    conversationReference,
                    async (context, token) =>
                    {
                        teamsChannelAccounts = await TeamsInfo.GetTeamMembersAsync(context, teamId, default);
                    },
                    default);

                var members = teamsChannelAccounts.Where(x=>!string.IsNullOrEmpty(x.Email)).Select(member => new MemberDetails { id = member.Id, email = member.Email, displayName = member.Name, givenName = member.GivenName, aadObjectId = member.AadObjectId, role = member.UserRole, upn = member.UserPrincipalName }).ToList();
               if (members != null)
                {
                    var graphClient = await this.graphServiceClientProvider.GetGraphClientApplication();

                    for (int i = 0; i < members.Count(); i++)
                    {
                        var userPhoto = await this.graphServiceClientHelper.GetUserPhotoAsync(graphClient, members[i].aadObjectId);
                        members[i].photo = userPhoto;
                    }

                }
                return this.Ok(members);
                //return this.Ok(teamsChannelAccounts.Select(member => new {id=member.Id,  email = member.Email, displayName = member.Name, givenName=member.GivenName, aadObjectId=member.AadObjectId,role=member.UserRole,upn=member.UserPrincipalName }));
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                this.logger.LogError(ex, "Error occurred while getting team member list.");
                throw;
            }
        }

        [Route("teammemberssearch")]
        public async Task<IActionResult> GetTeamMembersSearchAsync([FromHeader] string authorization, string teamId,string searchKey)
        {
            try
            {
                if (teamId == null)
                {
                    return this.BadRequest(new { message = "Team ID cannot be empty." });
                }

                var userClaims = this.GetUserClaims();

                IEnumerable<TeamsChannelAccount> teamsChannelAccounts = new List<TeamsChannelAccount>();
                var conversationReference = new ConversationReference
                {
                    ChannelId = teamId,
                    ServiceUrl = userClaims.ServiceUrl,

                };

                await this.botAdapter.ContinueConversationAsync(
                    this.appId,
                    conversationReference,
                    async (context, token) =>
                    {
                        teamsChannelAccounts = await TeamsInfo.GetTeamMembersAsync(context, teamId, default);
                    },
                    default);

                    var members = teamsChannelAccounts.Where(x => x.Name.ToLower().IndexOf(searchKey) >= 0 &&  !string.IsNullOrEmpty(x.Email)).Select(member => new MemberDetails { id = member.Id, email = member.Email, displayName = member.Name, givenName = member.GivenName, aadObjectId = member.AadObjectId, role = member.UserRole, upn = member.UserPrincipalName }).ToList();
               if (members != null)
                {
                    var graphClient = await this.graphServiceClientProvider.GetGraphClientApplication();

                    for (int i=0;i< members.Count(); i++)
                        {
                           var userPhoto=await this.graphServiceClientHelper.GetUserPhotoAsync(graphClient, members[i].aadObjectId);
                            members[i].photo = userPhoto;
                        }

                    }
                    return this.Ok(members);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                this.logger.LogError(ex, "Error occurred while getting team member list.");
                throw;
            }
        }
        [Route("chatmembers")]
        public async Task<IActionResult> GetChatMembersAsync([FromHeader] string authorization, string chatId)
        {
            try
            {
                if (chatId == null)
                {
                    return this.BadRequest(new { message = "Team ID cannot be empty." });
                }

                var userClaims = this.GetUserClaims();

                IEnumerable<TeamsChannelAccount> teamsChannelAccounts = new List<TeamsChannelAccount>();
               
                var conversationReference = new ConversationReference()
                {
                    Conversation=new ConversationAccount()
                    {
                        Id=chatId,
                        TenantId= this.tenantId
                    },
                    ServiceUrl = userClaims.ServiceUrl,
                };

               
                await this.botAdapter.ContinueConversationAsync(
                    this.appId,
                    conversationReference,
                    async (context, token) =>
                    {
                        teamsChannelAccounts = await TeamsInfo.GetMembersAsync(context,  default);
                    },
                    default);

                var members = teamsChannelAccounts.Where(x => !string.IsNullOrEmpty(x.Email)).Select(member => new MemberDetails { id = member.Id, email = member.Email, displayName = member.Name, givenName = member.GivenName, aadObjectId = member.AadObjectId, role = member.UserRole, upn = member.UserPrincipalName }).ToList();
                if (members != null)
                {
                    var graphClient = await this.graphServiceClientProvider.GetGraphClientApplication();

                    for (int i = 0; i < members.Count(); i++)
                    {
                        var userPhoto = await this.graphServiceClientHelper.GetUserPhotoAsync(graphClient, members[i].aadObjectId);
                        members[i].photo = userPhoto;
                    }

                }
                return this.Ok(members);
                //return this.Ok(teamsChannelAccounts.Select(member => new { id = member.Id, email = member.Email, displayName = member.Name, givenName = member.GivenName, aadObjectId = member.AadObjectId, role = member.UserRole, upn = member.UserPrincipalName }));
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                this.logger.LogError(ex, "Error occurred while getting team member list.");
                throw;
            }
        }

        [Route("chatmemberssearch")]
        public async Task<IActionResult> GetChatMembersSearchAsync([FromHeader] string authorization, string chatId,string searchKey)
        {
            try
            {
                if (chatId == null)
                {
                    return this.BadRequest(new { message = "Team ID cannot be empty." });
                }

                var userClaims = this.GetUserClaims();

                IEnumerable<TeamsChannelAccount> teamsChannelAccounts = new List<TeamsChannelAccount>();

                var conversationReference = new ConversationReference()
                {
                    Conversation = new ConversationAccount()
                    {
                        Id = chatId,
                        TenantId = this.tenantId
                    },
                    ServiceUrl = userClaims.ServiceUrl,
                };


                await this.botAdapter.ContinueConversationAsync(
                    this.appId,
                    conversationReference,
                    async (context, token) =>
                    {
                        teamsChannelAccounts = await TeamsInfo.GetMembersAsync(context, default);
                    },
                    default);

                var members = teamsChannelAccounts.Where(x => x.Name.ToLower().IndexOf(searchKey) >= 0 && !string.IsNullOrEmpty(x.Email)).Select(member => new MemberDetails { id = member.Id, email = member.Email, displayName = member.Name, givenName = member.GivenName, aadObjectId = member.AadObjectId, role = member.UserRole, upn = member.UserPrincipalName }).ToList();
                if (members != null)
                {
                    var graphClient = await this.graphServiceClientProvider.GetGraphClientApplication();
                    for (int i = 0; i < members.Count(); i++)
                    {
                        var userPhoto = await this.graphServiceClientHelper.GetUserPhotoAsync(graphClient, members[i].aadObjectId);
                        members[i].photo = userPhoto;
                    }

                }
                return this.Ok(members);
                //return this.Ok(teamsChannelAccounts.Select(member => new { id = member.Id, email = member.Email, displayName = member.Name, givenName = member.GivenName, aadObjectId = member.AadObjectId, role = member.UserRole, upn = member.UserPrincipalName }));
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                this.logger.LogError(ex, "Error occurred while getting team member list.");
                throw;
            }
        }
        /// <summary>
        /// Get the list of viswas behaviour.
        /// </summary>
        /// <returns>List of viswas behaviour item</returns>
        /// 

        [HttpPost]
        [Route("addvishvasbehaviour")]
        public async Task<IActionResult>AddVishvasBehaviour(VishvasBehaviour request)
        {
            try
            {
                var response = await this.applicationDetailProvider.InsertUpdateBehaviour(request).ConfigureAwait(false);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("getvishvasbehaviour")]
        public async Task<IActionResult> GetVishvasBehaviourDetails(SearchVishvasBehaviour searchScope)
        {
            try
            {
                var response = await this.applicationDetailProvider.GetAllBehaviour(searchScope).ConfigureAwait(false);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("getapplausecard")]
        public async Task<IActionResult> GetApplauseCardDetails(SearchApplauseCard searchScope)
        {
            try
            {
                var response = await this.applicationDetailProvider.GetAllCard(searchScope).ConfigureAwait(false);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getappsetting")]
        public async Task<IActionResult> GetAppSetting()
        {
            try
            {
                var response = await this.applicationDetailProvider.GetAppSetting().ConfigureAwait(false);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("addapplausecard")]
        public async Task<IActionResult> AddApplauseCard(ApplauseCard request)
        {
            try
            {
                var response = await this.applicationDetailProvider.InsertUpdateCard(request).ConfigureAwait(false);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("updateappsetting")]
        public async Task<IActionResult> UpdateAppSetting(AppSetting request)
        {
            try
            {
                var response = await this.applicationDetailProvider.UpdateAppSetting(request).ConfigureAwait(false);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("sendaward")]
        public async Task<IActionResult> SendAward(Award request)
        {
            try
            {
                var response = await this.applicationDetailProvider.InsertAward(request).ConfigureAwait(false);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("getawardlist")]
        public async Task<IActionResult> GetAwardList(SearchAward searchScope)
        {
            try
            {
                var response = await this.applicationDetailProvider.GetAwardList(searchScope).ConfigureAwait(false);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("getawardlistbycard")]
        public async Task<IActionResult> GetAwardListByCardId(SearchByCardId searchScope)
        {
            try
            {
                var response = await this.applicationDetailProvider.GetAwardListByCardId(searchScope).ConfigureAwait(false);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("getawardlistbyrecipent")]
        public async Task<IActionResult> GetAwardListByRecipent(SearchByRecipent searchScope)
        {
            try
            {
                var response = await this.applicationDetailProvider.GetAwardListByRecipent(searchScope).ConfigureAwait(false);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getawardedemployee")]
        public async Task<IActionResult> GetAwardedEmployee()
        {
            try
            {
                var response = await this.applicationDetailProvider.GetAwardedEmployee().ConfigureAwait(false);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        /// <summary>
        /// Get claims of user.
        /// </summary>
        /// <returns>User claims.</returns>
        private JwtClaims GetUserClaims()
        {
            var claims = this.User.Claims;
            var jwtClaims = new JwtClaims
            {
                FromId = claims.Where(claim => claim.Type == "fromId").Select(claim => claim.Value).First(),
                ServiceUrl = claims.Where(claim => claim.Type == "serviceURL").Select(claim => claim.Value).First(),
            };

            return jwtClaims;
        }

        /// <summary>
        /// Creates the error response as per the status codes in case of error.
        /// </summary>
        /// <param name="statusCode">Describes the type of error.</param>
        /// <param name="errorMessage">Describes the error message.</param>
        /// <returns>Returns error response with appropriate message and status code.</returns>
        private IActionResult GetErrorResponse(int statusCode, string errorMessage)
        {
            switch (statusCode)
            {
                case StatusCodes.Status401Unauthorized:
                    return this.StatusCode(
                      StatusCodes.Status401Unauthorized,
                      new Error
                      {
                          StatusCode = "signinRequired",
                          ErrorMessage = errorMessage,
                      });
                case StatusCodes.Status400BadRequest:
                    return this.StatusCode(
                      StatusCodes.Status400BadRequest,
                      new Error
                      {
                          StatusCode = "badRequest",
                          ErrorMessage = errorMessage,
                      });
                default:
                    return this.StatusCode(
                      StatusCodes.Status500InternalServerError,
                      new Error
                      {
                          StatusCode = "internalServerError",
                          ErrorMessage = errorMessage,
                      });
            }
        }
    }
}
