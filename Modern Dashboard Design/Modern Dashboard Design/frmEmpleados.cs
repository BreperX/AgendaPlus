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
    public partial class frmEmpleados : Form
    {
        SqlConnection conexion = new SqlConnection("server=DESKTOP-QRMARBH\\SQLEXPRESS; database=ProductosClientes;integrated security=true;");

        public frmEmpleados()
        {
            InitializeComponent();
            textBox2.MaxLength = 255;
            textBox3.MaxLength = 10;
            textBox4.MaxLength = 3;
            textBox5.MaxLength = 5;
            textBox6.MaxLength = 100;
        }

        public void LlenarTabla()
        {
            string consulta = "SELECT * FROM Empleados";
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
            textBox5.Clear();
            textBox6.Clear();
            textBox1.Focus();
        }

        private void frmEmpleados_Load(object sender, EventArgs e)
        {
            LlenarTabla();
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

                // Verificar si ya existe un registro con el mismo ID_Empleado
                int idEmpleado = int.Parse(textBox1.Text);
                string consultaVerificar = "SELECT COUNT(*) FROM Empleados WHERE ID_Empleado = @ID_Empleado";

                SqlCommand comandoVerificar = new SqlCommand(consultaVerificar, conexion);
                comandoVerificar.Parameters.AddWithValue("@ID_Empleado", idEmpleado);

                int count = (int)comandoVerificar.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Ya existe un registro con el mismo ID_Empleado.", "Error");
                    return;
                }

                // Si no existe, proceder con la inserción
                string consultaInsertar = "INSERT INTO Empleados (ID_Empleado, Nombre, Genero, Edad, Calificacion, Especialidad) " +
                                         "VALUES (@ID_Empleado, @Nombre, @Genero, @Edad, @Calificacion, @Especialidad)";

                SqlCommand comandoInsertar = new SqlCommand(consultaInsertar, conexion);
                comandoInsertar.Parameters.AddWithValue("@ID_Empleado", idEmpleado);
                comandoInsertar.Parameters.AddWithValue("@Nombre", textBox2.Text);
                comandoInsertar.Parameters.AddWithValue("@Genero", textBox3.Text);
                comandoInsertar.Parameters.AddWithValue("@Edad", textBox4.Text);
                comandoInsertar.Parameters.AddWithValue("@Calificacion", textBox5.Text);
                comandoInsertar.Parameters.AddWithValue("@Especialidad", textBox6.Text);

                int filasAfectadas = comandoInsertar.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Registro agregado correctamente.", "Éxito");
                    LimpiarCampos();
                    LlenarTabla();
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
            // Verificar si se ingresó un valor en el campo de ID_Empleado
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Por favor, ingrese un ID_Empleado antes de eliminar un registro.", "Error");
                return;
            }

            if (MessageBox.Show("¿Está seguro de que desea eliminar este registro?", "Confirmación", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    conexion.Open();

                    int idEmpleado = int.Parse(textBox1.Text);
                    string consultaBorrar = "DELETE FROM Empleados WHERE ID_Empleado = @ID_Empleado";
                    SqlCommand comandoBorrar = new SqlCommand(consultaBorrar, conexion);
                    comandoBorrar.Parameters.AddWithValue("@ID_Empleado", idEmpleado);

                    int num = comandoBorrar.ExecuteNonQuery();

                    if (num > 0)
                    {
                        MessageBox.Show("Registro eliminado correctamente", "Registro Eliminado");
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
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Verificar si se ingresó un valor en el campo de ID_Empleado
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Por favor, ingrese un ID_Empleado antes de editar un registro.", "Error");
                return;
            }

            try
            {
                conexion.Open();

                int idEmpleado = int.Parse(textBox1.Text);
                string consultaEditar = "UPDATE Empleados SET " +
                                        "Nombre = @Nombre, " +
                                        "Genero = @Genero, " +
                                        "Edad = @Edad, " +
                                        "Calificacion = @Calificacion, " +
                                        "Especialidad = @Especialidad " +
                                        "WHERE ID_Empleado = @ID_Empleado";

                SqlCommand comandoEditar = new SqlCommand(consultaEditar, conexion);
                comandoEditar.Parameters.AddWithValue("@ID_Empleado", idEmpleado);
                comandoEditar.Parameters.AddWithValue("@Nombre", textBox2.Text);
                comandoEditar.Parameters.AddWithValue("@Genero", textBox3.Text);
                comandoEditar.Parameters.AddWithValue("@Edad", textBox4.Text);
                comandoEditar.Parameters.AddWithValue("@Calificacion", textBox5.Text);
                comandoEditar.Parameters.AddWithValue("@Especialidad", textBox6.Text);

                int filasAfectadas = comandoEditar.ExecuteNonQuery();

                if (filasAfectadas > 0)
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
                    textBox1.Text = dgvRegistro.Rows[rowIndex].Cells[0].Value?.ToString() ?? string.Empty;
                    textBox2.Text = dgvRegistro.Rows[rowIndex].Cells[1].Value?.ToString() ?? string.Empty;
                    textBox3.Text = dgvRegistro.Rows[rowIndex].Cells[2].Value?.ToString() ?? string.Empty;
                    textBox4.Text = dgvRegistro.Rows[rowIndex].Cells[3].Value?.ToString() ?? string.Empty;
                    textBox5.Text = dgvRegistro.Rows[rowIndex].Cells[4].Value?.ToString() ?? string.Empty;
                    textBox6.Text = dgvRegistro.Rows[rowIndex].Cells[5].Value?.ToString() ?? string.Empty;
                }
                else
                {
                    LimpiarCampos();
                }
            }
            else
            {
                LimpiarCampos();
            }
        }
    }
}
