using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace PetCareIoTMiddleware.Helpers
{
    /// <summary>
    /// Extensions class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Convert object to Json String content.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>Http Content based on string.</returns>
        public static StringContent ToJsonContent(this object o)
        {
            return new StringContent(JsonConvert.SerializeObject(o), Encoding.ASCII, "application/json");
        }

        /// <summary>
        /// Convert object to Json string.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>JSON representation of selected object.</returns>
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}