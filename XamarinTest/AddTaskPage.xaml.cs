using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data.SqlClient;

namespace XamarinTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTaskPage : ContentPage
    {
        private readonly string ConnectionString = "Server=tcp:mainsrv.database.windows.net,1433;Initial Catalog=Database;Persist Security Info=False;User ID=adminsql;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        User user = new User();
        internal AddTaskPage(User _user)
        {
            user = _user;
            InitializeComponent();
        }


        private async void AddTaskClicked(object sender, EventArgs e)
        {
            try
            {  
                if(string.IsNullOrWhiteSpace(AddTaskEntry.Text))
                {                    
                    throw new Exception("Invalid input!");                   
                }
                DateTime SelectedDateTime = datePicker.Date + timePicker.Time;
                DateTime Now = DateTime.Now;
                int UserId = await GetUserIdFromUsername(user.Username);
                await AddTaskToUser(AddTaskEntry.Text, Now, SelectedDateTime, UserId);
                await OnDisplayAlert();
                ClearEntry();
            }
            catch (Exception ex)
            {                
                await OnDisplayAlert(ex);
            }
            
        }
        private async Task OnDisplayAlert(Exception ex)
        {
            await DisplayAlert("Error", $"{ex.Message}", "OK");
        }
        private async Task OnDisplayAlert()
        {
            await DisplayAlert("Success", "Your task has been added!", "OK");
        }
        private void ClearEntry()
        {
            AddTaskEntry.Text = string.Empty;
        }
        public async Task<int> GetUserIdFromUsername(string username)
        {
            int userId = -1;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT UserId FROM [Users] WHERE Username = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);
                object result = await command.ExecuteScalarAsync();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    userId = id;
                }
            }
            return userId;
        }
        public async Task AddTaskToUser(string description, DateTime start, DateTime end, int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("INSERT INTO [Tasks] (Description, BeginDateTask, FinishDateTask, UserId) VALUES (@Description, @BeginDateTask, @FinishDateTask, @UserId)", connection);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@BeginDateTask", start);
                    command.Parameters.AddWithValue("@FinishDateTask", end);
                    command.Parameters.AddWithValue("@UserId", userId);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                await OnDisplayAlert(ex);
            }
        }

    }
}