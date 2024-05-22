using FrondEnd_Proyecto_ll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace FrondEnd_Proyecto_ll.Webforms
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsername.Text;
            string clave = txtPassword.Text;

            int intentosFallidos = 0;

            if (Session["IntentosFallidos"] != null)
            {
                intentosFallidos = (int)Session["IntentosFallidos"];
            }

            if (intentosFallidos >= 2)
            {
                ActivarInactivarUsuario(txtUsername.Text, false);
                Session["IntentosFallidos"] = 0;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "Swal.fire('Usuario y/o contraseña incorrectos', ' ', 'warning');", true);
            }
            else
            {
                bool? usuarioActivo = await ObtenerEstadoUsuarioAsync(usuario);                

                if (usuarioActivo.HasValue)
                {
                    if (usuarioActivo.Value)
                    {
                        var requestParams = new Dictionary<string, string>
                        {
                            { "Usuario", usuario },
                            { "Clave", clave }
                        };
                        string baseUrl = "http://localhost:55513/api/Usuarios/login";

                        using (var client = new HttpClient())
                        {
                            var content = new FormUrlEncodedContent(requestParams);
                            var response = await client.PostAsync(baseUrl, content);

                            if (response.IsSuccessStatusCode)
                            {
                                Session["IntentosFallidos"] = 0;
                                var token = await response.Content.ReadAsStringAsync();
                                Session["Token"] = token;


                                var Nombreusuario = txtUsername.Text;
                                Session["Nombreusuario"] = Nombreusuario;

                                Response.Redirect("Default.aspx?token=" + token + "&Nombreusuario=" + Nombreusuario);
                            }
                            else
                            {
                                intentosFallidos++;
                                Session["IntentosFallidos"] = intentosFallidos;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "Swal.fire('Usuario y/o contraseña incorrectos', ' ', 'warning');", true);
                            }
                        }
                    }
                    else
                    {
                        ActivarInactivarUsuario(usuario, false);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "Swal.fire('Error', 'Su usuario se encuentra inactivo, por favor comuníquese con el administrador', 'warning');", true);
                        Session["IntentosFallidos"] = 0;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "Swal.fire('Error', 'Usuario y/o contraseña incorrectos', 'error');", true);
                }
            }
        }
        private async Task<bool> ActivarInactivarUsuario(string Usuario, bool activar)
        {
            var request = new
            {
                Usuario = Usuario,
                Activar = activar
            };

            string baseUrl = "http://localhost:55513/api/Usuarios/activar_inactivar_nombre";

            using (var client = new HttpClient())
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(baseUrl, content);

                return response.IsSuccessStatusCode;
            }
        }
        protected async Task<bool?> ObtenerEstadoUsuarioAsync(string username)
        {
            string baseUrl = "http://localhost:55513/api/Usuarios/estado_usuario/" + username;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(baseUrl);

                if (response.IsSuccessStatusCode)
                {
                    var estadoUsuario = await response.Content.ReadAsAsync<EstadoUsuarioResponse>();

                    return estadoUsuario.Activo == 1;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}