using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleApplication1.Func
{
    static class User32
    {

        [DllImport("USER32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("USER32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        public const int MOUSEEVENTF_LEFTDOWN = 0x2;
        public const int MOUSEEVENTF_LEFTUP = 0x4;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x8; //模拟鼠标右键按下
        public const int MOUSEEVENTF_RIGHTUP = 0x10; //模拟鼠标右键抬起
        public const int MOUSEEVENTF_MOVE = 0x1; //移动鼠标
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;//标示是否采用绝对坐标

        [DllImport("USER32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SetCursorPos(int x, int y);

        [DllImport("USER32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool GetCursorPos(out Point p);

        [DllImport("USER32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);


        [DllImport("USER32.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("USER32.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("USER32.DLL", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);


        [DllImport("USER32.DLL", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);


        [DllImport("USER32.DLL")]
        public static extern int GetWindowRect(IntPtr hwnd, out Rect lpRect);
        
    }
}
