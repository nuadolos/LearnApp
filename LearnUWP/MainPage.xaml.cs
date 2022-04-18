using LearnUWP.ErrorModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LearnUWP
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void WindowBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog contentDialog = new ContentDialog()
            {
                Title = "Подтверждение умственной отсталости",
                Content = "Вы подтверждаете свой кретинизм?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };

            var result = await contentDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
                WindowBtn.Content = "Ты правда кретин";
            else
                WindowBtn.Content = "Ладно, прощен";
        }

        private async void RegistredBtn_Click(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(new { Email = EmailUserBox.Text, Password = PasswordUserBox.Text, PasswordConfirm = RePasswordUserBox.Text });
            var response = await client.PostAsync("http://localhost:5243/api/AccountController/Register",
                new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                ContentDialog contentDialog = new ContentDialog()
                {
                    Title = "Пользователь зарегистрирован",
                    CloseButtonText = "Ок"
                };

                await contentDialog.ShowAsync();
            }
            else
            {
                var errors = JsonConvert.DeserializeObject<List<ValidateError>>(
                    await response.Content.ReadAsStringAsync());
                foreach (var error in errors)
                    ErrorsBlock.Text += error.Message + "\n";
            }
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(new { Email = EmailUserBox.Text, Password = PasswordUserBox.Text, RememberMe = true });
            var response = await client.PostAsync("http://localhost:5243/api/AccountController/Login",
                new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                ContentDialog contentDialog = new ContentDialog()
                {
                    Title = "Пользователь вошел в систему",
                    CloseButtonText = "Ок"
                };

                await contentDialog.ShowAsync();
            }
        }
    }
}
