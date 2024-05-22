<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registrar.aspx.cs" Inherits="FrondEnd_Proyecto_ll.Webforms.Registrar" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/styles.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div class="login-container">
            <asp:Panel ID="registerForm" runat="server" style="display: block">
                <h2>Registro</h2>
                <div>
                    <div class="input-container">
                        <asp:Label ID="lblNombreUsuario" runat="server" Text="Nombre de Usuario"></asp:Label>
                        <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
                    </div>
                    <div class="input-container">
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
                    </div>
                    <div class="input-container">
                        <asp:Label ID="lblApellidos" runat="server" Text="Apellidos"></asp:Label>
                        <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
                    </div>
                    <div class="input-container">
                        <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Required="true" TextMode="Email"></asp:TextBox>
                    </div>
                    <div class="input-container">
                        <asp:Label ID="lblContrasena" runat="server" Text="Contraseña"></asp:Label>
                        <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="form-control" Required="true"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="btn-register" OnClick="btnRegistrar_Click"/>
                    <a href="Login.aspx" class="toggle-link">¿Ya tienes una cuenta? Inicia sesión</a>
                </div>
            </asp:Panel>
            <asp:Label ID="lblRegistroEstado" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>