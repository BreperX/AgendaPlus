using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormAnimation;

namespace Modern_Dashboard_Design
{
    public partial class frmAnalytics : Form
    {
        SqlConnection conexion = new SqlConnection("server=DESKTOP-QRMARBH\\SQLEXPRESS; database=ProductosClientes;integrated security=true;");

        public frmAnalytics()
        {
            InitializeComponent();
            textBox2.MaxLength = 20;
            textBox3.MaxLength = 20;
            textBox4.MaxLength = 3;
            textBox1.MaxLength = 15;
            textBox5.MaxLength = 50;
            textBox6.MaxLength = 15;
        }

        public void llenar_tabla()
        {
            string consulta = "select * from Clientes"; // Cambio de 'Cliente' a 'Clientes'
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            dgvRegistro.DataSource = dt;
        }

        public void limpiar()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox1.Focus();
        }

        private void frmAnalytics_Load(object sender, EventArgs e)
        {
            llenar_tabla();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Verificar si los campos requeridos están vacíos
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Error");
                return;
            }

            try
            {
                conexion.Open();

                // Verificar si ya existe un registro con la misma Identificacion
                string identificacion = textBox1.Text;
                string consultaVerificar = "SELECT COUNT(*) FROM Clientes WHERE Cliente_ID = @Cliente_ID"; // Cambio de 'Identificacion' a 'Cliente_ID'

                SqlCommand comandoVerificar = new SqlCommand(consultaVerificar, conexion);
                comandoVerificar.Parameters.AddWithValue("@Cliente_ID", identificacion); // Cambio de '@Identificacion' a '@Cliente_ID'

                int count = (int)comandoVerificar.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Ya existe un registro con la misma Identificacion.", "Error");
                    return;
                }

                // Si no existe, proceder con la inserción
                string consultaInsertar = "INSERT INTO Clientes (Cliente_ID, Nombre, Apellido, Edad, Correo, Celular) " +
                                         "VALUES (@Cliente_ID, @Nombre, @Apellido, @Edad, @Correo, @Celular)";

                SqlCommand comandoInsertar = new SqlCommand(consultaInsertar, conexion);
                comandoInsertar.Parameters.AddWithValue("@Cliente_ID", identificacion); // Cambio de '@Identificacion' a '@Cliente_ID'
                comandoInsertar.Parameters.AddWithValue("@Nombre", textBox2.Text);
                comandoInsertar.Parameters.AddWithValue("@Apellido", textBox3.Text);
                comandoInsertar.Parameters.AddWithValue("@Edad", textBox4.Text);
                comandoInsertar.Parameters.AddWithValue("@Correo", textBox5.Text);
                comandoInsertar.Parameters.AddWithValue("@Celular", textBox6.Text);

                int filasAfectadas = comandoInsertar.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Registro agregado correctamente.", "Éxito");
                    limpiar();
                    llenar_tabla();
                }
                else
                {
                    MessageBox.Show("No se pudo agregar el registro.", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Inesperado");
            }
            finally
            {
                conexion.Close();
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            // Verificar si se ingresó un valor en el campo de Identificación
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Por favor, ingrese una identificación antes de eliminar un registro.", "Error");
                return; // Salir del método si falta la identificación
            }

            if (MessageBox.Show("¿Está seguro de que desea eliminar este registro?", "Confirmación", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                conexion.Open();
                string consulta = "delete from Clientes where Cliente_ID=" + textBox1.Text; // Cambio de 'Identificacion' a 'Cliente_ID'
                SqlCommand comando = new SqlCommand(consulta, conexion);
                int num = comando.ExecuteNonQuery();

                if (num > 0)
                {
                    MessageBox.Show("Registro eliminado correctamente", "Registro Eliminado");
                }
                else
                {
                    MessageBox.Show("No se encontró ningún registro para eliminar.", "Error");
                }

                conexion.Close();
                llenar_tabla();
                limpiar();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            conexion.Open();
            String consulta = "UPDATE Clientes SET " +
                             "Nombre = '" + textBox2.Text + "', " +
                             "Apellido = '" + textBox3.Text + "', " +
                             "Edad = " + textBox4.Text + ", " +
                             "Correo = '" + textBox5.Text + "', " +
                             "Celular = '" + textBox6.Text + "' " +
                             "WHERE Cliente_ID = " + textBox1.Text; // Cambio de 'Identificacion' a 'Cliente_ID'

            SqlCommand comando = new SqlCommand(consulta, conexion);
            int num = comando.ExecuteNonQuery();

            if (num > 0)
            {
                MessageBox.Show("Registro Modificado Correctamente", "Registro Modificado");
            }
            else
            {
                MessageBox.Show("No se encontró ningún registro para actualizar.", "Error");
            }

            conexion.Close();
            llenar_tabla();
            limpiar();
        }

        private void dgvRegistro_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dgvRegistro.SelectedCells[0].Value.ToString();
            textBox2.Text = dgvRegistro.SelectedCells[1].Value.ToString();
            textBox3.Text = dgvRegistro.SelectedCells[2].Value.ToString();
            textBox4.Text = dgvRegistro.SelectedCells[3].Value.ToString();
            textBox5.Text = dgvRegistro.SelectedCells[4].Value.ToString();
            textBox6.Text = dgvRegistro.SelectedCells[5].Value.ToString();
        }
    }
}
