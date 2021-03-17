using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace atestatnou
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
        DataTable dataTable = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\atestat\Admitere.mdf;Integrated Security=True;Connect Timeout=30");
            update("UPDATE Admitere SET media=0 , rezultat=''");
            select("SELECT * FROM Admitere");
        }

        private void update(string query)
        {
            con.Open();

            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            con.Close();
        }

        
        private void select(string query)
        {
            con.Open();

            dataTable.Clear();
            dataTable.Columns.Clear();

            cmd = new SqlCommand(query, con);
            
            sqlDataAdapter.SelectCommand = cmd;
            sqlDataAdapter.Fill(dataTable);

            dataGridView.DataSource = dataTable;

            con.Close();
        }

        

        private void Initbutton_Click(object sender, EventArgs e)
        {
            update("UPDATE Admitere SET media=(proba1+proba2-0.01)/2, rezultat='RESPINS'");
            update("UPDATE Admitere " +
                    "SET rezultat='ADMIS' " +
                    "WHERE Id in " +
                    "(SELECT TOP(20) Id FROM Admitere WHERE proba1>=5 AND proba2>=5 ORDER BY media DESC)");
            update("UPDATE Admitere SET oras='brasov' WHERE left(oras, 6)='brasov'");
            select("SELECT * FROM Admitere");

        }

        private void buttonFete_Click(object sender, EventArgs e)
        {
            select("SELECT Nume, Prenume, Rezultat, Media FROM Admitere WHERE Sex='f' ORDER BY Media DESC");
        }

        private void buttonBaieti_Click(object sender, EventArgs e)
        {
            select("SELECT Nume, Prenume, Rezultat, Media FROM Admitere WHERE Sex='m' ORDER BY Media DESC");
        }

        private void buttonUltimii5_Click(object sender, EventArgs e)
        {
            select("SELECT TOP(5) Nume, Prenume, Media, Datan, Oras FROM Admitere WHERE Rezultat='ADMIS' ORDER BY Media");
        }

        private void buttonPrimii5_Click(object sender, EventArgs e)
        {
            select("SELECT TOP(5) Nume, Prenume, Media, Datan, Oras FROM Admitere WHERE Rezultat='ADMIS' ORDER BY Media DESC");
        }

        private void buttonCuprins_Click(object sender, EventArgs e)
        {
            select("SELECT Nume, Prenume, Oras, Datan, Media FROM Admitere " +
                    "Where Rezultat='ADMIS' AND DATEADD(MONTH, 12*18, Datan)<=GETDATE() AND DATEADD(MONTH, 12*20, Datan)>=GETDATE() " +
                    "ORDER BY Datan, Nume");
        }

        private void buttonOrdP1_Click(object sender, EventArgs e)
        {
            select("SELECT Nume, Prenume, Proba1, Rezultat FROM Admitere ORDER BY Proba1 DESC");
        }

        private void buttonOrdP2_Click(object sender, EventArgs e)
        {
            select("SELECT Nume, Prenume, Proba2, Rezultat FROM Admitere ORDER BY Proba2 DESC");
        }

        private void buttonOrdAlfabetic_Click(object sender, EventArgs e)
        {
            select("SELECT Nume, Prenume, Media, Rezultat FROM Admitere ORDER BY Nume, Prenume");
        }

        private int numar(string query)
        {
            con.Open();

            cmd = new SqlCommand(query, con);
            int rez = Convert.ToInt16(cmd.ExecuteScalar());
            con.Close();

            return rez;
        }

        private void buttonStatistica_Click(object sender, EventArgs e)
        {
            int total = numar("SELECT COUNT(*) FROM Admitere");
            int total1 = numar("SELECT COUNT(*) FROM Admitere WHERE Media<=5.00 AND Media>=1.00");
            int total2 = numar("SELECT COUNT(*) FROM Admitere WHERE Media<=7.00 AND Media>=5.01");
            int total3 = numar("SELECT COUNT(*) FROM Admitere WHERE Media<=9.00 AND Media>=7.01");
            int total4 = numar("SELECT COUNT(*) FROM Admitere WHERE Media<=10.00 AND Media>=9.01");
            textBoxSub5.Text = Convert.ToString(total1 * 100 / total) + "%";
            textBoxSub7.Text = Convert.ToString(total2 * 100 / total) + "%";
            textBoxSub9.Text = Convert.ToString(total3 * 100 / total) + "%";
            textBoxSub10.Text = Convert.ToString(total4 * 100 / total) + "%";
        }

        private void buttonAdmis_Click(object sender, EventArgs e)
        {
            select("SELECT Nume, Prenume, Proba1, Proba2, Media, Datan, Rezultat, Sex, Id FROM Admitere WHERE LEFT(UPPER(Oras), 4)='CLUJ' AND Rezultat='ADMIS' ORDER BY Media");
        }

        private void buttonRespins_Click(object sender, EventArgs e)
        {
            select("SELECT Nume, Prenume, Proba1, Proba2, Media, Datan, Rezultat, Sex, Id FROM Admitere WHERE LEFT(UPPER(Oras), 4)='CLUJ' AND Rezultat='RESPINS' ORDER BY Media");
        }

        private void buttonFeteC_Click(object sender, EventArgs e)
        {
            select("SELECT TOP(3) Nume, Prenume, Oras, Media FROM Admitere WHERE Sex='f' AND LEFT(UPPER(Oras), 6)!='BRASOV' ORDER BY Media DESC, Proba1 DESC");
        }

        private void buttonBaietiC_Click(object sender, EventArgs e)
        {
            select("SELECT TOP(2) Nume, Prenume, Oras, Media FROM Admitere WHERE Sex='m' AND LEFT(UPPER(Oras), 6)!='BRASOV' ORDER BY Media DESC, Proba1 DESC");
        }

        private void buttonMerit_Click(object sender, EventArgs e)
        {
            //select("SELECT Nume, Prenume, Media Oras FROM Admitere WHERE (Media>=9.75 AND Media<=10) ORDER BY Nume, Prenume");
            select("SELECT Nume, Prenume, Media Oras FROM Admitere WHERE Media BETWEEN 9.75 AND 10 ORDER BY Nume, Prenume");
        }

        private void buttonStudii_Click(object sender, EventArgs e)
        {
            //select("SELECT Nume, Prenume, Media Oras FROM Admitere WHERE (Media>=8.50 AND Media<=9.74) ORDER BY Nume, Prenume");
            select("SELECT Nume, Prenume, Media Oras FROM Admitere WHERE Media BETWEEN 8.50 AND 9.74 ORDER BY Nume, Prenume");
        }

        private void buttonIncorporabili_Click(object sender, EventArgs e)
        {
            select("SELECT Nume, Prenume, Datan, Oras FROM Admitere WHERE Sex='m' AND LEFT(UPPER(Rezultat), 7)='RESPINS' AND DATEADD(YEAR, 20, Datan)<'2021-05-20' ORDER BY Nume, Prenume");
        }

        private void buttonNeincorporabili_Click(object sender, EventArgs e)
        {
            select("SELECT Nume, Prenume, Datan, Oras FROM Admitere WHERE Sex='m' AND NOT(LEFT(UPPER(Rezultat), 7)='RESPINS' AND DATEADD(YEAR, 20, Datan)<'2021-05-20') ORDER BY Nume, Prenume");
        }

        
        private void buttonStatisticaAdmisi_Click(object sender, EventArgs e)
        {
            int total = numar("SELECT COUNT(*) FROM Admitere WHERE UPPER(Oras)=UPPER('" + textBoxOras.Text +"')");
            labelTotal.Text += total;
            int admisi = numar("SELECT COUNT(*) FROM Admitere WHERE UPPER(Rezultat)='ADMIS' AND UPPER(Oras)=UPPER('" + textBoxOras.Text + "')");
            labelProcent.Text += ((Math.Round(1.00 * (admisi * 100 / total)) + "%")); 
        }
    }
}
