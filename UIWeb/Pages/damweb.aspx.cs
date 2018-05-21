using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using UIWeb.Models;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace UIWeb.Pages {
    public partial class damweb : System.Web.UI.Page {
        static bool bGerado;
        static bool bRefis = false;
        private static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };


        protected void Page_Load(object sender, EventArgs e) {
            DateTime DataDAM;
            if (!IsPostBack) {
                bGerado = false;
                ShowResult(false);
                String s = Request.QueryString["d"];
                lblVenctoDam.Text =this.Decrypt(s);


                if (!DateTime.TryParse(lblVenctoDam.Text, out DataDAM)) {
                    Response.Redirect("~/Pages/gtiMenu.aspx");
                } else {
                    String sDataVencto = lblVenctoDam.Text;
                    String sDataNow = DateTime.Now.ToString("dd/MM/yyyy");
                    if (DateTime.ParseExact(sDataVencto, "dd/MM/yyyy", null) < DateTime.ParseExact(sDataNow, "dd/MM/yyyy", null)) {
                        Response.Redirect("~/Pages/gtiMenu.aspx");
                    } else {
                        Int32 DifDias = ((TimeSpan)(DataDAM - DateTime.Now)).Days;
                        if (DifDias > 30) {
                            Response.Redirect("~/Pages/gtiMenu.aspx");
                        } 
                    }
                }

                //txtVencto.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
            TableResumo.Rows[0].Cells[1].Text = (nSomaTotal).ToString("#0.00");
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
            TableResumo.Rows[0].Cells[1].Text = (nSomaTotal).ToString("#0.00");
        }

        protected void btSelectNone_Click(object sender, EventArgs e) {
            foreach (GridViewRow r in grdMain.Rows)
                (r.FindControl("chkRow") as CheckBox).Checked = false;
            TableTotal.Rows[2].Cells[2].Text = "0,00";
            TableTotal.Rows[2].Cells[3].Text = "0,00";
            TableTotal.Rows[2].Cells[4].Text = "0,00";
            TableTotal.Rows[2].Cells[5].Text = "0,00";
            TableTotal.Rows[2].Cells[6].Text = "0,00";
            TableResumo.Rows[0].Cells[1].Text = "0,00";
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
            TableResumo.Rows[0].Cells[1].Text = "0,00";
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

        protected void btConsultar_Click(object sender, ImageClickEventArgs e) {
            bool isNum = false;
            int Num = 0;
            decimal nSomaPrincipal = 0;
            decimal nSomaJuros = 0;
            decimal nSomaMulta = 0;
            decimal nSomaCorrecao = 0;
            decimal nSomaTotal = 0;
            string num_cpf_cnpj = "";
            DateTime DataDAM;

            bGerado = false;
            String sTextoImagem = txtimgcode.Text;
            txtimgcode.Text = "";

            lblmsg.Text = "";
            lblMsg2.Text = "";
            lblNome.Text = "";
            lblDoc.Text = "";
            lblEndereco.Text = "";
            lblValidate.Text = "";


            if (optCPF.Checked && txtCPF.Text.Length < 14) {
                lblmsg.Text = "CPF inválido!";
                ShowResult(false);
                return;
            }
            if (optCNPJ.Checked && txtCNPJ.Text.Length < 18) {
                lblmsg.Text = "CNPJ inválido!";
                ShowResult(false);
                return;
            }

            if (optCPF.Checked) {
                num_cpf_cnpj = gtiCore.RetornaNumero(txtCPF.Text);
                if (!gtiCore.ValidaCpf(num_cpf_cnpj)) {
                    lblmsg.Text = "CPF inválido!";
                    ShowResult(false);
                    return;
                }
            } else {
                num_cpf_cnpj = gtiCore.RetornaNumero(txtCNPJ.Text);
                if (!gtiCore.ValidaCNPJ(num_cpf_cnpj)) {
                    lblmsg.Text = "CNPJ inválido!";
                    ShowResult(false);
                    return;
                }
            }


            if (optList.Items[0].Selected == true) {

                isNum = int.TryParse(txtCod.Text, out Num);
                if (!isNum) {
                    lblmsg.Text = "Código do imóvel inválido!";
                    ShowResult(false);
                    return;
                } else {
                    clsImovel imovel_class = new clsImovel();
                    bool bFind = imovel_class.ExisteImovel(Num);
                    if (bFind) {
                        ImovelStruct reg = imovel_class.LoadReg(Num);
                        List<ProprietarioStruct> regProp = imovel_class.ListaProprietario(Num, true);

                        lblEndereco.Text = reg.NomeLogradouro + ", " + reg.Numero + " " + reg.Complemento;
                        lblDoc.Text = reg.NomeBairro;
                        lblNome.Text = regProp[0].Nome;
                        if (optCPF.Checked) {
                            if (Convert.ToInt64(gtiCore.RetornaNumero(regProp[0].CPF)).ToString("00000000000") != num_cpf_cnpj) {
                                lblmsg.Text = "CPF não pertence ao proprietário deste imóvel!";
                                ShowResult(false);
                                return;
                            }
                        } else {
                            if (Convert.ToInt64(gtiCore.RetornaNumero(regProp[0].CPF)).ToString("00000000000000") != num_cpf_cnpj) {
                                lblmsg.Text = "CNPJ não pertence ao proprietário deste imóvel!";
                                ShowResult(false);
                                return;
                            }
                        }
                    } else {
                        lblmsg.Text = "Código do imóvel não cadastrado!";
                        ShowResult(false);
                        return;
                    }
                }
            } else {
                if (optList.Items[1].Selected == true) {
                    isNum = Int32.TryParse(txtCod.Text, out Num);
                    if (!isNum) {
                        lblmsg.Text = "Código da empresa inválido!";
                        ShowResult(false);
                        return;
                    } else {
                        clsEmpresa empresa_class = new clsEmpresa();
                        bool bFind = empresa_class.ExisteEmpresa(Num);
                        if (bFind) {
                            EmpresaStruct reg = empresa_class.LoadReg(Num);
                            lblEndereco.Text = reg.Endereco + ", " + reg.Numero + " " + reg.Complemento;
                            lblDoc.Text = reg.NomeBairro;
                            lblNome.Text = reg.RazaoSocial;

                            if (optCPF.Checked) {
                                if (Convert.ToInt64(gtiCore.RetornaNumero(reg.cpf_cnpj)).ToString("00000000000") != num_cpf_cnpj) {
                                    lblmsg.Text = "CPF não pertence ao proprietário deste imóvel!";
                                    ShowResult(false);
                                    return;
                                }
                            } else {
                                if (Convert.ToInt64(gtiCore.RetornaNumero(reg.cpf_cnpj)).ToString("00000000000000") != num_cpf_cnpj) {
                                    lblmsg.Text = "CNPJ não pertence ao proprietário deste imóvel!";
                                    ShowResult(false);
                                    return;
                                }
                            }
                        } else {
                            lblmsg.Text = "Inscrição Municipal não cadastrada!";
                            ShowResult(false);
                            return;
                        }
                    }
                } else {
                    if (optList.Items[2].Selected == true) {
                        isNum = Int32.TryParse(txtCod.Text, out Num);
                        if (!isNum) {
                            lblmsg.Text = "Código de contribuinte inválido!";
                            ShowResult(false);
                            return;
                        } else {
                            if (Num < 500000 || Num > 700000) {
                                lblmsg.Text = "Código de contribuinte inválido!";
                                ShowResult(false);
                                return;
                            } else {
                                clsCidadao cidadao_class = new clsCidadao();
                                bool bFind = cidadao_class.ExisteCidadao(Num);
                                if (bFind) {
                                    CidadaoStruct reg = cidadao_class.LoadReg(Num);
                                    if (reg.EtiquetaR != null && reg.EtiquetaR == "S") {
                                        lblEndereco.Text = reg.EnderecoR + ", " + reg.NumeroR + " " + reg.ComplementoR;
                                        lblDoc.Text = reg.NomeBairroR;

                                    } else {
                                        lblEndereco.Text = reg.EnderecoC + ", " + reg.NumeroC + " " + reg.ComplementoC;
                                        lblDoc.Text = reg.NomeBairroC;
                                    }
                                    lblNome.Text = reg.Nome;

                                    if (optCPF.Checked) {
                                        if (Convert.ToInt64(gtiCore.RetornaNumero(reg.Cpf)).ToString("00000000000") != num_cpf_cnpj) {
                                            lblmsg.Text = "CPF não pertence ao proprietário deste imóvel!";
                                            ShowResult(false);
                                            return;
                                        }
                                    } else {
                                        if (Convert.ToInt64(gtiCore.RetornaNumero(reg.Cnpj)).ToString("00000000000000") != num_cpf_cnpj) {
                                            lblmsg.Text = "CNPJ não pertence ao proprietário deste imóvel!";
                                            ShowResult(false);
                                            return;
                                        }
                                    }

                                } else {
                                    lblmsg.Text = "Contribuinte não cadastrado!";
                                    ShowResult(false);
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            if (!DateTime.TryParse(lblVenctoDam.Text, out DataDAM)) {
                lblmsg.Text = "Data de vencimento inválida.";
                ShowResult(false);
                return;
            } else {
                String sDataVencto = lblVenctoDam.Text;
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

            String sDataDAM = DataDAM.ToString("dd/MM/yyyy");
            clsDebito debito_class = new clsDebito();
            List<DebitoStructure> debitos = debito_class.Extrato(Num, 1990, 2050,  0, 99, 0, 99, 0, 999, 0, 99, 0, 99, DateTime.ParseExact(sDataDAM, "dd/MM/yyyy", null), 0);
            List<DebitoStructure> debitos2 = new List<DebitoStructure>();

            foreach (var item in debitos) {
                if (item.Codigo_Situacao == 3 || item.Codigo_Situacao == 19 || item.Codigo_Situacao == 38 || item.Codigo_Situacao == 39) {
                    DebitoStructure reg = new DebitoStructure();
                    reg.Codigo_Reduzido = item.Codigo_Reduzido;
                    reg.Ano_Exercicio = item.Ano_Exercicio;
                    reg.Codigo_Lancamento = Convert.ToInt16(item.Codigo_Lancamento);
                    reg.Descricao_Lancamento = item.Descricao_Lancamento;
                    reg.Sequencia_Lancamento = Convert.ToInt16(item.Sequencia_Lancamento);
                    reg.Numero_Parcela = Convert.ToInt16(item.Numero_Parcela);
                    reg.Complemento = item.Complemento;
                    reg.Data_Vencimento = Convert.ToDateTime(item.Data_Vencimento);
                    reg.Codigo_Situacao = Convert.ToInt16(item.Codigo_Situacao);
                    reg.Soma_Principal = item.Soma_Principal;
                    reg.Soma_Juros = item.Soma_Juros;
                    reg.Soma_Multa = item.Soma_Multa;
                    reg.Soma_Correcao = item.Soma_Correcao;
                    reg.Soma_Total = item.Soma_Total;
                    reg.Data_Ajuizamento = item.Data_Ajuizamento;

                    debitos2.Add(reg);
                }
            }

            if (debitos2.Count == 0) {
                lblDoc.Text = "";
                lblmsg.Text = "Não existem débitos.";
                ShowResult(false);
                return;
            }

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[13] { new DataColumn("Exercicio"), new DataColumn("Lancamento"), new DataColumn("Sequencia"),
                                new DataColumn("Parcela"),new DataColumn("Complemento"),new DataColumn("DtVencimento"),new DataColumn("VlPrincipal"),
                                new DataColumn("VlJuros"),new DataColumn("VlMulta"),new DataColumn("VlCorrecao"),new DataColumn("VlTotal"),new DataColumn("DtAjuiza"),new DataColumn("Protesto")});

            foreach (var item in debitos2) {
                dt.Rows.Add(item.Ano_Exercicio.ToString(), item.Codigo_Lancamento.ToString("000") + "-" + item.Descricao_Lancamento.ToString(), item.Sequencia_Lancamento.ToString(),
                            item.Numero_Parcela.ToString(), item.Complemento.ToString(), Convert.ToDateTime(item.Data_Vencimento).ToString("dd/MM/yyyy"),
                            item.Soma_Principal.ToString("#0.00"), item.Soma_Juros.ToString("#0.00"), item.Soma_Multa.ToString("#0.00"),
                            item.Soma_Correcao.ToString("#0.00"), item.Soma_Total.ToString("#0.00"), item.Data_Ajuizamento == DateTime.MinValue ? "NÃO" : "SIM", item.Codigo_Situacao ==38| item.Codigo_Situacao == 39 ? "SIM" : "NÃO");
                nSomaPrincipal += (decimal)item.Soma_Principal;
                nSomaJuros += (decimal)item.Soma_Juros;
                nSomaMulta += (decimal)item.Soma_Multa;
                nSomaCorrecao += (decimal)item.Soma_Correcao;
                nSomaTotal += (decimal)item.Soma_Total;
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
            TableResumo.Rows[0].Cells[1].Text = "0,00";
            TableResumo.Rows[1].Cells[1].Text = lblVenctoDam.Text;
        }

        protected void btPrint_Click(object sender, EventArgs e) {
            bool bParcUnica = false;
            bool bParcNormal = false;
            bool bAnoAtual = false;
            bool bAnoAnterior = false;
            

            lblmsg.Text = "";
            lblMsg2.Text = "";
            if (bGerado) {
                ShowResult(false);
                Response.Write("<script>alert('A guia já foi gerada!');</script>");
                return;
            }



            if (TableResumo.Rows[0].Cells[1].Text == "0,00")
                lblMsg2.Text = "Selecione os débitos que deseja pagar.";
            else
                foreach (GridViewRow row in grdMain.Rows) {
                    if (row.RowType == DataControlRowType.DataRow) {

                        if ((row.FindControl("chkRow") as CheckBox).Checked) {
                            if (Convert.ToInt16(row.Cells[4].Text) == 0)
                                bParcUnica = true;
                            if (Convert.ToInt16(row.Cells[4].Text) != 0)
                                bParcNormal = true;
                            if (Convert.ToDateTime(row.Cells[6].Text).Year < 2017)
                                bAnoAnterior = true;
                            if (Convert.ToDateTime(row.Cells[6].Text).Year >= 2017)
                                bAnoAtual = true;

                            //                            if (Convert.ToInt16(row.Cells[2].Text.Substring(0, 3)) == 5) {
                            //                                if (Convert.ToDateTime(row.Cells[6].Text) > Convert.ToDateTime("05/01/2015")) {
                            //                                    bGerado = false;
                            //                                    lblMsg2.Text = "ISS Variável com vencimento após 01/05/2015 não pode ser pago por DAM.";
                            //                                    return;
                            //                                }
                            //                            } else
                            if (row.Cells[12].Text == "SIM") {
                                bGerado = false;
                                lblMsg2.Text = "Débitos ajuizados não podem ser pagos através de DAM.";
                                return;
                            }
                            if (row.Cells[13].Text == "SIM") {
                                bGerado = false;
                                lblMsg2.Text = "Débitos protestados ou enviados para protesto não podem ser pagos através de DAM.";
                                return;
                            }
                        }
                    }
                }
            if (bRefis) {
                if (bAnoAnterior && bAnoAtual) {
                    bGerado = false;
                    clsGlobal.nPlano = 0;
                    lblMsg2.Text = "Não é possível pagar débitos débitos de 2017 com outros anos pelo Refis.";
                    return;
                }
                if (!bAnoAnterior && bAnoAtual) {
                    clsGlobal.nPlano = 0;
                }

            }

            if (bParcUnica && bParcNormal) {
                lblMsg2.Text = "Parcela ùnica não pode ser paga junto com outras parcelas.";
            } else {
                  GeraGuia();
            }
                

        }

        private void GeraGuia() {
            decimal tmpNumber = 0;
            bGerado = true;
            clsDebito debito_class = new clsDebito();
            List<DebitoStructure> lstExtrato = new List<DebitoStructure>();
            DebitoStructure reg;
            foreach (GridViewRow row in grdMain.Rows) {
                if (row.RowType == DataControlRowType.DataRow) {
                    if ((row.FindControl("chkRow") as CheckBox).Checked) {
                        reg = new DebitoStructure();
                        reg.Codigo_Reduzido = Convert.ToInt32(txtCod.Text);
                        reg.Ano_Exercicio = Convert.ToInt32(row.Cells[1].Text);
                        reg.Codigo_Lancamento = Convert.ToInt16(row.Cells[2].Text.Substring(0, 3));
                        reg.Sequencia_Lancamento = Convert.ToInt16(row.Cells[3].Text);
                        reg.Numero_Parcela = Convert.ToInt16(row.Cells[4].Text);
                        reg.Complemento = Convert.ToInt16(row.Cells[5].Text);
                        reg.Descricao_Lancamento = row.Cells[2].Text.Substring(4, row.Cells[2].Text.ToString().Length - 4);
                        reg.Data_Vencimento = Convert.ToDateTime(row.Cells[6].Text);
                        decimal.TryParse(row.Cells[7].Text, out tmpNumber);
                        reg.Soma_Principal = tmpNumber;
                        decimal.TryParse(row.Cells[8].Text, out tmpNumber);
                        reg.Soma_Juros = tmpNumber;
                        decimal.TryParse(row.Cells[9].Text, out tmpNumber);
                        reg.Soma_Multa = tmpNumber;
                        decimal.TryParse(row.Cells[10].Text, out tmpNumber);
                        reg.Soma_Correcao = tmpNumber;
                        decimal.TryParse(row.Cells[11].Text, out tmpNumber);
                        reg.Soma_Total = tmpNumber;

                        List<DebitoStructure> ListaTrib = debito_class.Extrato(reg.Codigo_Reduzido, Convert.ToInt16(reg.Ano_Exercicio), Convert.ToInt16(reg.Ano_Exercicio), Convert.ToInt16(reg.Codigo_Lancamento), Convert.ToInt16(reg.Codigo_Lancamento), Convert.ToInt16(reg.Sequencia_Lancamento), Convert.ToInt16(reg.Sequencia_Lancamento),
                            Convert.ToInt16(reg.Numero_Parcela), Convert.ToInt16(reg.Numero_Parcela), reg.Complemento, reg.Complemento, 0, 99, Convert.ToDateTime(reg.Data_Vencimento), 0);
                        String DescTributo="";
                        foreach (DebitoStructure Trib in ListaTrib) {
                            DescTributo = "";
                            foreach (TributoStructure a in Trib.Tributos) {
                                DescTributo += a.Codigo.ToString("000") + "-" + a.Descricao + "/";
                            }
                        }
                        DescTributo = DescTributo.Substring(0, DescTributo.Length - 1);
                        reg.Descricao_Tributo = DescTributo;

                        lstExtrato.Add(reg);
                    }
                }
            }

            decimal nValorGuia = 0;
            decimal.TryParse(TableTotal.Rows[2].Cells[6].Text, out nValorGuia);

            numdocumento regDoc = new numdocumento();
            regDoc.valorguia = nValorGuia;
            regDoc.emissor = "Gti.Web/Dam.Reg";
            regDoc.datadocumento = DateTime.Now;
            regDoc.registrado = true;
            int NumDoc= debito_class.GravaDocumento(regDoc);
           
            foreach (DebitoStructure Lanc in lstExtrato) {
                parceladocumento regParc = new parceladocumento();
                regParc.codreduzido = Lanc.Codigo_Reduzido;
                regParc.anoexercicio = Convert.ToInt16(Lanc.Ano_Exercicio);
                regParc.codlancamento = Convert.ToInt16(Lanc.Codigo_Lancamento);
                regParc.seqlancamento = Convert.ToInt16(Lanc.Sequencia_Lancamento);
                regParc.numparcela = Convert.ToByte(Lanc.Numero_Parcela);
                regParc.codcomplemento = Convert.ToByte(Lanc.Complemento);
                regParc.numdocumento = NumDoc;
                regParc.valorjuros = Convert.ToDecimal(Lanc.Soma_Juros);
                regParc.valormulta = Convert.ToDecimal(Lanc.Soma_Multa);
                regParc.valorcorrecao = Convert.ToDecimal(Lanc.Soma_Correcao);
                regParc.plano = Convert.ToInt16( clsGlobal.nPlano);
                
                debito_class.GravaParcelaDocumentoa(regParc);
            }

            String sDataDAM = lblVenctoDam.Text;
            if (lstExtrato.Count == 0) {
                lblMsg2.Text = "Selecione ao menos uma parcela.";
                return;
            }
            int nSid = debito_class.GravaBoletoDAM(lstExtrato, NumDoc, DateTime.ParseExact(sDataDAM, "dd/MM/yyyy", null));
            if (nSid > 0) {
                
                Session["sid"] = nSid;
//                if (Convert.ToInt32(txtCod.Text) == 38 || Convert.ToInt32(txtCod.Text) == 118777 || Convert.ToInt32(txtCod.Text) == 500000) {
                    Response.Redirect("~/Pages/damwebend2.aspx");
                    ShowResult(false);
                    Response.Write("<script>window.open('damwebend2.aspx','_blank');</script>");
  //              } else
    //                Response.Redirect("~/Pages/damwebend.aspx");
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

        private string Decrypt(string cipherText) {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(cipherText);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
           
        }



    }//end class
}