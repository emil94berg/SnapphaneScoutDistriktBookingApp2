using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SnapphaneScoutDistriktBookingApp.Data
{
    class AdminService
    {
        public static async Task<bool> TryLoginAdminAsync(string username, string userEmail, string password)
        {
            var collection = DB.AdminUserCollection();
            var filter = Builders<Models.Admin>.Filter.And(
                Builders<Models.Admin>.Filter.Eq(x => x.Name, username),
                Builders<Models.Admin>.Filter.Eq(x => x.Email, userEmail)
                );

            var adminUser = await collection.Find(filter).FirstOrDefaultAsync();

            if (adminUser == null)
            {
                return false;
            }

            bool isAdmin = BCrypt.Net.BCrypt.Verify(password, adminUser.PasswordHashed);
            if (isAdmin)
            {
                Data.UserSession.Instance.SetAdmin(true);
                Preferences.Set("IsAdmin", true);
            }

            return isAdmin;
        }

        public static async Task<bool> CheckIfAdminAsync(string username, string userEmail)
        {
            var collection = DB.AdminUserCollection();
            var filter = Builders<Models.Admin>.Filter.And(
                Builders<Models.Admin>.Filter.Eq(x => x.Name, username),
                Builders<Models.Admin>.Filter.Eq(x => x.Email, userEmail)
            );

            var adminUser = await collection.Find(filter).FirstOrDefaultAsync();

            return adminUser != null;
        }
    }
}
