using System;
using System.Configuration;
using System.ComponentModel;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using gtiNet.BLL;
using gtiNet.DAL;
using gtiNet.Modelos;
using System.Data.SqlClient;

namespace UIWeb.Pages {
    public partial class frmTramiteProcesso : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            LimpaDados();
        }

        protected void ShowHeader(bool bShow) {
            lblC1.Visible = bShow;
            lblC2.Visible = bShow;
            lblC3.Visible = bShow;
        }

        protected void LimpaDados() {
            ShowHeader(false);
            lblAssunto.Text = "";
            lblComplemento.Text = "";
            lblRequerente.Text = "";
            grdMain.DataSource = null;
            grdMain.DataBind();
        }

        protected void btPesquisar_Click(object sender, EventArgs e) {
            int nAno = 0, nNumero = 0;
            LimpaDados();
            Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();

            ProcessoBLL obj = new ProcessoBLL();
            List<Processo> regProc = obj.SeparaNumeroProcesso(txtNumProc.Text, true);
            lblMsg.Text = "";
            if (regProc[0].IsValid) {
                nAno = regProc[0].Ano;
                nNumero = regProc[0].Numero;
            } else {
                lblMsg.Text = "Nº de Processo inválido!";
                LimpaDados();
                return;
            }

            gtiNet.DAL.Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();

            Processo reg = new Processo();
            reg.Ano = nAno;
            reg.Numero = nNumero;
            List<Processo> Lista = obj.Listagem(reg, null);
            if (Lista.Count == 0) {
                lblMsg.Text = "Processo não cadastrado.";
                LimpaDados();
            } else {
                ShowHeader(true);
                lblAssunto.Text = Lista[0].AssuntoNome;
                lblComplemento.Text = Lista[0].Complemento;
                lblRequerente.Text = Lista[0].CidadaoNome;
                CarregaTramite(nNumero, nAno);
            }
        }

        protected void CarregaTramite(Int32 Numero, Int32 Ano) {
            gtiNet.DAL.Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();
            ProcessoBLL obj = new ProcessoBLL();
            List<Tramite> Lista = obj.RetornaTramite(Numero, Ano);
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Seq"), new DataColumn("Descricao"), new DataColumn("DataHora"), 
                                new DataColumn("Despacho")});

            foreach (var item in Lista) {
                dt.Rows.Add(item.Seq, item.Descricao, item.Datahora.ToString() == "01/01/1900" ? "" : item.Datahora.ToString(), item.Despachonome);

            }
            grdMain.DataSource = dt;
            grdMain.DataBind();

        }


    }//end class

}//end namespace