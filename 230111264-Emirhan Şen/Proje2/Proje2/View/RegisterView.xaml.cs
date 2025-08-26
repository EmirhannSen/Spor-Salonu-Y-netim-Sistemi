using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;

namespace Proje2.View
{
    public partial class RegisterView : Window
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;

        public RegisterView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AdSoyadTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                ErrorMessage.Text = "Lütfen tüm alanları doldurun.";
                ErrorMessage.Visibility = Visibility.Visible;
                SuccessMessage.Visibility = Visibility.Collapsed;
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    // 1. E-posta kontrolü
                    string kontrolQuery = "SELECT COUNT(*) FROM kullanici WHERE Email = @Email";
                    using (MySqlCommand kontrolCmd = new MySqlCommand(kontrolQuery, conn))
                    {
                        kontrolCmd.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                        int existingCount = Convert.ToInt32(await kontrolCmd.ExecuteScalarAsync());

                        if (existingCount > 0)
                        {
                            ErrorMessage.Text = "Bu e-posta adresi zaten kayıtlı.";
                            ErrorMessage.Visibility = Visibility.Visible;
                            SuccessMessage.Visibility = Visibility.Collapsed;
                            return;
                        }
                    }

                    // 2. Kayıt işlemi
                    string insertQuery = "INSERT INTO kullanici (AdSoyad, Email, Sifre) VALUES (@AdSoyad, @Email, @Sifre)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@AdSoyad", AdSoyadTextBox.Text);
                        cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                        cmd.Parameters.AddWithValue("@Sifre", PasswordBox.Password);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                SuccessMessage.Visibility = Visibility.Visible;
                ErrorMessage.Visibility = Visibility.Collapsed;
                await Task.Delay(2000);
                this.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = "Hata: " + ex.Message;
                ErrorMessage.Visibility = Visibility.Visible;
                SuccessMessage.Visibility = Visibility.Collapsed;
            }
        }

    }
}
