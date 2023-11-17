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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Modern_Dashboard_Design
{
    public partial class frmCalander : Form
    {
        SqlConnection conexion = new SqlConnection("server=DESKTOP-QRMARBH\\SQLEXPRESS; database=ProductosClientes;integrated security=true;");

        public frmCalander()
        {
            InitializeComponent();
            textBox1.MaxLength = 15;
            textBox2.MaxLength = 20;
            textBox3.MaxLength = 255;
            textBox4.MaxLength = 10;
            textBox5.MaxLength = 10;
            textBox6.MaxLength = 3;

        }

        public void llenar_tabla()
        {
            string consulta = "select * from Productos"; // Cambio de 'Producto' a 'Productos'
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

        private void frmCalander_Load(object sender, EventArgs e)
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
                MessageBox.Show("Por favor, complete todos los campos antes de agregar un registro.", "Error");
                return; // Salir del método si falta algún dato
            }

            try
            {
                conexion.Open();

                // Verificar si ya existe un registro con la misma IdProducto
                string idProducto = textBox1.Text;
                string consultaVerificar = "SELECT COUNT(*) FROM Productos WHERE ID_Producto = @ID_Producto"; // Cambio de 'IdProducto' a 'ID_Producto'

                SqlCommand comandoVerificar = new SqlCommand(consultaVerificar, conexion);
                comandoVerificar.Parameters.AddWithValue("@ID_Producto", idProducto); // Cambio de '@IdProducto' a '@ID_Producto'

                int count = (int)comandoVerificar.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Ya existe un registro con la misma IdProducto.", "Error");
                    return;
                }

                // Si no existe, proceder con la inserción
                string consultaInsertar = "INSERT INTO Productos (ID_Producto, Nombre, Descripcion, PrecioCompra, PrecioVenta, Existencias) " +
                                         "VALUES (@ID_Producto, @Nombre, @Descripcion, @PrecioCompra, @PrecioVenta, @Existencias)";

                SqlCommand comandoInsertar = new SqlCommand(consultaInsertar, conexion);
                comandoInsertar.Parameters.AddWithValue("@ID_Producto", idProducto); // Cambio de '@IdProducto' a '@ID_Producto'
                comandoInsertar.Parameters.AddWithValue("@Nombre", textBox2.Text);
                comandoInsertar.Parameters.AddWithValue("@Descripcion", textBox3.Text);
                comandoInsertar.Parameters.AddWithValue("@PrecioCompra", textBox4.Text);
                comandoInsertar.Parameters.AddWithValue("@PrecioVenta", textBox5.Text);
                comandoInsertar.Parameters.AddWithValue("@Existencias", textBox6.Text);

                int filasAfectadas = comandoInsertar.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Registro agregado correctamente", "Éxito");
                    limpiar();
                    llenar_tabla();
                }
                else
                {
                    MessageBox.Show("No se pudo agregar el registro", "Error");
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
            // Verificar si se ingresó un valor en el campo de IdProducto
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Por favor, ingrese un IdProducto antes de eliminar un registro.", "Error");
                return; // Salir del método si falta el IdProducto
            }

            if (MessageBox.Show("¿Está seguro de que desea eliminar este registro?", "Confirmación", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                conexion.Open();
                string consulta = "delete from Productos where ID_Producto=" + textBox1.Text; // Cambio de 'IdProducto' a 'ID_Producto'
                SqlCommand comando = new SqlCommand(consulta, conexion);
                int num = comando.ExecuteNonQuery();

                if (num > 0)
                {
                    MessageBox.Show("Registro eliminado Correctamente", "Registro Eliminado");
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
            String consulta = "UPDATE Productos SET " +
                             "Nombre = '" + textBox2.Text + "', " +
                             "Descripcion = '" + textBox3.Text + "', " +
                             "PrecioCompra = " + textBox4.Text + ", " +
                             "PrecioVenta = " + textBox5.Text + ", " +
                             "Existencias = " + textBox6.Text +
                             " WHERE ID_Producto = " + textBox1.Text; // Cambio de 'IdProducto' a 'ID_Producto'

            SqlCommand comando = new SqlCommand(consulta, conexion);
            int num = comando.ExecuteNonQuery();

            if (num > 0)
            {
                MessageBox.Show("Registro Modificado Correctamente", "Resgistro Modificado");
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

