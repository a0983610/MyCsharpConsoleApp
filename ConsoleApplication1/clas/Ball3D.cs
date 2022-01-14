using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace ConsoleApplication1
{
    class Ball3D
    {
        //-MaxX~MaxX
        public double x;
        //-MaxY~MaxY
        public double y;
        //0~MaxZ
        public double z;
        //半徑
        public double R;
        //位移量
        double dx;
        double dy;
        double dz;

        double MaxX;
        double MaxY;
        double MaxZ;

        Random rdm;

        public Ball3D(double MaxX, double MaxY, double MaxZ, double R, int seed)
        {
            this.R = R;
            this.MaxX = MaxX;
            this.MaxY = MaxY;
            this.MaxZ = MaxZ;
            rdm = new Random(seed);
            this.x = rdm.Next(doubleToInt(-MaxX + R), doubleToInt(MaxX - R));
            this.y = rdm.Next(doubleToInt(-MaxY + R), doubleToInt(MaxY - R));
            this.z = rdm.Next(doubleToInt(R), doubleToInt(MaxZ - R));
            dx = rdm.Next(-5, 5);
            dy = rdm.Next(-5, 5);
            dz = rdm.Next(-5, 5);
        }

        public Point3D getXyz()
        {
            return new Point3D(x, y, z);
        }

        private int doubleToInt(double i)
        {
            return Convert.ToInt32(Math.Round(i));
        }

        private double getPointDistance(Point3D A, Point3D B)
        {
            return Math.Sqrt(Math.Pow((A.X - B.X), 2) + Math.Pow((A.Y - B.Y), 2) + Math.Pow((A.Z - B.Z), 2));
        }

        private Point3D getPointByBall(Ball3D b)
        {
            return new Point3D(b.x, b.y, b.z);
        }

        private void inBox()
        {
            if (x - R < -MaxX && dx < 0)
            {
                dx = -dx;
            }
            if (x + R > MaxX && dx > 0)
            {
                dx = -dx;
            }
            if (y - R < -MaxY && dy < 0)
            {
                dy = -dy;
            }
            if (y + R > MaxY && dy > 0)
            {
                dy = -dy;
            }
            if (z - R < 0 && dz < 0)
            {
                dz = -dz;
            }
            if (z + R > MaxZ && dz > 0)
            {
                dz = -dz;
            }
        }

        public void move()
        {
            inBox();
            x += dx;
            y += dy;
            z += dz;
        }

        public void Collision(List<Ball3D> balls)
        {
            foreach (var it in balls)
            {
                if (!it.Equals(this))
                {
                    double D = getPointDistance(getPointByBall(this), getPointByBall(it));
                    if (D < it.R)
                    {
                        if (this.x > it.x) { this.dx = rdm.Next(1, 5); }
                        else { this.dx = rdm.Next(-5, 1); }
                        if (this.y > it.y) { this.dy = rdm.Next(1, 5); }
                        else { this.dy = rdm.Next(-5, 1); }
                        if (this.z > it.z) { this.dz = rdm.Next(1, 5); }
                        else { this.dz = rdm.Next(-5, 1); }
                    }

                }
            }
            move();
        }
    }
}
