<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="gtiMenu2.aspx.cs" Inherits="UIWeb.Pages.gtiMenu2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>

        function formata(campo, mask, evt) {

            if (document.all) { // Internet Explorer 
                key = evt.keyCode;
            }
            else { // Nestcape 
                key = evt.which;
            }

            string = campo.value;
            i = string.length;

            if (i < mask.length) {
                if (mask.charAt(i) == '§') {
                    return (key > 47 && key < 58);
                } else {
                    if (mask.charAt(i) == '!') { return true; }
                    for (c = i; c < mask.length; c++) {
                        if (mask.charAt(c) != '§' && mask.charAt(c) != '!')
                            campo.value = campo.value + mask.charAt(c);
                        else if (mask.charAt(c) == '!') {
                            return true;
                        } else {
                            return (key > 47 && key < 58);
                        }
                    }
                }
            } else return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="color: #3a8dcc;">
    
            <asp:Label ID="Label3" runat="server" ForeColor="#000066" Text="Sistema Tributário Municipal"></asp:Label>
            &nbsp;<br />
            <br />
        <asp:Label ID="Label1" runat="server" ForeColor="#CC0000" Text="Digite a data de pagamento (máximo de 30 dias):" Font-Underline="True"></asp:Label>
            <br />
        <br />
            <asp:TextBox ID="txtVencto" runat="server" MaxLength="10" onKeyPress="return formata(this, '§§/§§/§§§§', event)"></asp:TextBox>
            &nbsp;&nbsp;
                            <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Red" Text=""></asp:Label>
            <br />
            <br />
            <asp:Button ID="btOK" runat="server" OnClick="btOK_Click" Text="Continuar" />
        </div>
    </form>
</body>
</html>
