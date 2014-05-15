using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

namespace SWN.TTR.WebUI.Controls
{
    public partial class RequestSummary : System.Web.UI.UserControl
    {        
        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        public void LoadRequestSummary(int requestId)
        {
            TravelAndTrainingRequestFinder requestFinder = new TravelAndTrainingRequestFinder(this.Context.User);
            this.repSummary.DataSource = requestFinder.GetRequestSummaryByRequest(requestId);
            this.repSummary.DataBind();
        }

        protected void repSummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.Item ||
                item.ItemType == ListItemType.AlternatingItem)
            {
                BindRequestChildren(e);
                HideShowRequestPanels(e);                
            }
        }        

        private void HideShowRequestPanels(RepeaterItemEventArgs e)
        {
            // hide training if not chosen                
            Panel training = (Panel)e.Item.FindControl("pnlTraining");
            if ((bool)((DataRowView)e.Item.DataItem)["Has_Training"])
                training.Visible = true;
            else
                training.Visible = false;

            // hide travel if not chosen
            Panel travel = (Panel)e.Item.FindControl("pnlTravel");
            if ((bool)((DataRowView)e.Item.DataItem)["Has_Travel"])
                travel.Visible = true;
            else
                travel.Visible = false;

            // hide air if not chosen
            Panel air = (Panel)e.Item.FindControl("pnlAir");
            if ((bool)((DataRowView)e.Item.DataItem)["Travel_Has_Flight"])
                air.Visible = true;
            else
                air.Visible = false;

            // hide air if not chosen
            Panel ground = (Panel)e.Item.FindControl("pnlGround");
            if ((bool)((DataRowView)e.Item.DataItem)["Travel_Has_Ground"])
                ground.Visible = true;
            else
                ground.Visible = false;

            // hide lodging if not chosen
            Panel lodging = (Panel)e.Item.FindControl("pnlLodging");
            if ((bool)((DataRowView)e.Item.DataItem)["Travel_Has_Lodging"])
                lodging.Visible = true;
            else
                lodging.Visible = false;
        }

        private void BindRequestChildren(RepeaterItemEventArgs e)
        {
            Repeater rptApprovers = (Repeater)e.Item.FindControl("repApprovers");
            if (rptApprovers != null)
            {
                rptApprovers.DataSource =
                    ((DataRowView)e.Item.DataItem).CreateChildView("RequestSummary_ApprovalSummary");
                rptApprovers.DataBind();
            }

            Repeater rptGround = (Repeater)e.Item.FindControl("repGround");
            if (rptGround != null)
            {
                rptGround.DataSource =
                    ((DataRowView)e.Item.DataItem).CreateChildView("RequestSummary_GroundTransportationSummary");
                rptGround.DataBind();
            }
        }
    }
}