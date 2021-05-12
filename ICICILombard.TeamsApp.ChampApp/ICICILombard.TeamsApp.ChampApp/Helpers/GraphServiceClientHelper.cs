
using ICICILombard.TeamsApp.ChampApp.Models;
using ICICILombard.TeamsApp.ChampApp.Provider;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace ICICILombard.TeamsApp.ChampApp.Helpers
{
    public class GraphServiceClientHelper : IGraphServiceClientHelper
    {
        /// <summary>
        /// Telemetry service to log events and errors.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Logger servoice to log errors
        /// </summary>
       // private readonly ILogger logger;

        /// <summary>
        /// Graph service client provider to get graph service client
        /// </summary>
        private readonly IGraphServiceClientProvider graphServiceClientProvider;


        private readonly IConfiguration configuration;



        public GraphServiceClientHelper(IGraphServiceClientProvider graphServiceClientProvider, TelemetryClient telemetryClient, IConfiguration configuration)
        {
            this.graphServiceClientProvider = graphServiceClientProvider;
            this.telemetryClient = telemetryClient;
            this.configuration = configuration;

        }
        public async Task<ITeamMembersCollectionPage> GetTeamMembersAsync(string teamId)
        {
            try
            {
                GraphServiceClient graphClient = await this.graphServiceClientProvider.GetGraphClientApplication();

                var members = await graphClient.Teams[teamId].Members
                    .Request()
                    .GetAsync();                


                return members;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IChatMembersCollectionPage> GetChatMembersAsync(string chatId)
        {
            try
            {
                GraphServiceClient graphClient = await this.graphServiceClientProvider.GetGraphClientApplication();

                var members = await graphClient.Chats[chatId].Members
                    .Request()
                    .GetAsync();


                return members;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserProfile> GetUserProfileAsync(string email)
        {
            UserProfile _userProfile = null;
            try
            {
                _userProfile = new UserProfile();
                var graphClient = await this.graphServiceClientProvider.GetGraphClientApplication();
                // GraphServiceClient graphClient = this.graphServiceClientProvider.GetGraphApiClientDelegated(new[] { "User.Read.All" });

                //await Task.Delay(100);
                var response = await graphClient.Users[$"{email}"]
           .Request()
           .Select(u => new
           {
               u.DisplayName,
               u.JobTitle,
               u.GivenName,
               u.Mail,
               u.MobilePhone,
               u.OfficeLocation,
               u.PreferredLanguage,
               u.Surname,
               u.UserPrincipalName,
               u.Id
           })
           //.WithUsernamePassword(this.ServiceAccountUserId, getSecureString(this.ServiceAccountPassword))
           .GetAsync();

                //await Task.Delay(100);

                var result = await graphClient
                    .Users[$"{email}"]
                    .Photos["48x48"]
                    .Content
                    .Request()
                    //.WithUsernamePassword(this.ServiceAccountUserId, getSecureString(this.ServiceAccountPassword))
                    .GetAsync();
                var photo = "";
                if (result != null)
                {
                    byte[] bytes = new byte[result.Length];

                    result.Read(bytes, 0, (int)result.Length);

                    photo = "data:image/png;base64, " + Convert.ToBase64String(bytes);
                }

                _userProfile.DisplayName = response.DisplayName;
                _userProfile.JobTitle = response.JobTitle;
                _userProfile.GivenName = response.GivenName;
                _userProfile.Mail = response.Mail;
                _userProfile.MobilePhone = response.MobilePhone;
                _userProfile.OfficeLocation = response.OfficeLocation;
                _userProfile.PreferredLanguage = response.PreferredLanguage;
                _userProfile.Surname = response.Surname;
                _userProfile.UserPrincipalName = response.UserPrincipalName;
                _userProfile.Id = response.Id;
                _userProfile.PhotoUrl = photo;

                return _userProfile;
            }
            catch (Exception ex)
            {
                //this.telemetryClient.TrackException(ex);
                //logger.LogError($"Error in call get user profile for :{email} -> Error message: {ex.Message}");
                // return null;
                throw ex;
            }

        }



    }
}
