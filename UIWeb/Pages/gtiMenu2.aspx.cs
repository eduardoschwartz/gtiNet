using System;
using System.Security.Cryptography;
using System.Web;
using System.Text;


namespace UIWeb.Pages {
    public partial class gtiMenu2 : System.Web.UI.Page {
        DateTime DataDAM;

        private static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };


        protected void Page_Load(object sender, EventArgs e) {
            //txtVencto.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        protected void btOK_Click(object sender, EventArgs e) {
            if (!DateTime.TryParse(txtVencto.Text, out DataDAM)) {
                lblmsg.Text = "Data de vencimento inválida.";
                return;
            } else {
                String sDataVencto = txtVencto.Text;
                String sDataNow = DateTime.Now.ToString("dd/MM/yyyy");
                if (DateTime.ParseExact(sDataVencto, "dd/MM/yyyy", null) < DateTime.ParseExact(sDataNow, "dd/MM/yyyy", null)) {
                    lblmsg.Text = "Vencimento menor que a data atual.";
                    return;
                } else {
                    Int32 DifDias = ((TimeSpan)(DataDAM - DateTime.Now)).Days;
                    if (DifDias > 30) {
                        lblmsg.Text = "Vencimento máximo de 30 dias.";
                        return;
                    } else
                        Response.Redirect("~/Pages/damweb.aspx?d=" + HttpUtility.UrlEncode(this.Encrypt(DataDAM.ToString("dd/MM/yyyy"))));
                }
            }
        }



        private string Encrypt(string clearText) {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(clearText);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
           
        }
    }
}