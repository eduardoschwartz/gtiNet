using System;
using System.Collections.Generic;
using System.Linq;

namespace UIWeb.Models {
    public class clsEscritorioContabil {
        public EscritorioContabilStruct LoadReg(int nCodigo) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                var reg = (from c in db.escritoriocontabil
                           join l in db.logradouro
                           on c.codlogradouro equals l.codlogradouro
                           into cl1 from l in cl1.DefaultIfEmpty()
                           where c.codigoesc == nCodigo
                           select new {
                               c.codigoesc, c.nomeesc, c.cpf, c.cnpj, c.rg, c.crc, c.recebecarne, c.codlogradouro, c.nomelogradouro, c.numero, c.complemento,
                               c.uf, c.nomecidade, c.cep, c.telefone, c.email, c.im, c.nomebairro
                           }).FirstOrDefault();

                EscritorioContabilStruct Linha = new EscritorioContabilStruct();
                Linha.Codigo = reg.codigoesc;
                Linha.Nome = reg.nomeesc;
                Linha.Cpf = reg.cpf;
                Linha.Cnpj = reg.cnpj;
                Linha.Rg = reg.rg;
                Linha.CRC = reg.crc;
                Linha.RecebeCarne = reg.recebecarne == null ? false : Convert.ToBoolean(reg.recebecarne);
                Linha.CodigoLogradouro = reg.codlogradouro;
                Linha.Endereco = reg.nomelogradouro;
                Linha.Numero = reg.numero;
                Linha.Complemento = reg.complemento;
                Linha.NomeBairro = reg.nomebairro;
                Linha.NomeCidade = reg.nomecidade;
                Linha.Uf = reg.uf;
                Linha.Cep = reg.cep;
                Linha.Telefone = reg.telefone;
                Linha.Email = reg.email;
                Linha.IM = reg.im;

                return Linha;
            }
        }

        public List<escritoriocontabil> Lista() {
            using (TributacaoEntities db = new TributacaoEntities()) {
                var sql = (from e in db.escritoriocontabil orderby e.nomeesc select e);
                return sql.ToList();
            }
        }

        public void DeleteRecord(Int32 nCodigo) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    escritoriocontabil c = db.escritoriocontabil.First(i => i.codigoesc == nCodigo);
                    db.escritoriocontabil.Remove(c);
                    db.SaveChanges();
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

        public Int32 InsertRecord(escritoriocontabil Reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    Int32 maxCod = db.escritoriocontabil.Max(u => u.codigoesc);
                    Reg.codigoesc = Convert.ToInt16(maxCod + 1);
                    db.escritoriocontabil.Add(Reg);
                    db.SaveChanges();
                    return maxCod + 1;
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }

        public void UpdateRecord(escritoriocontabil Reg) {
            using (TributacaoEntities db = new TributacaoEntities()) {
                try {
                    Int32 nCodigo = Convert.ToInt32(Reg.codigoesc);
                   escritoriocontabil c = db.escritoriocontabil.First(i => i.codigoesc == nCodigo);
                    c.nomeesc = Reg.nomeesc;
                    c.rg = Reg.rg;
                    c.cpf = Reg.cpf;
                    c.cnpj = Reg.cnpj;
                    c.crc = Reg.crc;
                    c.nomelogradouro = Reg.codlogradouro == 0 ? Reg.nomelogradouro : null;
                    c.numero = Reg.numero;
                    c.complemento = Reg.complemento;
                    c.uf = Reg.uf;
                    c.nomecidade = Reg.nomecidade;
                    c.nomebairro = Reg.nomebairro;
                    c.cep = Reg.cep;
                    c.telefone = Reg.telefone;
                    c.email = Reg.email;
                    c.recebecarne = Reg.recebecarne;
                    c.im = Reg.im;
                    db.SaveChanges();
                } catch (Exception ex) {
                    throw (ex.InnerException);
                }
            }
        }


    }


    public class EscritorioContabilStruct {
        public int Codigo { get; set; }
        public String Nome { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string Rg { get; set; }
        public string CRC { get; set; }
        public int? CodigoLogradouro { get; set; }
        public string Endereco { get; set; }
        public int? Numero { get; set; }
        public string Complemento { get; set; }
        public short? CodigoBairro { get; set; }
        public string NomeBairro { get; set; }
        public short? CodigoCidade { get; set; }
        public string NomeCidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool? RecebeCarne { get; set; }
        public int? IM { get; set; }
    }
}