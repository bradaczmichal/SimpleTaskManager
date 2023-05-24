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
    public partial class AppPage : ContentPage
    {
        User user = new User();
        internal AppPage(User _user)
        {
            user = _user;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            FirstNameLabel.Text = "First name: " + user.FirstName;
            LastNameLabel.Text = "Last name: " + user.LastName;
            UsernameLabel.Text = "Username: " + user.Username;
        }
        private void TasksClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TaskPage(user));
        }

        private void AddTasksClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddTaskPage(user));
        }

        private void LogOutClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}