using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobloxAutoLauncher.RobloxPlaces.TitlebarExtension
{
    public class Index
    {
        public static void Init()
        {
            Task.Run(() => Application.Run(new Overlay()));
        }
    }
}
