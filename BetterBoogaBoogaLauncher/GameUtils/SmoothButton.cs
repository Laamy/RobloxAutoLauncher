using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RobloxAutoLauncher.GameUtils
{
    public class SmoothButton : Panel
    {
        private Color hoverColor = Color.LightBlue;
        private Color pressColor = Color.DarkBlue;
        private Color normalColor = Color.Transparent;
        private Color borderColor = Color.Black;
        private string text = "Button";

        [Category("Appearance")]
        [DefaultValue("Button")]
        public string ButtonText
        {
            get { return text; }
            set { text = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color HoverColor
        {
            get { return hoverColor; }
            set { hoverColor = value; }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        [Category("Appearance")]
        public Color PressColor
        {
            get { return pressColor; }
            set { pressColor = value; }
        }

        public SmoothButton()
        {
            this.Size = new Size(100, 40);
            this.BackColor = normalColor;
            this.DoubleBuffered = true;

            this.MouseEnter += (sender, e) => this.BackColor = hoverColor;
            this.MouseLeave += (sender, e) => this.BackColor = normalColor;
            this.MouseDown += (sender, e) => this.BackColor = pressColor;
            this.MouseUp += (sender, e) => this.BackColor = hoverColor;
        }

        public string Text
        {
            get { return text; }
            set { text = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Pen border = new Pen(BorderColor))
            using (Brush brush = new SolidBrush(ForeColor))
            {
                SizeF textSize = e.Graphics.MeasureString(text, this.Font);
                PointF textLocation = new PointF((this.Width - textSize.Width) / 2, (this.Height - textSize.Height) / 2);
                RectangleF textBounds = new RectangleF(textLocation, textSize);
                Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
                e.Graphics.DrawRectangle(border, bounds);
                e.Graphics.DrawString(text, this.Font, brush, textLocation);
            }
        }
    }
}
