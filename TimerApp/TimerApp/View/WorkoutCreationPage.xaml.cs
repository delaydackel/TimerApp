﻿using System;
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
    public partial class WorkoutCreationPage : ContentPage
    {
        private WorkoutCreationPageViewModel Vm;
        private DataTemplate workoutListViewItemTemplate;
        public ListView lv;
        public Button AddItemButton;
        public Grid LayoutGrid;

        public WorkoutCreationPage()
        {
            InitializeComponent();
            Vm = new WorkoutCreationPageViewModel();
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
            workoutListViewItemTemplate = CreateWorkoutItemTemplate();
            lv.ItemTemplate = workoutListViewItemTemplate;
            lv.SetBinding(ListView.ItemsSourceProperty, "WorkoutsCollection");
            lv.ItemSelected += Lv_ItemSelected;
            LayoutGrid.Children.Add(lv, 1, 0);
            LayoutGrid.Children.Add(AddItemButton, 1, 1);
            AddItemButton.Clicked += AddItemButton_Clicked;
            Content = LayoutGrid;

        }

        private void AddItemButton_Clicked(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new TimerSetCreationPage((e.SelectedItem as Workout).Id));
        }

        private DataTemplate CreateWorkoutItemTemplate()
        {
            DataTemplate template = new DataTemplate(() =>
            {
                var grid = new Grid();
                //grid.BackgroundColor = Color.Blue;
                //grid.HeightRequest = 50;
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var nameLabel = new Label { FontAttributes = FontAttributes.Bold };

                nameLabel.SetBinding(Label.TextProperty, "Name");
                grid.Children.Add(nameLabel, 0, 0);

                return new ViewCell { View = grid };
            });

            return template;
        }

        protected override void OnAppearing()
        {
            Vm.LoadWorkouts();
            base.OnAppearing();
        }
    }
}