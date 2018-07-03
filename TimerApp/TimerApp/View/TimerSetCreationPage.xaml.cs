using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimerApp.Model;
using TimerApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimerApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimerSetCreationPage : ContentPage
	{
        private TimerSetCreationPageViewModel Vm;
        public ListView lv;
        public Button AddItemButton;
        public Grid LayoutGrid;
        private DataTemplate timerSetListViewItemTemplate;
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

            lv = new ListView()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            lv.SetBinding(ListView.ItemsSourceProperty, "TimerSets");
            lv.ItemSelected += TimerSetListView_ItemSelected;
            
            timerSetListViewItemTemplate = CreateTimerSetItemTemplate();
            lv.ItemTemplate = timerSetListViewItemTemplate;
            //TimerSetListView = new SfListView()
            //{

            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //    VerticalOptions = LayoutOptions.CenterAndExpand,

            //};

            LayoutGrid.Children.Add(lv, 1, 0);
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
        public TimerSetCreationPage(string workouId):this()
        {
            Vm.WorkoutId = workouId;
            //Vm.SetId = setId;
        }

        private void TimerSetListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {            
            Navigation.PushAsync(new TimerCreationPage(Vm.WorkoutId, (e.SelectedItem as TimerSet).SetId));
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
            BindingContext = Vm;
        }
        private DataTemplate CreateTimerSetItemTemplate()
        {
            DataTemplate template = new DataTemplate(() =>
            {
                var grid = new Grid();
                //grid.BackgroundColor = Color.Blue;
                //grid.HeightRequest = 50;
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                //            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                var nameLabel = new Label { FontAttributes = FontAttributes.Bold };
                //var durationLabel = new Label();
                var repetitionsLabel = new Label { HorizontalTextAlignment = TextAlignment.End };
                var increaseRepetitionsButton = new Button() { Text = "+" };
                var decreaseRepetitionsButton = new Button() { Text = "-" };

                increaseRepetitionsButton.SetBinding(Button.CommandProperty, "IncreaseRepetitionsCommand");
                decreaseRepetitionsButton.SetBinding(Button.CommandProperty, "DecreaseRepetitionsCommand");
                //increaseRepetitionsButton.Clicked += IncreaseRepetitionsButton_Clicked;
                //decreaseRepetitionsButton.Clicked += DecreaseRepetitionsButton_Clicked;
                nameLabel.SetBinding(Label.TextProperty, "Name");
                //durationLabel.SetBinding(Label.TextProperty, "Repetitions");
                repetitionsLabel.SetBinding(Label.TextProperty, "Repetitions");

                grid.Children.Add(nameLabel,0,0);                
                grid.Children.Add(repetitionsLabel, 1, 0);
                grid.Children.Add(increaseRepetitionsButton, 2, 0);
                grid.Children.Add(decreaseRepetitionsButton, 3, 0);


                return new ViewCell { View = grid };
            });

            return template;
        }

        protected override void OnDisappearing()
        {
            Vm.SaveTimerSets();
            base.OnDisappearing();
        }
    }
}