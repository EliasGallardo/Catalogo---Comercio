using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace presentacion
{
    public partial class frmAgregar : Form
    {
        private Articulo art = null;


        public frmAgregar()
        {
            InitializeComponent();
        }

        public frmAgregar(Articulo articulo)
        {
            InitializeComponent();
            this.art = articulo;
            Text = "Modificar Articulo Selecionado";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            CatalogoNegocio negocio = new CatalogoNegocio();

            try
            {
                if (art != null)
                {

                    art.Codigo = txtCodigo.Text;
                    art.Nombre = txtNombre.Text;
                    art.Descripcion = txtDescripcion.Text;
                    art.TipoMarca = (Marca)cbMarca.SelectedItem;
                    art.TipoCategoria = (Categoria)cbCategoria.SelectedItem;
                    art.url = txtImagen.Text;
                    art.Precio= decimal.Parse(txtPreio.Text);

                    if (art.Id !=0)
                    {
                        negocio.modificar(art);
                        MessageBox.Show("MODIFICADO EXITOSAMENTE");
                    }
                    else
                    {
                        negocio.agregar(art);
                        MessageBox.Show("DATOS AGREGADOS CORRECTAMENTE");
                    }

                }

                Close();
            }
            catch (Exception)
            {

                MessageBox.Show("ERROR, VERIFIQUE LOS DATOS");
            }
        }

        private void frmAgregar_Load(object sender, EventArgs e)
        {
            CatalogoNegocio negocio = new CatalogoNegocio();
            


            try
            {
                cbMarca.DataSource = negocio.listarMarca();
                cbMarca.ValueMember = "Id";
                cbMarca.DisplayMember = "Descripcion";

                cbCategoria.DataSource = negocio.listarCategoria();
                cbCategoria.ValueMember = "Id";
                cbCategoria.DisplayMember = "Descripcion";


                if (art != null)
                {
                    
                    txtCodigo.Text = art.Codigo;
                    txtNombre.Text = art.Nombre;
                    txtDescripcion.Text = art.Descripcion;
                    txtImagen.Text = art.url;
                    cargarImagen(art.url);
                    txtPreio.Text = art.Precio.ToString();
                    cbMarca.SelectedValue = art.TipoMarca.Id;
                    cbCategoria.SelectedValue = art.TipoCategoria.Id;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            } 
                

            

            
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                IMAGEN.Load(imagen);
            }
            catch (Exception)
            {

                IMAGEN.Load("https://fotografiaperfecta.files.wordpress.com/2012/12/no-hay-imagen-disponible2.jpg");
            }
        }

        private void txtImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagen.Text);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtImagen.Clear();
            txtPreio.Clear();
            cbMarca.SelectedIndex=0;
            cbCategoria.SelectedIndex=0;    
        }
    }
}
