using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp
{
    class API
    {
        public static async Task SendEmail(string apiKey, string fromEmail, string toEmail, Models.Customer costumer)
        {


            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "Snapphane Scoutdistrikt");
            var subject = "Bokning av" + costumer.BookingType;
            var to = new EmailAddress(toEmail, "Mottagare");
            var plainTextContent = $"Namn: {costumer.Name} \n Tele nr: {costumer.Phone} \n Email: {costumer.Email} \n Vill boka \n {costumer.BookingType}"; //info //Namn
            var htmlContent = $"<strong> {plainTextContent} </strong>"; //info //Namn
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
            Console.WriteLine($"E-post skickad! Statuskod: {response.StatusCode}");
        }
    }
}
