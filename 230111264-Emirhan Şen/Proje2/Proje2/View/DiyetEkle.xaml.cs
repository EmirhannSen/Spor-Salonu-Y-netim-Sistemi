using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace Proje2.View
{
    public class DiyetProgram
    {
        public int Id { get; set; }
        public string OgunAdi { get; set; }
        public int Kalori { get; set; }
        public string OgunZamani { get; set; }

        public override string ToString()
        {
            return OgunAdi + " (" + Kalori + " kcal)";
        }
    }

    public partial class DiyetEkle : Window
    {
        private ObservableCollection<DiyetProgram> PazartesiListesi = new ObservableCollection<DiyetProgram>();
        private ObservableCollection<DiyetProgram> SaliListesi = new ObservableCollection<DiyetProgram>();
        private ObservableCollection<DiyetProgram> CarsambaListesi = new ObservableCollection<DiyetProgram>();
        private ObservableCollection<DiyetProgram> PersembeListesi = new ObservableCollection<DiyetProgram>();
        private ObservableCollection<DiyetProgram> CumaListesi = new ObservableCollection<DiyetProgram>();
        private ObservableCollection<DiyetProgram> CumartesiListesi = new ObservableCollection<DiyetProgram>();
        private ObservableCollection<DiyetProgram> PazarListesi = new ObservableCollection<DiyetProgram>();

        private string connStr = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;
        private ObservableCollection<DiyetProgram> OgunlerListesi = new ObservableCollection<DiyetProgram>();

        private int uyeId;
        private string adSoyad;

        public DiyetEkle(int gelenUyeId, string gelenAdSoyad)
        {
            InitializeComponent();

            uyeId = gelenUyeId;
            adSoyad = gelenAdSoyad;

            txtAdSoyad.Text = " | " + adSoyad;

            YükleOgunler();

            CbGunler.ItemsSource = new string[]
            {
                "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar"
            };

            CbOgunler.ItemsSource = OgunlerListesi;

            DgPazartesi.ItemsSource = PazartesiListesi;
            DgSali.ItemsSource = SaliListesi;
            DgCarsamba.ItemsSource = CarsambaListesi;
            DgPersembe.ItemsSource = PersembeListesi;
            DgCuma.ItemsSource = CumaListesi;
            DgCumartesi.ItemsSource = CumartesiListesi;
            DgPazar.ItemsSource = PazarListesi;

            Loaded += DiyetEkle_Loaded;
        }

        private void YükleOgunler()
        {
            OgunlerListesi.Clear();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sorgu = "SELECT Id, ogunAdi, Kalori, OgunZamani FROM diyetprogramlari";
                    using (MySqlCommand cmd = new MySqlCommand(sorgu, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OgunlerListesi.Add(new DiyetProgram
                            {
                                Id = reader.GetInt32("Id"),
                                OgunAdi = reader.GetString("ogunAdi"),
                                Kalori = reader.GetInt32("Kalori"),
                                OgunZamani = reader.GetString("OgunZamani")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Öğünler yüklenirken hata oluştu: " + ex.Message);
            }
        }

        private void DiyetEkle_Loaded(object sender, RoutedEventArgs e)
        {
            VerileriYukleUye();
        }

        private void VerileriYukleUye()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sorgu = @"
                        SELECT udp.gun, dp.Id, dp.ogunAdi, dp.Kalori, dp.OgunZamani
                        FROM uye_diyet_programi udp
                        JOIN diyetprogramlari dp ON udp.ogun_id = dp.Id
                        WHERE udp.uye_id = @uye_id";

                    using (MySqlCommand cmd = new MySqlCommand(sorgu, conn))
                    {
                        cmd.Parameters.AddWithValue("@uye_id", uyeId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var ogun = new DiyetProgram
                                {
                                    Id = reader.GetInt32("Id"),
                                    OgunAdi = reader.GetString("ogunAdi"),
                                    Kalori = reader.GetInt32("Kalori"),
                                    OgunZamani = reader.GetString("OgunZamani")
                                };

                                string gun = reader.GetString("gun");

                                switch (gun)
                                {
                                    case "Pazartesi": PazartesiListesi.Add(ogun); break;
                                    case "Salı": SaliListesi.Add(ogun); break;
                                    case "Çarşamba": CarsambaListesi.Add(ogun); break;
                                    case "Perşembe": PersembeListesi.Add(ogun); break;
                                    case "Cuma": CumaListesi.Add(ogun); break;
                                    case "Cumartesi": CumartesiListesi.Add(ogun); break;
                                    case "Pazar": PazarListesi.Add(ogun); break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Üyenin diyet programı yüklenirken hata oluştu: " + ex.Message);
            }
        }

        private void BtnEkle_Click(object sender, RoutedEventArgs e)
        {
            if (CbOgunler.SelectedItem is DiyetProgram secilenOgun && CbGunler.SelectedItem is string seciliGun)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        string sorgu = "INSERT INTO uye_diyet_programi (uye_id, ogun_id, gun) VALUES (@uye_id, @ogun_id, @gun)";
                        using (MySqlCommand cmd = new MySqlCommand(sorgu, conn))
                        {
                            cmd.Parameters.AddWithValue("@uye_id", uyeId);
                            cmd.Parameters.AddWithValue("@ogun_id", secilenOgun.Id);
                            cmd.Parameters.AddWithValue("@gun", seciliGun);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Listeye ekle (UI'da göster)
                    switch (seciliGun)
                    {
                        case "Pazartesi": PazartesiListesi.Add(secilenOgun); break;
                        case "Salı": SaliListesi.Add(secilenOgun); break;
                        case "Çarşamba": CarsambaListesi.Add(secilenOgun); break;
                        case "Perşembe": PersembeListesi.Add(secilenOgun); break;
                        case "Cuma": CumaListesi.Add(secilenOgun); break;
                        case "Cumartesi": CumartesiListesi.Add(secilenOgun); break;
                        case "Pazar": PazarListesi.Add(secilenOgun); break;
                    }

                    MessageBox.Show("Öğün başarıyla eklendi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ekleme sırasında hata oluştu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir öğün ve gün seçiniz.");
            }
        }



        private void KaydetGun(MySqlCommand cmd, ObservableCollection<DiyetProgram> liste, string gun)
        {
            foreach (var ogun in liste)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@uye_id", uyeId);
                cmd.Parameters.AddWithValue("@ogun_id", ogun.Id);
                cmd.Parameters.AddWithValue("@gun", gun);
                cmd.ExecuteNonQuery();
            }
        }

        private void BtnSil_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            DiyetProgram secilenOgun = btn.DataContext as DiyetProgram;

            if (secilenOgun == null)
                return;

            string seciliGun = (TabGunler.SelectedItem as TabItem)?.Header.ToString();
            if (string.IsNullOrEmpty(seciliGun))
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sorgu = @"DELETE FROM uye_diyet_programi 
                             WHERE uye_id = @uye_id AND ogun_id = @ogun_id AND gun = @gun LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(sorgu, conn))
                    {
                        cmd.Parameters.AddWithValue("@uye_id", uyeId);
                        cmd.Parameters.AddWithValue("@ogun_id", secilenOgun.Id);
                        cmd.Parameters.AddWithValue("@gun", seciliGun);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Listeden kaldır
                switch (seciliGun)
                {
                    case "Pazartesi": PazartesiListesi.Remove(secilenOgun); break;
                    case "Salı": SaliListesi.Remove(secilenOgun); break;
                    case "Çarşamba": CarsambaListesi.Remove(secilenOgun); break;
                    case "Perşembe": PersembeListesi.Remove(secilenOgun); break;
                    case "Cuma": CumaListesi.Remove(secilenOgun); break;
                    case "Cumartesi": CumartesiListesi.Remove(secilenOgun); break;
                    case "Pazar": PazarListesi.Remove(secilenOgun); break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme işlemi sırasında hata oluştu: " + ex.Message);
            }
        }

    }
}
