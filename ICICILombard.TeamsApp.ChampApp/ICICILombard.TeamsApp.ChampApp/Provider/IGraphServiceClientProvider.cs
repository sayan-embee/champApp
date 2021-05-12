using Microsoft.Graph;
using System.Threading.Tasks;

namespace ICICILombard.TeamsApp.ChampApp.Provider
{
    public interface IGraphServiceClientProvider
    {
         Task<GraphServiceClient> GetGraphClientApplication();

        Task<string> GetApplicationAccessToken();
    }
   
}
