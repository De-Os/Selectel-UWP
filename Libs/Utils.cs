using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.ViewManagement;

namespace Selectel.Libs
{
    public static class Utils
    {
        public static Uri AssetPath(string filename, string path = "UI/") => new Uri("ms-appx:///Assets/" + path + (DarkThemeEnabled() ? "Light" : "Dark") + "/" + filename);

        public static bool DarkThemeEnabled() => new UISettings().GetColorValue(UIColorType.Background).ToString() == "#FF000000";

        public static string LocString(string name, bool quotes = true)
        {
            string result;
            if (name.Contains("/"))
            {
                string[] splitted = name.Split("/");
                result = ResourceLoader.GetForCurrentView(splitted[0]).GetString(splitted[1]);
            }
            else result = ResourceLoader.GetForCurrentView().GetString(name);
            if (quotes) result = result.Replace("<<", "«").Replace(">>", "»");
            return result?.Length > 0 ? result : name;
        }

        public static DateTime ToDateTime(this int unixtime) => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixtime).ToLocalTime();

        public static int ToUnixTime(this DateTime time) => (int)time.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        public static int UnixTime() => DateTime.Now.ToUnixTime();
    }
}