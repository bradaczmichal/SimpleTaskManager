using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Data.SqlClient;

namespace XamarinTest
{
    public partial class MainPage : ContentPage
    {
        private readonly string ConnectionString = "Server=tcp:mainsrv.database.windows.net,1433;Initial Catalog=Database;Persist Security Info=False;User ID=adminsql;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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
                string username = LoginEntry.Text;
                string password = PasswordEntry.Text;
                if (await CheckIfUsernameExistInDataBase(username, password))
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        await connection.OpenAsync();
                        SqlCommand command = new SqlCommand("SELECT FirstName, LastName FROM [Users] WHERE Username = @Username AND Password = @Password" ,connection);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                string firstName = reader.GetString(0);
                                string lastName = reader.GetString(1);
                                User user = new User(firstName, lastName, username, password);
                                await Navigation.PushAsync(new AppPage(user));
                            }
                            
                        }                                           
                    }
                }
                else
                {
                    throw new Exception("User doesn't exist");
                }                             
            }
            catch (Exception ex)
            {
                await OnDisplayAlert(ex);
                ClearEntry();
            }
        }
        private async Task OnDisplayAlert(Exception ex)
        {
            await DisplayAlert("Error", $"{ex.Message}!", "OK");
        }
        private void ClearEntry()
        {
            LoginEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;          
        }
        private async Task<bool> CheckIfUsernameExistInDataBase(string username,string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT 1 FROM [Users] WHERE Username = @Username AND Password = @Password", connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    object result = await command.ExecuteScalarAsync();
                    return result != null;
                }
            }
            catch (Exception ex)
            {
                await OnDisplayAlert(ex);
                return false;
            }

            }
        }
    }

