using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Windows.Forms;

namespace proje1
{
    public partial class Giriş : Form
    {     // VERİTABANI BAĞLANTI NESNEMİZ
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=urundata;Integrated Security=True");
        SqlCommand komut;
        SqlDataAdapter da;
        Timer timer = new Timer();
        int kalanSure = 15; //  TİMER 1 İ 15 SANİYEDEN BAŞLATIYORUZ
        public Giriş()
        {
            InitializeComponent();
            timer1.Interval = 1000;  // PERİYODU 1 SANİYE OLARAK AYARLIYORUZ 
            timer1.Enabled = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // ÇIKIŞ BUTONU
            Application.Exit();
        }

        void kullanicigetir() //KULLANICI BİLGİLERİNİ GETİRME METODU
        {
            try
            {
                // VERİTABANI BAĞLANTISINI AÇ
                baglanti.Open();

                // VERİTABANINDAN KULLANICI BİLGİLERİNİ GETİREN SORGU            
                da = new SqlDataAdapter("SELECT * FROM kullanicitbl", baglanti);

                // DATATABLE OLUŞTUR 
                DataTable kullaniciTable = new DataTable();

                // KULLANICI BİLGİLERİNİ DATATABLEA DOLDUR
                da.Fill(kullaniciTable);
            }
            catch (Exception err)
            {       // HATA KONTROLÜ 
                MessageBox.Show("Kullanıcı bilgilerini getirme sırasında bir hata oluştu: " + err.Message);
            }
            finally
            {
                // Veritabanı bağlantısını kapat  
                baglanti.Close();
            }
        }

        private void Giriş_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e) // ENTER TUŞUNA BASILDIĞINDA GİRİŞ BUTONUNU ÇAĞIR
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)// GİRİŞ BUTONU
        {
            string kadi = textBox1.Text;
            string sifre = textBox2.Text;
            try
            {
                if (string.IsNullOrEmpty(sifre) && string.IsNullOrEmpty(kadi)) //TEXTBOXLARI KONTROL EDİYORUZ BOŞ OLUP OLMADIĞINI  
                {
                    MessageBox.Show("Kullanıcı adı ve şifre alanları boş bırakılamaz.");
                    return;
                }
                else if (string.IsNullOrEmpty(sifre) || string.IsNullOrEmpty(kadi))// ŞİFRE VEYA KULLANICIADI NA GÖRE MESAJ YOLLUYORUZ
                {
                    MessageBox.Show(string.IsNullOrEmpty(sifre) ? "Şifre alanı boş bırakılamaz." : "Kullanıcı Adı boş olamaz!");
                    return;
                }
                else
                {
                    baglanti.Open(); //BAĞLANTI AÇIYORUZ
                    SqlCommand komut = new SqlCommand(" select * From kullanicitbl where kullaniciadi= '" + @kadi + "' and sifre='" + @sifre + "'", baglanti);//SQL KOMUT SORGUSU VE PARAMETRE EKLİYORUZ
                    komut.Parameters.AddWithValue("@kadi", kadi);
                    komut.Parameters.AddWithValue("@sifre", sifre);
                    SqlDataReader oku = komut.ExecuteReader(); // SQLDATAREADER İLE SORGUYU ÇALIŞTIR VE OKU
                    if (oku.Read())
                    {
                        MessageBox.Show("Hoşgeldiniz " + kadi + "");
                        AnaMenu goster = new AnaMenu();
                        goster.Show();
                        this.Hide();

                    }
                    else
                    {
                        //HATALI GİRİŞ
                        MessageBox.Show("Hatalı Kullanıcı Adı Veya Şifre...");
                    }

                  
                }
            }
            catch (Exception err)
            {
                // Hata durumunda hata mesajını göster
                MessageBox.Show("Bir hata oluştu: " + err.Message);
            }
            baglanti.Close();
        }

        private void Giriş_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ÇIKIŞ YAPTIR
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                // Kalan süreyi bir saniye azalt
                kalanSure--;
                if (kalanSure >= 0)
                {


                    // Label1 üzerinde kalan süreyi göster
                    label3.Text = kalanSure.ToString() + "  ";
                }
                else
                {
                    // Süre tamamlandıysa timer'ı durdur ve uygulamayı kapat
                    timer.Stop();
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);

            }

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text) && !int.TryParse(textBox2.Text, out _)) //PASSWORD A SADECE İNT
            {
                MessageBox.Show("Şifre sadece sayısal bir değer içermelidir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Clear();
            }
        }
    }
}
