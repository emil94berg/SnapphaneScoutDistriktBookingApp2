using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class AddContactPopUpPage : ContentPage
{
	public AddContactPopUpPage()
	{
		InitializeComponent();
	}

    private async void OnClickedPopPopUp(object sender, EventArgs e)
    {
		var name = xName.Text;
		var email = xEmail.Text;
		var phone = xPhone.Text;
		var contact = new Models.Contact()
		{
			Name = name,
			Email = email,
			PhoneNumber = phone
		};
		await Data.DB.ContactCollection().InsertOneAsync(contact);
		await Navigation.PopAsync();
    }
}