using ConsoleApplication1.Func;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ConsoleApplication1
{
    public partial class Form6 : Form
    {
        public struct setData
        {
            public int x;
            public int y;
            public int K;

            public int delay;
        }

        static List<setData> setList = new List<setData>();

        public Form6()
        {
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;

            this.Text = "鍵盤輔助工具V2_隨機誤差";

            label1.Text = "Tab  9\r\nEnter 13; 108\r\nSpacebar 32\r\nLeft Arrow    37\r\nUp Arrow  38\r\nRight Arrow   39\r\nDw Arrow  40";
            button1.Focus();
            
            getSetting();
        }

        static string FindWindowName = "";

        /// <summary>
        /// 設定點擊後回到原點的延遲時間
        /// </summary>
        static int iSleep = 0;

        /// <summary>
        /// 取得設定的資料
        /// </summary>
        private void getSetting()
        {
            try
            {
                string sIniName = "jj6.ini";
                string Path = System.IO.Directory.GetCurrentDirectory() + "\\";

                if (File.Exists(Path + sIniName))
                {
                    using (StreamReader sr = new StreamReader(Path + sIniName))
                    {
                        //FindWindowName = sr.ReadLine();
                        try
                        {
                            iSleep = Convert.ToInt32(sr.ReadLine());
                        }
                        catch (Exception)
                        {
                            
                        }
                        
                        if (setList.Count != 0) setList.Clear();

                        string str = sr.ReadLine();
                        while (str != null)
                        {
                            string[] Arr = str.Split(';');
                            setData data = new setData();
                            try
                            {
                                data.x = Convert.ToInt32(Arr[0]);
                                data.y = Convert.ToInt32(Arr[1]);
                                data.K = Convert.ToInt32(Arr[2]);
                                if (Arr.Length >= 4) data.delay = Convert.ToInt32(Arr[3]);
                                else data.delay = 0;
                                setList.Add(data);
                            }
                            catch (Exception)
                            {
                            }
                            str = sr.ReadLine();
                        }
                    }
                    label2.Text = $@"已成功讀取{sIniName}，共讀取:{setList.Count}筆設定資料
按下開始後，開始效果。
效果開關快捷鍵F1";
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(Path + sIniName))
                    {
                        sw.WriteLine("設定按下左鍵後回到原點的持續時間(單位:千分之一秒)");
                        sw.WriteLine("X;Y;KeyCode;按下左鍵持續時間(單位:千分之一秒)");
                    }
                    label2.Text = "需先編輯設定" + sIniName + "後，再讀取設定資料\r\nKeyCode請GOOGLE";
                }
            }
            catch (Exception ex)
            {
                label2.Text = ex.ToString();
            }
        }

        //***取得全域按鍵的事件
        static int m_HookHandle = 0;

        /// <summary>
        /// SetWindowsHookEx
        /// </summary>
        private static void Install()
        {
            try
            {
                Process curProcess = Process.GetCurrentProcess();
                ProcessModule curModule = curProcess.MainModule;

                User32.HookProc m_HookProc = new User32.HookProc(HookProc);
                m_HookHandle = User32.SetWindowsHookEx(13, m_HookProc, GetModuleHandle(curModule.ModuleName), 0);

                curModule.Dispose();
                curProcess.Dispose();

                if (m_HookHandle == 0)
                    throw new Exception("Install Hook Faild.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        private static void Uninstall()
        {
            if (m_HookHandle != 0)
            {
                bool ret = User32.UnhookWindowsHookEx(m_HookHandle);

                if (ret)
                    m_HookHandle = 0;
                else
                    throw new Exception("Uninstall Hook Faild.");
            }
        }

        
        /// <summary>
        /// 繼續讀取按鍵 但是不做設定上的動作
        /// </summary>
        static bool isPause2 = false;
        private static int HookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (isPause == false)
                {
                    int wParam_Int32 = wParam.ToInt32();
                    if (nCode >= 0)
                    {
                        if (wParam_Int32 == 0x100 || wParam_Int32 == 0x104)
                        {
                            KEYBOARDLLHookStruct keyboardHookStruct = (KEYBOARDLLHookStruct)Marshal.PtrToStructure(lParam, typeof(KEYBOARDLLHookStruct));
                            //WM_KEYDOWN

                            if (keyboardHookStruct.VirtualKeyCode == 112)
                            {
                                //快捷按鈕"~" 暫時停止功能 等待~ 再度開始
                                //改F1(112)
                                isPause2 = !isPause2;
                                return -1;
                            }

                            if (isPause2 == false)
                            {
                                var data = setList.Where(x => x.K == keyboardHookStruct.VirtualKeyCode).ToList();
                                if (data.Count > 0)
                                {
                                    foreach (var it in data)
                                    {
                                        ME_LEFT(it);
                                    }
                                    //如果有設定 就不傳給下一個
                                    return -1;
                                }
                            }
                        }
                        //else if (wParam_Int32 == 0x101 || wParam_Int32 == 0x105)
                        //{
                        //    //WM_KEYUP

                        //}
                    }
                }
                
                return User32.CallNextHookEx(m_HookHandle, nCode, wParam, lParam);
            }
            catch (Exception)
            {
                return User32.CallNextHookEx(m_HookHandle, nCode, wParam, lParam);
            }
        }
        
        public struct KEYBOARDLLHookStruct
        {
            public int VirtualKeyCode;
            public int ScanCode;
            public int Flags;
            public int Time;
            public int ExtraInfo;
        }
        //******

        static System.IntPtr hl = IntPtr.Zero;
        public static void ME_LEFT(setData data)
        {
            Random rd = new Random();
            try
            {
                //System.IntPtr hl = User32.FindWindow(null, FindWindowName);
                //if (hl == IntPtr.Zero) return;

                //先記錄下原本的游標位置
                int X = Cursor.Position.X;
                int Y = Cursor.Position.Y;


                //if (hl != IntPtr.Zero) User32.SetForegroundWindow(hl);
                //移到目標位置
                var rdx = rd.Next(-5, 5);
                var rdy = rd.Next(-5, 5);
                if (data.x >= 0 && data.y >= 0)
                {
                    
                    var LEFTDOWN_X = data.x + rdx;
                    var LEFTDOWN_Y = data.y + rdy;
                    if (LEFTDOWN_X < 0) LEFTDOWN_X = 0;
                    if (LEFTDOWN_Y < 0) LEFTDOWN_Y = 0;

                    User32.SetCursorPos(LEFTDOWN_X, LEFTDOWN_Y);
                }
                //User32.mouse_event(User32.MOUSEEVENTF_ABSOLUTE | User32.MOUSEEVENTF_MOVE, 
                //    data.x * 65535 / SystemInformation.PrimaryMonitorSize.Width, data.y * 65535 / SystemInformation.PrimaryMonitorSize.Height, 0, 0);

                //點擊
                //User32.mouse_event(User32.MOUSEEVENTF_LEFTDOWN | User32.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                User32.mouse_event(User32.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                var rdt = rd.Next(0, 50);
                var SleepTime = iSleep + rdt;
                if (data.delay > 0) SleepTime = data.delay + rdt;
                if (SleepTime > 0) System.Threading.Thread.Sleep(SleepTime);

                //回到原本位置
                User32.SetCursorPos(X+ rdx, Y+ rdy);
                User32.mouse_event(User32.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

                //User32.mouse_event(User32.MOUSEEVENTF_ABSOLUTE | User32.MOUSEEVENTF_MOVE, X, Y, 0, 0);
                //User32.mouse_event(User32.MOUSEEVENTF_ABSOLUTE | User32.MOUSEEVENTF_MOVE,
                //    X * 65535 / SystemInformation.PrimaryMonitorSize.Width, Y * 65535 / SystemInformation.PrimaryMonitorSize.Height, 0, 0);
            }
            catch (Exception)
            {
            }
        }
        
        /// <summary>
        /// 取得座標
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = Cursor.Position.X + ";" + Cursor.Position.Y + ";";
                //"X:" + System.Windows.Forms.Cursor.Position.X + " Y:" + Cursor.Position.Y;
            }
            catch (Exception ex)
            {
                label2.Text = ex.ToString();
            }
        }

        bool isInstall = false;
        /// <summary>
        /// 開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (isInstall == false)
                {
                    Install();
                    isInstall = true;
                }
                else
                {
                    if (isPause)
                    {
                        isPause = false;
                        isPause2 = false;
                    }
                }
                Lab2text();
            }
            catch (Exception ex)
            {
                label2.Text = ex.ToString();
            }
        }
        /// <summary>
        /// 程式關閉 並取消註冊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //關閉程式
            try
            {
                if (isInstall) Uninstall();
                this.Close();
                Environment.Exit(Environment.ExitCode);
            }
            catch (Exception ex)
            {
                label2.Text = ex.ToString();
            }
        }
        
        /// <summary>
        /// 無論如何都會直接傳給下一個Hook
        /// </summary>
        static bool isPause = false;

        /// <summary>
        /// 暫停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (isInstall)
                {
                    isPause = !isPause;
                    if (isPause == false) isPause2 = false;
                    Lab2text();
                }
            }
            catch (Exception ex)
            {
                label2.Text = ex.ToString();
            }
        }

        private void Lab2text()
        {
            try
            {
                if (isPause)
                {
                    label2.Text = "效果暫停";
                }
                else
                {
                    label2.Text = "效果啟動中";
                }
            }
            catch (Exception ex)
            {
                label2.Text = ex.ToString();
            }
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                textBox1.Text = Cursor.Position.X + ";" + Cursor.Position.Y + ";" + e.KeyValue.ToString();
                try
                {
                    System.Windows.Forms.Clipboard.SetText(textBox1.Text + "\r\n");
                    label2.Text = "已複製到剪貼簿";
                }
                catch (Exception)
                {
                    label2.Text = "複製到剪貼簿失敗";
                }

            }
            catch (Exception ex)
            {
                label2.Text = label2.Text + ex.ToString();
            }
        }

        /// <summary>
        /// 重新載入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                isPause = true;
                getSetting();
            }
            catch (Exception ex)
            {
                label2.Text = ex.ToString();
            }
        }
    }
}
