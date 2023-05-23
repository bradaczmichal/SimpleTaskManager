using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinTest
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void SignUpClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }

        private void ExitClicked(object sender, EventArgs e)
        {
           System.Environment.Exit(0);
        }
        
        private async void LoginClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LoginEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
                {
                    throw new Exception("Invalid input");
                }
                await Navigation.PushAsync(new AppPage());
            }
            catch (Exception ex)
            {
                OnDisplayAlert(ex);
            }
        }
        private async void OnDisplayAlert(Exception ex)
        {
            await DisplayAlert("Error", $"{ex.Message}", "Ok");
        }
    }
}
