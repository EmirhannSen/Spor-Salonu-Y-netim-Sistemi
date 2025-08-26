using System;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Proje2.View
{
    public partial class UyeEkle : UserControl
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;

        public UyeEkle()
        {
            InitializeComponent();
            KategorileriYukle(); // İlk yükleme
        }

        private void KategorileriYukle()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT DISTINCT f.Kategori FROM fiyatlar f";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbKategori.ItemsSource = dt.DefaultView;
                    cmbKategori.DisplayMemberPath = "Kategori";
                    cmbKategori.SelectedValuePath = "Kategori";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kategori yükleme hatası: " + ex.Message);
            }
        }

        private void cmbKategori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbSure.ItemsSource = null;
            txtUcret.Clear();

            if (cmbKategori.SelectedValue != null)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        var query = "SELECT SureAy FROM fiyatlar WHERE Kategori = @kategoriId";
                        var cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@kategoriId", cmbKategori.SelectedValue);
                        var da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        cmbSure.ItemsSource = dt.DefaultView;
                        cmbSure.DisplayMemberPath = "SureAy";
                        cmbSure.SelectedValuePath = "SureAy";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Süre yüklenemedi: " + ex.Message);
                }
            }
        }

        private void cmbSure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtUcret.Clear();
            if (cmbKategori.SelectedValue != null && cmbSure.SelectedValue != null)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        var query = "SELECT Fiyat FROM fiyatlar WHERE Kategori = @kategoriId AND SureAy = @sureAy";
                        var cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@kategoriId", cmbKategori.SelectedValue);
                        cmd.Parameters.AddWithValue("@sureAy", cmbSure.SelectedValue);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                            txtUcret.Text = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fiyat getirilemedi: " + ex.Message);
                }
            }
        }

        private void Kaydet_Click(object sender, RoutedEventArgs e)
        {
            string ad = txtAd.Text.Trim();
            string soyad = txtSoyad.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            string yasText = txtYas.Text.Trim();
            string boyText = txtBoy.Text.Trim();
            string kiloText = txtKilo.Text.Trim();
            string cinsiyet = ((ComboBoxItem)cmbCinsiyet.SelectedItem)?.Content?.ToString();
            string kayitSuresiText = cmbSure.SelectedValue?.ToString();
            string kategoriAd = ((DataRowView)cmbKategori.SelectedItem)?["Kategori"].ToString();
            string ucretText = txtUcret.Text.Trim();
            DateTime? dogumTarihi = dpDogumTarihi.SelectedDate?.Date;
            DateTime kayitTarihi = DateTime.Today;

            if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(soyad) ||
                string.IsNullOrWhiteSpace(telefon) || string.IsNullOrWhiteSpace(yasText) ||
                string.IsNullOrWhiteSpace(boyText) || string.IsNullOrWhiteSpace(kiloText) ||
                string.IsNullOrWhiteSpace(cinsiyet) || string.IsNullOrWhiteSpace(kayitSuresiText) ||
                string.IsNullOrWhiteSpace(kategoriAd) || string.IsNullOrWhiteSpace(ucretText) || dogumTarihi == null)
            {
                MessageBox.Show("Lütfen tüm alanları eksiksiz doldurunuz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(yasText, out int yas) || yas < 10 || yas > 100)
            {
                MessageBox.Show("Geçerli bir yaş giriniz (10-100 arası).", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(boyText, out double boy) || boy < 50 || boy > 250)
            {
                MessageBox.Show("Geçerli bir boy giriniz (örneğin 170).", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            


            if (!int.TryParse(kiloText, out int kilo) || kilo < 10 || kilo > 300)
            {
                MessageBox.Show("Geçerli bir kilo giriniz (30-250 arası).", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(kayitSuresiText, out int kayitSuresi) || kayitSuresi <= 0)
            {
                MessageBox.Show("Geçerli bir kayıt süresi seçiniz.", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(ucretText, out decimal ucret) || ucret <= 0)
            {
                MessageBox.Show("Geçerli bir ücret değeri giriniz.", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string uyeQuery = @"INSERT INTO uyeler 
(ad, soyad, telefon, yas, boy, kilo, cinsiyet, kayit_suresi, kategori, dogumtarihi, kayit_tarihi)
VALUES (@ad, @soyad, @telefon, @yas, @boy, @kilo, @cinsiyet, @kayit_suresi, @kategori, @dogumtarihi, @kayit_tarihi);
SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(uyeQuery, conn);
                    cmd.Parameters.AddWithValue("@ad", ad);
                    cmd.Parameters.AddWithValue("@soyad", soyad);
                    cmd.Parameters.AddWithValue("@telefon", telefon);
                    cmd.Parameters.AddWithValue("@yas", yas);
                    cmd.Parameters.AddWithValue("@boy", boy);
                    cmd.Parameters.AddWithValue("@kilo", kilo);
                    cmd.Parameters.AddWithValue("@cinsiyet", cinsiyet);
                    cmd.Parameters.AddWithValue("@kayit_suresi", kayitSuresi);
                    cmd.Parameters.AddWithValue("@kategori", kategoriAd);
                    cmd.Parameters.AddWithValue("@dogumtarihi", dogumTarihi);
                    cmd.Parameters.AddWithValue("@kayit_tarihi", kayitTarihi);

                    int uyeID = Convert.ToInt32(cmd.ExecuteScalar());

                    string odemeQuery = @"INSERT INTO odemeler (UyeID, OdemeTarihi, Tutar) 
                                  VALUES (@uyeID, @odemeTarihi, @tutar)";
                    MySqlCommand odemeCmd = new MySqlCommand(odemeQuery, conn);
                    odemeCmd.Parameters.AddWithValue("@uyeID", uyeID);
                    odemeCmd.Parameters.AddWithValue("@odemeTarihi", kayitTarihi);
                    odemeCmd.Parameters.AddWithValue("@tutar", ucret);
                    odemeCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Üye ve ödeme başarıyla eklendi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                TemizleForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TemizleForm()
        {
            txtAd.Clear();
            txtSoyad.Clear();
            txtTelefon.Clear();
            txtYas.Clear();
            txtBoy.Clear();
            txtKilo.Clear();
            cmbCinsiyet.SelectedIndex = -1;
            cmbKategori.SelectedIndex = -1;
            cmbSure.ItemsSource = null;
            txtUcret.Clear();
            dpDogumTarihi.SelectedDate = null;
        }


    }
}