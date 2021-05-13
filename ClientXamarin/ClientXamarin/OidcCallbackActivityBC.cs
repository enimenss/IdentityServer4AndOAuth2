using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using System;


namespace ClientXamarin
{

    [Activity(Label = "OidcCallbackActivityBC")]
        [IntentFilter(new[] { Intent.ActionView },
            Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
            DataScheme = "xamarinformsclientsBC")]
           // DataHost = "callback")]
        public class OidcCallbackActivityBC : Activity
        {
            public static event Action<string> Callbacks;

            public OidcCallbackActivityBC()
            {
                Log.Debug("OidcCallbackActivityBC", "constructing OidcCallbackActivityBC");
            }

            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);

                Callbacks?.Invoke(Intent.DataString);

                Finish();
            
                StartActivity(typeof(MainActivity));
            }
        }
    }