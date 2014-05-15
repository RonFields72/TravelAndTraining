using SWN.TTR.Repository;
using SWN.TTR.Security;
using System.ComponentModel;
using SWN.TTR.Repository.Common;

namespace SWN.TTR
{
    /// <summary>
    /// Finds requests in the repository.
    /// </summary>
    public class TravelAndTrainingRequestFinder
    {
        private ITravelAndTrainingRequestRepository provider = null;
        private System.Security.Principal.IPrincipal user = null;

        public TravelAndTrainingRequestFinder(System.Security.Principal.IPrincipal user)            
        {
            this.user = user;
            this.provider = DataAccessFactory.CreateTTRRepository();
        }

        /// <summary>
        /// Gets my requests.
        /// </summary>
        /// <returns>All of my requests.</returns>
        public TravelAndTrainingRequestRepository.RequestViewDataTable GetMyRequests()
        {
            return provider.GetRequestsByRequestor(user.Identity.Name);
        }

        /// <summary>
        /// Gets my requests by status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>All of my requests by status.</returns>
        public TravelAndTrainingRequestRepository.RequestViewDataTable GetMyRequestsByStatus(RequestStatus status)
        {
            return provider.GetRequestsByRequestorAndStatus(user.Identity.Name,
                SqlHelper.ConvertToRequestStatus(status));
        }

        /// <summary>
        /// Gets all requests.
        /// </summary>
        /// <returns>All requests.</returns>
        /// <remarks>The user must be in the View All Requests role in order to perform this action.</remarks>
        public TravelAndTrainingRequestRepository.RequestViewDataTable GetAllRequests()
        {
            RequestOfficer officer = new RequestOfficer(this.user);
            if (officer.CanViewAllRequests())
                return provider.GetAllRequests();
            else
                throw new System.Security.SecurityException("User is not authorized to view all requests.");
        }

        /// <summary>
        /// Gets my pending requests.
        /// </summary>
        /// <returns>My pending requests.</returns>
        public TravelAndTrainingRequestRepository.RequestViewDataTable GetMyPendingRequests()
        {
            return provider.GetRequestsByPendingReviewer(user.Identity.Name);
        }

        /// <summary>
        /// Gets the request summary by request.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <returns>A request summary.</returns>
        public TravelAndTrainingRequestSummaryRepository GetRequestSummaryByRequest(int requestId)
        {
            return provider.GetRequestSummaryByRequest(requestId);
        }

        /// <summary>
        /// Gets the request by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The request.</returns>
        public TravelAndTrainingRequest GetRequestById(int id)
        {
            TravelAndTrainingRequest req = provider.GetRequestById(id);
            req.applicationContext = user;
            return req;
        }

        /// <summary>
        /// Gets requests that I have approved.
        /// </summary>
        /// <param name="requestId">The reviewer NetworkID(SWN\jonesky</param>
        /// <returns>Requests that were approved by the user.</returns>
        public TravelAndTrainingRequestRepository.ApproverViewDataTable GetDataByApprover(string reviewer)
        {
            return provider.GetDataByApprover(user.Identity.Name);
        }
    }
}