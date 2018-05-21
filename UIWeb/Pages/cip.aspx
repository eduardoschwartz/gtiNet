<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cip.aspx.cs" Inherits="UIWeb.Pages.cip" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">



    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 188px;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
     <div style="color: #3a8dcc;">
    
            <asp:Label ID="Label3" runat="server" ForeColor="#000066" Text="Sistema Tributário Municipal"></asp:Label>
            &nbsp;<br />
            Contribuição de Iluminação Pública - C.I.P.<br />
            <br />
            <table style="width: 350px;">
                
                <tr>
                    <td class="auto-style1">Número do Documento....:</td>
                    <td>
                <asp:TextBox ID="txtNumDoc" runat="server" Width="145px" MaxLength="9"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td>
                <asp:Button ID="btAcesso" runat="server" Text="Consultar" OnClick="btAcesso_Click" />
                    &nbsp;</td>
                </tr>
            </table>
            
             
         <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />

     </div>
        <br />
        <asp:Table ID="Tbl" runat="server" Width="854px" BorderStyle="Double" Height="19px" BorderColor="#3a8dcc" CaptionAlign="Left" Font-Bold="False">
            <asp:TableRow Width="550px" BorderStyle="Solid" BorderWidth="1px">
                <asp:TableCell Width="20%">Inscrição Municipal</asp:TableCell>
                <asp:TableCell runat="server" ID="IM" Width="80%" ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Nome do Proprietário</asp:TableCell>
                <asp:TableCell ID="NOME" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Endereço do imóvel</asp:TableCell>
                <asp:TableCell ID="ENDERECOIMOVEL" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Bairro do imóvel</asp:TableCell>
                <asp:TableCell ID="BAIRRO" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <br />
        <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="#0000CC" Text="Veja onde localizar o número do documento:"></asp:Label>
        <br />
        <br />
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/doc.jpg" />
    </form>

</body>
</html>
