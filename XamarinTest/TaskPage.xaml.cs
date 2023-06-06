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
    public partial class TaskPage : ContentPage
    {
        User user = new User();
        internal TaskPage(User _user)
        {
            user = _user;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ShowTasks();
           
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
    }
}