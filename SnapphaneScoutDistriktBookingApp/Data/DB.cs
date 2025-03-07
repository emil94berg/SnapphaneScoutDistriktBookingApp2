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
        public class MongoDBService
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
        }
    }
}

