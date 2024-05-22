using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace FrondEnd_Proyecto_ll.Webforms
{
    public partial class Busqueda_Nombre : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string NombreUsuario = Request.QueryString["Nombreusuario"];
                Session["NombreUsuario"] = NombreUsuario;
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

                string nombre = Request.QueryString["nombrePelicula"];

                var data = ObtenerPeliculaNombreAPI(nombre);

                Repeater1.DataSource = data;
                Repeater1.DataBind();

            }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "VerDetalle")
            {
                int idPelicula = Convert.ToInt32(e.CommandArgument);

                string NombreUsuario = Request.QueryString["Nombreusuario"];
                Session["Nombreusuario"] = NombreUsuario;
                Response.Redirect($"DetallePelicula.aspx?Nombreusuario={NombreUsuario}&IdPelicula={idPelicula}");
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