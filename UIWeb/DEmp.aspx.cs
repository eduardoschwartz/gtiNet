using System;


namespace UIWeb {


    public partial class DEmp : System.Web.UI.Page {

        protected void Page_Load(object sender, EventArgs e) {
            Response.Redirect("~/Pages/dadosEmpresa.aspx");
        }

    }

}