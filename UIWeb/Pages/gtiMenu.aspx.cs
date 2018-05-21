using System;

namespace UIWeb {
    public partial class gtiMenu : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void btOK_Click(object sender, EventArgs e) {
            
            if (optList.Items[0].Selected == true)
                Response.Redirect("~/Pages/gtiMenu2.aspx");
            else if (optList.Items[1].Selected == true)
                Response.Redirect("~/Pages/SegundaViaIPTU.aspx");
            else if (optList.Items[2].Selected == true)
                Response.Redirect("~/Pages/SegundaViaCIP.aspx");
            else if (optList.Items[3].Selected == true)
                Response.Redirect("~/Pages/detalhe_boleto.aspx");
        }

    }
}