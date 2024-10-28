using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace academica {
    public partial class Form1 : Form {
        db_conexion objConexion = new db_conexion();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        public int posicion = 0;
        String accion = "nuevo";

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            obtenerDatos();
        }
        private void obtenerDatos() {
            ds = objConexion.obtenerDatos();
            dt = ds.Tables["Peliculas"];
            dt.PrimaryKey = new DataColumn[] { dt.Columns["idPeliculas"] };
            grdDatos.DataSource = dt;

            mostrarDatos();
        }
        private void mostrarDatos() {
            txtTitulo.Text = dt.Rows[posicion].ItemArray[1].ToString();
            txtDirector.Text = dt.Rows[posicion].ItemArray[2].ToString();
            txtSinopsis.Text = dt.Rows[posicion].ItemArray[3].ToString();
            txtDuracion.Text = dt.Rows[posicion].ItemArray[4].ToString();
            txtClasificacion.Text = dt.Rows[posicion].ItemArray[5].ToString();
            txtGenero.Text = dt.Rows[posicion].ItemArray[6].ToString();
            lblregistros.Text = (posicion + 1) + " de " + dt.Rows.Count;
            activarDesBtnNevegacion(true);
        }
        private void activarDesBtnNevegacion(Boolean estado) {
            bool est = (posicion > 0 && posicion <= dt.Rows.Count - 1) || !estado;
            btnAnterior.Enabled = est;
            btnPrimero.Enabled = est;

            btnSiguiente.Enabled = estado;
            btnUltimo.Enabled = estado;
        }
        private void habDesControles(Boolean estado) {

            grbNavegacion.Enabled = !estado;
            btnEliminar.Enabled = !estado;
        }
        private void btnSiguiente_Click(object sender, EventArgs e) {
            if(posicion<dt.Rows.Count - 1) { 
                posicion+= 1;
                mostrarDatos();
            } else {
                activarDesBtnNevegacion(false);
            }
        }
        private void btnUltimo_Click(object sender, EventArgs e) {
            posicion = dt.Rows.Count - 1;
            mostrarDatos();
        }

        private void btnPrimero_Click(object sender, EventArgs e) {
            posicion = 0;
            mostrarDatos();
        }

        private void btnAnterior_Click(object sender, EventArgs e) {
            if(posicion > 0) {
                posicion -= 1;
                mostrarDatos();
            } else {
                activarDesBtnNevegacion(true);
            }
        }
        private void limpiarCajas() {
            txtTitulo.Text = "";
            txtDirector.Text = "";
            txtSinopsis.Text = "";
            txtDuracion.Text = "";
            txtClasificacion.Text = "";
            txtGenero.Text = "";
        }   
        private void btnNuevo_Click(object sender, EventArgs e) {
            if( btnNuevo.Text=="Nuevo") {
                btnNuevo.Text = "Guardar";
                btnModificar.Text = "Cancelar";
                limpiarCajas();
                accion = "nuevo";
                habDesControles(true);
            } else {//guardar
                String[] datos = {
                    accion,
                    dt.Rows[posicion].ItemArray[0].ToString(), //idAlumno
                    txtTitulo.Text,
                    txtDirector.Text,
                    txtSinopsis.Text,
                    txtDuracion.Text,
                    txtClasificacion.Text,
                    txtGenero.Text
                };
                String response = objConexion.administrarPeliculas(datos);
                if (response != "1") {
                    MessageBox.Show("Error: " + response, "Registrando datos de pelicula", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    obtenerDatos();
                    habDesControles(false);
                    btnNuevo.Text = "Nuevo";
                    btnModificar.Text = "Modificar";
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e) {
            if (btnModificar.Text == "Modificar") {
                btnModificar.Text = "Cancelar";
                btnNuevo.Text = "Guardar";
                accion = "modificar";
                habDesControles(true);
            } else {//cancelar

                mostrarDatos();
                habDesControles(false);
                btnModificar.Text = "Modificar";
                btnNuevo.Text = "Nuevo";
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Esta seguro de eliminar a: " + txtDirector.Text, "Eliminando Pelicula",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                String[] datos = {
                    "eliminar", dt.Rows[posicion].ItemArray[0].ToString(), //idAlumno
                };
                String response = objConexion.administrarPeliculas(datos);
                if (response != "1") {
                    MessageBox.Show("Error: " + response, "Eliminando datos de peliculas", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                } else {
                    obtenerDatos();
                }
            }
        }
        private void filtrarDatos(String buscar) {
            DataView dv = dt.DefaultView;
            dv.RowFilter = "Titulo like '%"+ buscar +"%' OR Director like '%"+ buscar +"%'";
            grdDatos.DataSource = dv;
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e) {
            filtrarDatos(txtBuscar.Text);
            seleccionarDatos();
        }
        private void seleccionarDatos() {
            posicion = dt.Rows.IndexOf(dt.Rows.Find(grdDatos.CurrentRow.Cells["idPeliculas"].Value.ToString()));
            mostrarDatos();
        }

        private void grdDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtDireccion_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblSinopsis_Click(object sender, EventArgs e)
        {

        }
    }
}
