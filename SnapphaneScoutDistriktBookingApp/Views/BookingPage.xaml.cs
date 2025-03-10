using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Syncfusion.Maui.Calendar;
using SnapphaneScoutDistriktBookingApp.Data;
using MongoDB.Driver;

namespace SnapphaneScoutDistriktBookingApp;

public partial class Canoe : ContentPage
{
    public Canoe()
	{
		InitializeComponent();
		BindingContext = new ViewModels.BookingViewModel();
		var task = Task.Run(() => ViableCanoesInt());
		task.Wait();
		Lediga_kanoter.Text = "Lediga kanoter : " + (14 - task.Result[0]).ToString();
		Lediga_stugor.Text = "Lediga stugor : " + (1 - task.Result[1]).ToString();
		Lediga_vindskydd.Text = "Lediga vindskydd : " + (4 - task.Result[2]).ToString();
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
	
    private async void OnConformation(object sender, EventArgs e)
    {

		Models.Customer.TypeOfBooking bookingtype = Models.Customer.TypeOfBooking.None;
		if(checkCanoe.IsChecked == true)
		{
			bookingtype |= Models.Customer.TypeOfBooking.Kanot;
		}
		if(checkCabin.IsChecked == true)
		{
			bookingtype |= Models.Customer.TypeOfBooking.Stuga;
		}
		if(checkLeanTo.IsChecked == true)
		{
			bookingtype |= Models.Customer.TypeOfBooking.Vindskydd;
		}
		if(checkCampGrounds.IsChecked == true)
		{
			bookingtype |= Models.Customer.TypeOfBooking.Lägerplats;
		}

		
		var custumer = new Models.Customer()
		{
			Name = myName.Text,
			Phone = myPhone.Text,
			Email = myEmail.Text,
			IsOrg = myCheckBox.IsChecked,
			OrgName = (myCheckBox.IsChecked == true ? orgNameInput.Text : ""),
			StartDate = MyStartDate.Date,
			EndDate = MyEndDate.Date,
			BookingType = bookingtype,
			NumberOfCanoes = int.TryParse(AntalKanoter.Text, out int result) ? result : null,
			NumberOfCabin = int.TryParse(AntalStuga.Text, out int result1) ? result1 : null,
			NumberOfCampground = int.TryParse(Lägerområde.Text, out int result2) ? result2 : null,
			NumberOfLeanTo = int.TryParse(Vindskydd.Text, out int result3) ? result3 : null
		};
		await Data.DB.BookingCollection().InsertOneAsync(custumer);
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
	public async Task<int[]> ViableCanoesInt()
	{
		var bookingCollection = await Data.DB.BookingCollection().Find(Builders<Models.Customer>.Filter.Where(x => x.EndDate >= DateTime.Now)).ToListAsync();
		int[] totalSum = new int[4];
		totalSum[0] = bookingCollection.Sum(x => x.NumberOfCanoes.GetValueOrDefault());
		if (totalSum[0] >= 14)
		{
			checkCanoe.IsVisible = false;
			BokaKanotNamn.IsVisible = false;
		}
		totalSum[1] = bookingCollection.Sum(x => x.NumberOfCabin.GetValueOrDefault());
		if (totalSum[1] >= 1)
		{
			checkCabin.IsVisible = false;
			BokaStugaNamn.IsVisible = false;
		}
        totalSum[2] = bookingCollection.Sum(x => x.NumberOfLeanTo.GetValueOrDefault());
		if (totalSum[2] >= 4)
		{
			checkLeanTo.IsVisible = false;
			BokaVindskyddNamn.IsVisible = false;
		}
        totalSum[3] = bookingCollection.Sum(x => x.NumberOfCampground.GetValueOrDefault());
		if (totalSum[3] >= 1000)
		{
			checkCampGrounds.IsVisible = false;
			BokaLägerområdeNamn.IsVisible = false;
		}
        return totalSum;
	}


}