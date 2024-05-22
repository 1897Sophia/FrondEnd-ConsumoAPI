using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrondEnd_Proyecto_ll.Models
{
    public class Peliculas
    {
        public int idPelicula { get; set; }
        public string titulo { get; set; }
        public string Descripcion { get; set; }
        public string Poster { get; set; }
        public DateTime fecha { get; set; }

        public string nombrePelicula { get; set; }

        public string involucrados { get; set; }

        public string criticos { get; set; }

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }

         

    }
}