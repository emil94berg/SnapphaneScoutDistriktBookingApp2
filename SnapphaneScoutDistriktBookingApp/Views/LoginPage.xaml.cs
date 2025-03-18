using SnapphaneScoutDistriktBookingApp.Data;
using System.Diagnostics;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string userName = NameEntry.Text?.Trim();
        string userEmail = EmailEntry.Text?.Trim();

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userEmail))
        {
            await DisplayAlert("Fel", "Du m�ste ange b�de namn och email!", "OK");
            return;
        }

        Data.UserSession.Instance.SetUser(userName, userEmail);

        await DisplayAlert("V�lkommen!", $"Hej, {Data.UserSession.Instance.UserName}!", "OK");

        bool isAdmin = await AdminService.CheckIfAdminAsync(
            Data.UserSession.Instance.UserName,
            Data.UserSession.Instance.UserEmail
            );

        if (isAdmin == true)
        {
            Debug.WriteLine("Admin login found");

            Entry passwordEntry = new Entry { Placeholder = "Ditt l�senord" };
            var popupPassword = new ContentPage
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 10,
                    Children =
                 {
                     new Label { Text = "L�senord" },
                     passwordEntry,
                     new Button
                     {
                         Text = "OK",
                         Command = new Command(async () =>
                         {
                             bool isAdmin = await AdminService.TryLoginAdminAsync(
                                 Data.UserSession.Instance.UserName,
                                 Data.UserSession.Instance.UserEmail,
                                 passwordEntry.Text
                             );

                             if (isAdmin)
                             {
                                 await DisplayAlert("Inloggning", "Admin inloggning lyckades!", "OK");
                                 await Navigation.PopModalAsync(); // Close modal on success
                                 
                             }
                             else
                             {
                                 await DisplayAlert("Fel", "Ogiltigt l�senord.", "OK");
                             }
                         })
                     }
                 }
                }
            };
            await Navigation.PushModalAsync(popupPassword);
        }
    }
}