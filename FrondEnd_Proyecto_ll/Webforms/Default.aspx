<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FrondEnd_Proyecto_ll.Webforms.Default" Async="true" Title="Principal"%>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <html>
<head>
                  <LINK REL=StyleSheet HREF="../css/Peliculas.css" TYPE="text/css" MEDIA=screen>


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
    <title></title>
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
        <a class="card" href='<%# "DetallePelicula.aspx?IdPelicula=" + Eval("idPelicula") %>'>
           
            <div class="front" style='<%# "background-image: url(" + Eval("Poster") + ");" %>'>
                <div class="details">
                    <h1><%# Eval("nombrePelicula") %></h1> 

                    <h2>Fecha de lanzamiento: <%# Eval("fecha") %></h2>

                    <p class="desc">Reseña: <%# Eval("Descripcion") %></p>
                    <div class="cast">
                       <br> <p class="desc"> Involucrados:<br> <%# Eval("involucrados")%><br></p> <br>
                    </div>                                                       
                </div>
            </div>
        </a>
        <div class="link">
             <asp:LinkButton ID="lnkVerDetalle" runat="server" CommandName="VerDetalle" CommandArgument='<%# Eval("idPelicula") %>' Text='<%# Eval("nombrePelicula") %>' />
        </div>
    </ItemTemplate>
</asp:Repeater>
         </div>
    </body>
    </html>
</asp:Content>