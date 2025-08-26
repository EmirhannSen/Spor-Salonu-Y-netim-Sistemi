using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Proje2.View
{
    public partial class LoginView : Window
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;
        public LoginView()
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

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }


        private void Top_DragDelta(object sender, DragDeltaEventArgs e)
        {
            this.Top += e.VerticalChange;
            this.Height -= e.VerticalChange;
        }

        private void Bottom_DragDelta(object sender, DragDeltaEventArgs e)
        {
            this.Height += e.VerticalChange;
        }

        private void Left_DragDelta(object sender, DragDeltaEventArgs e)
        {
            this.Left += e.HorizontalChange;
            this.Width -= e.HorizontalChange;
        }

        private void Right_DragDelta(object sender, DragDeltaEventArgs e)
        {
            this.Width += e.HorizontalChange;
        }

        private void TopLeft_DragDelta(object sender, DragDeltaEventArgs e)
        {
            this.Left += e.HorizontalChange;
            this.Top += e.VerticalChange;
            this.Width -= e.HorizontalChange;
            this.Height -= e.VerticalChange;
        }

        private void TopRight_DragDelta(object sender, DragDeltaEventArgs e)
        {
            this.Width += e.HorizontalChange;
            this.Top += e.VerticalChange;
            this.Height -= e.VerticalChange;
        }

        private void BottomLeft_DragDelta(object sender, DragDeltaEventArgs e)
        {
            this.Left += e.HorizontalChange;
            this.Width -= e.HorizontalChange;
            this.Height += e.VerticalChange;
        }

        private void BottomRight_DragDelta(object sender, DragDeltaEventArgs e)
        {
            this.Width += e.HorizontalChange;
            this.Height += e.VerticalChange;
        }
        private void SignUpTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RegisterView registerView = new RegisterView();
            registerView.ShowDialog();
        }
        private void Register_Click(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            RegisterView registerView = new RegisterView();
            registerView.ShowDialog();
            this.Show();
        }
        private void ForgotPassword_Click(object sender, MouseButtonEventArgs e)
        {
            ForgotPasswordView forgotPasswordView = new ForgotPasswordView();
            forgotPasswordView.ShowDialog();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string sifre = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Email ve şifre alanları boş bırakılamaz.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT AdSoyad FROM kullanici WHERE Email = @Email AND Sifre = @Sifre";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Sifre", sifre);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string adSoyad = reader["AdSoyad"].ToString(); // emin olduğumuz sütun
                                MainMenuView menu = new MainMenuView(adSoyad);
                                menu.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Email veya şifre hatalı!", "Giriş Başarısız", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Veritabanı bağlantı hatası: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }
    }
}



