using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrondEnd_Proyecto_ll.Webforms
{
    public partial class DetallePelicula : Page
    {
        string idUsuarioObtenido;
        protected void Page_Load(object sender, EventArgs e)
        {
            string NombreUsuario = Request.QueryString["Nombreusuario"];
            Session["NombreUsuario"] = NombreUsuario;
            string apiUrl = "http://localhost:55513/api/Usuarios/ObtenerIdPorNombre/" + NombreUsuario;

            HttpResponseMessage response = null; 

            using (HttpClient client = new HttpClient())
            {
                response = client.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);

                    if (result.idUsuario != null)
                    {
                        idUsuarioObtenido = result.idUsuario;

                    }
                    else
                    {

                    }
                }
            }
            if (!IsPostBack)
            {

                if (Request.QueryString["token"] != null)
                {
                    string token = Request.QueryString["token"];
                    Session["Token"] = token;
                    Session["TokenCreationTime"] = DateTime.Now;
                }

                if (Session["Token"] != null && Session["TokenCreationTime"] != null)
                {
                    DateTime tokenCreationTime = (DateTime)Session["TokenCreationTime"];
                    DateTime currentTime = DateTime.Now;
                    TimeSpan elapsedTime = currentTime - tokenCreationTime;
                    TimeSpan maxSessionTime = TimeSpan.FromSeconds(300000);

                    if (elapsedTime > maxSessionTime)
                    {
                        Response.Redirect("Login.aspx");
                    }
                    else
                    {
                        Session["TokenCreationTime"] = currentTime;
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
                int idPelicula;
                if (int.TryParse(Request.QueryString["IdPelicula"], out idPelicula))
                {
                    var pelicula = ObtenerPeliculaPorId(idPelicula);
                    var comentariosJerarquicos = ObtenerComentariosJerarquicosDesdeAPI(idPelicula);

                    if (pelicula != null)
                    {
                        imgPoster.ImageUrl = pelicula.poster;
                        ltTitulo.Text = pelicula.titulo;
                        ltDescripcion.Text = pelicula.descripcion;
                        ltFecha.Text = pelicula.fecha;

                        InvolucradosRepeater.DataSource = pelicula.involucrados;
                        InvolucradosRepeater.DataBind();

                        CriticosRepeater.DataSource = pelicula.criticos;
                        CriticosRepeater.DataBind();

                        ComentariosRepeater.DataSource = comentariosJerarquicos;
                        ComentariosRepeater.DataBind();
                    }
                }
            }
        }

        protected void btnCrearComentario_Click(object sender, EventArgs e)
        {

            int idPelicula;
            if (int.TryParse(Request.QueryString["IdPelicula"], out idPelicula))
            {

                int idUsuario = Int32.Parse(idUsuarioObtenido);
                string comentarioTexto = txtComentario.Text;

                if (string.IsNullOrWhiteSpace(comentarioTexto))
                {
                    return;
                }

                Models.Comentarios nuevoComentario = new Models.Comentarios
                {
                    IdUsuario = idUsuario,
                    IdPelicula = idPelicula,
                    ComentarioTexto = comentarioTexto,
                    IdComentarioPadre = null,
                };
                dynamic response = CrearComentarioEnAPI(nuevoComentario);

                if (response != null && response.StatusCode == HttpStatusCode.Created)
                {
                    txtComentario.Text = string.Empty;
                }
                else
                {

                }
                var comentariosJerarquicos = ObtenerComentariosJerarquicosDesdeAPI(idPelicula);

                ComentariosRepeater.DataSource = comentariosJerarquicos;
                ComentariosRepeater.DataBind();
            }
        }



        private dynamic CrearComentarioEnAPI(Models.Comentarios nuevoComentario)
        {
            using (var httpClient = new HttpClient())
            {
                var apiUrl = "http://localhost:55513/api/Comentario/CrearComentario";

                var json = JsonConvert.SerializeObject(nuevoComentario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = httpClient.PostAsync(apiUrl, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return new { StatusCode = HttpStatusCode.Created };
                }
                else
                {
                    return new { StatusCode = response.StatusCode };
                }
            }
        }
        protected void btnResponder_Click(object sender, EventArgs e)
        {
            Button btnResponder = (Button)sender;
            RepeaterItem item = (RepeaterItem)btnResponder.NamingContainer;
            TextBox txtRespuesta = (TextBox)item.FindControl("txtRespuesta");


            int idPelicula;

            if (int.TryParse(Request.QueryString["IdPelicula"], out idPelicula))
            {
                if (string.IsNullOrWhiteSpace(txtRespuesta.Text))
                {
                    return;
                }
                int idComentarioPadre = ObtenerIdComentarioPadre(btnResponder);
                Models.Comentarios nuevoComentario = new Models.Comentarios
                {
                    IdUsuario = Int32.Parse(idUsuarioObtenido),
                    IdPelicula = idPelicula,
                    ComentarioTexto = txtRespuesta.Text,
                    IdComentarioPadre = idComentarioPadre
                };
                dynamic resultado = CrearComentarioEnAPI(nuevoComentario);

                if (resultado.StatusCode == HttpStatusCode.Created)
                {

                    MostrarMensaje("Comentario creado con éxito.");
                    LimpiarCampos(txtRespuesta);
                    ActualizarComentarios();

                }
                else
                {
                    MostrarMensaje("Se produjo un error al crear el comentario.");
                }
            }
        }
        protected void btnResponderRespuesta_Click(object sender, EventArgs e)
        {
            Button btnResponderRespuesta = (Button)sender;
            RepeaterItem item = (RepeaterItem)btnResponderRespuesta.NamingContainer;
            TextBox txtRespuesta = (TextBox)item.FindControl("txtResponderComentario");

            int idPelicula;

            if (int.TryParse(Request.QueryString["IdPelicula"], out idPelicula))
            {
                if (string.IsNullOrWhiteSpace(txtRespuesta.Text))
                {
                    return;
                }
                int idComentarioPadre = ObtenerIdComentarioPadre2(btnResponderRespuesta);
                Models.Comentarios nuevoComentario = new Models.Comentarios
                {
                    IdUsuario = Int32.Parse(idUsuarioObtenido),
                    IdPelicula = idPelicula,
                    ComentarioTexto = txtRespuesta.Text,
                    IdComentarioPadre = idComentarioPadre
                };
                dynamic resultado = CrearComentarioEnAPI(nuevoComentario);

                if (resultado.StatusCode == HttpStatusCode.Created)
                {

                    MostrarMensaje("Comentario creado con éxito.");
                    LimpiarCampos(txtRespuesta);
                    ActualizarComentarios();

                }
                else
                {
                    MostrarMensaje("Se produjo un error al crear el comentario.");
                }
            }
        }
        private int ObtenerIdComentarioPadre(Button btnResponder)
        {
            RepeaterItem item = (RepeaterItem)btnResponder.NamingContainer;
            Label lblIdComentario = (Label)item.FindControl("lblIdComentario");
            int idComentarioPadre;

            if (int.TryParse(lblIdComentario.Text, out idComentarioPadre))
            {
                return idComentarioPadre;
            }

            return 0;
        }
        private int ObtenerIdComentarioPadre2(Button btnResponder)
        {
            RepeaterItem item = (RepeaterItem)btnResponder.NamingContainer;
            Label lblIdComentario2 = (Label)item.FindControl("lblIdComentario2");
            int idComentarioPadre;

            if (int.TryParse(lblIdComentario2.Text, out idComentarioPadre))
            {
                return idComentarioPadre;
            }

            return 0;
        }
        private void LimpiarCampos(TextBox txtRespuesta)
        {
            txtRespuesta.Text = string.Empty;
        }

        private void ActualizarComentarios()
        {
            int idPelicula;

            if (int.TryParse(Request.QueryString["IdPelicula"], out idPelicula))
            {
                var comentariosJerarquicos = ObtenerComentariosJerarquicosDesdeAPI(idPelicula);

                ComentariosRepeater.DataSource = comentariosJerarquicos;
                ComentariosRepeater.DataBind();
            }
        }

        private void MostrarMensaje(string mensaje)
        {
            string script = $"Swal.fire('{mensaje}')";
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", script, true);
        }


        protected void btnEliminar_Click(object sender, EventArgs e)
        {

            int idPeliculaObtenido;
            if (int.TryParse(Request.QueryString["IdPelicula"], out idPeliculaObtenido))
            {

                int idPelicula = idPeliculaObtenido;
                int idUsuario = Int32.Parse(idUsuarioObtenido);
                Button btnEliminar = (Button)sender;
                int idComentarioAEliminar;

                if (int.TryParse(btnEliminar.CommandArgument, out idComentarioAEliminar))
                {
                    bool eliminacionExitosa = EliminarComentarioEnAPI(idUsuario, idComentarioAEliminar);

                    if (eliminacionExitosa)
                    {
                        var comentariosJerarquicos = ObtenerComentariosJerarquicosDesdeAPI(idPelicula);

                        ComentariosRepeater.DataSource = comentariosJerarquicos;
                        ComentariosRepeater.DataBind();
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
        }


        protected void btnEliminarRespuesta_Click(object sender, EventArgs e)
        {

            int idPeliculaObtenido;
            if (int.TryParse(Request.QueryString["IdPelicula"], out idPeliculaObtenido))
            {


                int idPelicula = idPeliculaObtenido;
                int idUsuario = Int32.Parse(idUsuarioObtenido);
                Button btnEliminar = (Button)sender;
                int idComentarioAEliminar;

                if (int.TryParse(btnEliminar.CommandArgument, out idComentarioAEliminar))
                {
                    bool eliminacionExitosa = EliminarComentarioEnAPI(idUsuario, idComentarioAEliminar);

                    if (eliminacionExitosa)
                    {
                        var comentariosJerarquicos = ObtenerComentariosJerarquicosDesdeAPI(idPelicula);

                        ComentariosRepeater.DataSource = comentariosJerarquicos;
                        ComentariosRepeater.DataBind();
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
        }

        protected void btnEliminarRespuestaRespuesta_Click(object sender, EventArgs e)
        {
            int idPeliculaObtenido;
            if (int.TryParse(Request.QueryString["IdPelicula"], out idPeliculaObtenido))
            {
                int idPelicula = idPeliculaObtenido;
                int idUsuario = Int32.Parse(idUsuarioObtenido);
                Button btnEliminar = (Button)sender;
                int idComentarioAEliminar;

                if (int.TryParse(btnEliminar.CommandArgument, out idComentarioAEliminar))
                {
                    bool eliminacionExitosa = EliminarComentarioEnAPI(idUsuario, idComentarioAEliminar);

                    if (eliminacionExitosa)
                    {
                        var comentariosJerarquicos = ObtenerComentariosJerarquicosDesdeAPI(idPelicula);

                        ComentariosRepeater.DataSource = comentariosJerarquicos;
                        ComentariosRepeater.DataBind();
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
        }

        private bool EliminarComentarioEnAPI(int idUsuario, int idComentario)
        {
            using (var httpClient = new HttpClient())
            {
                var apiUrl = $"http://localhost:55513/api/Comentario/EliminarComentario/{idUsuario}/{idComentario}";

                var response = httpClient.DeleteAsync(apiUrl).Result;

                return response.IsSuccessStatusCode;
            }
        }

        private dynamic ObtenerPeliculaPorId(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string apiUrl = $"http://localhost:55513/api/Peliculas/ObtenerPeliculaPorId/{id}";
                var response = httpClient.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var comentariosJson = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject(comentariosJson);
                }
                return null;
            }
        }

        private dynamic ObtenerComentariosJerarquicosDesdeAPI(int idPelicula)
        {
            using (var httpClient = new HttpClient())
            {
                var apiUrl = "http://localhost:55513/api/Comentario/ObtenerComentariosConRespuestas?idPelicula=" + idPelicula;
                var response = httpClient.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var comentariosJson = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject(comentariosJson);
                }
                return null;
            }
        }


        private dynamic ObtenerPeliculaNombreAPI(string NombrePelicula)
        {
            using (var httpClient = new HttpClient())
            {
                var apiUrl = "http://localhost:55513/api/Peliculas/BuscarPeliculaPorNombre?nombrePelicula=" + NombrePelicula;
                var response = httpClient.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var comentariosJson = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject(comentariosJson);
                }
                return null;
            }
        }

        protected string GetStar(object calificacion)
        {
            int rating = Convert.ToInt32(calificacion);

            string starHtml = "";
            for (int i = 0; i < rating; i++)
            {
                starHtml += $"<img src='../Imagen/star.png' alt='Star' width='20' />";
            }

            for (int i = rating; i < 5; i++)
            {
                starHtml += "<img src='../Imagen/starempty.png' alt='Star' width='20' />";
            }
            return starHtml;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombrePelicula = txtBuscarPeli.Text.Trim();

            if (!string.IsNullOrEmpty(nombrePelicula))
            {
                Response.Redirect($"Busqueda_Nombre.aspx?nombrePelicula={Server.UrlEncode(nombrePelicula)}");
            }
        }
    }
}
