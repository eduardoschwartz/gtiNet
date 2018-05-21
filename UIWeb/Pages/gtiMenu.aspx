<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="gtiMenu.aspx.cs" Inherits="UIWeb.gtiMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="color: #3a8dcc;">
    
            <asp:Label ID="Label3" runat="server" ForeColor="#000066" Text="Sistema Tributário Municipal"></asp:Label>
            &nbsp;<br />
            <br />
        <asp:Label ID="Label1" runat="server" ForeColor="#CC0000" Text="Selecione o serviço desejado:" Font-Underline="True"></asp:Label>
            <br />
        <br />
            <asp:RadioButtonList ID="optList" runat="server">
                <asp:ListItem selected="True">Consulta e atualização de boletos vencidos para pagamento</asp:ListItem>
                <asp:ListItem>Emissão de 2ª via do carnê de IPTU (2018)</asp:ListItem>
                <asp:ListItem>Emissão de 2ª via da Contribuição de Iluminação Pública (CIP) (2018)</asp:ListItem>
                <asp:ListItem>Imprimir os detalhes de um boleto</asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <asp:Button ID="btOK" runat="server" OnClick="btOK_Click" Text="Continuar" />
        </div>
    </form>
</body>
</html>
