using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Articulo
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }


        [DisplayName("Tipo de Marca")]
        public Marca TipoMarca { get; set; }

        [DisplayName("Tipo de Categoria")]
        public Categoria TipoCategoria { get; set; }

        [DisplayName("Tipo de Url Imagen")]
        public string url { get; set; }
        public decimal Precio { get; set; }
    }
}
