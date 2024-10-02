using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace RentACar
{
    public partial class kullaniciList : Form
    {
        public kullaniciList()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\monster\\Desktop\\kullanici.mdb");

        private void listele(string aramaMetni = "")
        {
            listView1.Items.Clear(); 
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand();
            komut.Connection = baglanti;

            if (string.IsNullOrEmpty(aramaMetni))
            {
                komut.CommandText = "Select * From kullanici"; 
            }
            else
            {
                komut.CommandText = "Select * From kullanici Where ad LIKE @aramaMetni"; 
                komut.Parameters.AddWithValue("@aramaMetni", "%" + aramaMetni + "%");
            }

            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem ekle = new ListViewItem();
                ekle.Text = oku["ad"].ToString();
                ekle.SubItems.Add(oku["soyad"].ToString());
                ekle.SubItems.Add(oku["tc"].ToString());

                listView1.Items.Add(ekle);
            }

            baglanti.Close();
        }

        private void kullaniciList_Load(object sender, EventArgs e)
        {
            listele(); 
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listele(textBox1.Text); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            menu m = new menu();
            m.Show();
            this.Hide();
        }
    }
}
