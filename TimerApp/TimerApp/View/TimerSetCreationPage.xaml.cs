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
	public partial class TimerSetCreationPage : ContentPage
	{
        private TimerSetCreationPageViewModel Vm;
        public ListView TimerSetListView;
        public Button AddItemButton;
        public Grid LayoutGrid;
        public TimerSetCreationPage () 
		{
			InitializeComponent ();
            Vm = new TimerSetCreationPageViewModel();
            LayoutGrid = new Grid();
            LayoutGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            LayoutGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });

            LayoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.05, GridUnitType.Star) });
            LayoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            LayoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.05, GridUnitType.Star) });

            AddItemButton = new Button() { Text = "+" };

            TimerSetListView = new ListView();
            TimerSetListView.SetBinding(ListView.ItemsSourceProperty, "TimerSets");
            TimerSetListView.ItemSelected += TimerSetListView_ItemSelected;
            TimerSetListView.ItemTemplate = CreateTimerSetItemTemplate();
            //TimerSetListView = new SfListView()
            //{

            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //    VerticalOptions = LayoutOptions.CenterAndExpand,

            //};

            LayoutGrid.Children.Add(TimerSetListView, 0, 1);
            LayoutGrid.Children.Add(AddItemButton,1,1);
            //TimerSetListView.VerticalOptions = LayoutOptions.FillAndExpand;
            //TimerSetListView.HorizontalOptions = LayoutOptions.FillAndExpand;
            //TimerSetListView.AutoFitMode = AutoFitMode.None;
            //TimerSetListView.ItemSize = 100;
            //TimerSetListView.SelectionMode = SelectionMode.Single;


            //TimerSetListView.SetBinding(SfListView.ItemsSourceProperty, "TimerList");
            //TimerSetListView.ItemTemplate = CreateTimerSetItemTemplate();

            //TimerSetListView.SelectionChanged += TimerSetListView_SelectionChanged;

            //TimerSetListView.ItemTemplate = CreateTimerSetItemTemplate();
            AddItemButton.Clicked += AddItemButton_Clicked;
            Content = LayoutGrid;
		}


        private void TimerSetListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new TimerCreationPage(Vm.SetId));
           // throw new NotImplementedException();
        }


        //private void TimerSetListView_SelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        //{
        //    Navigation.PushAsync(new TimerCreationPage(Vm.SetId));
        //}
        public void AddItemButton_Clicked(object sender, EventArgs e)
        {
            Vm.AddTimerSet();
            //base.AddItemButton_Clicked(sender, e);
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
                //grid.HeightRequest = 50;
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                //            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                var nameLabel = new Label { FontAttributes = FontAttributes.Bold };
                var durationLabel = new Label();
                var repetitionsLabel = new Label { HorizontalTextAlignment = TextAlignment.End };
                var increaseRepetitionsButton = new Button() { Text = "+" };
                var decreaseRepetitionsButton = new Button() { Text = "-" };

                increaseRepetitionsButton.Clicked += IncreaseRepetitionsButton_Clicked;
                decreaseRepetitionsButton.Clicked += DecreaseRepetitionsButton_Clicked;
                nameLabel.SetBinding(Label.TextProperty, "Name");
                durationLabel.SetBinding(Label.TextProperty, "DurationText");
                repetitionsLabel.SetBinding(Label.TextProperty, "RepetitionsText");

                grid.Children.Add(nameLabel);
                grid.Children.Add(durationLabel, 1, 0);
                grid.Children.Add(repetitionsLabel, 2, 0);
                grid.Children.Add(increaseRepetitionsButton, 3, 0);
                grid.Children.Add(decreaseRepetitionsButton, 4, 0);


                return new ViewCell { View = grid };
            });

            return template;
        }

        private void DecreaseRepetitionsButton_Clicked(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void IncreaseRepetitionsButton_Clicked(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected override void OnDisappearing()
        {
            //Vm.SaveTimerSets(WorkoutNameEntry.Text);
            base.OnDisappearing();
        }
    }
}