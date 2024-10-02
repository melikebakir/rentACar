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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RentACar
{
    public partial class admgiris : Form
    {
        public admgiris()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\monster\\Desktop\\kullanici.mdb");
                baglanti.Open();
                OleDbCommand sorgu = new OleDbCommand("select email,sifre from admin where email=@mail and sifre=@pass", baglanti);
                sorgu.Parameters.AddWithValue("@mail", textBox1.Text);
                sorgu.Parameters.AddWithValue("@pass", textBox2.Text);
            
                OleDbDataReader dr;
                dr = sorgu.ExecuteReader();

                if (dr.Read())
                {
                    menu m = new menu();
                    m.Show();
                    this.Visible = false;

                }
                else
                {
                    baglanti.Close();
                    MessageBox.Show("Yönetici bilgisi bulunamadı..");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                textBox1.Clear();
                textBox2.Clear();
                
            }
        }
    }
}
