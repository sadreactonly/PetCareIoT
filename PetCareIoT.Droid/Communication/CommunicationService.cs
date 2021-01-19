using System.Net.Http;
using System.Threading.Tasks;
using PetCareIoT.Models;
using Newtonsoft.Json;
using PetCareIoT.Extensions;
using System;
using System.Threading;
using PetCareIoT.Configurations;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace PetCareIoT.Communication
{
	public class CommunicationService 
	{
		private string baseAddress;
		public CommunicationService()
        {
        }

        /// <summary>
        /// Sets base address from config.json
        /// </summary>
        /// <returns></returns>
        public  async Task Initialize()
        {
            using var cts = new CancellationTokenSource();
            var config = await ConfigurationManager.Instance.GetAsync(cts.Token);
            baseAddress = config.LocalNetworkUrl;
        }

        //need refactoring
        public async Task<int> GetInitialStates()
		{
            using var client = CreateClient();
            var result = await client.GetAsync("/api/arduino/get-light-state");
            if (result.IsSuccessStatusCode)
            {
                var jsonString = await result.Content.ReadAsStringAsync();
                return int.Parse(jsonString);
            }
            else return 0;
        }


        /// <summary>
        /// Start water pump.
        /// </summary>
        /// <returns>
        /// True if it's done, false if fails.
        /// </returns>
        public async Task<bool> StartPump()
		{
            using var client = CreateClient();
            var result = await client.GetAsync("/api/client/start-pump");
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Start feeder pump.
        /// </summary>
        /// <returns>
        /// True if it's done, false if fails.
        /// </returns>
        public async Task<bool> StartFeeder()
		{
            using var client = CreateClient();
            var result = await client.GetAsync("/api/client/start-feeder");
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;

        }

		public async Task<bool> SetLight(int state)
		{
            using var client = CreateClient();
            string path = $"/api/client/set-light-state/{state}";

            var result = await client.GetAsync(path);
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
		
		public async Task<DHT11Sensor> GetTemperatureAndHumidity()
		{
            using var client = CreateClient();
            var result = await client.GetAsync("/getTemperatureAndHumidity");
            if (result.IsSuccessStatusCode)
            {
                var jsonString = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DHT11Sensor>(jsonString);
            }
            else return default;
        }

		public async Task<Config> GetConfiguration()
		{
            using var client = CreateClient();
            var result = await client.GetAsync("/getConfiguration");
            if (result.IsSuccessStatusCode)
            {
                var jsonString = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Config>(jsonString);
            }
            else return null;
        }
	
		public async Task<bool> SetFeedingConfiguration(Time time, bool isEnabled)
        {
            using var client = CreateClient();
            var content = new
            {
                Time = time,
                IsEnabled = isEnabled
            };
            var result = await client.PostAsync("/setFeedingConfiguration/", content.AsJson());
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
		public async Task<bool> SetWateringConfiguration(Time time, bool isEnabled)
		{
            using var client = CreateClient();
            var content = new
            {
                Time = time,
                IsEnabled = isEnabled
            };
            var result = await client.PostAsync("/setWateringConfiguration/", content.AsJson());
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
		public async Task<bool> SetLightConfiguration(float lightLevel, bool isEnabled)
		{
            using var client = new HttpClient();
            var content = new
            {
                LightLevel = lightLevel,
                IsEnabled = isEnabled
            };

            var result = await client.PostAsync("/setLightConfiguration/", content.AsJson());
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Login(string _username, string _password)
        {
            using var client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
            var content = new
            {
                username = _username,
                password = _password
            };

            var result = await client.PostAsync("/api/authenticate/login", content.AsJson());
            if (result.IsSuccessStatusCode)
            {
                var jsonString = await result.Content.ReadAsStringAsync();
                JObject jObj = (JObject)JsonConvert.DeserializeObject(jsonString);

                var token = jObj["token"].ToString();
                var expiration = jObj["expiration"].ToString();
                CredentialsHelper.StoreCredentials(token, expiration);
                return true;
            }
            else
            {
                return false;
            }
        }
        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CredentialsHelper.GetToken());
            
           return httpClient;
        }
    }
}