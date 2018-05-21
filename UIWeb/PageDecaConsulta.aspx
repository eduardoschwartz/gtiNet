<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageDecaConsulta.aspx.cs" Inherits="UIWeb.PageDecaConsulta" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
            <asp:Label ID="Label3" runat="server" ForeColor="#000066" Text="Sistema Tributário Municipal"></asp:Label>
            &nbsp;<br />
            Consulta as DECAs lançadas<br />
            Integração - SIL<br />
            <br />
&nbsp;<br />
            Período de:
            <asp:TextBox ID="txtDataIni" runat="server" Width="90px"></asp:TextBox>
&nbsp; até&nbsp;
            <asp:TextBox ID="txtDataFim" runat="server" Width="90px"></asp:TextBox>
        &nbsp;
            <asp:Button ID="btConsultar" runat="server" OnClick="btConsultar_Click" Text="Consultar" />
        &nbsp;
            &nbsp;<asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Menu" />
        <br />
        <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />
        <br />
        <br />
            <asp:Label ID="Label4" runat="server" Text="Protocolos SIL:"></asp:Label>
&nbsp;&nbsp;
            <asp:DropDownList ID="cmbProtocolo" runat="server" Height="22px" OnSelectedIndexChanged="cmbProtocolo_SelectedIndexChanged" AutoPostBack="True">
            </asp:DropDownList>
            <br />
            &nbsp;
            <asp:Label ID="Label5" runat="server" Text="Documentos enviados"></asp:Label>
            :
            <asp:GridView ID="grdDoc" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" >
                <Columns>
                    <asp:BoundField DataField="Descricao" HeaderText="Descrição" ItemStyle-Width="200" ItemStyle-Height="20" >
<ItemStyle Height="20px" Width="150px"></ItemStyle>
                    </asp:BoundField>
                     <%--<asp:HyperLinkField DataTextField="Arqreal" ItemStyle-Width="200"  HeaderText="Download" />--%>
                    <asp:TemplateField>
            <ItemTemplate>
                <a href='<%#DataBinder.Eval(Container.DataItem, "Arqrnd") %>'> Download
                </a>
            </ItemTemplate>
        </asp:TemplateField>
                </Columns>

<HeaderStyle BackColor="#3AC0F2" ForeColor="White"></HeaderStyle>

            </asp:GridView>
            <br />
&nbsp;<br />
        <asp:button id="btBack" runat="server" text="Retornar" OnClick="btBack_Click" /> 
    
    </div>
    </form>
</body>
</html>
