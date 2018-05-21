using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace UIWeb.Models
{
    public class clsImovel {

        public ImovelStruct LoadReg(int nCodigo) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                var reg = (from i in db.cadimob
                           join c in db.condominio
                           on i.codcondominio equals c.cd_codigo
                           into ic from c in ic.DefaultIfEmpty()
                           where i.codreduzido == nCodigo
                           select new { i.codreduzido,i.distrito,i.setor,i.quadra,i.lote,i.seq,i.unidade,i.subunidade,c.cd_nomecond,i.imune,i.tipomat,i.nummat,i.li_num,i.li_compl,
                               i.li_quadras,i.li_lotes,i.resideimovel,i.inativo}).FirstOrDefault();

                ImovelStruct row = new ImovelStruct();
                if (reg == null)
                    return row;
                row.Codigo = nCodigo;
                row.Distrito =  reg.distrito;
                row.Setor = reg.setor;
                row.Quadra = reg.quadra;
                row.Lote = reg.lote;
                row.Seq = reg.seq;
                row.Unidade = reg.unidade;
                row.SubUnidade = reg.subunidade;
                row.Inscricao = reg.distrito + "." + reg.setor.ToString("00") + "." + reg.quadra.ToString("0000") + "." + reg.lote.ToString("00000") + "." + reg.seq.ToString("00") + "." + reg.unidade.ToString("00") + "." + reg.subunidade.ToString("000");
                row.NomeCondominio = reg.cd_nomecond.ToString();
                row.Imunidade = reg.imune == null ? false : Convert.ToBoolean(reg.imune);
                row.ResideImovel = reg.resideimovel == null ? false : Convert.ToBoolean(reg.resideimovel);
                row.Inativo = reg.inativo == null ? false : Convert.ToBoolean(reg.inativo);
                if (reg.tipomat == null || reg.tipomat == "M")
                    row.TipoMat = 'M';
                else
                    row.TipoMat = 'T';
                row.NumMatricula = reg.nummat;
                row.QuadraOriginal = reg.li_quadras == null ? "" : reg.li_quadras.ToString();
                row.LoteOriginal = reg.li_lotes == null ? "" : reg.li_lotes.ToString();

                EnderecoStruct regEnd = RetornaEndereco(nCodigo, gtiCore.TipoEndereco.Local);
                row.CodigoLogradouro = regEnd.CodLogradouro;
                row.NomeLogradouro = regEnd.Endereco;
                row.Numero = regEnd.Numero;
                row.Complemento = regEnd.Complemento;
                row.Cep = regEnd.Cep;
                row.CodigoBairro = regEnd.CodigoBairro;
                row.NomeBairro = regEnd.NomeBairro;

                return row;
            }
        }//End LoadReg

        public List<ProprietarioStruct> ListaProprietario(int CodigoImovel,bool Principal=false) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                var reg = (from p in db.proprietario
                           join c in db.cidadao on p.codcidadao equals c.codcidadao
                           where p.codreduzido == CodigoImovel
                           select new { p.codcidadao, c.nomecidadao, p.tipoprop, p.principal,c.cpf,c.cnpj });

                if (Principal)
                    reg = reg.Where(u => u.tipoprop == "P" && u.principal == true);

                List<ProprietarioStruct> Lista = new List<ProprietarioStruct>();
                foreach (var query in reg) {
                    ProprietarioStruct Linha = new ProprietarioStruct();
                    Linha.Codigo = query.codcidadao;
                    Linha.Nome = query.nomecidadao;
                    Linha.Tipo = Convert.ToChar(query.tipoprop);
                    Linha.Principal = Convert.ToBoolean(query.principal);
                    if (!string.IsNullOrEmpty(query.cpf) && query.cpf.ToString().Length > 5)
                        Linha.CPF = query.cpf;
                    else{
                        if (!string.IsNullOrEmpty(query.cnpj) && query.cnpj.ToString().Length > 10)
                            Linha.CPF = query.cnpj;
                        else
                            Linha.CPF = "";
                    }
                    Lista.Add(Linha);

                }
                return Lista;
            }
        }

        public List<LogradouroStruct> ListaLogradouro(String Filter="") {
            using (TributacaoEntities db = new TributacaoEntities()) {
                var reg = (from l in db.logradouro
                           select new { l.codlogradouro,l.endereco});
                if (!String.IsNullOrEmpty(Filter))
                    reg = reg.Where(u => u.endereco.Contains(Filter));

                List<LogradouroStruct> Lista = new List<LogradouroStruct>();
                foreach (var query in reg) {
                    LogradouroStruct Linha = new LogradouroStruct();
                    Linha.CodLogradouro = query.codlogradouro;
                    Linha.Endereco = query.endereco;
                    Lista.Add(Linha);
                }
                return Lista;
            }
        }

        public bool ExisteImovel(Int32 nCodigo) {
            bool bRet = false;
            using (TributacaoEntities db = new TributacaoEntities()) {
                var existingReg = db.cadimob.Count(a => a.codreduzido == nCodigo);
                if (existingReg != 0) {
                    bRet = true;
                }
            }
            return bRet;
        }

        public EnderecoStruct RetornaEndereco(int Codigo, gtiCore.TipoEndereco Tipo) {
            EnderecoStruct regEnd = new EnderecoStruct();
            using (TributacaoEntities db = new TributacaoEntities()) {
                if (Tipo == gtiCore.TipoEndereco.Local) {
                    var reg = (from i in db.cadimob
                               join b in db.bairro on i.li_codbairro equals b.codbairro into ib from b in ib.DefaultIfEmpty()
                               join fq in db.facequadra on new { p1 = i.distrito, p2 = i.setor, p3 = i.quadra, p4 = i.seq } equals new { p1 = fq.coddistrito, p2 = fq.codsetor, p3 = fq.codquadra, p4 = fq.codface } into ifq from fq in ifq.DefaultIfEmpty()
                               join l in db.logradouro on fq.codlogr equals l.codlogradouro into lfq from l in lfq.DefaultIfEmpty()
                               where i.codreduzido == Codigo && b.siglauf == "SP" && b.codcidade == 413
                               select new {
                                   i.li_num, i.li_codbairro, b.descbairro, fq.codlogr, l.endereco,i.li_compl
                               }).FirstOrDefault();
                    if (reg == null)
                        return regEnd;
                    else {
                        regEnd.CodigoBairro = reg.li_codbairro;
                        regEnd.NomeBairro = reg.descbairro.ToString();
                        regEnd.CodigoCidade = 413;
                        regEnd.NomeCidade = "JABOTICABAL";
                        regEnd.UF = "SP";
                        regEnd.CodLogradouro = reg.codlogr;
                        regEnd.Endereco = reg.endereco.ToString();
                        regEnd.Numero = reg.li_num;
                        regEnd.Complemento = reg.li_compl==null?"": reg.li_compl;
                        regEnd.CodigoBairro = reg.li_codbairro;
                        regEnd.NomeBairro = reg.descbairro;
                        clsCep Cep_Class = new clsCep();
                        regEnd.Cep = Cep_Class.RetornaCep(Convert.ToInt32(reg.codlogr), Convert.ToInt16(reg.li_num)) == 0 ? "14870000" : Cep_Class.RetornaCep(Convert.ToInt32(reg.codlogr), Convert.ToInt16(reg.li_num)).ToString("0000");
                    }
                } else if(Tipo == gtiCore.TipoEndereco.Entrega){
                    var reg = (from ee in db.endentrega
                               join b in db.bairro on new { p1 = ee.ee_uf, p2 = ee.ee_cidade, p3 = ee.ee_bairro } equals new { p1 = b.siglauf, p2 = (short?)b.codcidade, p3 = (short?)b.codbairro } into eeb from b in eeb.DefaultIfEmpty()
                               join c in db.cidade on new { p1 = ee.ee_uf, p2 = ee.ee_cidade } equals new { p1 = c.siglauf, p2 = (short?)c.codcidade } into eec from c in eec.DefaultIfEmpty()
                               join l in db.logradouro on ee.ee_codlog equals l.codlogradouro into lee from l in lee.DefaultIfEmpty()
                               where ee.codreduzido == Codigo 
                               select new {
                                   ee.ee_numimovel, ee.ee_bairro, b.descbairro,c.desccidade,ee.ee_uf,ee.ee_cidade,ee.ee_codlog,ee.ee_nomelog,l.endereco,ee.ee_complemento
                               }).FirstOrDefault();
                    if (reg == null)
                        return regEnd;
                    else {
                        regEnd.CodigoBairro = reg.ee_bairro;
                        regEnd.NomeBairro = reg.descbairro.ToString();
                        regEnd.CodigoCidade = reg.ee_cidade;
                        regEnd.NomeCidade = reg.desccidade;
                        regEnd.UF = "SP";
                        regEnd.CodLogradouro = reg.ee_codlog;
                        regEnd.Endereco = reg.ee_nomelog.ToString();
                        if (String.IsNullOrEmpty(regEnd.Endereco))
                            regEnd.Endereco = reg.endereco.ToString();
                        regEnd.Numero = reg.ee_numimovel;
                        regEnd.Complemento = reg.ee_complemento==null?"": reg.ee_complemento;
                        regEnd.CodigoBairro = reg.ee_bairro;
                        regEnd.NomeBairro = reg.descbairro;
                        clsCep Cep_Class = new clsCep();
                        regEnd.Cep = Cep_Class.RetornaCep(Convert.ToInt32(regEnd.CodLogradouro), Convert.ToInt16(reg.ee_numimovel)) == 0 ? "00000000" : Cep_Class.RetornaCep(Convert.ToInt32(regEnd.CodLogradouro), Convert.ToInt16(reg.ee_numimovel)).ToString("0000");
                    }
                }
            }

            return regEnd;
        }

    }//End class
    
           
        public class ImovelStruct{
        public int Codigo { get; set; }
        public short Distrito  { get; set; }
        public short Setor { get; set; }
        public short Quadra { get; set; }
        public int Lote { get; set; }
        public short Seq { get; set; }
        public short Unidade { get; set; }
        public short SubUnidade { get; set; }
        public string NomeCondominio { get; set; }
        public bool? Imunidade { get; set; }
        public string Inscricao { get; set; }
        public char TipoMat { get; set; }
        public long? NumMatricula { get; set; }
        public int? CodigoLogradouro { get; set; }
        public string NomeLogradouro { get; set; }
        public short? Numero { get; set; }
        public string Complemento { get; set; }
        public short? CodigoBairro { get; set; }
        public string NomeBairro { get; set; }
        public string QuadraOriginal { get; set; }
        public string LoteOriginal { get; set; }
        public bool? Inativo { get; set; }
        public bool? ResideImovel { get; set; }
        public string Cep { get; set; }
        public short EE_TipoEndereco { get; set; }
        public int? EE_CodigoLogradouro { get; set; }
        public string EE_NomeLogradouro { get; set; }
        public short? EE_Numero { get; set; }
        public string EE_Complemento { get; set; }
        public short? EE_CodigoBairro { get; set; }
        public short? EE_CodigoCidade { get; set; }
        public string EE_NomeCidade { get; set; }
        public string EE_UF { get; set; }
    }

    public class ProprietarioStruct {
        public int CodigoImovel { get; set; }
        public int Codigo { get; set; }
        public String Nome { get; set; }
        public char Tipo { get; set; }
        public  bool Principal { get; set; }
        public string CPF { get; set; }
    }

    public class LogradouroStruct {
        public int? CodLogradouro { get; set; }
        public string Endereco { get; set; }
    }

    public class EnderecoStruct {
        public int? CodLogradouro { get; set; }
        public string Endereco { get; set; }
        public short? Numero { get; set; }
        public string Complemento { get; set; }
        public string UF { get; set; }
        public short? CodigoBairro { get; set; }
        public string NomeBairro { get; set; }
        public short? CodigoCidade { get; set; }
        public string NomeCidade { get; set; }
        public string Cep { get; set; }
    }

}
