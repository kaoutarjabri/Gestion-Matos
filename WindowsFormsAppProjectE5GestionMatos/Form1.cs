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
using System.Drawing.Drawing2D;

namespace WindowsFormsAppProjectE5GestionMatos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = @"Server=.\SQLEXPRESS;Database=Livre;Trusted_Connection=True;";
            SqlConnection cn = new SqlConnection(str);
            cn.Open();

            string strSQL = "select l.Titre as TitreLivre, l.Auteur as AuteurLivre, " +
                "g.Nom as NomGenre from Livre l join Genre g on l.ID_GENRE = g.ID_GENRE";
            SqlCommand cmd = new SqlCommand(strSQL, cn);
            SqlDataReader rd = cmd.ExecuteReader();

            string letitre, auteur, nom_genre, phrase;
            while (rd.Read() == true)
            {
                auteur = rd["AuteurLivre"].ToString();
                letitre = rd["TitreLivre"].ToString();
                nom_genre = rd["NomGenre"].ToString();
                phrase = auteur + " a écrit " + letitre + ", qui est un " + nom_genre;
                listBoxGeneral.Items.Add(phrase);
            }

            rd.Close();
            cn.Close();

        }
    }
}
