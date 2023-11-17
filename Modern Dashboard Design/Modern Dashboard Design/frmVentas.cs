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
    public partial class frmVentas : Form
    {
        SqlConnection conexion = new SqlConnection("server=DESKTOP-QRMARBH\\SQLEXPRESS; database=ProductosClientes;integrated security=true;");
        int existenciasOriginal; // Variable para almacenar las existencias originales al editar

        public frmVentas()
        {
            InitializeComponent();
            // Configurar longitudes máximas para los campos
            textBox2.MaxLength = 10;
            textBox3.MaxLength = 10;
            textBox4.MaxLength = 10;

            // Configurar el campo de ID_Venta como de solo lectura
            textBox1.ReadOnly = true;
        }

        public void LlenarTabla()
        {
            // Consulta para seleccionar todos los registros de la tabla Ventas
            string consulta = "SELECT * FROM Ventas";
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            dgvRegistro.DataSource = dt;
        }

        public void LimpiarCampos()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            // Enfocar el primer campo
            textBox2.Focus();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Verificar si los campos requeridos están vacíos
            if (string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos antes de agregar un registro.", "Error");
                return;
            }

            try
            {
                conexion.Open();

                // Si no existe, proceder con la inserción
                string consultaInsertar = "INSERT INTO Ventas (Venta, Existencias, Producto_ID) " +
                                         "VALUES (@Venta, @Existencias, @Producto_ID)";

                SqlCommand comandoInsertar = new SqlCommand(consultaInsertar, conexion);
                comandoInsertar.Parameters.AddWithValue("@Venta", textBox2.Text);
                comandoInsertar.Parameters.AddWithValue("@Existencias", textBox3.Text);
                comandoInsertar.Parameters.AddWithValue("@Producto_ID", textBox4.Text); // Asignar el valor correcto

                int filasAfectadas = comandoInsertar.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    // Decrementar las existencias del producto correspondiente
                    int productoID = Convert.ToInt32(textBox4.Text);
                    int existenciasVendidas = Convert.ToInt32(textBox3.Text);

                    string consultaActualizarExistencias = "UPDATE Productos SET Existencias = Existencias - @Existencias " +
                                                           "WHERE ID_Producto = @Producto_ID";

                    SqlCommand comandoActualizarExistencias = new SqlCommand(consultaActualizarExistencias, conexion);
                    comandoActualizarExistencias.Parameters.AddWithValue("@Existencias", existenciasVendidas);
                    comandoActualizarExistencias.Parameters.AddWithValue("@Producto_ID", productoID);

                    int filasAfectadasExistencias = comandoActualizarExistencias.ExecuteNonQuery();

                    if (filasAfectadasExistencias > 0)
                    {
                        MessageBox.Show("Venta registrada correctamente.", "Éxito");
                        LimpiarCampos();
                        LlenarTabla();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar las existencias del producto.", "Error");
                    }
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
            // Verificar si se ingresó un valor en el campo de ID_Venta
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Por favor, ingrese un ID_Venta antes de eliminar un registro.", "Error");
                return;
            }

            try
            {
                conexion.Open();

                // Obtener las existencias originales del producto
                string consultaExistenciasOriginal = "SELECT Existencias FROM Ventas WHERE ID_Venta = @ID_Venta";
                SqlCommand comandoExistenciasOriginal = new SqlCommand(consultaExistenciasOriginal, conexion);
                comandoExistenciasOriginal.Parameters.AddWithValue("@ID_Venta", textBox1.Text);

                existenciasOriginal = (int)comandoExistenciasOriginal.ExecuteScalar();

                // Realizar la eliminación de la venta
                string consultaBorrar = "DELETE FROM Ventas WHERE ID_Venta=" + textBox1.Text;
                SqlCommand comandoBorrar = new SqlCommand(consultaBorrar, conexion);
                int num = comandoBorrar.ExecuteNonQuery();

                // Calcular las nuevas existencias después de borrar la venta
                int nuevasExistencias = existenciasOriginal - existenciasOriginal; // existenciasOriginal es la cantidad original de la venta

                // Actualizar las existencias del producto en la base de datos después de borrar la venta
                string consultaActualizarExistencias = "UPDATE Productos SET Existencias = @Existencias WHERE ID_Producto = @ID_Producto";
                SqlCommand comandoActualizarExistencias = new SqlCommand(consultaActualizarExistencias, conexion);
                comandoActualizarExistencias.Parameters.AddWithValue("@Existencias", nuevasExistencias);
                comandoActualizarExistencias.Parameters.AddWithValue("@ID_Producto", textBox4.Text); // Asegúrate de que este sea el parámetro correcto

                int filasAfectadasExistencias = comandoActualizarExistencias.ExecuteNonQuery();

                if (num > 0 && filasAfectadasExistencias > 0)
                {
                    MessageBox.Show("Registro eliminado Correctamente", "Registro Eliminado");
                    LlenarTabla();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se encontró ningún registro para eliminar.", "Error");
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

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Verificar si se ingresó un valor en el campo de ID_Venta
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Por favor, ingrese un ID_Venta antes de editar un registro.", "Error");
                return;
            }

            try
            {
                conexion.Open();

                // Obtener las existencias originales del producto
                string consultaExistenciasOriginal = "SELECT Existencias FROM Ventas WHERE ID_Venta = @ID_Venta";
                SqlCommand comandoExistenciasOriginal = new SqlCommand(consultaExistenciasOriginal, conexion);
                comandoExistenciasOriginal.Parameters.AddWithValue("@ID_Venta", textBox1.Text);

                existenciasOriginal = (int)comandoExistenciasOriginal.ExecuteScalar();

                // Realizar la actualización de la venta
                string consultaEditar = "UPDATE Ventas SET " +
                                        "Venta = @Venta, " +
                                        "Existencias = @Existencias, " +
                                        "Producto_ID = @Producto_ID " +
                                        "WHERE ID_Venta = @ID_Venta";

                SqlCommand comandoEditar = new SqlCommand(consultaEditar, conexion);
                comandoEditar.Parameters.AddWithValue("@ID_Venta", textBox1.Text);
                comandoEditar.Parameters.AddWithValue("@Venta", textBox2.Text);
                comandoEditar.Parameters.AddWithValue("@Existencias", textBox3.Text);
                comandoEditar.Parameters.AddWithValue("@Producto_ID", textBox4.Text); // Asegúrate de que este sea el parámetro correcto

                int filasAfectadas = comandoEditar.ExecuteNonQuery();

                // Calcular las nuevas existencias después de la edición
                int nuevasExistencias = existenciasOriginal + Convert.ToInt32(textBox3.Text) - existenciasOriginal; // existenciasOriginal es la cantidad original de la venta

                // Actualizar las existencias del producto en la base de datos
                string consultaActualizarExistencias = "UPDATE Productos SET Existencias = @Existencias WHERE ID_Producto = @ID_Producto";
                SqlCommand comandoActualizarExistencias = new SqlCommand(consultaActualizarExistencias, conexion);
                comandoActualizarExistencias.Parameters.AddWithValue("@Existencias", nuevasExistencias);
                comandoActualizarExistencias.Parameters.AddWithValue("@ID_Producto", textBox4.Text); // Asegúrate de que este sea el parámetro correcto

                int filasAfectadasExistencias = comandoActualizarExistencias.ExecuteNonQuery();

                if (filasAfectadas > 0 && filasAfectadasExistencias > 0)
                {
                    MessageBox.Show("Registro Modificado Correctamente", "Registro Modificado");
                    LlenarTabla();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se encontró ningún registro para actualizar.", "Error");
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

        private void dgvRegistro_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvRegistro.SelectedCells.Count > 0)
            {
                int rowIndex = dgvRegistro.SelectedCells[0].RowIndex;

                if (rowIndex < dgvRegistro.Rows.Count)
                {
                    // Verificar si el índice de la fila está dentro del rango válido
                    textBox1.Text = dgvRegistro.Rows[rowIndex].Cells[0].Value?.ToString() ?? string.Empty;
                    textBox2.Text = dgvRegistro.Rows[rowIndex].Cells[1].Value?.ToString() ?? string.Empty;
                    textBox3.Text = dgvRegistro.Rows[rowIndex].Cells[2].Value?.ToString() ?? string.Empty;
                    textBox4.Text = dgvRegistro.Rows[rowIndex].Cells[3].Value?.ToString() ?? string.Empty;
                }
                else
                {
                    // Limpiar los TextBox si el índice de la fila está fuera del rango
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                }
            }
            else
            {
                // Limpiar los TextBox si no hay celdas seleccionadas
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
            }
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            LlenarTabla();
        }
    }
}


