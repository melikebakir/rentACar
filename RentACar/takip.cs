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
    public partial class takip : Form
    {
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\monster\\Desktop\\kullanici.mdb");
        public takip()
        {
            InitializeComponent();
        }

        void listele()
        {
            baglanti.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter adtr = new OleDbDataAdapter("select * from takip order by teslimgun",baglanti);
            adtr.Fill(ds,"okunan veri");
            dataGridView1.DataSource = ds.Tables["okunan veri"];
            baglanti.Close();

        }
        private void takip_Load(object sender, EventArgs e)
        {
            listele();
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            menu m = new menu();
            m.Show();
            this.Close();
        }
    }
}
