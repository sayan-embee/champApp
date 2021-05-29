
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

        /// <summary>
        /// Get Iconfiguration values.
        /// </summary>
        private readonly IConfiguration configuration;


        /// <summary>
        /// Graph Service Client Helper Class
        /// </summary>
        /// <param name="graphServiceClientProvider">Graph Service Client Provider Reference.</param>
        /// <param name="telemetryClient">Telemetry Client</param>
        /// <param name="configuration">IConfiguration.</param>
        public GraphServiceClientHelper(IGraphServiceClientProvider graphServiceClientProvider, TelemetryClient telemetryClient, IConfiguration configuration)
        {
            this.graphServiceClientProvider = graphServiceClientProvider;
            this.telemetryClient = telemetryClient;
            this.configuration = configuration;

        }
        /// <summary>
        /// Get team channel member details based on channel Id
        /// </summary>
        /// <param name="teamId">Channel Id</param>
        /// <returns>ITeamMembersCollectionPage</returns>
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

        /// <summary>
        /// Get the chat members detais based on chat Id (one to one or group chat)
        /// </summary>
        /// <param name="chatId">Chat Id</param>
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

        
        /// <summary>
        /// Get the user profile details based on user mail.
        /// </summary>
        /// <param name="accessToken">Application access token</param>
        /// <param name="email">User Email Id.</param>
        /// <returns>string</returns>
        public async Task<UserProfile> GetUserProfileAsync(GraphServiceClient graphClient, string email)
        {
            UserProfile _userProfile = null;
            try
            {
                _userProfile = new UserProfile();
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
               u.Id,
               u.Department
           })
           .GetAsync();

               var photo = "";
                try
                {
                    var result = await graphClient
                        .Users[$"{email}"]
                        .Photos["48x48"]
                        .Content
                        .Request()
                        .GetAsync();

                    if (result != null)
                    {
                        byte[] bytes = new byte[result.Length];

                        result.Read(bytes, 0, (int)result.Length);

                        photo = "data:image/png;base64, " + Convert.ToBase64String(bytes);
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
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
                _userProfile.Department = response.Department;
                return _userProfile;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get the user profile image as based 64.
        /// </summary>
        /// <param name="accessToken">Application access token.</param>
        /// <param name="email">User email Id.</param>
        /// <returns>string</returns>
        public async Task<string> GetUserPhotoAsync(GraphServiceClient graphClient, string email)
        {
            var photo = "";


            try
            {
                var result = await graphClient
                        .Users[$"{email}"]
                        .Photos["48x48"]
                        .Content
                        .Request()
                        .GetAsync();

                if (result != null)
                {
                    byte[] bytes = new byte[result.Length];

                    result.Read(bytes, 0, (int)result.Length);

                    photo = "data:image/png;base64, " + Convert.ToBase64String(bytes);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return photo;
            }

            return photo;
        }


        /// <summary>
        /// Get the user profile image as based 64.
        /// </summary>
        /// <param name="accessToken">Application access token.</param>
        /// <param name="email">User email Id.</param>
        /// <returns>string</returns>
        public async Task<User> GetUserManagerAsync(GraphServiceClient graphClient, string email)
        {
            try
            {
                var result =(User) await graphClient
                        .Users[$"{email}"]
                        .Manager
                        .Request()
                        .GetAsync();
               
                return result;
               
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }


        /// <summary>
        /// Get the application access token.
        /// </summary>
        /// <returns>string</returns>
        public async Task<string> GetAccessTokenAsync()
        {
            return await this.graphServiceClientProvider.GetApplicationAccessToken();            
        }

        
    }
}
