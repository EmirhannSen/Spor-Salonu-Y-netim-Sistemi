using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Proje2.View
{
    public partial class DiyetEkrani : UserControl
    {
        // Diyet programları için ObservableCollection
        public ObservableCollection<DiyetProgrami> DiyetProgramlari { get; set; } = new ObservableCollection<DiyetProgrami>();

        // Veritabanı bağlantı stringi
        private string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;

        public DiyetEkrani()
        {
            InitializeComponent();
            // Diyet planlarını veritabanından yükle
            DiyetProgramlari = DiyetProgramiListele();
            lstDiyetler.ItemsSource = DiyetProgramlari;  // ListBox'ı ObservableCollection'a bağla
        }

        // Diyet programı ekleme
        private void EkleDiyetProgrami_Click(object sender, RoutedEventArgs e)
        {
            string ogunAdi = txtOgunAdi.Text.Trim();
            string kaloriStr = txtKalori.Text.Trim();
            string ogunZamani = (cmbOgunZamani.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(ogunAdi) || string.IsNullOrEmpty(kaloriStr) || string.IsNullOrEmpty(ogunZamani))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            if (!int.TryParse(kaloriStr, out int kalori))
            {
                MessageBox.Show("Geçerli bir kalori değeri girin.");
                return;
            }

            // Yeni diyet programı nesnesi oluştur
            DiyetProgrami yeniDiyet = new DiyetProgrami(ogunAdi, kalori, ogunZamani);

            // Veritabanına ekle
            DiyetProgramiEkle(yeniDiyet);

            // ObservableCollection'a ekle
            DiyetProgramlari.Add(yeniDiyet);

            // TextBox ve ComboBox'ları temizle
            txtOgunAdi.Clear();
            txtKalori.Clear();
            cmbOgunZamani.SelectedIndex = -1;
        }

        // Diyet programını veritabanına ekleme
        private void DiyetProgramiEkle(DiyetProgrami diyet)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO DiyetProgramlari (OgunAdi, Kalori, OgunZamani) VALUES (@OgunAdi, @Kalori, @OgunZamani)";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@OgunAdi", diyet.OgunAdi);
                    command.Parameters.AddWithValue("@Kalori", diyet.Kalori);
                    command.Parameters.AddWithValue("@OgunZamani", diyet.OgunZamani);

                    command.ExecuteNonQuery(); // Sorguyu çalıştır
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanı hatası: {ex.Message}");
            }
        }

        // Diyet programlarını veritabanından okuma
        private ObservableCollection<DiyetProgrami> DiyetProgramiListele()
        {
            ObservableCollection<DiyetProgrami> diyetProgramlari = new ObservableCollection<DiyetProgrami>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM DiyetProgramlari";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string ogunAdi = reader["OgunAdi"].ToString();
                        int kalori = Convert.ToInt32(reader["Kalori"]);
                        string ogunZamani = reader["OgunZamani"].ToString();

                        DiyetProgrami diyet = new DiyetProgrami(ogunAdi, kalori, ogunZamani);
                        diyetProgramlari.Add(diyet);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanı hatası: {ex.Message}");
            }

            return diyetProgramlari;
        }

        // Diyet silme işlemi
        private void SilDiyetProgrami_Click(object sender, RoutedEventArgs e)
        {
            // ListBox'tan seçili öğeyi al
            DiyetProgrami secilenDiyet = lstDiyetler.SelectedItem as DiyetProgrami;

            if (secilenDiyet == null)
            {
                MessageBox.Show("Lütfen silmek için bir diyet planı seçin.");
                return;
            }

            // Veritabanından silme
            DiyetProgramiSil(secilenDiyet);

            // ObservableCollection'dan silme
            DiyetProgramlari.Remove(secilenDiyet);
        }

        // Diyet programını veritabanından silme
        private void DiyetProgramiSil(DiyetProgrami diyet)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM DiyetProgramlari WHERE OgunAdi = @OgunAdi AND Kalori = @Kalori AND OgunZamani = @OgunZamani";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@OgunAdi", diyet.OgunAdi);
                    command.Parameters.AddWithValue("@Kalori", diyet.Kalori);
                    command.Parameters.AddWithValue("@OgunZamani", diyet.OgunZamani);

                    command.ExecuteNonQuery(); // Sorguyu çalıştır
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanı hatası: {ex.Message}");
            }
        }
    }

    // Diyet Programı Modeli
    public class DiyetProgrami
    {
        public string OgunAdi { get; set; }
        public int Kalori { get; set; }
        public string OgunZamani { get; set; }

        // Constructor
        public DiyetProgrami(string ogunAdi, int kalori, string ogunZamani)
        {
            OgunAdi = ogunAdi;
            Kalori = kalori;
            OgunZamani = ogunZamani;
        }

        // ToString metodunu geçersiz kılarak nesnenin nasıl görüntüleneceğini belirliyoruz
        public override string ToString()
        {
            return $"{OgunAdi} ({OgunZamani}) - {Kalori} Kalori";
        }
    }
}
