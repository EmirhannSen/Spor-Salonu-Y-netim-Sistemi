using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using System.Globalization;
using System.Collections.Generic;

namespace Proje2.View
{
    public partial class AnaSayfaView : UserControl
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;

        public AnaSayfaView()
        {
            InitializeComponent();
            SaatTarihGuncelle(); 
            VerileriYukle();
            
        }



        private void VerileriYukle()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Toplam Üye Sayısı
                    MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) FROM uyeler", conn);
                    txtToplamUye.Text = cmd1.ExecuteScalar().ToString();

                    // 2. Toplam Eğitmen Sayısı
                    MySqlCommand cmdE = new MySqlCommand("SELECT COUNT(*) FROM egitmenler", conn);
                    txtToplamEgitmen.Text = cmdE.ExecuteScalar().ToString();

                    // 3. Bu Ayki Yeni Üyeler
                    MySqlCommand cmd3 = new MySqlCommand("SELECT COUNT(*) FROM uyeler WHERE MONTH(kayit_tarihi) = MONTH(CURDATE()) AND YEAR(kayit_tarihi) = YEAR(CURDATE())", conn);
                    txtBuAykiUye.Text = cmd3.ExecuteScalar().ToString();

                    // 4. Bu Ayki Gelir
                    MySqlCommand cmd4 = new MySqlCommand("SELECT IFNULL(SUM(Tutar), 0) FROM odemeler WHERE MONTH(OdemeTarihi) = MONTH(CURDATE()) AND YEAR(OdemeTarihi) = YEAR(CURDATE())", conn);
                    decimal toplamGelir = Convert.ToDecimal(cmd4.ExecuteScalar());
                    txtBuAykiGelir.Text = toplamGelir.ToString("N2");

                    UyelikUyarilariniGetir();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yüklenirken hata oluştu: " + ex.Message);
            }



        }

        private void UyelikUyarilariniGetir()
        {
            List<string> uyarilar = new List<string>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
    SELECT ad, soyad, 
           DATEDIFF(DATE_ADD(kayit_tarihi, INTERVAL kayit_suresi MONTH), CURDATE()) AS kalanGun
    FROM uyeler
    WHERE DATEDIFF(DATE_ADD(kayit_tarihi, INTERVAL kayit_suresi MONTH), CURDATE()) <= 15
      AND DATEDIFF(DATE_ADD(kayit_tarihi, INTERVAL kayit_suresi MONTH), CURDATE()) >= 0;
";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string ad = reader["ad"].ToString();
                        string soyad = reader["soyad"].ToString();
                        int kalanGun = Convert.ToInt32(reader["kalanGun"]);

                        string mesaj = $"{ad} {soyad} - üyeliği {kalanGun} gün sonra bitiyor.";
                        uyarilar.Add(mesaj);
                    }

                }

                lstUyarilar.ItemsSource = uyarilar;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Uyarılar alınırken hata: " + ex.Message);
            }
        }




        private void SaatTarihGuncelle()
        {
            var turkceCulture = new CultureInfo("tr-TR");

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += (s, e) =>
            {
                DateTime now = DateTime.Now;

                // Örn: Pazartesi, 20 Mayıs 2025
                txtGununTarihi.Text = now.ToString("dddd, dd MMMM yyyy", turkceCulture);
                // Örn: 14:32:08
                txtGununSaati.Text = now.ToString("HH:mm");
            };

            timer.Start();
        }

    }
}
