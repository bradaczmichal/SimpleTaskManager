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
    public partial class AddTaskPage : ContentPage
    {
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
                    AddTaskEntry.Text = "";
                    throw new Exception("Invalid input!");                   
                }
                user.AddTask(AddTaskEntry.Text);
                await OnDisplayAlert();
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
    }
}