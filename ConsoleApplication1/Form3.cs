using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace ConsoleApplication1
{
    public partial class Form3 : Form
    {
        private int iWidth;
        private int iHeight;
        private int iZ = 400;

        private Thread thd1 = null;
        private int sleepTime = 25;

        List<Ball3D> BList = null;
        double Rxyz = 0;
        //==畫面控制參數
        int Count = 40;
        int BSize = 20;

        int Dz = 500;
        int Rx = 0;
        int Ry = 0;
        

        public Form3()
        {
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.Text = "點陣圖三維顯示";
            
            iWidth = pictureBox1.ClientSize.Width;
            iHeight = pictureBox1.ClientSize.Height;
            //******
            //pictureBox1.Image = getBitmap();
            //intiBList();
        }

        private void intiBList()
        {
            Random rdm = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            BList = new List<Ball3D>();
            for (int i = 0; i < Count; i++)
            {
                BList.Add(new Ball3D(iWidth / 2, iHeight / 2, iZ, BSize, rdm.Next()));
            }

            thd1 = new Thread(pictureBoxDisPlay);
            thd1.Start();
        }

        private Bitmap getBitmap()
        {
            Bitmap Bm = new Bitmap(iWidth, iHeight);
            Graphics G = Graphics.FromImage(Bm);

            Bitmap3D Bitmap3D = new Bitmap3D(G, iWidth, iHeight);
            Brush Bsh = new SolidBrush(Color.Teal);
            Pen Pen = new Pen(Color.Black, 1);

            Bitmap3D.setL(400);
            Bitmap3D.setDxyz(0, 0, Dz);

            Bitmap3D.setRelO(0, 0, 200);

            if (checkBox1.Checked)
            {
                Rxyz += 0.2;
                Rxyz %= 360;
            }
            Bitmap3D.setRxyz(Rx, Ry + Rxyz, 0);
            

            int iX = iWidth / 2;
            int iY = iHeight / 2;
            Bitmap3D.DrawLine(Pen, new Point3D(-iX, iY, iZ), new Point3D(iX, iY, iZ));
            Bitmap3D.DrawLine(Pen, new Point3D(iX, iY, iZ), new Point3D(iX, -iY, iZ));
            Bitmap3D.DrawLine(Pen, new Point3D(iX, -iY, iZ), new Point3D(-iX, -iY, iZ));
            Bitmap3D.DrawLine(Pen, new Point3D(-iX, -iY, iZ), new Point3D(-iX, iY, iZ));

            Bitmap3D.DrawLine(Pen, new Point3D(-iX, iY, iZ), new Point3D(-iX, iY, 0));
            Bitmap3D.DrawLine(Pen, new Point3D(iX, iY, iZ), new Point3D(iX, iY, 0));
            Bitmap3D.DrawLine(Pen, new Point3D(iX, -iY, iZ), new Point3D(iX, -iY, 0));
            Bitmap3D.DrawLine(Pen, new Point3D(-iX, -iY, iZ), new Point3D(-iX, -iY, 0));

            Bitmap3D.DrawLine(Pen, new Point3D(-iX, iY, 0), new Point3D(iX, iY, 0));
            Bitmap3D.DrawLine(Pen, new Point3D(iX, iY, 0), new Point3D(iX, -iY, 0));
            Bitmap3D.DrawLine(Pen, new Point3D(iX, -iY, 0), new Point3D(-iX, -iY, 0));
            Bitmap3D.DrawLine(Pen, new Point3D(-iX, -iY, 0), new Point3D(-iX, iY, 0));

            //bm3D.DrawPoint(Pen, new Point3D(0, 0, 0));

            Bitmap3D.DrawBalls(Bsh, Pen, BList);
            
            G.Dispose();
            return Bm;
        }

        private void pictureBoxDisPlay()
        {
            while (true)
            {
                Thread.Sleep(sleepTime);

                //foreach (var it in BList)
                //{
                //    it.move();
                //    //it.Collision(BList);
                //}

                Parallel.ForEach<Ball3D>(BList, it => { it.move(); });

                pictureBox1.Image = getBitmap();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thd1 != null) thd1.Abort();
            this.Close();
            Environment.Exit(Environment.ExitCode);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (thd1 == null)
            {
                Count = Convert.ToInt32(textBox1.Text);
                BSize = Convert.ToInt32(textBox2.Text);
                intiBList();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Dz = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            Rx = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            Ry = trackBar3.Value;
        }
    }
}
