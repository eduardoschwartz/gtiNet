<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="readVRExml.aspx.cs" Inherits="UIWeb.Pages.readVRExml" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 643px; font-family:Tahoma; font-size:11px;">
    <form id="form1" runat="server">
        <div style="color: #3a8dcc;">
            <asp:Label ID="Label3" runat="server" ForeColor="#000066" Text="Sistema Tributário Municipal"></asp:Label>
            &nbsp;<br />
            
            Importação de arquivos do Via Rápida Empresa - VRE<br />
            <br />
            <br />
            <br />
            <asp:Label runat="server" ID="lblArquivo"  Text="Escolha um arquivo para enviar..:" Font-Size="Small" ForeColor="Maroon"></asp:Label> <asp:FileUpload ID="FileUpload1" style=" margin-left:30px;" runat="server" /> 
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Àlvará" />
            <br />
            <br />

        <br /> <asp:button id="btEnviar" runat="server" text="Importar" OnClick="btEnviar_Click" Width="100px" /> &nbsp;&nbsp;&nbsp;
            <br />
            <br />
            <asp:GridView ID="grdMain" runat="server" AutoGenerateColumns="False" >
                <Columns>
                    <asp:BoundField DataField="Seq" HeaderText="Id">
                    <HeaderStyle ForeColor="Maroon" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nome" HeaderText="Razão Social">
                    <HeaderStyle ForeColor="Maroon" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="doc" HeaderText="Cpf/Cnpj">
                    <HeaderStyle ForeColor="Maroon" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Sit" HeaderText="Situação">
                    <HeaderStyle ForeColor="Maroon" HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>

            </asp:GridView>
            <br />
        <asp:Label ID="Statuslbl" runat="server" ForeColor="Red" />
            <br />
            <br />
        </div>
        <br />
    </form> 
</body>

</html>
