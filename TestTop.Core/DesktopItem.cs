using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.Core
{
    class DesktopItem
    {
        public Image Image { get; private set; }
        public Rectangle Rectangle { get; private set; }

        public DesktopItem(Image image, Rectangle rect)
        {
            Image = image;
            Rectangle = rect;
        }
    }
}
