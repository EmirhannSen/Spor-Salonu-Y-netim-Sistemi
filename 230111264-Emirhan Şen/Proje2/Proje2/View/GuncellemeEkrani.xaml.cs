using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows;

namespace Proje2.View
{
    public partial class GuncellemeEkrani : Window
    {
        private int uyeId;
        private string ad, soyad, telefon;
        private int yas;
        private double boy, kilo;
        private DateTime dogumTarihi;


        string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;

        public GuncellemeEkrani(string ad, string soyad, string telefon, int yas, double boy,
                        double kilo, DateTime dogumTarihi, int id)
        {
            InitializeComponent();

            this.ad = ad;
            this.soyad = soyad;
            this.telefon = telefon;
            this.yas = yas;
            this.boy = boy;
            this.kilo = kilo;
            this.dogumTarihi = dogumTarihi;
            this.uyeId = id;

            txtAd.Text = ad;
            txtSoyad.Text = soyad;
            txtTelefon.Text = telefon;
            txtYas.Text = yas.ToString();
            txtBoy.Text = boy.ToString();
            txtKilo.Text = kilo.ToString();
            dpDogumTarihi.SelectedDate = dogumTarihi;
        }


        private void GuncelleButton_Click(object sender, RoutedEventArgs e)
        {
            // Alanların kontrolü
            string ad = txtAd.Text;
            string soyad = txtSoyad.Text;
            string telefon = txtTelefon.Text;
            string yasText = txtYas.Text;
            string boyText = txtBoy.Text;
            string kiloText = txtKilo.Text;
            DateTime? dogumTarihi = dpDogumTarihi.SelectedDate;

            // 1. Boş alan kontrolü
            if (string.IsNullOrWhiteSpace(ad) ||
                string.IsNullOrWhiteSpace(soyad) ||
                string.IsNullOrWhiteSpace(telefon) ||
                string.IsNullOrWhiteSpace(yasText) ||
                string.IsNullOrWhiteSpace(boyText) ||
                string.IsNullOrWhiteSpace(kiloText) ||
                !dogumTarihi.HasValue)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.");
                return;
            }

            // 2. Sayısal alanların kontrolü
            if (!int.TryParse(yasText, out int yas))
            {
                MessageBox.Show("Yaş sayısal bir değer olmalıdır.");
                return;
            }

            if (!double.TryParse(boyText, out double boy))
            {
                MessageBox.Show("Boy ondalıklı sayısal bir değer olmalıdır. Örn: 1.75");
                return;
            }

            if (!int.TryParse(kiloText, out int kilo))
            {
                MessageBox.Show("Kilo sayısal bir değer olmalıdır.");
                return;
            }

            // 3. Güncelleme işlemi
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE uyeler 
                             SET ad = @ad, soyad = @soyad, telefon = @telefon, yas = @yas, boy = @boy, kilo = @kilo, 
                                 dogumtarihi = @dogumtarihi 
                             WHERE id = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@ad", ad);
                    cmd.Parameters.AddWithValue("@soyad", soyad);
                    cmd.Parameters.AddWithValue("@telefon", telefon);
                    cmd.Parameters.AddWithValue("@yas", yas);
                    cmd.Parameters.AddWithValue("@boy", boy);
                    cmd.Parameters.AddWithValue("@kilo", kilo);
                    cmd.Parameters.AddWithValue("@dogumtarihi", dogumTarihi.Value);
                    cmd.Parameters.AddWithValue("@id", uyeId);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Üye başarıyla güncellendi.");
                    UyeDetaylariniYenidenAc(uyeId);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }

            // Güncel üyeyi listele ve pencereyi kapat
            this.Close();
        }
        private void IptalButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            UyeDetaylariniYenidenAc(uyeId);
        }

        private void UyeDetaylariniYenidenAc(int uyeId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT ad, soyad, telefon, yas, boy, kilo, cinsiyet, kayit_suresi,
                                    kategori, dogumtarihi, kayit_tarihi,
                                    DATE_ADD(kayit_tarihi, INTERVAL kayit_suresi MONTH) AS bitis_tarihi
                             FROM uyeler
                             WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", uyeId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var detayEkrani = new UyeDetayEkrani(
                                    reader.GetString("ad"),
                                    reader.GetString("soyad"),
                                    reader.GetString("telefon"),
                                    reader.GetInt32("yas"),
                                    reader.GetDouble("boy"),
                                    reader.GetDouble("kilo"),
                                    reader.GetString("cinsiyet"),
                                    reader.GetInt32("kayit_suresi"),
                                    reader.GetString("kategori"),
                                    reader.GetDateTime("dogumtarihi"),
                                    reader.GetDateTime("kayit_tarihi"),
                                    reader.GetDateTime("bitis_tarihi"),
                                    uyeId
                                );

                                detayEkrani.ShowDialog();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Detay ekranı açılamadı: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
