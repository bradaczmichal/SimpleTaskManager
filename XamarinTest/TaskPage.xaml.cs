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

        private void EditButtonClicked(object sender, EventArgs e)
        {

        }
        private async Task OnDisplayAlert(Exception ex)
        {
            await DisplayAlert("Error", $"{ex.Message}", "OK");
        }
    }
}