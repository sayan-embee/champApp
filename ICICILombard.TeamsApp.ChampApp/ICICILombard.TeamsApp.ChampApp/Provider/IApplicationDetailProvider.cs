using ICICILombard.TeamsApp.ChampApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICILombard.TeamsApp.ChampApp.Provider
{
    public interface IApplicationDetailProvider
    {
        Task<ReturnResponseMessage> InsertUpdateCard(ApplauseCard _formData);
        Task<ReturnResponseMessage> InsertUpdateBehaviour(VishvasBehaviour _formData);
        Task<ReturnResponseMessage> UpdateAppSetting(AppSetting _formData);
        Task<IList<ApplauseCardDetails>> GetAllCard(SearchApplauseCard searchScope);
        Task<IList<VishvasBehaviourDetails>> GetAllBehaviour(SearchVishvasBehaviour searchScope);
        Task<AppSettingDetail> GetAppSetting();
        Task<ReturnResponseMessage> InsertAward(Award _formData);
        Task<IList<AwardList>> GetAwardList(SearchAward searchScope);
        Task<IList<AwardByCardList>> GetAwardListByCardId(SearchByCardId searchScope);
        Task<IList<AwardByRecipentList>> GetAwardListByRecipent(SearchByRecipent searchScope);
        Task<IList<AwardedEmployee>> GetAwardedEmployee();
    }
}
