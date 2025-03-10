

using SnapphaneScoutDistriktBookingApp.Data;

namespace SnapphaneScoutDistriktBookingApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = UserSession.Instance;
            CheckUserSession();
        }

        private async void OnChangeToCanoe(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Canoe());
        }

        private async void OnChangeToCabin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Cabin());
        }

        private async void OnChangeToCampGrounds(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CampGrounds());
        }

        private async void OnChangeToLeanTo(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LeanTo());
        }
        private async void CheckUserSession()
        {
            if (!Data.UserSession.Instance.IsUserSet())
            {
                string userName = await DisplayPromptAsync("Information om användare", "Skriv in ditt namn:");
                string userEmail = await DisplayPromptAsync("Information om användare", "Skriv in din email:");

                if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(userEmail))
                {
                    Data.UserSession.Instance.SetUser(userName, userEmail);
                    await DisplayAlert("Information sparat!", $"Välkommen, {Data.UserSession.Instance.UserName}!", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Ogiltigt namn eller email", "OK");
                }
            }
            else
            {
                await DisplayAlert("Välkommen tillbaka!", $"Hej, {Data.UserSession.Instance.UserName}!", "OK");
            }
        }

        private async void OnResetUser(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Ändra användare", "Är du säker på att du vill ändra användare?", "Ja", "Nej");
            if (confirm)
            {
                Data.UserSession.Instance.ResetUser();
                await DisplayAlert("Användarinformation återställd", "Nuvarande användare ändrad.", "OK");
                CheckUserSession();
                
            }
        }

        
    }

}
