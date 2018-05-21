using System;
using System.Collections.Generic;
using System.Linq;

namespace UIWeb.Models {
    public class clsCep {
        public List<cep> ListaCepLogradouro(Int32 CodigoLogradouro) {
            using(TributacaoEntities db = new TributacaoEntities()) {
                var sql = (from c in db.cep where c.codlogr == CodigoLogradouro select c);
                return sql.ToList();
            }
        }


        public int RetornaCep(Int32 CodigoLogradouro, Int16 Numero) {
            int nCep = 0;
            int Num1, Num2;
            bool bPar, bImpar;

            if(Numero % 2 == 0) {
                bPar = true; bImpar = false;
            } else {
                bPar = false; bImpar = true;
            }

            using(TributacaoEntities db = new TributacaoEntities()) {
                var Sql = (from c in db.cep where c.codlogr == CodigoLogradouro select c).ToList();
                if(Sql.Count == 0)
                    nCep = 14870000;
                else if(Sql.Count == 1)
                    nCep = Sql[0].cep1;
                else {
                    foreach(var item in Sql) {
                        Num1 = Convert.ToInt32(item.valor1.ToString());
                        Num2 = item.valor2==0?0: Convert.ToInt32(item.valor2.ToString());
                        if(Numero >= Num1 && Numero <= Num2) {
                            if((bImpar && item.impar == true) || (bPar && item.par == true)) {
                                nCep = item.cep1;
                                break;
                            }
                        } else if(Numero >= Num1 && Num2 == 0) {
                            if((bImpar && item.impar == true) || (bPar && item.par == true)) {
                                nCep = item.cep1;
                                break;
                            }
                        }
                    }
                }
            }
            return nCep;
        }



    }//end class
}//end namespace
