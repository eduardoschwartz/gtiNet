using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UIWeb.Models;

namespace UIWeb.Pages {
    public partial class detalhe_boleto : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                lblMsg.Text = "";
                txtCod.Text = "";
            }
        }

        protected void optCPF_CheckedChanged(object sender, EventArgs e) {
            if (optCPF.Checked) {
                txtCPF.Visible = true;
                txtCNPJ.Visible = false;
                txtCPF.Text = "";
                txtCNPJ.Text = "";
            }
        }

        protected void optCNPJ_CheckedChanged(object sender, EventArgs e) {
            if (optCNPJ.Checked) {
                txtCPF.Visible = false;
                txtCNPJ.Visible = true;
                txtCPF.Text = "";
                txtCNPJ.Text = "";
            }
        }


        protected void txtCod_TextChanged(object sender, EventArgs e) {
            lblMsg.Text = "";
        }

        protected void btConsultar_Click(object sender, EventArgs e) {
            string num_cpf_cnpj = "";

            lblMsg.Text = "";
            if (txtCod.Text.Trim() == "") {
                lblMsg.Text = "Digite o número do documento.";
                return;
            }
            if (txtCod.Text.Length < 17) {
                lblMsg.Text = "Número de documento inválido.";
                return;
            }

            if (optCPF.Checked && txtCPF.Text.Length < 14) {
                lblMsg.Text = "CPF inválido!";
                return;
            }
            if (optCNPJ.Checked && txtCNPJ.Text.Length < 18) {
                lblMsg.Text = "CNPJ inválido!";
                return;
            }

            if (optCPF.Checked) {
                num_cpf_cnpj = gtiCore.RetornaNumero(txtCPF.Text);
                if (!gtiCore.ValidaCpf(num_cpf_cnpj)) {
                    lblMsg.Text = "CPF inválido!";
                    return;
                }
            } else {
                num_cpf_cnpj = gtiCore.RetornaNumero(txtCNPJ.Text);
                if (!gtiCore.ValidaCNPJ(num_cpf_cnpj)) {
                    lblMsg.Text = "CNPJ inválido!";
                    return;
                }
            }
            int nNumDoc = Convert.ToInt32(txtCod.Text.Substring(txtCod.Text.Length - 8, 8));

            clsDebito Debito_class = new clsDebito();
            int nCodigo = 0;
            DateTime dDataDoc = Convert.ToDateTime("01/01/1900");
            decimal nValorGuia = 0;
            bool bExisteDoc = Debito_class.ExisteDocumento(nNumDoc);
            if (!bExisteDoc) {
                lblMsg.Text = "Número de documento não cadastrado.";
            } else {
                
                nCodigo = Debito_class.RetornaDocumentoCodigo(nNumDoc);
                numdocumento DadosDoc = Debito_class.RetornaDadosDocumento(nNumDoc);
                dDataDoc = Convert.ToDateTime(DadosDoc.datadocumento);
                nValorGuia = Convert.ToDecimal( DadosDoc.valorguia);
            }

            if (nCodigo < 100000) {
                clsImovel imovel_class = new clsImovel();
                ImovelStruct reg = imovel_class.LoadReg(nCodigo);
                List<ProprietarioStruct> regProp = imovel_class.ListaProprietario(nCodigo, true);
                if (optCPF.Checked) {
                    if (Convert.ToInt64(gtiCore.RetornaNumero(regProp[0].CPF)).ToString("00000000000") != num_cpf_cnpj) {
                        lblMsg.Text = "CPF informado não pertence a este documento.";
                        return;
                    }
                } else {
                    if (Convert.ToInt64(gtiCore.RetornaNumero(regProp[0].CPF)).ToString("00000000000000") != num_cpf_cnpj) {
                        lblMsg.Text = "CNPJ informado não pertence a este documento.";
                        return;
                    }
                }
            } else {
                if (nCodigo >= 100000 && nCodigo < 500000) {
                    clsEmpresa empresa_class = new clsEmpresa();
                    EmpresaStruct reg = empresa_class.LoadReg(nCodigo);
                    if (optCPF.Checked) {
                        if (Convert.ToInt64(gtiCore.RetornaNumero(reg.cpf_cnpj)).ToString("00000000000") != num_cpf_cnpj) {
                            lblMsg.Text = "CPF informado não pertence a este documento.";
                            return;
                        }
                    } else {
                        if (Convert.ToInt64(gtiCore.RetornaNumero(reg.cpf_cnpj)).ToString("00000000000000") != num_cpf_cnpj) {
                            lblMsg.Text = "CNPJ informado não pertence a este documento.";
                            return;
                        }
                    }
                } else {
                    clsCidadao cidadao_class = new clsCidadao();
                    CidadaoStruct reg = cidadao_class.LoadReg(nCodigo);
                    if (optCPF.Checked) {
                        if (Convert.ToInt64(gtiCore.RetornaNumero(reg.Cpf)).ToString("00000000000") != num_cpf_cnpj) {
                            lblMsg.Text = "CPF informado não pertence a este documento.";
                            return;
                        }
                    } 
                    else {
                        if (Convert.ToInt64(gtiCore.RetornaNumero(reg.Cnpj)).ToString("00000000000000") != num_cpf_cnpj) {
                            lblMsg.Text = "CNPJ informado não pertence a este documento.";
                            return;
                        }
                    }
                }
            }

            List<DebitoStructure>ListaParcelas= Carregaparcelas(nNumDoc,dDataDoc);
            int nSid = Debito_class.GravaDetalheDAM(ListaParcelas, txtCod.Text, dDataDoc,nValorGuia);
            printCarne(nSid);
        }

        private List<DebitoStructure> Carregaparcelas(int nNumDoc,DateTime dDataDoc) {
            int i = 0;
            clsDebito debito_class = new clsDebito();
            List<DebitoStructure> ListaParcelas = debito_class.ListaParcelasDocumento(nNumDoc);
            foreach (DebitoStructure Linha in ListaParcelas) {
                List<DebitoStructure> reg = debito_class.Extrato(Linha.Codigo_Reduzido,(short)Linha.Ano_Exercicio,(short)Linha.Ano_Exercicio,(short)Linha.Codigo_Lancamento,(short)Linha.Codigo_Lancamento,
                    (short)Linha.Sequencia_Lancamento,(short)Linha.Sequencia_Lancamento,(short)Linha.Numero_Parcela,(short)Linha.Numero_Parcela,Linha.Complemento,Linha.Complemento,0,99,dDataDoc,0);
                for (i = 0; i < ListaParcelas.Count; i++) {
                    if (ListaParcelas[i].Ano_Exercicio == Linha.Ano_Exercicio & ListaParcelas[i].Codigo_Lancamento == Linha.Codigo_Lancamento & ListaParcelas[i].Sequencia_Lancamento == Linha.Sequencia_Lancamento &
                        ListaParcelas[i].Numero_Parcela == Linha.Numero_Parcela & ListaParcelas[i].Complemento == Linha.Complemento)
                        break;
                }
                ListaParcelas[i].Soma_Principal = reg[0].Soma_Principal;
                ListaParcelas[i].Soma_Multa = reg[0].Soma_Multa;
                ListaParcelas[i].Soma_Juros = reg[0].Soma_Juros;
                ListaParcelas[i].Soma_Correcao = reg[0].Soma_Correcao;
                ListaParcelas[i].Soma_Total = reg[0].Soma_Total;
                ListaParcelas[i].Descricao_Lancamento = reg[0].Descricao_Lancamento;
                string DescTributo = "";
                DescTributo = "";
                foreach (TributoStructure a in reg[0].Tributos) {
                    DescTributo += a.Codigo.ToString("000") + "-" + a.Descricao + "/";
                }
                DescTributo = DescTributo.Substring(0, DescTributo.Length - 1);
                ListaParcelas[i].Descricao_Tributo = DescTributo;
                ListaParcelas[i].Data_Vencimento = reg[0].Data_Vencimento;
            }

            return ListaParcelas;

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
            List<boleto> ListaBoleto = Debito_Class.ListaBoletoDAM(nSid);
            DataSet Ds = gtiCore.ToDataSet(ListaBoleto);
            ReportDataSource rdsAct = new ReportDataSource("dsDam", Ds.Tables[0]);
            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/rptDetalheBoleto.rdlc");
            viewer.LocalReport.DataSources.Add(rdsAct); // Add  datasource here         
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            Debito_Class.DeleteDam(nSid);
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