<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="damwebend.aspx.cs" Inherits="UIWeb.Pages.damwebend" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        
        #form1 {
            height: 265px;
        }
        </style>




</head>
<body style="height: 643px; font-family:Tahoma; font-size:11px;">
    <form id="form1" runat="server">
        <div style="color: #3a8dcc;">
            <asp:Label ID="Label3" runat="server" ForeColor="#000066" Text="Sistema Tributário Municipal"></asp:Label>
            &nbsp;<br />
            Consulta de débitos em aberto<br />
            Emissão de Documento de Arrecadação Municipal (D.A.M.)<br />
            <br />
            <br />
            <asp:Label ID="Label6" runat="server" Font-Size="Small" Text="Código do contribuinte:"></asp:Label>
&nbsp;<asp:Label ID="lblCod" runat="server" Font-Size="Small" ForeColor="#990000" Text="000000"></asp:Label>
            <br />
            <asp:Label ID="Label7" runat="server" Font-Size="Small" Text="Nome/Razão social: "></asp:Label>
&nbsp;<asp:Label ID="lblNome" runat="server" Font-Size="Small" ForeColor="Maroon" Text="Label"></asp:Label>
                       <br />
            <asp:Label ID="Label1" runat="server" Font-Size="Small" Text="Data de vencimento: "></asp:Label>
&nbsp;<asp:Label ID="lblDtVencto" runat="server" Font-Size="Small" ForeColor="Maroon" Text="Label"></asp:Label>
          
                        <br />
            <asp:Label ID="Label4" runat="server" Font-Size="Small" Text="Valor total da guia: "></asp:Label>
&nbsp;<asp:Label ID="lblValorGuia" runat="server" Font-Size="Small" ForeColor="Maroon" Text="Label"></asp:Label>   <br />
            <br />
            <asp:Button ID="btPrint" runat="server" OnClick="btPrint_Click" Text="Imprimir" ToolTip="Imprimir a segunda via do carnê de IPTU" />
            <br />
            <br />
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <br />
            <br />
            <asp:HyperLink ID="HyperLink1" runat="server" Font-Size="Small" NavigateUrl="~/Pages/damweb.aspx">Consultar outro código</asp:HyperLink>
            <br />
            <br />
            <asp:HyperLink ID="HyperLink2" runat="server" Font-Size="Small" NavigateUrl="~/Pages/gtiMenu.aspx">Voltar ao menu principal</asp:HyperLink>
            <br />
        </div>
        <br />
    </form> 
</body>

    

</html>