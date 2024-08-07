using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.Common;
using System.Data.OleDb;
using System.Data;
 

namespace proje1
{
    public partial class SatisRapor : Form
    { //  SQL BAĞLANTISI 
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=urundata;Integrated Security=True");
        SqlCommand komut;
        SqlDataAdapter da;
        public SatisRapor()
        {
            InitializeComponent();
        }

        private void anaMenüyeDönToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  ANA MENÜYE DÖN
            SatisRapor srapor = new SatisRapor();
            srapor.Close();
            AnaMenu goster = new AnaMenu();
            goster.Show();
            this.Hide();
        }

        private void SatisRapor_Load(object sender, EventArgs e)
        {
            satisgetir();  // SATIŞGETİR METODU ÇAĞRILIYOR
        }
        void satisgetir() // SATIŞRAPOR TABLOSUNDAKİ VERİLERİ GETİRİYOR
        {

            baglanti.Open();
            da = new SqlDataAdapter("Select * from sts1", baglanti);
            DataTable sts1 = new DataTable();
            da.Fill(sts1);
            sts1.DefaultView.Sort = "satistarihi DESC";  // DATAGRİDVİEW İ YENİDEN ESKİYE DOĞRU SIRALAMA 

            dataGridView1.DataSource = sts1;
            baglanti.Close();
        }
        void satisara() // SATIŞ ARAMA METODU ÜRÜN ADINA GÖRE 
        {
            baglanti.Open();
            string sorgu = "Select * from sts1 where urunadi Like '%" + textBox1.Text + "%' ";
            SqlDataAdapter adap = new SqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            adap.Fill(ds, "sts1");
            this.dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();
        }


        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();// ÇIKIŞ 
        }

        private void SatisRapor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // ÇIKIŞ 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //  YAZDIRMA İŞLEMLERİ VE ÖNBASKI 
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
            printPreviewDialog.Document = printDocument1;
            printPreviewDialog.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            satisara(); // ÜRÜN ADINA GÖRE SATIŞ ARAMA KODU
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            // DATAGRİDVİEW DOUBLE CLİCK OLAYI TEXTBOX A VERİ GELİYOR
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // PRİNTDOCUMENT1 İÇİN PRİNTPAGE OLAYI
            Bitmap bm = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
            dataGridView1.DrawToBitmap(bm, new System.Drawing.Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));
            int x = (e.PageBounds.Width - bm.Width) / 2;
            int y = 45;  

            e.Graphics.DrawImage(bm, x, y);
         
       
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // TARİH ARALIĞINA GÖRE ARAMA YAPTIRMA KODU
            try
            {
                baglanti.Open();
                DateTime baslangicTarihi = dateTimePicker1.Value;
                DateTime bitisTarihi = dateTimePicker2.Value;
                string sorgu = "SELECT * FROM sts1 WHERE satistarihi BETWEEN @BaslangicTarihi AND @BitisTarihi";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@BaslangicTarihi", baslangicTarihi);
                komut.Parameters.AddWithValue("@BitisTarihi", bitisTarihi);

                using (SqlDataReader reader = komut.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        dataGridView1.DataSource = dataTable;
                    }
                    else
                    {
                        MessageBox.Show("Belirtilen tarih aralığında kayıt bulunamadı.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {   //  BAĞLANTI KAPATIYORUZ 
                baglanti.Close();
            }
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }
    }
}
