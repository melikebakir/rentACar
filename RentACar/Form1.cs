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
using System.Diagnostics.Eventing.Reader;

namespace RentACar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            kayit kyt = new kayit();
            kyt.Show();
            this.Visible = false;

        }

        public static string kullaniciAd;
        public static string kullaniciSoyad;
        public static string kullaniciTC;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\monster\\Desktop\\kullanici.mdb");
                baglanti.Open();
                OleDbCommand sorgu = new OleDbCommand("select ad, soyad, tc from kullanici where ad=@isim and soyad=@soyisim and tc=@kimlik", baglanti);
                sorgu.Parameters.AddWithValue("@isim", textBox1.Text);
                sorgu.Parameters.AddWithValue("@soyisim", textBox2.Text);
                sorgu.Parameters.AddWithValue("@kimlik", textBox3.Text);
                OleDbDataReader dr;
                dr = sorgu.ExecuteReader();

                if (dr.Read())
                {
                    kullaniciAd = dr["ad"].ToString();
                    kullaniciSoyad = dr["soyad"].ToString();
                    kullaniciTC = dr["tc"].ToString();

                    icerik frm = new icerik();
                    frm.Show();
                    this.Visible = false;
                }
                else
                {
                    baglanti.Close();
                    MessageBox.Show("Kayıtlı kullanıcı bulunamadı..");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                textBox1.Text = "";
                textBox2.Clear();
                textBox3.Clear();
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
