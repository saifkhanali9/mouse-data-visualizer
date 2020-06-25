using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;
using Newtonsoft.Json.Linq;
//using Math;
namespace mouseGradient
{
    public partial class Form1 : Form
    {
        int lastx1 = 0;
        int lasty1 = 0;
        int lastTime1 = 0;
        double vx = 0;
        double vy = 0;
        double v = 0;
        int vMag = 0;
        int red = 255;
        int green = 0;
        int blue = 0;
        int redNew = 0;
        int greenNew = 150;
        int timeDiff = 0;
        int xDiff = 0;
        int yDiff = 0;

        int check = 0;
        public Form1()
        {
            InitializeComponent();
            mainFunction();
        }
        private void mainFunction()
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"C:\Saif\Office\C#\Projects\MouseClick\file\1593092606943.json");
            while ((line = file.ReadLine()) != null)
            {
                JObject data = JObject.Parse(line);
                try
                {
                    if ((string)data["eventType"] == "mc")
                    {
                        using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                        {
                            g.DrawRectangle(new Pen(Color.Green), (int)(data["mouseX"]), (int)data["mouseY"], 6, 6);
                        }
                    }
                    paintPicture(data["mouseX"].Value<Int32>(), data["mouseY"].Value<Int32>(), data["time"].Value<Int32>());
                }
                catch
                {

                }
            }

            file.Close();
            // Suspend the screen.
        }
        //private int clip(int a)
        //{
        //    return Math.Max(0, Math.Min(255, a));
        //}
        private double clip(double a, double min=0, double max=1)
        {
            return Math.Max(min, Math.Min(max, a));
        }
        private Color interpolate(Color a, Color b, double ratio)
        {
            ratio = clip(ratio);
            return Color.FromArgb(128, (int)((b.R - a.R) * ratio + a.R), (int)((b.G - a.G) * ratio + a.G), (int)((b.B - a.B) * ratio + a.B));
        }
        private Color interpolate(List<Color> colors, double ratio)
        {
            ratio *= colors.Count;
            var lower = (int)clip(ratio, 0, colors.Count-2);
            var upper = lower + 1;
            return interpolate(colors[lower], colors[upper], ratio - lower);
        }
        private void paintPicture(int x1, int y1, int time1)
        {
            if (check == 0)
            {
                lastx1 = x1;
                lasty1 = y1;
                lastTime1 = time1;
                // isMouseDown = true;
                check = 1;
            }
            else//if our last point is not null, which in this case we have assigned above

            {
                if (pictureBox1.Image == null)//if no available bitmap exists on the picturebox to draw on
                {
                    //create a new bitmap
                    Bitmap bmp = new Bitmap(Convert.ToInt32(SystemParameters.VirtualScreenWidth), Convert.ToInt32(SystemParameters.VirtualScreenHeight));

                    pictureBox1.Image = bmp; //assign the picturebox.Image property to the bitmap created
                }

                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {//we need to create a Graphics object to draw on the picture box, its our main tool

                    //when making a Pen object, you can just give it color only or give it color and pen size
                    xDiff = Math.Abs(x1 - lastx1);
                    timeDiff = time1 - lastTime1;
                    yDiff = Math.Abs(y1 - lasty1);
                    vx = (xDiff *1.0) / timeDiff;
                    vy = (yDiff * 1.0) / timeDiff;
                    v = Math.Sqrt((vx * vx) + (vy * vy));
                    vMag = Convert.ToInt32(v);
                    double r = v/11;
                    if (v == 0)
                    {
                        SolidBrush redBrush = new SolidBrush(Color.Blue);
                        g.FillEllipse(redBrush, x1, y1, 4, 4);
                    }
                    else
                    {
                        Color color = interpolate(new List<Color>() { Color.Black, Color.Black, Color.FromArgb(0x330000), Color.FromArgb(0x660000), Color.FromArgb(0x660000), Color.FromArgb(0x800000), Color.FromArgb(0x8B0000), Color.FromArgb(0xCD0000), Color.FromArgb(0xEE0000), Color.FromArgb(0xFF0000), Color.FromArgb(0xFE0000), Color.FromArgb(0xFF2B2B),
                        Color.FromArgb(0xFF3030), Color.FromArgb(0xFF3333), Color.FromArgb(0xFF4040), Color.FromArgb(0xFF6666), Color.FromArgb(0xFF6A6A), Color.FromArgb(0xFF7777), Color.FromArgb(0xFF9090), Color.FromArgb(0xFF9393), Color.FromArgb(0xFF9999), Color.FromArgb(0xFFA9A9), Color.FromArgb(0xFFAAAA), Color.FromArgb(0xFFAEAE), Color.FromArgb(0xFFBBBB), Color.FromArgb(0xFFC1C1), Color.FromArgb(0xFFCCCC), Color.FromArgb(0xFFD5D5), Color.FromArgb(0xFFDDDD)}, r);
                        g.DrawLine(new Pen(color, 1), lastx1, lasty1, x1, y1);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                    }
                    //this is to give the drawing a more smoother, less sharper look
                }
                pictureBox1.Invalidate();//refreshes the picturebox

                lastx1 = x1;
                lasty1 = y1;
                lastTime1 = time1;
            }
        }
    }
}
//public void DrawLine(Pen pen, int x1, int y1, int x2, int y2);