using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrondEnd_Proyecto_ll.Webforms
{
    public partial class Registrar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void btnRegistrar_Click(object sender, EventArgs e)
        {
            string usuario = txtNombreUsuario.Text;
            string nombre = txtNombre.Text;
            string apellidos = txtApellidos.Text;
            string correo = txtEmail.Text;
            string clave = txtContrasena.Text;

            // Crear una solicitud con los datos del nuevo usuario
            var requestParams = new Dictionary<string, string>
            {
                { "Usuario", usuario },
                { "Nombre", nombre },
                { "Apellidos", apellidos },
                { "Correo", correo },
                { "Clave", clave }
            };

            string baseUrl = "http://localhost:55513/api/Usuarios/InsertarUsuario";

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(requestParams);
                var response = await client.PostAsync(baseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "Swal.fire('Usuario creado con éxito', ' ', 'success');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "Swal.fire('“Ha ocurrido un error intente de nuevo',' ', 'warning');", true);
                }
            }
        }
    }
}