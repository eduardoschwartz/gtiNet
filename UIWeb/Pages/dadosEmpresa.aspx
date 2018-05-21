<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dadosEmpresa.aspx.cs" Inherits="UIWeb.Pages.dadosEmpresa" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        
</head>
<body style="height: 643px; font-family:Tahoma; font-size:12px;">
    <form id="form1" runat="server">
     <div style="color: #3a8dcc;">
    
            <asp:Label ID="Label3" runat="server" ForeColor="#000066" Text="Sistema Tributário Municipal"></asp:Label>
            &nbsp;<br />
            Dados Cadastrais de Empresa<br />
            <br />
            <table style="width: 350px;">
                <tr>
                    <td class="auto-style1">Inscrição Municipal..:</td>
                    <td>
                <asp:TextBox ID="txtIM" runat="server" Width="83px" MaxLength="6" TextMode="Number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Número do CNPJ....:</td>
                    <td>
                <asp:TextBox ID="txtCNPJ" runat="server" Width="188px" MaxLength="14" TextMode="Number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td>
                <asp:Button ID="btAcesso" runat="server" Text="Consultar" OnClick="btAcesso_Click" />
                    &nbsp;<asp:Button ID="btPrint" runat="server" Text="Imprimir" OnClick="btPrint_Click" />
                    </td>
                </tr>
            </table>
            <br />
            
             
         <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />

     </div>
        <br />
        <br />
        <asp:Table ID="Tbl" runat="server" Width="610px" BorderStyle="Double" Height="19px" BorderColor="#3a8dcc" CaptionAlign="Left" Font-Bold="False">
            <asp:TableRow Width="550px" BorderStyle="Solid" BorderWidth="1px">
                <asp:TableCell Width="30%">Inscrição Municipal</asp:TableCell>
                <asp:TableCell runat="server" ID="IM" Width="70%" ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Razão Social</asp:TableCell>
                <asp:TableCell ID="RAZAOSOCIAL" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">CNPJ/CPF</asp:TableCell>
                <asp:TableCell ID="CNPJ" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Inscrição Estadual</asp:TableCell>
                <asp:TableCell ID="IE" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Endereço</asp:TableCell>
                <asp:TableCell ID="ENDERECO" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Data de Abertura</asp:TableCell>
                <asp:TableCell ID="DATAABERTURA" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Data de Encerramento</asp:TableCell>
                <asp:TableCell ID="DATAENCERRAMENTO" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow runat="server">
                <asp:TableCell runat="server">Situação</asp:TableCell>
                <asp:TableCell ID="SITUACAO" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow runat="server">
                <asp:TableCell runat="server">Email</asp:TableCell>
                <asp:TableCell ID="EMAIL" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow runat="server">
                <asp:TableCell runat="server">Telefone</asp:TableCell>
                <asp:TableCell ID="TELEFONE" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow runat="server">
                <asp:TableCell runat="server">Regime de ISS</asp:TableCell>
                <asp:TableCell ID="REGIMEISS" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow runat="server">
                <asp:TableCell runat="server">Vigilância Sanitária</asp:TableCell>
                <asp:TableCell ID="VIGSANIT" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow runat="server">
                <asp:TableCell runat="server">Taxa de Licença</asp:TableCell>
                <asp:TableCell ID="TAXALICENCA" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow runat="server">
                <asp:TableCell runat="server">Optante do Simples</asp:TableCell>
                <asp:TableCell ID="SIMPLES" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow runat="server">
                <asp:TableCell runat="server">Micro Emp.Individual</asp:TableCell>
                <asp:TableCell ID="MEI" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow runat="server">
                <asp:TableCell runat="server">Área</asp:TableCell>
                <asp:TableCell ID="AREA" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow runat="server">
                <asp:TableCell runat="server">Proprietário(s)</asp:TableCell>
                <asp:TableCell ID="PROPRIETARIO" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow runat="server">
                <asp:TableCell runat="server">Atividade(s)</asp:TableCell>
                <asp:TableCell ID="CNAE" runat="server"  ForeColor="#3a8dcc"></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </form>
</body>
</html>

