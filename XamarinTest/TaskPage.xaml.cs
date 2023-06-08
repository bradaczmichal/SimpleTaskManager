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
    public partial class TaskPage : ContentPage
    {
        private readonly string ConnectionString = "Server=tcp:mainsrv.database.windows.net,1433;Initial Catalog=Database;Persist Security Info=False;User ID=adminsql;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        User user = new User();
        internal TaskPage(User _user)
        {
            user = _user;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            int UserId = await GetUserIdFromUsername(user.Username);
            await LoadTasksForUser(UserId);
        }
        private void ShowTasks()
        {
            var TasksList = user.TasksList;
            bool isEmpty = TasksList.Count == 0 ? true : false;

            if (isEmpty)
            {
                NoTasksLabel.IsVisible = true;
                listView.IsVisible = false;
            }
            else
            {
                NoTasksLabel.IsVisible = false;
                listView.IsVisible = true;

                var groupedTasks = TasksList.GroupBy(task => task.FinishDateTask.Date);

                listView.ItemsSource = groupedTasks;
            }
        }

        private async void DeleteButtonClicked(object sender, EventArgs e)
        {
            try
            {
                bool answer = await DisplayAlert("Question", "Are you sure you want to delete the task?", "Yes", "No");
                if (answer)
                {
                    Button button = (Button)sender;
                    Tasks task = (Tasks)button.BindingContext;
                    int UserId = await GetUserIdFromUsername(user.Username);
                    int Index = user.TasksList.IndexOf(task);
                    string description = task.Description;
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        await connection.OpenAsync();
                        int TaskId = await GetTaskIdFromDescription(description);
                        SqlCommand command = new SqlCommand("DELETE FROM [Tasks] WHERE TaskId = @TaskId", connection);
                        command.Parameters.AddWithValue("@TaskId", TaskId);
                        await command.ExecuteNonQueryAsync();                            
                    }
                    await LoadTasksForUser(UserId);
                }  
                else
                {
                    return;
                }    
            }
            catch (Exception ex)
            {
               await OnDisplayAlert(ex);
            }
            
        }

        private async void EditButtonClicked(object sender, EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                Tasks task = (Tasks)button.BindingContext;
                string BeforeEditTask = task.Description;
                string Edit = await DisplayPromptAsync("Question", "Edit task:");
                int UserId = await GetUserIdFromUsername(user.Username);
                if (!string.IsNullOrEmpty(Edit))
                {
                    using(SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        await connection.OpenAsync();
                        int TaskId = await GetTaskIdFromDescription(BeforeEditTask);  
                        SqlCommand command = new SqlCommand("UPDATE [Tasks] SET Description = @Description WHERE TaskId = @TaskId", connection);
                        command.Parameters.AddWithValue("@Description", Edit);
                        command.Parameters.AddWithValue("@TaskId", TaskId);
                        await command.ExecuteNonQueryAsync();                        
                    }
                    await LoadTasksForUser(UserId);
                }
                else
                {
                    await LoadTasksForUser(UserId);
                }
            }
            catch(Exception ex)
            {
                await OnDisplayAlert(ex);
            }
        }      
        private async Task OnDisplayAlert(Exception ex)
        {
            await DisplayAlert("Error", $"{ex.Message}", "OK");
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
        public async Task<int> GetTaskIdFromDescription(string description)
        {
            int TaskId = -1;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT TaskId FROM [Tasks] WHERE Description = @Description", connection);
                command.Parameters.AddWithValue("@Description", description);
                object result = await command.ExecuteScalarAsync();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    TaskId = id;
                }

            }
            return TaskId;
        }
        private async Task LoadTasksForUser(int userId)
        {
            user.TasksList.Clear();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT TaskId, Description, BeginDateTask, FinishDateTask FROM [Tasks] WHERE UserId = @UserId ORDER BY FinishDateTask", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    string description = reader.GetString(1);
                    DateTime beginDate = reader.GetDateTime(2);
                    DateTime finishDate = reader.GetDateTime(3);
                    user.AddTask(description, beginDate, finishDate, userId);
                }
                reader.Close();
            }

            user.TasksList.Sort((x, y) => DateTime.Compare(x.FinishDateTask, y.FinishDateTask));

            ShowTasks();
        }

    }
}