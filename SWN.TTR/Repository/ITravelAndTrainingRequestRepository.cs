using SWN.TTR.Repository.Dto;
using SWN.TTR.Repository.TravelAndTrainingRequestRepositoryTableAdapters;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SWN.TTR.Repository
{
    public interface ITravelAndTrainingRequestRepository
    {
        // TODO: Change datatables to return LINQ to SQL


        #region Reporting

        TravelAndTrainingRequestRepository.RequestViewDataTable GetAllRequests();

        TravelAndTrainingRequestRepository.RequestViewDataTable GetRequestsByPendingReviewer(string reviewer);

        TravelAndTrainingRequestRepository.RequestViewDataTable GetRequestsByRequestor(string requestor);

        TravelAndTrainingRequestRepository.RequestViewDataTable GetRequestsByRequestorAndStatus(string requestor, string status);

        TravelAndTrainingRequestSummaryRepository GetRequestSummaryByRequest(int requestId);

        TravelAndTrainingRequestRepository.ApproverViewDataTable GetDataByApprover(string reviewer);

        #endregion

        void Save(TravelAndTrainingRequest request);

        TravelAndTrainingRequest GetRequestById(int id);

        void AddApprovalAudit(TravelAndTrainingRequest assignedTo, Approval approval);

        void Remove(int requestId);
    }
}