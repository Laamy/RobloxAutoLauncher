using System.Collections.Generic;
using System;
using System.Net;
using System.Web.Script.Serialization;

using Roblox.API;
using System.Collections.Concurrent;

namespace RobloxAutoLauncher.RobloxSDK
{
    // new wewbclient when requesting cuz no concurrency
    class RobloxAPI
    {
        private static ConcurrentDictionary<string, RobloxPlace> placeCache = new ConcurrentDictionary<string, RobloxPlace>();
        private static ConcurrentDictionary<string, RobloxUniverse> universeCache = new ConcurrentDictionary<string, RobloxUniverse>();
        private static string versionCache;

        private static RobloxPlace GetPlace(string id)
        {
            if (!placeCache.TryGetValue(id, out var cachedPlace))
            {
                var jss = new JavaScriptSerializer();
                cachedPlace = jss.Deserialize<RobloxPlace>(new WebClient().DownloadString($"https://apis.roblox.com/universes/v1/places/{id}/universe"));
                placeCache[id] = cachedPlace;
            }
            return cachedPlace;
        }

        public static RobloxUniverse GetMainUniverse(string id)
        {
            if (!universeCache.TryGetValue(id, out var cachedUniverse))
            {
                var place = GetPlace(id);
                var jss = new JavaScriptSerializer();
                var json = new WebClient().DownloadString($"https://games.roblox.com/v1/games?universeIds={place.UniverseId}");
                cachedUniverse = jss.Deserialize<RobloxUniverse>(json);
                universeCache[id] = cachedUniverse;
            }
            return cachedUniverse;
        }

        public static string GetVersion()
        {
            if (string.IsNullOrEmpty(versionCache))
            {
                var windowsPlyr = new JavaScriptSerializer().Deserialize<dynamic>(new WebClient().DownloadString("https://clientsettings.roblox.com/v1/client-version/WindowsPlayer"));
                versionCache = windowsPlyr["clientVersionUpload"];
            }
            return versionCache;
        }

        public static void DownloadRobloxInstaller(string version, string destination)
        {
            new WebClient().DownloadFile($"https://setup.rbxcdn.com/{version}-Roblox.exe", destination);
        }
    }
}

namespace Roblox.API
{
    public class RobloxPlace
    {
        public long UniverseId { get; set; }
    }

    public class Creator
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public bool isRNVAccount { get; set; }
        public bool hasVerifiedBadge { get; set; }
    }

    public class Datum
    {
        public long id { get; set; }
        public long rootPlaceId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string sourceName { get; set; }
        public string sourceDescription { get; set; }
        public Creator creator { get; set; }
        public object price { get; set; }
        public List<string> allowedGearGenres { get; set; }
        public List<object> allowedGearCategories { get; set; }
        public bool isGenreEnforced { get; set; }
        public bool copyingAllowed { get; set; }
        public int playing { get; set; }
        public int visits { get; set; }
        public int maxPlayers { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public bool studioAccessToApisAllowed { get; set; }
        public bool createVipServersAllowed { get; set; }
        public string universeAvatarType { get; set; }
        public string genre { get; set; }
        public bool isAllGenre { get; set; }
        public bool isFavoritedByUser { get; set; }
        public int favoritedCount { get; set; }
    }

    public class RobloxUniverse
    {
        public List<Datum> data { get; set; }
    }
}
