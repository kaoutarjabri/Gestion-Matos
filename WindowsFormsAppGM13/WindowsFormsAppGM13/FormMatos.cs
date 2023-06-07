using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppGM13
{
    public partial class FormMatos : Form
    {
        private string connstring = @"Server=.\SQLEXPRESS;Database=GM;Trusted_Connection=True;";

        public FormMatos()
        {
            InitializeComponent();
        }

        private void buttonFermer_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void FormMatos_Load(object sender, EventArgs e)
        {
            AffMatos();
            fillComboClient();
        }


        private void fillComboClient()
        {
            comboBoxClient.Items.Clear();

            SqlConnection cn = null;
            SqlCommand com = null;
            SqlDataReader sqr = null;

            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "select Nom from Client";
            com = new SqlCommand(strsql, cn);
            sqr = com.ExecuteReader();

            string str;
            while (sqr.Read() == true)
            {
                str = sqr["Nom"].ToString();
                comboBoxClient.Items.Add(str);
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


        private void AffMatos()
        {
            listBoxMatos.Items.Clear();

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
                    listBoxMatos.Items.Add(str);
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

        private void listBoxMatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxMatos.SelectedItem == null)
            {
                return;
            }

            string nom = listBoxMatos.SelectedItem.ToString();

            SqlConnection cn = null;
            SqlCommand com = null;
            SqlDataReader sqr = null;

            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "select m.Nom as 'matosnom', m.NoSerie as 'matosserie'," +
                " m.Date_install as 'matosdate', m.MTBF as 'matosmtbf'," +
                " m.Type as 'matostype', m.Marque as 'matosmarque', " +
                "c.Nom as 'clientnom' from MATERIEL m join CLIENT c on " +
                "m.ID_CLIENT = c.ID_CLIENT where m.Nom = '" + nom + "'";


            com = new SqlCommand(strsql, cn);
            sqr = com.ExecuteReader();
            sqr.Read();

            textBoxNom.Text = sqr["matosnom"].ToString();
            textBoxSerie.Text = sqr["matosserie"].ToString();
            dateTimePickerDI.Value = Convert.ToDateTime(sqr["matosdate"]);
            textBoxMTBF.Text = sqr["matosmtbf"].ToString();
            textBoxType.Text = sqr["matostype"].ToString();
            textBoxMarque.Text = sqr["matosmarque"].ToString();

            // fixer le client
            comboBoxClient.SelectedItem = sqr["clientnom"].ToString();


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

        private bool checkMatosName(string name)
        {
            SqlConnection cn = null;
            SqlCommand com = null;
            SqlDataReader sqr = null;

            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "select COUNT(*) as nb from MATERIEL where Nom = '" +
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
            bool b = checkMatosName(textBoxNom.Text);
            if (b == true)
            {
                MessageBox.Show("Ce nom de matériel existe déjà", "erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SqlConnection cn = null;
            SqlCommand com = null;

            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "INSERT INTO MATERIEL VALUES(@lenom, @laserie, @dateIns, " +
                "@leMTBF, @typo, @lamarque, @lidclient)";

            com = new SqlCommand(strsql, cn);
            com.Parameters.Add("@lenom", textBoxNom.Text.Trim());
            com.Parameters.Add("@laserie", textBoxSerie.Text.Trim());

            com.Parameters.Add("@dateIns", dateTimePickerDI.Value.ToString("yyyy-MM-dd"));

            com.Parameters.Add("@leMTBF", textBoxMTBF.Text.Trim());
            com.Parameters.Add("@typo", textBoxType.Text.Trim());
            com.Parameters.Add("@lamarque", textBoxMarque.Text.Trim());

            com.Parameters.Add("@lidclient", PontClient(comboBoxClient.SelectedItem.ToString()));

            com.ExecuteNonQuery();
        }

        private int PontClient(string nom)
        {
            SqlConnection cn = null;
            SqlCommand com = null;
            SqlDataReader sqr = null;

            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "select ID_CLIENT from client where Nom = '" + nom + "'";
            com = new SqlCommand(strsql, cn);
            sqr = com.ExecuteReader();
            sqr.Read();

            int id = Convert.ToInt32(sqr["ID_CLIENT"]);

            cn.Close();
            sqr.Close();

            return id;
        }

        private void buttonModifier_Click(object sender, EventArgs e)
        {
            if (listBoxMatos.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner un matériel ", "erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                listBoxMatos.Focus();
                return;
            }

            SqlConnection cn = null;
            SqlCommand com = null;
            cn = new SqlConnection(this.connstring);
            cn.Open();

            string strsql = "UPDATE MATERIEL SET Nom = @lenom, NoSerie = @laserie, " +
                "Date_install = @dateIns, MTBF = @leMTBF, Type = @typo, " +
                "Marque = @lamarque, ID_CLIENT =  @lidclient " +
                "where Nom = '" + listBoxMatos.SelectedItem + "'";

            com = new SqlCommand(strsql, cn);
            com.Parameters.Add("@lenom", textBoxNom.Text.Trim());
            com.Parameters.Add("@laserie", textBoxSerie.Text.Trim());

            com.Parameters.Add("@dateIns", dateTimePickerDI.Value.ToString("yyyy-MM-dd"));

            com.Parameters.Add("@leMTBF", textBoxMTBF.Text.Trim());
            com.Parameters.Add("@typo", textBoxType.Text.Trim());
            com.Parameters.Add("@lamarque", textBoxMarque.Text.Trim());

            com.Parameters.Add("@lidclient", PontClient(comboBoxClient.SelectedItem.ToString()));

            com.ExecuteNonQuery();

            cn.Close();



        }

        private void buttonSupprimer_Click(object sender, EventArgs e)
        {
            if (listBoxMatos.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner un matériel", "erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                listBoxMatos.Focus();
                return;
            }
            if (MessageBox.Show("Voulez vous vraiment supprimer ce matériel ?", "ATTENTION",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {               
                return;
            }

            SqlConnection cn = null;
            SqlCommand com = null;

            try
            {


                cn = new SqlConnection(this.connstring);
                cn.Open();

                string strsql = "delete from MATERIEL where Nom = '" + listBoxMatos.SelectedItem + "'";

                com = new SqlCommand(strsql, cn);
                com.ExecuteNonQuery();
                AffMatos();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
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
}
