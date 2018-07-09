using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimerApp.Control;
using TimerApp.Model;
using TimerApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TimerApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimerCreationPage : ContentPage
	{
        private TimerCreationPageViewModel Vm;
        public SfListView TimerListView;
        public Grid layoutGrid;
        public TimerCreationPage ()
		{
			InitializeComponent ();
            //timerItemTemplate = CreateTimerItemTemplate();
            //timerSelectedItemTemplate = CreateTimerSelectedItemTemplate();
            Vm = new TimerCreationPageViewModel();
            layoutGrid = LayoutGrid;
            TimerListView = new SfListView()
            {

                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                
            };
            layoutGrid.Children.Add(TimerListView, 1, 1);
            TimerListView.VerticalOptions = LayoutOptions.FillAndExpand;
            TimerListView.HorizontalOptions = LayoutOptions.FillAndExpand;
            TimerListView.AutoFitMode = AutoFitMode.None;
            TimerListView.ItemSize = 100;
            TimerListView.SelectionMode = SelectionMode.Single;
            

            TimerListView.SetBinding(SfListView.ItemsSourceProperty, "TimerList");
            TimerListView.ItemTemplate = CreateTimerItemTemplate();
            
            TimerListView.SelectionChanged += TimerListView_SelectionChanged;
            
            Content = layoutGrid;
        }
        public TimerCreationPage(string workoutId, string setId):this()
        {
            Vm.SetId = setId;
            Vm.WorkoutId = workoutId;
            Vm.LoadTimers();
        }

        private DataTemplate CreateTimerSelectedItemTemplate()
        {
            DataTemplate template = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.HeightRequest = 300; 
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.25, GridUnitType.Auto) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.25, GridUnitType.Auto) });
                var nameLabel = new Label { FontAttributes = FontAttributes.Bold };
                var durationLabel = new Label();
                var repetitionsLabel = new Label { HorizontalTextAlignment = TextAlignment.End };
                var nameEntry = new Entry() { Placeholder = "Name" };
                var durationPicker = new TimePicker();
                var increaseRepetitionButton = new Button() { Text = "+" };
                var decreaseRepetitionButton = new Button() { Text = "-" };

                nameLabel.SetBinding(Label.TextProperty, "Name");
                nameEntry.SetBinding(Entry.TextProperty, "Name");
                durationLabel.SetBinding(Label.TextProperty, "DurationText");
                repetitionsLabel.SetBinding(Label.TextProperty, "RepetitionsText");

                durationPicker.SetBinding(TimePicker.TimeProperty, "Duration",BindingMode.OneWayToSource);
                durationPicker.PropertyChanged += DurationPicker_PropertyChanged;
              
                grid.Children.Add(nameLabel);
                grid.Children.Add(durationLabel, 1, 0);
                grid.Children.Add(repetitionsLabel, 2, 0);
                grid.Children.Add(nameEntry, 0, 1);
                grid.Children.Add(durationPicker, 1, 1);
                grid.Children.Add(increaseRepetitionButton, 2, 1);
                grid.Children.Add(decreaseRepetitionButton, 3, 1);

                increaseRepetitionButton.Clicked += IncreaseRepetitionButton_Clicked;
                decreaseRepetitionButton.Clicked += DecreaseRepetitionButton_Clicked;
                return new ViewCell { View = grid };
            });

            return template;
        }

        private void DecreaseRepetitionButton_Clicked(object sender, EventArgs e)
        {
            if((TimerListView.CurrentItem as AtomicTimer).Repetitions >= 1)
            {
                Vm.TimerList[Vm.TimerList.IndexOf(TimerListView.CurrentItem as AtomicTimer)].Repetitions--;
                //(TimerListView.CurrentItem as AtomicTimer).Repetitions--;
                TimerListView.ItemsSource = Vm.TimerList;
                TimerListView.SelectedItemTemplate = CreateTimerSelectedItemTemplate();
                TimerListView.ItemTemplate = CreateTimerItemTemplate();
                //TimerListView.ForceUpdateItemSize();
            }
            
        }

        private void IncreaseRepetitionButton_Clicked(object sender, EventArgs e)
        {
            Vm.TimerList[Vm.TimerList.IndexOf(TimerListView.CurrentItem as AtomicTimer)].Repetitions++;
            //(TimerListView.CurrentItem as AtomicTimer).Repetitions++;
            //TimerListView.ItemsSource=Vm.TimerList;
            TimerListView.ForceUpdateItemSize(Vm.TimerList.IndexOf(TimerListView.CurrentItem as AtomicTimer));
            TimerListView.SelectedItemTemplate = CreateTimerSelectedItemTemplate();
            TimerListView.ItemTemplate = CreateTimerItemTemplate();

        }

        private void DurationPicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time")
            {
                TimeSpan duration;
                duration = new TimeSpan((long)((sender as TimePicker).Time.Ticks) / 60);
                Vm.TimerList[Vm.TimerList.IndexOf(TimerListView.CurrentItem as AtomicTimer)].Duration = duration;


            }

        }

        private void TimerListView_SelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            TimerListView.SelectedItemTemplate = CreateTimerSelectedItemTemplate();
            TimerListView.ItemTemplate = CreateTimerItemTemplate();
            //TimerListView.= 100;
            TimerListView.ForceUpdateItemSize(Vm.TimerList.IndexOf(TimerListView.CurrentItem as AtomicTimer));
            //try
            //{
            //    if (e.AddedItems.Count == 0) return; // don't do anything if we just de-selected the row


            //    // Search the current (last) selected item
            //    //dirty hax cuz observablecollection
            //    var blub = Vm.TimerList.Where(o => o.IsSelected == true);
            //    int lastIdx;
            //    if (blub.Count() != 0) { lastIdx = Vm.TimerList.IndexOf(blub.First()); }
            //    else { lastIdx = -1; }


            //    if (lastIdx >= 0)
            //    {
            //        Vm.TimerList[lastIdx].IsSelected = false; // De-select the last selected because now I have another selected item
            //    }


            //    // Search on your list the selectedItem
            //    int idx = Vm.TimerList.IndexOf((AtomicTimer)e.AddedItems[0]);

            //    // Set "Selected" to selected item
            //    Vm.TimerList[idx].IsSelected = true;



            //    ((SfListView)sender).SelectedItem = null; // de-select the row

            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = Vm;
            WorkoutNameEntry.Text = AppCore.CurrentWorkout.Timers.Where(set => set.SetId == Vm.SetId).First().Name;
        }

        public virtual DataTemplate CreateTimerItemTemplate()
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

       

        private void WorkoutNameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != e.OldTextValue)
            {
                var currentSet = AppCore.CurrentWorkout.Timers.Where(set => set.SetId == Vm.SetId).First();
                currentSet.Name = e.NewTextValue;
            }            
        }

        public virtual void AddItemButton_Clicked(object sender, EventArgs e)
        {
            Vm.AddTimer();
        }
        protected override void OnDisappearing()
        {
            Vm.SaveWorkouts(WorkoutNameEntry.Text);
            base.OnDisappearing();
        }
    }
}