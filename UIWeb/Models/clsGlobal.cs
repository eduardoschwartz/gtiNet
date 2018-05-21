using System;
using System.Collections.Generic;
using System.Linq;

namespace UIWeb.Models {
    public class clsGlobal {
        public static int nPlano;
        public static bool bCodigoProtesto = false;


        public Dados_Basicos Retorna_Dados_Basicos(int nCodigo) {
            Dados_Basicos ret = new Dados_Basicos();
            if (nCodigo < 100000) {
                clsImovel imovel_class = new clsImovel();
                bool bFind = imovel_class.ExisteImovel(nCodigo);
                if (bFind) {
                    ImovelStruct reg = imovel_class.LoadReg(nCodigo);
                    ret.codigo_reduzido = nCodigo;
                    ret.endereco = reg.NomeLogradouro;
                    ret.numero = Convert.ToInt16(reg.Numero);
                    ret.complemento = reg.Complemento;
                    ret.nome_bairro = reg.NomeBairro;
                    ret.Inscricao = reg.Inscricao;
                    List<ProprietarioStruct> regProp = imovel_class.ListaProprietario(nCodigo, true);
                    ret.nome = regProp[0].Nome;
                    ret.cpf_cnpj = regProp[0].CPF;
                    ret.nome_cidade = "JABOTICABAL";
                    ret.nome_uf = "SP";
                    ret.cep = reg.Cep;
                }
            } else if (nCodigo >= 100000 & nCodigo < 500000) {
                clsEmpresa empresa_class = new clsEmpresa();
                EmpresaStruct reg = empresa_class.LoadReg(nCodigo);
                ret.endereco = reg.Endereco;
                ret.numero = Convert.ToInt16(reg.Numero);
                ret.Inscricao = reg.Inscricao_estadual==null?"": reg.Inscricao_estadual;
                ret.complemento = reg.Complemento;
                ret.nome_bairro = reg.NomeBairro;
                ret.nome_cidade = reg.NomeCidade;
                ret.nome_uf = reg.NomeUF;
                ret.nome = reg.RazaoSocial;
                ret.cpf_cnpj = reg.cpf_cnpj;
                ret.cep = reg.Cep;
            } else {
                clsCidadao cidadao_class = new clsCidadao();
                CidadaoStruct reg = cidadao_class.LoadReg(nCodigo);
                ret.nome = reg.Nome;
                ret.cpf_cnpj = reg.Cnpj == null ? "" : reg.Cnpj;
                if (ret.cpf_cnpj == "")
                    ret.cpf_cnpj = reg.Cpf == null ? "" : reg.Cpf;
                if (reg.EtiquetaR != null && reg.EtiquetaR == "S") {
                    ret.endereco = reg.EnderecoR;
                    ret.numero = Convert.ToInt16(reg.NumeroR);
                    ret.complemento = reg.ComplementoR;
                    ret.nome_bairro = reg.NomeBairroR;
                    ret.nome_cidade = reg.NomeCidadeR;
                    ret.nome_uf = reg.UfR;
                    ret.cep = reg.CepR.ToString();
                } else {
                    ret.endereco = reg.EnderecoC;
                    ret.numero = Convert.ToInt16(reg.NumeroC);
                    ret.complemento = reg.ComplementoC;
                    ret.nome_bairro = reg.NomeBairroC;
                    ret.nome_cidade = reg.NomeCidadeC;
                    ret.nome_uf = reg.UfC;
                    ret.cep = reg.CepC.ToString();
                }
            }
       
            return ret;
       }

        public string ParametroGti(string Nome) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                string reg = (from p in db.parametros where p.nomeparam == Nome select p.valparam.ToString()).FirstOrDefault();
                return reg;
            }
        }

        public void DeleteSid(int nSid) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    db.boleto.RemoveRange(db.boleto.Where(i => i.sid == nSid));
                    db.SaveChanges();
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

    }

    public  class Dados_Basicos {
        public int codigo_reduzido { get; set; }
        public string nome { get; set; }
        public string cpf_cnpj { get; set; }
        public string Inscricao { get; set; }
        public string endereco { get; set; }
        public short numero { get; set; }
        public string complemento { get; set; }
        public string cep { get; set; }
        public string nome_bairro { get; set; }
        public string nome_cidade { get; set; }
        public string nome_uf { get; set; }
    }

}