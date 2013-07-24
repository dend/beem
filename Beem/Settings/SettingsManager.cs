using Microsoft.Phone.Shell;
using Coding4Fun.Toolkit.Storage;

namespace Beem.Settings
{
    public static class SettingsManager
    {
        public static bool AttemptToLoadSettings()
        {
            Binder.Instance.CurrentAppSettings = Serialize.Open<BeemSettings>("settings.xml");

            if (Binder.Instance.CurrentAppSettings.FirstLaunchFlag)
            {
                if (Binder.Instance.CurrentAppSettings.CanRunUnderLockScreen == true)
                    PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
                else
                    PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;

                return true;
            }
            else
            {
                return false; // This is the first run for this application.
            }
        }

        public static void StoreSettings()
        {
            Serialize.Save("settings.xml", Binder.Instance.CurrentAppSettings);
        }

    }
}
