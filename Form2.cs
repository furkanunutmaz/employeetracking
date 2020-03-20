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
using System.Text.RegularExpressions; // güvenli parola oluşturma kütüphanesi
using System.IO; // input output klasör işlemleri için kullanılır

namespace Personal_Takip_Programı
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SqlConnection baglan = new SqlConnection("Data Source=DESKTOP-6RUFCV1;Initial Catalog=personal;Integrated Security=True");

        private void kullanicilari_goster(string veriler)
        {
            SqlDataAdapter da = new SqlDataAdapter(veriler, baglan);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

        }
        private void personelleri_goster(string veriler)
        {
            SqlDataAdapter da = new SqlDataAdapter(veriler, baglan);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];//0. indisten itibaren doldur demek

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            kullanicilari_goster("select * from kullanicilar");
            personelleri_goster("select*from isciler");
            try
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\" + Form1.tcno + ".jpg");
            }
            catch (Exception)
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\resimyok.png");
            }
            //PERSONEL İŞLEMLERİ SEKMESİNİN DÜZENLENMESİ
            label10.Text = Form1.ad + " " + Form1.soyad;
            textBox1.MaxLength = 11;
            textBox2.MaxLength = 15;
            textBox3.MaxLength = 15;
            radioButton1.Checked = true;
            textBox4.MaxLength = 16;
            textBox5.MaxLength = 16;
            textBox6.MaxLength = 16;
            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox3.CharacterCasing = CharacterCasing.Upper;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            toolTip1.SetToolTip(this.textBox1, "11 haneli TC Kimlik Numaranızı giriniz");
            //KULLANICI İŞLEMLERİ SEKMESİNİN DÜZENLENMESİ
            radioButton3.Checked = true;
            maskedTextBox1.Mask = "00000000000";
            maskedTextBox2.Mask = "LL??????????????";
            maskedTextBox3.Mask = "LL??????????????";
            maskedTextBox4.Mask = "00000";
            maskedTextBox2.Text.ToUpper();
            maskedTextBox3.Text.ToUpper();

            DateTime zaman = DateTime.Now;
            int yıl = int.Parse(zaman.ToString("yyyy"));
            int ay = int.Parse(zaman.ToString("MM"));
            int gun = int.Parse(zaman.ToString("dd"));

            dateTimePicker1.MinDate = new DateTime(1960, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(yıl - 18, ay, gun);
            dateTimePicker1.Format = DateTimePickerFormat.Short;


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 11)
            {
                errorProvider1.SetError(textBox1, "Lütfen 11 hane giriniz");
            }
            else
            {
                errorProvider1.Clear();
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar >= 48 && (int)e.KeyChar <= 58 || (int)e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == true ||
                char.IsControl(e.KeyChar) == true ||
                char.IsSeparator(e.KeyChar) == true)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == true ||
                char.IsControl(e.KeyChar) == true ||
                char.IsSeparator(e.KeyChar) == true)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.MaxLength < 8)
            {
                errorProvider1.SetError(textBox4, "Kullanıcı adı en az 8 haneli olmalıdır.");
            }
            else
            {
                errorProvider1.Clear();
            }
        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == true ||
                char.IsControl(e.KeyChar) == true ||
                char.IsDigit(e.KeyChar) == true)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        int parola_skoru = 0;

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {
            string parola_seviyesi = "";
            int kucuk_harf_skoru = 0, buyuk_harf_skoru = 0, rakam_skoru = 0, sembol_skoru = 0;

            string sifre = textBox5.Text;
            string duzeltilmis_sifre = "";
            duzeltilmis_sifre = sifre;
            //
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('İ', 'I');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ı', 'i');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ö', 'O');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ö', 'o');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ü', 'U');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ü', 'u');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ğ', 'G');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ğ', 'g');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ç', 'C');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ç', 'c');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ş', 'S');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ş', 's');
            //
            if (sifre != duzeltilmis_sifre)
            {
                sifre = duzeltilmis_sifre;
                textBox5.Text = sifre;
                MessageBox.Show("Şifredeki Türkçe karakterler İngilizce karakterlere dönüştürülmüştür.");
            }
            //
            int az_karakter_sayisi = sifre.Length - Regex.Replace(sifre, "[a-z]", "").Length;
            kucuk_harf_skoru = Math.Min(2, az_karakter_sayisi) * 10;

            int AZ_karakter_sayisi = sifre.Length - Regex.Replace(sifre, "[A-Z]", "").Length;
            buyuk_harf_skoru = Math.Min(2, AZ_karakter_sayisi) * 10;

            int rakam_sayisi = sifre.Length - Regex.Replace(sifre, "[0-9]", "").Length;
            rakam_skoru = Math.Min(2, rakam_sayisi) * 10;

            int sembol_sayisi = sifre.Length - az_karakter_sayisi - AZ_karakter_sayisi - rakam_sayisi;
            sembol_skoru = Math.Min(2, sembol_sayisi) * 10;

            parola_skoru = sembol_skoru + buyuk_harf_skoru + kucuk_harf_skoru + rakam_skoru;

            if (sifre.Length == 9)
            {
                parola_skoru += 10;
            }
            if (sifre.Length == 10)
            {
                parola_skoru += 20;
            }

            if (kucuk_harf_skoru == 0 || buyuk_harf_skoru == 0 ||
                rakam_skoru == 0 || sembol_skoru == 0)
            {
                label22.ForeColor = Color.Red;
                label22.Text = "Şifre en az 1'er adet büyük harf,küçük harf,rakam ve sembol içermelidir";
            }
            if (kucuk_harf_skoru != 0 && buyuk_harf_skoru != 0 &&
               rakam_skoru != 0 && sembol_skoru != 0)
            {
                label22.Text = "";
            }
            if (parola_skoru <= 70)
            {
                parola_seviyesi = "Parolanız çok zayıf";
            }
            if (parola_skoru < 90 && parola_skoru > 70)
            {
                parola_seviyesi = "Parolanız orta seviye";
            }
            if (parola_skoru <= 100 && parola_skoru >= 90)
            {
                parola_seviyesi = "Parolanız güçlü";
            }

            label20.Text = "skor: " + parola_skoru;
            label21.Text = parola_seviyesi;

            progressBar1.Value = parola_skoru;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string yetki = "";
            bool kayitkontrol = false;
            baglan.Open();
            SqlCommand komut = new SqlCommand("select * from kullanicilar where TCno='" + textBox1.Text.ToString() + "'", baglan);
            SqlDataReader oku = komut.ExecuteReader();

            while (oku.Read())
            {
                kayitkontrol = true;
                break;
            }
            baglan.Close();
            if (kayitkontrol == false)
            {
                if (textBox1.MaxLength == 11 && textBox1.Text != "" && textBox2.Text != "" &&
                    textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                {

                    label1.ForeColor = Color.Black;
                    label2.ForeColor = Color.Black;
                    label3.ForeColor = Color.Black;
                    label4.ForeColor = Color.Black;
                    label5.ForeColor = Color.Black;
                    label6.ForeColor = Color.Black;
                    label7.ForeColor = Color.Black;

                    if (parola_skoru > 70)
                    {

                        if (radioButton1.Checked == true)
                        {
                            yetki = "Yönetici";
                        }
                        if (radioButton2.Checked == true)
                        {
                            yetki = "Kullanıcı";
                        }
                        try
                        {
                            baglan.Open();
                            SqlCommand komut1 = new SqlCommand("insert into kullanicilar values('" + textBox1.Text.ToString() + "','" + textBox2.Text.ToString() + "','" + textBox3.Text.ToString() + "','" + yetki + "','" + textBox4.Text.ToString() + "','" + textBox5.Text.ToString() + "')", baglan);

                            komut1.ExecuteNonQuery();
                            MessageBox.Show("Kullanıcı oluşturuldu.", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            baglan.Close();
                        }
                        catch (Exception hatamsj)
                        {
                            MessageBox.Show(hatamsj.Message);
                            baglan.Close();
                        }
                        baglan.Close();
                    }
                }
                else
                {

                    label1.ForeColor = Color.Red;
                    label2.ForeColor = Color.Red;
                    label3.ForeColor = Color.Red;
                    label4.ForeColor = Color.Red;
                    label5.ForeColor = Color.Red;
                    label6.ForeColor = Color.Red;
                    label7.ForeColor = Color.Red;
                    MessageBox.Show("Lütfen kırmızı alanları tekrar kontrol ediniz.", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Girilen TC Numarası kayıtlıdır", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }
    }
}
