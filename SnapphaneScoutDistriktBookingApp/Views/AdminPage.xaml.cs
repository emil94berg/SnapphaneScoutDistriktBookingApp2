namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class AdminPage : ContentPage
{
	public AdminPage()
	{
		InitializeComponent();
		BindingContext = new ViewModels.AdminPageViewModel();
	}

    private async void OnBookingSelected(object sender, SelectedItemChangedEventArgs e)
    {
		var booking = ((ListView)sender).SelectedItem as Models.Customer;
		if(booking != null)
		{
			var page = new BookingPopUpPage();
			page.BindingContext = booking;
			await Navigation.PushAsync(page);
		}
    }
}