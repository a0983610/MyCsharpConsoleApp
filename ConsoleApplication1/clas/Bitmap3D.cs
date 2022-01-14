using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace ConsoleApplication1
{
    /// <summary>
    /// 三維空間顯示_點陣圖
    /// </summary>
    class Bitmap3D
    {
        private Graphics G;
        int iWidth;
        int iHeight;
        
        /// <summary>
        /// 絕對座標原點(Z=0)
        /// </summary>
        Point O;

        /// <summary>
        /// 焦點與螢幕距離
        /// </summary>
        double L = 500;
        
        /// <summary>
        /// 平移 
        /// </summary>
        bool isDisplacement = false;
        double Dx = 0;
        double Dy = 0;
        double Dz = 0;
        
        /// <summary>
        /// 旋轉軸 角度
        /// </summary>
        bool isRotate = false;
        double Rx = 0;
        double Ry = 0;
        double Rz = 0;

        /// <summary>
        /// 設定相對座標原點
        /// </summary>
        bool isSetRelO = false;
        Point3D RelO = new Point3D(0, 0, 0);

        public Bitmap3D(Graphics G, int iWidth, int iHeight)
        {
            //調高畫值 消除鋸齒
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            G.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            G.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //******
            this.G = G;
            this.iWidth = iWidth;
            this.iHeight = iHeight;
            O.X = iWidth / 2;
            O.Y = iHeight / 2;
        }
        
        /// <summary>
        /// 轉換
        /// </summary>
        private int RoundToInt(double i)
        {
            return Convert.ToInt32(Math.Round(i, 0));
        }
        private double ToRadians(double degree)
        {
            return degree * Math.PI / 180;
        }
        private double ToDegree(double radians)
        {
            return radians * 180 / Math.PI;
        }

        /// <summary>
        /// 設定
        /// </summary>
        public void setL(double L)
        {
            this.L = L;
        }
        /// <summary>
        /// 旋轉後再位移
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void setDxyz(double x,double y,double z)
        {
            isDisplacement = true;
            Dx = x;
            Dy = y;
            Dz = z;
        }
        /// <summary>
        /// 設定各旋轉軸的旋轉角度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void setRxyz(double x, double y, double z)
        {
            isRotate = true;
            Rx = x;
            Ry = y;
            Rz = z;
        }
        /// <summary>
        /// 設定旋轉的中心點
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void setRelO(double x, double y, double z)
        {
            isSetRelO = true;
            this.RelO = new Point3D(x, y, z);
        }
        

        /// <summary>
        /// 依軸旋轉後座標
        /// </summary>
        private Point3D getRotateX(Point3D A, double Rx)
        {
            double tmpX = A.X;
            double tmpY = A.Y * Math.Cos(ToRadians(Rx)) - A.Z * Math.Sin(ToRadians(Rx));
            double tmpZ = A.Y * Math.Sin(ToRadians(Rx)) + A.Z * Math.Cos(ToRadians(Rx));

            return new Point3D(tmpX, tmpY, tmpZ);
        }
        private Point3D getRotateY(Point3D A, double Ry)
        {
            double tmpX = A.X * Math.Cos(ToRadians(Ry)) + A.Z * Math.Sin(ToRadians(Ry));
            double tmpY = A.Y;
            double tmpZ = -A.X * Math.Sin(ToRadians(Ry)) + A.Z * Math.Cos(ToRadians(Ry));

            return new Point3D(tmpX, tmpY, tmpZ);
        }
        private Point3D getRotateZ(Point3D A, double Rz)
        {
            double tmpX = A.X * Math.Cos(ToRadians(Rz)) - A.Y * Math.Sin(ToRadians(Rz));
            double tmpY = A.X * Math.Sin(ToRadians(Rz)) + A.Y * Math.Cos(ToRadians(Rz));
            double tmpZ = A.Z;

            return new Point3D(tmpX, tmpY, tmpZ);
        }

        
        /// <summary>
        /// 取得旋轉位移後的新3D點
        /// </summary>
        private Point3D getNewPoint3DByDR(Point3D A)
        {
            //換成相對座標再旋轉
            if (isSetRelO) A = getRelPoint3D(A, RelO);
            if (isRotate)
            {
                if (Rx != 0) A = getRotateX(A, this.Rx);
                if (Ry != 0) A = getRotateY(A, this.Ry);
                if (Rz != 0) A = getRotateZ(A, this.Rz);
            }
            //再換成絕對座標
            if (isSetRelO) A = getAbsPoint3D(A, RelO);

            if (isDisplacement)
            {
                A.X += Dx;
                A.Y += Dy;
                A.Z += Dz;
            }
            
            return A;
        }
        /// <summary>
        /// 取得絕對座標
        /// </summary>
        private Point3D getAbsPoint3D(Point3D A, Point3D O)
        {
            return new Point3D(O.X + A.X, O.Y + A.Y, O.Z + A.Z);
        }
        /// <summary>
        /// 取得相對座標
        /// </summary>
        private Point3D getRelPoint3D(Point3D A, Point3D O)
        {
            return new Point3D(A.X - O.X, A.Y - O.Y, A.Z - O.Z);
        }



        /// <summary>
        /// 三維(0,0,0) O原點 前方+z 沒有-z
        /// 換算成
        /// 二維 畫面左上(0,0) 下方+y 右方+x
        /// </summary>
        private Point getPoint(Point3D A,bool isDR = true)
        {
            if (isDR) A = getNewPoint3DByDR(A);
            
            //投影位置
            if (A.Z == 0) return new Point(O.X + RoundToInt(A.X), O.Y - RoundToInt(A.Y));
            double Yradians = Math.Atan2(A.Y, (A.Z + L));
            double Xradians = Math.Atan2(A.X, (A.Z + L));

            double Y = L * Math.Tan(Yradians);
            double X = L * Math.Tan(Xradians);

            return new Point(O.X + RoundToInt(X), O.Y - RoundToInt(Y));
        }
        public Point3D getPoint3D(Point3D A,double Rx, double Ry, double Rz)
        {
            if (Rx != 0) A = getRotateX(A, Rx);
            if (Ry != 0) A = getRotateY(A, Ry);
            if (Rz != 0) A = getRotateZ(A, Rz);
            return A;
        }

        /// <summary>
        /// 算出兩點距離
        /// </summary>
        private double getPointDistance(Point A, Point B)
        {
            return Math.Sqrt(Math.Pow((A.X - B.X), 2) + Math.Pow((A.Y - B.Y), 2));
        }
        public double getPointDistance(Point3D A, Point3D B)
        {
            return Math.Sqrt(Math.Pow((A.X - B.X), 2) + Math.Pow((A.Y - B.Y), 2) + Math.Pow((A.Z - B.Z), 2));
        }


        public void DrawPoint(Pen P,Point3D A)
        {
            Point3D t = getNewPoint3DByDR(A);
            //旋轉位移後Z座標 穿越螢幕 無法顯示
            if (t.Z < 0) return;

            Point A2D = getPoint(A);

            //排除XY到畫面外的點
            if (-iWidth > A2D.X) return;
            if (A2D.X > iWidth) return;
            if (-iHeight > A2D.Y) return;
            if (A2D.Y > iHeight) return;

            G.DrawEllipse(P, A2D.X, A2D.Y, 1, 1);
        }
        public void DrawLine(Pen P, Point3D A, Point3D B)
        {
            //取得移動後的新位置
            A = getNewPoint3DByDR(A);
            B = getNewPoint3DByDR(B);
            
            //判斷是否 到達無法顯示的位置
            if (A.Z > 0 && B.Z > 0)
            {
                Point A2d = getPoint(A, false);
                Point B2d = getPoint(B, false);

                G.DrawLine(P, A2d, B2d);
            }
            else if (A.Z < 0 && B.Z < 0) { }
            else if (B.Z < 0)
            {
                //按照斜率計算 到達Z=0時的座標 再劃出直線
                double xz = (A.X - B.X) / (A.Z - B.Z);
                double yz = (A.Y - B.Y) / (A.Z - B.Z);
                double newBx = B.X + xz * -B.Z;
                double newBy = B.Y + yz * -B.Z;

                Point3D newB = new Point3D(newBx, newBy, 0);
                Point A2d = getPoint(A, false);
                Point B2d = getPoint(newB, false);

                G.DrawLine(P, A2d, B2d);
            }
            else if (A.Z < 0)
            {
                double xz = (B.X - A.X) / (B.Z - A.Z);
                double yz = (B.Y - A.Y) / (B.Z - A.Z);
                double newAx = A.X + xz * -A.Z;
                double newAy = A.Y + yz * -A.Z;

                Point3D newA = new Point3D(newAx, newAy, 0);
                Point A2d = getPoint(newA, false);
                Point B2d = getPoint(B, false);

                G.DrawLine(P, A2d, B2d);
            }

        }
        public void DrawTriangle(Pen P, Point3D A, Point3D B, Point3D C)
        {
            DrawLine(P, A, B);
            DrawLine(P, B, C);
            DrawLine(P, C, A);
        }

        public void DrawBall(Pen P, Brush B, Point3D A, double R)
        {
            Point3D t = getNewPoint3DByDR(A);
            //旋轉位移後Z座標 穿越螢幕 無法顯示
            if (t.Z < 0) return;

            Point A2D = getPoint(A);

            //排除XY到畫面外的點
            if (-iWidth > A2D.X) return;
            if (A2D.X > iWidth) return;
            if (-iHeight > A2D.Y) return;
            if (A2D.Y > iHeight) return;

            //球的三軸方向的點
            Point3D YR = new Point3D(A.X, (A.Y + R), A.Z);
            Point3D XR = new Point3D((A.X + R), A.Y, A.Z);
            Point3D ZR = new Point3D(A.X, A.Y, (A.Z + R));

            //三點在平面上的座標
            Point YR2D = getPoint(YR);
            Point XR2D = getPoint(XR);
            Point ZR2D = getPoint(ZR);
            
            double RY2D = getPointDistance(A2D, YR2D);
            double RX2D = getPointDistance(A2D, XR2D);
            double RZ2D = getPointDistance(A2D, ZR2D);

            float R2D = Convert.ToSingle(RY2D > RX2D ? RY2D : (RX2D > RZ2D ? RX2D : RZ2D));
            if (R2D < 0.1) return;

            float newW = 2 * R2D;

            G.FillEllipse(B, (A2D.X - R2D), (A2D.Y - R2D), newW, newW);
            G.DrawEllipse(P, (A2D.X - R2D), (A2D.Y - R2D), newW, newW);


        }
        public void DrawBalls(Brush B, Pen P, List<Ball3D> Ball3D)
        {
            var tmp = (from p in Ball3D
                       orderby p.z descending
                       select p).ToList();

            foreach(var it in tmp)
            {
                DrawBall(P, B, it.getXyz(), it.R);
            }
        }




    }
}
