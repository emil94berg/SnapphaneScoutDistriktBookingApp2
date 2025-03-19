using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace SnapphaneScoutDistriktBookingApp.Data
{
    class DB
    {
        private static MongoClient GetClient()
        {
            const string connectionUri = "mongodb+srv://dbAdmin:DBadmin00@hultetbooking.h5urq.mongodb.net/?retryWrites=true&w=majority&appName=HultetBooking";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            return client;
        }

        public static IMongoCollection<Models.Customer> BookingCollection()
        {
            var client = GetClient();

            var database = client.GetDatabase("bookingsDB");
            var bookingCollection = database.GetCollection<Models.Customer>("bookings");
            return bookingCollection;
        }
        public static IMongoCollection<Models.Contact> ContactCollection()
        {
            var client = GetClient();

            var database = client.GetDatabase("contactsDB");
            var contactCollection = database.GetCollection<Models.Contact>("contacts");
            return contactCollection;
        }
        public static IMongoCollection<Models.Info> InfoCollection()
        {
            var client = GetClient();

            var database = client.GetDatabase("infoDB");
            var infoCollection = database.GetCollection<Models.Info>("infostring");
            return infoCollection;
        }
        public static async Task UpdateCheckBoxDatabaseAsync(Models.Customer costumer)
        {
            try
            {
                var collection = BookingCollection();
                var filter = Builders<Models.Customer>.Filter.Eq(x => x.Id, costumer.Id);
                var update = Builders<Models.Customer>.Update.Set(x => x.IsConfirmed, costumer.IsConfirmed);
                await collection.UpdateOneAsync(filter, update);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Fel vid uppdatering: {ex.Message}");
            }
        }
        public static IMongoCollection<Models.Admin> AdminUserCollection()
        {
            var client = GetClient();
            var database = client.GetDatabase("adminUsers");
            return database.GetCollection<Models.Admin>("adminUsers");
        }

        public static async Task<bool> RegisterAdminAsync(string userName, string userEmail, string password)
        {
            var collection = AdminUserCollection();

            var existingUser = await collection.Find(x => x.Name == userName).FirstOrDefaultAsync();

            if (existingUser != null)
            {
                return false;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var newAdmin = new Models.Admin
            {
                Name = userName,
                Email = userEmail,
                PasswordHashed = hashedPassword
            };

            await collection.InsertOneAsync(newAdmin);
            return true;
        }

    }
}

