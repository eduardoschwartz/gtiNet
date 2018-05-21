<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="damweb.aspx.cs" Inherits="UIWeb.Pages.damweb" %>


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



    <style type="text/css">
        .auto-style1 {
            width: 300px;
        }

        .auto-style2 {
            width: 217px;
        }

        #form1 {
            height: 641px;
        }
        .auto-style3 {
            color: blue;
        }
    </style>

</head>

<body style="height: 643px; font-family:Tahoma; font-size:11px;">
    <form id="form1" runat="server">
        <div style="color: #3a8dcc;">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:Label ID="Label3" runat="server" ForeColor="#000066" Text="Sistema Tributário Municipal"></asp:Label>
            &nbsp;<br />
            Consulta de débitos em aberto<br />
            Emissão de Documento de Arrecadação Municipal (D.A.M.)<br />
            <br />
            <asp:Panel ID="Panel2" runat="server" ForeColor="Black" BorderColor="#3399FF" BorderStyle="Solid" BorderWidth="1px" Width="675px">
                <table style="width: 100%;">
                    <tr>
                        <td class="auto-style1">
                            <asp:RadioButtonList ID="optList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="optList_SelectedIndexChanged" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="1">Imóvel</asp:ListItem>
                                <asp:ListItem Value="2">Empresa</asp:ListItem>
                                <asp:ListItem Value="3">Contribuintes</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:Label ID="lblNome" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            &nbsp;</td>
                        <td>
                            <asp:Label ID="lblEndereco" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">&nbsp;&nbsp;
                            <asp:Label ID="lblCod" runat="server" Text="Código do imóvel..:"></asp:Label>
                            &nbsp;
                            <asp:TextBox ID="txtCod" runat="server" BorderColor="#3399FF" BorderStyle="Solid" BorderWidth="1px" MaxLength="6" Width="70px" OnTextChanged="txtCod_TextChanged"></asp:TextBox>
                            &nbsp; (Sem dígito)</td>
                        <td>
                            <asp:Label ID="lblDoc" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr><td>&nbsp;
                        <asp:RadioButton ID="optCPF" runat="server" AutoPostBack="True" Checked="True" GroupName="optDoc" OnCheckedChanged="optCPF_CheckedChanged" Text="CPF" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optCNPJ" runat="server" AutoPostBack="True" GroupName="optDoc" OnCheckedChanged="optCNPJ_CheckedChanged" Text="CNPJ" />
                        </td><td>
                            <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Red" Text=""></asp:Label>
                        </td></tr>
                    <tr>
                        <td>&nbsp;&nbsp;
                         <asp:Label ID="Label1" runat="server" Text="CPF/CNPJ:"></asp:Label>
                            &nbsp;
                        &nbsp;
                            <asp:TextBox ID="txtCPF" runat="server" BorderColor="#3399FF" BorderStyle="Solid" BorderWidth="1px" MaxLength="14" Width="166px" TabIndex="1"  onKeyPress="return formata(this, '§§§.§§§.§§§-§§', event)"></asp:TextBox>
                            
                            <asp:TextBox ID="txtCNPJ" runat="server" BorderColor="#3399FF" BorderStyle="Solid" BorderWidth="1px" MaxLength="18" onKeyPress="return formata(this, '§§.§§§.§§§/§§§§-§§', event)" TabIndex="1" Visible="False" Width="166px"></asp:TextBox>
                        </td>
                        <td>&nbsp;&nbsp;
                         <asp:Label ID="Label2" runat="server" Text="Data de Vencimento:"></asp:Label>
                            &nbsp;
                        &nbsp;
                            <asp:Label ID="lblVenctoDam" runat="server" Text="Label" CssClass="auto-style3"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0">
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/CImage.ashx" />
                        </td>
                        <td class="auto-style2">&nbsp;Digite o conteúdo da imagem
                            <br />
                            <asp:TextBox ID="txtimgcode" runat="server" OnClick="btConsultar_Click" ViewStateMode="Disabled" Width="147px" TabIndex="3"></asp:TextBox>
                            &nbsp;<asp:ImageButton ID="btConsultar" runat="server" ImageAlign="AbsBottom" ImageUrl="~/Images/icon_24_buscar_on.png" OnClick="btConsultar_Click" ToolTip="Pesquisar débitos em aberto" TabIndex="4" />
                        </td>
                        <td>
                            <asp:Button ID="btSelectAll" runat="server" OnClick="btSelectAll_Click" Text="Selecionar todos" Width="120px" TabIndex="5" />
                            <br />
                            <asp:Button ID="btSelectNone" runat="server" OnClick="btSelectNone_Click" Text="Desmarcar todos" Width="120px" TabIndex="6" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <div style="color: #3a8dcc; width: 675px; height: 373px; overflow: auto;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdMain" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" HeaderStyle-BackColor="#3AC0F2" Width="990px" HeaderStyle-ForeColor="White" TabIndex="7">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRow" runat="server" AutoPostBack="True" OnCheckedChanged="chkRow_CheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Exercicio" HeaderStyle-HorizontalAlign="Center" HeaderText="Ano" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Lancamento" HeaderText="Lancamento" ItemStyle-Width="150" HtmlEncode="false">
                                    <ItemStyle Width="250px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Sequencia" HeaderStyle-HorizontalAlign="Center" HeaderText="Seq" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Parcela" HeaderStyle-HorizontalAlign="Center" HeaderText="Parc" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Complemento" HeaderStyle-HorizontalAlign="Center" HeaderText="Cpl" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DtVencimento" HeaderStyle-HorizontalAlign="Center" HeaderText="Dt.Vencto" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="VlPrincipal" HeaderStyle-HorizontalAlign="Right" HeaderText="Principal" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="VlJuros" HeaderStyle-HorizontalAlign="Right" HeaderText="Juros" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="VlMulta" HeaderStyle-HorizontalAlign="Right" HeaderText="Multa" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="VlCorrecao" HeaderStyle-HorizontalAlign="Right" HeaderText="Correção" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="VlTotal" HeaderStyle-HorizontalAlign="Right" HeaderText="Total" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DtAjuiza" HeaderStyle-HorizontalAlign="Center" HeaderText="Ajuizado" ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Protesto" HeaderStyle-HorizontalAlign="Center" HeaderText="Protesto" ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle Width="70px" />
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
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
        <br />
        <div style="width: 675px">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlTotal" runat="server" BackColor="#006699" BorderStyle="Solid" BorderWidth="1px" ForeColor="White" Height="93px" Width="100%">
                        <asp:Table ID="TableTotal" runat="server" CellPadding="5" HorizontalAlign="Right" Height="85px" Width="680px">
                            <asp:TableHeaderRow ID="TableHeaderRow1" runat="server" ForeColor="#FFCC66" Font-Bold="True">
                                <asp:TableHeaderCell Width="225px"></asp:TableHeaderCell>
                                <asp:TableHeaderCell HorizontalAlign="Right" Width="70px"></asp:TableHeaderCell>
                                <asp:TableHeaderCell HorizontalAlign="Right" Width="70px" Font-Bold="False" Font-Underline="True">Principal</asp:TableHeaderCell>
                                <asp:TableHeaderCell HorizontalAlign="Right" Width="70px" Font-Bold="False" Font-Underline="True">Juros</asp:TableHeaderCell>
                                <asp:TableHeaderCell HorizontalAlign="Right" Width="70px" Font-Bold="False" Font-Underline="True">Multa</asp:TableHeaderCell>
                                <asp:TableHeaderCell HorizontalAlign="Right" Width="70px" Font-Bold="False" Font-Underline="True">Correção</asp:TableHeaderCell>
                                <asp:TableHeaderCell HorizontalAlign="Right" Width="70px" Font-Bold="False" Font-Underline="True">Total</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow ID="TableRow1" runat="server" ForeColor="White">
                                <asp:TableCell Width="225px"></asp:TableCell>
                                <asp:TableCell ID="Descricao" runat="server" HorizontalAlign="Right" Width="150px">Total Devido:</asp:TableCell>
                                <asp:TableCell ID="Principal" runat="server" HorizontalAlign="Right" Width="70px">0,00</asp:TableCell>
                                <asp:TableCell ID="Juros" runat="server" HorizontalAlign="Right" Width="70px">0,00</asp:TableCell>
                                <asp:TableCell ID="Multa" runat="server" HorizontalAlign="Right" Width="70px">0,00</asp:TableCell>
                                <asp:TableCell ID="Correcao" runat="server" HorizontalAlign="Right" Width="70px">0,00</asp:TableCell>
                                <asp:TableCell ID="Total" runat="server" HorizontalAlign="Right" Width="70px">0,00</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow2" runat="server" ForeColor="Yellow">
                                <asp:TableCell Width="225px"></asp:TableCell>
                                <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="Right" Width="150px">Total Selecionado:</asp:TableCell>
                                <asp:TableCell ID="Principal2" runat="server" HorizontalAlign="Right" Width="70px">0,00</asp:TableCell>
                                <asp:TableCell ID="Juros2" runat="server" HorizontalAlign="Right" Width="70px">0,00</asp:TableCell>
                                <asp:TableCell ID="Multa2" runat="server" HorizontalAlign="Right" Width="70px">0,00</asp:TableCell>
                                <asp:TableCell ID="Correcao2" runat="server" HorizontalAlign="Right" Width="70px">0,00</asp:TableCell>
                                <asp:TableCell ID="Total2" runat="server" HorizontalAlign="Right" Width="70px">0,00</asp:TableCell>
                            </asp:TableRow>


                        </asp:Table>
                        <br />

                        <br />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div style="float: right;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Pnlresumo" runat="server" BorderStyle="Solid" BorderWidth="1px" ForeColor="White" HorizontalAlign="Center">
                            <asp:Table ID="TableResumo" runat="server" CellPadding="5" HorizontalAlign="Right" Height="65px" BackColor="SandyBrown">
                                
                                <asp:TableRow ID="RowTot" runat="server" ForeColor="Black">
                                    <asp:TableCell ID="TableCell12" runat="server" HorizontalAlign="Right" Width="150px">Total da Guia</asp:TableCell>
                                    <asp:TableCell ID="TotalGuia" runat="server" HorizontalAlign="Right" Width="70px">0,00</asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow ID="RowVencto" runat="server" ForeColor="Black">
                                    <asp:TableCell ID="TableCell16" runat="server" HorizontalAlign="Right" Width="150px">Data Vencimento</asp:TableCell>
                                    <asp:TableCell ID="lblVencto" runat="server" HorizontalAlign="Right" Width="70px">12/08/2014</asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>


                            <br />
                            <br />
                            <asp:Label ID="lblValidate" runat="server" Text="... " ForeColor="#CC0000"></asp:Label>
                        </asp:Panel>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div>
                <asp:Label ID="lblMsg2" runat="server" ForeColor="Maroon" Text="lblMsg2"></asp:Label>
                <br />
                <br />
                <asp:Button ID="btPrint" runat="server" align="left" colspan="2" OnClick="btPrint_Click" Text="Emissão de Guia" Width="150px" TabIndex="8" />
            </div>
        </div>

        <br />
        <br />

    </form>
</body>
</html>
