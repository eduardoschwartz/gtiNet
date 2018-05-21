using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using UIWeb.Models;


namespace UIWeb.Pages {
    public partial class SegundaViaIPTUFim : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            lblMsg.Text = "";
            if (!IsPostBack) {
                if (Session["sid"] != null && Session["sid"].ToString() != "") {
                    clsDebito Debito_Class = new clsDebito();
                    List<boletoguia> ListaBoleto = Debito_Class.ListaBoletoGuia(Convert.ToInt32(Session["sid"]));
                    lblCod.Text = ListaBoleto[0].codreduzido;
                    lblNome.Text = ListaBoleto[0].nome;
                } else
                    Response.Redirect("~/Pages/gtiMenu.aspx");
            }
        }


        private void printCarne(int nSid) {
            lblMsg.Text = "";
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            Session["sid"] = "";
            clsDebito Debito_Class = new clsDebito();
            List<boletoguia> ListaBoleto = Debito_Class.ListaBoletoGuia(nSid);
            if (ListaBoleto.Count > 0) {
                Debito_Class.GravaCarneWeb(Convert.ToInt32( ListaBoleto[0].codreduzido), 2018);
                DataSet Ds = gtiCore.ToDataSet(ListaBoleto);
                ReportDataSource rdsAct = new ReportDataSource("DataSet1", Ds.Tables[0]);
                ReportViewer viewer = new ReportViewer();
                viewer.LocalReport.Refresh();
               // viewer.LocalReport.ReportPath = Server.MapPath("~/Report/rptBoletoParcelado.rdlc");
                viewer.LocalReport.ReportPath = "Report/rptDamParcelado.rdlc";
                //ReportParameter[] param = new ReportParameter[5];
                //param[0] = new ReportParameter("Report_Parameter_0", "1st Para", true);
                viewer.LocalReport.DataSources.Add(rdsAct); // Add  datasource here         
                byte[] bytes = viewer.LocalReport.Render( "PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                Debito_Class.DeleteCarne(nSid);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename= guia_pmj" + "." + extension);
                Response.OutputStream.Write(bytes, 0, bytes.Length);
                Response.Flush();
                Response.End();
            } else
                lblMsg.Text = "A guia já foi impressa!";

        }

        protected void btPrint_Click(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty( Session["sid"].ToString() )) {
                printCarne(Convert.ToInt32(Session["sid"]));
                Session["sid"] = "";
            }
            else
                Response.Redirect("~/Pages/gtiMenu.aspx");
        }

    }
}