using ICICILombard.TeamsApp.ChampApp.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ICICILombard.TeamsApp.ChampApp.Provider
{
    public class ApplicationDetailProvider:IApplicationDetailProvider
    {
        string connectionString;
        public ApplicationDetailProvider(IOptionsMonitor<SqlDBOptions> sqlDBOptions)
        {
            connectionString = sqlDBOptions.CurrentValue.ConnectionString;
        }

        public Task<ReturnResponseMessage> InsertUpdateCard(ApplauseCard _formData)
        {
            ReturnResponseMessage returnObj = new ReturnResponseMessage();
            try
            {
                string sql = "Usp_InserUpdateCard";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 300;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CardId", _formData.CardId);
                    command.Parameters.AddWithValue("@CardName", _formData.CardName);
                    command.Parameters.AddWithValue("@CardImage", _formData.CardImage);
                    command.Parameters.AddWithValue("@IsActive", _formData.IsActive);
                    command.Parameters.AddWithValue("@CreatedByEmail", _formData.CreatedByEmail);
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnObj.Id = Convert.ToInt32(dataReader["Id"]);
                            returnObj.Msg = Convert.ToString(dataReader["Msg"]);
                            returnObj.ErrorMsg = Convert.ToString(dataReader["ErrorMsg"]);
                            returnObj.SuccessFlag = Convert.ToInt32(dataReader["SuccessFlag"]);
                            returnObj.RefNo = Convert.ToString(dataReader["RefNo"]);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(returnObj);
        }

        public Task<ReturnResponseMessage> InsertUpdateBehaviour(VishvasBehaviour _formData)
        {
            ReturnResponseMessage returnObj = new ReturnResponseMessage();
            try
            {
                string sql = "Usp_InserUpdateBehaviour";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 300;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BehaviourId", _formData.BehaviourId);
                    command.Parameters.AddWithValue("@BehaviourName", _formData.BehaviourName);
                    command.Parameters.AddWithValue("@IsActive", _formData.IsActive);
                    command.Parameters.AddWithValue("@CreatedByEmail", _formData.CreatedByEmail);
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnObj.Id = Convert.ToInt32(dataReader["Id"]);
                            returnObj.Msg = Convert.ToString(dataReader["Msg"]);
                            returnObj.ErrorMsg = Convert.ToString(dataReader["ErrorMsg"]);
                            returnObj.SuccessFlag = Convert.ToInt32(dataReader["SuccessFlag"]);
                            returnObj.RefNo = Convert.ToString(dataReader["RefNo"]);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(returnObj);
        }

        public Task<ReturnResponseMessage> UpdateAppSetting(AppSetting _formData)
        {
            ReturnResponseMessage returnObj = new ReturnResponseMessage();
            try
            {
                string sql = "Usp_UpdateAppSetting";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 300;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IsGroupSingle", _formData.IsGroupSingle);
                    command.Parameters.AddWithValue("@IsGroupMultiple", _formData.IsGroupMultiple);
                    command.Parameters.AddWithValue("@IsChannelSingle", _formData.IsChannelSingle);
                    command.Parameters.AddWithValue("@IsChannelMultiple", _formData.IsChannelMultiple);
                    command.Parameters.AddWithValue("@IsBehaviourRequired", _formData.IsBehaviourRequired);
                    command.Parameters.AddWithValue("@CreatedByEmail", _formData.CreatedByEmail);
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnObj.Id = Convert.ToInt32(dataReader["Id"]);
                            returnObj.Msg = Convert.ToString(dataReader["Msg"]);
                            returnObj.ErrorMsg = Convert.ToString(dataReader["ErrorMsg"]);
                            returnObj.SuccessFlag = Convert.ToInt32(dataReader["SuccessFlag"]);
                            returnObj.RefNo = Convert.ToString(dataReader["RefNo"]);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(returnObj);
        }

        public Task<IList<ApplauseCardDetails>> GetAllCard(SearchApplauseCard searchScope)
        {
            IList<ApplauseCardDetails> returnList = new List<ApplauseCardDetails>();
            try
            {
                string sql = "Usp_Card_GetAll";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 300;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CardId", searchScope.CardId);
                    command.Parameters.AddWithValue("@CardName", searchScope.CardName);
                    command.Parameters.AddWithValue("@IsActive", searchScope.IsActive);
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnList.Add(ConvertDataReaderDTO_ApplauseCardDetail(dataReader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(returnList);

        }

        private ApplauseCardDetails ConvertDataReaderDTO_ApplauseCardDetail(SqlDataReader dataReader)
        {
            ApplauseCardDetails obj = new ApplauseCardDetails();

            obj.CardId = Convert.ToInt32(dataReader["CardId"]);
            obj.CardName = Convert.ToString(dataReader["CardName"]);
            obj.CardImage = Convert.ToString(dataReader["CardImage"]);
            obj.IsActive = Convert.ToInt32(dataReader["IsActive"]);
            return obj;
        }

        public Task<IList<VishvasBehaviourDetails>> GetAllBehaviour(SearchVishvasBehaviour searchScope)
        {
            IList<VishvasBehaviourDetails> returnList = new List<VishvasBehaviourDetails>();
            try
            {
                string sql = "Usp_Behaviour_GetAll";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 300;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BehaviourId", searchScope.BehaviourId);
                    command.Parameters.AddWithValue("@BehaviourName", searchScope.BehaviourName);
                    command.Parameters.AddWithValue("@IsActive", searchScope.IsActive);
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnList.Add(ConvertDataReaderDTO_VishvasBehaviourDetail(dataReader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(returnList);

        }

        private VishvasBehaviourDetails ConvertDataReaderDTO_VishvasBehaviourDetail(SqlDataReader dataReader)
        {
            VishvasBehaviourDetails obj = new VishvasBehaviourDetails();

            obj.BehaviourId = Convert.ToInt32(dataReader["BehaviourId"]);
            obj.BehaviourName = Convert.ToString(dataReader["BehaviourName"]);
            obj.IsActive = Convert.ToInt32(dataReader["IsActive"]);
            return obj;
        }

        public Task<AppSettingDetail> GetAppSetting()
        {
            IList<AppSettingDetail> returnList = new List<AppSettingDetail>();
            try
            {
                string sql = "Usp_AppSetting_GetAll";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 300;
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnList.Add(ConvertDataReaderDTO_AppSettingDetail(dataReader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(returnList.FirstOrDefault());

        }

        private AppSettingDetail ConvertDataReaderDTO_AppSettingDetail(SqlDataReader dataReader)
        {
            AppSettingDetail obj = new AppSettingDetail();

            obj.IsGroupSingle = Convert.ToInt32(dataReader["IsGroupSingle"]);
            obj.IsGroupMultiple = Convert.ToInt32(dataReader["IsGroupMultiple"]);
            obj.IsChannelSingle = Convert.ToInt32(dataReader["IsChannelSingle"]);
            obj.IsChannelMultiple = Convert.ToInt32(dataReader["IsChannelMultiple"]);
            obj.IsBehaviourRequired = Convert.ToInt32(dataReader["IsBehaviourRequired"]);
            return obj;
        }

        public Task<ReturnResponseMessage> InsertAward(Award _formData)
        {
            ReturnResponseMessage returnObj = new ReturnResponseMessage();
            try
            {
                string sql = "Usp_Awards";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 300;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AwardedByEmail", _formData.AwardedByEmail);
                    command.Parameters.AddWithValue("@AwardedByName", _formData.AwardedByName);
                    command.Parameters.AddWithValue("@CardId", _formData.CardId);
                    command.Parameters.AddWithValue("@CardName", _formData.CardName);
                    command.Parameters.AddWithValue("@IsGroup", _formData.IsGroup);
                    command.Parameters.AddWithValue("@IsChat", _formData.IsChat);
                    command.Parameters.AddWithValue("@IsTeam", _formData.IsTeam);
                    command.Parameters.AddWithValue("@ChannelId", _formData.ChannelId);
                    command.Parameters.AddWithValue("@ChannelName", _formData.ChannelName);
                    command.Parameters.AddWithValue("@BehaviourId", _formData.BehaviourId);
                    command.Parameters.AddWithValue("@BehaviourName", _formData.BehaviourName);
                    command.Parameters.AddWithValue("@Notes", _formData.Notes);
                    command.Parameters.AddWithValue("@TeamId", _formData.TeamId);
                    command.Parameters.AddWithValue("@TeamName", _formData.TeamName);
                    command.Parameters.AddWithValue("@ChatId", _formData.ChatId);
                    command.Parameters.AddWithValue("@Department", _formData.Department);
                    command.Parameters.AddWithValue("@Designation", _formData.Designation);
                    command.Parameters.AddWithValue("@UserPrincipalName", _formData.AwardedByUPN);
                    command.Parameters.AddWithValue("@ReportingManagerName", _formData.ReportingManagerName);
                    command.Parameters.AddWithValue("@ReportingManagerEmail", _formData.ReportingManagerEmail);
                    command.Parameters.AddWithValue("@ReporitngManagerUPN", _formData.ReporitngManagerUPN);
                    SqlParameter tblParameter = new SqlParameter();
                    tblParameter.SqlDbType = SqlDbType.Structured;
                    tblParameter.ParameterName = "@Recipents";
                    tblParameter.TypeName = "UDT_AwardRecipent";
                    tblParameter.Value = GenerateDataTableRecipents(_formData.Recipents);
                    command.Parameters.Add(tblParameter);


                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnObj.Id = Convert.ToInt32(dataReader["Id"]);
                            returnObj.Msg = Convert.ToString(dataReader["Msg"]);
                            returnObj.ErrorMsg = Convert.ToString(dataReader["ErrorMsg"]);
                            returnObj.SuccessFlag = Convert.ToInt32(dataReader["SuccessFlag"]);
                            returnObj.RefNo = Convert.ToString(dataReader["RefNo"]);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(returnObj);
        }

        private DataTable GenerateDataTableRecipents(List<AwardRecipents> recipents)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RecipentEmail", typeof(string));
            dt.Columns.Add("RecipentName", typeof(string));
            dt.Columns.Add("UserPrincipalName", typeof(string));
            dt.Columns.Add("Department", typeof(string));
            dt.Columns.Add("Designation", typeof(string));            
            dt.Columns.Add("ReportingManagerName", typeof(string));
            dt.Columns.Add("ReportingManagerEmail", typeof(string));
            dt.Columns.Add("ReporitngManagerUPN", typeof(string));

            foreach (var dtl in recipents)
            {
                dt.Rows.Add(dtl.RecipentEmail, dtl.RecipentName, dtl.RecipentUPN, dtl.Department, dtl.Designation,dtl.ReportingManagerName,dtl.ReportingManagerEmail,dtl.ReporitngManagerUPN);
            }
            return dt;
        }

        public Task<IList<AwardList>> GetAwardList(SearchAward searchScope)
        {
            IList<AwardList> returnList = new List<AwardList>();
            try
            {
                string sql = "Usp_Award_GetAll";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 300;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FromDate", searchScope.FromDate);
                    command.Parameters.AddWithValue("@ToDate", searchScope.ToDate);
                    command.Parameters.AddWithValue("@RecipentName", searchScope.RecipentName);
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnList.Add(ConvertDataReaderDTO_AwardList(dataReader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(returnList);

        }

        private AwardList ConvertDataReaderDTO_AwardList(SqlDataReader dataReader)
        {
            AwardList obj = new AwardList();

            obj.AwardId = Convert.ToInt32(dataReader["AwardId"]);
            obj.AwardedByEmail = Convert.ToString(dataReader["AwardedByEmail"]);
            obj.AwardedByName = Convert.ToString(dataReader["AwardedByName"]);
            obj.RecipentName = Convert.ToString(dataReader["RecipentName"]);
            obj.RecipentEmail = Convert.ToString(dataReader["RecipentEmail"]);
            obj.CardId = Convert.ToInt32(dataReader["CardId"]);
            obj.CardName = Convert.ToString(dataReader["CardName"]);
            obj.CardImage = Convert.ToString(dataReader["CardImage"]);
            obj.AwardDate = Convert.ToString(dataReader["AwardDate"]);
            obj.BehaviourId = Convert.ToInt32(dataReader["BehaviourId"]);
            obj.BehaviourName = Convert.ToString(dataReader["BehaviourName"]);
            obj.Notes = Convert.ToString(dataReader["Notes"]);

            return obj;
        }

        public Task<IList<AwardByCardList>> GetAwardListByCardId(SearchByCardId searchScope)
        {
            IList<AwardByCardList> returnList = new List<AwardByCardList>();
            try
            {
                string sql = "Usp_AwardByAwardId_GetAll";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 300;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FromDate", searchScope.FromDate);
                    command.Parameters.AddWithValue("@ToDate", searchScope.ToDate);
                    command.Parameters.AddWithValue("@CardId", searchScope.CardId);
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnList.Add(ConvertDataReaderDTO_AwardByCardList(dataReader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(returnList);

        }

        private AwardByCardList ConvertDataReaderDTO_AwardByCardList(SqlDataReader dataReader)
        {
            AwardByCardList obj = new AwardByCardList();

            obj.RecipentName = Convert.ToString(dataReader["RecipentName"]);
            obj.RecipentEmail = Convert.ToString(dataReader["RecipentEmail"]);
            obj.CardId = Convert.ToInt32(dataReader["CardId"]);
            obj.CardName = Convert.ToString(dataReader["CardName"]);
            obj.CardImage = Convert.ToString(dataReader["CardImage"]);
            obj.AwardCount = Convert.ToInt32(dataReader["AwardCount"]);

            return obj;
        }

        public Task<IList<AwardedEmployee>> GetAwardedEmployee()
        {
            IList<AwardedEmployee> returnList = new List<AwardedEmployee>();
            try
            {
                string sql = "Usp_AwardedEmployee_GetAll";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 300;
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnList.Add(ConvertDataReaderDTO_AwardedEmployee(dataReader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(returnList);

        }

        private AwardedEmployee ConvertDataReaderDTO_AwardedEmployee(SqlDataReader dataReader)
        {
            AwardedEmployee obj = new AwardedEmployee();

            obj.EmployeeName = Convert.ToString(dataReader["EmployeeName"]);
            obj.EmployeeEmail = Convert.ToString(dataReader["EmployeeEmail"]);
            return obj;
        }

        public Task<IList<AwardByRecipentList>> GetAwardListByRecipent(SearchByRecipent searchScope)
        {
            IList<AwardByRecipentList> returnList = new List<AwardByRecipentList>();
            try
            {
                string sql = "Usp_AwardByEmployee_GetAll";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 300;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FromDate", searchScope.FromDate);
                    command.Parameters.AddWithValue("@ToDate", searchScope.ToDate);
                    command.Parameters.AddWithValue("@RecipentEmail", searchScope.RecipentEmail);
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnList.Add(ConvertDataReaderDTO_AwardByRecipent(dataReader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(returnList);

        }

        private AwardByRecipentList ConvertDataReaderDTO_AwardByRecipent(SqlDataReader dataReader)
        {
            AwardByRecipentList obj = new AwardByRecipentList();

            obj.RecipentName = Convert.ToString(dataReader["RecipentName"]);
            obj.RecipentEmail = Convert.ToString(dataReader["RecipentEmail"]);
            obj.CardId = Convert.ToInt32(dataReader["CardId"]);
            obj.CardName = Convert.ToString(dataReader["CardName"]);
            obj.CardImage = Convert.ToString(dataReader["CardImage"]);
            obj.AwardCount = Convert.ToInt32(dataReader["AwardCount"]);

            return obj;
        }
    }
}
