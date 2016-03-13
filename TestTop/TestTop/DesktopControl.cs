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
        public DesktopHelper help { get; private set; }
        List<Image> DeskItem = new List<Image>();
        public DesktopControl()
        {
            InitializeComponent();
            help = new DesktopHelper(); 
        }

        public void Add(Image image)
        {
 
            //position(ref rectangle);
            DeskItem.Add(image);
            Invalidate();
        }
        /*private void position(ref Rectangle rect)
        {
            Rectangle interRectangle;
            //DeskItem = DeskItem.OrderBy(t => t.Rectangle.Y).ToList();
               foreach (var item in DeskItem)
               {
                   Rectangle rectangle = item.Rectangle;
                   while (rectangle.IntersectsWith(rect))
                   {
                       interRectangle = Rectangle.Intersect(rect, rectangle);
                       rect.X += interRectangle.Width;
            
                       if (rect.X + rect.Width > Width)
                       {
                           rect.X = 0;
                           rect.Y += interRectangle.Height;
                       }
            
                   }
               }
        }*/

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            Font f = new Font(FontFamily.GenericMonospace, (float)200 ,FontStyle.Bold);
            /*foreach (var image in DeskItem)
            {
                e.Graphics.DrawImage(image.Image, image.Rectangle);
                e.Graphics.DrawString(DeskItem.IndexOf(image).ToString(), f, Brushes.Red, image.Rectangle);
            }*/
            int x = 0, y = 0,maxHeight=0;
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
                e.Graphics.DrawString(i.ToString(), f, Brushes.Red, new Rectangle(x,y,image.Width,image.Height));

                x += image.Width;
                
            }
        }

        private void DesktopControl_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
