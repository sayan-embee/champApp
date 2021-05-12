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
        //Task<UserProfile> GetUserProfileAsync(string email);
        //Task<List<UserProfile>> GetUserProfilesAsync(string ssoToken,List<string> email);

        Task<ITeamMembersCollectionPage> GetTeamMembersAsync(string teamId);
        Task<UserProfile> GetUserProfileAsync(string email);

        Task<IChatMembersCollectionPage> GetChatMembersAsync(string chatId);



    }
}
