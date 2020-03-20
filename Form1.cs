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
using Personal_Takip_Otomasyonu;

namespace Personal_Takip_Programı
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglan = new SqlConnection("Data Source=DESKTOP-6RUFCV1;Initial Catalog=personal;Integrated Security=True");

        public static string tcno, ad, soyad, yetki;

        int giris_hakki = 3; bool durum = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.AcceptButton = giris;
            this.CancelButton = cikis;
            label5.Text = Convert.ToString(giris_hakki);
            radioButton1.Checked = true;
        }


        private void giris_Click(object sender, EventArgs e)
        {
            if (giris_hakki != 0)
            {
                if (radioButton1.Checked == true)
                {
                    durum = true;
                    try
                    {
                        baglan.Open();

                        string sql = "select *from kullanicilar where [Kullanıcı Adı]=@kullaniciadi and Parola=@sifre and Yetki=@yetki";
                        SqlParameter prm_ad = new SqlParameter("kullaniciadi", textBox1.Text.Trim());
                        SqlParameter prm_sifre = new SqlParameter("sifre", textBox2.Text.Trim());
                        SqlParameter prm_yetki = new SqlParameter("yetki", radioButton1.Text);

                        SqlCommand komut = new SqlCommand(sql, baglan);

                        komut.Parameters.Add(prm_ad);
                        komut.Parameters.Add(prm_sifre);
                        komut.Parameters.Add(prm_yetki);

                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(komut);
                        da.Fill(dt);

                        SqlDataReader oku = komut.ExecuteReader();
                        while (oku.Read())
                        {
                            ListViewItem ekle = new ListViewItem();
                            tcno = oku["TCno"].ToString();
                            ad = oku["Ad"].ToString();
                            soyad = oku["Soyad"].ToString();
                            yetki = oku["Yetki"].ToString();
                        }

                        if (dt.Rows.Count > 0)
                        {
                            Form2 form2 = new Form2();
                            form2.Show();
                            this.Hide();
                        }

                        komut.ExecuteNonQuery();



                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Hatalı giriş yaptınız. Lütfen tekrar kontrol ediniz.", "SKY Kontrol Takip Programı",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    baglan.Close();
                }
                else if (radioButton2.Checked == true)
                {
                    try
                    {
                        baglan.Open();
                        string sql = "select * from kullanicilar where [Kullanıcı Adı]=@kullaniciadi and Parola=@sifre and Yetki=@yetki";
                        SqlParameter prm_ad = new SqlParameter("kullaniciadi", textBox1.Text.Trim());
                        SqlParameter prm_sifre = new SqlParameter("sifre", textBox2.Text.Trim());
                        SqlParameter prm_yetki = new SqlParameter("yetki", radioButton2.Text);
                        SqlCommand komut = new SqlCommand(sql, baglan);
                        komut.Parameters.Add(prm_ad);
                        komut.Parameters.Add(prm_sifre);
                        komut.Parameters.Add(prm_yetki);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(komut);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Form3 form3 = new Form3();
                            form3.Show();
                            this.Hide();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Hatalı giriş yaptınız. Lütfen tekrar kontrol ediniz.", "SKY Kontrol Takip Programı",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (durum == false)
            {
                giris_hakki--;
                label5.Text = Convert.ToString(giris_hakki);

            }
            if (giris_hakki == 0)
            {
                giris.Enabled = false;
                MessageBox.Show("3 defa hatalı giriş yaptınız için programa girişiniz engellenmiştir.", "SKY Takip Programı",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

    }
}
