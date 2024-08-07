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
using System.Data;
using System.Data.SqlClient;

namespace proje1
{
    public partial class UrunIslem : Form
    { // SQL BAĞLANTISI OLUŞTURMA
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=urundata;Integrated Security=True");
        SqlCommand komut;
        SqlDataAdapter da;
        public UrunIslem()
        {
            InitializeComponent();
        }

        SqlConnection baglan = new SqlConnection("DATA SOURCE=LAPTOP-K5D6LQPM\\TEW_SQLEXPRESS");

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {   // ÜRÜN TABLOSUNA VERİ EKLEME KODU
            try
            {    // TextBox'ların boş olup olmadığını kontrol et
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("Veri Girişi Yapınız!..");
                }
                else
                {
                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();
                    string kayit = "INSERT INTO uruntbl(urunkodu, urunadi, urunfiyati, adet, marka, tedarikci) VALUES (@urunkodu, @urunadi, @urunfiyati, @adet, @marka, @tedarikci)";
                    SqlCommand komut = new SqlCommand(kayit, baglanti);
                    komut.Parameters.AddWithValue("@urunkodu", textBox1.Text);
                    komut.Parameters.AddWithValue("@urunadi", textBox2.Text);
                    komut.Parameters.AddWithValue("@urunfiyati", textBox3.Text);
                    komut.Parameters.AddWithValue("@adet", textBox4.Text);
                    komut.Parameters.AddWithValue("@marka", comboBox1.Text);
                    komut.Parameters.AddWithValue("@tedarikci", comboBox2.Text);
             

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

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit(); // ÇIKIŞ
        }

        private void anamenüToolStripMenuItem_Click(object sender, EventArgs e)
        {   // ANAMENÜYE DÖNME KODU
            UrunIslem urnislem = new UrunIslem();
            urnislem.Close();
            AnaMenu goster = new AnaMenu();
            goster.Show();
            this.Hide();

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void UrunIslem_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'urundataDataSet1.uruntbl' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.uruntblTableAdapter1.Fill(this.urundataDataSet1.uruntbl);
            urungetir();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        void urungetir()
        {
            // ÜRÜN TABLOSUNDAKİ VERİLERİ GETİRME KODU
            baglanti.Open();
            da = new SqlDataAdapter("Select * from uruntbl", baglanti);
            DataTable uruntbl = new DataTable();
            da.Fill(uruntbl);
            dataGridView1.DataSource = uruntbl;
            baglanti.Close();
        }

        private void UrunIslem_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // ÇIKIŞ

        }

        private void button8_Click(object sender, EventArgs e)
        {   // ÜRÜM TABLOSUNDAKİ VERİLERİ GÜNCELLEME KODU

            try
            {
                if (textBox2.Text == dataGridView1.CurrentRow.Cells[1].Value.ToString())
                {
                    MessageBox.Show("veri aynı!!!");
                }
                else
                {
                    DialogResult result = MessageBox.Show("Ürünü Güncellemek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        if (baglanti.State == ConnectionState.Closed)
                            baglanti.Open();
                        string güncelle = ("update uruntbl set urunadi = '" + textBox2.Text + "',urunfiyati='" + textBox3.Text + "',adet='" + textBox4.Text + "',marka='" + comboBox1.Text + "',tedarikci='" + comboBox2.Text + "'  where urunkodu= " + textBox1.Text + " ");
                        //string güncelle = ("update tedarikcitbl set tedarikciadi = '" + textBox1.Text + "', where markaid=" + textBox2.Text + " ");
                        SqlCommand komut = new SqlCommand(güncelle, baglanti);
                        komut.Parameters.AddWithValue("@UrunAdi", textBox2.Text);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Güncelleme işlemi başarılı.");
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        comboBox1.Text = "";
                        comboBox2.Text = "";
                    }
                    baglanti.Close();

                }
            }
            catch
            {
                MessageBox.Show("İşlem Sırasında Hata Meydana Geldi Tekrar Deneyiniz !!!");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        { // ÜRÜN TABLOSUNDAKİ VERİLERİ SİLME KODU
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                { MessageBox.Show("Veri Girişi Yapınız!.."); }
                else
                {
                    DialogResult result = MessageBox.Show("Ürünü silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        if (baglanti.State == ConnectionState.Closed)

                            baglanti.Open();

                        string silme = "DELETE FROM uruntbl WHERE urunadi = @urunadi";

                        SqlCommand komut = new SqlCommand(silme, baglanti);
                        komut.Parameters.AddWithValue("@urunadi", textBox2.Text);

                        komut.ExecuteNonQuery();


                        MessageBox.Show("Silme İşlemi Başarılı.");
                    }
                    baglanti.Close();

                }

            }
            catch (Exception err)
            {
                MessageBox.Show("İşlem Sırasında Hata Meydana Geldi: " + err.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {       // ÜRÜN TABLOSUNDAKİ ÜRÜNLERİ ARAMA KODU
            try
            {
                baglanti.Open();
                string sorgu = "SELECT * FROM uruntbl WHERE " +
                           "urunadi LIKE '%" + textBox2.Text + "%' AND " +
                           "urunkodu LIKE '%" + textBox1.Text + "%' AND " +
                           "urunfiyati LIKE '%" + textBox3.Text + "%' AND " +
                           "adet LIKE '%" + textBox4.Text + "%' AND " +
                           "marka LIKE '%" + comboBox1.Text + "%' AND " +
                           "tedarikci LIKE '%" + comboBox2.Text + "%'";

                //  string sorgu = "Select * from uruntbl where urunadi Like '%" + textBox2.Text + "%' ";
                SqlDataAdapter adap = new SqlDataAdapter(sorgu, baglanti);
                DataSet ds = new DataSet();
                adap.Fill(ds, "uruntbl");
                this.dataGridView1.DataSource = ds.Tables[0];
                baglanti.Close();
            }
            catch (Exception err) { MessageBox.Show(err.Message); }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // DOUBLECLİCK OLAYI İLE METİN KUTULARINA VERİ ÇEKME
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            comboBox2.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();


        }

        private void button2_Click(object sender, EventArgs e)
        {   // METİN KUTULARINI TEMİZLEME KODU
            textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = "";
            comboBox1.Text = ""; comboBox2.Text = "";




        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
        }

        private void dataGridView1_DoubleClick_1(object sender, EventArgs e)
        {

            // DOUBLECLİCK OLAYI İLE METİN KUTULARINA VERİ ÇEKME
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            comboBox2.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        }
    }

}
