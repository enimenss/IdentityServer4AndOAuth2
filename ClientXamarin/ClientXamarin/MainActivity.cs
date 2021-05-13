using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Xamarin.Forms;
using IdentityModel.OidcClient;
using System.Net.Http;
using IdentityModel.OidcClient.Browser;
using Plugin.CurrentActivity;

namespace ClientXamarin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        OidcClient _client;
        LoginResult _result;

        Lazy<HttpClient> _apiClient = new Lazy<HttpClient>(() => new HttpClient());
        protected override void OnCreate(Bundle savedInstanceState)
        {

            DependencyService.Register<ChromeCustomTabsBrowser>();

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClickAsync;

            FloatingActionButton fab1 = FindViewById<FloatingActionButton>(Resource.Id.fab1);
            fab1.Click += FabOnClickAsync1;

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            var browser = DependencyService.Get<IBrowser>();

            var options = new OidcClientOptions
            {
                Authority = "https://diplomski-identity.azurewebsites.net",
                ClientId = "Xamarin",
                Scope = "openid profile offline_access",
                RedirectUri = "xamarinformsclients://callback",
                PostLogoutRedirectUri = "xamarinformsclients://callback",
                Browser = browser,  
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect
            };

            _client = new OidcClient(options);
            _apiClient.Value.BaseAddress = new Uri("https://diplomski-identity.azurewebsites.net/");
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

        private async void FabOnClickAsync(object sender, EventArgs eventArgs)
        {
            _result = await _client.LoginAsync(new LoginRequest());
            Android.Views.View view = (Android.Views.View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }


        private async void FabOnClickAsync1(object sender, EventArgs eventArgs)
        {
            await _client.LogoutAsync(new LogoutRequest());
            Android.Views.View view = (Android.Views.View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
