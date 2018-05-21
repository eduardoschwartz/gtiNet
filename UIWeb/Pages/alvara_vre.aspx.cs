using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using UIWeb.Models;

namespace UIWeb {
    public partial class alvara_vre : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
        }

        protected void btPrint_Click(object sender, EventArgs e) {
            int Num = 0;
            String sTextoImagem = txtimgcode.Text;
            txtimgcode.Text = "";

            if (sTextoImagem.ToUpper() != this.Session["CaptchaImageText"].ToString().ToUpper()) {
                lblmsg.Text = "Código da imagem inválido.";
                return;
            }

            clsEmpresa Empresa_class = new clsEmpresa();
            bool isNum = Int32.TryParse(txtCod.Text, out Num);
            if (!isNum) {
                lblmsg.Text = "Inscrição Municipal inválida!";
                return;
            } else {
                bool bExiste = Empresa_class.ExisteEmpresa(Num);
                if (!bExiste) {
                    lblmsg.Text = "Inscrição Municipal inválida!";
                    return;
                }
            }

            SilStructure Sil = Empresa_class.CarregaSil(Num);

            if (Sil.Codigo == 0) {
                lblmsg.Text = "Solicitação inválida!";
                return;}
            else if (Sil.Protocolo == null) {
                lblmsg.Text = "Solicitação inválida!";
                return; } 
            else if (Sil.Data_Validade < DateTime.Now) {
                lblmsg.Text = "Solicitação inválida!";
                return;
            }
           

            lblmsg.Text = "";
            this.txtimgcode.Text = "";
            EmiteAlvara(Num);
        }

        private void EmiteAlvara(int Codigo) {
   
            clsEmpresa Empresa_class = new clsEmpresa();
            EmpresaStruct empresa = Empresa_class.LoadReg(Codigo);
            SilStructure sil = Empresa_class.CarregaSil(Codigo);
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.LocalReport.ReportPath = "Report/rptAlvara_vre.rdlc";

            string _protocolo = sil.Protocolo == null ? "" : sil.Protocolo;
            string _endereco = empresa.Endereco + ", " + empresa.Numero + " " + empresa.Complemento;

            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("RazaoSocial", empresa.RazaoSocial));
            parameters.Add(new ReportParameter("Protocolo", _protocolo==""?" ":_protocolo));
            parameters.Add(new ReportParameter("Endereco", _endereco==""?" ":_endereco));
            parameters.Add(new ReportParameter("Cidade", empresa.NomeCidade == "" ? " " : empresa.NomeCidade));
            parameters.Add(new ReportParameter("Horario", empresa.Horario == "" ? " " : empresa.Horario));
            parameters.Add(new ReportParameter("Bairro", empresa.NomeBairro == "" ? " " : empresa.NomeBairro));
            parameters.Add(new ReportParameter("Cep", empresa.Cep == "" ? " " : empresa.Cep));
            parameters.Add(new ReportParameter("CPF", empresa.cpf_cnpj == "" ? " " : empresa.cpf_cnpj));
            parameters.Add(new ReportParameter("Inscricao", empresa.Codigo.ToString()));
            parameters.Add(new ReportParameter("InscEstadual", string.IsNullOrWhiteSpace(empresa.Inscricao_estadual) ? " " : empresa.Inscricao_estadual));
            parameters.Add(new ReportParameter("Atividade", empresa.AtividadeExtenso == "" ? " " : empresa.AtividadeExtenso));

            viewer.LocalReport.SetParameters(parameters);

            //     viewer.LocalReport.DataSources.Add(rdsAct); // Add  datasource here         
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename= guia_pmj" + "." + extension);
            Response.OutputStream.Write(bytes, 0, bytes.Length);
            Response.Flush();
            Response.End();

        }

       
    }//end class
}//end namespace