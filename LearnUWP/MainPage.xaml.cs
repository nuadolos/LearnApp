using LearnEFEntities.Http;
using LearnUWP.ErrorModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            var obj = new { Email = EmailUserBox.Text, Password = PasswordUserBox.Text, PasswordConfirm = RePasswordUserBox.Text };
            bool result = await HttpRequest.PostRequestAsync(obj, "http://localhost:5243/api/Account/Regist");

            if (result)
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
                StringBuilder errors = new StringBuilder();

                foreach (var error in HttpRequest.Errors)
                {
                    errors.AppendLine(error.Message);
                }

                ContentDialog contentDialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = errors.ToString(),
                    CloseButtonText = "Ок"
                };

                await contentDialog.ShowAsync();
            }
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var obj = new { Email = EmailUserBox.Text, Password = PasswordUserBox.Text };
            bool result = await HttpRequest.PostRequestAsync(obj, "http://localhost:5243/api/Account/Login");

            if (result)
            {
                ContentDialog contentDialog = new ContentDialog()
                {
                    Title = "Пользователь вошел в систему",
                    CloseButtonText = "Ок"
                };

                await contentDialog.ShowAsync();
            }
            else
            {
                StringBuilder errors = new StringBuilder();

                foreach (var error in HttpRequest.Errors)
                {
                    errors.AppendLine(error.Message);
                }

                ContentDialog contentDialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = errors.ToString(),
                    CloseButtonText = "Ок"
                };

                await contentDialog.ShowAsync();
            }
        }
    }
}
