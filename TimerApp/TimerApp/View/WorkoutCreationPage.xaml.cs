using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimerApp.Control;
using TimerApp.Model;
using TimerApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimerApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutCreationPage : ContentPage
    {
        private WorkoutCreationPageViewModel Vm;
        private DataTemplate workoutListViewItemTemplate;
        public ListView lv;
        public Button AddItemButton;
        public Button ResetItemsButton;
        public Grid LayoutGrid;

        public WorkoutCreationPage()
        {
            InitializeComponent();
            Vm = new WorkoutCreationPageViewModel();
            LayoutGrid = new Grid();
            LayoutGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            LayoutGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
            LayoutGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });

            LayoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.05, GridUnitType.Star) });
            LayoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            LayoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.05, GridUnitType.Star) });

            AddItemButton = new Button() { Text = "+" };
            ResetItemsButton = new Button() { Text = "reset" };
            lv = new ListView()
            {
                
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //BackgroundColor = Color.Red
            };
            workoutListViewItemTemplate = CreateWorkoutItemTemplate();
            lv.ItemTemplate = workoutListViewItemTemplate;
            lv.SetBinding(ListView.ItemsSourceProperty, "WorkoutsCollection");
            lv.ItemSelected += Lv_ItemSelected;
            
            LayoutGrid.Children.Add(lv, 1, 0);
            LayoutGrid.Children.Add(AddItemButton, 1, 1);
            LayoutGrid.Children.Add(ResetItemsButton, 1,2);
            AddItemButton.Clicked += AddItemButton_Clicked;
            ResetItemsButton.Clicked += ResetItemsButton_Clicked;
            Content = LayoutGrid;

        }

        private void ResetItemsButton_Clicked(object sender, EventArgs e)
        {
            Vm.ResetData();
            //throw new NotImplementedException();
        }

        private void AddItemButton_Clicked(object sender, EventArgs e)
        {
            Vm.AddWorkout();

            //throw new NotImplementedException();
        }

        private void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem != null)
            {

                AppCore.CurrentWorkout = (e.SelectedItem as Workout);
                Navigation.PushAsync(new TimerSetCreationPage(AppCore.CurrentWorkout.Id));
            }
        }

        private DataTemplate CreateWorkoutItemTemplate()
        {
            DataTemplate template = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.BackgroundColor = Color.LightGray;
                //grid.HeightRequest = 50;
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var nameLabel = new Label { FontAttributes = FontAttributes.Bold };

                nameLabel.SetBinding(Label.TextProperty, "Name");
                grid.Children.Add(nameLabel, 0, 0);
                //var longPress = new GestureRecognizer();

                return new ViewCell { View = grid };
            });

            return template;
        }

        protected override void OnAppearing()
        {
            lv.SelectedItem = null;
            Vm.LoadWorkouts();
            base.OnAppearing();
            BindingContext = Vm;
        }
        protected override void OnDisappearing()
        {
            Vm.SaveWorkouts();
            base.OnDisappearing();
        }
    }
}