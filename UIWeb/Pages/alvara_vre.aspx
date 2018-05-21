<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="alvara_vre.aspx.cs" Inherits="UIWeb.alvara_vre" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        
        .auto-style2 {
            width: 217px;
        }

        #form1 {
            height: 141px;
        }
        .auto-style3 {
            width: 333px;
        }
    </style>




</head>
<body style="height: 643px; font-family:Tahoma; font-size:11px;">
    <form id="form1" runat="server">
        <div style="color: #3a8dcc;">
            <asp:Label ID="Label3" runat="server" ForeColor="#000066" Text="Sistema Tributário Municipal"></asp:Label>
            &nbsp;<br />
            
            Emissão de alvará de funcionamento para empresas cadastradas pelo Via Rápida Empresa<br />
            <br />
            <asp:Panel ID="Panel2" runat="server" ForeColor="Black" BorderColor="#3399FF" BorderStyle="Solid" BorderWidth="1px" Width="675px">
                <table style="width: 100%;">
                   
                    
                    <tr>
                        <td class="auto-style3">&nbsp;&nbsp;
                            <asp:Label ID="lblCod" runat="server" Text="Inscrição Municipal.:"></asp:Label>
                            &nbsp;
                            <asp:TextBox ID="txtCod" runat="server" BorderColor="#3399FF" BorderStyle="Solid" BorderWidth="1px" MaxLength="6" Width="70px" ></asp:TextBox>
                            &nbsp; </td>
                        <td>
                            <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Red" Text=""></asp:Label>
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
                            <asp:TextBox ID="txtimgcode" runat="server" ViewStateMode="Disabled" Width="147px"></asp:TextBox>
                            &nbsp;</td>
                       <td><asp:Button ID="btPrint" runat="server" OnClick="btPrint_Click" Text="Imprimir" ToolTip="Gerar segunda via"  /></td>
                    </tr>
                </table>
            </asp:Panel>
            <br />

        </div>
        <br />
    </form> 
</body>

    

</html>
