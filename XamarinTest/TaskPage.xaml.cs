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

            string[] tasks = user.Tasks;
            List<ItemViewModel>items = new List<ItemViewModel>();

            bool CheckIfEmpty = tasks.All(string.IsNullOrEmpty);

            if (CheckIfEmpty)
            {
                NoTasksLabel.IsVisible = true;
                listView.IsVisible = false;
            }
            else
            {
                NoTasksLabel.IsVisible = false;
                listView.IsVisible = true;
                for (int i = 0; i < tasks.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(tasks[i]))
                    {
                        string numberWithDot = $"{i + 1}.";
                        items.Add(new ItemViewModel { Number = numberWithDot, Text = tasks[i] });
                    }
                }
                listView.ItemsSource = items;
            }          
        }
    }
}