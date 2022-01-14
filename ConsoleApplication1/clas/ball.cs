using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class ball
    {
        //位置
        public int x;
        public int y;
        //半徑
        public int R;
        //移動角度0~360
        int V;
        //每次移動長度
        double L;
        private double dX;
        private double dY;

        private Random rdm;

        private int MaxX;
        private int MaxY;

        //必要初始化
        public ball(int seed, int R, int L, int MaxX, int MaxY)
        {
            rdm = new Random(seed);
            this.MaxX = MaxX;
            this.MaxY = MaxY;
            this.L = L;
            this.R = R;

            int b = 50;

            x = rdm.Next(R + b, MaxX - R - b);
            y = rdm.Next(R + b, MaxY - R - b);
            V = rdm.Next(0, 360);

            dX = getdX();
            dY = getdY();
        }

        public void setXY(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        private int dbToInt(double db)
        {
            return Convert.ToInt32(Math.Round(db));
        }

        private int VQuadrant(int v)
        {
            if (0 < v && v < 90) return 1;
            if (90 < v && v < 180) return 2;
            if (180 < v && v < 270) return 3;
            if (270 < v && v < 360) return 4;
            return 0;
        }

        private int XYQuadrant(int x, int y)
        {
            if (this.x < x && this.y < y) return 1;
            if (this.x < x && this.y > y) return 4;
            if (this.x > x && this.y < y) return 2;
            if (this.x > x && this.y > y) return 3;
            return 0;
        }

        private int getRdmNewV(int Quadrant)
        {
            switch (Quadrant)
            {
                case 1:
                    return rdm.Next(1, 89);
                case 2:
                    return rdm.Next(89, 179);
                case 3:
                    return rdm.Next(179, 269);
                case 4:
                    return rdm.Next(269, 359);
            }
            return rdm.Next(0, 359);
        }

        private double getdX()
        {
            if (VQuadrant(this.V) == 0)
            {
                if (V == 0) return L;
                if (V == 180) return -L;
                return 0;
            }
            else
            {
                return (L * Math.Cos(V * Math.PI / 180));
            }
        }

        private double getdY()
        {
            if (VQuadrant(this.V) == 0)
            {
                if (V == 90) return -L;
                if (V == 270) return L;
                return 0;
            }
            else
            {
                return -(L * Math.Sin(V * Math.PI / 180));
            }
        }

        public void setV(int v)
        {
            dX = getdX();
            dY = getdY();
            this.V = v;
        }

        public void setL(double L)
        {
            dX = getdX();
            dY = getdY();
            this.L = L;
        }

        private int getV(double dx, double dy)
        {
            return dbToInt(Math.Atan((-dy / dx) / Math.PI * 180));
        }

        private void inBox()
        {
            if (x - R < 0)
            {
                //左
                if (dX < 0)
                {
                    dX = -dX;
                    V = getV(dX, dY);
                }
            }

            if (x + R > MaxX)
            {
                //右
                if (dX > 0)
                {
                    dX = -dX;
                    V = getV(dX, dY);
                }
            }

            if (y - R < 0)
            {
                //上
                if (dY < 0)
                {
                    dY = -dY;
                    V = getV(dX, dY);
                }
            }

            if (y + R > MaxY)
            {
                //下
                if (dY > 0)
                {
                    dY = -dY;
                    V = getV(dX, dY);
                }
            }
        }

        public void Move(bool isInBox)
        {
            double nX;
            double nY;

            //限定在畫面中
            if (isInBox) inBox();

            nX = x + dX;
            nY = y + dY;
            x = dbToInt(nX);
            y = dbToInt(nY);
        }

        public void Pon(List<ball> balls)
        {
            foreach (ball it in balls)
            {
                if (!this.Equals(it))
                {
                    //不是自己
                    double r = Math.Sqrt((Math.Pow((x - it.x), 2) + Math.Pow((y - it.y), 2)));
                    if (r <= R + it.R + 1)
                    {
                        //如果同象限方向的接觸不改變方向
                        if (XYQuadrant(it.x, it.y) == 1 && VQuadrant(this.V) != 3) { setV(getRdmNewV(3)); }
                        else if (XYQuadrant(it.x, it.y) == 2 && VQuadrant(this.V) != 4) { setV(getRdmNewV(4)); }
                        else if (XYQuadrant(it.x, it.y) == 3 && VQuadrant(this.V) != 1) { setV(getRdmNewV(1)); }
                        else if (XYQuadrant(it.x, it.y) == 4 && VQuadrant(this.V) != 2) { setV(getRdmNewV(2)); }
                        else if (XYQuadrant(it.x, it.y) == 0)
                        {
                            //在XY軸上
                            if (this.x < it.x && it.V != 180) { setV(180); }//此球在左
                            else if (it.x < this.x && it.V != 0) { setV(0); }//此球在右
                            else if (this.y < it.y && it.V != 270) { setV(270); }//此球在下
                            else if (this.y > it.y && it.V != 90) { setV(90); }//此球在上
                        }
                        //接觸後改變速度
                        setL(rdm.Next(1, 5));
                        break;
                    }
                }
            }
            Move(true);

        }

        public void PonMouse(List<ball> balls, int MouseX, int MouseY)
        {
            double MouseR = Math.Sqrt((Math.Pow((x - MouseX), 2) + Math.Pow((y - MouseY), 2)));
            if (MouseR <= R + 10)
            {
                //如果同象限方向的接觸不改變方向
                if (XYQuadrant(MouseX, MouseY) == 1 && VQuadrant(this.V) != 3) { setV(getRdmNewV(3)); }
                else if (XYQuadrant(MouseX, MouseY) == 2 && VQuadrant(this.V) != 4) { setV(getRdmNewV(4)); }
                else if (XYQuadrant(MouseX, MouseY) == 3 && VQuadrant(this.V) != 1) { setV(getRdmNewV(1)); }
                else if (XYQuadrant(MouseX, MouseY) == 4 && VQuadrant(this.V) != 2) { setV(getRdmNewV(2)); }
                else if (XYQuadrant(MouseX, MouseY) == 0)
                {
                    //在XY軸上
                    if (this.x < MouseX) { setV(180); }//此球在左
                    else if (MouseX < this.x) { setV(0); }//此球在右
                    else if (this.y < MouseY) { setV(270); }//此球在下
                    else if (this.y > MouseY) { setV(90); }//此球在上
                }
                //接觸後改變速度
                setL(rdm.Next(3, 5));
            }
            Pon(balls);

        }

        //BUG
        public void ElasticCollision(List<ball> balls)
        {
            foreach (ball it in balls)
            {
                if (!this.Equals(it))
                {
                    double r = Math.Sqrt((Math.Pow((x - it.x), 2) + Math.Pow((y - it.y), 2)));
                    if (r < R + it.R + 1)
                    {
                        //接觸後 兩個速度方向交換
                        int Vtmp = V;
                        double Ltmp = L;
                        setV(it.V);
                        setL(it.L);
                        it.setV(Vtmp);
                        it.setL(Ltmp);
                    }
                }
            }
            Move(true);
        }

    }
}
