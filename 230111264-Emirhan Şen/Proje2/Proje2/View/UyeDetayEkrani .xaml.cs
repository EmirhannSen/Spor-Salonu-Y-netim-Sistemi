using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Proje2.View
{

    public partial class UyeDetayEkrani : Window
    {
        private int uyeId;
        public Action UyeSilindiCallback { get; set; } // dışarıdan atanacak

        public UyeDetayEkrani(string ad, string soyad, string telefon, int yas, double boy,
                              double kilo, string cinsiyet, int kayitSuresi, string kategori,
                              DateTime dogumTarihi, DateTime kayitTarihi, DateTime bitisTarihi, int id)
        {
            InitializeComponent();

            uyeId = id;

            lblAd.Text = ad;
            lblSoyad.Text = soyad;
            lblTelefon.Text = telefon;
            lblYas.Text = yas.ToString();
            lblBoy.Text = boy.ToString();
            lblKilo.Text = kilo.ToString();
            lblCinsiyet.Text = cinsiyet;
            lblKayitSuresi.Text = kayitSuresi.ToString();
            lblKategori.Text = kategori;
            lblDogumTarihi.Text = dogumTarihi.ToString("dd.MM.yyyy");
            lblKayitTarihi.Text = kayitTarihi.ToString("dd.MM.yyyy");
            lblBitisTarihi.Text = bitisTarihi.ToString("dd.MM.yyyy");
        }

        private void BtnProgramAta_Click(object sender, RoutedEventArgs e)
        {
            string adSoyad = lblAd.Text + " " + lblSoyad.Text;
            ProgramAta ekran = new ProgramAta(uyeId, adSoyad);
            ekran.Show();
            this.Close();
        }

        private void BtnDiyetEkle_Click(object sender, RoutedEventArgs e)
        {
            string adSoyad = lblAd.Text + " " + lblSoyad.Text;
            DiyetEkle ekran = new DiyetEkle(uyeId, adSoyad);
            ekran.Show();
            this.Close();
        }





        private void BtnGuncelle_Click(object sender, RoutedEventArgs e)
        {
            // Üye detaylarını almak için gereken veriler (Bu verileri formdaki alanlardan alıyoruz)
            string ad = lblAd.Text;
            string soyad = lblSoyad.Text;
            string telefon = lblTelefon.Text;
            int yas = int.Parse(lblYas.Text);
            double boy = double.Parse(lblBoy.Text);
            double kilo = double.Parse(lblKilo.Text);
            DateTime dogumTarihi = DateTime.Parse(lblDogumTarihi.Text);

            // Güncelleme ekranını göster
            GuncellemeEkrani guncelle = new GuncellemeEkrani(ad, soyad, telefon, yas, boy, kilo, dogumTarihi, uyeId);
            guncelle.ShowDialog();
            
            // Kapanınca mevcut ekranı da kapatabiliriz ya da bilgileri yenileyebiliriz
            this.Close();
        }



        private void BtnSil_Click(object sender, RoutedEventArgs e)
{
    MessageBoxResult sonuc = MessageBox.Show("Bu üyeyi silmek istediğinize emin misiniz?", "Üye Sil", MessageBoxButton.YesNo, MessageBoxImage.Warning);

    if (sonuc == MessageBoxResult.Yes)
    {
        try
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbBaglantisi"].ConnectionString;

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();

                using (var tran = conn.BeginTransaction()) // işlemleri bir bütün olarak yap
                {
                    try
                    {
                        // 1. Egzersiz programı sil
                        string deleteProgramQuery = "DELETE FROM programlar WHERE uye_id = @uyeId";
                        using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(deleteProgramQuery, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@uyeId", uyeId);
                            cmd.ExecuteNonQuery();
                        }

                        // 2. Diyet programı sil
                        string deleteDiyetQuery = "DELETE FROM uye_diyet_programi WHERE uye_id = @uyeId";
                        using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(deleteDiyetQuery, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@uyeId", uyeId);
                            cmd.ExecuteNonQuery();
                        }

                        // 3. Üyeyi sil
                        string deleteUserQuery = "DELETE FROM uyeler WHERE id = @uyeId";
                        using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(deleteUserQuery, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@uyeId", uyeId);
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit(); // tüm işlemler başarılıysa commit et

                        MessageBox.Show("Üye ve ilişkili kayıtlar başarıyla silindi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                        UyeSilindiCallback?.Invoke();
                        this.Close();
                    }
                    catch (Exception exInner)
                    {
                        tran.Rollback(); // hata olursa işlemleri geri al
                        MessageBox.Show($"Silme işlemi başarısız oldu: {exInner.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Veritabanı bağlantı hatası: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}





        private void GeriButton_Click(object sender, RoutedEventArgs e)
        {
            // Önceki ekrana dönmek için, örneğin:
            this.Close(); // Bu, şu anki pencereyi kapatır.
        }


    }



}
