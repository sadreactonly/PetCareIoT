using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using PetCareIoT.Communication;
using Newtonsoft.Json.Linq;
using Com.Airbnb.Lottie;
using Android.Support.V4.Widget;
using PetCareIoT.Extensions;
using System.Threading;
using PetCareIoT.Configurations;
using Android.Content;
using Android.Preferences;
using PetCareIoT.Models;

namespace PetCareIoT
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        private ToggleButton lightSwitch;
        private Button pumpButton;
        private Button dhtButton;
        private Button feederButton;
        private TextView lightState;
        private LottieAnimationView animationView;
        private UIManager uiManager;
        private Gauge tempGauge;
        private Gauge humidityGauge;
        ProgressBar progressBarFeed;
        ProgressBar progressBarDHT;
        ProgressBar progressBarWater;
        private CommunicationService communicationService;

        private readonly string[] animations = new string[5] { "eating_cat.json", "sleeping_cat.json", "surprised_cat.json", "hungry_cat.json", "happy_cat.json" };

        protected  async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var swipeLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_container);
            swipeLayout.Refresh += SwipeLayout_Refresh;

            animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);

            lightSwitch = FindViewById<ToggleButton>(Resource.Id.toggleLightSwitch);   
            lightState = FindViewById<TextView>(Resource.Id.textLightState);
           
            pumpButton = FindViewById<Button>(Resource.Id.buttonPump);
			pumpButton.Click += PumpButton_Click;

            dhtButton = FindViewById<Button>(Resource.Id.buttonGetDht);
			dhtButton.Click += DhtButton_Click;

            feederButton = FindViewById<Button>(Resource.Id.buttonFeed);
			feederButton.Click += FeederButton_Click;

            tempGauge = FindViewById<Gauge>(Resource.Id.tempGauge);
            humidityGauge = FindViewById<Gauge>(Resource.Id.humidityGauge);

            SetGauges();

            progressBarFeed = FindViewById<ProgressBar>(Resource.Id.progressBarFeed);
            progressBarDHT = FindViewById<ProgressBar>(Resource.Id.progressBarDHT);
            progressBarWater = FindViewById<ProgressBar>(Resource.Id.progressBarWater);

            communicationService = new CommunicationService();
            await communicationService.Initialize();
            uiManager = new UIManager();
           
            lightSwitch.CheckedChange += LightSwitch_CheckedChange;

         //   SetupData();

           
        }

        private void SetGauges()
        {
            tempGauge.TotalNicks = 120;
            tempGauge.MinValue = 0;
            tempGauge.MaxValue = 100;
            tempGauge.ValuePerNick = 1;
            tempGauge.InitValue = 0;
            tempGauge.UpperText = Resources.GetString(Resource.String.temperature);
            tempGauge.LowerText = Resources.GetString(Resource.String.temp_unit);

            
            humidityGauge.TotalNicks = 120;
            humidityGauge.MinValue = 0;
            humidityGauge.MaxValue = 100;
            humidityGauge.ValuePerNick = 1;
            humidityGauge.InitValue = 0;
            humidityGauge.UpperText = Resources.GetString(Resource.String.humidity);
            humidityGauge.LowerText = Resources.GetString(Resource.String.humidity_unit);
            
        }
        private void SwipeLayout_Refresh(object sender, EventArgs e)
        {
            //SetupData();
            var swipeLayout = sender as SwipeRefreshLayout;
            swipeLayout.Refreshing = false;
        }

        private void PlayRandomAnimation()
        {
            Random rand = new Random();
            RunOnUiThread(() =>
            {
                animationView.SetAnimation(animations[rand.Next(0, 4)]);
                animationView.PlayAnimation();
            });
        }

        private async void FeederButton_Click(object sender, EventArgs e)
		{
            
            feederButton.StartLoading(progressBarFeed);
            var result = await SocketCommunicationService.StartFeeder();
            string message = "There was an error on server.";

            feederButton.StopLoading(progressBarFeed);

            if (result)
            {
                message = "Done.";
                PlayRandomAnimation();
            }
            uiManager.CreateToast(ApplicationContext, message);
        }

		private async void SetupData()
        {
           
              int result =  await communicationService.GetInitialStates();
              //var jObj = (JObject)result;

                RunOnUiThread(() => {

                    PlayRandomAnimation();
                    //var t = float.Parse(jObj["temperature"].ToString());
                   // var h = float.Parse(jObj["humidity"].ToString());

                    //tempGauge.MoveToValue(t);
                    //humidityGauge.MoveToValue(h);

                    //string state = result;//jObj["lightState"].ToString();
                    if (result == 1)
                    {
                        lightSwitch.Checked = true;
                        lightState.Text = string.Format(Resources.GetString(Resource.String.lightState), "on.");
                    }
                    else
                    {
                        lightSwitch.Checked = false;
                        lightState.Text = string.Format(Resources.GetString(Resource.String.lightState), "off.");
                    }
                });
               

        }

        private async void DhtButton_Click(object sender, EventArgs e)
		{
            dhtButton.StartLoading(progressBarDHT);

            try { 
            var result = await communicationService.GetTemperatureAndHumidity();
            string message = "There was an error on server.";
            Thread.Sleep(3000);
            if (result != null)
            {
                message = "Done";
                RunOnUiThread(() => {
                    tempGauge.MoveToValue(result.Temperature);
                    humidityGauge.MoveToValue(result.Humidity);
                });
            }
            dhtButton.StopLoading(progressBarDHT);
            }
            catch (Exception)
            {
                uiManager.CreateToast(this.ApplicationContext, "Not implemented.");
            }

            
		}

        
		private async void LightSwitch_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
		{
            if (e.IsChecked)
			{
                await SocketCommunicationService.SetLight((int)EventType.LIGHT_ON);
                lightState.Text = string.Format(Resources.GetString(Resource.String.lightState), "on.");
            }
            else
			{
                await SocketCommunicationService.SetLight((int)EventType.LIGHT_OFF);
                lightState.Text = string.Format(Resources.GetString(Resource.String.lightState), "off.");
            }
        }

		private async void PumpButton_Click(object sender, EventArgs e)
		{
            pumpButton.StartLoading(progressBarWater);
            //var result = await communicationService.StartPump();
            var result = await SocketCommunicationService.StartPump();

            string message = "There was an error on server.";
           
      
            if (result)
            {
                message = "Done.";
                PlayRandomAnimation();
            }
            pumpButton.StopLoading(progressBarWater);

            uiManager.CreateToast(this.ApplicationContext, message);
        }

		public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                StartActivity(typeof(ConfigurationActivity));
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
