<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FrondEnd_Proyecto_ll.Webforms.Login" async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/styles.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Iniciar sesión</title>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div class="login-container">
            <asp:Panel ID="loginForm" runat="server" style="display: block">
                <h2>Iniciar Sesión</h2>
                <div>
                    <div class="input-container">
                        <asp:Label ID="lblUsername" runat="server" Text="Usuario"></asp:Label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
                    </div>
                    <div class="input-container">
                        <asp:Label ID="lblPassword" runat="server" Text="Contraseña"></asp:Label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" Required="true"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" CssClass="btn-login" OnClick="btnLogin_Click" />
                    <a href="Registrar.aspx" class="toggle-link">Crear cuenta</a>
                </div>
            </asp:Panel>
            <asp:Label ID="lblestado" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
