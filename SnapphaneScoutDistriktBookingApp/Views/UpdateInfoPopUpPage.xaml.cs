namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class UpdateInfoPopUpPage : ContentPage
{
	public UpdateInfoPopUpPage()
	{
		InitializeComponent();
		BindingContext = new ViewModels.InfoPageViewModel();
	}
}