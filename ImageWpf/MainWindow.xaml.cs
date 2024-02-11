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

        private async Task Button_Click_2Async(object sender, RoutedEventArgs e)
        {
            try
            {
                string _name = name.Text;
                int _age = int.Parse(age.Text);
                string _address = address.Text;
                string _photo = photo.Text;
                string _role = role.Text;
                int _income = int.Parse(income.Text);

                bool success = await CreateNewUser(_name, _age, _address, _photo, _role, _income);

                if (success)
                {
                    MessageBox.Show("User created successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to create user.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task<bool> CreateNewUser(string name, int age, string address, string photo, string role, int income)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Формируем URL для запроса
                    string apiUrl = $"/CreateNewUser";

                    // Подготавливаем данные для отправки
                    var requestData = new
                    {
                        name,
                        age,
                        address,
                        photo,
                        role,
                        income
                    };

                    // Преобразуем данные в формат JSON
                    string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

                    // Создаем контент для HTTP-запроса
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Отправляем POST-запрос на сервер
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Проверяем успешность запроса
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
