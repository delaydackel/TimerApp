using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private DataTemplate timerItemTemplate;
        private DataTemplate timerSelectedItemTemplate;
        private TimerCreationPageViewModel Vm;
        public TimerCreationPage ()
		{
			InitializeComponent ();
            timerItemTemplate = CreateTimerItemTemplate();
            timerSelectedItemTemplate = CreateTimerSelectedItemTemplate();
            Vm = new TimerCreationPageViewModel();
            TimerListView.VerticalOptions = LayoutOptions.FillAndExpand;
            TimerListView.HorizontalOptions = LayoutOptions.FillAndExpand;
            TimerListView.AutoFitMode = AutoFitMode.Height;
            TimerListView.SelectionMode = SelectionMode.Single;
            TimerListView.ItemTemplate = timerItemTemplate;
            TimerListView.SelectedItemTemplate = timerSelectedItemTemplate;
            TimerListView.SelectionChanged += TimerListView_SelectionChanged;



        }

        private DataTemplate CreateTimerSelectedItemTemplate()
        {
            DataTemplate template = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.HeightRequest = 300; 
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                var nameLabel = new Label { FontAttributes = FontAttributes.Bold };
                var durationLabel = new Label();
                var repetitionsLabel = new Label { HorizontalTextAlignment = TextAlignment.End };
                var durationPicker = new TimePicker();
                var increaseRepetitionButton = new Button() { Text = "+" };
                var decreaseRepetitionButton = new Button() { Text = "-" };

                nameLabel.SetBinding(Label.TextProperty, "Name");
                durationLabel.SetBinding(Label.TextProperty, "DurationText");
                repetitionsLabel.SetBinding(Label.TextProperty, "RepetitionsText");

                durationPicker.SetBinding(Picker.ItemsSourceProperty, "Duration", BindingMode.TwoWay);
                durationPicker.PropertyChanged += DurationPicker_PropertyChanged;
                grid.Children.Add(nameLabel);
                grid.Children.Add(durationLabel, 1, 0);
                grid.Children.Add(repetitionsLabel, 2, 0);
                grid.Children.Add(durationPicker, 0, 1);
                grid.Children.Add(increaseRepetitionButton, 1, 1);
                grid.Children.Add(decreaseRepetitionButton, 2, 1);


                return new ViewCell { View = grid };
            });

            return template;
        }

        private void DurationPicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Vm.TimerList[Vm.TimerList.IndexOf(TimerListView.CurrentItem as AtomicTimer)].Duration = (sender as TimePicker).Time;
            TimerListView.ForceLayout();
        }

        private void TimerListView_SelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
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
        }

        private DataTemplate CreateTimerItemTemplate()
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

        }

        
    }
}