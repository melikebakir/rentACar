using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace RentACar
{
    public partial class icerik : Form
    {
        private string secilenAracPlaka;
        private decimal secilenAracFiyat;
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\monster\\Desktop\\kullanici.mdb");

        public icerik()
        {
            InitializeComponent();

            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            label7.Visible = false;
        }

        public void SetSecilenAracPlaka(string plaka)
        {
            secilenAracPlaka = plaka;
        }

        public void SetSecilenAracFiyat(decimal fiyat)
        {
            secilenAracFiyat = fiyat;
            label7.Text = fiyat.ToString("C"); // Fiyatı para birimi formatında göster
            label7.Visible = true; 
        }
        private void aracikaldir(string plaka)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Plaka"].Value != null && row.Cells["Plaka"].Value.ToString() == plaka)
                {
                    dataGridView1.Rows.Remove(row);
                    break;
                }
            }
        }
       private void icerik_Load(object sender, EventArgs e)
        {
            string[] iller = { "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Aksaray", "Amasya", "Ankara", "Antalya",
                "Ardahan", "Artvin", "Aydın", "Balıkesir", "Bartın", "Batman", "Bayburt", "Bilecik", "Bingöl", "Bitlis",
                "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Düzce", "Edirne",
                "Elazığ", "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkâri", "Hatay",
                "Iğdır", "Isparta", "İstanbul", "İzmir", "Kahramanmaraş", "Karabük", "Karaman", "Kars", "Kastamonu",
                "Kayseri", "Kilis", "Kırıkkale", "Kırklareli", "Kırşehir", "Kocaeli", "Konya", "Kütahya", "Malatya",
                "Manisa", "Mardin", "Mersin", "Muğla", "Muş", "Nevşehir", "Niğde", "Ordu", "Osmaniye", "Rize", "Sakarya",
                "Samsun", "Şanlıurfa", "Siirt", "Sinop", "Sivas", "Şırnak", "Tekirdağ", "Tokat", "Trabzon", "Tunceli",
                "Uşak", "Van", "Yalova", "Yozgat", "Zonguldak" };

            comboBox1.Items.AddRange(iller);

            VerileriYukle();

            Saatler(comboBox2);
            Saatler(comboBox3);

            comboBox2.SelectedIndexChanged += new EventHandler(comboBox_SelectionChanged);
            comboBox3.SelectedIndexChanged += new EventHandler(comboBox_SelectionChanged);
            dateTimePicker1.ValueChanged += new EventHandler(dateTimePicker_ValueChanged);
            dateTimePicker2.ValueChanged += new EventHandler(dateTimePicker_ValueChanged);
        }

        private void VerileriYukle(string konum = null)
        {
            try
            {                
                if (comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
                {                  
                    return;
                }

                DateTime alisTarihi = dateTimePicker1.Value.Date;
                DateTime teslimTarihi = dateTimePicker2.Value.Date;
                TimeSpan alisSaati = TimeSpan.Parse(comboBox2.SelectedItem.ToString());
                TimeSpan teslimSaati = TimeSpan.Parse(comboBox3.SelectedItem.ToString());

                DateTime alisZamani = alisTarihi + alisSaati;
                DateTime teslimZamani = teslimTarihi + teslimSaati;

                baglanti.Open();

                string query = @"
            SELECT Plaka, ResimYolu, Marka, Model
            FROM araclar
            WHERE Plaka NOT IN (
                SELECT plaka
                FROM takip
                WHERE 
                    (@alisZamani BETWEEN alisgun + alissaat AND teslimgun + teslimsaat OR @teslimZamani BETWEEN alisgun + alissaat AND teslimgun + teslimsaat) OR
                    (alisgun + alissaat BETWEEN @alisZamani AND @teslimZamani OR teslimgun + teslimsaat BETWEEN @alisZamani AND @teslimZamani)
            )";

                if (!string.IsNullOrEmpty(konum))
                {
                    query += " AND Konum = @Konum";
                }

                OleDbDataAdapter da = new OleDbDataAdapter(query, baglanti);
                da.SelectCommand.Parameters.AddWithValue("@alisZamani", alisZamani);
                da.SelectCommand.Parameters.AddWithValue("@teslimZamani", teslimZamani);
                if (!string.IsNullOrEmpty(konum))
                {
                    da.SelectCommand.Parameters.AddWithValue("@Konum", konum);
                }

                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.Columns.Clear();

                DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
                imageColumn.HeaderText = "";
                imageColumn.Name = "Resim";
                dataGridView1.Columns.Add(imageColumn);

                dataGridView1.Columns.Add("Plaka", "Plaka");
                dataGridView1.Columns.Add("Marka", "Marka");
                dataGridView1.Columns.Add("Model", "Model");

                dataGridView1.RowTemplate.Height = 200;
                dataGridView1.Columns["Resim"].Width = 250;
                dataGridView1.Columns["Plaka"].Visible = false;
                dataGridView1.Columns["Marka"].Width = 150;
                dataGridView1.Columns["Model"].Width = 150;

                foreach (DataRow row in dt.Rows)
                {
                    Image img = null;
                    if (File.Exists(row["ResimYolu"].ToString()))
                    {
                        img = Image.FromFile(row["ResimYolu"].ToString());
                    }
                    dataGridView1.Rows.Add(img, row["Plaka"], row["Marka"], row["Model"]);
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
        private void Saatler(ComboBox comboBox)
        {
            for (int hour = 0; hour < 24; hour++)
            {
                for (int minute = 0; minute < 60; minute += 30)
                {
                    comboBox.Items.Add(string.Format("{0:D2}:{1:D2}", hour, minute));
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string secilenIl = comboBox1.SelectedItem.ToString();
            VerileriYukle(secilenIl);
        }

        private bool Kontrol()
        {
            if (comboBox1.SelectedItem == null ||
                comboBox2.SelectedItem == null ||
                comboBox3.SelectedItem == null ||
                dateTimePicker1.Value == null ||
                dateTimePicker2.Value == null)
            {
                return false;
            }

            return true;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (!Kontrol())
                {
                    MessageBox.Show("Lütfen önce tarih ve konum bilgileri eksiksiz doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string plaka = dataGridView1.Rows[e.RowIndex].Cells["Plaka"].Value.ToString();
                AracDetay aracDetayForm = new AracDetay(this);
                aracDetayForm.BilgileriYukle(plaka);
                aracDetayForm.ShowDialog();
            }
        }

        private void btnsec_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string plaka = dataGridView1.CurrentRow.Cells["Plaka"].Value.ToString();
                AracDetay aracDetayForm = new AracDetay(this);
                aracDetayForm.BilgileriYukle(plaka);
                aracDetayForm.ShowDialog();
               
            }
        }

        private void btnkirala_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(secilenAracPlaka))
            {
                MessageBox.Show("Lütfen bir araç seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Kontrol())
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Aracı kiralamak istediğinize emin misiniz?", "Kiralama Onayı", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    baglanti.Open();
                    OleDbCommand komut = new OleDbCommand("INSERT INTO takip (ad, soyad, tc, plaka, alisgun, alissaat, teslimgun, teslimsaat) " +
                        "VALUES (@ad, @soyad, @tc, @plaka, @alisgun, @alissaat, @teslimgun, @teslimsaat)", baglanti);
                    komut.Parameters.AddWithValue("@ad", Form1.kullaniciAd);
                    komut.Parameters.AddWithValue("@soyad", Form1.kullaniciSoyad);
                    komut.Parameters.AddWithValue("@tc", Form1.kullaniciTC);
                    komut.Parameters.AddWithValue("@plaka", secilenAracPlaka);
                    komut.Parameters.AddWithValue("@alisgun", dateTimePicker1.Value.Date);
                    komut.Parameters.AddWithValue("@alissaat", comboBox2.SelectedItem.ToString());
                    komut.Parameters.AddWithValue("@teslimgun", dateTimePicker2.Value.Date);
                    komut.Parameters.AddWithValue("@teslimsaat", comboBox3.SelectedItem.ToString());

                    komut.ExecuteNonQuery();
                    baglanti.Close();

                    string alisBilgi = "Alış Tarihi: " + dateTimePicker1.Value.Date.ToString("dd/MM/yyyy") + " " + comboBox2.SelectedItem.ToString();
                    string teslimBilgi = "Teslim Tarihi: " + dateTimePicker2.Value.Date.ToString("dd/MM/yyyy") + " " + comboBox3.SelectedItem.ToString();
                    string mesaj = "Araç başarıyla kiralandı.\n" + "Plaka: " + secilenAracPlaka + "\n" + alisBilgi + "\n" + teslimBilgi;

                    MessageBox.Show(mesaj);

                    // Kiralanan aracı DataGridView'den kaldır
                    aracikaldir(secilenAracPlaka);

                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata: " + hata.Message);
                    if (baglanti.State == ConnectionState.Open)
                    {
                        baglanti.Close();
                    }
                }
            }
        }

        public void HesaplaFiyat()
        {

            if (comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
            {
                return;
            }

            try
            {
                baglanti.Open();
                string query = "SELECT Fiyat FROM araclar WHERE Plaka = @plaka";
                OleDbCommand komut = new OleDbCommand(query, baglanti);
                komut.Parameters.AddWithValue("@plaka", secilenAracPlaka);

                object result = komut.ExecuteScalar();
                if (result != null)
                {
                    decimal gunlukFiyat = Convert.ToDecimal(result);
                    DateTime alisTarihi = dateTimePicker1.Value.Date;
                    DateTime teslimTarihi = dateTimePicker2.Value.Date;

                    TimeSpan alisSaati = TimeSpan.Parse(comboBox2.SelectedItem.ToString());
                    TimeSpan teslimSaati = TimeSpan.Parse(comboBox3.SelectedItem.ToString());

                    TimeSpan kiralamaSuresi = teslimTarihi - alisTarihi;
                    int gunSayisi = kiralamaSuresi.Days;

                    if (teslimSaati > alisSaati)
                    {
                        gunSayisi += 1;
                    }

                    decimal toplamFiyat = gunlukFiyat * gunSayisi;
                    label7.Text = $" {toplamFiyat:C}";
                    label7.Visible = true; 
                }
                else
                {
                    MessageBox.Show("Araç fiyatı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void comboBox_SelectionChanged(object sender, EventArgs e)
        {
            VerileriYukle(comboBox1.SelectedItem?.ToString());
            HesaplaFiyat();
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            VerileriYukle(comboBox1.SelectedItem?.ToString());
            HesaplaFiyat();
        }
    }
}

