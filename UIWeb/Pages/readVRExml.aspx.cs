using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UIWeb.Models;

namespace UIWeb.Pages {
    public partial class readVRExml : System.Web.UI.Page {
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e) {

        }
        
        protected void btEnviar_Click(object sender, EventArgs e) {
            if (!FileUpload1.HasFile) {
                Statuslbl.Text = "Selecione um arquivo para enviar";
                return;
            }
            if (FileUpload1.PostedFile.ContentType.CompareTo("text/xml") == 0) {
                try {
                    UploadArquivo();
                } catch {
                    throw;
                }
            } else {
                Statuslbl.Text = "Apenas arquivos em Xml podem ser enviados";
            }
        }

        private void UploadArquivo() {
            String path = Server.MapPath("~/VRExml/");

            if (FileUpload1.PostedFile.ContentLength < 5102400) {
                string MyPath = @""; // \\networkmachine\foo\bar OR C:\foo\bar
                string MyPathWithoutDriveOrNetworkShare = FileUpload1.PostedFile.FileName.Substring(Path.GetPathRoot(FileUpload1.PostedFile.FileName).Length);

                try {
                    FileUpload1.SaveAs(path + MyPathWithoutDriveOrNetworkShare);
                    List<EmpresaStruct>Lista= ReadFile(path + MyPathWithoutDriveOrNetworkShare);
                    if (Lista.Count > 0) {
                        FillListView(Lista);
                        Grava_Empresas_Vre(Lista);
                        foreach (EmpresaStruct reg in Lista) {
                            foreach (DataRow dr in dt.Rows) {
                                if ((string)dr["Seq"] == reg.id) {
                                    if (reg.Already_inDB)
                                        dr["Sit"] = "Duplicado";
                                    else
                                        dr["Sit"] = "Importado";
                                }
                            }
                        }
                        grdMain.DataSource = dt;
                        grdMain.DataBind();
                    } else {
                        Statuslbl.Text = "Arquivo inválido";
                    }
                    Statuslbl.Text = Lista.Count.ToString() + " Empresas analisadas.";
                } catch  {
                    Statuslbl.Text = "Arquivo inválido";
                    throw;
                }
                
            } else
                Statuslbl.Text = "Upload status: Tamanho máximo 5Mb!";
        }

        private void FillListView(List<EmpresaStruct>Lista) {
            try { 
            DataColumn cl = new DataColumn("Seq"); 
            dt.Columns.Add(cl); 
            cl = new DataColumn("Nome"); 
            dt.Columns.Add(cl);
            cl = new DataColumn("Doc");
            dt.Columns.Add(cl);
            cl = new DataColumn("Sit");
            dt.Columns.Add(cl);

            foreach (EmpresaStruct reg in Lista) {
                DataRow dr = dt.NewRow();
                dr[0] = reg.id;
                dr[1] = reg.Nome.Replace ("&amp;","&");
                dr[2] = reg.Cnpj;
                dr[3] = "";
                dt.Rows.Add(dr);  
            }
            grdMain.DataSource = dt;
            grdMain.DataBind();
            } catch  {
                Statuslbl.Text = "Arquivo inválido";
                throw;
            }

        }

        private List<EmpresaStruct> ReadFile(string sFileName) {
            XElement xmlDoc = XElement.Load(sFileName);
            var empresas = (from cust in xmlDoc.Descendants("Empresa")
                            select new EmpresaStruct {
                                id = cust.Attribute("id").Value,
                                Nome = cust.Element("NomeEmpresarial").Value,
                                Cnpj = cust.Element("CNPJ").Value,
                                DataAbertura = Convert.ToDateTime(cust.Element("DataAbertura").Value),
                                Porte = cust.Element("Porte").Value,
                                NumeroRegistro = cust.Element("NumeroRegistro").Value,
                                TipoRegistro = cust.Element("TipoRegistro").Value,
                                TipoMei = cust.Element("TipoMEI").Value,
                                NomeResponsavel = cust.Element("NomeResponsavel").Value,
                                CpfResponsavel = cust.Element("CPFResponsavel").Value,
                                DDDContato1 = (cust.Elements("DDD1").Any() ? cust.Element("DDD1").Value : ""),
                                FoneContato1 = (cust.Elements("Telefone1").Any() ? cust.Element("Telefone1").Value : ""),
                                DDDContato2 = (cust.Elements("DDD2").Any() ? cust.Element("DDD2").Value : ""),
                                FoneContato2 = (cust.Elements("Telefone2").Any() ? cust.Element("Telefone2").Value : ""),
                                EmailContato = (cust.Elements("Email").Any() ? cust.Element("Email").Value : ""),
                                Endereco = (from end in cust.Descendants("Endereco")
                                            select new EnderecoStruct {
                                                Logradouro = end.Element("Logradouro").Value,
                                                Numero = end.Element("NumeroLogradouro").Value,
                                                SetorQuadraLote = end.Element("SetorQuadraLote").Value,
                                                TipoLogradouro = end.Element("TipoLogradouro").Value,
                                                Complemento = end.Elements("Complemento").Any() ? end.Element("Complemento").Value : "",
                                                Bairro = end.Element("Bairro").Value,
                                                Cidade = end.Element("Municipio") == null ? "" : end.Element("Municipio").Value,
                                                UF = end.Element("UF").Value,
                                                Cep = end.Element("CEP").Value
                                            }).FirstOrDefault(),
                                Atividade = (from atv in cust.Descendants("Atividades")
                                             select new AtividadeStruct {
                                                 Codigo = atv.Elements("CNAE").Select(x => x.Attribute("codigo").Value).Where(s => s != string.Empty).ToArray(),
                                                 Principal = atv.Elements("CNAE").Select(x => x.Attribute("principal").Value).Where(s => s != string.Empty).ToArray(),
                                                 Exercida = atv.Elements("CNAE").Select(x => x.Attribute("exercida").Value).Where(s => s != string.Empty).ToArray()
                                             }).ToList(),
                                ClassifCRCPJ = cust.Element("ClassificacaoCRCContadorPJ").Value,
                                NumeroCRCPJ = cust.Element("NumeroCRCContadorPJ").Value,
                                CNPJContador = cust.Element("CNPJContador").Value,
                                TipoCRCPF = cust.Elements("TipoCRCContadorPF").Any() ? cust.Element("TipoCRCContadorPF").Value : "",
                                TipoCRCPJ = cust.Elements("TipoCRCContadorPJ").Any() ? cust.Element("TipoCRCContadorPJ").Value : "",
                                ClassifCRCPF = cust.Elements("ClassificacaoCRCContadorPF").Any() ? cust.Element("ClassificacaoCRCContadorPF").Value : "",
                                NumeroCRCPF = cust.Elements("NumeroCRCContadorPF").Any() ? cust.Element("NumeroCRCContadorPF").Value : "",
                                UFCRCPF = cust.Elements("UFCRCContadorPF").Any() ? cust.Element("UFCRCContadorPF").Value : "",
                                UFCRCPJ = cust.Elements("UFCRCContadorPJ").Any() ? cust.Element("UFCRCContadorPJ").Value : "",
                                CPFContador = cust.Elements("CPFContador").Any() ? cust.Element("CPFContador").Value : "",
                                Licenciamento = (from lic in cust.Descendants("Licenciamento")
                                                 select new LicenciamentoStruct {
                                                     Solicitacao = lic.Elements("Solicitacao").Select(x => x.Attribute("id").Value).Where(s => s != string.Empty).FirstOrDefault(),
                                                     Orgao = lic.Elements("Orgao").Select(x => x.Attribute("id").Value).Where(s => s != string.Empty).FirstOrDefault(),
                                                     Status = lic.Elements("Status").Select(x => x.Attribute("id").Value).Where(s => s != string.Empty).FirstOrDefault(),
                                                     Risco = lic.Elements("Risco").Select(x => x.Attribute("id").Value).Where(s => s != string.Empty).FirstOrDefault(),
                                                     Numero = lic.Elements("Numero").Any() ? lic.Element("Numero").Value : "",
                                                     DataEmissao = lic.Elements("DataEmissao").Any() ? Convert.ToDateTime(lic.Element("DataEmissao").Value) : Convert.ToDateTime("01/01/1900"),
                                                     DataVencimento = lic.Elements("DataVencimento").Any() ? Convert.ToDateTime(lic.Element("DataVencimento").Value) : Convert.ToDateTime("01/01/1900"),
                                                     Pergunta = lic.Elements("Pergunta").Select(x => x.Attribute("id").Value).Where(s => s != string.Empty).ToList(),
                                                     Resposta = lic.Elements("Pergunta").Select(x => x.Attribute("resposta").Value).Where(s => s != string.Empty).ToList(),
                                                     Declaracao = lic.Elements("Declaracao").Select(x => x.Attribute("id").Value).Where(s => s != string.Empty).ToList(),
                                                     Imovel = (from imv in lic.Descendants("Imovel")
                                                               select new ImovelStruct {
                                                                   AreaEstabelecimento = imv.Element("AreaEstabelecimento").Value,
                                                                   NomeProprietario = imv.Element("NomeProprietario").Value,
                                                                   EmailProprietario = imv.Element("EmailProprietario").Value,
                                                                   TelefoneProprietario = imv.Element("TelefoneProprietario").Value,
                                                                   NomeResponsavelUso = imv.Element("NomeResponsavelUso").Value,
                                                                   TelefoneResponsavelUso = imv.Element("TelefoneResponsavelUso").Value,
                                                                   AreaTotal = imv.Element("AreaTotal").Value,
                                                                   Pavimentos = imv.Element("Pavimentos").Value,
                                                                   Contiguo = imv.Element("Contiguo").Value,
                                                                   OutrosUsos = imv.Element("OutrosUsos").Value
                                                               }).FirstOrDefault()
                                                 }).ToList(),
                                Viabilidade = (from via in cust.Descendants("Viabilidade")
                                               select new ViabilidadeStruct {
                                                   Solicitacao = via.Elements("Solicitacao").Select(x => x.Attribute("id").Value).Where(s => s != string.Empty).ToArray(),
                                                   Status = via.Elements("Status").Select(x => x.Attribute("id").Value).Where(s => s != string.Empty).ToArray(),
                                                   DataStatus = via.Elements("DataStatus").Any() ? via.Element("DataStatus").Value : "",
                                                   RestricaoOperacao = via.Elements("RestricaoOperacao").Select(x => x.Attribute("id").Value).Where(s => s != string.Empty).ToArray()
                                               }).ToList(),
                                Sociedade = (from soc in cust.Descendants("Sociedade")
                                             select new SociedadeStruct {
                                                 Socio = (from sc in soc.Descendants("Socio")
                                                          select new SocioStruct {
                                                              Tipo = sc.Element("Tipo").Value,
                                                              Nome = sc.Element("Nome").Value,
                                                              Numero = sc.Element("Numero").Value,
                                                              PaisOrigem = sc.Element("CodigoPaisOrigem").Value
                                                          }
                                                          ).ToList()
                                             }).ToList()
                            }).ToList();



            return empresas;
        }

        private void Grava_Empresas_Vre(List<EmpresaStruct> Lista) {
            clsEmpresa empresa_class = new clsEmpresa();
            foreach (EmpresaStruct item in Lista) {
                if (empresa_class.ExisteEmpresa_Vre(Convert.ToInt32(item.id))) {
                    item.Already_inDB = true;
                } else {
                    try {
                        vre_empresa reg = new vre_empresa();
                        reg.nome_arquivo = FileUpload1.PostedFile.FileName;
                        reg.data_importacao = DateTime.Now;
                        reg.id = Convert.ToInt32(item.id);
                        reg.razao_social = item.Nome.ToString().Replace("&amp;", "&");
                        reg.cnpj = item.Cnpj;
                        reg.data_abertura = item.DataAbertura;
                        reg.porte = Convert.ToByte(item.Porte);
                        reg.numero_registro = item.NumeroRegistro;
                        reg.tipo_registro = Convert.ToByte(item.TipoRegistro);
                        reg.tipo_mei = Convert.ToByte(item.TipoMei);
                        reg.cpf_responsavel = item.CpfResponsavel;
                        reg.nome_responsavel = item.NomeResponsavel;
                        reg.fone_contato1 = item.DDDContato1 + " " + item.FoneContato1;
                        reg.fone_contato2 = item.DDDContato2 + " " + item.FoneContato2;
                        reg.email_contato = item.EmailContato;
                        reg.setor_quadra_lote = item.Endereco.SetorQuadraLote;
                        reg.tipo_logradouro =  item.Endereco.TipoLogradouro.Length>15? item.Endereco.TipoLogradouro.Substring(0,15): item.Endereco.TipoLogradouro;
                        reg.nome_logradouro = item.Endereco.Logradouro;
                        reg.numero_imovel = gtiCore.IsNumeric(item.Endereco.Numero.ToString()) ? Convert.ToInt32(gtiCore.RetornaNumero(item.Endereco.Numero)) : 0;
                        reg.complemento = gtiCore.Truncate(item.Endereco.Complemento, 47, "...").ToString().TrimEnd();
                        reg.bairro = item.Endereco.Bairro;
                        reg.cidade = item.Endereco.Cidade;
                        reg.uf = item.Endereco.UF;
                        reg.cep = item.Endereco.Cep;
                        reg.area_estabelecimento = item.Licenciamento[0].Imovel == null ? 0 : Convert.ToDouble(item.Licenciamento[0].Imovel.AreaEstabelecimento);
                        reg.nome_proprietario = item.Licenciamento[0].Imovel == null ? "" : item.Licenciamento[0].Imovel.NomeProprietario;
                        reg.email_proprietario = item.Licenciamento[0].Imovel == null ? "" : item.Licenciamento[0].Imovel.EmailProprietario;
                        reg.fone_proprietario = item.Licenciamento[0].Imovel== null ? "" : item.Licenciamento[0].Imovel.TelefoneProprietario;
                        reg.email_proprietario = item.Licenciamento[0].Imovel == null ? "" : item.Licenciamento[0].Imovel.EmailProprietario;
                        reg.nome_responsavel_uso = item.Licenciamento[0].Imovel == null ? "" : item.Licenciamento[0].Imovel.NomeResponsavelUso;
                        reg.fone_responsavel_uso = item.Licenciamento[0].Imovel == null ? "" : item.Licenciamento[0].Imovel.TelefoneResponsavelUso;
                        reg.area_total = item.Licenciamento[0].Imovel == null ? 0 : Convert.ToDouble(item.Licenciamento[0].Imovel.AreaTotal);
                        reg.pavimentos = item.Licenciamento[0].Imovel == null ? Convert.ToByte(0) : Convert.ToByte(item.Licenciamento[0].Imovel.Pavimentos);
                        reg.contiguo = item.Licenciamento[0].Imovel == null ? Convert.ToByte(0) : Convert.ToByte(item.Licenciamento[0].Imovel.Contiguo);
                        reg.outros_usos = item.Licenciamento[0].Imovel== null ? Convert.ToByte(0) : Convert.ToByte(item.Licenciamento[0].Imovel.OutrosUsos);
                        reg.classif_CRC_PJ = item.ClassifCRCPJ == null ? Convert.ToByte(0) : Convert.ToByte(item.ClassifCRCPJ);
                        reg.classif_CRC_PF = item.ClassifCRCPF == null ? Convert.ToByte(0) : Convert.ToByte(item.ClassifCRCPF);
                        reg.numero_CRC_PJ = item.NumeroCRCPJ;
                        reg.cnpj_contador = item.CNPJContador;
                        reg.tipo_CRC_PF = item.TipoCRCPF;
                        reg.tipo_CRC_PJ = item.TipoCRCPJ;
                        reg.numero_CRC_PF = item.NumeroCRCPF;
                        reg.uf_CRC_PF = item.UFCRCPF;
                        reg.uf_CRC_PJ = item.UFCRCPJ;
                        reg.cpf_contador = item.CPFContador;

                        empresa_class.InsertEmpresaVre(reg);
                    } catch (Exception ex) {
                        throw ex;
                    }



                    item.Already_inDB = false;

                    for (int i = 0; i < item.Atividade[0].Codigo.Count(); i++) {
                        vre_atividade regatv = new vre_atividade();
                        regatv.Id = Convert.ToInt32(item.id);
                        regatv.cnae = item.Atividade[0].Codigo[i].ToString();
                        regatv.principal =Convert.ToBoolean( Convert.ToInt16(item.Atividade[0].Principal[i].ToString()));
                        regatv.exercida = Convert.ToBoolean(Convert.ToInt16(item.Atividade[0].Exercida[i].ToString()));

                        try {
                            empresa_class.InsertAtividadeVre(regatv);
                        } catch (Exception ex) {
                            throw ex;
                        }
                                               
                    }
                    for (int i = 0; i < item.Sociedade[0].Socio.Count(); i++) {
                        vre_socio regsoc = new vre_socio();
                        regsoc.Id = Convert.ToInt32(item.id);
                        regsoc.nome = item.Sociedade[0].Socio[i].Nome.ToString();
                        regsoc.numero = item.Sociedade[0].Socio[i].Numero.ToString();
                        try {
                            empresa_class.InsertSocioVre(regsoc);
                        } catch (Exception ex) {
                            throw ex;
                        }
                        
                    }
                    for (int i = 0; i < item.Licenciamento.Count(); i++) {
                        vre_licenciamento reglic = new vre_licenciamento();
                        reglic.empresa_id = Convert.ToInt32(item.id);
                        reglic.solicitacao_Id = Convert.ToInt32(item.Licenciamento[i].Solicitacao);
                        reglic.orgao = Convert.ToInt32(item.Licenciamento[i].Orgao);
                        reglic.status = Convert.ToInt32(item.Licenciamento[i].Status);
                        reglic.Numero = item.Licenciamento[i].Numero;
                        reglic.Risco = Convert.ToBoolean(Convert.ToInt16(item.Licenciamento[i].Risco));
                        reglic.Data_Emissao = Convert.ToDateTime(item.Licenciamento[i].DataEmissao);
                        reglic.Data_Vencimento = Convert.ToDateTime(item.Licenciamento[i].DataVencimento);
                       
                        try {
                            empresa_class.InsertLicenciamentoVre(reglic);
                        } catch (Exception ex) {
                            throw ex;
                        }

                        /*                        for (int p = 0; p < item.Licenciamento[0].Pergunta.Count(); p++) {
                                                    vre_pergunta regper = new vre_pergunta();
                                                    regper.empresa_id = Convert.ToInt32(item.id);
                                                    regper.solicitacao_Id = Convert.ToInt32(item.Licenciamento[0].Solicitacao);
                                                    regper.orgao = Convert.ToInt32(item.Licenciamento[0].Orgao);
                                                    regper.pergunta_id = Convert.ToInt32(item.Licenciamento[0].Pergunta[p]);
                                                    regper.resposta = Convert.ToBoolean( Convert.ToInt16(item.Licenciamento[0].Resposta[p]));
                                                    empresa_class.InsertPerguntaVre(regper);
                                                }
                                                for (int p = 0; p < item.Licenciamento[0].Declaracao.Count(); p++) {
                                                    vre_declaracao regdec = new vre_declaracao();
                                                    regdec.empresa_id = Convert.ToInt32(item.id);
                                                    regdec.solicitacao_id = Convert.ToInt32(item.Licenciamento[0].Solicitacao);
                                                    regdec.orgao = Convert.ToInt32(item.Licenciamento[0].Orgao);
                                                    regdec.declaracao_id = Convert.ToInt32(item.Licenciamento[0].Declaracao[p]);
                                                    empresa_class.InsertDeclaracaoVre(regdec);
                                                }*/

                    }
                }
            }
        }

        class EmpresaStruct {
            public string id { get; set; }
            public string Nome { get; set; }
            public string Cnpj { get; set; }
            public DateTime DataAbertura { get; set; }
            public string Porte { get; set; }
            public string NumeroRegistro { get; set; }
            public string TipoRegistro { get; set; }
            public string TipoMei { get; set; }
            public string CpfResponsavel { get; set; }
            public string NomeResponsavel { get; set; }
            public string DDDContato1 { get; set; }
            public string FoneContato1 { get; set; }
            public string DDDContato2 { get; set; }
            public string FoneContato2 { get; set; }
            public string EmailContato { get; set; }
            public EnderecoStruct Endereco { get; set; }
            public List<AtividadeStruct> Atividade { get; set; }
            public string ClassifCRCPJ { get; set; }
            public string TipoCRCPJ { get; set; }
            public string NumeroCRCPJ { get; set; }
            public string CNPJContador { get; set; }
            public string TipoCRCPF { get; set; }
            public string ClassifCRCPF { get; set; }
            public string NumeroCRCPF { get; set; }
            public string UFCRCPF { get; set; }
            public string UFCRCPJ { get; set; }
            public string CPFContador { get; set; }
            public List<LicenciamentoStruct> Licenciamento { get; set; }
            public List<ViabilidadeStruct> Viabilidade { get; set; }
            public List<SociedadeStruct> Sociedade { get; set; }
            public bool Already_inDB { get; set; }
        }

        class EnderecoStruct {
            public string SetorQuadraLote { get; set; }
            public string TipoLogradouro { get; set; }
            public string Logradouro { get; set; }
            public string Numero { get; set; }
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public string Cep { get; set; }
            public string UF { get; set; }
            public string Cidade { get; set; }
        }

        class AtividadeStruct {
            public string[] Codigo { get; set; }
            public string[] Principal { get; set; }
            public string[] Exercida { get; set; }
        }

        class ViabilidadeStruct {
            public string[] Solicitacao { get; set; }
            public string[] Status { get; set; }
            public string DataStatus { get; set; }
            public string[] RestricaoOperacao { get; set; }
        }

        class LicenciamentoStruct {
            public string Solicitacao { get; set; }
            public string Orgao { get; set; }
            public string Status { get; set; }
            public string Risco { get; set; }
            public string Numero { get; set; }
            public DateTime DataEmissao { get; set; }
            public DateTime DataVencimento { get; set; }
            public List<string> Pergunta { get; set; }
            public List<string> Resposta { get; set; }
            public List<string> Declaracao { get; set; }
            public ImovelStruct Imovel { get; set; }
        }

        class ImovelStruct {
            public string AreaEstabelecimento { get; set; }
            public string NomeProprietario { get; set; }
            public string EmailProprietario { get; set; }
            public string TelefoneProprietario { get; set; }
            public string NomeResponsavelUso { get; set; }
            public string TelefoneResponsavelUso { get; set; }
            public string AreaTotal { get; set; }
            public string Pavimentos { get; set; }
            public string Contiguo { get; set; }
            public string OutrosUsos { get; set; }
        }

        class SociedadeStruct {
            public List<SocioStruct> Socio { get; set; }
        }

        class SocioStruct {
            public string Tipo { get; set; }
            public string Nome { get; set; }
            public string Numero { get; set; }
            public string PaisOrigem { get; set; }
        }

        protected void Button1_Click(object sender, EventArgs e) {
            Response.Redirect("~/Pages/alvara_vre.aspx");
        }
    }
}