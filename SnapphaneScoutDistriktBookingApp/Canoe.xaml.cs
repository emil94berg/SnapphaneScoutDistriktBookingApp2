using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace SnapphaneScoutDistriktBookingApp;

public partial class Canoe : ContentPage
{
    public Canoe()
	{
		InitializeComponent();
		BindingContext = new BookingViewModel();

	}
    private void OnCheckChange(object sender, CheckedChangedEventArgs e)
    {
		if (e.Value)
		{
			statusLabel.Text = "Scoutmedlem";
			hiddenLabel.IsVisible = true;
			orgNameInput.IsVisible = true;
		}
		else
		{
			statusLabel.Text = "Icke scoutmedlem";
			hiddenLabel.IsVisible = false;
			orgNameInput.IsVisible = false;
		}
    }
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
			if(result.HasValue)
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
    private void OnConformation(object sender, EventArgs e)
    {

		Models.Customer.TypeOfBooking bookingtype = Models.Customer.TypeOfBooking.None;
		if(checkCanoe.IsChecked == true)
		{
			bookingtype |= Models.Customer.TypeOfBooking.Canoe;
		}
		if(checkCabin.IsChecked == true)
		{
			bookingtype |= Models.Customer.TypeOfBooking.Cabin;
		}
		if(checkLeanTo.IsChecked == true)
		{
			bookingtype |= Models.Customer.TypeOfBooking.LeanTo;
		}
		if(checkCampGrounds.IsChecked == true)
		{
			bookingtype |= Models.Customer.TypeOfBooking.CampGrounds;
		}

		
		var custumer = new Models.Customer()
		{
			Name = myName.Text,
			Phone = myPhone.Text,
			Email = myEmail.Text,
			IsOrg = myCheckBox.IsChecked,
			OrgName = (myCheckBox.IsChecked == true ? hiddenLabel.Text : ""),
			StartDate = MyStartDate.Date,
			EndDate = MyEndDate.Date,
			BookingType = bookingtype,
			NumberOfCanoes = int.TryParse(AntalKanoter.Text, out int result) ? result : null,
			NumberOfCabin = int.TryParse(AntalStuga.Text, out int result1) ? result1 : null,
			NumberOfCampground = int.TryParse(Lägerområde.Text, out int result2) ? result2 : null,
			NumberOfLeanTo = int.TryParse(Vindskydd.Text, out int result3) ? result3 : null
		};

		API.SendEmail("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", "emil.berg@campusnykoping.se", custumer.Email, custumer);
		
    }
	

    private void OnCheckCanoe(object sender, CheckedChangedEventArgs e)
    {
		if (e.Value)
		{
			AntalKanoter.IsVisible = true;
		}
		else
		{
			AntalKanoter.IsVisible = false;
        }
    }

    private void OnCheckCabin(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            AntalStuga.IsVisible = true;
        }
        else
        {
            AntalStuga.IsVisible = false;
        }
    }

    private void OnCampGrounds(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            Lägerområde.IsVisible = true;
        }
        else
        {
            Lägerområde.IsVisible = false;
        }
    }

    private void OnLeanTo(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            Vindskydd.IsVisible = true;
        }
        else
        {
            Vindskydd.IsVisible = false;
        }
    }
}