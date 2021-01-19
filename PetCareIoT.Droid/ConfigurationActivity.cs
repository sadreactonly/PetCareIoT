using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PetCareIoT.Communication;
using PetCareIoT.Configurations;
using PetCareIoT.Models;

namespace PetCareIoT
{
    [Activity(Label = "ConfigurationActivity")]
    public class ConfigurationActivity : Activity
    {
        private Button saveFeedConfigButton;
        private Button saveWaterConfigButton;
        private Button feedTimeDialogButton;
        private Button waterTimeDialogButton;
        private TextView feedTextView;
        private TextView waterTextView;
        private Switch waterSwitch;
        private Switch feedSwitch;

        private CommunicationService communicationService;
        Config config = new Config();
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.configuration_layout);

            communicationService = new CommunicationService();
           
            feedTimeDialogButton = FindViewById<Button>(Resource.Id.feedTimeButton);
            waterTimeDialogButton = FindViewById<Button>(Resource.Id.waterTimeButton);

            saveFeedConfigButton = FindViewById<Button>(Resource.Id.buttonSaveFeedingConfig);
            saveWaterConfigButton = FindViewById<Button>(Resource.Id.buttonSaveWateringConfig);

            saveFeedConfigButton.Click += SaveFeedConfigButton_Click;
            saveWaterConfigButton.Click += SaveWaterConfigButton_Click;
            feedTextView = FindViewById<TextView>(Resource.Id.textViewFeedingTime);
            waterTextView = FindViewById<TextView>(Resource.Id.textViewWateringTime);

            feedTimeDialogButton.Click += FeedTimeDialogButton_Click;
            waterTimeDialogButton.Click += WaterTimeDialogButton_Click;

            waterSwitch = FindViewById<Switch>(Resource.Id.switchWatering);
            feedSwitch = FindViewById<Switch>(Resource.Id.switchWatering);

            try
            {
                config = await communicationService.GetConfiguration();
            }
            catch (Exception) 
            {
                Finish();
            }
            

            if (config.FeedingTime!=null)
            {
                RunOnUiThread(() =>
                {
                    feedTimeDialogButton.Enabled = true;
                    waterTimeDialogButton.Enabled = true;
                    feedTextView.Text = config.FeedingTime.ToString();
                    waterTextView.Text = config.WateringTime.ToString();
                });
            }
        }

        private async void SaveWaterConfigButton_Click(object sender, EventArgs e)
        {
            await communicationService.SetWateringConfiguration(config.WateringTime, waterSwitch.Checked);
        }

        private async void SaveFeedConfigButton_Click(object sender, EventArgs e)
        {
            await communicationService.SetFeedingConfiguration(config.FeedingTime, feedSwitch.Checked);
        }

        private void WaterTimeDialogButton_Click(object sender, EventArgs e)
        {
            var dialog = new TimePickerDialog(this, WaterTimeSet, config.WateringTime.Hours, config.WateringTime.Minutes, true);
            dialog.Show(); 
        }

        private void WaterTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            if(config!=null)
            {
                config.WateringTime.Hours = e.HourOfDay;
                config.WateringTime.Minutes = e.Minute;
                RunOnUiThread(() =>
                {
                    waterTextView.Text = config.WateringTime.ToString();
                });
            }
           
        }

        private void FeedTimeDialogButton_Click(object sender, EventArgs e)
        {
            var dialog = new TimePickerDialog(this, FeedTimeSet, config.FeedingTime.Hours, config.FeedingTime.Minutes, true);
            dialog.Show();
        }

        private void FeedTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            if (config != null)
            {
                config.FeedingTime.Hours = e.HourOfDay;
                config.FeedingTime.Minutes = e.Minute;
                RunOnUiThread(() =>
                {
                    feedTextView.Text = config.FeedingTime.ToString();
                });
            }
        }
    }
}