using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobloxAutoLauncher
{
    public class SoundPlayer
    {
        public static async Task PlayClick()
        {
            await Task.Run(() =>
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "RobloxAutoLauncher.Resources.click_sound.wav";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (var player = new System.Media.SoundPlayer(stream))
                        {
                            player.PlaySync();  
                        }
                    }
                    else
                    {
                        throw new FileNotFoundException("Embedded sound file not found.");
                    }
                }
            });
        }
    }
}
