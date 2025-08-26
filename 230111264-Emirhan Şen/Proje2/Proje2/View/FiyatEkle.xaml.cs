using System;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Proje2.View
{
    public partial class FiyatEkle : UserControl
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;

        public FiyatEkle()
        {
            InitializeComponent();
            KategorileriYukle();
            ListeleFiyatlar();
        }

        private void KategorileriYukle()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Id, Ad FROM kategoriler";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbKategori.ItemsSource = dt.DefaultView;
                    cmbKategori.DisplayMemberPath = "Ad";
                    cmbKategori.SelectedValuePath = "Ad";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kategoriler yüklenirken hata: " + ex.Message);
            }
        }

        private void ListeleFiyatlar()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT f.Id, f.Kategori AS KategoriAdi, f.SureAy, f.Fiyat
                        FROM fiyatlar f
                        LEFT JOIN kategoriler k ON f.Kategori = k.Ad";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridFiyatlar.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fiyatlar yüklenirken hata: " + ex.Message);
            }
        }

        private void BtnKaydet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbKategori.SelectedItem == null ||
                    !int.TryParse(txtSure.Text, out int sureAy) ||
                    !decimal.TryParse(txtFiyat.Text, out decimal fiyat))
                {
                    MessageBox.Show("Lütfen geçerli bilgiler girin.");
                    return;
                }

                string kategoriAdi = ((DataRowView)cmbKategori.SelectedItem)["Ad"].ToString();

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO fiyatlar (Kategori, SureAy, Fiyat) VALUES (@kategoriAd, @sure, @fiyat)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kategoriAd", kategoriAdi);
                        cmd.Parameters.AddWithValue("@sure", sureAy);
                        cmd.Parameters.AddWithValue("@fiyat", fiyat);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Fiyat başarıyla eklendi.");
                txtSure.Clear();
                txtFiyat.Clear();
                cmbKategori.SelectedIndex = -1;

                KategorileriYukle();
                ListeleFiyatlar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt sırasında hata: " + ex.Message);
            }
        }

        private void SilButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridFiyatlar.SelectedItem == null)
            {
                MessageBox.Show("Silmek için bir satır seçiniz.");
                return;
            }

            DataRowView selected = dataGridFiyatlar.SelectedItem as DataRowView;
            int fiyatId = Convert.ToInt32(selected["Id"]);

            MessageBoxResult result = MessageBox.Show("Seçili fiyatı silmek istiyor musunuz?", "Onay", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM fiyatlar WHERE Id = @id";
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", fiyatId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Fiyat silindi.");
                    ListeleFiyatlar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme hatası: " + ex.Message);
                }
            }
        }
    }
}
