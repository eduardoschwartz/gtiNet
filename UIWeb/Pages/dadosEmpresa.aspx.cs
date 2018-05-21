using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using UIWeb.Models;


namespace UIWeb.Pages {
    public partial class dadosEmpresa : System.Web.UI.Page {
        public static string sCnae2;
        public static string sSocio2;
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                txtCNPJ.Text = "";
                txtIM.Text = "";
                lblMsg.Text = "";
            }
        }

        protected void btAcesso_Click(object sender, EventArgs e) {
            lblMsg.Text = "";
            ClearTable();
            clsEmpresa empresa_class = new clsEmpresa();
            if (string.IsNullOrWhiteSpace(txtIM.Text) && string.IsNullOrWhiteSpace(txtCNPJ.Text))
                lblMsg.Text = "Erro: Informação necessária.";
            else {
                if (!string.IsNullOrWhiteSpace(txtIM.Text) && !string.IsNullOrWhiteSpace(txtCNPJ.Text))
                    lblMsg.Text = "Erro: Digite a inscrição municipal ou o cnpj da empresa.";

                else {
                    if (!string.IsNullOrWhiteSpace(txtIM.Text)) {
                        if(!empresa_class.ExisteEmpresa(Convert.ToInt32(txtIM.Text)))
                            lblMsg.Text = "Erro: Cadastro inexistente.";
                        else
                            FillTable();
                    } else {
                        string sCnpj = txtCNPJ.Text.PadLeft(14, '0');
                        int nCodigo=empresa_class.ExisteEmpresaCnpj(sCnpj);
                        if (!string.IsNullOrWhiteSpace(sCnpj) && nCodigo == 0)
                            lblMsg.Text = "Erro: Cadastro inexistente.";
                        else {
                            txtIM.Text = nCodigo.ToString("000000");
                            FillTable();
                        }
                    }
                }
            }
        }

        private void ClearTable() {
            IM.Text = "";
            CNPJ.Text = "";
            RAZAOSOCIAL.Text = "";
            DATAABERTURA.Text = "";
            DATAENCERRAMENTO.Text = "";
            SITUACAO.Text = "";
            IE.Text = "";
            EMAIL.Text = "";
            TELEFONE.Text = "";
            REGIMEISS.Text = "";
            VIGSANIT.Text = "";
            TAXALICENCA.Text = "";
            SIMPLES.Text = "";
            MEI.Text = "";
            PROPRIETARIO.Text = "";
            CNAE.Text = "";
            ENDERECO.Text = "";
        }

        private void FillTable() {
            clsEmpresa empresa_class = new clsEmpresa();
            Int32 Codigo = Convert.ToInt32(txtIM.Text);
            EmpresaStruct reg = empresa_class.LoadReg(Codigo);
            if(reg.Juridica)
                CNPJ.Text = Convert.ToUInt64(reg.cpf_cnpj).ToString(@"00\.000\.000\/0000\-00");
            else {
                if(reg.cpf_cnpj.Length>1)
                    CNPJ.Text = Convert.ToUInt64(reg.cpf_cnpj).ToString(@"000\.000\.000\-00");
                else
                    CNPJ.Text = "";
            }
            IM.Text = reg.Codigo.ToString();
            RAZAOSOCIAL.Text = reg.RazaoSocial;
            IE.Text = reg.Inscricao_estadual;
            DATAABERTURA.Text = reg.Data_Abertura.ToString("dd/MM/yyyy");
            DATAENCERRAMENTO.Text=String.IsNullOrEmpty(reg.Data_Encerramento.ToString())?"": Convert.ToDateTime(reg.Data_Encerramento).ToString("dd/MM/yyyy");
            SITUACAO.Text = reg.Situacao;
            ENDERECO.Text = reg.Endereco + ", " + reg.Numero + " " + reg.Complemento + " ";
            ENDERECO.Text += reg.NomeBairro + "-" + reg.NomeCidade + "/" + reg.NomeUF + " Cep: " + reg.Cep;
            EMAIL.Text = reg.Email;
            TELEFONE.Text = reg.Telefone;
            AREA.Text = string.Format("{0:0.00}",reg.Area);
            string sRegime = empresa_class.RegimeEmpresa(Codigo);
            if (sRegime == "F")
                sRegime = "ISS FIXO";
            else {
                if (sRegime == "V")
                    sRegime = "ISS VARIÁVEL";
                else {
                    if (sRegime == "E")
                        sRegime = "ISS ESTIMADO";
                    else
                        sRegime = "NENHUM";
                }
            }
            REGIMEISS.Text = sRegime;
            VIGSANIT.Text = empresa_class.Empresa_tem_VS(Codigo) ? "SIM" : "NÃO";
            TAXALICENCA.Text = empresa_class.Empresa_tem_TL(Codigo) ? "SIM" : "NÃO";
            MEI.Text = empresa_class.Empresa_Mei(Codigo) ? "SIM" : "NÃO";
            List<CidadaoStruct> ListaSocio = empresa_class.ListaSocio(Codigo);
            string sSocio = "";
            sSocio2 = "";
            foreach (CidadaoStruct Socio in ListaSocio) {
                sSocio += Socio.Nome + System.Environment.NewLine;
                sSocio2 += Socio.Nome + ", ";
            }
            if (!string.IsNullOrWhiteSpace(sSocio2))
                sSocio2 = sSocio2.Substring(0, sSocio2.Length - 2);
            PROPRIETARIO.Text = "<pre>" + sSocio + "</pre>";

             List<CnaeStruct> ListaCnae = empresa_class.ListaCnae(Codigo);
             string sCnae = "";
             sCnae2 = "";
             foreach (CnaeStruct cnae in ListaCnae) {
                 sCnae += cnae.Cnae + "-" + cnae.Descricao + System.Environment.NewLine;
                 sCnae2 += cnae.Cnae + "-" + cnae.Descricao + System.Environment.NewLine;
             }
                 if (!string.IsNullOrWhiteSpace(sCnae2))
                   sCnae2 = sCnae2.Substring(0, sCnae2.Length - 1);

             CNAE.Text = "<pre>" + sCnae + "</pre>";
             SIMPLES.Text = empresa_class.Empresa_Simples(Codigo) ? "SIM" : "NÃO";
        }

        protected void btPrint_Click(object sender, EventArgs e) {
            if (String.IsNullOrWhiteSpace(RAZAOSOCIAL.Text))
                lblMsg.Text = "Selecione uma empresa para imprimir";
            else {
                lblMsg.Text = "";

                List<DEmpresa> aLista = new List<DEmpresa>();
                int nSid = gtiCore.GetRandomNumber();
                DEmpresa reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Inscrição Municipal";
                reg.valor = IM.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Razão Social";
                reg.valor = RAZAOSOCIAL.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "CNPJ/CPF";
                reg.valor = CNPJ.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Data de Abertura";
                reg.valor = DATAABERTURA.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Data de Encerramento";
                reg.valor = DATAENCERRAMENTO.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Inscrição Estadual";
                reg.valor = IE.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Situação";
                reg.valor = SITUACAO.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Endereço";
                reg.valor = ENDERECO.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Email";
                reg.valor = EMAIL.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Telefone";
                reg.valor = TELEFONE.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Regime de ISS";
                reg.valor = REGIMEISS.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Vigilância Sanitária";
                reg.valor = VIGSANIT.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Taxa de Licença";
                reg.valor = TAXALICENCA.Text;
                aLista.Add(reg); reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Optante do Simples";
                reg.valor = SIMPLES.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Micro Emp. Individual";
                reg.valor = MEI.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Área";
                reg.valor = AREA.Text;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Proprietário";
                reg.valor = sSocio2;
                aLista.Add(reg);
                reg = new DEmpresa();
                reg.sid = nSid;
                reg.nome = "Atividades";
                reg.valor = sCnae2;
                aLista.Add(reg);
                clsEmpresa empresa_class = new clsEmpresa();
                empresa_class.Grava_DEmp(aLista);

                List<DEmpresa> ListaEmp = empresa_class.ListaDEmpresa(nSid);
                DataTable dt = gtiCore.ConvertToDatatable(ListaEmp);

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                DataSet Ds = gtiCore.ToDataSet(ListaEmp);
                ReportDataSource rdsAct = new ReportDataSource("dsDadosEmpresa", Ds.Tables[0]);
                ReportViewer viewer = new ReportViewer();
                viewer.LocalReport.Refresh();
                viewer.LocalReport.ReportPath = Server.MapPath("~/Report/rptDadosEmpresa.rdlc");
                //viewer.LocalReport.ReportPath = "Report/rptDadosEmpresa.rdlc";
                //ReportParameter[] param = new ReportParameter[5];
                //param[0] = new ReportParameter("Report_Parameter_0", "1st Para", true);
                viewer.LocalReport.DataSources.Add(rdsAct); // Add  datasource here         
                byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                empresa_class.Delete_DEmpresa(nSid);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename= guia_pmj" + "." + extension);
                Response.OutputStream.Write(bytes, 0, bytes.Length);
                Response.Flush();
                Response.End();

            }
        }
    }
       
}