using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace Proje2.View
{
    public partial class EgzersizEkrani : UserControl
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;

        public ObservableCollection<string> KasGruplari { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Egzersiz> Egzersizler { get; set; } = new ObservableCollection<Egzersiz>();

        public EgzersizEkrani()
        {
            InitializeComponent();

            // Liste bağlantıları
            lstEgzersizler.ItemsSource = Egzersizler;
            lstKasGruplari.ItemsSource = KasGruplari;
            cmbKasGrubu.ItemsSource = KasGruplari;

            LoadKasGruplari();
            LoadEgzersizler();
        }

        private void LoadKasGruplari()
        {
            KasGruplari.Clear();

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ad FROM kas_gruplari";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string grupAdi = reader["ad"] as string;
                            if (!string.IsNullOrWhiteSpace(grupAdi))
                                KasGruplari.Add(grupAdi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kas grupları yüklenirken hata: " + ex.Message);
            }
        }

        private void LoadEgzersizler()
        {
            Egzersizler.Clear();

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT egzersiz_adi, kas_grubu FROM egzersizadi";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ad = reader["egzersiz_adi"] as string;
                            string grup = reader["kas_grubu"] as string;

                            if (!string.IsNullOrWhiteSpace(ad) && !string.IsNullOrWhiteSpace(grup))
                                Egzersizler.Add(new Egzersiz(ad, grup));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Egzersizler yüklenirken hata: " + ex.Message);
            }
        }

        private void EkleEgzersiz_Click(object sender, RoutedEventArgs e)
        {
            string ad = txtEgzersizAdi.Text.Trim();
            string grup = cmbKasGrubu.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(grup))
            {
                MessageBox.Show("Lütfen egzersiz adı ve kas grubu seçin.");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO egzersizadi (egzersiz_adi, kas_grubu) VALUES (@ad, @grup)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ad", ad);
                        cmd.Parameters.AddWithValue("@grup", grup);
                        cmd.ExecuteNonQuery();
                    }
                }

                Egzersizler.Add(new Egzersiz(ad, grup));
                txtEgzersizAdi.Clear();
                cmbKasGrubu.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Egzersiz eklenirken hata: " + ex.Message);
            }
        }

        private void SilEgzersiz_Click(object sender, RoutedEventArgs e)
        {
            if (lstEgzersizler.SelectedItem is Egzersiz secili)
            {
                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM egzersizadi WHERE egzersiz_adi = @ad";
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ad", secili.EgzersizAdi);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    Egzersizler.Remove(secili);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Egzersiz silinirken hata: " + ex.Message);
                }
            }
        }

        private void EkleKasGrubu_Click(object sender, RoutedEventArgs e)
        {
            string yeni = txtYeniKasGrubu.Text.Trim();

            if (string.IsNullOrWhiteSpace(yeni))
            {
                MessageBox.Show("Kas grubu adı boş olamaz.");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO kas_gruplari (ad) VALUES (@ad)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ad", yeni);
                        cmd.ExecuteNonQuery();
                    }
                }

                KasGruplari.Add(yeni);
                txtYeniKasGrubu.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kas grubu eklenirken hata: " + ex.Message);
            }
        }

        private void SilKasGrubu_Click(object sender, RoutedEventArgs e)
        {
            string secili = lstKasGruplari.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(secili)) return;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM kas_gruplari WHERE ad = @ad";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ad", secili);
                        cmd.ExecuteNonQuery();
                    }
                }

                KasGruplari.Remove(secili);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kas grubu silinirken hata: " + ex.Message);
            }
        }
    }

    public class Egzersiz
    {
        public string EgzersizAdi { get; set; }
        public string KasGrubu { get; set; }

        public Egzersiz(string ad, string grup)
        {
            EgzersizAdi = ad;
            KasGrubu = grup;
        }

        public override string ToString() => $"{EgzersizAdi} ({KasGrubu})";
    }
    
}
