using ICICILombard.TeamsApp.ChampApp.Models;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICILombard.TeamsApp.ChampApp.Helpers
{
    public interface IGraphServiceClientHelper
    {
        /// <summary>
        /// Get team channel member details based on channel Id
        /// </summary>
        /// <param name="teamId">Channel Id</param>
        /// <returns>ITeamMembersCollectionPage</returns>
        Task<ITeamMembersCollectionPage> GetTeamMembersAsync(string teamId);



        /// <summary>
        /// Get the user profile details based on user mail.
        /// </summary>
        /// <param name="graphClient">GraphServiceClient</param>
        /// <param name="email">User Email Id.</param>
        /// <returns>UserProfile</returns>
        Task<UserProfile> GetUserProfileAsync(GraphServiceClient graphClient, string email);

        /// <summary>
        /// Get the chat members detais based on chat Id (one to one or group chat)
        /// </summary>
        /// <param name="chatId">Chat Id</param>
        /// <returns>IChatMembersCollectionPage</returns>
        Task<IChatMembersCollectionPage> GetChatMembersAsync(string chatId);

        /// <summary>
        /// Get the user profile image as based 64.
        /// </summary>
        /// <param name="graphClient">GraphServiceClient.</param>
        /// <param name="email">User email Id.</param>
        /// <returns>string</returns>
        Task<string> GetUserPhotoAsync(GraphServiceClient graphClient, string email);

        Task<User> GetUserManagerAsync(GraphServiceClient graphClient, string email);
        /// <summary>
        /// Get the application access token.
        /// </summary>
        /// <returns>string</returns>
        Task<string> GetAccessTokenAsync();

    }
}
