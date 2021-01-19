using System.Net.Http;
using System.Text;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace PetCareIoT.Extensions
{
    public static class Extensions
    {
        public static StringContent AsJson(this object o)
        {
            return new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Convert object to Json string.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static void StartLoading(this Button button, ProgressBar progressBar)
        {
            button.TextScaleX = 0;
            progressBar.Visibility = ViewStates.Visible;
            progressBar.Indeterminate = true;
        }

        public static void StopLoading(this Button button, ProgressBar progressBar)
        {
            button.TextScaleX = 1;
            progressBar.Visibility = ViewStates.Gone;
            progressBar.Indeterminate = false;
        }
    }
}