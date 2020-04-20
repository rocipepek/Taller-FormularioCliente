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

namespace WinClientes
{
    public partial class frmClientes : Form
    {
        public int IDCliente;
        public frmClientes()
        {
            IDCliente = 0;
            InitializeComponent();
            reestablecer();
        }
        /// <summary>
        /// /////////////////////////////////////////////CAPA DE PRESENTACION A LA CAPA DE NEGOCIO --- NO PASE DIRECTAMENTE A LA CAPA DE DATOS//////////////////////////
        /// </summary>
        private class Cliente
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Fecha_Nac { get; set; }
            public string Direccion { get; set; }
        }

        SqlConnection conectar = new SqlConnection("Data Source=ROCIO\\ROCIO;Initial Catalog=Taller;Integrated Security=true;");

        
        private void toolStripButtonNuevo_Click(object sender, EventArgs e) //Crea un nuevo cliente //TODO: Verificar que los campos no esten vacios
        {
            string strNombre, strApellido, strFechaNacimiento, strDireccion;
            strNombre = txtNombre.Text;
            strApellido = txtApellido.Text;
            strFechaNacimiento = dtpFecha.Value.Year.ToString() + '/' + dtpFecha.Value.Month.ToString() + '/' + dtpFecha.Value.Day.ToString();
            strDireccion = txtDireccion.Text;

            if (IDCliente.Equals(0)) //Si no esta registrado
            {

                List<Cliente> lista = new List<Cliente>();
               

                //SqlConnection conectar = new SqlConnection("Data Source=ROCIO\\ROCIO;Initial Catalog=Taller;Integrated Security=true;");

                string consulta = string.Format("Insert into clientes (Nombre, Apellido, Fecha_Nacimiento, Direccion) values ('{0}','{1}','{2}','{3}')",
                   strNombre, strApellido, strFechaNacimiento, strDireccion);

                SqlCommand comando = new SqlCommand(consulta, conectar);

                conectar.Open();
                comando.ExecuteNonQuery();
                conectar.Close();

                MessageBox.Show("Cliente " + txtNombre.Text + " " + txtApellido.Text + " registrado con exito", "Registro Exitoso");

            }
            else //Esta registrado --> Se modifica cliente
            {
                
                string consulta = string.Format("Update clientes set Nombre = '{0}', Apellido =  '{1}' , Fecha_Nacimiento =  '{2}', Direccion = '{3}' where IDCliente =  '{4}'",
               strNombre, strApellido, strFechaNacimiento, strDireccion, IDCliente);

                SqlCommand comando = new SqlCommand(consulta, conectar);

                conectar.Open();
                comando.ExecuteNonQuery();
                conectar.Close();

                MessageBox.Show("Cliente " + txtNombre.Text + " " + txtApellido.Text + " modificado con exito", "Modificación Exitosa");
                
            }

            reestablecer();

        }

        private void toolStripButtonGuardar_Click(object sender, EventArgs e) //Consulta la base de datos para actualizar los datos de un cliente.
        {
           /* string strNombre, strApellido, strFechaNacimiento, strDireccion;
            strNombre = txtNombre.Text;
            strApellido = txtApellido.Text;
            strFechaNacimiento = dtpFecha.Value.Year.ToString() + '/' + dtpFecha.Value.Month.ToString() + '/' + dtpFecha.Value.Day.ToString();
            strDireccion = txtDireccion.Text;

            //SqlConnection conectar = new SqlConnection("Data Source=ROCIO\\ROCIO;Initial Catalog=Taller;Integrated Security=true;");

            string consulta = string.Format("Update clientes set Nombre = '{0}', Apellido =  '{1}' , Fecha_Nacimiento =  '{2}', Direccion = '{3}' where IDCliente =  '{4}'",
                strNombre, strApellido, strFechaNacimiento, strDireccion, IDCliente);

            SqlCommand comando = new SqlCommand(consulta, conectar);

            conectar.Open();
            comando.ExecuteNonQuery();
            conectar.Close();

            MessageBox.Show("Cliente " + txtNombre.Text + " " + txtApellido.Text + " modificado con exito", "Modificación Exitosa");
            reestablecer();*/
        }

        private void toolStripButtonActualizar_Click(object sender, EventArgs e) //Muestra la lista de clientes en el data grid
        {
           /* List<Cliente> lista = new List<Cliente>();

           // SqlConnection conectar = new SqlConnection("Data Source=ROCIO\\ROCIO;Initial Catalog=Taller;Integrated Security=true;");
            String consulta = "SELECT IDCliente, Nombre, Apellido, Fecha_Nacimiento, Direccion FROM clientes";
            SqlCommand comando = new SqlCommand(consulta, conectar);

            conectar.Open();
            SqlDataReader reader = comando.ExecuteReader(); //Se lee una unica vez

            while (reader.Read()) //Consulta la primer linea para leer
            {
                Cliente pCliente = new Cliente();
                pCliente.Id = reader.GetInt32(0);
                pCliente.Nombre = reader.GetString(1);
                pCliente.Apellido = reader.GetString(2);
                pCliente.Fecha_Nac = reader.GetString(3);
                pCliente.Direccion = reader.GetString(4);


                lista.Add(pCliente);
            }

            conectar.Close();
            dgvBuscar.DataSource = lista;*/
        }


        private void dgvBuscar_Click_1(object sender, EventArgs e) //Obtengo los datos de un cliente y los muestro en el formulario
        {
            IDCliente = Convert.ToInt32(dgvBuscar.CurrentRow.Cells[0].Value);
            txtNombre.Text = Convert.ToString(dgvBuscar.CurrentRow.Cells[1].Value);
            txtApellido.Text = Convert.ToString(dgvBuscar.CurrentRow.Cells[2].Value);

            dtpFecha.Value = Convert.ToDateTime(dgvBuscar.CurrentRow.Cells[3].Value);
            txtDireccion.Text = Convert.ToString(dgvBuscar.CurrentRow.Cells[4].Value);

            toolStripButtonNuevo.Text = "Guardar";
        }

        public void reestablecer()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            dtpFecha.Value = System.DateTime.Today;
            txtDireccion.Text = "";
            IDCliente = 0;
            toolStripButtonNuevo.Text = "Nuevo";

            mostrarRegistro();
        }

        private void mostrarRegistro()
        {
            List<Cliente> lista = new List<Cliente>();

            //SqlConnection conectar = new SqlConnection("Data Source=ROCIO\\ROCIO;Initial Catalog=Taller;Integrated Security=true;");
            String consulta = "SELECT IDCliente, Nombre, Apellido, Fecha_Nacimiento, Direccion FROM clientes";
            SqlCommand comando = new SqlCommand(consulta, conectar);

            conectar.Open();
            SqlDataReader reader = comando.ExecuteReader(); //Se lee una unica vez

            while (reader.Read()) //Consulta la primer linea para leer
            {
                Cliente pCliente = new Cliente();
                pCliente.Id = reader.GetInt32(0);
                pCliente.Nombre = reader.GetString(1);
                pCliente.Apellido = reader.GetString(2);
                pCliente.Fecha_Nac = reader.GetString(3);
                pCliente.Direccion = reader.GetString(4);


                lista.Add(pCliente);
            }

            conectar.Close();
            dgvBuscar.DataSource = lista;

        }

        private void toolStripButtonEliminar_Click(object sender, EventArgs e)
        {
            if (IDCliente.Equals(0))
            {
                //No se selecciono ningun cliente
                MessageBox.Show("Seleccione un cliente", "Atención");
            }
            else
            {
                if (MessageBox.Show("¿Está seguro que desea eliminar el cliente?", "Eliminar Cliente", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    
                    String consulta = "DELETE from clientes where IDCliente= '"+IDCliente+"'";
                    SqlCommand comando = new SqlCommand(consulta, conectar);

                    conectar.Open();
                    comando.ExecuteNonQuery();
                    conectar.Close();

                    reestablecer();
                }
            }
        }

        private void toolStripButtonCancelar_Click(object sender, EventArgs e)
        {
            reestablecer();
        }
    }
}
