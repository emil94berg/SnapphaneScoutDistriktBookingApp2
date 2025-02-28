namespace SnapphaneScoutDistriktBookingApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
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

        //private void OnCounterClicked(object sender, EventArgs e)
        //{
        //    count++;

        //    if (count == 1)
        //        CounterBtn.Text = $"Clicked {count} time";
        //    else
        //        CounterBtn.Text = $"Clicked {count} times";

        //    SemanticScreenReader.Announce(CounterBtn.Text);
        //}
    }

}
