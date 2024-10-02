using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RentACar
{
    public partial class aracEkle : Form
    {
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\monster\\Desktop\\kullanici.mdb");
        DataSet ds = new DataSet();

        public aracEkle()
        {
            InitializeComponent();
        }

        void listele(string aramaMetni = "")
        {
            try
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand();
                komut.Connection = baglanti;

                if (string.IsNullOrEmpty(aramaMetni))
                {
                    komut.CommandText = "SELECT * FROM araclar";
                }
                else
                {
                    komut.CommandText = "SELECT * FROM araclar WHERE Plaka LIKE @aramaMetni OR Konum LIKE @aramaMetni OR Marka LIKE @aramaMetni OR Seri LIKE @aramaMetni OR Model LIKE " +
                        "@aramaMetni OR Yil LIKE @aramaMetni OR Yakit LIKE @aramaMetni OR Vites LIKE @aramaMetni OR KM LIKE @aramaMetni OR KasaTipi LIKE @aramaMetni " +
                        "OR MotorGucu LIKE @aramaMetni OR MotorHacmi LIKE @aramaMetni OR Fiyat LIKE @aramaMetni OR Hasar LIKE @aramaMetni";
                    komut.Parameters.AddWithValue("@aramaMetni", "%" + aramaMetni + "%");
                }

                OleDbDataAdapter adt = new OleDbDataAdapter(komut);
                ds.Clear();
                adt.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.DataSource = ds.Tables[0];
                }
                else
                {
                    MessageBox.Show("Tablo bulunamadı veya tablo boş.");
                }
                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "JPEG Files(*.jpg;*.jpeg;*.jpe;*.jfif)|*.jpg|All Files(*.*)|*.*";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                    textBox13.Text = openFileDialog1.FileName.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("INSERT INTO araclar (Plaka,Konum, Marka, Seri, Model, Yil, Yakit, Vites, KM, KasaTipi, MotorGucu, MotorHacmi, Fiyat, ResimYolu, Hasar)" +
                    " VALUES (@Plaka,@Konum, @Marka, @Seri, @Model, @Yil, @Yakit, @Vites, @KM, @KasaTipi, @MotorGucu, @MotorHacmi, @Fiyat, @ResimYolu, @Hasar)", baglanti);
                
                komut.Parameters.AddWithValue("@Plaka", textBox16.Text);
                komut.Parameters.AddWithValue("@Konum", textBox1.Text);
                komut.Parameters.AddWithValue("@Marka", textBox2.Text);
                komut.Parameters.AddWithValue("@Seri", textBox3.Text);
                komut.Parameters.AddWithValue("@Model", textBox4.Text);
                komut.Parameters.AddWithValue("@Yil", textBox5.Text);
                komut.Parameters.AddWithValue("@Yakit", textBox6.Text);
                komut.Parameters.AddWithValue("@Vites", textBox7.Text);
                komut.Parameters.AddWithValue("@KM", textBox8.Text);
                komut.Parameters.AddWithValue("@KasaTipi", textBox9.Text);
                komut.Parameters.AddWithValue("@MotorGucu", textBox10.Text);
                komut.Parameters.AddWithValue("@MotorHacmi", textBox11.Text);
                komut.Parameters.AddWithValue("@Fiyat", textBox12.Text);
                komut.Parameters.AddWithValue("@ResimYolu", textBox13.Text);
                komut.Parameters.AddWithValue("@Hasar", textBox14.Text);
                
                komut.ExecuteNonQuery();
                baglanti.Close();
                listele();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
        }

        private void aracEkle_Load_1(object sender, EventArgs e)
        {
            listele();
        }
        public static int selectedAracPlaka;
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)

            {
                textBox1.Text = dataGridView1.CurrentRow.Cells["Konum"].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells["Marka"].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells["Seri"].Value.ToString();
                textBox4.Text = dataGridView1.CurrentRow.Cells["Model"].Value.ToString();
                textBox5.Text = dataGridView1.CurrentRow.Cells["Yil"].Value.ToString();
                textBox6.Text = dataGridView1.CurrentRow.Cells["Yakit"].Value.ToString();
                textBox7.Text = dataGridView1.CurrentRow.Cells["Vites"].Value.ToString();
                textBox8.Text = dataGridView1.CurrentRow.Cells["KM"].Value.ToString();
                textBox9.Text = dataGridView1.CurrentRow.Cells["KasaTipi"].Value.ToString();
                textBox10.Text = dataGridView1.CurrentRow.Cells["MotorGucu"].Value.ToString();
                textBox11.Text = dataGridView1.CurrentRow.Cells["MotorHacmi"].Value.ToString();
                textBox12.Text = dataGridView1.CurrentRow.Cells["Fiyat"].Value.ToString();
                textBox13.Text = dataGridView1.CurrentRow.Cells["ResimYolu"].Value.ToString();
                textBox14.Text = dataGridView1.CurrentRow.Cells["Hasar"].Value.ToString();
                textBox16.Text = dataGridView1.CurrentRow.Cells["Plaka"].Value.ToString();

                pictureBox2.ImageLocation = dataGridView1.CurrentRow.Cells["ResimYolu"].Value.ToString();
            }
            else
            {
                MessageBox.Show("Veri yok.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Seçili satırı güncellemek istediğinize emin misiniz?", "Satır Güncelleme Onayı", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    OleDbCommand komut = new OleDbCommand("UPDATE araclar SET Plaka = @Plaka ,Konum = @Konum, Marka = @Marka, Seri = @Seri, Model = @Model, Yil = @Yil," +
                        " Yakit = @Yakit, Vites = @Vites, KM = @KM, KasaTipi = @KasaTipi, MotorGucu = @MotorGucu, MotorHacmi = @MotorHacmi, Fiyat = @Fiyat, " +
                        "ResimYolu = @ResimYolu, Hasar = @Hasar WHERE Plaka = @Plaka", baglanti);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       

                    komut.Parameters.AddWithValue("@Plaka", textBox16.Text);
                    komut.Parameters.AddWithValue("@Konum", textBox1.Text);
                    komut.Parameters.AddWithValue("@Marka", textBox2.Text);
                    komut.Parameters.AddWithValue("@Seri", textBox3.Text);
                    komut.Parameters.AddWithValue("@Model", textBox4.Text);
                    komut.Parameters.AddWithValue("@Yil", textBox5.Text);
                    komut.Parameters.AddWithValue("@Yakit", textBox6.Text);
                    komut.Parameters.AddWithValue("@Vites", textBox7.Text);
                    komut.Parameters.AddWithValue("@KM", textBox8.Text);
                    komut.Parameters.AddWithValue("@KasaTipi", textBox9.Text);
                    komut.Parameters.AddWithValue("@MotorGucu", textBox10.Text);
                    komut.Parameters.AddWithValue("@MotorHacmi", textBox11.Text);
                    komut.Parameters.AddWithValue("@Fiyat", textBox12.Text);
                    komut.Parameters.AddWithValue("@ResimYolu", textBox13.Text);
                    komut.Parameters.AddWithValue("@Hasar", textBox14.Text);
                    
                    baglanti.Open();
                    int affectedRows = komut.ExecuteNonQuery();
                    baglanti.Close();

                    if (affectedRows > 0)
                    {
                        listele();
                        MessageBox.Show("Satır başarıyla güncellendi.");
                    }
                    else
                    {
                        MessageBox.Show("Güncelleme başarısız. Satır bulunamadı.");
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata: " + hata.Message);
                    baglanti.Close();
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Seçili satırı silmek istediğinize emin misiniz?", "Satır Silme Onayı", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string plaka = textBox16.Text; // Plaka alanı üzerinden silme işlemi yapılacak
                    baglanti.Open();

                    OleDbCommand komut = new OleDbCommand("DELETE FROM araclar WHERE Plaka = @Plaka", baglanti);

                    komut.Parameters.AddWithValue("@Plaka", plaka);
                    
                    int affectedRows = komut.ExecuteNonQuery(); // Sorguyu veritabanında çalıştır ve etkilenen satır sayısını al

                    baglanti.Close();

                    if (affectedRows > 0)
                    {
                        listele(); 
                        MessageBox.Show("Satır başarıyla silindi.");
                    }
                    else
                    {
                        MessageBox.Show("Silme başarısız. Satır bulunamadı.");
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata: " + hata.Message);
                    baglanti.Close(); 
                }
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            menu m = new menu();
            m.Show();
            this.Hide();
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            string aramaMetni = textBox15.Text;
            listele(aramaMetni);
        }
       
    }

    }
