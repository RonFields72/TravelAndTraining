using SWN.TTR.Common;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SWN.TTR.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Security;
using SWN.TTR.Security;


namespace SWN.TTR.WebUI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CreateNewRequest : System.Web.UI.Page
    {          
        private bool requestIsLoaded = false;
        private List<string> packingErrors = new List<string>();

        protected int RequestId
        {
            get
            {
                return System.Convert.ToInt32(this.ViewState["REQUEST"]);
            }
            set
            {
                this.ViewState["REQUEST"] = value;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!Page.IsPostBack)
            {
                this.RequestId = System.Convert.ToInt32(HttpContext.Current.Request.QueryString["id"]);
                if (this.RequestId != 0)
                {
                    TravelAndTrainingRequestFinder requestFinder = new TravelAndTrainingRequestFinder(this.User);
                    TravelAndTrainingRequest request = requestFinder.GetRequestById(this.RequestId);

                    // adamsb 2/27/09 Check status for routing
                    if (request != null)
                    {
                        if (request.CanBeReviewed)
                        {
                            RequestOfficer securityOfficer = new RequestOfficer(this.User);
                            if (securityOfficer.CanApprove())
                                Response.Redirect(string.Format("~/Approvers/RequestApproval.aspx?id={0}",
                                    request.Id), false);
                            else
                            {
                                // if not a final approver, 
                                // load the request, set the active step to the confirmation page
                                // and load the summary
                                this.UnpackRequest(request);
                                requestIsLoaded = true;
                                this.wzCreateRequest.ActiveStepIndex = 2;
                                this.LoadSummary(request);
                            }
                        }
                        else if (request.CanBeSaved)
                        {
                            // if not a final approver, 
                            // load the request, set the active step to the confirmation page
                            // and load the summary
                            this.UnpackRequest(request);
                            requestIsLoaded = true;
                            this.wzCreateRequest.ActiveStepIndex = 2;
                            this.LoadSummary(request);
                        }
                        else
                            Response.Redirect("~/Main.aspx", false);
                    }            
                    //if (request != null && request.CanBeSaved)
                    //{
                    //    this.UnpackRequest(request);
                    //    // if its pending review by someone, then load the summary page
                    //    if (request.PendingReviewBy.Length > 0)
                    //    {
                    //        requestIsLoaded = true;
                    //        this.wzCreateRequest.ActiveStepIndex = 2;
                    //        this.LoadSummary(request);
                    //    }
                    //}
                    //else
                    //    Response.Redirect("~/Main.aspx", false);                   
                }                
            }
        }

        #region Modal Popup

        protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
        {
            this.programmaticModalPopup.Hide();
        }

        #endregion

        #region DataBinding

        protected void LoadEventStates(object sender, EventArgs e)
        {
            StateServices.StateServiceSoapClient client =
                new SWN.TTR.WebUI.StateServices.StateServiceSoapClient();
            client.ClientCredentials.Windows.AllowedImpersonationLevel =
               System.Security.Principal.TokenImpersonationLevel.Delegation;
            DropDownList ddl = (DropDownList)sender;
            ddl.DataSource = client.GetAll();
            ddl.DataBind();
        }

        protected void AddDefaultEventState(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl != null)
            {
                ListItem li = ddl.Items.FindByValue("-");
                if (li != null)
                    ddl.Items.Remove(li);
                ddl.Items.Insert(0, new ListItem("[Choose a State]", "-"));
                
            }
        }

        protected void AddDefaultTravelState(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl != null)
            {
                ListItem li = ddl.Items.FindByValue("-");
                if (li != null)
                    ddl.Items.Remove(li);
                ddl.Items.Insert(0, new ListItem("[Choose a State]", "-"));
                
            }
        }

        protected void AddDefaultDestinationAirMode(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl != null)
            {
                ListItem li = ddl.Items.FindByValue("NONE");
                if (li != null)
                    ddl.Items.Remove(li);
                ddl.Items.Insert(0, new ListItem("[Choose a Mode]", "NONE"));
            }
        }

        protected void AddDefaultReturnAirMode(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl != null)
            {
                ListItem li = ddl.Items.FindByValue("NONE");
                if (li != null)
                    ddl.Items.Remove(li);
                ddl.Items.Insert(0, new ListItem("[Choose a Mode]", "NONE"));
            }
        }

        protected void LoadTravelStates(object sender, EventArgs e)
        {
            StateServices.StateServiceSoapClient client =
                new SWN.TTR.WebUI.StateServices.StateServiceSoapClient();
            client.ClientCredentials.Windows.AllowedImpersonationLevel =
               System.Security.Principal.TokenImpersonationLevel.Delegation;
            DropDownList ddl = (DropDownList)sender;
            ddl.DataSource = client.GetAll();
            ddl.DataBind();
        }

        protected void LoadDestinationAirModes(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ddl.DataSource = this.sqldsAirMode;
            ddl.DataBind();
        }

        protected void LoadReturnAirModes(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ddl.DataSource = this.sqldsAirMode;
            ddl.DataBind();
        }

        protected void LoadGroundModes(object sender, EventArgs e)
        {
            CheckBoxList ddl = (CheckBoxList)sender;
            ddl.DataSource = this.sqldsGroundModes;
            ddl.DataBind();
        }

        protected void RemoveDefaultGroundMode(object sender, EventArgs e)
        {
            CheckBoxList ckb = (CheckBoxList)sender;
            if (ckb != null)
            {
                ListItem li = ckb.Items.FindByValue("NONE");
                if (li != null)
                    ckb.Items.Remove(li);                
            }
        }

        protected void LoadEmployees(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            OdsFinder odsFinder = new OdsFinder();
            ddl.DataSource = odsFinder.GetActiveEmployees();
            ddl.DataBind();
        }

        protected void AddDefaultEmployee(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl != null)
            {
                ListItem li = ddl.Items.FindByValue("-");
                if (li != null)
                    ddl.Items.Remove(li);
                ddl.Items.Insert(0, new ListItem("[Choose a Reviewer]", "-"));
            }
        }

        #endregion

        #region Request Actions

        protected void SaveRequest(object sender, EventArgs e)
        {
            TravelAndTrainingRequest request = PackRequest();            
            ValidationResults results =
                Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<TravelAndTrainingRequest>(request, "Save");
            foreach (string packingError in this.packingErrors)
                results.AddResult(new ValidationResult(packingError, null, "", "", null));
            if (results.IsValid)
            {
                request.Save();
                // once it's saved, persist that ID to the viewstate to allow them to continue saving
                this.RequestId = request.Id;
                this.litStatus.Text = Resources.Resources.RequestSaved;
                if (this.wzCreateRequest.ActiveStepIndex == 2) this.LoadSummary(request);
            }
            else
            {
                foreach(ValidationResult result in results)
                    this.blstErrors.Items.Add(result.Message);
                this.programmaticModalPopup.Show();
            }
        }

        protected void SubmitRequest(object sender, EventArgs e)
        {
            if (this.txtReviewers.Text.Length == 0)
            {
                this.blstErrors.Items.Add(Resources.Resources.ReviewerRequired);
                this.programmaticModalPopup.Show();
            }
            else
            {
                OdsFinder odsFinder = new OdsFinder();
                SWN.TTR.Repository.Dto.Employee reviewer = odsFinder.GetEmployeeByFullName(this.txtReviewers.Text);
                if (reviewer == null)
                {
                    this.blstErrors.Items.Add(string.Format(Resources.Resources.ReviewerSelectedInvalid,
                        this.txtReviewers.Text));
                    this.programmaticModalPopup.Show();
                }
                else
                {
                    TravelAndTrainingRequest request = PackRequest();
                    ValidationResults results =
                        Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<TravelAndTrainingRequest>(request, "Save", "Event", "Estimates");
                    // add any UI validation that failed while gather the request object
                    foreach (string packingError in this.packingErrors)
                        results.AddResult(new ValidationResult(packingError, null, "", "", null));

                    if (results.IsValid)
                    {
                        try
                        {
                            request.RouteTo(new System.Net.Mail.MailAddress(reviewer.EmailAddress));
                            Response.Redirect("~/RequestConfirmation.aspx", false);
                        }
                        catch (InvalidOperationException ioex)
                        {
                            this.blstErrors.Items.Add(ioex.Message);
                            this.programmaticModalPopup.Show();
                        }
                    }
                    else
                    {
                        foreach (ValidationResult vr in results)
                            this.blstErrors.Items.Add(vr.Message);
                        this.programmaticModalPopup.Show();
                    }    

                }
            }

            
                    
        }

        protected void DeleteRequest(object sender, EventArgs e)
        {
            if (this.RequestId == 0)
            {
                this.blstErrors.Items.Add(Resources.Resources.SaveRequiredForDeletion);
                this.programmaticModalPopup.Show();
            }
            else
            {
                TravelAndTrainingRequest request = PackRequest();
                try
                {
                    request.Delete();
                    Response.Redirect("~/Main.aspx", false);
                }
                catch (SecurityException sex)
                {
                    this.blstErrors.Items.Add(sex.Message);
                    this.programmaticModalPopup.Show();
                }
                catch (InvalidOperationException iex)
                {
                    this.blstErrors.Items.Add(iex.Message);
                    this.programmaticModalPopup.Show();
                }                
            }   
        }

        #endregion

        protected void LoadTrainingPanel(object sender, EventArgs e)
        {
            CheckBox ckb = (CheckBox)sender;
            if (ckb.Checked)
                this.pnlTraining.Visible = true;
            else
            {
                this.pnlTraining.Visible = false;
                this.txtTrainEnd.Text = "";
                this.txtTrainName.Text = "";
                this.txtTrainStart.Text = "";                
                this.txtTrainCost.Text = "";
            }
        }

        protected void LoadTravelPanel(object sender, EventArgs e)
        {
            CheckBox ckb = (CheckBox)sender;
            if (ckb.Checked)
                this.pnlTravel.Visible = true;
            else
            {
                this.pnlTravel.Visible = false;
                this.txtTravelCity.Text = "";
                this.ddlTravelStates.SelectedIndex = -1;                
                this.ckbTravelAir.Checked = false;
                this.ckbTravelGround.Checked = false;
                this.ckbTravelLodging.Checked = false;
                
                this.ckbAirInteroffice.Checked = false;
                this.ddlAirDestinationMode.SelectedIndex = -1;
                this.ddlAirReturnMode.SelectedIndex = -1;
                this.txtAirCost.Text = "";

                this.txtLodgingCost.Text = "";
                this.txtLodgingNights.Text = "";

                foreach (ListItem groundMode in this.ckblstGroundModes.Items)
                    groundMode.Selected = false;
                this.txtGroundCost.Text = "";
            }
        }

        protected void AirTransportationChanged(object sender, EventArgs e)
        {
            CheckBox ckb = (CheckBox)sender;
            if (ckb.Checked)
                this.pnlAir.Visible = true;
            else
            {
                this.pnlAir.Visible = false;
                this.ckbAirInteroffice.Checked = false;
                this.ddlAirDestinationMode.SelectedIndex = -1;
                this.ddlAirReturnMode.SelectedIndex = -1;
                this.txtAirCost.Text = "";
            }
        }

        protected void GroundTransportationChanged(object sender, EventArgs e)
        {
            CheckBox ckb = (CheckBox)sender;
            if (ckb.Checked)
                this.pnlGround.Visible = true;
            else
            {
                this.pnlGround.Visible = false;
                foreach (ListItem groundMode in this.ckblstGroundModes.Items)
                    groundMode.Selected = false;
                this.txtGroundCost.Text = "";
            }
        }

        protected void LodgingChanged(object sender, EventArgs e)
        {
            CheckBox ckb = (CheckBox)sender;
            if (ckb.Checked)
                this.pnlLodging.Visible = true;
            else
            {
                this.pnlLodging.Visible = false;
                this.txtLodgingCost.Text = "";
                this.txtLodgingNights.Text = "";
            }
        }

        protected void LoadAirPanel(object sender, EventArgs e)
        {
            CheckBox ckb = (CheckBox)sender;
            if (ckb.Checked)
                this.pnlAir.Visible = true;
            else
                this.pnlAir.Visible = false;
        }

        protected void LoadGroundPanel(object sender, EventArgs e)
        {
            CheckBox ckb = (CheckBox)sender;
            if (ckb.Checked)
                this.pnlGround.Visible = true;
            else
                this.pnlGround.Visible = false;
        }

        protected void LoadLodgingPanel(object sender, EventArgs e)
        {
            CheckBox ckb = (CheckBox)sender;
            if (ckb.Checked)
                this.pnlLodging.Visible = true;
            else
                this.pnlLodging.Visible = false;
        }

        protected void LoadInterOffice(object sender, EventArgs e)
        {
            CheckBox ckb = (CheckBox)sender;
            if (ckb.Checked)
            {
                this.ddlAirDestinationMode.SelectedValue = "COMP";
                this.ddlAirReturnMode.SelectedValue = "COMP";
                this.ddlAirReturnMode.Enabled = false;
                this.ddlAirDestinationMode.Enabled = false;
                this.txtAirCost.Text = "1";
                this.txtAirCost.Visible = false;
            }
            else
            {
                this.ddlAirReturnMode.Enabled = true;
                this.ddlAirDestinationMode.Enabled = true;
                this.txtAirCost.Visible = true;
            }
        }

        protected void InterOfficeChanged(object sender, EventArgs e)
        {            
            CheckBox ckb = (CheckBox)sender;
            if (ckb.Checked)
            {
                this.ddlAirDestinationMode.SelectedValue = "COMP";
                this.ddlAirReturnMode.SelectedValue = "COMP";
                this.ddlAirReturnMode.Enabled = false;
                this.ddlAirDestinationMode.Enabled = false;
                this.txtAirCost.Text = "";
                this.txtAirCost.Visible = false;
            }
            else
            {
                this.ddlAirReturnMode.Enabled = true;
                this.ddlAirDestinationMode.Enabled = true;
                this.txtAirCost.Visible = true;
                this.ddlAirDestinationMode.SelectedIndex = -1;
                this.ddlAirReturnMode.SelectedIndex = -1;
            }
        }

        #region Wizard Events

        protected void wzCreateRequest_Init(object sender, EventArgs e)
        {
            // default the UI
            this.pnlAir.Visible = false;
            this.pnlGround.Visible = false;
            this.pnlLodging.Visible = false;
            this.pnlTraining.Visible = false;
            this.pnlTravel.Visible = false;
        }

        protected void wzCreateRequest_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            switch (e.NextStepIndex)
            {
                case 1:
                    // zuvichc 2/27/09 Validate Event information before next step
                    TravelAndTrainingRequest request = PackRequest();
                    ValidationResults results =
                        Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<TravelAndTrainingRequest>(request, "Event");
                    // add any UI validation that failed while gather the request object
                    foreach (string packingError in this.packingErrors)
                        results.AddResult(new ValidationResult(packingError, null, "", "", null));

                    if (!results.IsValid)
                    {
                        foreach (ValidationResult vr in results)
                            this.blstErrors.Items.Add(vr.Message);
                        this.programmaticModalPopup.Show();
                        e.Cancel = true;
                    }

                    if (this.ckbTravel.Checked)
                    {
                        if (this.txtTravelCity.Text.Length == 0)
                            this.txtTravelCity.Text = this.txtEventCity.Text;
                        if (this.ddlTravelStates.SelectedIndex < 1)
                            this.ddlTravelStates.SelectedIndex = this.ddlEventStates.SelectedIndex;
                    }
                    
                    if (this.ckbTraining.Checked)
                    {
                        

                        if (this.txtTrainStart != this.txtEventStartDate)
                            this.txtTrainStart.Text = this.txtEventStartDate.Text;
                        if (this.txtTrainEnd != this.txtEventEndDate)
                            this.txtTrainEnd.Text = this.txtEventEndDate.Text;
                        if (this.txtTrainName.Text.Length == 0)
                            this.txtTrainName.Text = this.txtEvent.Text;
                    }

                    if (!Page.IsValid)
                    {
                        this.txtTrainStart.Text = this.txtEventStartDate.Text;
                    }
                    if (!Page.IsValid)
                    {
                        this.txtTrainEnd.Text = this.txtEventEndDate.Text;
                    }
                   
                    break;

                   

                case 2:
                    TravelAndTrainingRequest estimatesRequest = PackRequest();
                    ValidationResults estimateResults =
                        Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<TravelAndTrainingRequest>(estimatesRequest, "Estimates");
                    // add any UI validation that failed while gather the request object
                    foreach (string packingError in this.packingErrors)
                        estimateResults.AddResult(new ValidationResult(packingError, null, "", "", null));

                    if (!estimateResults.IsValid)
                    {
                        foreach (ValidationResult vr in estimateResults)
                            this.blstErrors.Items.Add(vr.Message);
                        this.programmaticModalPopup.Show();
                        e.Cancel = true;
                    }
                    break;
                default:
                    break;
            }
        }
       
    
       

        protected void wzCreateRequest_ActiveStepChanged(object sender, EventArgs e)
        {
            switch (this.wzCreateRequest.ActiveStepIndex)
            {                
                case 2:
                    if (!requestIsLoaded)
                        LoadSummary(PackRequest());
                    break;
                default:
                    break;
            }
        }

        protected void wzCreateRequest_PreviousButtonClick(object sender, EventArgs e)
        {
            this.wzCreateRequest.ActiveStepIndex -= 1;
        }

        #endregion

        #region Request Packing/Unpacking

        private void UnpackRequest(TravelAndTrainingRequest request)
        {
            this.txtEvent.Text = request.EventName;
            if (!request.EventStart.IsDefault())
                this.txtEventStartDate.Text = request.EventStart.ToShortDateString();
            if (!request.EventEnd.IsDefault())
                this.txtEventEndDate.Text = request.EventEnd.ToShortDateString();
            this.txtEventCity.Text = request.EventCity;
            this.ddlEventStates.SelectedValue = request.EventState;
            this.txtEventPurpose.Text = request.BusinessPurpose;
            this.txtEventProjects.Text = request.ProjectName;
            this.ckbTraining.Checked = request.HasTraining;
            this.ckbTravel.Checked = request.HasTravel;

            this.txtTrainName.Text = request.Training.TrainingName;
            if (!request.Training.StartingDate.IsDefault())
                this.txtTrainStart.Text = request.Training.StartingDate.ToShortDateString();
            if (!request.Training.EndingDate.IsDefault())
                this.txtTrainEnd.Text = request.Training.EndingDate.ToShortDateString();
            if(request.Training.TotalCost != BizObjectValidator.DEFAULT_COST)
                this.txtTrainCost.Text = request.Training.TotalCost.ToString("0");

            this.txtTravelCity.Text = request.Travel.DestinationCity;
            this.ddlTravelStates.SelectedValue = request.Travel.DestinationState;

            this.ckbTravelAir.Checked = request.Travel.HasFlightArrangements;
            this.ckbTravelGround.Checked = request.Travel.HasGroundArrangements;
            this.ckbTravelLodging.Checked = request.Travel.HasLodgingArrangements;

            this.ckbAirInteroffice.Checked = request.Travel.FlightArrangements.IsInterofficeOnly;
            this.ddlAirDestinationMode.SelectedValue = request.Travel.FlightArrangements.DestinationMethod;
            this.ddlAirReturnMode.SelectedValue = request.Travel.FlightArrangements.ReturnMethod;
            if(request.Travel.FlightArrangements.TotalCost != BizObjectValidator.DEFAULT_COST)
                this.txtAirCost.Text = request.Travel.FlightArrangements.TotalCost.ToString("0");

            if(request.Travel.LodgingArrangements.NumberOfNights > 0)
                this.txtLodgingNights.Text = request.Travel.LodgingArrangements.NumberOfNights.ToString();
            if(request.Travel.LodgingArrangements.TotalCost != BizObjectValidator.DEFAULT_COST)
                this.txtLodgingCost.Text = request.Travel.LodgingArrangements.TotalCost.ToString("0");

            foreach (string mode in request.Travel.GroundArrangements.Modes)
                this.ckblstGroundModes.Items.FindByValue(mode).Selected = true;
            if(request.Travel.GroundArrangements.TotalCost != BizObjectValidator.DEFAULT_COST)
                this.txtGroundCost.Text = request.Travel.GroundArrangements.TotalCost.ToString("0");

            this.txtMiscComments.Text = request.MiscealleanousCostsComments;
            if(request.MisceallaneousCosts != BizObjectValidator.DEFAULT_COST)
                this.txtMiscCost.Text = request.MisceallaneousCosts.ToString("0");
        }

        private TravelAndTrainingRequest PackRequest()
        {
            TravelAndTrainingRequest request = null;
            if (this.RequestId == 0)
                request = new TravelAndTrainingRequest(this.User);
            else
            {
                TravelAndTrainingRequestFinder requestFinder = new TravelAndTrainingRequestFinder(this.User);
                request = requestFinder.GetRequestById(this.RequestId);
            }
            
            request.EventName = this.txtEvent.Text;
            if (this.txtEventStartDate.Text.Length > 0)
            {
                try
                {
                    request.EventStart = DateTime.Parse(this.txtEventStartDate.Text);
                }
                catch (FormatException)
                {
                    request.EventStart = BizObjectValidator.DEFAULT_DATETIME;
                    this.packingErrors.Add("Event start date is not valid.");
                }
            }
            else
                request.EventStart = BizObjectValidator.DEFAULT_DATETIME;
            if (this.txtEventEndDate.Text.Length > 0)
            {
                try
                {
                    request.EventEnd = DateTime.Parse(this.txtEventEndDate.Text);
                }
                catch (FormatException)
                {
                    request.EventEnd = BizObjectValidator.DEFAULT_DATETIME;
                    this.packingErrors.Add("Event end date is not valid.");
                }
            }
            else
                request.EventEnd = BizObjectValidator.DEFAULT_DATETIME;
            request.EventCity = this.txtEventCity.Text;
            request.EventState = this.ddlEventStates.SelectedValue;
            request.BusinessPurpose = this.txtEventPurpose.Text;
            request.ProjectName = this.txtEventProjects.Text;

            if (this.ckbTraining.Checked)
                request.HasTraining = true;
            else
                request.HasTraining = false;            
            request.Training.TrainingName = this.txtTrainName.Text;
            if (this.txtTrainStart.Text.Length > 0)
            {
                try
                {
                    request.Training.StartingDate = DateTime.Parse(this.txtTrainStart.Text);
                }
                catch (FormatException)
                {                   
                    request.Training.StartingDate = BizObjectValidator.DEFAULT_DATETIME;
                    this.packingErrors.Add("Start date is not valid.");
                }
            }
            else
                request.Training.StartingDate = BizObjectValidator.DEFAULT_DATETIME;
            if (this.txtTrainEnd.Text.Length > 0)
            {
                try
                {
                    request.Training.EndingDate = DateTime.Parse(this.txtTrainEnd.Text);
                }
                catch (FormatException)
                {
                    request.Training.EndingDate = BizObjectValidator.DEFAULT_DATETIME;
                    this.packingErrors.Add("End date is not valid.");
                }
            }
            else
                request.Training.EndingDate = BizObjectValidator.DEFAULT_DATETIME;
            if (this.txtTrainCost.Text.Length > 0)
            {
                try
                {
                    request.Training.TotalCost = System.Convert.ToDecimal(this.txtTrainCost.Text);
                }
                catch (FormatException)
                {
                    request.Training.TotalCost = BizObjectValidator.DEFAULT_COST;
                    this.packingErrors.Add("Training estimated cost is not valid.");
                }
            }
            else
                request.Training.TotalCost = BizObjectValidator.DEFAULT_COST;      
            
            if (this.ckbTravel.Checked)
                request.HasTravel = true;
            else
                request.HasTravel = false;            
            request.Travel.DestinationCity = this.txtTravelCity.Text;
            request.Travel.DestinationState = this.ddlTravelStates.SelectedValue;            
            if (this.ckbTravelAir.Checked)
                request.Travel.HasFlightArrangements = true;
            else
                request.Travel.HasFlightArrangements = false;

            request.Travel.FlightArrangements.DestinationMethod = this.ddlAirDestinationMode.SelectedValue;
            request.Travel.FlightArrangements.ReturnMethod = this.ddlAirReturnMode.SelectedValue;

            if (this.txtAirCost.Text.Length > 0)
            {
                try
                {
                    request.Travel.FlightArrangements.TotalCost =
                        System.Convert.ToDecimal(this.txtAirCost.Text);
                }
                catch (FormatException)
                {
                    request.Travel.FlightArrangements.TotalCost = BizObjectValidator.DEFAULT_COST;
                    this.packingErrors.Add("Flight estimated cost is not valid.");
                }
            }
            else
                request.Travel.FlightArrangements.TotalCost = BizObjectValidator.DEFAULT_COST;            
            
            if (this.ckbTravelGround.Checked)
                request.Travel.HasGroundArrangements = true;
            else
                request.Travel.HasGroundArrangements = false;

            request.Travel.GroundArrangements.Modes.Clear();
            foreach (ListItem groundMode in this.ckblstGroundModes.Items)
                if(groundMode.Selected)
                    request.Travel.GroundArrangements.Modes.Add(groundMode.Value);

            if (this.txtGroundCost.Text.Length > 0)
            {
                try
                {
                    request.Travel.GroundArrangements.TotalCost =
                        System.Convert.ToDecimal(this.txtGroundCost.Text);
                }
                catch (FormatException)
                {
                    request.Travel.GroundArrangements.TotalCost = BizObjectValidator.DEFAULT_COST;
                    this.packingErrors.Add("Ground estimated cost is not valid.");
                }
            }
            else
                request.Travel.GroundArrangements.TotalCost = BizObjectValidator.DEFAULT_COST;   
            
            if (this.ckbTravelLodging.Checked)
                request.Travel.HasLodgingArrangements = true;
            else
                request.Travel.HasLodgingArrangements = false;

            if (this.txtLodgingNights.Text.Length > 0)
            {
                try
                {
                    request.Travel.LodgingArrangements.NumberOfNights =
                        System.Convert.ToInt32(this.txtLodgingNights.Text);
                }
                catch (FormatException)
                {
                    request.Travel.LodgingArrangements.NumberOfNights = 0;
                    this.packingErrors.Add("Lodging # of nights is not valid.");
                }
            }
            else
                request.Travel.LodgingArrangements.NumberOfNights = 0;
            if (this.txtLodgingCost.Text.Length > 0)
            {
                try
                {
                    request.Travel.LodgingArrangements.TotalCost =
                        System.Convert.ToDecimal(this.txtLodgingCost.Text);            
                }
                catch (FormatException)
                {
                    request.Travel.LodgingArrangements.TotalCost = BizObjectValidator.DEFAULT_COST;
                    this.packingErrors.Add("Lodging estimated cost is not valid.");
                }
            }
            else
                request.Travel.LodgingArrangements.TotalCost = BizObjectValidator.DEFAULT_COST;  
            request.MiscealleanousCostsComments = this.txtMiscComments.Text;
            if (this.txtMiscCost.Text.Length > 0)
            {
                try
                {
                    request.MisceallaneousCosts = System.Convert.ToDecimal(this.txtMiscCost.Text);
                }
                catch (FormatException)
                {
                    request.MisceallaneousCosts = BizObjectValidator.DEFAULT_COST;
                    this.packingErrors.Add("Additional estimated costs is not valid.");
                }
            }
            else
                request.MisceallaneousCosts = BizObjectValidator.DEFAULT_COST;  
            return request;
        }

        private void LoadSummary(TravelAndTrainingRequest request)
        {
            this.litcEventName.Text = request.EventName;
            if (!request.EventStart.IsDefault())
                this.litcEventStart.Text = request.EventStart.ToShortDateString();
            if (!request.EventEnd.IsDefault())
                this.litcEventEnd.Text = request.EventEnd.ToShortDateString();
            this.litcEventCity.Text = request.EventCity;
            if (string.CompareOrdinal(request.EventState, "-") != 0)
                this.litcEventState.Text = request.EventState;
            this.litcEventPurpose.Text = request.BusinessPurpose;
            this.litcEventProjects.Text = request.ProjectName;

            if (request.HasTraining)
            {
                this.pnlcnfTraining.Visible = true;
                this.litcTrainName.Text = request.Training.TrainingName;
                if (!request.Training.StartingDate.IsDefault())
                    this.litcTrainStart.Text = request.Training.StartingDate.ToShortDateString();
                if (!request.Training.EndingDate.IsDefault())
                    this.litcTrainEnd.Text = request.Training.EndingDate.ToShortDateString();
                this.litcTrainCost.Text = request.Training.TotalCost.ToString("0");
            }
            else
                this.pnlcnfTraining.Visible = false;

            if (request.HasTravel)
            {
                this.pnlcnfTravel.Visible = true;
                this.litcTravelCity.Text = request.Travel.DestinationCity;
                if (string.CompareOrdinal(request.Travel.DestinationState, "-") != 0)
                    this.litcTravelState.Text = request.Travel.DestinationState;
                if (request.Travel.HasFlightArrangements)
                {
                    this.pnlcnfAir.Visible = true;
                    if (this.ddlAirDestinationMode.SelectedIndex > 0)
                        this.litcAirDest.Text = this.ddlAirDestinationMode.Items.FindByValue(request.Travel.FlightArrangements.DestinationMethod).Text;
                    if (this.ddlAirReturnMode.SelectedIndex > 0)
                        this.litcAirReturn.Text = this.ddlAirReturnMode.Items.FindByValue(request.Travel.FlightArrangements.ReturnMethod).Text;
                    this.litcAirCost.Text = request.Travel.FlightArrangements.TotalCost.ToString("0");
                }
                else
                    this.pnlcnfAir.Visible = false;
                if (request.Travel.HasGroundArrangements)
                {
                    this.pnlcnfGround.Visible = true;
                    StringBuilder sb = new StringBuilder();
                    foreach (string groundMode in request.Travel.GroundArrangements.Modes)
                        sb.Append(this.ckblstGroundModes.Items.FindByValue(groundMode).Text + ",");
                    if (sb.Length > 0)
                        sb.Remove(sb.Length - 1, 1);
                    this.litcGroundModes.Text = sb.ToString();
                    this.litcGroundCost.Text = request.Travel.GroundArrangements.TotalCost.ToString("0");
                }
                else
                    this.pnlcnfGround.Visible = false;
                if (request.Travel.HasLodgingArrangements)
                {
                    this.pnlcnfLodging.Visible = true;
                    this.litcLodgingNights.Text = request.Travel.LodgingArrangements.NumberOfNights.ToString();
                    this.litcLodgingCost.Text = request.Travel.LodgingArrangements.TotalCost.ToString("0");
                }
                else
                    this.pnlcnfLodging.Visible = false;
            }
            else
                this.pnlcnfTravel.Visible = false;
            this.litcGenComments.Text = request.MiscealleanousCostsComments;
            this.litcMiscCost.Text = request.MisceallaneousCosts.ToString("0");
            this.litcTotalCost.Text = request.TotalCost.ToString("0");
        }

        #endregion        

        protected void txtTrainCost_Init(object sender, EventArgs e)
            {
                TextBox txt = (TextBox)sender;
                if (txt != null)
                {
                    txt.Text = "0";
                }
               
            }


     }
}