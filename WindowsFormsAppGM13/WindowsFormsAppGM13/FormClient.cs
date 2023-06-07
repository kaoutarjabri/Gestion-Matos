using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsAppGM13
{
    public partial class FormClient : Form
    {
        private string connstring = @"Server=.\SQLEXPRESS;Database=GM;Trusted_Connection=True;";

        public FormClient()
        {
            InitializeComponent();
        }

        private void buttonFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormClient_Load(object sender, EventArgs e)
        {
            affclients();

        }

        private void affclients()
        {
            listBoxClient.Items.Clear();

            SqlConnection cn = null;
            SqlCommand com = null;
            SqlDataReader sqr = null;

            try
            {
                cn = new SqlConnection(this.connstring);
                cn.Open();

                string strsql = "select Nom from CLIENT";
                com = new SqlCommand(strsql, cn);
                sqr = com.ExecuteReader();

                string str;
                while (sqr.Read() == true)
                {
                    str = sqr["Nom"].ToString();
                    listBoxClient.Items.Add(str);
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

        private void listBoxClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxClient.SelectedItem == null)
            {
                return;
            }

            string nom = listBoxClient.SelectedItem.ToString();

            SqlConnection cn = null;
            SqlCommand com = null;
            SqlDataReader sqr = null;

            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "select * from client where Nom = '" + nom + "'";
            com = new SqlCommand(strsql, cn);
            sqr = com.ExecuteReader();
            sqr.Read();

            textBoxNom.Text = sqr["Nom"].ToString();
            textBoxAdresse.Text = sqr["Adresse"].ToString();
            textBoxMail.Text = sqr["Mail"].ToString();
            textBoxTel.Text = sqr["Tel"].ToString();

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

        private bool checkClientName(string name)
        {
            SqlConnection cn = null;
            SqlCommand com = null;
            SqlDataReader sqr = null;

            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "select COUNT(*) as nb from CLIENT where Nom = '" +
                textBoxNom.Text + "'";
            com = new SqlCommand(strsql, cn);
            sqr = com.ExecuteReader();
            sqr.Read();
            int i = Convert.ToInt32(sqr["nb"]);

            if (i == 1)
                return true;
            else
                return false;
        }

      


        private void buttonAjouter_Click(object sender, EventArgs e)
        {
            bool b = checkClientName(textBoxNom.Text);
            if (b == true)
            {
                MessageBox.Show("Ce nom de société existe déjà", "erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SqlConnection cn = null;
            SqlCommand com = null;

            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "INSERT INTO CLIENT VALUES(@lenom, @ladresse," +
                "@lemail, @letel)";
            com = new SqlCommand(strsql, cn);
            com.Parameters.Add("@lenom", textBoxNom.Text.Trim());
            com.Parameters.Add("@ladresse", textBoxAdresse.Text);
            com.Parameters.Add("@lemail", textBoxMail.Text);
            com.Parameters.Add("@letel", textBoxTel.Text);


            com.ExecuteNonQuery();

            textBoxNom.Text = textBoxAdresse.Text = textBoxMail.Text = textBoxTel.Text = "";

            affclients();

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

        private void buttonModifier_Click(object sender, EventArgs e)
        {
            if (listBoxClient.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner une société", "erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                listBoxClient.Focus();
                return;       
            }
            SqlConnection cn = null;
            SqlCommand com = null;
            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "UPDATE CLIENT SET Nom = @lenom, Adresse = @ladresse, " +
                "Mail = @lemail, Tel = @letel " +
                "where Nom = '" + listBoxClient.SelectedItem + "'";

            com = new SqlCommand(strsql, cn);
            com.Parameters.Add("@lenom", textBoxNom.Text.Trim());
            com.Parameters.Add("@ladresse", textBoxAdresse.Text);
            com.Parameters.Add("@lemail", textBoxMail.Text);
            com.Parameters.Add("@letel", textBoxTel.Text);

            com.ExecuteNonQuery();

            affclients();

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

        private void buttonSupprimer_Click(object sender, EventArgs e)
        {
            if (listBoxClient.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner une société", "erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                listBoxClient.Focus();
                return;
            }
            SqlConnection cn = null;
            SqlCommand com = null;
            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "delete from client where Nom = '" + listBoxClient.SelectedItem + "'";

            com = new SqlCommand(strsql, cn);
            com.ExecuteNonQuery();
            affclients();

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
    }
}
