using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using UIWeb.Models;

namespace UIWeb.Pages {
    public partial class damwebend2 : System.Web.UI.Page {


        protected void Page_Load(object sender, EventArgs e) {
            int nCodigo = 0;
            if (!IsPostBack) {
                if (Session["sid"] != null && Session["sid"].ToString() != "") {
                    clsDebito Debito_Class = new clsDebito();
                    List<boleto> ListaBoleto = Debito_Class.ListaBoletoDAM(Convert.ToInt32(Session["sid"]));
                    txtDtVenc.Text = Convert.ToDateTime(ListaBoleto[0].datadam).ToString("dd/MM/yyyy");
                    txtValor.Text = Convert.ToDouble(ListaBoleto[0].valordam).ToString("#0.00");

                    txtcpfCnpj.Text = ListaBoleto[0].cpf;
                    txtrefTran.Text = "287353200" + ListaBoleto[0].numdoc2.Substring(0,8); 

                    nCodigo = Convert.ToInt32( ListaBoleto[0].codreduzido);
                    if (nCodigo < 100000) {
                        //Imóvel
                        clsImovel Imovel = new clsImovel();
                        int nTipoEndereco = Imovel.LoadReg(nCodigo).EE_TipoEndereco;
                        EnderecoStruct reg = Imovel.RetornaEndereco(nCodigo, nTipoEndereco==0?gtiCore.TipoEndereco.Local:nTipoEndereco==1?gtiCore.TipoEndereco.Entrega:gtiCore.TipoEndereco.Proprietario);
                        txtNome.Text = Imovel.ListaProprietario(nCodigo, true)[0].Nome;
                        txtEndereco.Text = reg.Endereco + ", " + reg.Numero.ToString() + " " + reg.Complemento + " " + reg.NomeBairro ;
                        txtCidade.Text = reg.NomeCidade;
                        txtCep.Text = reg.Cep;
                        txtUF.Text = reg.UF;
                    } else {
                        if(nCodigo>=100000 && nCodigo < 500000) {
                            //Empresa
                            clsEmpresa Empresa = new clsEmpresa();
                            EmpresaStruct reg = Empresa.LoadReg(nCodigo);
                            txtNome.Text = reg.RazaoSocial;
                            txtEndereco.Text = reg.Endereco + ", " + reg.Numero.ToString() + " " + reg.Complemento + " " + reg.NomeBairro;
                            txtCidade.Text = reg.NomeCidade;
                            txtCep.Text = reg.Cep;
                            txtUF.Text = reg.NomeUF;
                        } else {
                            //Cidadão
                            clsCidadao Cidadao = new clsCidadao();
                            CidadaoStruct reg = Cidadao.LoadReg(nCodigo);
                            txtNome.Text = reg.Nome;
                            txtEndereco.Text = reg.EnderecoR + ", " + reg.NumeroR.ToString() + " " + reg.ComplementoR + " " + reg.NomeBairroR;
                            txtCidade.Text = reg.NomeCidadeR;
                            txtCep.Text = reg.CepR.ToString();
                            txtUF.Text = reg.UfR;
                        }
                    }
                    UpdateDatabase();
                } else
                    Response.Redirect("~/Pages/gtiMenu.aspx");
            }
            else
                Response.Redirect("~/Pages/gtiMenu.aspx");
        }

        protected void btPrint_Click(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(Session["sid"].ToString())) {
                printCarne(Convert.ToInt32(Session["sid"]));
                Session["sid"] = "";
            } else
                Response.Redirect("~/Pages/gtiMenu.aspx");

        }

        private void printCarne(int nSid) {
        }

        public static String RetornaNumero(String Numero) {
            if (String.IsNullOrEmpty(Numero))
                return "0";
            else
                return Regex.Replace(Numero, @"[^\d]", "");
        }

        public void UpdateDatabase() {
            if (txtCidade.Text.Length == 0) {
            } else {
                comercio_eletronico Reg = new comercio_eletronico();
                Reg.cep = Convert.ToInt32(gtiCore.RetornaNumero(txtCep.Text));
                Reg.cidade = txtCidade.Text.Length > 50 ? txtCidade.Text.Substring(0, 50) : txtCidade.Text;
                Reg.cpfcnpj = gtiCore.RetornaNumero(txtcpfCnpj.Text);
                Reg.dataemissao = DateTime.Now;
                Reg.datavencto = gtiCore.IsDate(txtDtVenc.Text) ? Convert.ToDateTime(txtDtVenc.Text) : Convert.ToDateTime("01/01/1900");
                Reg.endereco = txtEndereco.Text.Length > 200 ? txtEndereco.Text.Substring(0, 200) : txtEndereco.Text;
                Reg.nome = txtNome.Text.Length > 100 ? txtNome.Text.Substring(0, 100) : txtNome.Text;
                Reg.nossonumero = txtrefTran.Text;
                Reg.numdoc = Convert.ToInt32(txtrefTran.Text.Right(8));
                Reg.uf = txtUF.Text;
                Reg.usuario = "DAM/Web";
                Reg.valorguia = Convert.ToDecimal(txtValor.Text);

                clsDebito Debito_Class = new clsDebito();
                if (Debito_Class.ExisteComercioEletronico(Reg.numdoc))
                    Response.Redirect("~/Pages/gtiMenu.aspx");
                else
                    Debito_Class.InsertBoletoComercioEletronico(Reg);
            }
        }
        

    }
}