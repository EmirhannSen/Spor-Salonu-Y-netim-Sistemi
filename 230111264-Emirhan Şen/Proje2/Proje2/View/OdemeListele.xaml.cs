using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Proje2.View
{
    public partial class OdemeListele : UserControl
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;

        public OdemeListele()
        {
            InitializeComponent();
            ListeleOdemeler();
        }

        private void ListeleOdemeler()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT o.OdemeID, CONCAT(u.ad, ' ', u.soyad) AS UyeAdiSoyad, DATE(o.OdemeTarihi) AS OdemeTarihi, o.Tutar " +
                                   "FROM odemeler o INNER JOIN uyeler u ON o.UyeID = u.id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // DataGrid'e veri bağla
                    dataGridOdemeler.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }



        private void Sil_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridOdemeler.SelectedItem == null)
            {
                MessageBox.Show("Lütfen silinecek ödemeyi seçin.");
                return;
            }

            DataRowView selectedRow = (DataRowView)dataGridOdemeler.SelectedItem;
            int odemeId = Convert.ToInt32(selectedRow["OdemeID"]);

            var result = MessageBox.Show("Seçili ödemeyi silmek istediğinizden emin misiniz?", "Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM odemeler WHERE OdemeID = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", odemeId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Ödeme silindi.");
                    ListeleOdemeler(); // Listeyi yenile
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme hatası: " + ex.Message);
                }
            }
        }
    }
}
