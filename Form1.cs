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
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"C:\Users\Saif Ali Khan\Desktop\test.json");
            while ((line = file.ReadLine()) != null)
            {
                JObject data = JObject.Parse(line);
                try
                {
                    if ((string)data["eventType"] == "md")
                    {
                        using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                        {
                            g.DrawRectangle(new Pen(Color.Green), (int)(data["mouseX"]), (int)data["mouseY"], 5, 5);
                        }
                    }
                    paintPicture(data["mouseX"].Value<Int32>(), data["mouseY"].Value<Int32>(), data["time"].Value<Int32>());
                }
                catch
                {
                    // Do nothing
                }
            }
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);
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
                    double r = v/9;
                    if (v == 0)
                    {
                        //g.DrawRectangle(new Pen(Color.White), x1, y1, 3, 3);
                    }
                    else

                    {
                        //Color color = interpolate(new List<Color>() { Color.FromArgb(0x0500ff), Color.FromArgb(0x0400ff), Color.FromArgb(0x0300ff), Color.FromArgb(0x0200ff), Color.FromArgb(0x0100ff), Color.FromArgb(0x0000ff), Color.FromArgb(0x0002ff), Color.FromArgb(0x0012ff), Color.FromArgb(0x0022ff), Color.FromArgb(0x0032ff), Color.FromArgb(0x0044ff), Color.FromArgb(0x0054ff), Color.FromArgb(0x0064ff), Color.FromArgb(0x0074ff), Color.FromArgb(0x0084ff), Color.FromArgb(0x0094ff), Color.FromArgb(0x00a4ff), Color.FromArgb(0x00b4ff), Color.FromArgb(0x00c4ff), Color.FromArgb(0x00d4ff), Color.FromArgb(0x00e4ff), Color.FromArgb(0x00fff4), Color.FromArgb(0x00ffd0), Color.FromArgb(0x00ffa8), Color.FromArgb(0x00ff83), Color.FromArgb(0x00ff5c), Color.FromArgb(0x00ff36), Color.FromArgb(0x00ff10), Color.FromArgb(0x17ff00), Color.FromArgb(0x3eff00), Color.FromArgb(0x65ff00), Color.FromArgb(0x8aff00), Color.FromArgb(0xb0ff00), Color.FromArgb(0xd7ff00), Color.FromArgb(0xfdff00), Color.FromArgb(0xFFfa00), Color.FromArgb(0xFFf000), Color.FromArgb(0xFFe600), Color.FromArgb(0xFFdc00), Color.FromArgb(0xFFd200), Color.FromArgb(0xFFc800), Color.FromArgb(0xFFbe00), Color.FromArgb(0xFFb400), Color.FromArgb(0xFFaa00), Color.FromArgb(0xFFa000), Color.FromArgb(0xFF9600), Color.FromArgb(0xFF8c00), Color.FromArgb(0xFF8200), Color.FromArgb(0xFF7800), Color.FromArgb(0xFF6e00), Color.FromArgb(0xFF6400), Color.FromArgb(0xFF5a00), Color.FromArgb(0xFF5000), Color.FromArgb(0xFF4600), Color.FromArgb(0xFF3c00), Color.FromArgb(0xFF3200), Color.FromArgb(0xFF2800), Color.FromArgb(0xFF1e00), Color.FromArgb(0xFF1400), Color.FromArgb(0xFF0a00), Color.FromArgb(0xFF0000) }, r);
                        Color color = interpolate(new List<Color>() { Color.FromArgb(0xffffff), Color.FromArgb(0x0000ff), Color.FromArgb(0x00fff4), Color.FromArgb(0xfdff00), Color.FromArgb(0xFF5000), Color.FromArgb(0xFF4600), Color.FromArgb(0xFF0000) }, r);
                        g.DrawLine(new Pen(color, 2), lastx1, lasty1, x1, y1);
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

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.FromArgb(10,10,0);
        }
    }
}
//public void DrawLine(Pen pen, int x1, int y1, int x2, int y2);