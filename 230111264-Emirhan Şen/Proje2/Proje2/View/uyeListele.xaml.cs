using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;

namespace Proje2.View
{
    public partial class uyeListele : UserControl
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;
        List<UyeModel> uyeListesiVerisi = new List<UyeModel>();

        public uyeListele()
        {
            InitializeComponent();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            uyeListesi.ItemsSource = null; // Listeyi sıfırla (garanti temizleme)
            uyeListesiVerisi.Clear();      // Listeyi temizle
            ListeleUye();                  // Yeniden listele
        }


        private void ListeleUye()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT id, ad, soyad, telefon, yas, boy, kilo, cinsiyet, kayit_suresi, 
                                    kategori, dogumtarihi, kayit_tarihi,
                                    DATE_ADD(kayit_tarihi, INTERVAL kayit_suresi MONTH) AS bitis_tarihi
                             FROM uyeler";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        uyeListesiVerisi.Add(new UyeModel
                        {
                            Id = reader.GetInt32("id"),
                            Ad = reader.GetString("ad"),
                            Soyad = reader.GetString("soyad"),
                            Telefon = reader.GetString("telefon"),
                            Yas = reader.GetInt32("yas"),
                            Boy = reader.GetDouble("boy"),
                            Kilo = reader.GetDouble("kilo"),
                            Cinsiyet = reader.GetString("cinsiyet"),
                            KayitSuresi = reader.GetInt32("kayit_suresi"),
                            Kategori = reader.GetString("kategori"),
                            DogumTarihi = reader.GetDateTime("dogumtarihi"),
                            KayitTarihi = reader["kayit_tarihi"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["kayit_tarihi"]),
                            BitisTarihi = reader["bitis_tarihi"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["bitis_tarihi"]),
                        });
                    }

                    reader.Close();
                    uyeListesi.ItemsSource = uyeListesiVerisi;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void Uye_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid grid && grid.DataContext is UyeModel uye)
            {
                // Detay ekranını açarken, kayitTarihi ve bitisTarihi de gönderiyoruz
                var detay = new UyeDetayEkrani(
    uye.Ad, uye.Soyad, uye.Telefon, uye.Yas, uye.Boy, uye.Kilo, uye.Cinsiyet,
    uye.KayitSuresi, uye.Kategori, uye.DogumTarihi, uye.KayitTarihi, uye.BitisTarihi, uye.Id);

                // 🔁 Silinirse listeyi güncelle
                detay.UyeSilindiCallback = () =>
                {
                    UserControl_Loaded(this, null); // Listeyi yenile
                };

                detay.ShowDialog();

            }
        }

    }


    public class UyeModel
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Telefon { get; set; }
        public int Yas { get; set; }
        public double Boy { get; set; }
        public double Kilo { get; set; }
        public string Cinsiyet { get; set; }
        public int KayitSuresi { get; set; }
        public string Kategori { get; set; }
        public DateTime DogumTarihi { get; set; }

        public DateTime KayitTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }

        public string adSoyad => $"{Ad} {Soyad}";

        public string ResimYolu
        {
            get
            {
                if (Cinsiyet == "Kadın")
                    return "/Imagess/women2.png"; // Kadın resmi
                else
                    return "/Imagess/men2.png"; // Erkek resmi
            }
        }

    }
}