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

namespace ConsoleApplication1
{
    public partial class Form2 : Form
    {
        private int iWidth;
        private int iHeight;

        Thread t1 = null;
        List<ball> balls;


        private int Bcount = 50;
        private int Bsize = 10;
        private int sleepTime = 30;

        private int MouseX;
        private int MouseY;

        public Form2()
        {
            InitializeComponent();

            //******
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.Text = "一堆球";


            iWidth = pictureBox1.ClientSize.Width;
            iHeight = pictureBox1.ClientSize.Height;
            //initBalls();
            
            //test
            //Random rdm = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            //var b = new ball(rdm.Next(), R: Bsize, L: rdm.Next(1, 5), MaxX: iWidth, MaxY: iHeight);
            //b.setXY(0, 0);
            //balls = new List<ball>();
            //balls.Add(b);
            //pictureBox1.Image = getBitmapByBalls(balls);            
            //
        }

        /// <summary>
        /// 建立球物件
        /// </summary>
        private void initBalls()
        {
            Random rdm = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            if (!int.TryParse(textBox1.Text, out Bcount)) Bcount = 50;
            if (!int.TryParse(textBox2.Text, out Bsize)) Bsize = 10;


            balls = new List<ball>();
            for(int i = 0; i < Bcount; i++)
            {
                //balls.Add(new ball(r, R: 10, L: 4, MaxX: iWidth, MaxY: iHeight));
                balls.Add(new ball(rdm.Next(), R: Bsize, L: rdm.Next(2, 5), MaxX: iWidth, MaxY: iHeight));
            }
        }

        /// <summary>
        /// 畫出所有球
        /// </summary>
        /// <param name="balls"></param>
        /// <returns></returns>
        private Bitmap getBitmapByBalls(List<ball> balls)
        {
            Bitmap Bm = new Bitmap(iWidth, iHeight);
            Graphics G = Graphics.FromImage(Bm);
            
            //調高畫值 消除鋸齒
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            G.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            G.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            
            //球
            foreach(var it in balls)
            {
                DrawBall(G, it);
            }

            //邊界
            Pen p = new Pen(Color.Red, 3);
            G.DrawLine(p, 0, 0, iWidth, 0);//上
            G.DrawLine(p, 0, iHeight-1, iWidth, iHeight-1);//下
            G.DrawLine(p, 0, 0, 0, iHeight);//左
            G.DrawLine(p, iWidth-1, 0, iWidth-1, iHeight);//右

            //障礙物
            //Brush b = new SolidBrush(Color.Gold);
            //G.FillEllipse(b, MouseX - 10, MouseY - 10, 20, 20);
            //G.DrawEllipse(p, MouseX - 10, MouseY - 10, 20, 20);

            G.Dispose();
            return Bm;
        }

        /// <summary>
        /// 畫一個球
        /// </summary>
        /// <param name="G"></param>
        /// <param name="ball"></param>
        private void DrawBall(Graphics G, ball ball)
        {
            CirCle(G, ball.x, ball.y, ball.R);
        }
        
        private void CirCle(Graphics G, int x, int y,int r)
        {
            //超出畫面
            if (x < -r || y < -r) return;
            if (x > iWidth+r || y > iHeight+r) return;

            int r2 = r * 2;
            Brush B = new SolidBrush(Color.Teal);
            G.FillEllipse(B, new Rectangle(x - r, y - r, r2, r2));
            Pen P = new Pen(Color.Navy, 3);
            G.DrawEllipse(P, new Rectangle(x - r, y - r, r2, r2));
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (balls == null)
            {
                initBalls();
            }

            if (t1 == null)
            {
                t1 = new Thread(pictureBoxDisPlay);
                t1.IsBackground = true;
                t1.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (t1 != null) t1.Abort();
            this.Close();
            Environment.Exit(Environment.ExitCode);
        }

        
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseX = e.X;
            MouseY = e.Y;
            label1.Text = MouseX + "," + MouseY;
        }


        private void pictureBoxDisPlay()
        {
            while (true)
            {
                Thread.Sleep(sleepTime);
                foreach (var it in balls)
                {
                    it.PonMouse(balls, MouseX, MouseY);
                    //it.ElasticCollision(balls);
                }

                //舊圖加新圖
                //Bitmap old = (Bitmap)pictureBox1.Image;
                //Graphics gOld = Graphics.FromImage(old);
                //gOld.DrawImage(getBitmapByBalls(balls), 0, 0);
                //pictureBox1.Image = old;

                pictureBox1.Image = getBitmapByBalls(balls);
            }
        }

    }
}
