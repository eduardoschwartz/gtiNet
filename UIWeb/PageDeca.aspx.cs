using System;

namespace UIWeb {
    public partial class PageDeca : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            Response.Redirect("~/Pages/envioDeca.aspx");
        }

     
    }
}