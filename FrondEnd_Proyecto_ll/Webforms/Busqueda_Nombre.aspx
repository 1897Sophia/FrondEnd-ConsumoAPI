<%@ Page Title="Detalles de Película" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Busqueda_Nombre.aspx.cs" Inherits="FrondEnd_Proyecto_ll.Webforms.Busqueda_Nombre" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!DOCTYPE html>
    <html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <script type="text/javascript">
            var maxInactivityTime = 300000;

            var tokenCreationTime = '<%= Session["TokenCreationTime"] != null ? Session["TokenCreationTime"] : DateTime.MinValue %>';
            var lastInteractionTime = new Date(tokenCreationTime);

            function checkInactivity() {
                var currentTime = new Date();
                var elapsedTime = currentTime - lastInteractionTime;

                if (elapsedTime > maxInactivityTime) {
                    window.location.href = 'Login.aspx';
                }
            }
            setInterval(function () {
                checkInactivity();
            }, maxInactivityTime);
        </script>



        <title>Películas</title>
        <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;700&display=swap" rel="stylesheet">
        <LINK REL=StyleSheet HREF="../css/Peliculas.css" TYPE="text/css" MEDIA=screen>
        <style>
                body{
                background-color: #033540;
                }
        </style>
    </head>
    <body>

    <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark" style="background-color:#015366; margin: 5px;">

            <div class="container">
                <a class="navbar-brand" runat="server">Peliculas SGW</a>
                <button type="button" class="navbar-toggler" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" title="Alternar navegación" aria-controls="navbarSupportedContent"
                    aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <a href='Default.aspx?NombreUsuario=<%= Session["NombreUsuario"] %>'>Inicio</a>
                    </ul>

            <form class="form-inline my-2 my-lg-0">
                               
                <asp:TextBox ID="txtBuscarPeli" runat="server" class="form-control mr-sm-2" type="search" placeholder="Buscar..." aria-label="Buscar"></asp:TextBox>
                <asp:Button ID="btnBuscar" class="btn btn-outline-light my-2 my-sm-0" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
            </form>
        </div>
    </div>
</nav>


        <div id="spinner"></div>
        <div class="wrapper">
    <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
        <ItemTemplate>
            <div class="card">
                <asp:LinkButton ID="lnkVerDetalle" runat="server" CommandName="VerDetalle" CommandArgument='<%# Eval("idPelicula") %>' />
                <div class="front" style='<%# "background-image: url(" + Eval("poster") + ");" %>'>
                    <div class="details">
<%--                        <h1><%# Eval("nombre") %></h1>--%>
                        <a href='<%# "DetallePelicula.aspx?IdPelicula=" + Eval("idPelicula") + "&Nombreusuario=" + Session["Nombreusuario"] %>'>
                             "<%# Eval("nombre") %>"
                        </a>
                        <h2>Fecha de lanzamiento: <%# Eval("fecha") %></h2>
                        <div class="rating">
                            <%--<span><%# Eval("Calificacion") %>/5</span>--%>
                        </div>
                        <p class="desc">Reseña: <%# Eval("descripcion") %></p>
                        <div class="cast">
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

        </div>
    </body>
    </html>
</asp:Content>


