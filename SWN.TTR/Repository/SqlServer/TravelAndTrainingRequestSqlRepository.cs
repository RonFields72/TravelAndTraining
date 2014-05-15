using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using SWN.TTR.Repository.Common;
using SWN.TTR.Repository.Dto;
using SWN.TTR.Repository.TravelAndTrainingRequestRepositoryTableAdapters;

namespace SWN.TTR.Repository.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public class TravelAndTrainingRequestSqlRepository : ITravelAndTrainingRequestRepository
    {
        #region ITravelAndTrainingRequestRepository Members

        public TravelAndTrainingRequestSummaryRepository GetRequestSummaryByRequest(int requestId)
        {
            using (SqlConnection cn = new SqlConnection(SqlHelper.TTRConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = "[dbo].[swn_sp_Travel_Training_Request_Summary_Retrieve_By_Id]";
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@Id", requestId);
                    cm.Connection.Open();
                    using (SqlDataReader rdr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        using (TravelAndTrainingRequestSummaryRepository repository = new TravelAndTrainingRequestSummaryRepository())
                        {
                            repository.RequestSummary.BeginLoadData();
                            repository.RequestSummary.Load(rdr);
                            repository.RequestSummary.EndLoadData();

                            repository.GroundTransportationSummary.BeginLoadData();
                            repository.GroundTransportationSummary.Load(rdr);
                            repository.GroundTransportationSummary.EndLoadData();

                            repository.ApprovalSummary.BeginLoadData();
                            repository.ApprovalSummary.Load(rdr);
                            repository.ApprovalSummary.EndLoadData();

                            return repository;
                        }
                    }
                }
            }
        }

        public TravelAndTrainingRequestRepository.RequestViewDataTable GetAllRequests()
        {
            using (TravelAndTrainingRequestRepository repository = new TravelAndTrainingRequestRepository())
            {
                using (RequestViewTableAdapter adapter = new RequestViewTableAdapter())
                {
                    repository.RequestView.BeginLoadData();
                    adapter.Fill(repository.RequestView);
                    repository.RequestView.EndLoadData();
                    return repository.RequestView;
                }
            }
        }

        public TravelAndTrainingRequestRepository.RequestViewDataTable GetRequestsByPendingReviewer(string reviewer)
        {
            using (TravelAndTrainingRequestRepository repository = new TravelAndTrainingRequestRepository())
            {
                using (RequestViewTableAdapter adapter = new RequestViewTableAdapter())
                {
                    repository.RequestView.BeginLoadData();
                    adapter.FillByPendingReviewer(repository.RequestView, reviewer);
                    repository.RequestView.EndLoadData();
                    return repository.RequestView;
                }
            }
        }

        public TravelAndTrainingRequestRepository.RequestViewDataTable GetRequestsByRequestor(string requestor)
        {
            using (TravelAndTrainingRequestRepository repository = new TravelAndTrainingRequestRepository())
            {
                using (RequestViewTableAdapter adapter = new RequestViewTableAdapter())
                {
                    repository.RequestView.BeginLoadData();
                    adapter.FillByRequestor(repository.RequestView, requestor);
                    repository.RequestView.EndLoadData();
                    return repository.RequestView;
                }
            }
        }
        //jonesky added for the approver view on the main page
        public TravelAndTrainingRequestRepository.ApproverViewDataTable GetDataByApprover(string reviewer)
        {
            using (TravelAndTrainingRequestRepository repository = new TravelAndTrainingRequestRepository())
            {
                using (ApproverViewTableAdapter adaptor = new ApproverViewTableAdapter())
                {
                    repository.ApproverView.BeginLoadData();
                    adaptor.FillByApprover(repository.ApproverView, reviewer);
                    repository.ApproverView.EndLoadData();
                    return repository.ApproverView;
                }
            }

        }
        public TravelAndTrainingRequestRepository.RequestViewDataTable GetRequestsByRequestorAndStatus(string requestor, string status)
        {
            using (TravelAndTrainingRequestRepository repository = new TravelAndTrainingRequestRepository())
            {
                using (RequestViewTableAdapter adapter = new RequestViewTableAdapter())
                {
                    repository.RequestView.BeginLoadData();
                    adapter.FillByRequestorStatus(repository.RequestView, requestor, status);
                    repository.RequestView.EndLoadData();
                    return repository.RequestView;
                }
            }
        }

        public void Save(TravelAndTrainingRequest request)
        {
            using (SqlConnection cn = new SqlConnection(SqlHelper.TTRConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    AddTrainingRequestInsertParameters(request, cm);
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Connection.Open();
                    using (SqlTransaction trans = cn.BeginTransaction())
                    {
                        cm.Transaction = trans;
                        try
                        {
                            if (request.Id == 0)
                            {
                                cm.CommandText = "dbo.swn_sp_Travel_Training_Request_Insert";
                                // do an insert on the ground transportation
                                request.Id = System.Convert.ToInt32(cm.ExecuteScalar());
                                AddGroundTransportation(request, trans);
                            }
                            else
                            {
                                cm.Parameters.AddWithValue("@Id", request.Id);
                                cm.CommandText = "dbo.swn_sp_Travel_Training_Request_Update";
                                int rowsAffected = System.Convert.ToInt32(cm.ExecuteScalar());
                                RemoveGroundTransportation(request, trans);
                                AddGroundTransportation(request, trans);
                            }
                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        public TravelAndTrainingRequest GetRequestById(int id)
        {
            using (SqlConnection cn = new SqlConnection(SqlHelper.TTRConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = "dbo.swn_sp_Travel_Training_Request_Retrieve_By_Id";
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@Id", id);
                    cm.Connection.Open();
                    using (SqlDataReader rdr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (rdr.Read())
                        {
                            TravelAndTrainingRequest result = new TravelAndTrainingRequest();
                            result.Id = System.Convert.ToInt32(rdr["Id"]);
                            result.Requestor = rdr["Requested_By"].ToString();
                            result.RequestedOn = System.Convert.ToDateTime(rdr["Requested_DateTime"]);
                            result.EventStart = System.Convert.ToDateTime(rdr["Event_Start_Date"]);
                            result.EventEnd = System.Convert.ToDateTime(rdr["Event_End_Date"]);
                            result.Status = SqlHelper.ConvertToRequestStatus(rdr["Status"].ToString());
                            result.HasTraining = System.Convert.ToBoolean(rdr["Has_Training"]);
                            result.HasTravel = System.Convert.ToBoolean(rdr["Has_Travel"]);
                            result.EventName = rdr["Event_Name"].ToString();
                            result.EventCity = rdr["Event_Location_City"].ToString();
                            result.EventState = rdr["Event_Location_State"].ToString();
                            result.BusinessPurpose = rdr["Event_Business_Purpose"].ToString();
                            result.ProjectName = rdr["Event_Associated_Project"].ToString();
                            result.PendingReviewBy = rdr["Pending_Review_By"].ToString();
                            result.FinalReviewedBy = rdr["Final_Reviewed_By"].ToString();
                            result.FinalReviewedOn = System.Convert.ToDateTime(rdr["Final_Reviewed_DateTime"]);
                            result.Training = new SWN.TTR.Estimates.TrainingEstimate();
                            result.Training.TrainingName = rdr["Training_Name"].ToString();
                            result.Training.StartingDate = System.Convert.ToDateTime(rdr["Training_Start_Date"]);
                            result.Training.EndingDate = System.Convert.ToDateTime(rdr["Training_End_Date"]);
                            result.Training.TotalCost = System.Convert.ToDecimal(rdr["Training_Total_Cost"]);
                            result.Travel = new SWN.TTR.Estimates.TravelEstimate();
                            result.Travel.DestinationState = rdr["Travel_Destination_State"].ToString();
                            result.Travel.DestinationCity = rdr["Travel_Destination_City"].ToString();
                            result.Travel.HasFlightArrangements = System.Convert.ToBoolean(rdr["Travel_Has_Flight"]);
                            result.Travel.HasGroundArrangements = System.Convert.ToBoolean(rdr["Travel_Has_Ground"]);
                            result.Travel.HasLodgingArrangements = System.Convert.ToBoolean(rdr["Travel_Has_Lodging"]);
                            result.MiscealleanousCostsComments = rdr["Travel_Comments"].ToString();
                            result.Travel.FlightArrangements.DestinationMethod = rdr["Travel_Flight_Destination_Id"].ToString();
                            result.Travel.FlightArrangements.ReturnMethod = rdr["Travel_Flight_Return_Id"].ToString();
                            result.Travel.FlightArrangements.TotalCost = System.Convert.ToDecimal(rdr["Travel_Flight_Total_Cost"]);
                            result.Travel.GroundArrangements.TotalCost = System.Convert.ToDecimal(rdr["Travel_Ground_Total_Cost"]);
                            result.Travel.LodgingArrangements.NumberOfNights = System.Convert.ToInt32(rdr["Travel_Lodging_Number_Of_Nights"]);
                            result.Travel.LodgingArrangements.TotalCost = System.Convert.ToDecimal(rdr["Travel_Lodging_Total_Cost"]);
                            result.MisceallaneousCosts = System.Convert.ToDecimal(rdr["Misc_Costs"]);

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                result.Travel.GroundArrangements.Modes.Add(rdr["Ground_Transportation_Id"].ToString());
                            }
                            return result;
                        }
                        else
                            return null;
                    }
                }
            }
        }

        public void AddApprovalAudit(TravelAndTrainingRequest assignedTo, Approval approval)
        {
            using (SqlConnection cn = new SqlConnection(SqlHelper.TTRConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = "dbo.swn_sp_Approval_Audit_Insert";
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@Request_Id", assignedTo.Id);
                    cm.Parameters.AddWithValue("@Reviewed_By", approval.ReviewedBy);
                    cm.Parameters.AddWithValue("@Reviewed_On", approval.ReviewedOn);
                    cm.Parameters.AddWithValue("@Reviewal_Status_Id", SqlHelper.ConvertToApprovalStatus(approval.Status));
                    cm.Parameters.AddWithValue("@Comments", approval.Comments);
                    cn.Open();
                    cm.ExecuteNonQuery();
                }
            }
        }

        public void Remove(int requestId)
        {
            using (SqlConnection cn = new SqlConnection(SqlHelper.TTRConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = "dbo.swn_sp_Travel_Training_Request_Delete";
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@Id", requestId);
                    cn.Open();
                    cm.ExecuteNonQuery();
                }
            }
        }

        #endregion

        private static void AddGroundTransportation(TravelAndTrainingRequest request, SqlTransaction trans)
        {
            foreach (string groundMode in request.Travel.GroundArrangements.Modes)
            {
                using (SqlCommand cm = trans.Connection.CreateCommand())
                {
                    cm.Transaction = trans;
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "dbo.swn_sp_Travel_Training_Request_Ground_Transportation_Insert";
                    cm.Parameters.AddWithValue("@Request_Id", request.Id);
                    cm.Parameters.AddWithValue("@Ground_Transportation_Id", groundMode);
                    cm.ExecuteNonQuery();
                }
            }
        }

        private static void RemoveGroundTransportation(TravelAndTrainingRequest request, SqlTransaction trans)
        {
            using (SqlCommand cm = trans.Connection.CreateCommand())
            {
                cm.Transaction = trans;
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "dbo.swn_sp_Travel_Training_Request_Ground_Transportation_Delete_By_Request";
                cm.Parameters.AddWithValue("@Request_Id", request.Id);
                cm.ExecuteNonQuery();
            }
        }

        private static void AddTrainingRequestInsertParameters(TravelAndTrainingRequest request, SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Requested_By", request.Requestor);
            cm.Parameters.AddWithValue("@Requested_DateTime", request.RequestedOn);
            cm.Parameters.AddWithValue("@Event_Start_Date", request.EventStart);
            cm.Parameters.AddWithValue("@Event_End_Date", request.EventEnd);
            cm.Parameters.AddWithValue("@Status", SqlHelper.ConvertToRequestStatus(request.Status));
            cm.Parameters.AddWithValue("@Has_Training", request.HasTraining);
            cm.Parameters.AddWithValue("@Has_Travel", request.HasTravel);
            cm.Parameters.AddWithValue("@Event_Name", request.EventName);
            cm.Parameters.AddWithValue("@Event_Location_City", request.EventCity);
            cm.Parameters.AddWithValue("@Event_Location_State", request.EventState);
            cm.Parameters.AddWithValue("@Event_Business_Purpose", request.BusinessPurpose);
            cm.Parameters.AddWithValue("@Event_Associated_Project", request.ProjectName);
            cm.Parameters.AddWithValue("@Pending_Review_By", request.PendingReviewBy);
            cm.Parameters.AddWithValue("@Final_Reviewed_By", request.FinalReviewedBy);
            cm.Parameters.AddWithValue("@Final_Reviewed_DateTime", request.FinalReviewedOn);            
            cm.Parameters.AddWithValue("@Training_Name", request.Training.TrainingName);
            cm.Parameters.AddWithValue("@Training_Start_Date", request.Training.StartingDate);
            cm.Parameters.AddWithValue("@Training_End_Date", request.Training.EndingDate);
            cm.Parameters.AddWithValue("@Training_Total_Cost", request.Training.TotalCost);
            cm.Parameters.AddWithValue("@Travel_Destination_State", request.Travel.DestinationState);
            cm.Parameters.AddWithValue("@Travel_Destination_City", request.Travel.DestinationCity);
            cm.Parameters.AddWithValue("@Travel_Has_Flight", request.Travel.HasFlightArrangements);
            cm.Parameters.AddWithValue("@Travel_Has_Ground", request.Travel.HasGroundArrangements);
            cm.Parameters.AddWithValue("@Travel_Has_Lodging", request.Travel.HasLodgingArrangements);
            cm.Parameters.AddWithValue("@Travel_Comments", request.MiscealleanousCostsComments);
            cm.Parameters.AddWithValue("@Travel_Flight_Destination_Id", request.Travel.FlightArrangements.DestinationMethod);
            cm.Parameters.AddWithValue("@Travel_Flight_Return_Id", request.Travel.FlightArrangements.ReturnMethod);
            cm.Parameters.AddWithValue("@Travel_Flight_Total_Cost", request.Travel.FlightArrangements.TotalCost);
            cm.Parameters.AddWithValue("@Travel_Flight_Is_Interoffice", request.Travel.FlightArrangements.IsInterofficeOnly);
            cm.Parameters.AddWithValue("@Travel_Ground_Total_Cost", request.Travel.GroundArrangements.TotalCost);
            cm.Parameters.AddWithValue("@Travel_Lodging_Number_Of_Nights", request.Travel.LodgingArrangements.NumberOfNights);
            cm.Parameters.AddWithValue("@Travel_Lodging_Total_Cost", request.Travel.LodgingArrangements.TotalCost);
            cm.Parameters.AddWithValue("@Total_Cost", request.TotalCost);
            cm.Parameters.AddWithValue("@Misc_Costs", request.MisceallaneousCosts);
        }
    }
}