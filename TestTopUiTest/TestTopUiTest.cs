using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Drawing;

namespace TestTopUiTest
{
    [TestClass]
    public class TestTopUiTest
    {
        [TestMethod]
        public void MainTest()
        {
            var form = new Form();
            form.AutoScaleMode = AutoScaleMode.Dpi;
            form.WindowState = FormWindowState.Maximized;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Show();

            var screen = SystemInformation.VirtualScreen;
            var screenClass = form.CreateGraphics();
            var width = screen.Width;
            var height = screen.Height;
            


            using (Bitmap bmpScreenCapture = new Bitmap(width / 96 * 120, height / 96 * 120))
            {
                using (Graphics g = Graphics.FromImage(bmpScreenCapture))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, //TODO Idee vom Stream 4.2.2017
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0, bmpScreenCapture.Size,
                                     CopyPixelOperation.SourceCopy );
                }
                bmpScreenCapture.Save("screenshot.bmp");
            }
        }
    }
}
