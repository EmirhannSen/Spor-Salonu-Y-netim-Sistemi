using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace Proje2.View
{
    public partial class EgitmenlerView : UserControl
    {
        // Veritabanı bağlantısı için connection string'i alıyoruz
        string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;

        public EgitmenlerView()
        {
            InitializeComponent();
            EgitmenleriGetir();  // Başlangıçta eğitmenleri getir
        }

        // Eğitmenleri veritabanından çekip DataGrid'e aktarmak için kullanılan metod
        private void EgitmenleriGetir()
        {
            List<Egitmen> egitmenler = new List<Egitmen>();  // Eğitmenleri tutmak için liste oluşturuluyor
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();  // Veritabanı bağlantısını açıyoruz
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM egitmenler", conn);  // SQL sorgusu
                    MySqlDataReader reader = cmd.ExecuteReader();  // Verileri okuyoruz

                    // Veritabanındaki tüm eğitmenleri listeye ekliyoruz
                    while (reader.Read())
                    {
                        egitmenler.Add(new Egitmen
                        {
                            Id = reader.GetInt32("id"),
                            Ad = reader.GetString("ad"),
                            Soyad = reader.GetString("soyad"),
                            Telefon = reader.GetString("telefon"),
                            Email = reader.GetString("email"),
                            KayitTarihi = reader.GetDateTime("kayit_tarihi")  // Tarihi DateTime olarak alıyoruz
                        });
                    }

                    // Çekilen veriyi DataGrid'e aktar
                    EgitmenlerDataGrid.ItemsSource = egitmenler;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri çekilirken hata oluştu: " + ex.Message);  // Hata mesajı
            }
        }

        // Eğitmen eklemek için buton tıklama olayı
        private void EgitmenEkle_Click(object sender, RoutedEventArgs e)
        {
            // Yeni bir eğitmen kaydetme formu açılıyor
            EgitmenKayitView egitmenKayitView = new EgitmenKayitView();
            egitmenKayitView.ShowDialog();  // Modal pencereyi açıyoruz

            // Yeni eğitmen kaydedildikten sonra verileri tekrar çek
            EgitmenleriGetir();
        }

        // DataGrid'deki seçili eğitmeni güncellemek için kullanılıyor
        private void EgitmenlerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EgitmenlerDataGrid.SelectedItem is Egitmen egitmen)
            {
                // Seçilen eğitmenin bilgileriyle güncelleme penceresini açıyoruz
                EgitmenKayitView egitmenKayitView = new EgitmenKayitView(egitmen);
                egitmenKayitView.ShowDialog();  // Modal pencereyi açıyoruz

                // Eğitmen güncellenince verileri tekrar getir
                EgitmenleriGetir();
            }
        }

        // Silme işlemini gerçekleştiren metot
        private void SilButton_Click(object sender, RoutedEventArgs e)
        {
            // DataGrid'den seçili öğeyi al
            var egitmen = EgitmenlerDataGrid.SelectedItem as Egitmen;

            if (egitmen == null)
            {
                MessageBox.Show("Lütfen silmek için bir eğitmen seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kullanıcıya silme onayı sor
            MessageBoxResult result = MessageBox.Show($"Eğitmen {egitmen.Ad} {egitmen.Soyad} silinsin mi?", "Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("DELETE FROM egitmenler WHERE id = @id", conn);
                        cmd.Parameters.AddWithValue("@id", egitmen.Id);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Eğitmen başarıyla silindi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                    EgitmenleriGetir(); // Listeyi güncelle
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme işlemi sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



    }

    // Eğitmen sınıfı, veritabanındaki bir eğitmeni temsil ediyor
    public class Egitmen
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public DateTime KayitTarihi { get; set; }
    }

}
