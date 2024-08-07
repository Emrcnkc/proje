using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.OleDb;
using System.Linq.Expressions;

namespace proje1
{
    public partial class MarkaIslem : Form
    {
        //  SQL NESNESİ OLUŞTURULUYOR 
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=urundata;Integrated Security=True");
        SqlCommand komut;
        SqlDataAdapter da;
        public MarkaIslem()
        {
            InitializeComponent();
        }

        private void MarkaIslem_Load(object sender, EventArgs e)
        {
            markagetir();
        }
        void markagetir()  // MARKA TABLOSUNDAKİ VERİLERİ GETİRİYOR
        {
            baglanti.Open(); // BAĞLANTIYI AÇIYORUZ
            da = new SqlDataAdapter("Select * from markatbl", baglanti); // MARKA TBL DEKİ VERİLERİ ÇEKİYORUZ
            DataTable markatbl = new DataTable();
            da.Fill(markatbl);
            dataGridView1.DataSource = markatbl; // DATAGRİDVİEW E VERİLERİ ATIYORUZ
            baglanti.Close(); //    BAĞLANTIYI KAPATIYORUZ
        }

        private void anaMenüyeDönToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  ANA MENÜYE DÖNME 
            MarkaIslem mrk = new MarkaIslem();
            mrk.Close();        //  MARKA  İŞLEM FORMUNU KAPAT
            //  ANA MENÜ FORMUNU GÖSTER
            AnaMenu goster = new AnaMenu();
            goster.Show();
            this.Hide();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ÇIKIŞ 
            Application.Exit();
        }

        private void MarkaIslem_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ÇIKIŞ
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // MARKA TABLOSUNDAKİ VERİLERİ MARKA ADINA VEYA İD SİNE GÖRE ARAMA
            try
            {
                string sorgu = "SELECT * FROM markatbl WHERE markaadi LIKE '%" + textBox1.Text + "%' AND markaid LIKE '%" + textBox2.Text + "%'";

                baglanti.Open();
                SqlDataAdapter adap = new SqlDataAdapter(sorgu, baglanti);
                DataSet ds = new DataSet();
                adap.Fill(ds, "markatbl");
                this.dataGridView1.DataSource = ds.Tables[0];
                baglanti.Close();
            }
            catch (Exception err) { MessageBox.Show(err.Message); }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = ""; textBox2.Text = "";  //  TEXTBOXLARI TEMİZLEME KODU
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {     // TextBox'ların boş olup olmadığını kontrol et

                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("Veri Girişi Yapınız!..");
                }
                else
                {   // Bağlantı kapalıysa aç
                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();
                    // MARKA TBL YE VERİ EKLEME KODU 
                    string kayit = "INSERT INTO markatbl (markaid, markaadi) VALUES (@markaid, @markaadi)";
                    SqlCommand komut = new SqlCommand(kayit, baglanti);
                    komut.Parameters.AddWithValue("@markaadi", textBox1.Text);
                    komut.Parameters.AddWithValue("@markaid", textBox2.Text);
                    try
                    {
                        int affectedRows = komut.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Kayıt İşlemi Gerçekleşti !!!");
                        }
                        else
                        {
                            MessageBox.Show("Bu veri zaten mevcut. Aynı veriyi ekleyemezsiniz!");
                        }
                    }
                    catch (SqlException err)
                    {
                        if (err.Number == 2627) // Hata kodu 2627, primary key hatasını temsil eder
                        {
                            MessageBox.Show("Bu marka ID zaten kullanılıyor. Lütfen farklı bir marka ID girin.");
                        }
                        else
                        {
                            MessageBox.Show("Bilinmeyen bir hata oluştu: " + err.Message);
                        }
                    }

                    baglanti.Close();
                }
            }

            catch (Exception err)
            {
                MessageBox.Show("İşlem Sırasında Hata Meydana Geldi: " + err.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                { MessageBox.Show("Veri Girişi Yapınız!.."); }
                else
                {
                    baglanti.Open();
                    DialogResult result = MessageBox.Show("Ürünü silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        string silme = "DELETE FROM markatbl WHERE markaadi = @MarkaAdi";
                        SqlCommand komut = new SqlCommand(silme, baglanti);
                        komut.Parameters.AddWithValue("@MarkaAdi", textBox1.Text);

                        komut.ExecuteNonQuery();

                        MessageBox.Show("Silme İşlemi Başarılı.");
                        textBox1.Text = "";
                        textBox2.Text = "";
                    }
                    baglanti.Close();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Hata oluştu: " + err.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // MARKA TBL DEKİ VERİLERİ GÜNCELLEME KODU
            try
            {
                if (textBox1.Text == dataGridView1.CurrentRow.Cells[1].Value.ToString())
                {
                    MessageBox.Show("veri aynı!!!");

                }
                else
                {
                    baglanti.Open();
                    DialogResult result = MessageBox.Show("Ürünü Güncellemek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        string güncelle = ("update markatbl set markaadi = '" + textBox1.Text + "' where markaid=" + textBox2.Text + " ");
                        SqlCommand komut = new SqlCommand(güncelle, baglanti);
                        komut.Parameters.AddWithValue("@MarkaAdi", textBox1.Text);
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show("Güncelleme işlemi başarılı.");
                        textBox1.Text = "";
                        textBox2.Text = "";
                    }
                    baglanti.Close();

                }
            }
            catch (Exception err) { MessageBox.Show(err.Message); }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString(); // DATAGRİDVİEWDEKİ VERİYE DOUBLE CLİCK YAPINCA VERİYİ TEXTBOX A ÇEKME
            textBox2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); // DATAGRİDVİEWDEKİ VERİYE DOUBLE CLİCK YAPINCA VERİYİ TEXTBOX A ÇEKM
        }
    }
}
