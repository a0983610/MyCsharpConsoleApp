using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows;

using ConsoleApplication1.Func;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleApplication1
{
    static class Test1
    {
        static void Main(string[] args)
        {
            try
            {
                using (Bitmap bm = new Bitmap("C:\\Users\\jianjhehong\\Desktop\\截圖\\01.JPG"))
                {
                    testbb(bm);
                }


                //using (Bitmap bm = new Bitmap(100, 255))
                //{
                //    int t = 0;
                //    for (int i = 0; i < bm.Height; i++)
                //    {
                //        for (int j = 0; j < bm.Width; j++)
                //        {
                //            if (i % 5 == 0 && j == 0) t++;//每5 +1
                //            double temp= 255d * (t * 5) / 100;
                //            int z = Convert.ToInt32(temp);

                //            if (z > 255) z = 255;

                //            if (i == 100)
                //            {
                //                bm.SetPixel(j, i, Color.FromArgb(255, 255, 0, 0));
                //            }
                //            else
                //            {
                //                if (i > 100) continue;
                //                bm.SetPixel(j, i, Color.FromArgb(255, z, z, z));
                //            }

                //        }
                //    }

                //    bm.Save("C:\\Users\\jianjhehong\\Desktop\\截圖\\ColorTest.JPG", ImageFormat.Jpeg);
                //}

                //Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
            finally
            {
                
            }
        }


        static bool testbb(Bitmap bm1)
        {
            try
            {
                Color w = Color.FromArgb(255, 255, 255, 255);
                Color b = Color.FromArgb(255, 0, 0, 0);
                for (int i = 0; i < bm1.Height; i++)
                {
                    for (int j = 0; j < bm1.Width; j++)
                    {
                        Color c = bm1.GetPixel(j, i);

                        if (j + 1 < bm1.Width)
                        {
                            Color c1 = bm1.GetPixel(j + 1, i);
                            if (ddddc(c, c1))
                            {
                                bm1.SetPixel(j, i, b);
                                continue;
                            }
                            else
                            {
                                bm1.SetPixel(j, i, w);
                            }
                        }
                        else if (j - 1 >= 0)
                        {
                            Color c1 = bm1.GetPixel(j - 1, i);
                            if (ddddc(c, c1))
                            {
                                bm1.SetPixel(j, i, b);
                                continue;
                            }
                            else
                            {
                                bm1.SetPixel(j, i, w);
                            }
                        }
                        else if (i + 1 < bm1.Height)
                        {
                            Color c1 = bm1.GetPixel(j, i + 1);
                            if (ddddc(c, c1))
                            {
                                bm1.SetPixel(j, i, b);
                                continue;
                            }
                            else
                            {
                                bm1.SetPixel(j, i, w);
                            }
                        }
                        else if (i - 1 >= 0)
                        {
                            Color c1 = bm1.GetPixel(j, i - 1);
                            if (ddddc(c,c1))
                            {
                                bm1.SetPixel(j, i, b);
                                continue;
                            }
                            else
                            {
                                bm1.SetPixel(j, i, w);
                            }
                        }
                    }
                }
                bm1.Save("C:\\Users\\jianjhehong\\Desktop\\截圖\\01+.JPG");

                
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
        
        static bool ddddc(Color c1,Color c2)
        {
            int aa = 12;
            if (Math.Abs(c1.R - c2.R) < aa)
            {
                if (Math.Abs(c1.G - c2.G) < aa)
                {
                    if (Math.Abs(c1.B - c2.B) < aa)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        

    }
}
