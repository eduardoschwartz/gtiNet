using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using gtiNet.BLL;
using gtiNet.DAL;
using gtiNet.Modelos;
using CrystalDecisions.CrystalReports.Engine;

namespace UIWeb.Pages {
    public partial class dam : System.Web.UI.Page {
        static decimal nValorTaxa;
        static bool bGerado;

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                bGerado = false;
                Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();
                DebitoBLL obj = new DebitoBLL();
                nValorTaxa = obj.TaxaExpediente(DateTime.Now.Year);
                ShowResult(false);
                txtVencto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lblMsg2.Text = "";
            }
        }

        protected void txtCod_TextChanged(object sender, EventArgs e) {
            ShowResult(false);
        }

        protected void btSelectAll_Click(object sender, EventArgs e) {
            bool isNum;
            decimal nSomaPrincipal = 0;
            decimal nSomaJuros = 0;
            decimal nSomaMulta = 0;
            decimal nSomaCorrecao = 0;
            decimal nSomaTotal = 0;
            decimal Num = 0;
            foreach (GridViewRow r in grdMain.Rows) {
                (r.FindControl("chkRow") as CheckBox).Checked = true;
                isNum = decimal.TryParse(r.Cells[7].Text, out Num);
                nSomaPrincipal += Num;
                isNum = decimal.TryParse(r.Cells[8].Text, out Num);
                nSomaJuros += Num;
                isNum = decimal.TryParse(r.Cells[9].Text, out Num);
                nSomaMulta += Num;
                isNum = decimal.TryParse(r.Cells[10].Text, out Num);
                nSomaCorrecao += Num;
                isNum = decimal.TryParse(r.Cells[11].Text, out Num);
                nSomaTotal += Num;
            }
            TableTotal.Rows[2].Cells[2].Text = nSomaPrincipal.ToString("#0.00");
            TableTotal.Rows[2].Cells[3].Text = nSomaMulta.ToString("#0.00");
            TableTotal.Rows[2].Cells[4].Text = nSomaJuros.ToString("#0.00");
            TableTotal.Rows[2].Cells[5].Text = nSomaCorrecao.ToString("#0.00");
            TableTotal.Rows[2].Cells[6].Text = nSomaTotal.ToString("#0.00");
            TableResumo.Rows[1].Cells[1].Text = (nSomaTotal + nValorTaxa).ToString("#0.00");
        }

        protected void chkRow_CheckedChanged(object sender, EventArgs e) {
            bool isNum;
            decimal nSomaPrincipal = 0;
            decimal nSomaJuros = 0;
            decimal nSomaMulta = 0;
            decimal nSomaCorrecao = 0;
            decimal nSomaTotal = 0;
            decimal Num = 0;

            CheckBox chk = (sender as CheckBox);
            GridView gv = chk.NamingContainer.Parent.Parent as GridView;
            foreach (GridViewRow row in gv.Rows) {
                if (row.RowType == DataControlRowType.DataRow) {
                    if ((row.FindControl("chkRow") as CheckBox).Checked) {
                        isNum = decimal.TryParse(row.Cells[7].Text, out Num);
                        nSomaPrincipal += Num;
                        isNum = decimal.TryParse(row.Cells[8].Text, out Num);
                        nSomaJuros += Num;
                        isNum = decimal.TryParse(row.Cells[9].Text, out Num);
                        nSomaMulta += Num;
                        isNum = decimal.TryParse(row.Cells[10].Text, out Num);
                        nSomaCorrecao += Num;
                        isNum = decimal.TryParse(row.Cells[11].Text, out Num);
                        nSomaTotal += Num;
                    }
                }
            }

            TableTotal.Rows[2].Cells[2].Text = nSomaPrincipal.ToString("#0.00");
            TableTotal.Rows[2].Cells[3].Text = nSomaMulta.ToString("#0.00");
            TableTotal.Rows[2].Cells[4].Text = nSomaJuros.ToString("#0.00");
            TableTotal.Rows[2].Cells[5].Text = nSomaCorrecao.ToString("#0.00");
            TableTotal.Rows[2].Cells[6].Text = nSomaTotal.ToString("#0.00");
            TableResumo.Rows[1].Cells[1].Text = (nSomaTotal + nValorTaxa).ToString("#0.00");
        }

        protected void btSelectNone_Click(object sender, EventArgs e) {
            foreach (GridViewRow r in grdMain.Rows)
                (r.FindControl("chkRow") as CheckBox).Checked = false;
            TableTotal.Rows[2].Cells[2].Text = "0,00";
            TableTotal.Rows[2].Cells[3].Text = "0,00";
            TableTotal.Rows[2].Cells[4].Text = "0,00";
            TableTotal.Rows[2].Cells[5].Text = "0,00";
            TableTotal.Rows[2].Cells[6].Text = "0,00";
            TableResumo.Rows[1].Cells[1].Text = "0,00";
        }

        protected void btConsultar_Click(object sender, ImageClickEventArgs e) {
            bool isNum = false;
            Int32 Num = 0;
            decimal nSomaPrincipal = 0;
            decimal nSomaJuros = 0;
            decimal nSomaMulta = 0;
            decimal nSomaCorrecao = 0;
            decimal nSomaTotal = 0;
            DateTime DataDAM;

            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", "alert('Sistema em Manutenção!')", true);
            //      return;

            bGerado = false;
            String sTextoImagem = txtimgcode.Text;
            txtimgcode.Text = "";
            Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();

            lblmsg.Text = "";
            lblMsg2.Text = "";

            if (optList.Items[0].Selected == true) {

                isNum = Int32.TryParse(txtCod.Text, out Num);
                if (!isNum) {
                    lblmsg.Text = "Código do imóvel inválido!";
                    return;
                } else {
                    Imovel reg = new Imovel();
                    reg.Codigo = Num;
                    ImovelBLL imovel = new ImovelBLL();
                    List<Imovel> lst = imovel.Listagem(reg, null);
                    if (lst.Count > 0) {
                        lblEndereco.Text = lst[0].LogradouroNome + ", " + lst[0].Numero;
                        lblDoc.Text = lst[0].BairroNome;
                        List<ProprietarioImovel> lstP = imovel.ListaProprietario(Num, true);
                        lblNome.Text = lstP[0].Nome;
                    } else {
                        lblmsg.Text = "Código do imóvel não cadastrado!";
                        return;
                    }
                }
            } else {
                if (optList.Items[1].Selected == true) {
                    isNum = Int32.TryParse(txtCod.Text, out Num);
                    if (!isNum) {
                        lblmsg.Text = "Código da empresa inválido!";
                        return;
                    } else {
                        Empresa reg = new Empresa();
                        reg.Codigo = Num;
                        EmpresaBLL empresa = new EmpresaBLL();
                        List<Empresa> lst = empresa.Listagem(reg, null);
                        if (lst.Count > 0) {
                            lblEndereco.Text = lst[0].RazaoSocial;
                            lblDoc.Text = lst[0].LogradouroNome + ", " + lst[0].Numero;
                            lblNome.Text = lst[0].BairroNome + " " + lst[0].CidadeNome + "/" + lst[0].UF;
                        } else {
                            lblmsg.Text = "Inscrição Municipal não cadastrada!";
                            return;
                        }
                    }
                } else {
                    if (optList.Items[2].Selected == true) {
                        isNum = Int32.TryParse(txtCod.Text, out Num);
                        if (!isNum) {
                            lblmsg.Text = "Código de contribuinte inválido!";
                            return;
                        } else {
                            if (Num < 500000 || Num > 700000) {
                                lblmsg.Text = "Código de contribuinte inválido!";
                                return;
                            } else {
                                Cidadao reg = new Cidadao();
                                reg.Codigo = Num;
                                CidadaoBLL cidadao = new CidadaoBLL();
                                List<Cidadao> lst = cidadao.Listagem(reg, null);
                                if (lst.Count > 0) {
                                    lblEndereco.Text = lst[0].LogradouroFora + ", " + lst[0].Numero;
                                    lblDoc.Text = lst[0].NomeBairro + " - " + lst[0].NomeCidade + "/" + lst[0].NomeUF;
                                    lblNome.Text = lst[0].Nome;
                                } else {
                                    lblmsg.Text = "Contribuinte não cadastrado!";
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            if (!DateTime.TryParse(txtVencto.Text, out DataDAM)) {
                lblmsg.Text = "Data de vencimento inválida.";
                ShowResult(false);
                return;
            } else {
                String sDataVencto = txtVencto.Text;
                String sDataNow = DateTime.Now.ToString("dd/MM/yyyy");
                if (DateTime.ParseExact(sDataVencto, "dd/MM/yyyy", null) < DateTime.ParseExact(sDataNow, "dd/MM/yyyy", null)) {
                    lblmsg.Text = "Vencimento menor que a data atual.";
                    ShowResult(false);
                    return;
                } else {
                    Int32 DifDias = ((TimeSpan)(DataDAM - DateTime.Now)).Days;
                    if (DifDias > 30) {
                        lblmsg.Text = "Vencimento máximo de 30 dias.";
                        ShowResult(false);
                        return;
                    }
                }
            }

            if (sTextoImagem.ToUpper() != this.Session["CaptchaImageText"].ToString().ToUpper()) {
                lblmsg.Text = "Código da imagem inválido.";
                ShowResult(false);
                return;
            } else {
                ShowResult(true);
                lblmsg.Text = "";
                lblMsg2.Text = "";
            }

            this.txtimgcode.Text = "";

            DebitoBLL obj = new DebitoBLL();
            String sDataDAM = DataDAM.ToString("dd/MM/yyyy");
            List<Debito> debitos = obj.Listagem(Num, DateTime.ParseExact(sDataDAM, "dd/MM/yyyy", null));
            List<Debito> debitos2 = new List<Debito>();

            foreach (var item in debitos) {
                //                if (item.Parcela != 0 && item.Situacao == 3) {
                if (item.Situacao == 3) {
                    bool bFind = false;
                    int nPos = 0;
                    foreach (var item2 in debitos2) {

                        if (item2.Exercicio == item.Exercicio && item2.Lancamento == item.Lancamento && item2.Sequencia == item.Sequencia &&
                            item2.Parcela == item.Parcela && item2.Complemento == item.Complemento) {
                            bFind = true;
                            break;
                        }
                        nPos += 1;
                    }
                    if (bFind) {
                        debitos2[nPos].VlTributo += item.VlTributo;
                        debitos2[nPos].VlJuros += item.VlJuros;
                        debitos2[nPos].VlMulta += item.VlMulta;
                        debitos2[nPos].VlCorrecao += item.VlCorrecao;
                        debitos2[nPos].VlTotal += item.VlTotal;
                    } else {
                        Debito reg = new Debito();
                        reg.Codigo = item.Codigo;
                        reg.Exercicio = item.Exercicio;
                        reg.Lancamento = item.Lancamento;
                        reg.DescLancamento = item.DescLancamento;
                        reg.Sequencia = item.Sequencia;
                        reg.Parcela = item.Parcela;
                        reg.Complemento = item.Complemento;
                        reg.DtVencimento = item.DtVencimento;
                        reg.Situacao = item.Situacao;
                        reg.VlTributo = item.VlTributo;
                        reg.VlJuros = item.VlJuros;
                        reg.VlMulta = item.VlMulta;
                        reg.VlCorrecao = item.VlCorrecao;
                        reg.VlTotal = item.VlTotal;
                        reg.DtAjuiza = item.DtAjuiza;
                        debitos2.Add(reg);
                    }
                }
            }

            if (debitos2.Count == 0) {
                lblDoc.Text = "";
                lblmsg.Text = "Não existem débitos.";
                ShowResult(false);
                return;
            }

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[12] { new DataColumn("Exercicio"), new DataColumn("Lancamento"), new DataColumn("Sequencia"),
                                new DataColumn("Parcela"),new DataColumn("Complemento"),new DataColumn("DtVencimento"),new DataColumn("VlPrincipal"),
                                new DataColumn("VlJuros"),new DataColumn("VlMulta"),new DataColumn("VlCorrecao"),new DataColumn("VlTotal"),new DataColumn("DtAjuiza")});

            foreach (var item in debitos2) {
                dt.Rows.Add(item.Exercicio.ToString(), item.Lancamento.ToString("000") + "-" + item.DescLancamento.ToString(), item.Sequencia.ToString(),
                            item.Parcela.ToString(), item.Complemento.ToString(), item.DtVencimento.ToString("dd/MM/yyyy"),
                            item.VlTributo.ToString("#0.00"), item.VlJuros.ToString("#0.00"), item.VlMulta.ToString("#0.00"),
                            item.VlCorrecao.ToString("#0.00"), item.VlTotal.ToString("#0.00"), item.DtAjuiza == DateTime.MinValue ? "NÃO" : "SIM");
                nSomaPrincipal += (decimal)item.VlTributo;
                nSomaJuros += (decimal)item.VlJuros;
                nSomaMulta += (decimal)item.VlMulta;
                nSomaCorrecao += (decimal)item.VlCorrecao;
                nSomaTotal += (decimal)item.VlTotal;
            }

            grdMain.DataSource = dt;
            grdMain.DataBind();

            TableTotal.Rows[1].Cells[2].Text = nSomaPrincipal.ToString("#0.00");
            TableTotal.Rows[1].Cells[3].Text = nSomaMulta.ToString("#0.00");
            TableTotal.Rows[1].Cells[4].Text = nSomaJuros.ToString("#0.00");
            TableTotal.Rows[1].Cells[5].Text = nSomaCorrecao.ToString("#0.00");
            TableTotal.Rows[1].Cells[6].Text = nSomaTotal.ToString("#0.00");

            TableTotal.Rows[2].Cells[2].Text = "0,00";
            TableTotal.Rows[2].Cells[3].Text = "0,00";
            TableTotal.Rows[2].Cells[4].Text = "0,00";
            TableTotal.Rows[2].Cells[5].Text = "0,00";
            TableTotal.Rows[2].Cells[6].Text = "0,00";
            TableResumo.Rows[0].Cells[1].Text = nValorTaxa.ToString("#0.00");
            TableResumo.Rows[1].Cells[1].Text = "0,00";
            TableResumo.Rows[2].Cells[1].Text = txtVencto.Text;
        }

        protected void optList_SelectedIndexChanged(object sender, EventArgs e) {
            if (optList.Items[0].Selected == true) {
                lblCod.Text = "Código do imóvel..:";
                txtCod.Width = 70;
            } else if (optList.Items[1].Selected == true) {
                lblCod.Text = "Inscrição Municipal...";
                txtCod.Width = 70;
            } else if (optList.Items[2].Selected == true) {
                lblCod.Text = "Código cidadão...";
                txtCod.Width = 120;
            }
            ShowResult(false);
        }

        private void ShowResult(bool bShow) {
            TableTotal.Rows[1].Cells[2].Text = "0,00";
            TableTotal.Rows[1].Cells[3].Text = "0,00";
            TableTotal.Rows[1].Cells[4].Text = "0,00";
            TableTotal.Rows[1].Cells[5].Text = "0,00";
            TableTotal.Rows[1].Cells[6].Text = "0,00";
            TableTotal.Rows[2].Cells[2].Text = "0,00";
            TableTotal.Rows[2].Cells[3].Text = "0,00";
            TableTotal.Rows[2].Cells[4].Text = "0,00";
            TableTotal.Rows[2].Cells[5].Text = "0,00";
            TableTotal.Rows[2].Cells[6].Text = "0,00";
            TableResumo.Rows[1].Cells[1].Text = "0,00";
            pnlTotal.Visible = bShow;
            Pnlresumo.Visible = bShow;
            btPrint.Visible = bShow;
            btSelectAll.Visible = bShow;
            btSelectNone.Visible = bShow;
            if (!bShow) {
                grdMain.DataSource = null;
                grdMain.DataBind();
                lblNome.Text = "";
                lblDoc.Text = "";
                lblEndereco.Text = "";
                lblValidate.Text = "";

            }
        }

        protected void btPrint_Click(object sender, EventArgs e) {
            bool bParcUnica = false;
            bool bParcNormal = false;
            lblmsg.Text = "";
            lblMsg2.Text = "";
            if (bGerado) {
                ShowResult(false);
                Response.Write("<script>alert('A guia já foi gerada!');</script>");

                //lblmsg.Text = "A guia já foi gerada!";
                return;
            }
            if (TableResumo.Rows[1].Cells[1].Text == "0,00")
                lblMsg2.Text = "Selecione os débitos que deseja pagar.";
            else
                foreach (GridViewRow row in grdMain.Rows) {
                    if (row.RowType == DataControlRowType.DataRow) {

                        if ((row.FindControl("chkRow") as CheckBox).Checked) {
                            if (Convert.ToInt16(row.Cells[4].Text) == 0)
                                bParcUnica = true;
                            if (Convert.ToInt16(row.Cells[4].Text) != 0)
                                bParcNormal = true;

                            if (Convert.ToInt16(row.Cells[2].Text.Substring(0, 3)) == 5) {
                                if (Convert.ToDateTime(row.Cells[6].Text) > Convert.ToDateTime("05/01/2015")) {
                                    bGerado = false;
                                    //                       Response.Write("<script>alert('ISS Variável com vencimento após 01/05/2015 não pode ser pago por DAM!');</script>");
                                    lblMsg2.Text = "ISS Variável com vencimento após 01/05/2015 não pode ser pago por DAM.";
                                    return;
                                }
                            } else
                                if (row.Cells[12].Text == "SIM") {
                                bGerado = false;
                                lblMsg2.Text = "Débitos ajuizados não podem ser pagos através de DAM.";
                                return;
                            }
                        }
                    }
                }
            if (bParcUnica && bParcNormal)
                lblMsg2.Text = "Parcela ùnica não pode ser paga junto com outras parcelas.";

            else
                GeraGuia();
        }


        private void GeraGuia() {
            decimal tmpNumber = 0;
            StringBuilder sFullTrib;
            bGerado = true;

            List<Debito> lstExtrato = new List<Debito>();
            DebitoBLL objDebitoBLL = new DebitoBLL();
            Decimal nTaxaExp = objDebitoBLL.TaxaExpediente(DateTime.Now.Year);
            Debito reg;
            foreach (GridViewRow row in grdMain.Rows) {
                if (row.RowType == DataControlRowType.DataRow) {
                    if ((row.FindControl("chkRow") as CheckBox).Checked) {
                        reg = new Debito();
                        reg.Codigo = Convert.ToInt32(txtCod.Text);
                        reg.Exercicio = Convert.ToInt32(row.Cells[1].Text);
                        reg.Lancamento = Convert.ToInt16(row.Cells[2].Text.Substring(0, 3));
                        reg.Sequencia = Convert.ToInt16(row.Cells[3].Text);
                        reg.Parcela = Convert.ToInt16(row.Cells[4].Text);
                        reg.Complemento = Convert.ToInt16(row.Cells[5].Text);
                        reg.DescLancamento = row.Cells[2].Text.Substring(4, row.Cells[2].Text.ToString().Length - 4);
                        reg.DtVencimento = Convert.ToDateTime(row.Cells[6].Text);
                        decimal.TryParse(row.Cells[7].Text, out tmpNumber);
                        reg.VlTributo = tmpNumber;
                        decimal.TryParse(row.Cells[8].Text, out tmpNumber);
                        reg.VlJuros = tmpNumber;
                        decimal.TryParse(row.Cells[9].Text, out tmpNumber);
                        reg.VlMulta = tmpNumber;
                        decimal.TryParse(row.Cells[10].Text, out tmpNumber);
                        reg.VlCorrecao = tmpNumber;
                        decimal.TryParse(row.Cells[11].Text, out tmpNumber);
                        reg.VlTotal = tmpNumber;

                        sFullTrib = new StringBuilder();

                        List<Debito> ListaTrib = objDebitoBLL.ListaTributos(reg);
                        foreach (Debito Trib in ListaTrib) {
                            if (sFullTrib.ToString().IndexOf(Trib.CodigoTributo.ToString("000")) == -1) {
                                String CodTributo = Trib.CodigoTributo.ToString("000");
                                String DescTributo = Trib.DescTributo;
                                sFullTrib.Append(CodTributo + "-" + DescTributo + "/");
                            }
                        }
                        sFullTrib.Remove(sFullTrib.Length - 1, 1);
                        reg.DescTributo = sFullTrib.ToString();

                        lstExtrato.Add(reg);
                    }
                }
            }

            //taxa de expediente
            reg = new Debito();
            reg.Codigo = Convert.ToInt32(txtCod.Text);
            reg.Exercicio = DateTime.Now.Year;
            reg.Lancamento = 4;
            reg.Sequencia = 0;
            reg.Parcela = 1;
            reg.Complemento = 0;
            reg.DtVencimento = Convert.ToDateTime(txtVencto.Text);
            reg.DescTributo = "003-TAXA EXP.DOC.";
            reg.DescLancamento = "";
            reg.VlTributo = nTaxaExp;
            reg.VlJuros = 0;
            reg.VlMulta = 0;
            reg.VlCorrecao = 0;
            reg.VlTotal = nTaxaExp;
            lstExtrato.Add(reg);

            float nValorGuia = 0;
            float.TryParse(TableTotal.Rows[2].Cells[6].Text, out nValorGuia);
            Int32 NumDoc = objDebitoBLL.GravaNovoDocumento(nValorGuia);
            objDebitoBLL.GravaDAMWeb(NumDoc);
            // NumDoc = Convert.ToInt32(txtimgcode.Text);

            foreach (Debito Lanc in lstExtrato) {
                objDebitoBLL.GravaParcelaDocumento(Lanc, NumDoc);
            }

            String sDataDAM = txtVencto.Text;
            Int32 nSID = objDebitoBLL.GravaBoletoDAM(lstExtrato, NumDoc, DateTime.ParseExact(sDataDAM, "dd/MM/yyyy", null));

            Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();

            DebitoBLL obj = new DebitoBLL();
            DataTable lista = obj.FillDataTableBoleto(nSID);
            ReportDocument crystalReport = new ReportDocument();
            crystalReport.Load(Server.MapPath("~/Report/boletodam.rpt"));

            crystalReport.SetDataSource(lista);

            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            objDebitoBLL.DeleteSID(nSID);

            try {
                crystalReport.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "DAM" + nSID.ToString());
            } catch {
            } finally {
                crystalReport.Close();
                crystalReport.Dispose();
            }
        }





    }
}