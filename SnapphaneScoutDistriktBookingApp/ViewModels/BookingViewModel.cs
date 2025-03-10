using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnapphaneScoutDistriktBookingApp.ViewModels
{
    public partial class BookingViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime startDate = DateTime.Today;

        [ObservableProperty]
        private DateTime endDate = DateTime.Today.AddDays(1);

        public ICommand SelectStartDateCommand => new AsyncRelayCommand(async () => await SelectStartDate());
        public ICommand SelectEndDateCommand => new AsyncRelayCommand(async () => await SelectEndDate());




        private async Task SelectStartDate()
        {
            DateTime? result = await ShowDatePicker(StartDate);
            if (result.HasValue)
            {
                StartDate = result.Value;
            }
        }
        private async Task SelectEndDate()
        {
            DateTime? result = await ShowDatePicker(EndDate);
            if (result.HasValue)
            {
                EndDate = result.Value;
            }
        }
        private async Task<DateTime?> ShowDatePicker(DateTime initalDate)
        {
            var datePicker = new DatePicker { Date = initalDate };

            var popup = new ContentPage
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Children =
                    {
                        new Label { Text = "Välj ett datum", FontSize = 20},
                        datePicker,
                        new Button
                        {
                            Text = "OK",
                            Command = new Command(() => Application.Current.MainPage.Navigation.PopModalAsync())

                        }
                    }
                }
            };
            await Application.Current.MainPage.Navigation.PushModalAsync(popup);
            await Task.Delay(100);

            return datePicker.Date;

        }
    }
}
