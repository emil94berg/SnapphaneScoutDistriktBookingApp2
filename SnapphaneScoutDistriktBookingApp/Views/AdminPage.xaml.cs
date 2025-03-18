using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.ViewModels;
using System.Threading.Tasks;

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

    

    private async void OnClickedAddContact(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.AddContactPopUpPage());
    }

    private async void OnClickedChangeInfo(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new Views.UpdateInfoPopUpPage());
    }

    private async void OnCheckBoxConformationSendEmail(object sender, CheckedChangedEventArgs e)
    {
        if(sender is CheckBox checkBox && checkBox.BindingContext is Models.Customer costumer && costumer.EmailConformation == false)
        {
            if (e.Value)
            {
                Data.API.SendEmailConformation("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", "emil.berg@campusnykoping.se", costumer.Email, costumer);
                var filter = Builders<Models.Customer>.Filter.Eq(x => x.Id, costumer.Id);
                var update = Builders<Models.Customer>.Update.Set(x => x.EmailConformation, true);
                await Data.DB.BookingCollection().UpdateOneAsync(filter, update);
                Task.Delay(2000);

            }
        }
        
    }
}