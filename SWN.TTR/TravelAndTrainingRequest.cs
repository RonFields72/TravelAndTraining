#region Using Directives

using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using SWN.TTR.Common;
using SWN.TTR.Email;
using SWN.TTR.Estimates;
using SWN.TTR.Properties;
using SWN.TTR.Repository;
using SWN.TTR.Repository.Common;
using SWN.TTR.Repository.Dto;
using SWN.TTR.Security;
using SWN.TTR.Validation;
using System;
using System.Collections.Generic;
using System.Web;

#endregion

namespace SWN.TTR
{
    /// <summary>
    /// A travel and training request object.
    /// </summary>
    [HasSelfValidation()]
    public class TravelAndTrainingRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TravelAndTrainingRequest"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TravelAndTrainingRequest(System.Security.Principal.IPrincipal context)            
        {
            this.applicationContext = context;
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TravelAndTrainingRequest"/> class.
        /// </summary>
        internal TravelAndTrainingRequest()
        {
            // Used by the repository to create a new instance          
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.eventName = "B";
            this.eventCity = "";
            this.eventState = "-";
            this.businessPurpose = "";
            this.projectName = "";
            this.EventStart = BizObjectValidator.DEFAULT_DATETIME;
            this.EventEnd = BizObjectValidator.DEFAULT_DATETIME;
            this.Requestor = this.applicationContext.Identity.Name;
            this.RequestedOn = DateTime.Now;
            this.Status = RequestStatus.New;
            this.PendingReviewBy = "";
            this.FinalReviewedOn = BizObjectValidator.DEFAULT_DATETIME;
            this.FinalReviewedBy = "";
            this.HasTraining = false;
            this.HasTravel = false;
            this.travel = new TravelEstimate();
            this.training = new TrainingEstimate();
            this.misceallaneousCostsComments = "";
            this.MisceallaneousCosts = BizObjectValidator.DEFAULT_COST;
        }

        #region Properties

        /// <summary>
        /// The logged user's application context.
        /// </summary>
        internal System.Security.Principal.IPrincipal applicationContext;

        private ITravelAndTrainingRequestRepository ttrProvider = DataAccessFactory.CreateTTRRepository();
        private IOdsRepository odsProvider = DataAccessFactory.CreateOdsRepository();

        public int Id { get; internal set; }

        private string eventName;

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>The name of the event.</value>
        [StringLengthValidator(0, 200,
            Ruleset = "Save",
            Tag = "event name",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [NotNullValidator(Ruleset = "Save")]
        [StringLengthValidator(1, RangeBoundaryType.Inclusive, 0, RangeBoundaryType.Ignore,
            Ruleset = "Save",
            Tag = "event name",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [StringLengthValidator(1, RangeBoundaryType.Inclusive, 0, RangeBoundaryType.Ignore,
            Ruleset = "Event",
            Tag = "event name",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public string EventName
        {
            get { return eventName; }
            set
            {
                eventName = value.NullToEmptyString();
            }
        }

        private string eventCity;

        /// <summary>
        /// Gets or sets the event city.
        /// </summary>
        /// <value>The event city.</value>
        [StringLengthValidator(0, 50,
            Ruleset = "Save",
            Tag = "event city",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [NotNullValidator(Ruleset = "Save")]
        [StringLengthValidator(1, RangeBoundaryType.Inclusive, 0, RangeBoundaryType.Ignore,
            Ruleset = "Event",
            Tag = "event city",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public string EventCity
        {
            get { return eventCity; }
            set { eventCity = value.NullToEmptyString(); }
        }

        private string eventState;

        /// <summary>
        /// Gets or sets event state.
        /// </summary>
        /// <value>The event state.</value>
        [StringLengthValidator(0, 2,
            Ruleset = "Save",
            Tag = "event state",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [NotNullValidator(Ruleset = "Save")]
        [StringLengthValidator(2, RangeBoundaryType.Inclusive, 2, RangeBoundaryType.Inclusive,
            Ruleset = "Event",
            Tag = "event state",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public string EventState
        {
            get { return eventState; }
            set { eventState = value.NullToEmptyString(); }
        }

        private string businessPurpose;

        /// <summary>
        /// Gets or sets the business purpose.
        /// </summary>
        /// <value>The business purpose.</value>
        [StringLengthValidator(0, 100,
            Ruleset = "Save",
            Tag = "business purpose",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [NotNullValidator(Ruleset = "Save")]
        public string BusinessPurpose
        {
            get { return businessPurpose; }
            set { businessPurpose = value.NullToEmptyString(); }
        }

        private string projectName;

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        [StringLengthValidator(0, 50,
            Ruleset = "Save",
            Tag = "associated project",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [NotNullValidator(Ruleset = "Save")]
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value.NullToEmptyString(); }
        }

        /// <summary>
        /// Gets or sets the event start date/time.
        /// </summary>
        /// <value>The event start date/time.</value>
        [DateTimeRangeValidator(BizObjectValidator.MIN_DATETIME, RangeBoundaryType.Inclusive,
            BizObjectValidator.MAX_DATETIME, RangeBoundaryType.Inclusive,
            Ruleset = "Save",
            Tag = "event start date",
            MessageTemplateResourceName = "InvalidDateRangeMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [RelativeDateTimeValidator(-1, DateTimeUnit.Day, RangeBoundaryType.Inclusive,
            0, DateTimeUnit.Day, RangeBoundaryType.Ignore,
            Ruleset = "Event",
            Tag = "event start date",
            MessageTemplateResourceName = "DateGreaterEqualThanTodayMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public DateTime EventStart { get; set; }

        /// <summary>
        /// Gets or sets the event end date/time.
        /// </summary>
        /// <value>The event end date/time.</value>
        [DateTimeRangeValidator(BizObjectValidator.MIN_DATETIME, RangeBoundaryType.Inclusive,
            BizObjectValidator.MAX_DATETIME, RangeBoundaryType.Inclusive,
            Ruleset = "Save",
            Tag = "event end date",
            MessageTemplateResourceName = "InvalidDateRangeMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [PropertyComparisonValidator("EventStart", ComparisonOperator.GreaterThanEqual,
            Ruleset = "Event",
            Tag = "event end date",
            MessageTemplateResourceName = "EndDateInvalidMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public DateTime EventEnd { get; set; }

        /// <summary>
        /// Gets or sets the requestor.
        /// </summary>
        /// <remarks>The format is [Domain Name]\[Identity].</remarks>
        /// <value>The requestor.</value>
        [StringLengthValidator(0, 25,
            Ruleset = "Save",
            Tag = "requestor",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public string Requestor { get; internal set; }

        /// <summary>
        /// Gets or sets the date the request was initiated.
        /// </summary>
        /// <value>The requested the date the request was initiated.</value>
        [DateTimeRangeValidator(BizObjectValidator.MIN_DATETIME, RangeBoundaryType.Inclusive,
            BizObjectValidator.MAX_DATETIME, RangeBoundaryType.Inclusive,
            Ruleset = "Save",
            Tag = "requested date",
            MessageTemplateResourceName = "InvalidDateRangeMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public DateTime RequestedOn { get; internal set; }

        /// <summary>
        /// Gets or sets the status of the request..
        /// </summary>
        /// <value>The request status.</value>
        public RequestStatus Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this request has training.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this request has training; otherwise, <c>false</c>.
        /// </value>
        public bool HasTraining { get; set; }

        private TrainingEstimate training;

        /// <summary>
        /// Gets or sets the training estimates.
        /// </summary>
        /// <value>The training estimates.</value>
        public TrainingEstimate Training
        {
            get
            {
                return training;
            }
            set { training = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this request has travel.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this request has travel; otherwise, <c>false</c>.
        /// </value>
        public bool HasTravel { get; set; }

        private TravelEstimate travel;

        /// <summary>
        /// Gets or sets the travel estimates.
        /// </summary>
        /// <value>The travel estimates.</value>
        public TravelEstimate Travel
        {
            get
            {
                return travel;
            }
            set { travel = value; }
        }

        private string misceallaneousCostsComments;

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        [StringLengthValidator(0, 200,
            Ruleset = "Save",
            Tag = "additional comments",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public string MiscealleanousCostsComments
        {
            get { return misceallaneousCostsComments; }
            set { misceallaneousCostsComments = value.NullToEmptyString(); }
        }

        /// <summary>
        /// Gets or sets the misceallaneous costs.
        /// </summary>
        /// <value>The misceallaneous costs.</value>
        public decimal MisceallaneousCosts { get; set; }

        /// <summary>
        /// Gets or sets the person assigned to review the request.
        /// </summary>
        /// <remarks>The format is [Domain Name]\[Identity].</remarks>
        /// <value>The pperson assigned to review the request.</value>
        [StringLengthValidator(0, 25,
            Ruleset = "Save",
            Tag = "reviewed by",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public string PendingReviewBy { get; internal set; }

        /// <summary>
        /// Gets or sets the the date the request was final reviewed.
        /// </summary>
        /// <value>The final reviewed date/time.</value>
        public DateTime FinalReviewedOn { get; internal set; }

        /// <summary>
        /// Gets or sets the person who final reviewed the request..
        /// </summary>
        /// <remarks>The format is [Domain Name]\[Identity].</remarks>
        /// <value>The final reviewed by person.</value>
        [StringLengthValidator(0, 25,
            Ruleset = "Save",
            Tag = "pending review by",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public string FinalReviewedBy { get; internal set; }

        /// <summary>
        /// Gets the total cost of the request.
        /// </summary>
        /// <value>The total cost.</value>
        public decimal TotalCost
        {
            get
            {
                return this.training.TotalCost +
                    this.travel.TotalCost +
                    this.MisceallaneousCosts;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this request can be saved.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this request can be saved; otherwise, <c>false</c>.
        /// </value>        
        public bool CanBeSaved
        {
            get
            {
                if (this.Status == RequestStatus.New
                        && string.CompareOrdinal(this.Requestor, this.applicationContext.Identity.Name) == 0)
                    return true;
                else if (CanBeReviewed)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this request can be reviewed.  In order to be able to 
        /// review a request, it must be:
        /// <list type="bullet">
        /// <item>Pending review by the currently logged user and in a PendingApproval state.</item>
        /// </list>
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this request can be reviewed; otherwise, <c>false</c>.
        /// </value>
        public bool CanBeReviewed
        {
            get
            {
                if (string.CompareOrdinal(this.applicationContext.Identity.Name, this.PendingReviewBy) == 0 &&
                      this.Status == RequestStatus.PendingApproval)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this request can be deleted.  A request can be deleted if
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this request can be deleted; otherwise, <c>false</c>.
        /// </value>
        public bool CanBeDeleted
        {
            get
            {
                if (string.CompareOrdinal(this.applicationContext.Identity.Name, this.Requestor) == 0 &&
                    this.Status == RequestStatus.New &&
                    this.Id > 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region Validation

        [SelfValidation(Ruleset = "Save")]
        public void DoValidateSave(ValidationResults results)
        {
            results.AddAllResults(
                Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<TrainingEstimate>(this.Training, "Save"));
            results.AddAllResults(
                Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<TravelEstimate>(this.Travel, "Save"));
        }

        [SelfValidation(Ruleset = "Event")]
        public void DoValidateEvent(ValidationResults results)
        {
            // Have to choose either travel or training
            if (!this.HasTraining && !this.HasTravel)
            {
                results.AddResult(new ValidationResult("You must enter either travel or training",
                    typeof(TravelAndTrainingRequest), "", "", null));
            }
        }

        [SelfValidation(Ruleset = "Estimates")]
        public void DoValidateEstimates(ValidationResults results)
        {
            if (this.HasTraining)
            {
                results.AddAllResults(
                    Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<TrainingEstimate>(this.Training, "Estimates"));
                if (!this.Training.StartingDate.IsBetween(this.EventStart, this.EventEnd, true))
                {
                    results.AddResult(new ValidationResult(
                           "The start date is not valid.",
                           typeof(TrainingEstimate), "", "training start date", null));
                }
                if (!this.Training.EndingDate.IsBetween(this.EventStart, this.EventEnd, true))
                {
                    results.AddResult(new ValidationResult(
                           "The end date is not valid",
                           typeof(TrainingEstimate), "", "training start date", null));
                }
            }
            if (this.HasTravel)
            {
                results.AddAllResults(
                    Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<TravelEstimate>(this.Travel, "Estimates"));
            }

            if (this.MiscealleanousCostsComments.Length > 0 && this.MisceallaneousCosts <= 0M)
            {
                results.AddResult(new ValidationResult("General comments estimated cost is required.",
                        typeof(TravelAndTrainingRequest),
                        "", "", null));
            }
            else if (this.MisceallaneousCosts != 0M &&
                this.MiscealleanousCostsComments.Length == 0)
            {
                results.AddResult(new ValidationResult("General comments are required.",
                        typeof(TravelAndTrainingRequest),
                        "", "", null));
            }            
        }

        //[SelfValidation(Ruleset = "Submit")]
        //public void DoValidateSubmit(ValidationResults results)
        //{
        //    if (this.MiscealleanousCostsComments.Length > 0 && this.MisceallaneousCosts <= 0M)
        //    {
        //        results.AddResult(new ValidationResult("Additional cost is required.",
        //                typeof(TravelAndTrainingRequest),
        //                "", "", null));
        //    }
        //    else if (this.MisceallaneousCosts != 0M &&
        //        this.MiscealleanousCostsComments.Length == 0)
        //    {
        //        results.AddResult(new ValidationResult("Additional cost comments are required.",
        //                typeof(TravelAndTrainingRequest),
        //                "", "", null));
        //    }
        //    // Have to choose either travel or training
        //    if (!this.HasTraining && !this.HasTravel)
        //    {
        //        results.AddResult(new ValidationResult("You must enter either travel or training",
        //            typeof(TravelAndTrainingRequest), "", "", null));
        //    }
        //    else
        //    {
        //        if (this.HasTraining)
        //        {
        //            results.AddAllResults(
        //                Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<TrainingEstimate>(this.Training, "Submit"));
        //            if (!this.Training.StartingDate.IsBetween(this.EventStart, this.EventEnd, true))
        //            {
        //                results.AddResult(new ValidationResult(
        //                       "The training start date must be within the event dates.",
        //                       typeof(TrainingEstimate), "", "training start date", null));
        //            }
        //            if (!this.Training.EndingDate.IsBetween(this.EventStart, this.EventEnd, true))
        //            {
        //                results.AddResult(new ValidationResult(
        //                       "The training end date must be within the event dates.",
        //                       typeof(TrainingEstimate), "", "training start date", null));
        //            }
        //        }
        //        if (this.HasTravel)
        //        {
        //            results.AddAllResults(
        //                Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<TravelEstimate>(this.Travel, "Submit"));
        //        }
        //    }
        //}

        #endregion

        #region Methods

        /// <summary>
        /// Deletes this request.
        /// </summary>
        public void Delete()
        {
            RequestOfficer officer = new RequestOfficer(this.applicationContext);
            if (officer.CanDelete())
            {
                if (this.CanBeDeleted)
                    ttrProvider.Remove(this.Id);
                else
                    throw new System.InvalidOperationException(Resources.DeleteNotAllowedException);
            }
            else
                throw new System.Security.SecurityException(Resources.DeleteNotAuthorizedException);
        }

        /// <summary>
        /// Saves this request.
        /// </summary>
        public void Save()
        {
            RequestOfficer officer = new RequestOfficer(this.applicationContext);
            if (officer.CanSave())
            {
                if (this.CanBeSaved)
                    ttrProvider.Save(this);
                else
                    throw new System.InvalidOperationException(Resources.SaveNotAllowedException);
            }
            else
                throw new System.Security.SecurityException(Resources.SaveNotAuthorizedException);

        }

        /// <summary>
        /// Gets all of the final approvers.
        /// </summary>
        /// <returns>The final approvers.</returns>
        public List<Employee> GetFinalApprovers()
        {
            return this.odsProvider.GetEmployeesByRole(System.Configuration.ConfigurationManager.AppSettings["MenuSystem"],
                System.Configuration.ConfigurationManager.AppSettings["MenuSubSystem"],
                "FINAL");
        }

        /// <summary>
        /// Approves this request.
        /// </summary>
        public void Approve()
        {
            this.Approve("");
        }

        /// <summary>
        /// Approves this request with comments. The request must be reviewable prior to approval.
        /// </summary>
        /// <param name="comments">The comments.</param>
        public void Approve(string comments)
        {
            RequestOfficer officer = new RequestOfficer(this.applicationContext);
            if (officer.CanApprove())
            {
                if (CanBeReviewed)
                    AddApprovalAudit(ApprovalStatus.Approved, comments);
                else
                    throw new System.InvalidOperationException(Resources.ApproveNotAllowedException);
            }
            else
                throw new System.Security.SecurityException(Resources.ApproveNotAuthorizedException);
        }

        /// <summary>
        /// Final approves this request.
        /// </summary>
        public void FinalApprove()
        {
            this.FinalApprove("");
        }

        /// <summary>
        /// Final approves this request with comments.
        /// </summary>
        /// <param name="comments">The comments.</param>
        public void FinalApprove(string comments)
        {
            // if you have the authority and it can be reviewed, then final approve
            RequestOfficer officer = new RequestOfficer(this.applicationContext);
            if (officer.CanApprove())
            {
                if (CanBeReviewed)
                {
                    this.Status = RequestStatus.Approved;
                    this.FinalReviewedBy = this.applicationContext.Identity.Name;
                    this.FinalReviewedOn = DateTime.Now;
                    this.PendingReviewBy = "";                    
                    ttrProvider.Save(this);
                    // audit the approval
                    AddApprovalAudit(ApprovalStatus.FinalApproved, comments);
                    // send out an email to the requestor
                    RequestNotifier notifier = new RequestNotifier();
                    notifier.SendRequestFinalReviewed(this, comments);
                }
                else
                    throw new System.InvalidOperationException(Resources.FinalApproveNotAllowedException);
            }
            else
                throw new System.Security.SecurityException(Resources.FinalApproveNotAuthorizedException);
        }

        /// <summary>
        /// Denies this request with comments.
        /// </summary>
        /// <param name="comments">The comments.</param>
        public void Deny(string comments)
        {
            // if you have the authority and it can be reviewed, then deny
            RequestOfficer officer = new RequestOfficer(this.applicationContext);
            if (officer.CanDeny())
            {
                if (CanBeReviewed)
                {
                    // comments are required for denial
                    if (string.IsNullOrEmpty(comments))
                        throw new System.InvalidOperationException(Resources.CommentsMissingForDenyException);
                    else
                    {
                        this.PendingReviewBy = "";
                        this.Status = RequestStatus.Denied;
                        this.FinalReviewedBy = this.applicationContext.Identity.Name;
                        this.FinalReviewedOn = DateTime.Now;
                        ttrProvider.Save(this);
                        // audit the reviewal
                        AddApprovalAudit(ApprovalStatus.Denied, comments);
                        // notify the requestor
                        RequestNotifier notifier = new RequestNotifier();
                        notifier.SendRequestFinalReviewed(this, comments);
                    }
                }
                else
                    throw new System.InvalidOperationException(Resources.DenyNotAllowedException);
            }
            else
                throw new System.Security.SecurityException(Resources.DenyNotAuthorizedException);
        }

        /// <summary>
        /// Routes the request to the specified recipient. A request can be routed if:
        /// <list type="bullet">
        /// <item>The status is New.</item>
        /// <item>The status is pending approval and the recipient is a final approver.</item>
        /// <item>The proper rights are granted to the logged user.</item>
        /// </list>
        /// </summary>
        /// <param name="recipient">The recipient of the request.</param>
        public void RouteTo(System.Net.Mail.MailAddress recipient)
        {           
            RequestOfficer officer = new RequestOfficer(this.applicationContext);
            if (officer.CanRoute())
            {
                switch (this.Status)
                {
                    case RequestStatus.New:
                        this.Status = RequestStatus.PendingApproval;
                        this.Route(recipient);
                        break;
                    case RequestStatus.PendingApproval:
                        if (this.CanBeReviewed)
                        {
                            if (officer.IsUserFinalApprover() && !IsRecipientFinalApprover(recipient))
                                throw new System.InvalidOperationException(Resources.RouteToNonApproverException);
                            else
                                this.Route(recipient);
                        }
                        else
                            throw new System.InvalidOperationException(Resources.RouteNotAuthorizedException);                        
                        break;
                    default:
                        throw new System.InvalidOperationException(string.Format(Resources.RouteNotAllowedException, this.Status));
                }
            }
            else
                throw new System.Security.SecurityException(Resources.RouteNotAuthorizedException);
        }

        private void Route(System.Net.Mail.MailAddress recipient)
        {
            this.PendingReviewBy = 
                this.odsProvider.GetEmployeeByWorkEmailAddress(recipient.Address).NetworkId;
            ttrProvider.Save(this);
            AddApprovalAudit(ApprovalStatus.Routed, "");
            RequestNotifier pendingApprovalNotifier = new RequestNotifier();
            pendingApprovalNotifier.SendForReview(this, recipient);
        }

        private void AddApprovalAudit(ApprovalStatus status, string comments)
        {
            Approval approval = new Approval();
            approval.Status = status;
            approval.ReviewedBy = this.applicationContext.Identity.Name;
            approval.ReviewedOn = DateTime.Now;
            approval.Comments = comments;
            ttrProvider.AddApprovalAudit(this, approval);
        }

        private bool IsRecipientFinalApprover(System.Net.Mail.MailAddress recipient)
        {
            string recipientNetworkId = this.odsProvider.GetEmployeeByWorkEmailAddress(recipient.Address).NetworkId;
            RolesServices.RolesServiceSoapClient client =
                new RolesServices.RolesServiceSoapClient();
            client.ClientCredentials.Windows.AllowedImpersonationLevel =
                System.Security.Principal.TokenImpersonationLevel.Delegation;
            List<string> roles = new List<string>
                (client.GetRolesForUser(recipientNetworkId.Replace(@"SWN\", ""),
                System.Configuration.ConfigurationManager.AppSettings["MenuSystem"],
                System.Configuration.ConfigurationManager.AppSettings["MenuSubSystem"]));

            if (roles.Contains("FINAL"))
                return true;
            else
                return false;
        }

        #endregion
    }
}