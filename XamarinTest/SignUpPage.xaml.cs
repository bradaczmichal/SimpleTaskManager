using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinTest
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignUpPage : ContentPage
	{
		public SignUpPage ()
		{
			InitializeComponent ();
		}

        private async void RegisterClicked(object sender, EventArgs e)
        {
			try
			{
				if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) || string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
					string.IsNullOrWhiteSpace(UsernameEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
	
				{
					throw new Exception("Invalid input");
				}
				string firstName = FirstNameEntry.Text;
				string lastName = LastNameEntry.Text;
				string username = UsernameEntry.Text;
				string password = PasswordEntry.Text;
				User user = new User(firstName,lastName,username,password);
				DisplayGreeting(firstName, lastName);
				await Navigation.PushAsync(new MainPage());
			}
			catch (Exception ex)
            {
				DisplayInvalidInput(ex);
            }
        }
		private async void DisplayInvalidInput(Exception ex)
        {
			await DisplayAlert("Error",$"{ex.Message}", "Ok");
        }
		private async void DisplayGreeting(string FirstName, string LastName)
        {
			await DisplayAlert("Success", $"Congratulations {FirstName } {LastName} your account has been created!", "OK");
        }
    }
}