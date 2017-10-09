using System.Drawing;

namespace TestTop.Core
{
    public class DesktopIcon
    {
        public Point Location { get; internal set; }
        public int Index { get; private set; }
        public string Name { get; private set; }

        public DesktopIcon(int index, string name, Point location)
        {
            Index = index;
            Location = location;
            Name = name;
        }

    }
}
