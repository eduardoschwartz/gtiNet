using System;
using UIWeb.Models;

namespace UIWeb.Pages {
    public partial class cip : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
          //  txtNumDoc.Attributes.Add("onkeyup", "formataValor(this,' " + txtNumDoc.Text + " ')");
            if (!IsPostBack) {
                txtNumDoc.Text = "";
                lblMsg.Text = "";
            }

        }

        protected void btAcesso_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtNumDoc.Text))
                lblMsg.Text = "Erro: Digite o nº do documento.";
            else {
                int number;
                bool result = Int32.TryParse(txtNumDoc.Text, out number);
                if (result) {
                    ClearTable();
                    clsDebito debito_class = new clsDebito();
                    bool bExiste = debito_class.ExisteDocumentoCIP(number);
                    if (!bExiste)
                        lblMsg.Text = "Erro: Documento inválido.";
                    else
                        FillTable(number);
                } else
                    lblMsg.Text = "Erro: Documento inválido.";
            }
        }


        private void ClearTable() {
            IM.Text = "";
            NOME.Text = "";
            ENDERECOIMOVEL.Text = "";
            BAIRRO.Text = "";
        }

        private void FillTable(int NumDocumento) {
            clsDebito debito_class = new clsDebito();

            clsGlobal global_class = new clsGlobal();
            Dados_Basicos regDados = global_class.Retorna_Dados_Basicos(debito_class.CodigoCIP(NumDocumento));
            IM.Text = regDados.codigo_reduzido.ToString();
            NOME.Text = regDados.nome;
            ENDERECOIMOVEL.Text = regDados.endereco + ", " + regDados.numero.ToString() + " " + regDados.complemento.ToString();
            BAIRRO.Text = regDados.nome_bairro.ToString();
        }


    }
}