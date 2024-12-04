using System.Drawing;
using System.Windows.Forms;

namespace RobloxAutoLauncher.GameUtils
{
    public class ProcessBarControl : Panel
    {
        private int progress = 0;

        public ProcessBarControl()
        {
            this.Width = 200;
            this.Height = 200;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.BackColor = Color.FromArgb(40, 40, 40);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int progressWidth = (int)(this.Width * (progress / 100.0));
            e.Graphics.FillRectangle(Brushes.Green, 0, 0, progressWidth, this.Height);
        }

        public void SetProgress(int value)
        {
            if (value >= 0 && value <= 100)
            {
                progress = value;
                this.Invalidate();
            }
        }

        public int GetProgress() => progress;
    }
}
