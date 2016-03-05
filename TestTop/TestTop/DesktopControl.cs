using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTop
{
    public partial class DesktopControl : UserControl
    {
        //List<DesktopItem> deskItems = new List<DesktopItem>();
        public DesktopControl()
        {
            InitializeComponent();
        }
        /*
        public void UpdateControl()
        {
            Invalidate();
        }

        public void Add(Image image)
        {
            Rectangle rectangle = new Rectangle(new Point(0, 0), image.Size);
            position(ref rectangle);
        }
        private void position(ref Rectangle rect)
        {
            Rectangle interRectangle;
            foreach (var item in deskItems)
            {
                Rectangle rectangle = item.Rect;
                while (rectangle.IntersectsWith(rect))
                {
                    interRectangle = Rectangle.Intersect(rect, rectangle);
                    rect.X += interRectangle.Width;

                    if(rect.X > Width)
                    {
                        rect.X = 0;
                        rect.Y += interRectangle.Y;
                    }
                }
            }
        }

        private void DesktopControl_Paint(object sender, PaintEventArgs e)
        {
            foreach (var image in deskItems)
            {
                e.Graphics.DrawImage(image.Image, image.Rect);
            }
        }*/
    }
}
