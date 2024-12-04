namespace RobloxAutoLauncher
{
    public class LauncherArgs
    {
        public string GameInfo, PlaceLauncherUrl, RobloxLocale, GameLocale;
        public ulong LaunchTime, TrackerId;
    }

    class Launcher
    {
        // legacy stuff but im keeping it here just incase..
        public static LauncherArgs ParseArgs(string input)
        {
            LauncherArgs output = new LauncherArgs();

            // roblox-player:1
            // +launchmode:play+
            // gameinfo:datastuff+
            // launchtime:1733294379523+
            // placelauncherurl:https%3A%2F%2Fwww.roblox.com%2FGame%2FPlaceLauncher.ashx%3Frequest%3DRequestPrivateGame%26browserTrackerId%3D1732781422831002%26placeId%3D11729688377%26accessCode%3D127780a6-37e2-4a22-af1c-7635ce00fbac%26joinAttemptId%3Dcf70c30f-7a39-4f04-8c76-dba01e6f4199%26joinAttemptOrigin%3DprivateServerListJoin+
            // browsertrackerid:1732781422831002+
            // robloxLocale:en_us+
            // gameLocale:en_us+
            // channel:+
            // LaunchExp:InApp

            // roblox-player:1
            // +launchmode:play
            // +gameinfo:TOKENIMNOTSHOWINGYOUSHITCUNT
            // +launchtime:1733203423337
            // +placelauncherurl:https%3A%2F%2Fwww.roblox.com%2FGame%2FPlaceLauncher.ashx%3Frequest%3DRequestGame%26browserTrackerId%3D1732781422831002%26placeId%3D4483381587%26isPlayTogetherGame%3Dfalse%26joinAttemptId%3D1c7feee9-8573-4ca7-ad53-ae340f0dfaa4%26joinAttemptOrigin%3DPlayButton
            // +browsertrackerid:1732781422831002
            // +robloxLocale:en_us
            // +gameLocale:en_us
            // +channel:
            // +LaunchExp:InApp

            string[] args = input.Split('+');

            foreach (string arg in args)
            {
                var argTokens = arg.Split(':');
                switch (argTokens[0])
                {
                    case "gameinfo":
                        output.GameInfo = argTokens[1];
                        break;

                    case "launchtime":
                        output.LaunchTime = ulong.Parse(argTokens[1]);
                        break;

                    case "placelauncherurl":
                        output.PlaceLauncherUrl = argTokens[1];
                        break;

                    case "browsertrackerid":
                        output.TrackerId = ulong.Parse(argTokens[1]);
                        break;

                    case "robloxLocale":
                        output.RobloxLocale = argTokens[1];
                        break;

                    case "gameLocale":
                        output.GameLocale = argTokens[1];
                        break;
                }
            }

            return output;
        }
    }
}
