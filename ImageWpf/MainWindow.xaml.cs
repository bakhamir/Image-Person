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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net;
using ImageWpf.Model;
using Newtonsoft.Json;
using System.Net.Http;

namespace ImageWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = @"C:\Users\БахытжановА\Desktop\cat.png";
            byte[] imageDataPc = File.ReadAllBytes(imagePath);
            string stringDataFromComp = Convert.ToBase64String(imageDataPc);

            string imageUrl = "https://png.pngtree.com/png-clipart/20230512/original/pngtree-isolated-cat-on-white-background-png-image_9158356.png";
            WebClient webClient = new WebClient();
            byte[] byteFromUrl = webClient.DownloadData(imageUrl);
            string stringDataFromUrl = Convert.ToBase64String((byte[])byteFromUrl);

            var data = new Data();
            data.fromPc = stringDataFromComp;
            data.fromUrl = stringDataFromUrl;

            var json = JsonConvert.SerializeObject(data);
            var body = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "http://localhost:5196/UploadImages";
            using (HttpClient client = new HttpClient())

            {
                var response = await client.PostAsync(url, body);
                var res = await response.Content.ReadAsStringAsync();
                bool result = JsonConvert.DeserializeObject<bool>(res);
                MessageBox.Show((result) ? "SUCCESS" : "ERROR");
            }



        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var url = "http://localhost:5196/DownloadImages";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    if (response != null)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        List<Data> result = JsonConvert.DeserializeObject<List<Data>>(json);
                        var fromUrl = Convert.FromBase64String(result[0].fromUrl); 
                        var fromPc = Convert.FromBase64String(result[0].fromPc);

                        image1.Source = (BitmapSource)new ImageSourceConverter().ConvertFrom(fromUrl);
                        image2.Source = (BitmapSource)new ImageSourceConverter().ConvertFrom(fromPc);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var url = "http://localhost:5196/CreateNewUser";
            using (HttpClient client = new HttpClient())
            {

            }
        }
    }
}
