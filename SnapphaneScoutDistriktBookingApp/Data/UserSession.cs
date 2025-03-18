using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace SnapphaneScoutDistriktBookingApp.Data
{
    public class UserSession : INotifyPropertyChanged
    {
        private static readonly Lazy<UserSession> instance = new(() => new UserSession());
        public event PropertyChangedEventHandler? PropertyChanged;

        private UserSession()
        {
            LoadUserData();
        }

        public static UserSession Instance => instance.Value;
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }
        public string UserEmail { get; set; } = string.Empty;

        public bool IsAdmin { get; private set; } = false;

        protected void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserName"));
        }
        private void LoadUserData()
        {
            UserName = Preferences.Get("Användarnamn", string.Empty);
            UserEmail = Preferences.Get("Användarmail", string.Empty);
            IsAdmin = Preferences.Get("IsAdmin", false);
        }

        public void SetUser(string userName, string userEmail)
        {
            UserName = userName;
            UserEmail = userEmail;
            Preferences.Set("Användarnamn", userName);
            Preferences.Set("Användarmail", userEmail);
        }

        public bool IsUserSet()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(UserEmail);
        }

        public void ResetUser()
        {
            Preferences.Remove("Användarnamn");
            Preferences.Remove("Användarmail");
            Preferences.Remove("IsAdmin");
            UserName = string.Empty;
            UserEmail = string.Empty;
            IsAdmin = false;
        }

        public void SetAdmin(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }
    }
}