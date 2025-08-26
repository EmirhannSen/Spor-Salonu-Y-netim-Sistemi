using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows;

namespace Proje2.View
{
    public partial class EgitmenKayitView : Window
    {
        private Egitmen egitmen;
        string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;

        public EgitmenKayitView()
        {
            InitializeComponent();
        }

        public EgitmenKayitView(Egitmen egitmen) : this()
        {
            this.egitmen = egitmen;
            AdTextBox.Text = egitmen.Ad;
            SoyadTextBox.Text = egitmen.Soyad;
            TelefonTextBox.Text = egitmen.Telefon;
            EmailTextBox.Text = egitmen.Email;
        }

        private void Kaydet_Click(object sender, RoutedEventArgs e)
        {
            // Formdaki boş alanları kontrol et
            if (string.IsNullOrWhiteSpace(AdTextBox.Text) || string.IsNullOrWhiteSpace(SoyadTextBox.Text) ||
                string.IsNullOrWhiteSpace(TelefonTextBox.Text) || string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Eğer boş alan varsa, işlem yapılmaz
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd;

                    // Eğer egitmen null ise yeni kayıt yapıyoruz, yoksa güncelleme işlemi
                    if (egitmen == null) // Yeni kaydetme
                    {
                        cmd = new MySqlCommand("INSERT INTO egitmenler (ad, soyad, telefon, email, kayit_tarihi) VALUES (@ad, @soyad, @telefon, @email, @kayit_tarihi)", conn);
                    }
                    else // Güncelleme
                    {
                        cmd = new MySqlCommand("UPDATE egitmenler SET ad = @ad, soyad = @soyad, telefon = @telefon, email = @email, kayit_tarihi = @kayit_tarihi WHERE id = @id", conn);
                        cmd.Parameters.AddWithValue("@id", egitmen.Id);
                    }

                    cmd.Parameters.AddWithValue("@ad", AdTextBox.Text);
                    cmd.Parameters.AddWithValue("@soyad", SoyadTextBox.Text);
                    cmd.Parameters.AddWithValue("@telefon", TelefonTextBox.Text);
                    cmd.Parameters.AddWithValue("@email", EmailTextBox.Text);
                    cmd.Parameters.AddWithValue("@kayit_tarihi", DateTime.Now.ToString("yyyy-MM-dd"));  // Yalnızca tarih kısmı

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Eğitmen kaydedildi.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }



        private void Iptal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
