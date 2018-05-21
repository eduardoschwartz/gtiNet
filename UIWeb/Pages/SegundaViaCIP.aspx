<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SegundaViaCIP.aspx.cs" Inherits="UIWeb.Pages.SegundaViaCIP" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        
        #form1 {
            height: 170px;
        }
        .auto-style3 {
            width: 333px;
        }
        .auto-style4 {
            width: 433px;
        }
        .auto-style5 {
            height: 230px;
        }
        .auto-style6 {
            font-size: x-small;
        }
    </style>

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
<body style="height: 643px; font-family:Tahoma; font-size:11px;">
    <form id="form1" runat="server" class="auto-style5">
        <div style="color: #3a8dcc;">
            <asp:Label ID="Label3" runat="server" ForeColor="#000066" Text="Sistema Tributário Municipal"></asp:Label>
            &nbsp;<br />
            
            Emissão de segunda via da Contribuição de Iluminação Pública (CIP) - 2018<br />
            <br />
            <asp:Panel ID="Panel2" runat="server" ForeColor="Black" BorderColor="#3399FF" BorderStyle="Solid" BorderWidth="1px" Width="603px">
                <table style="width: 100%;">


                    <tr>
                        <td class="auto-style3">&nbsp;&nbsp;
                            <asp:Label ID="lblCod" runat="server" Text="Código do imóvel..:" Font-Size="X-Small"></asp:Label>
                            &nbsp;
                            <asp:TextBox ID="txtCod" runat="server" BorderColor="#3399FF" BorderStyle="Solid" BorderWidth="1px" MaxLength="6" Width="70px"  onKeyPress="return formata(this, '§§§§§§', event)"></asp:TextBox>
                            &nbsp; <span class="auto-style6">(Sem dígito)</span></td>

                    </tr>
                    <tr>
                        <td class="auto-style3">&nbsp;&nbsp;
                            <asp:Label ID="Label1" runat="server" Text="Inscrição Cadastral..:" Font-Size="X-Small"></asp:Label>
                            &nbsp;<asp:TextBox ID="txtIC" runat="server" BorderColor="#3399FF" BorderStyle="Solid" BorderWidth="1px" MaxLength="25" Width="197px" onKeyPress="return formata(this, '§.§§.§§§§.§§§§§.§§.§§.§§§', event)"></asp:TextBox>
                        </td>

                    </tr>


                </table>
                <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Red" Text=""></asp:Label>
                <br />
                <table border="0">
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/CImage.ashx" />
                        </td>
                        <td class="auto-style4">&nbsp;<span class="auto-style6">Digite o conteúdo da imagem</span>
                            <br />
                            <asp:TextBox ID="txtimgcode" runat="server" ViewStateMode="Disabled" Width="147px"></asp:TextBox>
                            &nbsp;<br />
                            <br />
                            <asp:Button ID="btPrint" runat="server" OnClick="btPrint_Click" Text="Imprimir o carnê" ToolTip="Gerar segunda via" />
                            <br />
                        </td>
                      
                    </tr>
                </table>
            </asp:Panel>
            <br />

        </div>
        <br />
    </form> 
</body>

    

</html>
