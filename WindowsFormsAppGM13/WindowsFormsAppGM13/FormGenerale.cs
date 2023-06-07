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

namespace WindowsFormsAppGM13
{
    public partial class FormGenerale : Form
    {
        private string connstring = @"Server=.\SQLEXPRESS;Database=GM;Trusted_Connection=True;";

        public FormGenerale()
        {
            InitializeComponent();
        }

        private void buttonClient_Click(object sender, EventArgs e)
        {
            FormClient dlg = new FormClient();
            dlg.ShowDialog();
        }

        private void buttonMatos_Click(object sender, EventArgs e)
        {
            FormMatos dlg = new FormMatos();
            dlg.ShowDialog();
        }

        private void FormGenerale_Load(object sender, EventArgs e)
        {
            comboBoxMatos.Items.Clear();

            SqlConnection cn = null;
            SqlCommand com = null;
            SqlDataReader sqr = null;

            try
            {
                cn = new SqlConnection(this.connstring);
                cn.Open();

                string strsql = "select Nom from MATERIEL";
                com = new SqlCommand(strsql, cn);
                sqr = com.ExecuteReader();

                string str;
                while (sqr.Read() == true)
                {
                    str = sqr["Nom"].ToString();
                    comboBoxMatos.Items.Add(str);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }

            if (cn != null)
            {
                cn.Close();
                cn.Dispose();
                if (com != null)
                {
                    com.Dispose();
                    if (sqr != null)
                    {
                        sqr.Close();
                    }
                }
            }
        }

        private int PontMatos(string nom)
        {
            SqlConnection cn = null;
            SqlCommand com = null;
            SqlDataReader sqr = null;

            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "select ID_MAT from MATERIEL where Nom = '" + nom + "'";
            com = new SqlCommand(strsql, cn);
            sqr = com.ExecuteReader();
            sqr.Read();

            int id = Convert.ToInt32(sqr["ID_MAT"]);

            cn.Close();
            sqr.Close();

            return id;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxMatos.SelectedIndex == -1)
            {
                MessageBox.Show("Sélectionnez un matériel !!", "erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SqlConnection cn = null;
            SqlCommand com = null;
           
            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "INSERT INTO INTERVENTION VALUES(@dateinter, @comment," +
                "@letech, @idmat)";
            com = new SqlCommand(strsql, cn);
            com.Parameters.Add("@dateinter", dateTimePickerAddInter.Value.ToString("yyyy-MM-dd"));
            com.Parameters.Add("@comment", textBoxComment.Text);
            com.Parameters.Add("@letech", textBoxTech.Text);
            com.Parameters.Add("@idmat", PontMatos(comboBoxMatos.SelectedItem.ToString()));

            com.ExecuteNonQuery();

            textBoxComment.Text = textBoxTech.Text = "";
            comboBoxMatos.SelectedIndex = -1;

            MessageBox.Show("Intervention ajoutée, merci", "Succès",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            if (cn != null)
            {
                cn.Close();
                cn.Dispose();
                if (com != null)
                {
                    com.Dispose();
                }
            }
            
        }

        private void textBoxComment_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
