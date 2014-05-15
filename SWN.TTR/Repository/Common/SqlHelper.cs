using System.Configuration;
using SWN.TTR.Repository.Dto;

namespace SWN.TTR.Repository.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SqlHelper
    {
        public static readonly string OdsConnectionString = ConfigurationManager.ConnectionStrings["cnODS"].ConnectionString;
        public static readonly string OdsConnectionProvider = ConfigurationManager.ConnectionStrings["cnODS"].ProviderName;
        public static readonly string TTRConnectionString = ConfigurationManager.ConnectionStrings["SWN.TTR.Properties.Settings.cnTTR"].ConnectionString;
        public static readonly string TTRConnectionProvider = ConfigurationManager.ConnectionStrings["SWN.TTR.Properties.Settings.cnTTR"].ProviderName;

        internal static RequestStatus ConvertToRequestStatus(string status)
        {
            switch (status)
            {
                case "NEW":
                    return RequestStatus.New;
                case "APR":
                    return RequestStatus.Approved;
                case "DNY":
                    return RequestStatus.Denied;
                case "PEN":
                    return RequestStatus.PendingApproval;
                default:
                    throw new System.InvalidOperationException("Invalid status for request.");
            }
        }

        internal static string ConvertToRequestStatus(RequestStatus status)
        {
            switch (status)
            {
                case RequestStatus.Approved:
                    return "APR";
                case RequestStatus.Denied:
                    return "DNY";
                case RequestStatus.New:
                    return "NEW";
                case RequestStatus.PendingApproval:
                    return "PEN";
                default:
                    throw new System.InvalidOperationException("Invalid status for request.");
            }
        }

        internal static string ConvertToApprovalStatus(ApprovalStatus status)
        {
            switch (status)
            {
                case ApprovalStatus.Approved:
                    return "APR";
                case ApprovalStatus.Denied:
                    return "DNY";
                case ApprovalStatus.FinalApproved:
                    return "FNA";
                case ApprovalStatus.Routed:
                    return "RTE";
                default:
                    throw new System.InvalidOperationException("Invalid approval status.");
            }
        }        
    }
}