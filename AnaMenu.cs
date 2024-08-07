using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proje1
{
    public partial class AnaMenu : Form
    {
        public AnaMenu()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit(); //ÇIKIŞ BUTONU 

        }

        private void AnaMenu_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UrunIslem goster = new UrunIslem();
            goster.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AnaMenu anaMenu = new AnaMenu();
            MarkaIslem goster = new MarkaIslem();
            goster.Show();
            this.Hide();
            anaMenu.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AnaMenu anaMenu = new AnaMenu();
            TedarikciIslem goster = new TedarikciIslem();
            goster.Show();
            this.Hide();
            anaMenu.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AnaMenu anaMenu = new AnaMenu();
            SatisIslemleri goster = new SatisIslemleri();
            goster.Show();
            this.Hide();
            anaMenu.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AnaMenu anaMenu = new AnaMenu();
            SatisRapor goster = new SatisRapor();
            goster.Show();
            this.Hide();
            anaMenu.Close();
        }

        private void AnaMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            // FORM KAPATMA
            Application.Exit();
        }

        private void kULLANICIGİRİŞİNEDÖNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Giriş goster = new Giriş();     //KULLANICI LOGİN EKRANINA DÖN
            goster.Show();
            this.Hide();
        }
    }
}
