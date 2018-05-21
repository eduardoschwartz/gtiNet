using System;
using System.Configuration;
using gtiNet.Modelos;
using gtiNet.BLL;
using System.Web.UI.WebControls;
using gtiNet.DAL;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using CrystalDecisions.CrystalReports.Engine;
using System.Web;
using System.Data.SqlClient;

namespace UIWeb {
    

    public partial class DEmp : System.Web.UI.Page {
        public static string sCnae2;
        public static string sSocio2;
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                txtCNPJ.Text = "";
                txtIM.Text = "";
                lblMsg.Text = "";
            }
        }

        protected void btAcesso_Click(object sender, EventArgs e) {
            lblMsg.Text = "";
            ClearTable();
            if (string.IsNullOrWhiteSpace(txtIM.Text) && string.IsNullOrWhiteSpace(txtCNPJ.Text))
                lblMsg.Text = "Erro: Informação necessária.";
            else {
                if (!string.IsNullOrWhiteSpace(txtIM.Text) && !string.IsNullOrWhiteSpace(txtCNPJ.Text))
                    lblMsg.Text = "Erro: Digite a inscrição municipal ou o cnpj da empresa.";

                else {
                    if (!string.IsNullOrWhiteSpace(txtIM.Text))
                        if (!EmpresaExisteIM(Convert.ToInt32(txtIM.Text)))
                            lblMsg.Text = "Erro: Cadastro inexistente.";
                        else
                            FillTable();
                    else {
                        string sCnpj = txtCNPJ.Text.PadLeft(14, '0');
                        if (!string.IsNullOrWhiteSpace(sCnpj) && !EmpresaExisteCNPJ(sCnpj))
                            lblMsg.Text = "Erro: Cadastro inexistente.";
                        else
                            FillTable();
                    }
                }
            }
        }

        private bool EmpresaExisteIM(int nCodigo) {
            bool nRet = false;
            Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();
            EmpresaBLL obj = new EmpresaBLL();
            Empresa reg = new Empresa();
            reg.Codigo = nCodigo;
            List<Empresa> Lista = obj.Listagem(reg, "");
            if (Lista.Count > 0) {
                IM.Text = Lista[0].Codigo.ToString();
                nRet = true;
            }
            return nRet;
        }


        private bool EmpresaExisteCNPJ(string Cnpj) {
            bool nRet = false;
            Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();
            EmpresaBLL obj = new EmpresaBLL();
            Empresa reg = new Empresa();
            reg.CNPJ = Cnpj;
            List<Empresa> Lista = obj.Listagem(reg, "");
            if (Lista.Count > 0) {
                IM.Text = Lista[0].Codigo.ToString();
                nRet = true;
            }
            return nRet;
        }

        private bool EmpresaIsSimples(int nCodigo) {
            bool nRet = false;
            Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIEicon"].ToString();
            EmpresaBLL obj = new EmpresaBLL();
            Empresa reg = new Empresa();
            reg.Codigo = nCodigo;
            nRet = obj.IsSimples(nCodigo);
            return nRet;
        }

        private void FillTable() {
            sSocio2 = "";
            sCnae2 = "";

            Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();
            EmpresaBLL obj = new EmpresaBLL();
            Empresa reg = new Empresa();
            Int32 Codigo = Convert.ToInt32(IM.Text);
            reg.Codigo = Codigo;
            List<Empresa> Lista = obj.Listagem(reg, "");
            if (Lista[0].CNPJ.Length > 1)
                CNPJ.Text = Convert.ToUInt64(Lista[0].CNPJ).ToString(@"00\.000\.000\/0000\-00");
            else {
                if (Lista[0].CPF.Length > 1)
                    CNPJ.Text = Convert.ToUInt64(Lista[0].CPF).ToString(@"000\.000\.000\-00");
                else
                    CNPJ.Text = "";
            }
            IE.Text = Lista[0].InscEstadual.ToString();
            RAZAOSOCIAL.Text = Lista[0].RazaoSocial;
            DATAABERTURA.Text = Lista[0].DataAbertura.ToString("dd/MM/yyyy");
            if (Lista[0].DataEncerramento.Year > 1900)
                DATAENCERRAMENTO.Text = Lista[0].DataEncerramento.ToString("dd/MM/yyyy");
            else
                DATAENCERRAMENTO.Text = "";
            if (Lista[0].EmpresaSuspensa)
                SITUACAO.Text = "SUSPENSA";
            else {
                if (DATAENCERRAMENTO.Text == "")
                    SITUACAO.Text = "ATIVA";
                else
                    SITUACAO.Text = "ENCERRADA";
            }
            EMAIL.Text = Lista[0].Email.ToString();
            TELEFONE.Text = Lista[0].telefone.ToString();
            AREA.Text = Lista[0].AreaOcupada < 2 ? "N/A" : Lista[0].AreaOcupada.ToString() + " m²";
            String sRegime = obj.RegimeEmpresa(Codigo);
            if (sRegime == "F")
                sRegime = "ISS FIXO";
            else {
                if (sRegime == "V")
                    sRegime = "ISS VARIÁVEL";
                else {
                    if (sRegime == "E")
                        sRegime = "ISS ESTIMADO";
                    else
                        sRegime = "NENHUM";
                }
            }
            REGIMEISS.Text = sRegime;
            VIGSANIT.Text = obj.EmpresaTemVS(Codigo) ? "SIM" : "NÃO";
            TAXALICENCA.Text = obj.EmpresaTemTL(Codigo) ? "SIM" : "NÃO";
            MEI.Text = obj.IsMei(Codigo) ? "SIM" : "NÃO";
            Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIEicon"].ToString();
            SIMPLES.Text = obj.IsSimples(Codigo) ? "SIM" : "NÃO";
            Dados.StringDeConexao = ConfigurationManager.ConnectionStrings["GTIconnection"].ToString();
            List<Cidadao> ListaSocio = obj.ListaEmpresaSocio(Codigo);
            string sSocio = "";
            sSocio2 = "";
            foreach (Cidadao Socio in ListaSocio) {
                sSocio += Socio.Nome + System.Environment.NewLine;
                sSocio2 += Socio.Nome + ", ";
            }
            if (!string.IsNullOrWhiteSpace(sSocio2))
                sSocio2 = sSocio2.Substring(0, sSocio2.Length - 2);
            PROPRIETARIO.Text = "<pre>" + sSocio + "</pre>";
            List<Cnaes> ListaCnae = obj.ListaCnae(Codigo);
            string sCnae = "";
            sCnae2 = "";
            foreach (Cnaes cnae in ListaCnae) {
                sCnae += cnae.Cnae + "-" + cnae.Descricao + System.Environment.NewLine;
                sCnae2 += cnae.Cnae + "-" + cnae.Descricao + System.Environment.NewLine;
            }
        //    if (!string.IsNullOrWhiteSpace(sCnae2))
          //      sCnae2 = sCnae2.Substring(0, sCnae2.Length - 1);

            CNAE.Text = "<pre>" + sCnae + "</pre>";
        }

        private void ClearTable() {
            IM.Text = "";
            CNPJ.Text = "";
            RAZAOSOCIAL.Text = "";
            DATAABERTURA.Text = "";
            DATAENCERRAMENTO.Text = "";
            SITUACAO.Text = "";
            IE.Text = "";
            EMAIL.Text = "";
            TELEFONE.Text = "";
            REGIMEISS.Text = "";
            VIGSANIT.Text = "";
            TAXALICENCA.Text = "";
            SIMPLES.Text = "";
            MEI.Text = "";
            PROPRIETARIO.Text = "";
            CNAE.Text = "";
        }

        protected void btPrint_Click(object sender, EventArgs e) {
            if (String.IsNullOrWhiteSpace(RAZAOSOCIAL.Text))
                lblMsg.Text = "Selecione uma empresa para imprimir";
            else {
                lblMsg.Text = "";

                List<DEmpStruct> aLista = new List<DEmpStruct>(); 
                DebitoDAL obj = new DebitoDAL();
                Int32 SID = obj.GetSID();
                DEmpStruct reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Inscrição Municipal";
                reg.Valor = IM.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Razão Social";
                reg.Valor = RAZAOSOCIAL.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "CNPJ/CPF";
                reg.Valor = CNPJ.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Data de Abertura";
                reg.Valor = DATAABERTURA.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Data de Encerramento";
                reg.Valor = DATAENCERRAMENTO.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Inscrição Estadual";
                reg.Valor = IE.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Situação";
                reg.Valor = SITUACAO.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Email";
                reg.Valor = EMAIL.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Telefone";
                reg.Valor = TELEFONE.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Regime de ISS";
                reg.Valor = REGIMEISS.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Vigilância Sanitária";
                reg.Valor = VIGSANIT.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Taxa de Licença";
                reg.Valor = TAXALICENCA.Text;
                aLista.Add(reg); reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Optante do Simples";
                reg.Valor = SIMPLES.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Micro Emp. Individual";
                reg.Valor = MEI.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Área";
                reg.Valor = AREA.Text;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Proprietário";
                reg.Valor = sSocio2;
                aLista.Add(reg);
                reg = new DEmpStruct();
                reg.nSid = SID;
                reg.Nome = "Atividades";
                reg.Valor = sCnae2;
                aLista.Add(reg);


                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = Dados.StringDeConexao;
                cn.Open();
                int nPos = 1;
                foreach (DEmpStruct registro in aLista) {
                    try {
                        SqlCommand cmd = new SqlCommand(); cmd.Connection = cn;
                        String Sql = "insert into DEmp(sid,nome,valor,seq) values(@sid,@nome,@valor,@seq)";
                        cmd.CommandText = Sql;
                        cmd.Parameters.AddWithValue("@sid", SID);
                        cmd.Parameters.AddWithValue("@nome", registro.Nome);
                        cmd.Parameters.AddWithValue("@valor", registro.Valor==null?"":registro.Valor);
                        cmd.Parameters.AddWithValue("@seq", nPos);
                       
                        cmd.ExecuteScalar();
                        nPos++;
                    }
                    catch (SqlException ex) {
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex) {
                        throw new Exception(ex.Message);
                    }
                }
                cn.Close();

                DataTable tabela = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("select * from DEmp where sid=" + SID, Dados.StringDeConexao);
                da.Fill(tabela);

                ReportDocument crystalReport = new ReportDocument();
                crystalReport.Load(Server.MapPath("~/Report/DEmpresa.rpt"));

                crystalReport.SetDataSource(tabela);
                DeleteSID(SID);

                HttpContext.Current.Response.Buffer = false;
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();


                try {
                    crystalReport.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "DEmp");
                }
                catch {
                    
                }
                finally {
                    crystalReport.Close();
                    crystalReport.Dispose();
                }


            }
        }

        private void DeleteSID(Int32 SID) {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = Dados.StringDeConexao;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            String Sql = "delete from DEmp where sid=@sid";
            cmd.CommandText = Sql;
            cmd.Parameters.AddWithValue("@sid", SID);
            cn.Open(); cmd.ExecuteScalar();
            cmd.Dispose(); cn.Close();
        }

    }


    public class DEmpStruct {
        public int nSid { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
    }

}