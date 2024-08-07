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

namespace proje1
{
    public partial class TedarikciIslem : Form
    {
        // SQL BAĞLANTISI 
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=urundata;Integrated Security=True");
        SqlCommand komut;
        SqlDataAdapter da;
        public TedarikciIslem()
        {
            InitializeComponent();
        }

        private void anaMenüyeDönToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ANA MENÜYE DÖNME KODU
            TedarikciIslem tislem = new TedarikciIslem();
            tislem.Close();
            AnaMenu goster = new AnaMenu();
            goster.Show();
            this.Hide();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ÇIKIŞ
            Application.Exit();
        }

        private void TedarikciIslem_Load(object sender, EventArgs e)
        {
            tedarikcigetir();
        }
        void tedarikcigetir()
        { // TEDARİKÇİ TABLOSUNDAKİ VERİLERİ GETİREN METOD
            baglanti.Open();
            da = new SqlDataAdapter("Select * from tedarikcitbl", baglanti);
            DataTable tedarikcitbl = new DataTable();
            da.Fill(tedarikcitbl);
            dataGridView1.DataSource = tedarikcitbl;
            baglanti.Close();
        }

        private void TedarikciIslem_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ÇIKIŞ
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TEDARİKÇİ TABLOSUNDAKİ VERİLERİ ARAMA KODU
            try
            {
                baglanti.Open();
                string sorgu = "SELECT * FROM tedarikcitbl WHERE " +
               "tedarikciid LIKE '%" + textBox3.Text + "%' AND " +
               "tedarikciadi LIKE '%" + textBox1.Text + "%' AND " +
                "tedarikcimail LIKE '%" + textBox4.Text + "%' AND " +
               "tedarikciadres LIKE '%" + textBox2.Text + "%'  ";

                //string sorgu = "Select * from tedarikcitbl where tedarikciadi Like '%" + textBox1.Text + "%' ";
                SqlDataAdapter adap = new SqlDataAdapter(sorgu, baglanti);
                DataSet ds = new DataSet();
                adap.Fill(ds, "tedarikcitbl");
                this.dataGridView1.DataSource = ds.Tables[0];
                baglanti.Close();
            }
            catch (Exception err) { MessageBox.Show(err.Message); }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //  TEXTBOXLARI TEMİZLEME 
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            maskedTextBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // TEDARİKÇİ TABLOSUNA VERİ EKLEME KODU
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(maskedTextBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
                { // TEXTBOXLARIN BOŞ OLUP OLMADIĞININ KONTROLÜ
                    MessageBox.Show("Veri Girişi Yapınız!..");
                }
                else
                {
                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();
                    string kayit = "insert into tedarikcitbl(tedarikciid,tedarikciadi,tedarikcitel,tedarikciadres,tedarikcimail) values (@tedarikciid,@tedarikciadi,@tedarikcitel,@tedarikciadres,@tedarikcimail)";
                    SqlCommand komut = new SqlCommand(kayit, baglanti);
                    komut.Parameters.AddWithValue("@tedarikciadi", textBox1.Text);
                    komut.Parameters.AddWithValue("@tedarikcitel", maskedTextBox1.Text);
                    komut.Parameters.AddWithValue("@tedarikciadres", textBox2.Text);
                    komut.Parameters.AddWithValue("@tedarikciid", textBox3.Text);
                    komut.Parameters.AddWithValue("@tedarikcimail", textBox4.Text);

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
            // TEDARİKÇİ TABLOSUNDAKİ VERİLERİ SİLME KODU 
            try
            {
                baglanti.Open();
                DialogResult result = MessageBox.Show("Ürünü silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string silme = ("DELETE FROM tedarikcitbl WHERE tedarikciadi = @Tedarikciadi");
                    SqlCommand komut = new SqlCommand(silme, baglanti);
                    komut.Parameters.AddWithValue("@Tedarikciadi", textBox1.Text);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Silme İşlemi Başarılı.");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    maskedTextBox1.Text = "";
                }
                baglanti.Close();



            }
            catch (Exception err) { MessageBox.Show(err.Message); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // TEDARİKÇİ TABLOSUNDAKİ VERİLERİ GÜNCELLEME KDOU
            try
            {
                if (textBox1.Text == dataGridView1.CurrentRow.Cells[1].Value.ToString())
                {
                    MessageBox.Show("Veri aynı!!!");
                }
                else
                {
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        DialogResult result = MessageBox.Show("Ürünü Güncellemek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            baglanti.Open();
                            string güncelle = ("update tedarikcitbl set tedarikciadi='" + textBox1.Text + "',tedarikcitel='" + maskedTextBox1.Text + "',tedarikcimail='" + textBox4.Text + "',tedarikciadres='" + textBox2.Text + "' where tedarikciid=" + textBox3.Text + " ");
                            SqlCommand komut = new SqlCommand(güncelle, baglanti);
                            komut.Parameters.AddWithValue("@TedarikciAdi", textBox1.Text);
                            komut.ExecuteNonQuery();
                            MessageBox.Show("Güncelleme işlemi başarılı.");
                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox3.Text = "";
                            textBox4.Text = "";
                            maskedTextBox1.Text = "";
                        }
                        baglanti.Close();


                    }
                }
            }
            catch (Exception err) { MessageBox.Show(err.Message); }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            // DOUBLE CLİCK İLE DATAGRİDVİEW DEKİ VERİLERİ TEXTBOXLARA ÇEKME
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            //  maskedTextBox1.Text = dataGridView1.CurrentRow.Cells[2].Value?.ToString() ?? "";
        }
    }
}
