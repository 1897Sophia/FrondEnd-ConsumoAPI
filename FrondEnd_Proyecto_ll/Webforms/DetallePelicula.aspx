<%@ Page Title="Detalles de Película" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetallePelicula.aspx.cs" Inherits="FrondEnd_Proyecto_ll.Webforms.DetallePelicula" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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

    <link rel="StyleSheet" href="../css/DetallePelicula.css" type="text/css" media="screen">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css">

    <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark" style="background-color: #015366; margin: 5px;">

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



    <div class="movie-details">
        <!-- Caja del póster en el lado derecho -->
        <div class="poster-container">
            <asp:Image ID="imgPoster" runat="server" AlternateText="Póster de la película" CssClass="poster-image" />
        </div>
        <!-- Información de la película en el lado izquierdo -->
        <div class="info-container">
            <p class="title">
                <asp:Literal ID="ltTitulo" runat="server" />
            </p>
            <p class="release-date-text">
                Fecha de lanzamiento:
                <asp:Literal ID="ltFecha" runat="server" />
            </p>

            <h2>Reseña:</h2>
            <p class="description">
                <asp:Literal ID="ltDescripcion" runat="server" />
            </p>

            <!-- Involucrados -->
            <div class="involucrados-container">
                <h2>Involucrados</h2>
                <ul>
                    <asp:Repeater ID="InvolucradosRepeater" runat="server">
                        <ItemTemplate>
                            <li>
                                <strong>
                                    <%# Eval("involucrado.nombre") %>  <%# Eval("involucrado.apellidos") %>
                                    <br>
                                    Rol: <%# Eval("involucrado.rol") %><br>
                                </strong>
                                <i class="fab fa-facebook"></i>Facebook: <a <%# Eval("involucrado.facebook") %>><%# Eval("involucrado.facebook") %></a><br>
                                <i class="fab fa-instagram"></i>Instagram: <a <%# Eval("involucrado.instagram") %>><%# Eval("involucrado.instagram") %></a><br>
                                <i class="fab fa-twitter"></i>Twitter: <a <%# Eval("involucrado.twitter") %>><%# Eval("involucrado.twitter") %></a><br>
                                Otros: <%# Eval("involucrado.otros") %>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>

            <!-- Calificaciones -->
            <div class="criticos-container">
                <h2>Calificaciones</h2>
                <ul>
                    <asp:Repeater ID="CriticosRepeater" runat="server">
                        <ItemTemplate>
                            <li>
                                <strong><%# Eval("nombreCritico") %></strong> - Calificación:
                                <asp:Literal runat="server" Text='<%# GetStar(Eval("calificacion")) %>'></asp:Literal>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>

        <!-- Separador entre detalles de la película y comentarios -->
        <div class="separator"></div>
    </div>

    <!-- Comentarios en una caja aparte -->
    <div class="comments-box">

        <h2>Deja un comentario</h2>
        <asp:TextBox ID="txtComentario" runat="server" CssClass="input-Comentario" placeholder="Agrega un comentario"></asp:TextBox>
        <asp:Button ID="btnCrearComentario" runat="server" Text="Agregar Comentario" OnClick="btnCrearComentario_Click" CssClass="btnResponder" />

        <div class="comments-container">
<asp:Repeater ID="ComentariosRepeater" runat="server">
    <ItemTemplate>
        <div class="comment">
            <p>
                <asp:Label ID="lblIdComentario" runat="server" Text='<%# Eval("IdComentario") %>' Visible="false" />
            </p>
            <p><%# Eval("NombreUsuario") %> - <%# Eval("Fecha") %></p>
            <p><%# Eval("ComentarioTexto") %></p>
            <asp:TextBox ID="txtRespuesta" runat="server" placeholder="Responder..."></asp:TextBox>
            <asp:Button ID="btnResponder" runat="server" Text="Responder" OnClick="btnResponder_Click" CommandArgument='<%# Eval("IdComentario") %>' CssClass="btnResponder" />
            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click" CommandArgument='<%# Eval("IdComentario") %>' CssClass="btnEliminar" />

            <asp:Repeater ID="RespuestasRepeater" runat="server" DataSource='<%# Eval("Respuestas") %>'>
                <ItemTemplate>
                    <div class="reply">
                        <p>
                            <asp:Label ID="lblIdComentario2" runat="server" Text='<%# Eval("IdComentario") %>' Visible="false" />
                        </p>
                        <p><%# Eval("NombreUsuario") %> - <%# Eval("Fecha") %></p>
                        <p><%# Eval("ComentarioTexto") %></p>
                        <asp:TextBox ID="txtResponderComentario" runat="server" placeholder="Responder..." />
                        <asp:Button ID="btnResponderRespuesta" runat="server" Text="Responder" OnClick="btnResponderRespuesta_Click" CommandArgument='<%# Eval("IdComentario") %>' CssClass="btnResponder" />
                        <asp:Button ID="btnEliminarRespuesta" runat="server" Text="Eliminar" OnClick="btnEliminarRespuesta_Click" CommandArgument='<%# Eval("IdComentario") %>' CssClass="btnEliminar" />
                    </div>
                    <asp:Repeater ID="RespuestasRespuestasRepeater" runat="server" DataSource='<%# Eval("Respuestas") %>'>
                        <ItemTemplate>
                            <div class="reply">
                                <p>
                                    <asp:Label ID="lblIdComentario3" runat="server" Text='<%# Eval("IdComentario") %>' Visible="false" />
                                </p>
                                <p><%# Eval("NombreUsuario") %> - <%# Eval("Fecha") %></p>
                                <p><%# Eval("ComentarioTexto") %></p>
                                <asp:TextBox ID="txtResponderRespuestaRespuesta" runat="server" placeholder="Responder..." />
                                <asp:Button ID="btnResponderRespuestaRespuesta" runat="server" Text="Responder"  CommandArgument='<%# Eval("IdComentario") %>' CssClass="btnResponder" />
                                <asp:Button ID="btnEliminarRespuestaRespuesta" runat="server" Text="Eliminar" OnClick="btnEliminarRespuestaRespuesta_Click"  CommandArgument='<%# Eval("IdComentario") %>' CssClass="btnEliminar" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                </ItemTemplate>
            </asp:Repeater>
        </div>
    </ItemTemplate>
</asp:Repeater>
        </div>
    </div>
</asp:Content>









