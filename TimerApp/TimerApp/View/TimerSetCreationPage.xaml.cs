using Syncfusion.ListView.XForms;
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
	public partial class TimerSetCreationPage : TimerCreationPage
	{
        private TimerSetCreationPageViewModel Vm;
        public SfListView TimerSetListView;
		public TimerSetCreationPage () 
		{
			InitializeComponent ();
            Vm = new TimerSetCreationPageViewModel();
            TimerSetListView = new SfListView()
            {

                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,

            };
            layoutGrid.Children.Add(TimerSetListView, 1, 1);
            TimerSetListView.VerticalOptions = LayoutOptions.FillAndExpand;
            TimerSetListView.HorizontalOptions = LayoutOptions.FillAndExpand;
            TimerSetListView.AutoFitMode = AutoFitMode.None;
            TimerSetListView.ItemSize = 100;
            TimerSetListView.SelectionMode = SelectionMode.Single;


            TimerSetListView.SetBinding(SfListView.ItemsSourceProperty, "TimerList");
            TimerSetListView.ItemTemplate = CreateTimerSetItemTemplate();

            TimerSetListView.SelectionChanged += TimerSetListView_SelectionChanged;

            TimerSetListView.ItemTemplate = CreateTimerSetItemTemplate();
		}

        private void TimerSetListView_SelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            Navigation.PushAsync(new TimerCreationPage(Vm.SetId));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private DataTemplate CreateTimerSetItemTemplate()
        {
            DataTemplate template = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.HeightRequest = 50;
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                //            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                var nameLabel = new Label { FontAttributes = FontAttributes.Bold };
                var durationLabel = new Label();
                var repetitionsLabel = new Label { HorizontalTextAlignment = TextAlignment.End };


                nameLabel.SetBinding(Label.TextProperty, "Name");
                durationLabel.SetBinding(Label.TextProperty, "DurationText");
                repetitionsLabel.SetBinding(Label.TextProperty, "RepetitionsText");

                grid.Children.Add(nameLabel);
                grid.Children.Add(durationLabel, 1, 0);
                grid.Children.Add(repetitionsLabel, 2, 0);


                return new ViewCell { View = grid };
            });

            return template;
        }

    }
}