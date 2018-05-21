using gtiNet.BLL;
using gtiNet.DAL;
using gtiNet.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;


namespace UIWeb {
    public partial class PageDecaConsulta : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                txtDataIni.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDataFim.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        protected void btBack_Click(object sender, EventArgs e) {
            Response.Redirect("~/PageDeca.aspx");
        }

        protected void btConsultar_Click(object sender, EventArgs e) {
            cmbProtocolo.Items.Clear();
            if(!CoreDAL.IsDate(txtDataIni.Text)){
                lblMsg.Text = "Data inicial inválida!";
                return;
            }

            if (!CoreDAL.IsDate(txtDataFim.Text)) {
                lblMsg.Text = "Data final inválida!";
                return;
            }

            DateTime dDataIni=Convert.ToDateTime(txtDataIni.Text);
            DateTime dDataFim=Convert.ToDateTime(txtDataFim.Text);

            if (dDataIni > dDataFim) {
                lblMsg.Text = "Data inicial inválida!";
                return;
            }

            CarregaDecas(dDataIni,dDataFim);
        }

        private void CarregaDecas(DateTime dDataIni, DateTime dDataFim) {
            gtiNet.DAL.Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();
            EmpresaBLL obj = new EmpresaBLL();
            List<Decafile> Lista = obj.ListaDecaData(dDataIni, dDataFim,"");
            foreach (var item in Lista) {
                bool bFind = false;
                for (int y = 0; y < cmbProtocolo.Items.Count; y++) {
                    if (cmbProtocolo.Items[y].ToString() == item.Protocolosil.ToString()) {
                        bFind = true;
                        break;
                    }
                }
                if(!bFind)
                    cmbProtocolo.Items.Add(item.Protocolosil.ToString());
            }
            if (cmbProtocolo.Items.Count > 0) CarregaDoc();
        }


        protected void cmbProtocolo_SelectedIndexChanged(object sender, EventArgs e) {
            CarregaDoc();
        }


        private void CarregaDoc() {
            grdDoc.DataSource=null;
            DateTime dDataIni = Convert.ToDateTime(txtDataIni.Text);
            DateTime dDataFim = Convert.ToDateTime(txtDataFim.Text);
            String sProtocolo = cmbProtocolo.Text;
            gtiNet.DAL.Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();
            EmpresaBLL obj = new EmpresaBLL();
            List<Decafile> Lista = obj.ListaDecaData(dDataIni, dDataFim, sProtocolo);
            grdDoc.DataSource=Lista;
            grdDoc.DataBind();
        }

      
        protected void Button2_Click(object sender, EventArgs e) {
            Response.Redirect("~/Pages/gtiMenu.aspx");
        }
    }//end class
}