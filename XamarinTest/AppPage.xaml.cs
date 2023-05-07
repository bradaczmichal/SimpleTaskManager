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
	public partial class Page1 : ContentPage
	{
		public Page1 ()
		{
			InitializeComponent ();
		}

        private void TasksClicked(object sender, EventArgs e)
        {
			Navigation.PushAsync(new TaskPage());
        }
		
        private void AddTasksClicked(object sender, EventArgs e)
        {
			Navigation.PushAsync(new AddTaskPage());
        }

        private void LogOutClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}