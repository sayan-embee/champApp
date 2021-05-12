using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICILombard.TeamsApp.ChampApp.Models
{
    public class ApplicationCommonModel
    {
    }

    public class ReturnResponseMessage
    {
        public string Msg { get; set; }
        public string ErrorMsg { get; set; }
        public int SuccessFlag { get; set; }
        public int Id { get; set; }
        public string RefNo { get; set; }
    }

    public class ApplauseCard
    {
        public int CardId { get; set; }
        public string CardName { get; set; }
        public string CardImage { get; set; }
        public int IsActive { get; set; }
        public string CreatedByEmail { get; set; }
    }

    public class ApplauseCardDetails
    {
        public int CardId { get; set; }
        public string CardName { get; set; }
        public string CardImage { get; set; }
        public int IsActive { get; set; }
    }

    public class SearchApplauseCard
    {
        public int CardId { get; set; }
        public string CardName { get; set; }
        public int? IsActive { get; set; }
    }

    public class VishvasBehaviour
    {
        public int BehaviourId { get; set; }
        public string BehaviourName { get; set; }
        public int IsActive { get; set; }
        public string CreatedByEmail { get; set; }
    }

    public class VishvasBehaviourDetails
    {
        public int BehaviourId { get; set; }
        public string BehaviourName { get; set; }
        public int IsActive { get; set; }
    }

    public class SearchVishvasBehaviour
    {
        public int BehaviourId { get; set; }
        public string BehaviourName { get; set; }
        public int? IsActive { get; set; }
    }

    public class AppSetting
    {
        public int IsGroupSingle { get; set; }
        public int IsGroupMultiple { get; set; }
        public int IsChannelSingle { get; set; }
        public int IsChannelMultiple { get; set; }
        public int IsBehaviourRequired { get; set; }
        public string CreatedByEmail { get; set; }
    }

    public class AppSettingDetail
    {
        public int IsGroupSingle { get; set; }
        public int IsGroupMultiple { get; set; }
        public int IsChannelSingle { get; set; }
        public int IsChannelMultiple { get; set; }
        public int IsBehaviourRequired { get; set; }
    }

    public class AwardRecipents
    {
        public string RecipentName { get; set; }
        public string RecipentEmail { get; set; }
    }

    public class Award
    {
        public string AwardedByEmail { get; set; }
        public string AwardedByName { get; set; }
        public int CardId { get; set; }
        public string CardName { get; set; }
        public int IsGroup { get; set; }
        public int IsTeam { get; set; }
        public int IsChat { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public int BehaviourId { get; set; }
        public string BehaviourName { get; set; }
        public string Notes { get; set; }
        public List<AwardRecipents> Recipents { get; set; }
    }

    public class AwardList
    {
        public int AwardId { get; set; }
        public string AwardedByEmail { get; set; }
        public string AwardedByName { get; set; }
        public int CardId { get; set; }
        public string CardName { get; set; }
        public string CardImage { get; set; }
        public string AwardDate { get; set; }
        public int BehaviourId { get; set; }
        public string BehaviourName { get; set; }
        public string RecipentName { get; set; }
        public string RecipentEmail { get; set; }
        public string Notes { get; set; }
    }

    public class SearchAward
    {
        public string RecipentName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class AwardByCardList
    {
        public int CardId { get; set; }
        public string CardName { get; set; }
        public string CardImage { get; set; }
        public string RecipentName { get; set; }
        public string RecipentEmail { get; set; }
        public int AwardCount { get; set; }
    }

    public class SearchByCardId
    {
        public int CardId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class SearchByRecipent
    {
        public string RecipentEmail { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class AwardByRecipentList
    {
        public int CardId { get; set; }
        public string CardName { get; set; }
        public string CardImage { get; set; }
        public string RecipentName { get; set; }
        public string RecipentEmail { get; set; }
        public int AwardCount { get; set; }
    }

    public class AwardedEmployee
    {
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
    }

}
