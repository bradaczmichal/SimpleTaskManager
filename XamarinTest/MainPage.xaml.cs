using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinTest
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void greenClicked(object sender, EventArgs e)
        {
            //labelColor.Text = "Color clicked: green";
        }

        private void blueClicked(object sender, EventArgs e)
        {
            //labelColor.Text = "Color clicked: blue";
        }
    }
}
