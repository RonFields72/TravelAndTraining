using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SWN.TTR.WebUI
{
    public partial class RequestSummary : System.Web.UI.Page
    {
        private readonly int REQUEST_ID = System.Convert.ToInt32(HttpContext.Current.Request.QueryString["id"]);

        protected void Page_Init(object sender, EventArgs e)
        {
            this.ucSummary.LoadRequestSummary(this.REQUEST_ID);
        }
    }
}
