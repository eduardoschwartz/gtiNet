using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace UIWeb.Models {
    public class clsDebito {

        public List<DebitoStructure> Extrato(int Codigo, short Ano1, short Ano2, short Lanc1, short Lanc2, short Seq1, short Seq2, short Parc1, short Parc2, short Compl1, short Compl2, short nStatus1, short nStatus2, DateTime dDataNow, byte bAjuizado) {
            // bool bRefis = Convert.ToInt32(clsGlobal.ParametroGti("ANISTIA")) == 1 ? true : false;
            bool bRefis = true;
            bool bRefisDI = true;
            Decimal nPerc = 0;
            decimal nValorPrincipal, nValorMulta, nValorJuros, nValorCorrecao, nValorTotal; ;

            DataSet Ds = new DataSet();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["GTIconnection"].ConnectionString))
            using (var cmd = new SqlCommand("spExtratoNew", con)) {
                cmd.Parameters.Add(new SqlParameter("@CodReduz1", SqlDbType.Int)).Value = Codigo;
                cmd.Parameters.Add(new SqlParameter("@CodReduz2", SqlDbType.Int)).Value = Codigo;
                cmd.Parameters.Add(new SqlParameter("@AnoExercicio1", SqlDbType.Int)).Value = Ano1;
                cmd.Parameters.Add(new SqlParameter("@AnoExercicio2", SqlDbType.Int)).Value = Ano2;
                cmd.Parameters.Add(new SqlParameter("@CodLancamento1", SqlDbType.Int)).Value = Lanc1;
                cmd.Parameters.Add(new SqlParameter("@CodLancamento2", SqlDbType.Int)).Value = Lanc2;
                cmd.Parameters.Add(new SqlParameter("@SeqLancamento1", SqlDbType.Int)).Value = Seq1;
                cmd.Parameters.Add(new SqlParameter("@SeqLancamento2", SqlDbType.Int)).Value = Seq2;
                cmd.Parameters.Add(new SqlParameter("@NumParcela1", SqlDbType.Int)).Value = Parc1;
                cmd.Parameters.Add(new SqlParameter("@NumParcela2", SqlDbType.Int)).Value = Parc2;
                cmd.Parameters.Add(new SqlParameter("@CodComplemento1", SqlDbType.Int)).Value = Compl1;
                cmd.Parameters.Add(new SqlParameter("@CodComplemento2", SqlDbType.Int)).Value = Compl2;
                cmd.Parameters.Add(new SqlParameter("@StatusLanc1", SqlDbType.Int)).Value = nStatus1;
                cmd.Parameters.Add(new SqlParameter("@StatusLanc2", SqlDbType.Int)).Value = nStatus2;
                cmd.Parameters.Add(new SqlParameter("@DataNow", SqlDbType.DateTime)).Value = dDataNow;

                using (var da = new SqlDataAdapter(cmd)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(Ds);
                }
                con.Open();
                //SqlCommand command = new SqlCommand("Select count(*) from protesto where codigo="+Codigo, con);
                SqlCommand command = new SqlCommand("Select count(*) from debitoparcela where codreduzido=" + Codigo + " and statuslanc=38", con);
                int result =(Int32) command.ExecuteScalar();
                clsGlobal.bCodigoProtesto = result <= 0 ? false : true;
                command.Dispose();
            }
            List<DebitoStructure> Lista = new List<DebitoStructure>();
            List<DebitoStructure> Lista2 = new List<DebitoStructure>();
            List<TributoStructure> ListaTributo=new List<TributoStructure>();
            int nPos = 0;
            foreach (DataRow reg in Ds.Tables[0].Rows) {

                if (Convert.ToInt16(reg["statuslanc"]) != 3 && Convert.ToInt16(reg["statuslanc"]) != 19 && Convert.ToInt16(reg["statuslanc"]) != 38 && Convert.ToInt16(reg["statuslanc"]) != 39)
                    goto Proximo;
                bool bFind = false;
                
                foreach (var item2 in Lista) {
                    if (Convert.ToInt32(reg["anoexercicio"]) == item2.Ano_Exercicio && Convert.ToInt32(reg["codlancamento"]) == item2.Codigo_Lancamento && Convert.ToInt32(reg["seqlancamento"]) == item2.Sequencia_Lancamento &&
                        Convert.ToInt16(reg["numparcela"]) == item2.Numero_Parcela && Convert.ToInt16(reg["codcomplemento"]) == item2.Complemento) {
                        bFind = true;
                        break;
                    }
                }
                
                if (!bFind) {
                    DebitoStructure item = new DebitoStructure();
                    item.Usuario = reg["usuario"].ToString();
                    item.Numero_Documento = reg["numdocumento"].ToString() == "" ? 0 : Convert.ToDouble(reg["numdocumento"]);
                    item.Codigo_Reduzido = Convert.ToInt32(reg["codreduzido"]);
                    item.Ano_Exercicio = Convert.ToInt32(reg["anoexercicio"]);
                    item.Codigo_Lancamento = Convert.ToInt32(reg["codlancamento"]);
                    item.Sequencia_Lancamento = Convert.ToInt32(reg["seqlancamento"]);
                    item.Numero_Parcela = Convert.ToInt16(reg["numparcela"]);
                    item.Complemento = Convert.ToInt16(reg["codcomplemento"]);
                    item.Codigo_Situacao = Convert.ToInt16(reg["statuslanc"]);
                    item.Usuario = "GTI/Web";
                    item.Descricao_Lancamento = reg["desclancamento"].ToString();
                    item.Data_Vencimento = Convert.ToDateTime(reg["datavencimento"]);
                    item.Soma_Principal =Convert.ToDecimal(reg["valortributo"]);
                    item.Soma_Juros = Convert.ToDecimal(reg["valorjuros"]);
                    item.Soma_Multa = Convert.ToDecimal(reg["valormulta"]);
                    item.Soma_Correcao = Convert.ToDecimal(reg["valorcorrecao"]);
                    item.Soma_Total = Convert.ToDecimal(reg["valortotal"]);
                    item.Data_Ajuizamento = (reg["dataajuiza"] is DBNull) ? DateTime.MinValue : Convert.ToDateTime(reg["dataajuiza"]);

               //     if (item.Sequencia_Lancamento == 5 && item.Numero_Parcela == 18)
                   //     bRefis = true;
                    
                    if (bRefis) {
                        if (Convert.ToDateTime(reg["datavencimento"]) <= Convert.ToDateTime("31/12/2016")) {
                            Int16 CodLanc = Convert.ToInt16(reg["codlancamento"]);
                            if (CodLanc != 48 || CodLanc != 69 || CodLanc != 78) {
                               
                                if (dDataNow <= Convert.ToDateTime("29/09/2017")) {
                                    nPerc = 1M;
                                    clsGlobal.nPlano = 16;
                                } else if (dDataNow > Convert.ToDateTime("29/09/2017") && dDataNow <= Convert.ToDateTime("31/10/2017")) {
                                    nPerc = 0.8M;
                                    clsGlobal.nPlano = 17;
                                } else if (dDataNow > Convert.ToDateTime("31/10/2017") && dDataNow <= Convert.ToDateTime("30/11/2017")) {
                                    nPerc = 0.6M;
                                    clsGlobal.nPlano = 18;
                                } else if (dDataNow > Convert.ToDateTime("30/11/2017") && dDataNow <= Convert.ToDateTime("22/12/2017")) {
                                    nPerc = 0.5M;
                                    clsGlobal.nPlano = 19;
                                }
                                item.Soma_Juros = Convert.ToDecimal(reg["valorjuros"]) - (Convert.ToDecimal(reg["valorjuros"]) * nPerc);
                                item.Soma_Multa = Convert.ToDecimal(reg["valormulta"]) - (Convert.ToDecimal(reg["valormulta"]) * nPerc);
                                item.Soma_Total = item.Soma_Principal + item.Soma_Juros + item.Soma_Multa + item.Soma_Correcao;
                            }
                        }
                    }

                    if (bRefisDI) {
                        if (Convert.ToDateTime(reg["datavencimento"]) < Convert.ToDateTime("27/07/2017")) {
                            Int16 CodLanc = Convert.ToInt16(reg["codlancamento"]);
                            if (CodLanc == 81) {
                                item.Soma_Juros = 0;
                                item.Soma_Multa = 0;
                                item.Soma_Total = item.Soma_Principal + item.Soma_Juros + item.Soma_Multa + item.Soma_Correcao;
                                clsGlobal.nPlano = 23;
                            }
                        }
                    }

                    ListaTributo = new List<TributoStructure>();
                    TributoStructure RegTrib = new TributoStructure();
                    RegTrib.Codigo = Convert.ToInt32(reg["codtributo"]);
                    RegTrib.Descricao = reg["abrevtributo"].ToString();
                    RegTrib.Valor_Principal = Convert.ToDecimal(reg["valortributo"]);
                    RegTrib.Valor_Juros = item.Soma_Juros;
                    RegTrib.Valor_Multa = item.Soma_Multa;
                    RegTrib.Valor_Correcao = Convert.ToDecimal(reg["valorcorrecao"]);
                    RegTrib.Valor_Total = item.Soma_Total;
                    ListaTributo.Add(RegTrib);
                    item.Tributos = ListaTributo;
                    nValorPrincipal = 0;nValorMulta = 0;nValorJuros = 0;nValorCorrecao = 0;nValorTotal = 0;
                    foreach (TributoStructure a in ListaTributo) {
                        nValorPrincipal += a.Valor_Principal;
                        nValorJuros += a.Valor_Juros;
                        nValorMulta += a.Valor_Multa;
                        nValorCorrecao += a.Valor_Correcao;
                        nValorTotal += a.Valor_Total;
                    }
                    item.Soma_Principal = nValorPrincipal;
                    item.Soma_Juros = nValorJuros;
                    item.Soma_Multa = nValorMulta;
                    item.Soma_Correcao = nValorCorrecao;
                    item.Soma_Total = nValorTotal;

                    Lista.Add(item);
                    nPos += 1;
                } else {
                    TributoStructure RegTrib = new TributoStructure();
                    RegTrib.Codigo = Convert.ToInt32(reg["codtributo"]);
                    RegTrib.Descricao = reg["abrevtributo"].ToString();
                    RegTrib.Valor_Principal = Convert.ToDecimal(reg["valortributo"]);
//                    RegTrib.Valor_Juros = Convert.ToDecimal(reg["valorjuros"]);
  //                  RegTrib.Valor_Multa = Convert.ToDecimal(reg["valormulta"]);
                    RegTrib.Valor_Correcao = Convert.ToDecimal(reg["valorcorrecao"]);
                    //                RegTrib.Valor_Total = Convert.ToDecimal(reg["valortotal"]);


                    if (Convert.ToDateTime(reg["datavencimento"]) <= Convert.ToDateTime("31/12/2016")) {
                        if (Convert.ToInt32(reg["codlancamento"]) != 48 || Convert.ToInt32(reg["codlancamento"]) != 69 || Convert.ToInt32(reg["codlancamento"]) != 78) {
                            RegTrib.Valor_Juros = RegTrib.Valor_Juros + (Convert.ToDecimal(reg["valorjuros"]) - ((Convert.ToDecimal(reg["valorjuros"])) * nPerc));
                            RegTrib.Valor_Multa = RegTrib.Valor_Multa + (Convert.ToDecimal(reg["valormulta"]) - ((Convert.ToDecimal(reg["valormulta"])) * nPerc));
                            RegTrib.Valor_Total = RegTrib.Valor_Principal + RegTrib.Valor_Juros + RegTrib.Valor_Multa + RegTrib.Valor_Correcao;
                        } else {
                            RegTrib.Valor_Juros = Convert.ToDecimal(reg["valorjuros"]);
                            RegTrib.Valor_Multa = Convert.ToDecimal(reg["valormulta"]);
                            RegTrib.Valor_Total = Convert.ToDecimal(reg["valortotal"]);
                        }
                    }else {
                        RegTrib.Valor_Juros = Convert.ToDecimal(reg["valorjuros"]);
                        RegTrib.Valor_Multa = Convert.ToDecimal(reg["valormulta"]);
                        RegTrib.Valor_Total = Convert.ToDecimal(reg["valortotal"]);
                    }

                    ListaTributo.Add(RegTrib);
                    nValorPrincipal = 0; nValorMulta = 0; nValorJuros = 0; nValorCorrecao = 0; nValorTotal = 0;
                    foreach (TributoStructure a in ListaTributo) {

                        nValorPrincipal += a.Valor_Principal;
                        nValorJuros += a.Valor_Juros;
                        nValorMulta += a.Valor_Multa;
                        nValorCorrecao += a.Valor_Correcao;
                        nValorTotal += a.Valor_Total;
                    }
                    Lista[nPos - 1].Soma_Principal = nValorPrincipal;
                    Lista[nPos - 1].Soma_Juros = nValorJuros;
                    Lista[nPos - 1].Soma_Multa = nValorMulta;
                    Lista[nPos - 1].Soma_Correcao = nValorCorrecao;
                    Lista[nPos - 1].Soma_Total = nValorTotal;
                    Lista[nPos-1].Tributos = ListaTributo;
                }
            Proximo:;
            }

            return Lista;

        }

        public List<boletoguia> ListaBoletoGuia(int nSid) {
            List<boletoguia> reg;
            using (TributacaoEntities db = new TributacaoEntities()) {
                 reg = (from b in db.boletoguia where b.sid == nSid select b).ToList();
                return reg;
            }
        }

        public List<boleto> ListaBoletoDAM(int nSid) {
            List<boleto> reg;
            using (TributacaoEntities db = new TributacaoEntities()) {
                reg = (from b in db.boleto where b.sid == nSid select b).ToList();
                return reg;
            }
        }

        public void InsertBoletoGuia(boletoguia Reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.boletoguia.Add(Reg);
                    db.SaveChanges();
                    return;
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

        public void GravaCarneWeb(int Codigo,int Ano)
        {
            using (TributacaoEntities db = new TributacaoEntities())
            {
                try
                {
                    laseriptu b = db.laseriptu.First(i => i.codreduzido == Codigo && i.ano == Ano);
                    b.carne_web = 1;
                    db.SaveChanges();
                    return;
                }
                catch (Exception ex)
                {
                    throw (ex.InnerException);
                }
            }
        }

        public void InsertBoletoDam(boleto Reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.boleto.Add(Reg);
                    db.SaveChanges();
                    return;
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

        public void Insert_Numero_Segunda_Via(segunda_via_web Reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.segunda_via_web.Add(Reg);
                    db.SaveChanges();
                    return;
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

        public List<DebitoStructure> ListaParcelasDocumento(int nNumDoc) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                var reg = (from p in db.parceladocumento where p.numdocumento==nNumDoc
                           select new { p.codreduzido, p.anoexercicio, p.codlancamento, p.seqlancamento, p.numparcela, p.codcomplemento});
                List<DebitoStructure> Lista = new List<DebitoStructure>();
                foreach (var query in reg) {
                    DebitoStructure Linha = new DebitoStructure();
                    Linha.Codigo_Reduzido = query.codreduzido;
                    Linha.Ano_Exercicio = query.anoexercicio;
                    Linha.Codigo_Lancamento = query.codlancamento;
                    Linha.Sequencia_Lancamento = query.seqlancamento;
                    Linha.Numero_Parcela = query.numparcela;
                    Linha.Complemento = query.codcomplemento;
                    Lista.Add(Linha);
                }
                return Lista;
            }
        }

        public List<DebitoStructure> ListaParcelasIPTU(int nCodigo,int nAno) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                var reg = (from dp in db.debitoparcela 
                           join dt in db.debitotributo on new {p1=dp.codreduzido,p2=dp.anoexercicio,p3=dp.codlancamento,p4=dp.seqlancamento,p5=dp.numparcela,p6=dp.codcomplemento} 
                                                   equals new {p1=dt.codreduzido,p2=dt.anoexercicio,p3=dt.codlancamento,p4=dt.seqlancamento,p5=dt.numparcela,p6=dt.codcomplemento } into dpdt from dt in dpdt.DefaultIfEmpty()
                           join pd in db.parceladocumento on new { p1 = dp.codreduzido, p2 = dp.anoexercicio, p3 = dp.codlancamento, p4 = dp.seqlancamento, p5 = dp.numparcela, p6 = dp.codcomplemento }
                                                      equals new { p1 = pd.codreduzido, p2 = pd.anoexercicio, p3 = pd.codlancamento, p4 = pd.seqlancamento, p5 = pd.numparcela, p6 = pd.codcomplemento } into dppd from pd in dppd.DefaultIfEmpty()
                           where dp.codreduzido == nCodigo && dp.anoexercicio==nAno && dp.codlancamento==1 && dp.seqlancamento==0  && dp.statuslanc==3
                           orderby new {dp.numparcela,dp.codcomplemento}
                           select new {dp.codreduzido,dp.anoexercicio,dp.codlancamento,dp.seqlancamento ,dp.numparcela,dp.codcomplemento,dp.datavencimento,dt.valortributo,pd.numdocumento});


                List<DebitoStructure> Lista = new List<DebitoStructure>();
                foreach (var query in reg) {
                    foreach(DebitoStructure item in Lista) {
                        if (item.Numero_Parcela == query.numparcela && item.Complemento == query.codcomplemento  )
                            goto Proximo;
                    }
                    DebitoStructure Linha = new DebitoStructure();
                    Linha.Codigo_Reduzido = query.codreduzido;
                    Linha.Ano_Exercicio = query.anoexercicio;
                    Linha.Codigo_Lancamento = query.codlancamento;
                    Linha.Sequencia_Lancamento = query.seqlancamento;
                    Linha.Numero_Parcela = query.numparcela;
                    Linha.Complemento = query.codcomplemento;
                    Linha.Soma_Principal = Convert.ToDecimal( query.valortributo);
                    Linha.Data_Vencimento = query.datavencimento;
                    Linha.Numero_Documento = query.numdocumento;
                    Lista.Add(Linha);
                    Proximo:;
                }
                return Lista;
            }
        }

        public List<DebitoStructure> ListaParcelasCIP(int nCodigo, int nAno) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                var reg = (from dp in db.debitoparcela
                           join dt in db.debitotributo on new { p1 = dp.codreduzido, p2 = dp.anoexercicio, p3 = dp.codlancamento, p4 = dp.seqlancamento, p5 = dp.numparcela, p6 = dp.codcomplemento }
                                                   equals new { p1 = dt.codreduzido, p2 = dt.anoexercicio, p3 = dt.codlancamento, p4 = dt.seqlancamento, p5 = dt.numparcela, p6 = dt.codcomplemento } into dpdt from dt in dpdt.DefaultIfEmpty()
                           join pd in db.parceladocumento on new { p1 = dp.codreduzido, p2 = dp.anoexercicio, p3 = dp.codlancamento, p4 = dp.seqlancamento, p5 = dp.numparcela, p6 = dp.codcomplemento }
                                                      equals new { p1 = pd.codreduzido, p2 = pd.anoexercicio, p3 = pd.codlancamento, p4 = pd.seqlancamento, p5 = pd.numparcela, p6 = pd.codcomplemento } into dppd from pd in dppd.DefaultIfEmpty()
                           where dp.codreduzido == nCodigo && dp.anoexercicio == nAno && dp.codlancamento == 79 && dp.seqlancamento == 0 && dp.statuslanc == 3
                           orderby new { dp.numparcela, dp.codcomplemento }
                           select new { dp.codreduzido, dp.anoexercicio, dp.codlancamento, dp.seqlancamento, dp.numparcela, dp.codcomplemento, dp.datavencimento, dt.valortributo, pd.numdocumento });


                List<DebitoStructure> Lista = new List<DebitoStructure>();
                foreach (var query in reg) {
                    foreach (DebitoStructure item in Lista) {
                        if (item.Numero_Parcela == query.numparcela && item.Complemento == query.codcomplemento)
                            goto Proximo;
                    }
                    DebitoStructure Linha = new DebitoStructure();
                    Linha.Codigo_Reduzido = query.codreduzido;
                    Linha.Ano_Exercicio = query.anoexercicio;
                    Linha.Codigo_Lancamento = query.codlancamento;
                    Linha.Sequencia_Lancamento = query.seqlancamento;
                    Linha.Numero_Parcela = query.numparcela;
                    Linha.Complemento = query.codcomplemento;
                    Linha.Soma_Principal = Convert.ToDecimal(query.valortributo);
                    Linha.Data_Vencimento = query.datavencimento;
                    Linha.Numero_Documento = query.numdocumento;
                    Lista.Add(Linha);
                    Proximo:;
                }
                return Lista;
            }
        }

        public void DeleteCarne(int nSid) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.boletoguia.RemoveRange(db.boletoguia.Where( i => i.sid == nSid));
                    db.SaveChanges();
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

        public void DeleteDam(int nSid) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.boleto.RemoveRange(db.boleto.Where(i => i.sid == nSid));
                    db.SaveChanges();
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

        public laseriptu CarregaIPTU(int nCodigo,int nAno) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                laseriptu reg = (from l in db.laseriptu where l.ano==nAno && l.codreduzido==nCodigo select l).FirstOrDefault();
                return reg;
            }
        }

        public int GravaDocumento(numdocumento Reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    Int32 maxCod = db.numdocumento.Max(u => u.numdocumento1);
                    Reg.numdocumento1 = Convert.ToInt32(maxCod + 1);
                    db.numdocumento.Add(Reg);
                    db.SaveChanges();
                    return maxCod + 1;
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
               
            }

        }

        public void GravaParcelaDocumentoa(parceladocumento Reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.parceladocumento.Add(Reg);
                    db.SaveChanges();
                    return;
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

        public Int32 GravaBoletoDAM(List<DebitoStructure> lstDebito, Int32 nNumDoc, DateTime DataBoleto) {
            int nSid = gtiCore.GetRandomNumber(), nCodigo = lstDebito[0].Codigo_Reduzido, nNumero = 0, Pos = 0;
            string sInscricao = "", sNome = "",sQuadra = "", sLote = "", sCPF = "", sEndereco = "", sComplemento = "", sBairro = "", sCidade = "", sUF = "";
            decimal SomaPrincipal = 0, SomaTotal = 0;

            string sNumDoc = nNumDoc.ToString() + "-" + gtiCore.RetornaDVDocumento(nNumDoc).ToString();
            string sNumDoc2 = nNumDoc.ToString() + gtiCore.RetornaDVDocumento(nNumDoc).ToString();
            string sNumDoc3 = nNumDoc.ToString() + gtiCore.Modulo11(nNumDoc.ToString("0000000000000")).ToString();

            gtiCore.TipoContribuinte tpContribuinte = nCodigo < 100000 ? 
                gtiCore.TipoContribuinte.Imovel : nCodigo >= 100000 && nCodigo < 400000 ? gtiCore.TipoContribuinte.Empresa : gtiCore.TipoContribuinte.Cidadao;

            clsGlobal global_class = new clsGlobal();
            Dados_Basicos regDados = global_class.Retorna_Dados_Basicos(nCodigo);
            sNome = regDados.nome;
            sCPF = regDados.cpf_cnpj;
            sInscricao = regDados.Inscricao;
            sQuadra = "";
            sLote = "";
            sEndereco = regDados.endereco == null ? "" : regDados.endereco;
            nNumero = regDados.numero;
            sComplemento = regDados.complemento == null ? "" : regDados.complemento;
            sBairro = regDados.nome_bairro == null ? "" : regDados.nome_bairro;
            sCidade = regDados.nome_cidade == null ? "" : regDados.nome_cidade;
            sUF = regDados.nome_uf;

            global_class.DeleteSid(nSid);

            foreach (DebitoStructure reg in lstDebito) {
                SomaPrincipal +=  Convert.ToDecimal(reg.Soma_Principal);
                SomaTotal += Convert.ToDecimal(reg.Soma_Total);
            }

            StringBuilder sFullLanc = new StringBuilder();

            foreach (DebitoStructure Lanc in lstDebito) {
                if (sFullLanc.ToString().IndexOf(Lanc.Descricao_Lancamento) == -1) {
                    String DescLanc = Lanc.Descricao_Lancamento;
                    sFullLanc.Append(DescLanc + "/");
                }
            }
            sFullLanc.Remove(sFullLanc.Length - 1, 1);

            //codigo de barras
            /*            String sNossoNumero = "2678478";
                        String sDigitavel = "001900000";
                        String sDV = gtiCore.Calculo_DV10(sDigitavel).ToString().Trim();
                        sDigitavel += sDV + "0" + sNossoNumero + "01";
                        sDV = gtiCore.Calculo_DV10(gtiCore.strRight(sDigitavel, 10)).ToString().Trim();
                        sDigitavel += sDV + gtiCore.strRight(sNumDoc3, 8) + "18";
                        sDV = gtiCore.Calculo_DV10(gtiCore.strRight(sDigitavel, 10)).ToString().Trim();
                        sDigitavel += sDV;

                        DateTime dDataBase = DateTime.ParseExact("07/10/1997", "dd/MM/yyyy", null);
                        Int32 FatorVencto = ((TimeSpan)(DataBoleto - dDataBase)).Days;
                        String sQuintoGrupo = FatorVencto.ToString("0000");
                        sQuintoGrupo += Convert.ToInt32(gtiCore.RetornaNumero(SomaTotal.ToString("#0.00"))).ToString("0000000000");

                        String sBarra = "0019" + FatorVencto.ToString("0000") + Convert.ToInt32(gtiCore.RetornaNumero(SomaTotal.ToString("#0.00"))).ToString("0000000000") + "00000026784780";
                        sBarra += sNumDoc3 + "18";
                        sDV = gtiCore.Calculo_DV11(sBarra).ToString().Trim();
                        sBarra = sBarra.Substring(0, 4) + sDV + sBarra.Substring(4, sBarra.Length - 4);

                        sDigitavel += sDV + sQuintoGrupo;
                        String sDigitavel2 = sDigitavel.Substring(0, 5) + "." + sDigitavel.Substring(5, 5) + " " + sDigitavel.Substring(10, 5) + "." + sDigitavel.Substring(15, 6) + " ";
                        sDigitavel2 += sDigitavel.Substring(21, 5) + "." + sDigitavel.Substring(26, 6) + " " + sDigitavel.Substring(32, 1) + " " + sDigitavel.Substring(sDigitavel.Length - 14);
                        sBarra = gtiCore.Gera2of5Str(sBarra);*/

            decimal nValorguia = Math.Truncate(Convert.ToDecimal(SomaTotal * 100));
            string NumBarra = gtiCore.Gera2of5Cod((nValorguia).ToString(), Convert.ToDateTime(DataBoleto), Convert.ToInt32(nNumDoc), Convert.ToInt32(nCodigo));
            string numbarra2a = NumBarra.Substring(0, 13);
            string numbarra2b = NumBarra.Substring(13, 13);
            string numbarra2c = NumBarra.Substring(26, 13);
            string numbarra2d = NumBarra.Substring(39, 13);
            string strBarra = gtiCore.Gera2of5Str(numbarra2a.Substring(0, 11) + numbarra2b.Substring(0, 11) + numbarra2c.Substring(0, 11) + numbarra2d.Substring(0, 11));
            string sBarra = gtiCore.Mask(strBarra);



            foreach (DebitoStructure reg in lstDebito) {
                try {
                    boleto regBoleto = new boleto();
                    regBoleto.usuario = "GTI.Web.Dam";
                    regBoleto.computer = "Internet";
                    regBoleto.sid = nSid;
                    regBoleto.seq = Convert.ToInt16(Pos);
                    regBoleto.inscricao = sInscricao;
                    regBoleto.codreduzido = reg.Codigo_Reduzido.ToString();
                    regBoleto.nome = gtiCore.Truncate(sNome.Trim(), 37, "...");
                    regBoleto.cpf = sCPF;
                    regBoleto.endereco = gtiCore.Truncate(sEndereco, 37, "...");
                    regBoleto.numimovel = Convert.ToInt16(nNumero);
                    regBoleto.complemento = gtiCore.Truncate(sComplemento, 27, "...");
                    regBoleto.bairro = gtiCore.Truncate(sBairro, 27, "...");
                    regBoleto.cidade = sCidade;
                    regBoleto.uf = sUF;
                    regBoleto.quadra=gtiCore.Truncate(sQuadra, 15, "");
                    regBoleto.lote=gtiCore.Truncate(sLote, 10, "");
                    regBoleto.fulllanc = gtiCore.Truncate(sFullLanc.ToString(), 1997, "...");
                    regBoleto.fulltrib = gtiCore.Truncate(reg.Descricao_Tributo.Trim(), 1997, "...");
                    regBoleto.numdoc = sNumDoc;
                    regBoleto.datadam =Convert.ToDateTime(DataBoleto);
                    regBoleto.nomefunc = "GTI.Web";
                    regBoleto.anoexercicio = reg.Ano_Exercicio;
                    regBoleto.codlancamento = Convert.ToInt16(reg.Codigo_Lancamento);
                    regBoleto.seqlancamento = Convert.ToInt16(reg.Sequencia_Lancamento);
                    regBoleto.numparcela = Convert.ToInt16(reg.Numero_Parcela);
                    regBoleto.codcomplemento = Convert.ToInt16(reg.Complemento);
                    regBoleto.datavencto = Convert.ToDateTime(reg.Data_Vencimento);
                    regBoleto.aj = reg.Data_Ajuizamento==null||  reg.Data_Ajuizamento == DateTime.MinValue ? "N" : "S";
                    regBoleto.da = reg.Ano_Exercicio == DateTime.Now.Year ? "N" : "S";
                    regBoleto.principal = Convert.ToDecimal(reg.Soma_Principal);
                    regBoleto.juros = Convert.ToDecimal(reg.Soma_Juros);
                    regBoleto.multa = Convert.ToDecimal(reg.Soma_Multa);
                    regBoleto.correcao = Convert.ToDecimal(reg.Soma_Correcao);
                    regBoleto.total = Convert.ToDecimal(reg.Soma_Total);
                    regBoleto.numdoc2 = sNumDoc2;
                    regBoleto.digitavel = "";
                    regBoleto.codbarra = sBarra;
                    regBoleto.valordam = SomaTotal;
                    regBoleto.valorprincdam = SomaPrincipal;
                    regBoleto.numbarra2a = numbarra2a;
                    regBoleto.numbarra2b = numbarra2b;
                    regBoleto.numbarra2c = numbarra2c;
                    regBoleto.numbarra2d = numbarra2d;
                    InsertBoletoDam(regBoleto);
                    Pos++;
                } catch (SqlException ex) {
                    throw new Exception(ex.Message);
                } catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
            }
            return nSid;
        }

        public Int32 GravaDetalheDAM(List<DebitoStructure> lstDebito, string nNumDoc, DateTime DataBoleto,decimal nValorGuia) {
            int nSid = gtiCore.GetRandomNumber(), nCodigo = lstDebito[0].Codigo_Reduzido, nNumero = 0, Pos = 0;
            string sInscricao = "", sNome = "", sQuadra = "", sLote = "", sCPF = "", sEndereco = "", sComplemento = "", sBairro = "", sCidade = "", sUF = "";
            decimal SomaPrincipal = 0, SomaTotal = 0;

            gtiCore.TipoContribuinte tpContribuinte = nCodigo < 100000 ?
                gtiCore.TipoContribuinte.Imovel : nCodigo >= 100000 && nCodigo < 400000 ? gtiCore.TipoContribuinte.Empresa : gtiCore.TipoContribuinte.Cidadao;

            clsGlobal global_class = new clsGlobal();
            Dados_Basicos regDados = global_class.Retorna_Dados_Basicos(nCodigo);
            sNome = regDados.nome;
            sCPF = regDados.cpf_cnpj;
            sInscricao = regDados.Inscricao;
            sQuadra = "";
            sLote = "";
            sEndereco = regDados.endereco == null ? "" : regDados.endereco;
            nNumero = regDados.numero;
            sComplemento = regDados.complemento == null ? "" : regDados.complemento;
            sBairro = regDados.nome_bairro == null ? "" : regDados.nome_bairro;
            sCidade = regDados.nome_cidade == null ? "" : regDados.nome_cidade;
            sUF = regDados.nome_uf;

            global_class.DeleteSid(nSid);

            foreach (DebitoStructure reg in lstDebito) {
                SomaPrincipal += Convert.ToDecimal(reg.Soma_Principal);
                SomaTotal += Convert.ToDecimal(reg.Soma_Total);
            }

            StringBuilder sFullLanc = new StringBuilder();

            foreach (DebitoStructure Lanc in lstDebito) {
                if (sFullLanc.ToString().IndexOf(Lanc.Descricao_Lancamento) == -1) {
                    String DescLanc = Lanc.Descricao_Lancamento;
                    sFullLanc.Append(DescLanc + "/");
                }
            }
            sFullLanc.Remove(sFullLanc.Length - 1, 1);


            decimal nValorTotal = nValorGuia;


            foreach (DebitoStructure reg in lstDebito) {
                try {
                    boleto regBoleto = new boleto();
                    regBoleto.usuario = "GTI.Web.Dam";
                    regBoleto.computer = "Internet";
                    regBoleto.sid = nSid;
                    regBoleto.seq = Convert.ToInt16(Pos);
                    regBoleto.inscricao = sInscricao;
                    regBoleto.codreduzido = reg.Codigo_Reduzido.ToString();
                    regBoleto.nome = gtiCore.Truncate(sNome.Trim(), 37, "...");
                    regBoleto.cpf = sCPF;
                    regBoleto.endereco = gtiCore.Truncate(sEndereco, 37, "...");
                    regBoleto.numimovel = Convert.ToInt16(nNumero);
                    regBoleto.complemento = gtiCore.Truncate(sComplemento, 27, "...");
                    regBoleto.bairro = gtiCore.Truncate(sBairro, 27, "...");
                    regBoleto.cidade = sCidade;
                    regBoleto.uf = sUF;
                    regBoleto.quadra = gtiCore.Truncate(sQuadra, 15, "");
                    regBoleto.lote = gtiCore.Truncate(sLote, 10, "");
                    regBoleto.fulllanc = gtiCore.Truncate(sFullLanc.ToString(), 1997, "...");
                    regBoleto.fulltrib = gtiCore.Truncate(reg.Descricao_Tributo.Trim(), 1997, "...");
                    regBoleto.numdoc = "0";
                    regBoleto.datadam = Convert.ToDateTime(DataBoleto);
                    regBoleto.nomefunc = "GTI.Web";
                    regBoleto.anoexercicio = reg.Ano_Exercicio;
                    regBoleto.codlancamento = Convert.ToInt16(reg.Codigo_Lancamento);
                    regBoleto.seqlancamento = Convert.ToInt16(reg.Sequencia_Lancamento);
                    regBoleto.numparcela = Convert.ToInt16(reg.Numero_Parcela);
                    regBoleto.codcomplemento = Convert.ToInt16(reg.Complemento);
                    regBoleto.datavencto = Convert.ToDateTime(reg.Data_Vencimento);
                    regBoleto.aj = reg.Data_Ajuizamento == null || reg.Data_Ajuizamento == DateTime.MinValue ? "N" : "S";
                    regBoleto.da = reg.Ano_Exercicio == DateTime.Now.Year ? "N" : "S";
                    regBoleto.principal = Convert.ToDecimal(reg.Soma_Principal);
                    regBoleto.juros = Convert.ToDecimal(reg.Soma_Juros);
                    regBoleto.multa = Convert.ToDecimal(reg.Soma_Multa);
                    regBoleto.correcao = Convert.ToDecimal(reg.Soma_Correcao);
                    regBoleto.total = Convert.ToDecimal(reg.Soma_Total);
                    regBoleto.numdoc2 = nNumDoc;
                    regBoleto.digitavel = "";
                    regBoleto.codbarra = "";
                    regBoleto.valordam = nValorGuia;
                    regBoleto.valorprincdam = SomaPrincipal;
                    regBoleto.numbarra2a = "";
                    regBoleto.numbarra2b = "";
                    regBoleto.numbarra2c = "";
                    regBoleto.numbarra2d ="";
                    InsertBoletoDam(regBoleto);
                    Pos++;
                } catch (SqlException ex) {
                    throw new Exception(ex.Message);
                } catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
            }
            return nSid;
        }

        public bool ExisteDocumentoCIP(int nNumDocumento) {
            bool bRet = false;
            using (TributacaoEntities db = new TributacaoEntities()) {
                var existingReg = db.parceladocumento.Where(a=>a.anoexercicio==2017 && a.codlancamento==79).Count(a => a.numdocumento == nNumDocumento);
                if (existingReg != 0) {
                    bRet = true;
                }
            }
            return bRet;
        }

        public bool ExisteDocumento(int nNumDocumento) {
            bool bRet = false;
            using (TributacaoEntities db = new TributacaoEntities()) {
                var existingReg = db.numdocumento.Where(a => a.numdocumento1>1).Count(a => a.numdocumento1 == nNumDocumento);
                if (existingReg != 0) {
                    bRet = true;
                }
            }
            return bRet;
        }

        public int RetornaDocumentoCodigo(int nNumDocumento) {
            int nRet = 0;
            using (TributacaoEntities db = new TributacaoEntities()) {
                nRet = (from m in db.parceladocumento where m.numdocumento == nNumDocumento select m.codreduzido).FirstOrDefault();
            }
            return nRet;
        }

        public DateTime RetornaDocumentoData(int nNumDocumento) {
            DateTime dRet = Convert.ToDateTime("01/01/1900");
            using (TributacaoEntities db = new TributacaoEntities()) {
                dRet = (from m in db.numdocumento where m.numdocumento1 == nNumDocumento select (DateTime)m.datadocumento).FirstOrDefault();
            }
            return dRet;
        }

        public numdocumento RetornaDadosDocumento(int nNumDocumento) {
            numdocumento reg;
            using (TributacaoEntities db = new TributacaoEntities()) {
                reg = (from m in db.numdocumento where m.numdocumento1 == nNumDocumento select m).FirstOrDefault();
            }
            return reg;
        }

        public int CodigoCIP(int nNumDocumento) {
            int Sql = 0;
            using (TributacaoEntities db = new TributacaoEntities()) {
                Sql = (from b in db.parceladocumento where b.anoexercicio == 2017 && b.codlancamento==79 && b.numdocumento==nNumDocumento select b.codreduzido).FirstOrDefault();
            }
            return Sql;
        }


        public bool ExisteComercioEletronico(int nNumDocumento) {
            bool bRet = false;
            using (TributacaoEntities db = new TributacaoEntities()) {
                var existingReg = db.comercio_eletronico.Where(a => a.numdoc>1).Count(a => a.numdoc == nNumDocumento);
                if (existingReg != 0) {
                    bRet = true;
                }
            }
            return bRet;
        }



        public void InsertBoletoComercioEletronico(comercio_eletronico Reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.comercio_eletronico.Add(Reg);
                    db.SaveChanges();
                    return;
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

    }//end class

    public class DebitoStructure {
        public string Usuario { get; set; }
        public int Codigo_Reduzido { get; set; }
        public int Ano_Exercicio { get; set; }
        public int Codigo_Lancamento { get; set; }
        public string Descricao_Lancamento { get; set; }
        public int Sequencia_Lancamento { get; set; }
        public short? Numero_Parcela { get; set; }
        public short Complemento { get; set; }
        public DateTime? Data_Vencimento { get; set; }
        public DateTime Data_Base { get; set; }
        public int MyProperty { get; set; }
        public int Codigo_Situacao { get; set; }
        public string Nome_Situacao { get; set; }
        public string Numero_Processo { get; set; }
        public int Codigo_Tributo { get; set; }
        public string Descricao_Tributo { get; set; }
        public string Abreviatura_Tributo { get; set; }
        public Decimal Soma_Principal { get; set; }
        public Decimal Soma_Multa { get; set; }
        public Decimal Soma_Juros { get; set; }
        public Decimal Soma_Correcao { get; set; }
        public Decimal Soma_Total { get; set; }
        public DateTime? Data_Inscricao { get; set; }
        public DateTime? Data_Ajuizamento { get; set; }
        public int? Numero_Livro { get; set; }
        public int? Pagina_Livro { get; set; }
        public int? Numero_Certidao { get; set; }
        public bool Notificado { get; set; }
        public DateTime? Data_Pagamento { get; set; }
        public double Numero_Documento { get; set; }
        public Decimal Valor_Pago { get; set; }
        public Decimal Valor_Pago_Real { get; set; }
        public int? Numero_Execucao { get; set; }
        public int? Ano_execucao { get; set; }
        public string Processo_CNJ { get; set; }
        public List<TributoStructure> Tributos { get; set; }
    }

    public class TributoStructure {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public Decimal Valor_Principal { get; set; }
        public Decimal Valor_Multa { get; set; }
        public Decimal Valor_Juros { get; set; }
        public Decimal Valor_Correcao { get; set; }
        public Decimal Valor_Total { get; set; }
    }



}//end namespace