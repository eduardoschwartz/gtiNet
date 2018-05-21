using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UIWeb.Models {
    public class clsEmpresa {

        public bool ExisteEmpresa(int nCodigo) {
            bool bRet = false;
            using (TributacaoEntities db = new TributacaoEntities()) {
                var existingReg = db.mobiliario.Count(a => a.codigomob == nCodigo);
                if (existingReg != 0) {
                    bRet = true;
                }
            }
            return bRet;
        }

        public int ExisteEmpresaCnpj(string sCNPJ) {
            int nCodigo = 0;
            using (TributacaoEntities db = new TributacaoEntities()) {
                var existingReg = db.mobiliario.Count(a => a.cnpj == sCNPJ);
                if (existingReg != 0) {
                    int reg = (from m in db.mobiliario where m.cnpj == sCNPJ select m.codigomob).FirstOrDefault();
                    nCodigo = reg;
                }
            }
            return nCodigo;
        }

        public bool EmpresaSuspensa(int nCodigo) {
            bool bRet = false;
            using (TributacaoEntities db = new TributacaoEntities()) {
                // var eventos = db.Database.SqlQuery<spMobiliarioEvento_Result>("[spMobiliarioEvento]").ToList();
                var existingReg = db.mobiliarioevento.Count(a => a.codmobiliario == nCodigo);
                if (existingReg != 0) {
                    int sit = (from m in db.mobiliarioevento where m.codmobiliario == nCodigo orderby m.dataevento descending select m.codtipoevento).FirstOrDefault();
                    if (sit == 2)
                        bRet = true;
                }
            }
            return bRet;
        }

        public string RegimeEmpresa(int nCodigo) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                int tributo = (from m in db.mobiliarioatividadeiss where m.codmobiliario == nCodigo select m.codtributo).FirstOrDefault();
                if (tributo ==11)
                    return "F";
                else {
                    if (tributo == 12)
                        return "E";
                    else {
                        if (tributo == 13)
                            return "V";
                        else
                            return "N";
                    }
                }
            }
        }

        public EmpresaStruct LoadReg(int nCodigo) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                var reg = (from m in db.mobiliario
                           join b in db.bairro on new {p1=m.codbairro.Value,p2=m.codcidade.Value,p3=m.siglauf } equals new {p1= b.codbairro, p2=b.codcidade,p3=b.siglauf } into mb from b in mb.DefaultIfEmpty()
                           join c in db.cidade on new { p1 = m.codcidade.Value, p2 = m.siglauf } equals new { p1 = c.codcidade, p2 = c.siglauf } into mc from c in mc.DefaultIfEmpty()
                           join l in db.logradouro on m.codlogradouro equals l.codlogradouro into lm from l in lm.DefaultIfEmpty()
                           join h in db.horariofunc on m.horario equals h.codhorario into hm from h in hm.DefaultIfEmpty()
                           where m.codigomob == nCodigo
                           select new {m.codigomob, m.razaosocial,b.descbairro,m.codlogradouro,l.endereco,m.numero,m.complemento,c.desccidade,m.siglauf,m.cpf,m.cnpj,m.inscestadual,
                           m.dataabertura,m.dataencerramento,m.emailcontato,m.fonecontato,m.areatl,h.deschorario,m.horarioext,m.ativextenso}).FirstOrDefault();

                EmpresaStruct row = new EmpresaStruct();
                if (reg == null)
                    return row;
                row.Codigo = nCodigo;
                row.RazaoSocial = reg.razaosocial;
                row.NomeBairro = reg.descbairro;
                row.NomeCidade = reg.desccidade;
                row.NomeUF = reg.siglauf;
                row.Endereco = reg.endereco;
                row.Numero = reg.numero;
                row.Complemento=reg.complemento;
                row.cpf_cnpj=reg.cnpj == null? "" : reg.cnpj;
                if (row.cpf_cnpj == "")
                    row.cpf_cnpj = reg.cpf == null ? "" : reg.cpf;

                if (!string.IsNullOrEmpty(reg.cpf) && reg.cpf.ToString().Length > 10)
                    row.cpf_cnpj = reg.cpf;
                else {
                    if (!string.IsNullOrEmpty(reg.cnpj) && reg.cnpj.ToString().Length > 10)
                        row.cpf_cnpj = reg.cnpj;
                    else
                        row.cpf_cnpj = "";
                }
                
                if (reg.cnpj != null)
                    row.Juridica = true;
                else
                    row.Juridica = false;

                clsCep Cep_Class = new clsCep();
                row.Cep = Cep_Class.RetornaCep(Convert.ToInt32(reg.codlogradouro), Convert.ToInt16(reg.numero)) == 0 ? "00000000" : Cep_Class.RetornaCep(Convert.ToInt32(reg.codlogradouro), Convert.ToInt16(reg.numero)).ToString("0000");
                row.Inscricao_estadual = reg.inscestadual;
                row.Data_Abertura = Convert.ToDateTime(reg.dataabertura);
                row.Data_Encerramento = reg.dataencerramento == null ? null : reg.dataencerramento;
                string sSituacao = "";
                if (gtiCore.IsDate(row.Data_Encerramento))
                    sSituacao = "ENCERRADA";
                else {
                    if (EmpresaSuspensa(nCodigo))
                        sSituacao = "SUSPENSA";
                    else
                        sSituacao = "ATIVA";
                }
                row.Situacao = sSituacao;
                row.Email = reg.emailcontato == null ? "" : reg.emailcontato;
                row.Telefone = reg.fonecontato == null ? "" : reg.fonecontato;
                row.Area= reg.areatl == null ? 0 : Convert.ToDecimal(reg.areatl);
                string horario = reg.horarioext == null || reg.horarioext == "" ? "" : reg.horarioext;
                if (horario == "")
                    row.Horario = reg.deschorario == null ? "" : reg.deschorario;
                else
                    row.Horario = horario;

                row.AtividadeExtenso= reg.ativextenso == null ? "" : reg.ativextenso;

                return row;
            }
        }//End LoadReg

        public SilStructure CarregaSil(int Codigo) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                var reg = (from s in db.sil where s.codigo == Codigo
                           select new {
                               s.codigo, s.protocolo, s.data_emissao, s.data_validade, s.area_imovel
                           }).FirstOrDefault();

                SilStructure row = new SilStructure();
                if (reg == null)
                    return row;
                row.Codigo = Codigo;
                row.Data_Emissao = reg.data_emissao;
                row.Data_Validade = reg.data_validade;
                row.Protocolo = reg.protocolo;
                row.AreaImovel = reg.area_imovel;
                return (row);
            }
        }


        public bool Empresa_tem_VS(int nCodigo) {
            bool bRet = false;
            using (TributacaoEntities db = new TributacaoEntities()) {
                var existingReg = db.mobiliarioatividadevs2.Count(a => a.codmobiliario == nCodigo);
                if (existingReg != 0) {
                    bRet = true;
                }
            }
            return bRet;
        }

        public bool Empresa_tem_TL(int nCodigo) {
            bool ret = true;
            using (TributacaoEntities db = new TributacaoEntities()) {
                byte? isento = (from m in db.mobiliario where m.codigomob == nCodigo && m.isentotaxa!=null select m.isentotaxa).FirstOrDefault();
                if (Convert.ToBoolean(isento))
                    return false;
            }
            return ret;
        }

        public bool Empresa_Mei(int nCodigo) {
            bool ret = true;
            using (TributacaoEntities db = new TributacaoEntities()) {
                var existingReg = db.mei.Count(a => a.codigo == nCodigo);
                if (existingReg == 0) {
                    ret = false;
                } else {
                    DateTime? datafim = (from m in db.mei where m.codigo == nCodigo select m.datafim).FirstOrDefault();
                    if (gtiCore.IsDate(datafim))
                        return false;
                }
            }
            return ret;
        }

        public bool Empresa_Simples(int nCodigo) {
            bool ret = true;
            using (GTI_EiconEntities db = new GTI_EiconEntities()) {
                var existingReg = db.tb_inter_empr_snacional_giss.Count(a => a.INSCRICAO == nCodigo);
                if (existingReg == 0) {
                    ret = false;
                } else {
                    DateTime? datafim = (from m in db.tb_inter_empr_snacional_giss
                                         where m.INSCRICAO == nCodigo orderby m.DATA_FIM descending select m.DATA_FIM).FirstOrDefault();
                    if (datafim!=null)
                        return false;
                }
            }
            return ret;
        }

        public List<CidadaoStruct>ListaSocio(int nCodigo) {
            List<CidadaoStruct> Lista=new List<CidadaoStruct>();
            clsCidadao cidadao_classs = new clsCidadao();
            using (TributacaoEntities db = new TributacaoEntities()) {
                List<int> Socios = (from m in db.mobiliarioproprietario where m.codmobiliario == nCodigo select m.codcidadao).ToList();
                foreach(int Cod in Socios) {
                    CidadaoStruct reg = cidadao_classs.LoadReg(Cod);
                    Lista.Add(reg);
                }
                return Lista;
            }
        }

        public List<CnaeStruct> ListaCnae(int nCodigo) {
            List<CnaeStruct> Lista = new List<CnaeStruct>();
            using (TributacaoEntities db = new TributacaoEntities()) {
               var rows = (from m in db.mobiliariocnae join c in db.cnaesubclasse on
                           new { p1 = m.divisao, p2 = m.grupo, p3 = m.classe, p4 = m.subclasse } equals
                           new { p1 = c.divisao, p2 = c.grupo, p3 = c.classe, p4 = c.subclasse }
                           where m.codmobiliario==nCodigo
                           select new { m.cnae, c.descricao });
                foreach (var reg in rows) {
                    CnaeStruct Linha = new CnaeStruct();
                    Linha.Cnae = reg.cnae;
                    Linha.Descricao = reg.descricao;
                    Lista.Add(Linha);
                }
                return Lista;
            }
        }

        public void Grava_DEmp(List<DEmpresa> Lista) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    foreach (DEmpresa reg in Lista) {
                        db.DEmpresa.Add(reg);
                        db.SaveChanges();
                    }
                } catch (Exception ex) {
                    throw (ex.InnerException);

                }
            }
        }

        public List<DEmpresa> ListaDEmpresa(int nSid) {
            List<DEmpresa> reg;
            using (TributacaoEntities db = new TributacaoEntities()) {
                reg = (from b in db.DEmpresa where b.sid == nSid select b).ToList();
                return reg;
            }
        }

        public void Delete_DEmpresa(int nSid) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.DEmpresa.RemoveRange(db.DEmpresa.Where(i => i.sid == nSid));
                    db.SaveChanges();
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

        public void InsertDeclaracaoVre(vre_declaracao reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.vre_declaracao.Add(reg);
                    db.SaveChanges();

                } catch (Exception ex) {
                    throw (ex.InnerException);

                }
            }
        }

        public void InsertPerguntaVre(vre_pergunta reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.vre_pergunta.Add(reg);
                    db.SaveChanges();

                } catch (Exception ex) {
                    throw (ex.InnerException);

                }
            }
        }

        public void InsertLicenciamentoVre(vre_licenciamento reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.vre_licenciamento.Add(reg);
                    db.SaveChanges();

                } catch (Exception ex) {
                    throw (ex.InnerException);

                }
            }
        }

        public void InsertSocioVre(vre_socio reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.vre_socio.Add(reg);
                    db.SaveChanges();

                } catch (Exception ex) {
                    throw (ex.InnerException);

                }
            }
        }

        public void InsertAtividadeVre(vre_atividade reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.vre_atividade.Add(reg);
                    db.SaveChanges();

                } catch (Exception ex) {
                    throw (ex.InnerException);

                }
            }
        }

        public void InsertEmpresaVre(vre_empresa reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.vre_empresa.Add(reg);
                    db.SaveChanges();

                } catch (Exception ex) {
                    throw (ex.InnerException);

                }
            }
        }

        public bool ExisteEmpresa_Vre(int nCodigo) {
            bool bRet = false;
            using (TributacaoEntities db = new TributacaoEntities()) {
                var existingReg = db.vre_empresa.Count(a => a.id == nCodigo);
                if (existingReg != 0) {
                    bRet = true;
                }
            }
            return bRet;
        }

    }//end class

    public class EmpresaStruct {
        public int Codigo { get; set; }
        public string RazaoSocial { get; set; }
        public string Endereco { get; set; }
        public short? Numero { get; set; }
        public string Complemento { get; set; }
        public string NomeBairro { get; set; }
        public string NomeCidade { get; set; }
        public string NomeUF { get; set; }
        public string Cep { get; set; }
        public bool Juridica { get; set; }
        public string cpf_cnpj { get; set; }
        public string Inscricao_estadual { get; set; }
        public DateTime Data_Abertura { get; set; }
        public DateTime? Data_Encerramento { get; set; }
        public string Situacao { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public decimal Area { get; set; }
        public string Horario { get; set; }
        public string AtividadeExtenso { get; set; }

    }

    public class CnaeStruct {
        public string Cnae { get; set; }
        public string Descricao { get; set; }
    }

    public class SilStructure {
        public int Codigo { get; set; }
        public string Protocolo { get; set; }
        public DateTime? Data_Emissao { get; set; }
        public DateTime? Data_Validade { get; set; }
        public double? AreaImovel { get; set; }
    }


}