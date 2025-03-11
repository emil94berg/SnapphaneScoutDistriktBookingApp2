using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnapphaneScoutDistriktBookingApp.ViewModels
{
    public partial class BookingViewModel : ObservableObject
    {
        public BookingViewModel()
        {
            LoadViableNumbers();
        }

        
        [ObservableProperty]
        private string _canoesAvailable;
       
        [ObservableProperty]
        private string _cabinAvailable;
       
        [ObservableProperty]
        private string _leanToAvailable;
       
        [ObservableProperty]
        private string _campGroundAvailable;
       

        public async void LoadViableNumbers()
        {
            int[] thisNumbers = await ViableCanoesInt();
            CanoesAvailable = (14-thisNumbers[0]).ToString();
            CabinAvailable = (1-thisNumbers[1]).ToString();
            LeanToAvailable = (4-thisNumbers[2]).ToString();
            CampGroundAvailable = thisNumbers[3].ToString();

        }

        public async Task<int[]> ViableCanoesInt()
        {
            DateTime start = NewStartDate;
            DateTime end = NewEndDate;
            var bookingCollection = await Data.DB.BookingCollection().Find(Builders<Models.Customer>.Filter.Where(x => x.EndDate >= end && x.StartDate <= start)).ToListAsync();
            int[] totalSum = new int[4];
            totalSum[0] = bookingCollection.Sum(x => x.NumberOfCanoes.GetValueOrDefault());
            
            totalSum[1] = bookingCollection.Sum(x => x.NumberOfCabin.GetValueOrDefault());
            
            totalSum[2] = bookingCollection.Sum(x => x.NumberOfLeanTo.GetValueOrDefault());
            
            totalSum[3] = bookingCollection.Sum(x => x.NumberOfCampground.GetValueOrDefault());
            
            return totalSum;
        }




















        //----------------------------------------------------------------------------------------
        [ObservableProperty]
        private DateTime newStartDate = DateTime.Today;

        [ObservableProperty]
        private DateTime newEndDate = DateTime.Today.AddDays(1);

        public ICommand SelectStartDateCommand => new AsyncRelayCommand(async () => await SelectStartDate());
        public ICommand SelectEndDateCommand => new AsyncRelayCommand(async () => await SelectEndDate());




        private async Task SelectStartDate()
        {
            DateTime? result = await ShowDatePicker(NewStartDate);
            if (result.HasValue)
            {
                NewStartDate = result.Value;
                
            }
        }
        private async Task SelectEndDate()
        {
            DateTime? result = await ShowDatePicker(NewEndDate);
            if (result.HasValue)
            {
                NewEndDate = result.Value;
                
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
