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
        public string RecipentUPN { get; set; }
        public string RecipentAadObjectId { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReportingManagerEmail { get; set; }
        public string ReporitngManagerUPN { get; set; }
    }

    public class Award
    {
        public string AwardedByEmail { get; set; }
        public string AwardedByName { get; set; }
        public string AwardedByUPN { get; set; }
        public string AwardedByAadObjectId { get; set; }
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
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public string ChatId { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReportingManagerEmail { get; set; }
        public string ReporitngManagerUPN { get; set; }
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
    public class TeamsAwardRecipent
    {
        public string UserId { get; set; }
        public string UPN { get; set; }
    }

    public class MemberDetails
    {
        public string id { get; set; }
        public string email { get; set; }
        public string displayName { get; set; }
        public string givenName { get; set; }
        public string aadObjectId { get; set; }
        public string role { get; set; }
        public string upn { get; set; }
        public string photo { get; set; }
    }
}
