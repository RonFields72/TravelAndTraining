using SWN.TTR;
using SWN.TTR.Security;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web;

namespace SWN.TTR.WebUI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Main : System.Web.UI.Page
    {
        private readonly string REQUEST_VIEW_QUERYSTRING = HttpContext.Current.Request.QueryString["v"];
        private TravelAndTrainingRequestFinder requestFinder = new TravelAndTrainingRequestFinder(HttpContext.Current.User);
        private RequestOfficer officer = new RequestOfficer(HttpContext.Current.User);
        string NetworkID = System.Environment.UserDomainName + "\\" + System.Environment.UserName;

        protected void CheckViewAllVisibility(object sender, EventArgs e)
        {
            HyperLink lnk = (HyperLink)sender;            
            if (officer.CanViewAllRequests())
                lnk.Visible = true;
            else
                lnk.Visible = false;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (REQUEST_VIEW_QUERYSTRING)
            {
                case "mypen":
                    this.vwRequests.ActiveViewIndex = 1;
                    this.gvMyPendingRequests.DataSource = requestFinder.GetMyPendingRequests();
                    this.gvMyPendingRequests.DataBind();
                    break;
                case "all":                    
                    if (officer.CanViewAllRequests())
                    {                        
                        this.vwRequests.ActiveViewIndex = 2;
                        this.gvAllRequests.DataSource = requestFinder.GetAllRequests();
                        this.gvAllRequests.DataBind();
                    }
                    else
                    {                     
                        this.vwRequests.ActiveViewIndex = 1;
                        this.gvMyPendingRequests.DataSource = requestFinder.GetMyPendingRequests();
                        this.gvMyPendingRequests.DataBind();
                    }
                    break;
                case "my":
                    this.vwRequests.ActiveViewIndex = 0;
                    this.gvMyRequests.DataSource = requestFinder.GetMyRequests();
                    this.gvMyRequests.DataBind();
                    break;

                case "myapp":
                    this.vwRequests.ActiveViewIndex = 3;
                   
                    this.gvApprovedRequests.DataSource = requestFinder.GetDataByApprover(NetworkID);
                    
                    this.gvApprovedRequests.DataBind();
                    break;

                default:
                    this.vwRequests.ActiveViewIndex = 1;
                    this.gvMyPendingRequests.DataSource = requestFinder.GetMyPendingRequests();
                    this.gvMyPendingRequests.DataBind();
                    break;
            }
        }

        /// <summary>
        /// Handles the Sorting event of the gvMyRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gvMyRequests_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            this.gvMyRequests.DataSource = this.SortDataTable(requestFinder.GetMyRequests() as DataTable, false);
            this.gvMyRequests.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the gvMyPendingRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gvMyPendingRequests_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            this.gvMyPendingRequests.DataSource = this.SortDataTable(requestFinder.GetMyPendingRequests() as DataTable, false);
            this.gvMyPendingRequests.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the gvAllRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gvAllRequests_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            this.gvAllRequests.DataSource = this.SortDataTable(requestFinder.GetAllRequests() as DataTable, false);
            this.gvAllRequests.DataBind();
        }
        protected void gvApprovedRequests_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            this.gvApprovedRequests.DataSource = this.SortDataTable(requestFinder.GetDataByApprover(NetworkID) as DataTable, false);
            this.gvAllRequests.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvMyRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvMyRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvMyRequests.DataSource = this.SortDataTable(requestFinder.GetMyRequests(), true);
            this.gvMyRequests.PageIndex = e.NewPageIndex;
            this.gvMyRequests.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvMyPendingRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvMyPendingRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvMyPendingRequests.DataSource = this.SortDataTable(requestFinder.GetMyPendingRequests(), true);
            this.gvMyPendingRequests.PageIndex = e.NewPageIndex;
            this.gvMyPendingRequests.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvAllRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvAllRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvAllRequests.DataSource = this.SortDataTable(requestFinder.GetAllRequests(), true);
            this.gvAllRequests.PageIndex = e.NewPageIndex;
            this.gvAllRequests.DataBind();
        }
        protected void gvApprovedRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvApprovedRequests.DataSource = this.SortDataTable(requestFinder.GetDataByApprover(NetworkID), true);
            this.gvApprovedRequests.PageIndex = e.NewPageIndex;
            this.gvApprovedRequests.DataBind();
        
        }

        /// <summary>
        /// Handles the RowDataBound event of the gvMyPendingRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvMyPendingRequests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView view = (DataRowView)e.Row.DataItem;
                HyperLink editLink = (HyperLink)e.Row.FindControl("lnkEdit");
                int id = System.Convert.ToInt32(view["Id"]);
                if (!User.IsInRole("FINAL"))
                    editLink.NavigateUrl = string.Format("~/CreateNewRequest.aspx?id={0}", id);
                else
                    editLink.NavigateUrl = string.Format("~/Approvers/RequestApproval.aspx?id={0}", id);                                 
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the gvMyRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvMyRequests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView view = (DataRowView)e.Row.DataItem;
                HyperLink editLink = (HyperLink)e.Row.FindControl("lnkEdit");
                int id = System.Convert.ToInt32(view["Id"]);
                if (view["Status"].ToString() == "NEW")
                    editLink.NavigateUrl = string.Format("~/CreateNewRequest.aspx?id={0}", id);
                else
                    editLink.NavigateUrl = string.Format("~/RequestSummary.aspx?id={0}", id);
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the gvAllRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvAllRequests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView view = (DataRowView)e.Row.DataItem;
                HyperLink editLink = (HyperLink)e.Row.FindControl("lnkEdit");
                int id = System.Convert.ToInt32(view["Id"]);
                editLink.NavigateUrl = string.Format("~/RequestSummary.aspx?id={0}", id);
            }
        }

        protected void gvApprovedRequests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView view = (DataRowView)e.Row.DataItem;
                HyperLink editLink = (HyperLink)e.Row.FindControl("lnkEdit");
                int id = System.Convert.ToInt32(view["Id"]);
                editLink.NavigateUrl = string.Format("~/RequestSummary.aspx?id={0}", id);
            }
        }

        #region Sorting

        /// <summary>
        /// Gets or sets the grid view sort direction.
        /// </summary>
        /// <value>The grid view sort direction.</value>
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        /// Gets or sets the grid view sort expression.
        /// </summary>
        /// <value>The grid view sort expression.</value>
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }

        /// <summary>
        /// Gets the sort direction.
        /// </summary>
        /// <returns></returns>
        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";
                    break;
                case "DESC":
                    GridViewSortDirection = "ASC";
                    break;
            }
            return GridViewSortDirection;
        }

        /// <summary>
        /// Sorts the data table.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="isPageIndexChanging">if set to <c>true</c> [is page index changing].</param>
        /// <returns></returns>
        protected DataView SortDataTable(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dv = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                        dv.Sort = String.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                    else
                        dv.Sort = String.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                }
                return dv;
            }
            else
                return new DataView();
        }

        #endregion 

       
       
    }
}