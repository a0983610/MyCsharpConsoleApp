using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class ImageTest
    {
        public string Path = System.IO.Directory.GetCurrentDirectory();

        public int FontEmsize = 22;
        public float width = 200;
        public float height = 50;

        public string DirectoryName = "Pic";

        public void ImageDrawString(string drawString, string FileName)
        {
            using (Image i = Image.FromFile(FileName))
            {
                using (Graphics G = Graphics.FromImage(i))
                {

                    //調高畫值 消除鋸齒
                    G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    G.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    G.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    //******

                    int H = i.Height;
                    int W = i.Width;

                    // Create font and brush.
                    using (Font drawFont = new Font("Arial", FontEmsize))
                    {
                        SolidBrush drawBrush = new SolidBrush(Color.Yellow);

                        // Create rectangle for drawing.
                        float x = W - width;
                        float y = H - height;

                        RectangleF drawRect = new RectangleF(x, y, width, height);

                        // Set format of string.
                        using (StringFormat drawFormat = new StringFormat())
                        {
                            drawFormat.Alignment = StringAlignment.Center;

                            // Draw string to screen.
                            G.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);

                            string newFileName = FileName.Substring(FileName.LastIndexOf("\\"));

                            CreateDirectory(DirectoryName);
                            i.Save(Path + "\\" + DirectoryName + "\\" + newFileName, ImageFormat.Jpeg);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 資料夾
        /// </summary>
        /// <param name="sName"></param>
        public void CreateDirectory(string sName)
        {
            if (!Directory.Exists(Path + sName))
            {
                Directory.CreateDirectory(Path + "\\" + sName + "\\");
            }
        }
    }
}
