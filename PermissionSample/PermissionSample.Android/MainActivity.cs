using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Permission = Plugin.Permissions.Abstractions.Permission;

namespace PermissionSample.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TextView _resultText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            _resultText = FindViewById<TextView>(Resource.Id.ResultText);

            var phoneButton = FindViewById<Button>(Resource.Id.PhoneButton);
            phoneButton.Click += PhoneButton_Click;

            var locationButton = FindViewById<Button>(Resource.Id.LocationButton);
            locationButton.Click += LocationButton_Click;

            var cameraButton = FindViewById<Button>(Resource.Id.CameraButton);
            cameraButton.Click += CameraButton_Click;
        }

        private async void PhoneButton_Click(object sender, EventArgs e)
        {
            var status = await Core.PermissionManager.RequestPermissionsAsync(Permission.Phone, Permission.Contacts);

            _resultText.Text = GetPermissionStatus(status);
        }

        private async void LocationButton_Click(object sender, EventArgs e)
        {
            var status = await Core.PermissionManager.RequestPermissionsAsync(Permission.Location);

            _resultText.Text = GetPermissionStatus(status);
        }

        private async void CameraButton_Click(object sender, EventArgs e)
        {
            var status = await Core.PermissionManager.RequestPermissionsAsync(Permission.Camera, Permission.MediaLibrary);

            _resultText.Text = GetPermissionStatus(status);
        }

        private string GetPermissionStatus(Dictionary<Permission, PermissionStatus> status)
        {
            var sb = new StringBuilder();
            sb.Append("Result:\n\n");

            foreach (var pair in status)
            {
                sb.Append($"Permission: {pair.Key.ToString()}\n" +
                    $"Status: {pair.Value.ToString()}\n\n");
            }

            return sb.ToString();
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
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

