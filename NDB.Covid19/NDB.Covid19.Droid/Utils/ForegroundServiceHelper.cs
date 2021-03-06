using Android.App;
using Android.Content;
using Android.OS;

namespace NDB.Covid19.Droid.Utils
{
    public class ForegroundServiceHelper
    {
        public static void StartForegroundServiceCompat<T>(Context context, Bundle args = null) where T : Service
        {
            Intent intent = new Intent(context, typeof(T));
            if (args != null)
            {
                intent.PutExtras(args);
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                context.StartForegroundService(intent);
            }
            else
            {
                context.StartService(intent);
            }
        }

        public static void StopForegroundServiceCompat<T>(Context context) where T : Service
        {
            Intent intent = new Intent(context, typeof(T));
            context.StopService(intent);
        }
    }
}