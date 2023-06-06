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
                listView.ItemsSource = null;
                listView.ItemsSource = TasksList;
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
                    int Index = user.TasksList.IndexOf(task);
                    user.RemoveTask(Index);
                    ShowTasks();
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
                int Index = user.TasksList.IndexOf(task);
                string Edit = await DisplayPromptAsync("Question", "Edit task:");
                if(!string.IsNullOrEmpty(Edit))
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        await connection.OpenAsync();
                        SqlCommand command = new SqlCommand("UPDATE [Tasks] SET Description = @Description WHERE Description = @BeforeDescription", connection);
                        command.Parameters.AddWithValue("@Description", Edit);
                        command.Parameters.AddWithValue("@BeforeDescription", BeforeEditTask);
                        await command.ExecuteNonQueryAsync();
                    }
                    user.EditTask(Index, Edit);
                }
                else
                {
                    user.EditTask(Index, BeforeEditTask);
                }                
                ShowTasks();
            }
            catch(Exception ex)
            {
                await OnDisplayAlert(ex);
            }
        }

        private async void ListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Tasks task)
            {
                string AlertMessage = "Task begin: " + task.BeginDateTask + "\n" + "Task finish: " + task.FinishDateTask;
                await OnDisplayAlert(AlertMessage);
            }
            if (sender is ListView listView)
                listView.SelectedItem = null;
        }
        private async Task OnDisplayAlert(Exception ex)
        {
            await DisplayAlert("Error", $"{ex.Message}", "OK");
        }

        private async Task OnDisplayAlert(string message)
        {
           await DisplayAlert("Task duration", message, "OK");
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
        private async Task LoadTasksForUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT TaskId, Description, BeginDateTask, FinishDateTask FROM [Tasks] WHERE UserId = @UserId", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    int taskId = reader.GetInt32(0);
                    string description = reader.GetString(1);
                    DateTime beginDate = reader.GetDateTime(2);
                    DateTime finishDate = reader.GetDateTime(3);

                    if (!user.TasksList.Exists(task => (task.Description == description)
                        && (task.BeginDateTask == beginDate) && (task.FinishDateTask) == finishDate))
                    {
                        user.AddTask(description, beginDate, finishDate, userId);
                    }
                    else
                    {
                        continue;
                    }
                }
                reader.Close();
            }
            ShowTasks();
        }       
    }
}