using gtiNet.DAL;
using Microsoft.Reporting.WebForms;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace UIWeb {
    public partial class Boleto : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void Button1_Click(object sender, EventArgs e) {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
              string extension = string.Empty;


            //This is optional if you have parameter then you can add parameters as much as you want
            //     ReportParameter[] param = new ReportParameter[5];
            //         param[0] = new ReportParameter("Report_Parameter_0", "1st Para", true);
            //       param[1] = new ReportParameter("Report_Parameter_1", "2nd Para", true);
            //     param[2] = new ReportParameter("Report_Parameter_2", "3rd Para", true);
            //   param[3] = new ReportParameter("Report_Parameter_3", "4th Para", true);
            // param[4] = new ReportParameter("Report_Parameter_4", "5th Para");

            Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();
            SqlConnection cn = new SqlConnection(Dados.StringDeConexao);
            SqlDataAdapter da = new SqlDataAdapter();
            System.Data.DataSet dsData = new System.Data.DataSet();
            String Sql = "select * from boleto";
            SqlCommand cmd = new SqlCommand(Sql, cn);
            cmd.CommandType = CommandType.Text;
            cn.Open();
            da.SelectCommand = cmd;
            da.Fill(dsData);
            cn.Close();


            ReportDataSource rdsAct = new ReportDataSource("DataSet1",dsData.Tables[0]);
            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.LocalReport.ReportPath = "Report/Ficha_Compensacao.rdlc"; //This is your rdlc name.
           // viewer.LocalReport.SetParameters(param);
            viewer.LocalReport.DataSources.Add(rdsAct); // Add  datasource here         
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            // byte[] bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.          
            // System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename= filename" + "." + extension);
            Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
            Response.Flush(); // send it to the client to download  
            Response.End();
        }
    }
}