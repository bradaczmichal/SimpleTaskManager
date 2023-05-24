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
            var Tasks = user.Tasks;
            bool isEmpty = Tasks.Count == 0 ? true : false;

            if (isEmpty)
            {
                NoTasksLabel.IsVisible = true;
                listView.IsVisible = false;
            }
            else
            {
                NoTasksLabel.IsVisible = false;
                listView.IsVisible = true;                                         
                listView.ItemsSource = Tasks;
            }
        }       
        private void DeleteButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Tasks task = (Tasks)button.BindingContext;
            //user.RemoveTask(task);
        }

        private void EditButtonClicked(object sender, EventArgs e)
        {

        }
    }
}