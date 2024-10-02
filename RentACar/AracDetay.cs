using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace RentACar
{
    public partial class AracDetay : Form
    {
        private icerik parentForm;
        private string plaka;
       
       public AracDetay(icerik parent)
        {
            InitializeComponent();
            parentForm = parent;
            this.FormClosed += new FormClosedEventHandler(this.AracDetay_FormClosed);
        }
        private void AracDetay_FormClosed(object sender, FormClosedEventArgs e)
        {          
            parentForm.HesaplaFiyat();  // AracDetay formu kapandığında fiyatı hesapla
        }

        public void BilgileriYukle(string plaka)
        {
            this.plaka = plaka;
            OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\monster\\Desktop\\kullanici.mdb");
            try
            {
                baglanti.Open();
                string query = "SELECT Konum, Marka, Seri, Model, Yil, Yakit, Vites, KM, KasaTipi, MotorGucu, MotorHacmi, Fiyat, ResimYolu, Hasar FROM araclar WHERE Plaka = @plaka";
                OleDbCommand komut = new OleDbCommand(query, baglanti);
                komut.Parameters.AddWithValue("@plaka", plaka);

                OleDbDataReader reader = komut.ExecuteReader();
                if (reader.Read())
                {
                    label1.Text = reader["Konum"].ToString();
                    label2.Text = reader["Marka"].ToString();
                    label3.Text = reader["Seri"].ToString();
                    label4.Text = reader["Model"].ToString();
                    label5.Text = reader["Yil"].ToString();
                    label6.Text = reader["Yakit"].ToString();
                    label7.Text = reader["Vites"].ToString();
                    label8.Text = reader["KM"].ToString();
                    label9.Text = reader["KasaTipi"].ToString();
                    label10.Text = reader["MotorGucu"].ToString();
                    label11.Text = reader["MotorHacmi"].ToString();
                    label12.Text = reader["Fiyat"].ToString();
                    label13.Text = reader["Hasar"].ToString();

                    if (File.Exists(reader["ResimYolu"].ToString()))
                    {
                        pictureBox1.Image = Image.FromFile(reader["ResimYolu"].ToString());
                    }
                }

                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            icerik icr = new icerik();
            icr.Show();
            this.Hide();
        }

        private void btnsec_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Araç seçimini onaylıyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Araç seçimi başarılı.");
                parentForm.SetSecilenAracPlaka(this.plaka);

                if (decimal.TryParse(label12.Text, out decimal fiyat))
                {
                    parentForm.SetSecilenAracFiyat(fiyat); // Günlük fiyat bilgisini geçiyoruz
                }
                else
                {
                    MessageBox.Show("Fiyat bilgisi geçerli bir sayı değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.Close();
            }
        }
    }
}
