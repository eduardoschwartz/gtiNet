using System;
using System.Linq;

namespace UIWeb.Models {
    public class clsCidadao {
        public Int32 InsertRecord(cidadao Reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    Int32 maxCod = db.cidadao.Max(u => u.codcidadao);
                    Reg.codcidadao = maxCod + 1;
                    db.cidadao.Add(Reg);
                    db.SaveChanges();
                    return maxCod + 1;
                } catch (Exception ex) {
                    throw (ex.InnerException);

                }
            }
        }

        public void UpdateRecord(cidadao Reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    Int32 nCodigo = Convert.ToInt32(Reg.codcidadao);
                    cidadao c = db.cidadao.First(i => i.codcidadao == nCodigo);
                    c.nomecidadao = Reg.nomecidadao;
                    c.juridica = Reg.juridica;
                    c.rg = Reg.rg;
                    c.orgao = Reg.orgao;
                    c.cpf = Reg.cpf;
                    c.cnpj = Reg.cnpj;
                    c.data_nascimento = Reg.data_nascimento;
                    c.profissao = Reg.profissao;
                    c.etiqueta = Reg.etiqueta;
                    if (Reg.etiqueta == "S") {
                        c.codlogradouro = Reg.codlogradouro;
                        c.nomelogradouro = Reg.codlogradouro == 0 ? Reg.nomelogradouro : null;
                        c.numimovel = Reg.numimovel;
                        c.complemento = Reg.complemento;
                        c.siglauf = Reg.siglauf;
                        c.codcidade = Reg.codcidade;
                        c.codbairro = Reg.codbairro;
                        c.cep = Reg.cep;
                        c.pais = Reg.pais;
                        c.telefone = Reg.telefone;
                        c.email = Reg.email;
                    } else {
                        c.codlogradouro = null;
                        c.nomelogradouro = null;
                        c.numimovel = null;
                        c.complemento = null;
                        c.siglauf = null;
                        c.codcidade = null;
                        c.codbairro = null;
                        c.cep = null;
                        c.pais = null;
                        c.telefone = null;
                        c.email = null;
                    }
                    if (Reg.etiqueta2 == "S") {
                        c.codlogradouro2 = Reg.codlogradouro2;
                        c.nomelogradouro2 = Reg.codlogradouro2 == 0 ? Reg.nomelogradouro2 : null;
                        c.numimovel2 = Reg.numimovel2;
                        c.complemento2 = Reg.complemento2;
                        c.siglauf2 = Reg.siglauf2;
                        c.codcidade2 = Reg.codcidade2;
                        c.codbairro2 = Reg.codbairro2;
                        c.cep2 = Reg.cep2;
                        c.pais2 = Reg.pais2;
                        c.telefone2 = Reg.telefone2;
                        c.email2 = Reg.email2;
                    } else {
                        c.codlogradouro2 = null;
                        c.nomelogradouro2 = null;
                        c.numimovel2 = null;
                        c.complemento2 = null;
                        c.siglauf2 = null;
                        c.codcidade2 = null;
                        c.codbairro2 = null;
                        c.cep2 = null;
                        c.pais2 = null;
                        c.telefone2 = null;
                        c.email2 = null;
                    }
                    db.SaveChanges();
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

        public void DeleteRecord(Int32 nCodigo) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    cidadao c = db.cidadao.First(i => i.codcidadao == nCodigo);
                    db.cidadao.Remove(c);
                    db.SaveChanges();
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

        public cidadao CNPJDuplicado(String sCNPJ) {
            if (string.IsNullOrEmpty(sCNPJ))
                return null;
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    cidadao c = db.cidadao.First(i => i.cnpj == sCNPJ);
                    return c;
                } catch (Exception) {
                    return null;
                }
            }
        }

        public cidadao CPFDuplicado(String sCPF) {
            if (string.IsNullOrEmpty(sCPF))
                return null;
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    cidadao c = db.cidadao.First(i => i.cpf == sCPF);
                    return c;
                } catch (Exception) {
                    return null;
                }
            }
        }

        public bool ExisteCidadao(Int32 nCodigo) {
            bool bRet = false;
            using (TributacaoEntities db = new TributacaoEntities()) {
                var existingReg = db.cidadao.Count(a => a.codcidadao == nCodigo);
                if (existingReg != 0) {
                    bRet = true;
                }
            }
            return bRet;
        }

        public CidadaoStruct LoadReg(Int32 nCodigo) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                var reg = (from c in db.cidadao join l in db.logradouro  on c.codlogradouro equals l.codlogradouro into cl1 from l in cl1.DefaultIfEmpty()
                           join l2 in db.logradouro  on c.codlogradouro2 equals l2.codlogradouro into cl2 from l2 in cl2.DefaultIfEmpty()
                           join c2 in db.cidade on new {p1=c.siglauf,  p2=(short)c.codcidade } equals new {p1=c2.siglauf, p2=c2.codcidade } into c2c from c2 in c2c.DefaultIfEmpty()
                           join c3 in db.cidade on new { p1 = c.siglauf, p2 = (short)c.codcidade } equals new { p1 = c3.siglauf, p2 = c3.codcidade } into c3c from c3 in c3c.DefaultIfEmpty()
                           where c.codcidadao == nCodigo
                           select new {
                               c.codcidadao, c.nomecidadao, c.cpf, c.cnpj, c.rg, c.orgao, c.profissao, c.data_nascimento, c.juridica,
                               c.codlogradouro, c.codlogradouro2, enderecoR = l.endereco, enderecoC = l2.endereco, c.numimovel, c.numimovel2, c.complemento, c.complemento2,
                               c.etiqueta, c.etiqueta2, c.siglauf, c.siglauf2, c.codbairro, c.codbairro2, c.codcidade, c.codcidade2, c.cep, c.cep2, c.pais, c.pais2, c.telefone, c.telefone2,
                               c.email, c.email2, c.nomelogradouro, c.nomelogradouro2,descidadeR= c2.desccidade,descidadeC=c3.desccidade 
                           }).FirstOrDefault();


                CidadaoStruct Linha = new CidadaoStruct();
                Linha.Codigo = reg.codcidadao;
                Linha.Nome = reg.nomecidadao;
                Linha.Cpf = reg.cpf;
                Linha.Cnpj = reg.cnpj;

                if (!string.IsNullOrEmpty(reg.cpf) && reg.cpf.ToString().Length > 10) {
                    Linha.Cpf = reg.cpf;
                    Linha.Cnpj = "";
                } else {
                    if (!string.IsNullOrEmpty(reg.cnpj) && reg.cnpj.ToString().Length > 10) {
                        Linha.Cpf = "";
                        Linha.Cnpj = reg.cnpj;
                    } else {
                        Linha.Cpf = "";
                        Linha.Cnpj = "";
                    }
                }

                Linha.Rg = reg.rg;
                Linha.Orgao = reg.orgao;
                Linha.Profissao = reg.profissao;
                Linha.DataNascto = reg.data_nascimento;
                Linha.Juridica = Convert.ToByte(reg.juridica);
                Linha.CodigoLogradouroR = reg.codlogradouro;
                Linha.CodigoLogradouroC = reg.codlogradouro2;
                if (reg.codcidade == 413)
                    Linha.EnderecoR = reg.enderecoR;
                else
                    Linha.EnderecoR = reg.nomelogradouro;
                if (reg.codcidade2 == 413)
                    Linha.EnderecoC = reg.enderecoC;
                else
                    Linha.EnderecoC = reg.nomelogradouro2;
                Linha.NumeroR = reg.numimovel;
                Linha.NumeroC = reg.numimovel2;
                Linha.ComplementoR = reg.complemento;
                Linha.ComplementoC = reg.complemento2;
                Linha.EtiquetaR = reg.etiqueta;
                Linha.EtiquetaC = reg.etiqueta2;
                Linha.UfR = reg.siglauf;
                Linha.UfC = reg.siglauf2;
                Linha.CodigoBairroR = reg.codbairro;
                Linha.CodigoBairroC = reg.codbairro2;
                Linha.CodigoCidadeR = reg.codcidade;
                Linha.CodigoCidadeC = reg.codcidade2;
                Linha.NomeCidadeR = reg.descidadeR;
                Linha.NomeCidadeC = reg.descidadeC;
                Linha.CepR = reg.cep;
                Linha.CepC = reg.cep2;
                Linha.PaisR = reg.pais;
                Linha.PaisC = reg.pais2;
                Linha.TelefoneR = reg.telefone;
                Linha.TelefoneC = reg.telefone2;
                Linha.EmailR = reg.email;
                Linha.EmailC = reg.email2;

                return Linha;
            }
        }

        public string NomeCidadao(Int32 nCodigo) {
            String Nome = "";
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    var Reg = (from c in db.cidadao where c.codcidadao == nCodigo select c.nomecidadao).First();
                    Nome = Reg.ToString();
                } catch (Exception) {
                }
                return Nome;
            }
        }

     /*   public String TipoUsuario(Int32 nCodigo) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                var Reg = (from e in db.espolio join tu in db.tipousuario on e.tipo equals tu.codigo where e.codigo == nCodigo select tu).FirstOrDefault();
                if (Reg == null)
                    return "";
                else
                    return Reg.nome;
            }
        }*/


    }


    public class CidadaoStruct {
        public int Codigo { get; set; }
        public String Nome { get; set; }
        public DateTime? DataNascto { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string Rg { get; set; }
        public string Orgao { get; set; }
        public byte Juridica { get; set; }
        public string Profissao { get; set; }
        public int? CodigoLogradouroR { get; set; }
        public string EnderecoR { get; set; }
        public short? NumeroR { get; set; }
        public string ComplementoR { get; set; }
        public short? CodigoBairroR { get; set; }
        public string NomeBairroR { get; set; }
        public short? CodigoCidadeR { get; set; }
        public string NomeCidadeR { get; set; }
        public string UfR { get; set; }
        public int? CepR { get; set; }
        public string PaisR { get; set; }
        public string TelefoneR { get; set; }
        public string EmailR { get; set; }
        public int? CodigoLogradouroC { get; set; }
        public string EnderecoC { get; set; }
        public short? NumeroC { get; set; }
        public string ComplementoC { get; set; }
        public short? CodigoBairroC { get; set; }
        public string NomeBairroC { get; set; }
        public short? CodigoCidadeC { get; set; }
        public string NomeCidadeC { get; set; }
        public string UfC { get; set; }
        public int? CepC { get; set; }
        public string PaisC { get; set; }
        public string TelefoneC { get; set; }
        public string EmailC { get; set; }
        public string EtiquetaR { get; set; }
        public string EtiquetaC { get; set; }
    }

}