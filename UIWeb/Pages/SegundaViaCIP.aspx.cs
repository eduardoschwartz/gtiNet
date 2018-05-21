using System;
using System.Collections.Generic;
using UIWeb.Models;


namespace UIWeb.Pages {
    public partial class SegundaViaCIP : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void btPrint_Click(object sender, EventArgs e) {
            int Num = 0;
            String sTextoImagem = txtimgcode.Text;
            txtimgcode.Text = "";
            clsImovel Imovel_class = new clsImovel();
            bool isNum = Int32.TryParse(txtCod.Text, out Num);
            if (!isNum) {
                lblmsg.Text = "Código do imóvel inválido!";
                return;
            } else {
                bool bExiste = Imovel_class.ExisteImovel(Num);
                if (!bExiste) {
                    lblmsg.Text = "Código do imóvel inválido!";
                    return;
                } else {
                    if (String.IsNullOrWhiteSpace(txtIC.Text)) {
                        lblmsg.Text = "Inscrição cadastral obrigatória!";
                        return;
                    } else {
                        ImovelStruct reg = Imovel_class.LoadReg(Num);
                        if (txtIC.Text != reg.Inscricao) {
                            lblmsg.Text = "Inscrição cadastral obrigatória!";
                            return;
                        }
                    }
                }
            }

            if (sTextoImagem.ToUpper() != this.Session["CaptchaImageText"].ToString().ToUpper()) {
                lblmsg.Text = "Código da imagem inválido.";
                return;
            }

            lblmsg.Text = "";
            this.txtimgcode.Text = "";
            int nSid = gravaCarne();
            if (nSid > 0) {
                Session["sid"] = nSid;
                Response.Redirect("~/Pages/SegundaViaCIPFim.aspx");
            }
        }


        private int gravaCarne() {
            int nSid = gtiCore.GetRandomNumber();
            int nImovel = Convert.ToInt32(txtCod.Text);
            clsDebito Debito_Class = new clsDebito();
            clsImovel Imovel_class = new clsImovel();
            List<DebitoStructure> Extrato_Lista = Debito_Class.ListaParcelasCIP(nImovel, 2018);
            if (Extrato_Lista.Count == 0) {
                lblmsg.Text = "Não é possível emitir segunda via para este código";
                return 0;
            }
            short nSeq = 0;
            foreach (DebitoStructure item in Extrato_Lista) {
                ImovelStruct dados_imovel = Imovel_class.LoadReg(item.Codigo_Reduzido);
                List<ProprietarioStruct> lstProprietario = Imovel_class.ListaProprietario(item.Codigo_Reduzido, true);
                boletoguia reg = new boletoguia();
                reg.usuario = "Gti.Web/2ViaIPTU";
                reg.computer = "web";
                reg.sid = nSid;
                reg.seq = nSeq;
                reg.codreduzido = item.Codigo_Reduzido.ToString("000000");
                reg.nome = lstProprietario[0].Nome;
                reg.cpf = lstProprietario[0].CPF;
                reg.endereco = dados_imovel.NomeLogradouro;
                reg.numimovel = dados_imovel.Numero;
                reg.complemento = dados_imovel.Complemento.Length > 10 ? dados_imovel.Complemento.Substring(0, 10) : dados_imovel.Complemento;
                reg.bairro = dados_imovel.NomeBairro;
                reg.cidade = "JABOTICABAL";
                reg.uf = "SP";
                reg.desclanc = "CONTRIBUIÇÃO DE ILUMINAÇÃO PÚBLICA (CIP-2018)";
                reg.fulllanc = "CONTRIBUIÇÃO DE ILUMINAÇÃO PÚBLICA (CIP-2018)";
                reg.numdoc = item.Numero_Documento.ToString();
                reg.numparcela = item.Numero_Parcela;

                reg.datavencto = Convert.ToDateTime(item.Data_Vencimento);
                reg.numdoc2 = item.Numero_Documento.ToString();
                reg.digitavel = "linha digitavel";
                reg.valorguia = Convert.ToDecimal(item.Soma_Principal);
                laseriptu RegIPTU = Debito_Class.CarregaIPTU(item.Codigo_Reduzido, 2018);
                //reg.totparcela = RegIPTU.qtdeparc;
                //string sFullLanc = "Dados do Imovel:" + Environment.NewLine + Environment.NewLine + "Área do terreno: " + string.Format("{0:#.00}", Convert.ToDecimal(RegIPTU.areaterreno.ToString())) + " m²";
                //sFullLanc += Environment.NewLine + "Área construída: " + string.Format("{0:#.00}", Convert.ToDecimal(RegIPTU.areaconstrucao.ToString())) + " m²";
                //sFullLanc += Environment.NewLine + "Testada principal: " + string.Format("{0:#.00}", Convert.ToDecimal(RegIPTU.testadaprinc.ToString())) + " m";
                //sFullLanc += Environment.NewLine + "Valor venal territorial: R$ " + string.Format("{0:#.00}", Convert.ToDecimal(RegIPTU.vvt.ToString()));
                //sFullLanc += Environment.NewLine + "Valor venal predial: R$ " + string.Format("{0:#.00}", Convert.ToDecimal(RegIPTU.vvc.ToString()));
                //sFullLanc += Environment.NewLine + "Valor venal imóvel: R$ " + string.Format("{0:#.00}", Convert.ToDecimal(RegIPTU.vvi.ToString()));
                //sFullLanc += Environment.NewLine + "Valor IPTU parcelado: R$ " + string.Format("{0:#.00}", Convert.ToDecimal((RegIPTU.valortotalparc * RegIPTU.qtdeparc).ToString()));
                //sFullLanc += Environment.NewLine + "Valor IPTU único: R$ " + string.Format("{0:#.00}", Convert.ToDecimal(RegIPTU.valortotalunica.ToString()));

                reg.totparcela = 3;
                reg.obs = "";
                reg.numproc = "Q:" + dados_imovel.QuadraOriginal.ToString().Trim() + " L:" + dados_imovel.LoteOriginal.ToString().Trim();
                reg.cep = dados_imovel.Cep;

                //*** CÓDIGO DE BARRAS ***

                decimal nValorguia = Math.Truncate(Convert.ToDecimal(reg.valorguia * 100));
                string NumBarra = gtiCore.Gera2of5Cod((nValorguia).ToString(), Convert.ToDateTime(item.Data_Vencimento), Convert.ToInt32(reg.numdoc), Convert.ToInt32(reg.codreduzido));
                reg.numbarra2a = NumBarra.Substring(0, 13);
                reg.numbarra2b = NumBarra.Substring(13, 13);
                reg.numbarra2c = NumBarra.Substring(26, 13);
                reg.numbarra2d = NumBarra.Substring(39, 13);
                string strBarra = gtiCore.Gera2of5Str(reg.numbarra2a.Substring(0, 11) + reg.numbarra2b.Substring(0, 11) + reg.numbarra2c.Substring(0, 11) + reg.numbarra2d.Substring(0, 11));
                reg.codbarra = gtiCore.Mask(strBarra);

                Debito_Class.InsertBoletoGuia(reg);

                segunda_via_web reg_sv = new segunda_via_web();
                reg_sv.numero_documento = Convert.ToInt32(item.Numero_Documento);
                reg_sv.data = DateTime.Now;
                Debito_Class.Insert_Numero_Segunda_Via(reg_sv);

                nSeq++;
            }

            return nSid;
        }




    }
}