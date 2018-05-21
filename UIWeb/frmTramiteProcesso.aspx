<%@ Page Title="" Language="C#" MasterPageFile="~/GtiMaster.Master" AutoEventWireup="true" CodeBehind="frmTramiteProcesso.aspx.cs" Inherits="UIWeb.Pages.frmTramiteProcesso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="conteudo" runat="server">
    <div id="Header1" style="height: 77px">
    
        <br />
    
        <asp:Label ID="Label6" runat="server" Text="Nº do Processo: " Font-Bold="False" ForeColor="#000066"></asp:Label>&nbsp;<asp:TextBox ID="txtNumProc" runat="server" Width="124px"></asp:TextBox>
                &nbsp;<asp:Button ID="btPesquisar" runat="server" OnClick="btPesquisar_Click" Text="Pesquisar" />
        &nbsp;&nbsp;
        <asp:Label ID="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>

        <br />
    
        <asp:Label ID="Label7" runat="server" Text="(Ex: 100-2/2014)" Font-Bold="True" Font-Size="Small" ForeColor="#006600"></asp:Label>

        <br />
        </div>
        <div id="Header2" style="height: auto">
        <table>
            <tr>
                <td><asp:Label ID="lblC1" runat="server" Text="Assunto:" Font-Bold="False" ForeColor="Black"></asp:Label></td>
                <td><asp:Label ID="lblAssunto" runat="server" ForeColor="#000066" Font-Bold="False"></asp:Label></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblC2" runat="server" Text="Complemento:" Font-Bold="False" ForeColor="Black"></asp:Label></td>
                <td><asp:Label ID="lblComplemento" runat="server" ForeColor="#000066" Font-Bold="False"></asp:Label></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblC3" runat="server" Text="Requerente:" Font-Bold="False" ForeColor="Black"></asp:Label></td>
                <td><asp:Label ID="lblRequerente" runat="server" ForeColor="#000066" Font-Bold="False"></asp:Label></td>
            </tr>
        </table>

            <br />
        </div>
        <div id="Grid" style="height: auto">
            <asp:GridView ID="grdMain" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" HeaderStyle-BackColor="#3AC0F2" Width="990px" HeaderStyle-ForeColor="White" Font-Size="Small">
                <Columns>
                    <asp:BoundField DataField="Seq" HeaderStyle-HorizontalAlign="Center" HeaderText="Seq" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40">
                        <HeaderStyle HorizontalAlign="Center" Width="12px" />
                        <ItemStyle Width="10px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Descricao" HeaderText="Descrição" ItemStyle-Width="150" HtmlEncode="false">
                        <HeaderStyle Width="200px" />
                        <ItemStyle Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DataHora" HeaderStyle-HorizontalAlign="Center" HeaderText="Data/Hora" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30">
                        <HeaderStyle HorizontalAlign="Center" Width="50px" />
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Despacho" HeaderStyle-HorizontalAlign="Center" HeaderText="Despacho" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="80px" HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <RowStyle ForeColor="#000066" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#00547E" />
            </asp:GridView>
        </div>
</asp:Content>
