using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimerApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimerApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimerPage : ContentPage
    {
        TimerPageViewModel Vm;
        public TimerPage()
        {
            Vm = new TimerPageViewModel();
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = Vm;
            //Content.BindingContext = Vm;
        }

    }
}