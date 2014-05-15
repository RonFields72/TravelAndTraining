using SWN.TTR.Common;
using SWN.TTR.Properties;
using SWN.TTR.Repository;
using SWN.TTR.Repository.Dto;
using System;
using System.Configuration;
using System.Net.Mail;
using SWN.TTR.Repository.Common;

namespace SWN.TTR.Email
{
    /// <summary>
    /// Sends notifications out for requests.
    /// </summary>
    public class RequestNotifier
    {
        private IOdsRepository provider = null;

        public RequestNotifier()           
        {
            this.provider = DataAccessFactory.CreateOdsRepository();
        }

        /// <summary>
        /// Sends the request for review.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="recipient">The recipient.</param>
        public void SendForReview(TravelAndTrainingRequest request, System.Net.Mail.MailAddress recipient)
        {
            using (MailMessage notification = new MailMessage())
            {
                notification.Priority = MailPriority.High;
                notification.Subject = Resources.RequestReviewalEmailSubject;
                notification.IsBodyHtml = true;
                notification.To.Add(recipient);
                notification.Body =
                    string.Format(Resources.ReviewRequestEmail,
                    ConfigurationManager.AppSettings["PendingRequestsLink"],
                    request.EventName,
                    request.EventStart.ToString("d"),
                    request.EventEnd.ToString("d"),
                    provider.GetEmployeeByNetworkID(request.Requestor).FullName,
                    request.TotalCost.ToString("$0"));
                Send(notification);
            }
        }

        /// <summary>
        /// Sends the request back to the requestor once it has been final reviewed.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="comments">The comments.</param>
        public void SendRequestFinalReviewed(TravelAndTrainingRequest request, string comments)
        {
            using (MailMessage notification = new MailMessage())
            {
                notification.Priority = MailPriority.High;
                notification.Subject = string.Format(Resources.RequestActedOnSubject, request.Status);
                notification.IsBodyHtml = true;
                Employee requestor = provider.GetEmployeeByNetworkID(request.Requestor);
                notification.To.Add(requestor.EmailAddress);
                notification.Body =
                    string.Format(Resources.RequestActedOnEmail,
                    ConfigurationManager.AppSettings["SubmittedRequestsLink"],                    
                    request.Status,                    
                    request.EventName,
                    request.EventStart.ToString("d"),
                    request.EventEnd.ToString("d"),
                    provider.GetEmployeeByNetworkID(request.FinalReviewedBy).FullName,
                    comments);
                Send(notification);
            }
        }

        private static void Send(MailMessage email)
        {
            SmtpClient client = new SmtpClient();
            client.Send(email);
        }
    }
}