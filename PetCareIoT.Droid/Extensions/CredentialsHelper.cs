using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Preferences;


namespace PetCareIoT.Extensions
{
    public static class CredentialsHelper
    {
        public static void StoreCredentials(string token, string expiration)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString("token", token);
            editor.PutString("expiration", expiration);
            editor.Apply();        
        }

        
        public static string GetExpiration()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);

            var token = prefs.GetString("expiration", "none");
            return token;
        }

        public static string GetToken()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);

            var token = prefs.GetString("token", "none");
            return token;
        }
    }
}