using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTop.Core
{
    public partial class DesktopControl : UserControl
    {
        public DesktopHelper DesktopHelper { get; private set; }
        Dictionary<string, Image> screenshots;


        public DesktopControl()
        {
            InitializeComponent();
            screenshots = new Dictionary<string, Image>();
            DesktopHelper = new DesktopHelper();
        }

        public void Add(string desktopName, Image image)
        {
            if(!screenshots.ContainsKey(desktopName))
                screenshots.Add(desktopName, image);

            screenshots[desktopName] = image;

            Invalidate();
        }


        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            Font f = new Font(FontFamily.GenericMonospace, 30f, FontStyle.Bold);
            int x = 0, y = 0, maxHeight = 0;

           foreach (var screenshot in screenshots)
            {
                Image image = screenshot.Value;
                maxHeight = Math.Max(image.Height, maxHeight);

                if (x + image.Width > ClientSize.Width)
                {
                    x = 0;
                    y += maxHeight;
                    maxHeight = image.Height + 5;
                }
                e.Graphics.DrawImage(image, x, y);
                e.Graphics.DrawString(screenshot.Key, f, Brushes.Red, new Rectangle(x, y, image.Width, image.Height));

                x += image.Width + 5;
                
            }
        }

        private void DesktopControl_Resize(object sender, EventArgs e) => Invalidate();

    }
}
