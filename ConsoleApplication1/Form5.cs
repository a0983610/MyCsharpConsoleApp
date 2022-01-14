using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication1
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "求打賞";
            this.TopMost = true;

            textBox1.DragDrop += textBox1_DragDrop;
            textBox1.DragEnter += textBox1_DragEnter;
            textBox1.KeyDown += textBox1_KeyDown;

            getInitSetting();
        }
        
        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop) as string[];
            StringBuilder sb = new StringBuilder();
            foreach (var it in data)
            {
                //int idx = it.LastIndexOf("\\") + 1;
                //if (idx < it.Length)
                //{
                //    string text = it.Substring(idx);
                //    sb.Append(text + "\r\n");
                //}

                sb.Append(it + ";");
            }
            textBox1.Text += sb.ToString();
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        /// <summary>
        /// 產生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            ImageTest ImageTest = new ImageTest();
            TryGetPara(ImageTest);

            ImageTest.DirectoryName = "processing";
            ImageTest.Path = System.IO.Directory.GetCurrentDirectory();

            string FileNames = textBox1.Text;
            string[] str = FileNames.Split(';');

            //foreach(var it in str)
            //{
            //    if (File.Exists(it))
            //    {
            //        label4.Text = it;
            //        ImageTest.ImageDrawString(textBox5.Text, it);
            //    }
            //}

            //******
            label4.Text = "處理中......";
            Parallel.ForEach(str, it => 
            {
                if (File.Exists(it))
                {
                    ImageTest.ImageDrawString(textBox5.Text, it);
                }
            });
            //******

            //完成處理後
            button1.Enabled = true;
            textBox1.Text = "";
            label4.Text = "全部已完成";
        }

        /// <summary>
        /// 取得畫面上的參數
        /// </summary>
        /// <param name="ImageTest"></param>
        private void TryGetPara(ImageTest ImageTest)
        {
            try
            {
                ImageTest.FontEmsize = Convert.ToInt32(textBox4.Text);
            }
            catch (Exception)
            {
                ImageTest.FontEmsize = 22;
            }

            try
            {
                ImageTest.height = Convert.ToInt32(textBox3.Text);
            }
            catch (Exception)
            {
                ImageTest.height = 50;
            }

            try
            {
                ImageTest.width = Convert.ToInt32(textBox2.Text);
            }
            catch (Exception)
            {
                ImageTest.width = 200;
            }
        }

        /// <summary>
        /// 從ini檔案取資料到畫面上
        /// </summary>
        private void getInitSetting()
        {
            string Path = System.IO.Directory.GetCurrentDirectory();
            if(File.Exists(Path + "\\jj5.ini"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(Path + "\\jj5.ini"))
                    {
                        string ini = sr.ReadToEnd();
                        string[] str = ini.Split(';');

                        textBox2.Text = str[0];//w
                        textBox3.Text = str[1];//h
                        textBox4.Text = str[2];//size
                        textBox5.Text = str[3];//str
                    }
                }
                catch (Exception)
                {
                    
                }
            }
        }

        /// <summary>
        /// 結束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string Path = System.IO.Directory.GetCurrentDirectory();
            try
            {
                using (StreamWriter sw = new StreamWriter(Path + "\\jj5.ini"))
                {
                    sw.Write(string.Format("{0};{1};{2};{3}", textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text));
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception)
            {
                
            }
            
            this.Close();
            this.Dispose();
            Environment.Exit(Environment.ExitCode);
        }
    }
}
