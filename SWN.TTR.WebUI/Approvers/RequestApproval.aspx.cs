using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SWN.TTR.WebUI.Approvers
{
    public partial class RequestApproval : System.Web.UI.Page
    {
        protected readonly int REQUEST_ID = System.Convert.ToInt32(HttpContext.Current.Request.QueryString["id"]);
        private const int COMMENTS_LENGTH = 100;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                this.ucSummary.LoadRequestSummary(REQUEST_ID);
        }

        protected void LoadEditRequestLink(object sender, EventArgs e)
        {
            HyperLink lnk = (HyperLink)sender;
            lnk.NavigateUrl = string.Format("~/CreateNewRequest.aspx?id={0}", this.REQUEST_ID);
        }

        protected void ApproveRequest(object sender, EventArgs e)
        {
            try
            {
                if (this.IsCommentsValid)
                {
                    TravelAndTrainingRequestFinder requestFinder = new TravelAndTrainingRequestFinder(this.User);
                    TravelAndTrainingRequest request = requestFinder.GetRequestById(REQUEST_ID);
                    request.Approve(this.txtComments.Text);
                    this.litStatus.Text = "The request has been approved.";
                }
                else
                {
                    this.litError.Text = string.Format("Comments cannot be more than {0} characters.", COMMENTS_LENGTH);
                }                
            }
            catch (System.Security.SecurityException ex)
            {
                this.litError.Text = ex.Message;
            }
            catch (InvalidOperationException oex)
            {
                this.litError.Text = oex.Message;
            }
            finally
            {
                this.ucSummary.LoadRequestSummary(REQUEST_ID);
            }
        }

        protected void DenyRequest(object sender, EventArgs e)
        {
            try
            {
                if (this.IsCommentsValid)
                {
                    TravelAndTrainingRequestFinder requestFinder = new TravelAndTrainingRequestFinder(this.User);
                    TravelAndTrainingRequest request = requestFinder.GetRequestById(REQUEST_ID);
                    request.Deny(this.txtComments.Text);
                    this.litStatus.Text = "The request has been denied. You may now close your browser or go Home to view more requests.";
                }
                else
                {
                    this.litError.Text = string.Format("Comments cannot be more than {0} characters.", COMMENTS_LENGTH);
                }                
            }
            catch (System.Security.SecurityException ex)
            {
                this.litError.Text = ex.Message;
            }
            catch (InvalidOperationException oex)
            {
                this.litError.Text = oex.Message;
            }
            finally
            {
                this.ucSummary.LoadRequestSummary(REQUEST_ID);
            }
        }

        protected void FinalApproveRequest(object sender, EventArgs e)
        {
            try
            {
                if (this.IsCommentsValid)
                {
                    TravelAndTrainingRequestFinder requestFinder = new TravelAndTrainingRequestFinder(this.User);
                    TravelAndTrainingRequest request = requestFinder.GetRequestById(REQUEST_ID);
                    request.FinalApprove(this.txtComments.Text);
                    this.litStatus.Text = "The request has been final approved. You may now close your browser or go Home to view more requests.";
                }
                else
                {
                    this.litError.Text = string.Format("Comments cannot be more than {0} characters.", COMMENTS_LENGTH);
                }                
            }
            catch (System.Security.SecurityException ex)
            {
                this.litError.Text = ex.Message;
            }
            catch (InvalidOperationException oex)
            {
                this.litError.Text = oex.Message;
            }
            finally
            {
                this.ucSummary.LoadRequestSummary(REQUEST_ID);
            }
        }

        protected void Route(object sender, EventArgs e)
        {
            try
            {
                TravelAndTrainingRequestFinder requestFinder = new TravelAndTrainingRequestFinder(this.User);
                TravelAndTrainingRequest request = requestFinder.GetRequestById(REQUEST_ID);
                request.RouteTo(new System.Net.Mail.MailAddress(this.ddlApprovers.SelectedValue));
                this.litStatus.Text = "The request has been routed. You may now close your browser or go Home to view more requests.";
            }
            catch (System.Security.SecurityException ex)
            {
                this.litError.Text = ex.Message;
            }
            catch (InvalidOperationException oex)
            {
                this.litError.Text = oex.Message;
            }
            finally
            {
                this.ucSummary.LoadRequestSummary(REQUEST_ID);
            }
        }

        protected void LoadApprovers(object sender, EventArgs e)
        {
            TravelAndTrainingRequest request = new TravelAndTrainingRequest(this.User);
            this.ddlApprovers.DataSource = request.GetFinalApprovers();
            this.ddlApprovers.DataBind();
        }

        // TODO: this should be done on an entity
        protected bool IsCommentsValid
        {
            get
            {
                return this.txtComments.Text.Length <= COMMENTS_LENGTH;
            }
        }
    }    
}
