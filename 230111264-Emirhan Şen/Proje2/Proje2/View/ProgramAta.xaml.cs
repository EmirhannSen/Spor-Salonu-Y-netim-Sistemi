using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Proje2.View
{
    public partial class ProgramAta : Window, INotifyPropertyChanged
    {
        private int uyeId;
        private string uyeAdiSoyadi;

        public ObservableCollection<string> KasGrubuListesi { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Egzersizler> EgzersizListesi { get; set; } = new ObservableCollection<Egzersizler>();
        public ObservableCollection<Egzersizler> FiltrelenmisEgzersizler { get; set; } = new ObservableCollection<Egzersizler>();

        public ObservableCollection<string> Gunler { get; set; } = new ObservableCollection<string>
        {
            "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar"
        };

        public ObservableCollection<Egzersizler> PazartesiListesi { get; set; } = new ObservableCollection<Egzersizler>();
        public ObservableCollection<Egzersizler> SaliListesi { get; set; } = new ObservableCollection<Egzersizler>();
        public ObservableCollection<Egzersizler> CarsambaListesi { get; set; } = new ObservableCollection<Egzersizler>();
        public ObservableCollection<Egzersizler> PersembeListesi { get; set; } = new ObservableCollection<Egzersizler>();
        public ObservableCollection<Egzersizler> CumaListesi { get; set; } = new ObservableCollection<Egzersizler>();
        public ObservableCollection<Egzersizler> CumartesiListesi { get; set; } = new ObservableCollection<Egzersizler>();
        public ObservableCollection<Egzersizler> PazarListesi { get; set; } = new ObservableCollection<Egzersizler>();

        private string secilenKasGrubu;
        public string SecilenKasGrubu
        {
            get => secilenKasGrubu;
            set
            {
                if (secilenKasGrubu != value)
                {
                    secilenKasGrubu = value;
                    OnPropertyChanged(nameof(SecilenKasGrubu));
                    EgzersizleriFiltrele();
                }
            }
        }

        private Egzersizler secilenEgzersiz;
        public Egzersizler SecilenEgzersiz
        {
            get => secilenEgzersiz;
            set
            {
                secilenEgzersiz = value;
                OnPropertyChanged(nameof(SecilenEgzersiz));
            }
        }

        private string secilenGun;
        public string SecilenGun
        {
            get => secilenGun;
            set
            {
                secilenGun = value;
                OnPropertyChanged(nameof(SecilenGun));
            }
        }

        public string Set { get; set; }
        public string Tekrar { get; set; }
        public string Sure { get; set; }

        public ProgramAta(int uyeId, string adSoyad)
        {
            InitializeComponent();
            this.uyeId = uyeId;
            this.uyeAdiSoyadi = adSoyad;

            lblUyeAdi.Text = $"| {uyeAdiSoyadi}";

            DataContext = this;

            KasGrubuYukle();
            EgzersizleriYukle();
            ProgramlariYukle();

        }

        private void KasGrubuYukle()
        {
            try
            {
                using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT DISTINCT kas_grubu FROM egzersizadi";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        KasGrubuListesi.Add(reader.GetString("kas_grubu"));
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kas grupları yüklenemedi: " + ex.Message);
            }
        }

        private void EgzersizleriYukle()
        {
            try
            {
                using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT id, egzersiz_adi, kas_grubu FROM egzersizadi";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        EgzersizListesi.Add(new Egzersizler
                        {
                            Id = reader.GetInt32("id"),
                            EgzersizAdi = reader.GetString("egzersiz_adi"),
                            KasGrubu = reader.GetString("kas_grubu")
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Egzersizler yüklenemedi: " + ex.Message);
            }
        }

        private void ProgramlariYukle()
        {
            try
            {
                using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString))
                {
                    conn.Open();
                    string query = @"SELECT egzersiz_adi, set_sayisi, tekrar_sayisi, sure, gun 
                             FROM programlar 
                             WHERE uye_id = @uyeId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@uyeId", uyeId);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var egzersiz = new Egzersizler
                        {
                            EgzersizAdi = reader["egzersiz_adi"]?.ToString(),
                            Set = reader["set_sayisi"] == DBNull.Value ? "" : reader["set_sayisi"].ToString(),
                            Tekrar = reader["tekrar_sayisi"] == DBNull.Value ? "" : reader["tekrar_sayisi"].ToString(),
                            Sure = reader["sure"] == DBNull.Value ? "" : reader["sure"].ToString()
                        };

                        string gun = reader["gun"]?.ToString();
                        switch (gun)
                        {
                            case "Pazartesi": PazartesiListesi.Add(egzersiz); break;
                            case "Salı": SaliListesi.Add(egzersiz); break;
                            case "Çarşamba": CarsambaListesi.Add(egzersiz); break;
                            case "Perşembe": PersembeListesi.Add(egzersiz); break;
                            case "Cuma": CumaListesi.Add(egzersiz); break;
                            case "Cumartesi": CumartesiListesi.Add(egzersiz); break;
                            case "Pazar": PazarListesi.Add(egzersiz); break;
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıtlı programlar yüklenemedi: " + ex.Message);
            }
        }


        private void EgzersizleriFiltrele()
        {
            FiltrelenmisEgzersizler.Clear();

            var filtreli = EgzersizListesi
                .Where(e => e.KasGrubu == SecilenKasGrubu)
                .ToList();

            foreach (var egzersiz in filtreli)
                FiltrelenmisEgzersizler.Add(egzersiz);

            OnPropertyChanged(nameof(FiltrelenmisEgzersizler));
        }

        private void Ekle_Click(object sender, RoutedEventArgs e)
        {
            if (SecilenEgzersiz == null || string.IsNullOrEmpty(SecilenGun))
            {
                MessageBox.Show("Lütfen egzersiz ve gün seçin.");
                return;
            }

            if (!int.TryParse(Set, out int setDegeri))
            {
                MessageBox.Show("Set alanına yalnızca sayı girilmelidir.");
                return;
            }

            if (!int.TryParse(Tekrar, out int tekrarDegeri))
            {
                MessageBox.Show("Tekrar alanına yalnızca sayı girilmelidir.");
                return;
            }

            if (!int.TryParse(Sure, out int sureDegeri))
            {
                MessageBox.Show("Süre alanına yalnızca sayı girilmelidir.");
                return;
            }

            var yeni = new Egzersizler
            {
                EgzersizAdi = SecilenEgzersiz.EgzersizAdi,
                Set = setDegeri.ToString(),
                Tekrar = tekrarDegeri.ToString(),
                Sure = sureDegeri.ToString()
            };

            switch (SecilenGun)
            {
                case "Pazartesi": PazartesiListesi.Add(yeni); break;
                case "Salı": SaliListesi.Add(yeni); break;
                case "Çarşamba": CarsambaListesi.Add(yeni); break;
                case "Perşembe": PersembeListesi.Add(yeni); break;
                case "Cuma": CumaListesi.Add(yeni); break;
                case "Cumartesi": CumartesiListesi.Add(yeni); break;
                case "Pazar": PazarListesi.Add(yeni); break;
            }

            ProgramiKaydet(yeni, SecilenGun);

            Set = Tekrar = Sure = "";
            SecilenEgzersiz = null;
            SecilenGun = null;

            DataContext = null;
            DataContext = this;
        }


        private void ProgramiKaydet(Egzersizler egzersiz, string gun)
        {
            try
            {
                using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO programlar (uye_id, egzersiz_adi, set_sayisi, tekrar_sayisi, sure, gun) 
                                     VALUES (@uyeId, @egzersiz, @set, @tekrar, @sure, @gun)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@uyeId", uyeId);
                    cmd.Parameters.AddWithValue("@egzersiz", egzersiz.EgzersizAdi);
                    cmd.Parameters.AddWithValue("@set", egzersiz.Set);
                    cmd.Parameters.AddWithValue("@tekrar", egzersiz.Tekrar);
                    cmd.Parameters.AddWithValue("@sure", egzersiz.Sure);
                    cmd.Parameters.AddWithValue("@gun", gun);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program kaydedilirken hata oluştu: " + ex.Message);
            }
        }

        private void Sil_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is Egzersizler egzersiz)
            {
                string gun = (TabGunler.SelectedItem as TabItem)?.Header?.ToString();

                if (string.IsNullOrEmpty(gun))
                    return;

                // Listeden sil
                switch (gun)
                {
                    case "Pazartesi": PazartesiListesi.Remove(egzersiz); break;
                    case "Salı": SaliListesi.Remove(egzersiz); break;
                    case "Çarşamba": CarsambaListesi.Remove(egzersiz); break;
                    case "Perşembe": PersembeListesi.Remove(egzersiz); break;
                    case "Cuma": CumaListesi.Remove(egzersiz); break;
                    case "Cumartesi": CumartesiListesi.Remove(egzersiz); break;
                    case "Pazar": PazarListesi.Remove(egzersiz); break;
                }

                // Veritabanından sil
                try
                {
                    using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString))
                    {
                        conn.Open();
                        string query = @"DELETE FROM programlar 
                                 WHERE uye_id = @uyeId AND egzersiz_adi = @egzersizAdi AND gun = @gun
                                       AND set_sayisi = @set AND tekrar_sayisi = @tekrar AND sure = @sure";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@uyeId", uyeId);
                        cmd.Parameters.AddWithValue("@egzersizAdi", egzersiz.EgzersizAdi);
                        cmd.Parameters.AddWithValue("@gun", gun);
                        cmd.Parameters.AddWithValue("@set", egzersiz.Set);
                        cmd.Parameters.AddWithValue("@tekrar", egzersiz.Tekrar);
                        cmd.Parameters.AddWithValue("@sure", egzersiz.Sure);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme işlemi sırasında hata oluştu: " + ex.Message);
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Egzersizler
    {
        public int Id { get; set; }
        public string EgzersizAdi { get; set; }
        public string KasGrubu { get; set; }

        public string Set { get; set; }
        public string Tekrar { get; set; }
        public string Sure { get; set; }
    }
}
