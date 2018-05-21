<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="envioDeca.aspx.cs" Inherits="UIWeb.Pages.envioDeca" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <br />
            <a href="../Deca/DECANOVO.pdf" >Download da Declaração Cadastral - DECA</a>
        <br />
        <br />
        <br />
        Acesso VRE:
        <asp:TextBox ID="txtAcesso" runat="server" MaxLength="8" TextMode="Password"></asp:TextBox>
&nbsp;&nbsp;
        <asp:Button ID="btAcesso" runat="server" OnClick="btAcesso_Click" Text="Acessar" />
            <br />
        <br />
    </div>
    </form>
</body>
</html>
