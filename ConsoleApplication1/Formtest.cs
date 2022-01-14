using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    static class Formtest
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            string str = "3";
            //if (args.Length > 0) str = args[0];
            switch (str)
            {
                case "1"://地圖 html
                    using (Form1 testForm = new Form1())
                    {
                        testForm.ShowDialog();
                    }
                    break;
                case "2"://2D球
                    using (Form2 testForm = new Form2())
                    {
                        testForm.ShowDialog();
                    }
                    break;
                case "3"://3D球
                    using (Form3 testForm = new Form3())
                    {
                        testForm.ShowDialog();
                    }
                    break;
                case "4"://檔名
                    Thread td4 = new Thread(new ThreadStart(FM4));
                    td4.SetApartmentState(ApartmentState.STA);
                    td4.Start();
                    break;
                case "5"://圖片附上文字
                    Thread td5 = new Thread(new ThreadStart(FM5));
                    td5.SetApartmentState(ApartmentState.STA);
                    td5.Start();
                    break;
                case "6"://快捷鍵 滑鼠點擊畫面 特定位置
                    using (Form6 testForm = new Form6())
                    {
                        testForm.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                        testForm.Location = new System.Drawing.Point(0, 35);
                        testForm.ShowDialog();
                    }
                    break;
                case "7":
                    break;
            }
        }

        static void FM4()
        {
            using (Form4 testForm = new Form4())
            {
                testForm.ShowDialog();
            }
        }

        static void FM5()
        {
            using (Form5 testForm = new Form5())
            {
                testForm.ShowDialog();
            }
        }
    }
}
