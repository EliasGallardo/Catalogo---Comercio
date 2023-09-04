using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;


namespace presentacion
{
    public partial class frmPrincipal : Form
    {
        private List<Articulo> listaArticulo;


        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            try
            {
                frmAgregar frmAgregar = new frmAgregar();
                frmAgregar.ShowDialog();
                cargarGrilla();
            }
            catch (Exception)
            {

                MessageBox.Show("ERROR AL CARGAR EL FORMULARIO");
                Close();
            }
            
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
           
            cargarGrilla();

           
            cbCampo.Items.Add("CODIGO");
            cbCampo.Items.Add("NOMBRE");
            cbCampo.Items.Add("MARCA");
            cbCampo.Items.Add("CATEGORIA");
            cbCampo.Items.Add("PRECIO");
            cbCampo.SelectedIndex = 0;
            cbCriterio.SelectedIndex = 0;

        }

        private void cargarGrilla()
        {
            CatalogoNegocio negocio = new CatalogoNegocio();

            listaArticulo = negocio.Listar();
            GRILLA.DataSource = listaArticulo;
            GRILLA.Columns[6].Visible = false;


        }

        private void GRILLA_SelectionChanged(object sender, EventArgs e)
        {
            Articulo seleccioando = (Articulo)GRILLA.CurrentRow.DataBoundItem;

            try
            {
                imagen.Load(seleccioando.url);
            }
            catch (Exception)
            {

                imagen.Load("https://fotografiaperfecta.files.wordpress.com/2012/12/no-hay-imagen-disponible2.jpg");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (GRILLA.Rows.Count > 0)
            {
                CatalogoNegocio negocio = new CatalogoNegocio();
                Articulo seleccionado;
                seleccionado = (Articulo)GRILLA.CurrentRow.DataBoundItem;

                try
                {
                    DialogResult respuesta = MessageBox.Show("¿ DE VERDAD QUERES ELIMINARLO ?", "ELIMINANDO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (respuesta == DialogResult.Yes) 
                    {
                        negocio.eliminar(seleccionado);
                        cargarGrilla();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (GRILLA.Rows.Count > 0)
            {
                Articulo seleccionado;
                seleccionado = (Articulo)GRILLA.CurrentRow.DataBoundItem;

                frmAgregar frmAgregar = new frmAgregar(seleccionado);
                frmAgregar.ShowDialog();
                cargarGrilla();
            }
            else
            {
                MessageBox.Show("NO HAY DATOS SELECCIONADOS");
            }
        }

        private void cbCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbCampo.SelectedItem.ToString();

            if (opcion == "CODIGO" ||  opcion == "PRECIO")
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("MAYOR A");
                cbCriterio.Items.Add("MENOR A");
                cbCriterio.Items.Add("IGUAL A");
                cbCriterio.SelectedIndex = 0;
            }
            else if (opcion == "NOMBRE" || opcion == "MARCA" || opcion == "CATEGORIA")
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("COMIENZA CON");
                cbCriterio.Items.Add("TERMINA CON");
                cbCriterio.Items.Add("CONTIENE");
                cbCriterio.SelectedIndex = 0;
            }  
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CatalogoNegocio negocio = new CatalogoNegocio();

            try
            {
                string campo = cbCampo.SelectedItem.ToString();
                string criterio = cbCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;

                GRILLA.DataSource = negocio.filtrar(campo, criterio,filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            

        }
    }
}
