using System;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PetCareIoT.Communication;
using PetCareIoT.Configurations;
using PetCareIoT.Extensions;

namespace PetCareIoT
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        private TextInputEditText username;
        private TextInputEditText password;
        private Button loginButton;
        private ProgressBar loginProggresBar;
        private readonly UIManager uiManager = new UIManager();
        private  CommunicationService communicationService;

        protected async override  void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.login_layout);
            communicationService = new CommunicationService();

            username = FindViewById<TextInputEditText>(Resource.Id.name_edit_text);
            password = FindViewById<TextInputEditText>(Resource.Id.pass_edit_text);

            loginButton = FindViewById<Button>(Resource.Id.buttonLogin);
            loginButton.Click += LoginButton_Click;
            loginProggresBar = FindViewById<ProgressBar>(Resource.Id.progressBarLogin);
            ConfigurationManager.Initialize(new AndroidConfigurationStreamProviderFactory(() => this));

            await communicationService.Initialize();

            var token = CredentialsHelper.GetToken();
            var expiration = CredentialsHelper.GetExpiration();
            if (!token.Equals("none") && !expiration.Equals("none"))
            {
                if(DateTime.Now < DateTime.Parse(expiration))
                {
                    StartActivity(typeof(MainActivity));
                }
            }
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(username.Text) && string.IsNullOrEmpty(password.Text))
            {
                uiManager.CreateToast(this, "Fill all fields");
            }
            else
            {
                loginButton.StartLoading(loginProggresBar);
                bool loginResult = await communicationService.Login(username.Text, password.Text);
                if(loginResult)
                {
                    StartActivity(typeof(MainActivity));
                }
                else
                {
                    uiManager.CreateToast(this, "Wrong username/password");
                }
                loginButton.StopLoading(loginProggresBar);
            }
        }
    }
}