using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UIWeb.Models;

namespace UIWeb.Pages {
    public partial class boletoBB : System.Web.UI.Page {
        public string u;
        protected void Page_Load(object sender, EventArgs e) {
            
            String s = Request.QueryString["f1"];
            txtNome.Text = s;
            s = Request.QueryString["f2"];
            txtEndereco.Text = s;
            s = Request.QueryString["f3"];
            txtDtVenc.Text = s;
            s = Request.QueryString["f4"];
            txtcpfCnpj.Text = s;
            s = Request.QueryString["f5"];
            txtrefTran.Text = s;
            s = Request.QueryString["f6"];
            txtValor.Text = s;
            s = Request.QueryString["f7"];
            txtCidade.Text = s;
            s = Request.QueryString["f8"];
            txtUF.Text = s;
            s = Request.QueryString["f9"];
            txtCep.Text = s;
            s = Request.QueryString["f10"];
            u = s;

            /*  txtNome.Text = "SÃO SEBASTIÃO AÇAÍ";
              txtEndereco.Text = "AV TIRADENTES, 330 - CENTRO";
              txtDtVenc.Text = "15/01/2018";
              txtcpfCnpj.Text = "03203004801";
              txtrefTran.Text = "28735320016301528";
              txtValor.Text = "253,00";
              txtCidade.Text = "JABOTICABAL";
              txtUF.Text = "SP";
              txtCep.Text = "14870-021";
              u = "SCHWARTZ-Dam";*/



            UpdateDatabase();
        }

        public static String RetornaNumero(String Numero) {
            if (String.IsNullOrEmpty(Numero))
                return "0";
            else
                return Regex.Replace(Numero, @"[^\d]", "");
        }

       
        public void UpdateDatabase()
        {
            comercio_eletronico Reg = new comercio_eletronico();
            Reg.cep = Convert.ToInt32(RetornaNumero(txtCep.Text));
            Reg.cidade = txtCidade.Text.Length>50? txtCidade.Text.Substring(0, 50):txtCidade.Text;
            Reg.cpfcnpj = RetornaNumero(txtcpfCnpj.Text);
            Reg.dataemissao = DateTime.Now;
            Reg.datavencto =  gtiCore.IsDate(txtDtVenc.Text)?  Convert.ToDateTime(txtDtVenc.Text):Convert.ToDateTime("01/01/1900");
            Reg.endereco = txtEndereco.Text.Length>200?txtEndereco.Text.Substring(0,200):txtEndereco.Text;
            Reg.nome = txtNome.Text.Length>100?  txtNome.Text.Substring(0, 100):txtNome.Text;
            Reg.nossonumero = txtrefTran.Text;
            Reg.numdoc = Convert.ToInt32(txtrefTran.Text.Right(8));
            Reg.uf = txtUF.Text;
            Reg.usuario = String.IsNullOrEmpty(u)? "DAM/Web": u;
            Reg.valorguia = Convert.ToDecimal(txtValor.Text);

            clsDebito Debito_Class = new clsDebito();
            Debito_Class.InsertBoletoComercioEletronico(Reg);
        }

        protected void btResumo_Click(object sender, EventArgs e) {

        }

        protected void btResumo_Unload(object sender, EventArgs e) {
            
        }
    }
}