using FrondEnd_Proyecto_ll.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrondEnd_Proyecto_ll.Webforms
{
    public partial class Default : System.Web.UI.Page
    {
        
        protected async void Page_Load(object sender, EventArgs e)
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

                List<Peliculas> data = await ObtenerDatosDesdeAPI();

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
                Response.Redirect($"DetallePelicula.aspx?Nombreusuario={NombreUsuario}&IdPelicula={idPelicula}");
            }
        }



        private async Task<List<Peliculas>> ObtenerDatosDesdeAPI()
        {
            using (var client = new HttpClient())
            {
                string apiUrl = "http://localhost:55513/api/Peliculas/ObtenerPeliculasRecientes";
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var peliculas = await response.Content.ReadAsAsync<List<Peliculas>>();
                    return peliculas;
                }

                return null;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombrePelicula = txtBuscarPeli.Text.Trim();
            string NombreUsuario = Request.QueryString["Nombreusuario"];
            Session["NombreUsuario"] = NombreUsuario;
            if (!string.IsNullOrEmpty(nombrePelicula))
            {
                Response.Redirect($"Busqueda_Nombre.aspx?nombrePelicula={Server.UrlEncode(nombrePelicula)}&Nombreusuario={NombreUsuario}");
            }
        }
    }
}
