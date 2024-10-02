using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentACar
{
    public partial class kayit : Form
    {
        public kayit()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\monster\\Desktop\\kullanici.mdb");

       
        private void veriKaydet()
        {
            try
            {
                baglanti.Open();

                OleDbCommand kontrol = new OleDbCommand("SELECT COUNT(*) FROM kullanici WHERE tc = @tc", baglanti);
                kontrol.Parameters.AddWithValue("@tc", textBox3.Text);
                int kullaniciSayisi = (int)kontrol.ExecuteScalar();

                if (kullaniciSayisi > 0)
                {
                    MessageBox.Show("Bu kullanıcı zaten kayıtlı!");
                }
                OleDbCommand kaydet = new OleDbCommand("INSERT INTO kullanici (ad, soyad, tc) VALUES('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "')", baglanti);
                kaydet.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Kayıt işlemi başarılı :)");
                icerik frm = new icerik();
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            veriKaydet();
        }
    }


}
