using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data;
using System.Data.SqlClient;
using Xamarin.Essentials;

namespace XamarinTest
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignUpPage : ContentPage
	{
        private readonly SqlConnection ConnectionString = new SqlConnection(@"Server=tcp:mainsrv.database.windows.net,1433;Initial Catalog=Database;Persist Security Info=False;User ID=adminsql;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
				await SaveToDataBase (firstName, lastName, username, password);
				await OnDisplayAlert(firstName, lastName);		
				await Navigation.PushAsync(new MainPage());
			}
			catch (Exception ex)
            {
				await OnDisplayAlert(ex);
				ClearEntry();
            }
        }
		private async Task OnDisplayAlert(Exception ex)
        {
			await DisplayAlert("Error",$"{ex.Message}", "OK");
        }
		private async Task OnDisplayAlert(string FirstName, string LastName)
        {
			await DisplayAlert("Success", $"Congratulations {FirstName } {LastName} your account has been created!", "OK");
        }
		private void ClearEntry()
        {
			FirstNameEntry.Text = string.Empty;
			LastNameEntry.Text = string.Empty;
			UsernameEntry.Text = string.Empty;
			PasswordEntry.Text = string.Empty;
		}
        private async Task SaveToDataBase(string firstName, string lastName,  string username, string password)
        {
            try
            {
                ConnectionString.Open();
                SqlCommand insert = new SqlCommand("INSERT INTO [Users] VALUES (@FirstName, @LastName, @Username, @Password)", ConnectionString);
                insert.Parameters.AddWithValue("@FirstName", firstName);
                insert.Parameters.AddWithValue("@LastName", lastName);
                insert.Parameters.AddWithValue("@Username", username);
                insert.Parameters.AddWithValue("@Password", password);
                insert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                await OnDisplayAlert(ex);
            }
            finally
            {
                ConnectionString.Close();
            }
        }
    }
}