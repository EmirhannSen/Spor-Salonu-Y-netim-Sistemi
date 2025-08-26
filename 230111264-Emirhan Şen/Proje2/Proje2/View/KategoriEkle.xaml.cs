using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Proje2.View
{
    public partial class KategoriEkle : UserControl
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;

        public KategoriEkle()
        {
            InitializeComponent();
            KategorileriYukle();
        }

        private void KategorileriYukle()
        {
            try
            {
                List<Kategori> kategoriListesi = new List<Kategori>();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Kategoriler";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kategoriListesi.Add(new Kategori
                            {
                                Id = reader.GetInt32("Id"),
                                Ad = reader.GetString("Ad")
                            });
                        }
                    }
                }

                dgKategoriler.ItemsSource = kategoriListesi;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Kategori yüklenirken hata: " + ex.Message);
            }
        }

        private void BtnEkle_Click(object sender, RoutedEventArgs e)
        {
            string kategoriAdi = txtKategori.Text.Trim();

            if (string.IsNullOrWhiteSpace(kategoriAdi))
            {
                MessageBox.Show("Kategori adı boş olamaz.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Kategoriler (Ad) VALUES (@ad)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ad", kategoriAdi);
                        cmd.ExecuteNonQuery();
                    }
                }

                txtKategori.Clear();
                KategorileriYukle(); // Event burada otomatik tetiklenir
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kategori eklenirken hata: " + ex.Message);
            }
        }

        private void BtnSil_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                if (MessageBox.Show("Bu kategoriyi silmek istediğinizden emin misiniz?", "Onay", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "DELETE FROM Kategoriler WHERE Id = @id";
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        KategorileriYukle();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Silme işlemi sırasında hata: " + ex.Message);
                    }
                }
            }
        }

        public class Kategori
        {
            public int Id { get; set; }
            public string Ad { get; set; }
        }
    }
}
