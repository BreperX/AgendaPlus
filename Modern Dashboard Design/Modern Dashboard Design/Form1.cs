using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Modern_Dashboard_Design
{
    public partial class Form1 : Form
    {

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn
     (
          int nLeftRect,
          int nTopRect,
          int nRightRect,
          int nBottomRect,
          int nWidthEllipse,
         int nHeightEllipse

      );

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        public Form1()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            pnlNav.Height = btnDashboard.Height;
            pnlNav.Top = btnDashboard.Top;
            pnlNav.Left = btnDashboard.Left;
            btnDashboard.BackColor = Color.FromArgb(44, 62, 80);

            lblTitle.Text = "Inicio";
            this.PnlFormLoader.Controls.Clear();
            frmDashboard FrmDashboard_Vrb = new frmDashboard() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmDashboard_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmDashboard_Vrb);
            FrmDashboard_Vrb.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            
        }

        private void FlowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnDashboard.Height;
            pnlNav.Top = btnDashboard.Top;
            pnlNav.Left = btnDashboard.Left;
            btnDashboard.BackColor = Color.FromArgb(44, 62, 80);

            lblTitle.Text = "Inicio";
            this.PnlFormLoader.Controls.Clear();
            frmDashboard FrmDashboard_Vrb = new frmDashboard() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmDashboard_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmDashboard_Vrb);
            FrmDashboard_Vrb.Show();
        }

        private void BtnAnalytics_Click(object sender, EventArgs e)  //esta es la seccion de Clientes tiene el nombre de Analitics ya que era un proyecto anterior
        {
            pnlNav.Height = btnClientes.Height;
            pnlNav.Top = btnClientes.Top;
            btnClientes.BackColor = Color.FromArgb(44, 62, 80);

            lblTitle.Text = "Clientes";
            this.PnlFormLoader.Controls.Clear();
            frmAnalytics FrmDashboard_Vrb = new frmAnalytics() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmDashboard_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmDashboard_Vrb);
            FrmDashboard_Vrb.Show();
        }

        private void BtnCalander_Click(object sender, EventArgs e) //esta es la seccion de productos tiene el nombre de calendario ya que era un proyecto anterior
        {
            pnlNav.Height = btnProductos.Height;
            pnlNav.Top = btnProductos.Top;
            btnProductos.BackColor = Color.FromArgb(44, 62, 80);

            lblTitle.Text = "Productos";
            this.PnlFormLoader.Controls.Clear();
            frmCalander FrmDashboard_Vrb = new frmCalander() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmDashboard_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmDashboard_Vrb);
            FrmDashboard_Vrb.Show();
        }

        private void btnCompras_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnCompras.Height;
            pnlNav.Top = btnCompras.Top;
            btnCompras.BackColor = Color.FromArgb(44, 62, 80);

            lblTitle.Text = "Compras";
            this.PnlFormLoader.Controls.Clear();
            frmCompras FrmDashboard_Vrb = new frmCompras() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmDashboard_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmDashboard_Vrb);
            FrmDashboard_Vrb.Show();
        }


        private void BtnDashboard_Leave(object sender, EventArgs e)
        {
            btnDashboard.BackColor = Color.FromArgb(44, 62, 80);
        }

        private void BtnAnalytics_Leave(object sender, EventArgs e)
        {
            btnClientes.BackColor = Color.FromArgb(44, 62, 80);
        }

        private void BtnCalander_Leave(object sender, EventArgs e)
        {
            btnProductos.BackColor = Color.FromArgb(44, 62, 80);
        }


        private void Label11_Click(object sender, EventArgs e)
        {

        }

        private void Label14_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Label15_Click(object sender, EventArgs e)
        {

        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void PnlFormLoader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnVentas.Height;
            pnlNav.Top = btnVentas.Top;
            btnVentas.BackColor = Color.FromArgb(44, 62, 80);

            lblTitle.Text = "Ventas";
            this.PnlFormLoader.Controls.Clear();
            frmVentas FrmDashboard_Vrb = new frmVentas() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmDashboard_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmDashboard_Vrb);
            FrmDashboard_Vrb.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            pnlNav.Height = btnEmpleados.Height;
            pnlNav.Top = btnEmpleados.Top;
            btnEmpleados.BackColor = Color.FromArgb(44, 62, 80);

            lblTitle.Text = "Empleados";
            this.PnlFormLoader.Controls.Clear();
            frmEmpleados FrmDashboard_Vrb = new frmEmpleados() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmDashboard_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnlFormLoader.Controls.Add(FrmDashboard_Vrb);
            FrmDashboard_Vrb.Show();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
