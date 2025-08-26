using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using Proje2.View;
using System.Windows.Input;
using System.Collections.ObjectModel;


namespace Proje2.View
{ 
public partial class MainMenuView : Window
{
    private List<MainMenuItem> menuItems;
    public ObservableCollection<MainMenuItem> MenuItems { get; set; }
        public MainMenuView(string adSoyad)
    {
        InitializeComponent();

            HosgeldinAdSoyadText.Text = adSoyad;

            menuItems = new List<MainMenuItem>
{
    new MainMenuItem("Ana Sayfa", "/image/home-button.png", () => new AnaSayfaView()),
    new MainMenuItem("Eğitmenler", "/Imagess/add-group.png", () => new EgitmenlerView()),
    new MainMenuItem("Üye Ekle", "/Imagess/add-user.png", () => new UyeEkle()),
    new MainMenuItem("Üye Listele", "/Imagess/users.png", () => new uyeListele()),
    new MainMenuItem("Egzersiz", "/Imagess/dumbell.png", () => new EgzersizEkrani()),
    new MainMenuItem("Diyet", "/Imagess/balanced-diet.png", () => new DiyetEkrani()),
    new MainMenuItem("Fiyatlandırma", "/image/price-tag.png", () => new FiyatEkle()),
    new MainMenuItem("Kategoriler", "/image/rectangle.png", () => new KategoriEkle()),
    new MainMenuItem("Ödemeler", "/image/dollar.png", () => new OdemeListele())
};


            SideMenu.ItemsSource = menuItems;
    }

        private void SideMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SideMenu.SelectedItem is MainMenuItem selectedItem)
            {
                // Seçilen menü öğesinin sayfasını yeniden oluştur
                var newPage = selectedItem.PageFactory.Invoke();

                // Yeni sayfayı içerik alanına ata
                MainContent.Content = newPage;
            }
        }



        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CikisYap_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Çıkış Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Giriş ekranına geri dön
                LoginView login = new LoginView();
                login.Show();

                // Bu pencereyi kapat
                this.Close();
            }
        }



    }
}
