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
            IApplicationDetailProvider applicationDetailProvider
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
            this.applicationDetailProvider = applicationDetailProvider;
        }

        [Route("teammembers")]
        public  async Task<IActionResult> GetTeamMembersAsync([FromHeader] string authorization, string teamId)
        {
            try
            {
                string ssoToken = authorization.Substring("Bearer".Length + 1);

                if (teamId == null)
                {
                    return this.BadRequest(new { message = "Team ID cannot be empty." });
                }                
                var members=await this.graphServiceClientHelper.GetTeamMembersAsync(teamId);

                return  this.Ok(members);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while getting team member list.");
                throw;
            }
        }


        [Route("chatmembers")]
        public async Task<IActionResult> GetChatMembersAsync([FromHeader] string authorization, string chatId)
        {
            try
            {
                try
                {
                    string ssoToken = authorization.Substring("Bearer".Length + 1);

                    if (chatId == null)
                    {
                        return this.BadRequest(new { message = "Chat ID cannot be empty." });
                    }
                    var members = await this.graphServiceClientHelper.GetChatMembersAsync(chatId);

                    return this.Ok(members);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred while getting chat member list.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while getting chat member list.");
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

        [Route("viswas-behaviour")]
        [HttpGet]
        public async Task<IActionResult> GetViswasBehaviourAsync()
        {
            try
            {
                List<ViswasBehaviour> viswasBehaviourList = new List<ViswasBehaviour>();
                viswasBehaviourList.Add(new ViswasBehaviour() { Title = "Walking Togather", Id = "1" });
                viswasBehaviourList.Add(new ViswasBehaviour() { Title = "Genorisity", Id = "2" });

                return this.Ok(await Task.FromResult( viswasBehaviourList));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while viswas behaviour list.");
                throw;
            }
        }

        /// <summary>
        /// Get the list of viswas behaviour.
        /// </summary>
        /// <returns>List of viswas behaviour item</returns>
        [Route("badges")]
        [HttpGet]
        public async Task<IActionResult> GetBadgesAsync()
        {
            try
            {
                List<BadgeDetail> badgeDetailList = new List<BadgeDetail>();
                badgeDetailList.Add(new BadgeDetail() { Name = "Platinium", Id = "1", Description= "Platinium",ImageUrl= this.appBaseUrl+"/images/platinum.png",Color= "#cfe8fd" });
                badgeDetailList.Add(new BadgeDetail() { Name = "Gold", Id = "2", Description = "Gold", ImageUrl = this.appBaseUrl + "/images/gold.png", Color = "#fbcd7c" });
                badgeDetailList.Add(new BadgeDetail() { Name = "Silver", Id = "3", Description = "Silver", ImageUrl = this.appBaseUrl + "/images/silver.png", Color = "#efedea" });
                badgeDetailList.Add(new BadgeDetail() { Name = "Bronze", Id = "4", Description = "Bronze", ImageUrl = this.appBaseUrl + "/images/bronze.png", Color = "#fbd5bd" });
                badgeDetailList.Add(new BadgeDetail() { Name = "Well done", Id = "5", Description = "Bronze", ImageUrl = this.appBaseUrl + "/images/welldone.png", Color = "#c0fdaa" });
                badgeDetailList.Add(new BadgeDetail() { Name = "Thank you", Id = "6", Description = "Thank you", ImageUrl = this.appBaseUrl + "/images/thankyou.png", Color = "#d597f9" });
              
                return this.Ok(await Task.FromResult(badgeDetailList));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while badge list.");
                throw;
            }
        }

        /// <summary>
        /// Get call to retrieve list of all badges created in Issuer group.
        /// </summary>
        /// <param name="email">User's Teams account email ID.</param>
        /// <returns>Returns collection of all badges created in Issuer group.</returns>
       /* [Route("allbadges")]
        public async Task<IActionResult> GetAllBadges(string email)
        {
            try
            {
                var userBadgrRole = await this.AssignBadgrUserRoleAsync(email);
                if (string.IsNullOrEmpty(userBadgrRole))
                {
                    this.logger.LogError("Failed to fetch or add user to role.");
                    return this.GetErrorResponse(StatusCodes.Status400BadRequest, "Failed to fetch or add user to role.");
                }

                var allBadges=new List<string>() ;// await this.badgeUserHelper.GetAllBadgesAsync();

                this.logger.LogInformation("Call to badge service succeeded");
                return this.Ok(new { allBadges, userBadgrRole });
            }
            catch (UnauthorizedAccessException ex)
            {
                this.logger.LogError(ex, "Failed to get user Badgr token to make call to API.");
                return this.GetErrorResponse(StatusCodes.Status401Unauthorized, "Badgr access token for user is found empty.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while making call to badge service.");
                return this.GetErrorResponse(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }*/

        /// <summary>
        /// Get call to retrieve list of badges earned by the user in Issuer group.
        /// </summary>
        /// <returns>Returns collection of earned badges created in Issuer group.</returns>
       /* [Route("earnedbadges")]
        [HttpGet]
        public async Task<IActionResult> GetEarnedBadges()
        {
            try
            {
                var earnedBadges = await this.badgeUserHelper.GetEarnedBadgesAsync();

                this.logger.LogInformation("Call to badge service succeeded");
                return this.Ok(earnedBadges);
            }
            catch (UnauthorizedAccessException ex)
            {
                this.logger.LogError(ex, "Failed to get user token to make call to API.");
                return this.GetErrorResponse(StatusCodes.Status401Unauthorized, "Badgr access token for user is found empty.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while making call to badge service.");
                return this.GetErrorResponse(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }*/

        /// <summary>
        /// Post call to award badge to multiple user.
        /// </summary>
        /// <param name="assertionDetails">Recipient and award details.</param>
        /// <returns>Returns true for successful operation.</returns>
       /* [Route("awardbadge")]
        [HttpPost]
        public async Task<IActionResult> AwardBadgeToUsersAsync([FromBody] AssertionDetail assertionDetails)
        {
            try
            {
                if (assertionDetails == null)
                {
                    return this.BadRequest(new { message = "Details for awarding badge cannot be empty." });
                }

                return this.Ok(await this.badgeUserHelper.AwardBadgeToUsersAsync(assertionDetails));
            }
            catch (UnauthorizedAccessException ex)
            {
                this.logger.LogError(ex, "Failed to get user token to make call to API.");
                return this.GetErrorResponse(StatusCodes.Status401Unauthorized, "Badgr access token for user is found empty.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while making call to badge service.");
                return this.GetErrorResponse(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }*/

        /// <summary>
        /// Assign user staff role if its first time sign in.
        /// </summary>
        /// <param name="userTeamsEmail">Logged in user's Teams account email ID.</param>
        /// <returns>Returns user role.</returns>
      /*  private async Task<string> AssignBadgrUserRoleAsync(string userTeamsEmail)
        {
            try
            {
                // If user email ID matches with email used for login in Badgr, then check if user has any role in issuer group.
                var userRoleInBadgr = await this.badgeIssuerHelper.GetUserRoleAsync(userTeamsEmail);

                if (string.IsNullOrEmpty(userRoleInBadgr))
                {
                    // If user is not part of Issuer group, then add user in issuer group and assign "staff" role to user.
                    var userProfile = await this.badgrUserHelper.GetBadgeUserDetailsAsync();

                    userRoleInBadgr = await this.badgeIssuerHelper.AssignUserRoleAsync(userProfile);
                }

                return userRoleInBadgr;
            }
            catch (UnauthorizedAccessException ex)
            {
                this.logger.LogError(ex, "Failed to get user token to make call to API.");
                return null;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while making call to badge service.");
                return null;
            }
        }*/

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
