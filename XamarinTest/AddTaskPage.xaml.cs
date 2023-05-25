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
        DateTime SelectedTime;
        DateTime Now;
        internal AddTaskPage(User _user)
        {
            user = _user;
            InitializeComponent();
        }
        internal AddTaskPage(User _user, DateTime start, DateTime end)
        {
            user = _user;
            Now = start;
            SelectedTime = end;
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
                user.AddTask(AddTaskEntry.Text, Now, SelectedDateTime);
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
    }
}