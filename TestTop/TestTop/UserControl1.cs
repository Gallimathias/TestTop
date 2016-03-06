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
    public partial class UserControl1 : UserControl
    {
        public DesktopHelper help { get; private set; }
        List<DesktopItem> DeskItem = new List<DesktopItem>();
        public UserControl1()
        {
            InitializeComponent();
            help = new DesktopHelper(); 
        }

        public void Add(Image image)
        {
            Rectangle rectangle = new Rectangle(new Point(0, 0), image.Size);
            position(ref rectangle);
            DeskItem.Add(new DesktopItem(image, rectangle));
            Invalidate();
        }
        private void position(ref Rectangle rect)
        {
            Rectangle interRectangle;
            DeskItem = DeskItem.OrderBy(t => t.Rectangle.Y).ToList();
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
        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            Font f = new Font(FontFamily.GenericMonospace, (float)100 ,FontStyle.Bold);
            foreach (var image in DeskItem)
            {
                e.Graphics.DrawImage(image.Image, image.Rectangle);
                e.Graphics.DrawString(DeskItem.IndexOf(image).ToString(), f, Brushes.Black, image.Rectangle);
            }
        }
    }
}
