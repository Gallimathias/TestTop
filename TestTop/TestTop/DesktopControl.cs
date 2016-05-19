using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestTop.Core;

namespace TestTop
{
    public partial class DesktopControl : UserControl
    {
        public DesktopHelper DesktopHelper { get; private set; }
        List<Image> DeskItem = new List<Image>();
        public DesktopControl()
        {
            InitializeComponent();
            DesktopHelper = new DesktopHelper();
        }

        public void Add(Image image)
        {
            DeskItem.Add(image);
            Invalidate();
        }


        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            Font f = new Font(FontFamily.GenericMonospace, 200f, FontStyle.Bold);
            int x = 0, y = 0, maxHeight = 0;
            for (int i = 0; i < DeskItem.Count; i++)
            {
                Image image = DeskItem[i];
                maxHeight = Math.Max(image.Height, maxHeight);

                if (x + image.Width > this.ClientSize.Width)
                {
                    x = 0;
                    y += maxHeight;
                    maxHeight = image.Height;
                }
                e.Graphics.DrawImage(image, x, y);
                e.Graphics.DrawString(i.ToString(), f, Brushes.Red, new Rectangle(x, y, image.Width, image.Height));

                x += image.Width;
            }
        }

        private void DesktopControl_Resize(object sender, EventArgs e) => Invalidate();

    }
}
