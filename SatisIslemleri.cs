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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq.Expressions;

namespace proje1
{
    public partial class SatisIslemleri : Form
    { // SQL BAĞLANTISI OLUŞTURMA 
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=urundata;Integrated Security=True");
        SqlCommand komut;
        SqlDataAdapter da;
        public SatisIslemleri()
        {
            InitializeComponent();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit(); // ÇIKIŞ
        }

        private void anaMenüyeDönToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ANA MENÜYE DÖNME 
            SatisIslemleri sislem = new SatisIslemleri();
            sislem.Close();
            AnaMenu goster = new AnaMenu();
            goster.Show();
            this.Hide();
        }
        public void temizle()
        {
            // METİN KUTULARINI TEMİZLEME KODU
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        public void satisyap()
        { // ÜRÜN TABLOSUNDAN SATIŞ YAPTIRAN VE BU SATIŞLARI STOKTAN DÜŞEN VE BU SATIŞLARI SATIŞRAPOR TABLOSUNDA GÖSTEREN KODLAR
            try
            {
                baglanti.Open();

                int satisAdet, stokAdet, yeniStokAdet;
                satisAdet = Convert.ToInt16(textBox6.Text);

                int stokadet;
                stokadet = Convert.ToInt16(textBox5.Text);

                if (satisAdet > stokadet)
                {
                    MessageBox.Show("Stokta yeterli ürün bulunmamaktadır.");
                }
                else
                {
                    yeniStokAdet = stokadet - satisAdet;
                    //sts1 tbl ye satış ekledim
                    string ekle = "INSERT INTO sts1(urunkodu, urunadi, urunfiyati, adet) VALUES (@urunkodu, @urunadi, @urunfiyati, @adet)";
                    SqlCommand komut = new SqlCommand(ekle, baglanti);
                    komut.Parameters.AddWithValue("@urunkodu", textBox2.Text);
                    komut.Parameters.AddWithValue("@urunadi", textBox3.Text);
                    komut.Parameters.AddWithValue("@urunfiyati", textBox7.Text);
                    komut.Parameters.AddWithValue("@adet", textBox6.Text);
                    komut.ExecuteNonQuery();

                    //urun tbl deki stokları güncelledim

                    string stokGuncelle = "UPDATE uruntbl SET adet = @yeniAdet WHERE urunkodu = @urunkodu";
                    SqlCommand guncelleKomut = new SqlCommand(stokGuncelle, baglanti);
                    guncelleKomut.Parameters.AddWithValue("@urunkodu", textBox2.Text);
                    guncelleKomut.Parameters.AddWithValue("@yeniAdet", yeniStokAdet);
                    guncelleKomut.ExecuteNonQuery();

                    MessageBox.Show("SATIŞ İŞLEMİ BAŞARILI");


                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            baglanti.Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void SatisIslemleri_Load(object sender, EventArgs e)
        {
            this.uruntblTableAdapter.Fill(this.urundataDataSet.uruntbl);
            satisgetir(); // METODUMUZU ÇAĞIRIYORUZ


        }
        void satisgetir()
        {
            // URUN TABLOSUNDAKİ VERİLERİ GETİREN METOD
            baglanti.Open();
            da = new SqlDataAdapter("Select * from uruntbl", baglanti);
            DataTable uruntbl = new DataTable();
            da.Fill(uruntbl);
            dataGridView1.DataSource = uruntbl;
            baglanti.Close();
        }

        private void SatisIslemleri_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // ÇIKIŞ

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string sorgu = "Select * from uruntbl where urunadi Like '%" + textBox1.Text + "%' ";
            SqlDataAdapter adap = new SqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            adap.Fill(ds, "uruntbl");
            this.dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            // DOUBLE CLİCK OLAYI İLE METİN KUTULARINA VERİ ÇEKME 
            textBox2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            comboBox2.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            satisyap(); // METODUMUZU ÇAĞIRIYORUZ
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void button3_Click(object sender, EventArgs e)
        {
            // METİN KUTULARINI TEMİZLEME KODU
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox6.Text))
            {
                if (int.TryParse(textBox6.Text, out int adet))
                {
                    // ADET FİYAT HESABI YAPIYORUZ 
                    // TEXTBOX 6 YA GÖRE 7 DEĞİŞİYOR 
                    try
                    {
                        double fiyat = Convert.ToDouble(textBox4.Text);
                        double toplamFiyat = adet * fiyat;
                        textBox7.Text = toplamFiyat.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen sadece sayı giriniz.");
                    textBox6.Text = ""; // Sayı dışında bir şey girildiğinde textBox'ı temizle
                }

            }
        }
    }
}
