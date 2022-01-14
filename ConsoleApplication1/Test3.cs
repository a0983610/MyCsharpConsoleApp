using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Test3
    {
        static void Main(string[] args)
        {
            try
            {
                decimal z = 200;
                Random rd = new Random();
                List<decimal> tmp = new List<decimal>();
                for(int i = 0; i < 100000; i++)
                {
                    decimal g = 0;
                    for(int j = 0; j < z; j++)
                    {
                        if (rd.Next(0, 100) >= 98)
                        {
                            g++;

                        }
                    }
                    
                    tmp.Add(g / z);
                }

                using (StreamWriter sw=new StreamWriter("C:\\Users\\jianjhehong\\Desktop\\EXE測試\\實驗2.txt"))
                {
                    foreach(var it in tmp)
                    {
                        sw.WriteLine(it);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
