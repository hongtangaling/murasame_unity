using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.IO;
using static TransparentWindow;
using UnityEngine.XR;
using UnityEngine;
/// <summary>
/// 一共可选择三种样式
/// </summary>
public enum enumWinStyle
{
    /// <summary>
    /// 置顶
    /// </summary>
    WinTop,
    /// <summary>
    /// 透明
    /// </summary>
    Apha,
    /// <summary>
    /// 置顶并且透明
    /// </summary>
    WinTopApha,
    /// <summary>
    /// 置顶透明并且可以穿透
    /// </summary>
    WinTopAphaPenetrate
}
public class TransparentWindow : MonoBehaviour
{

    // 定义RECT结构体，表示窗口的矩形区域
    [Serializable]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    #region Win函数常量
    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }
    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr hwnd);

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("User32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

    [DllImport("user32.dll")]
    static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);

    [DllImport("Dwmapi.dll")]
    static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]

    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
    private const int WS_POPUP = 0x800000;
    private const int GWL_EXSTYLE = -20;
    private const int GWL_STYLE = -16;
    private const int WS_EX_LAYERED = 0x00080000;
    private const int WS_BORDER = 0x00800000;
    private const int WS_CAPTION = 0x00C00000;
    private const int SWP_SHOWWINDOW = 0x0040;
    private const int LWA_COLORKEY = 0x00000001;
    private const int LWA_ALPHA = 0x00000002;
    private const int WS_EX_TRANSPARENT = 0x20;
    //
    private const int ULW_COLORKEY = 0x00000001;
    private const int ULW_ALPHA = 0x00000002;
    private const int ULW_OPAQUE = 0x00000004;
    private const int ULW_EX_NORESIZE = 0x00000008;

    public const int SW_HIDE = 0;
    public const int SW_MAXIMIZE = 3;
    public const int SW_SHOW = 0;
    public const int SW_MINIMIZE = 6;
    public const int SW_RESTORE = 9;


    #endregion
    //
    public string strProduct;//项目名称
    public enumWinStyle WinStyle = enumWinStyle.WinTop;//窗体样式
    //
    public int ResWidth ;//窗口宽度
    public int ResHeight ;//窗口高度
    //
    public int currentX;//窗口左上角坐标x
    public int currentY;//窗口左上角坐标y
    //
    //private bool isWinTop;//是否置顶
    private bool isApha;//是否透明
    private bool isAphaPenetrate;//是否要穿透窗体

    IntPtr hwnd;
    //float xx;
    //bool bx;

    bool istop = true;

    // Use this for initialization
    void Awake()
    {
        //xx = 0f;
        //bx = false;
        Screen.fullScreen = false;
        //#if UNITY_EDITOR
        //       print("编辑模式不更改窗体");
        //#else
        switch (WinStyle)
        {
            case enumWinStyle.WinTop:
                isApha = false;
                isAphaPenetrate = false;
                break;
            case enumWinStyle.Apha:
                //isWinTop = false;
                isApha = true;
                isAphaPenetrate = false;
                break;
            case enumWinStyle.WinTopApha:
                isApha = true;
                isAphaPenetrate = false;
                break;
            case enumWinStyle.WinTopAphaPenetrate:
                isApha = true;
                isAphaPenetrate = true;
                break;
        }

        //
        //窗口句柄
        hwnd = GetActiveWindow();

        //
        if (isApha)
        {

            SetWindowCanTransparent(isAphaPenetrate);

            //保持中间位置：因为是从左上角算起的，所以获得屏幕像素后要减去窗体宽高的一半
            currentX = Screen.currentResolution.width / 2 - Screen.width / 2;
            currentY = Screen.currentResolution.height / 2 - Screen.height / 2; ;

            SetWindowPos(hwnd, -1, currentX, currentY, ResWidth, ResHeight, SWP_SHOWWINDOW);
            var margins = new MARGINS() { cxLeftWidth = -1 };
            //
            DwmExtendFrameIntoClientArea(hwnd, ref margins);
            //SetLayeredWindowAttributes(hwnd, 0, 255, 1);

        }
        else
        {
            //单纯去边框
            SetWindowLong(hwnd, GWL_STYLE, WS_POPUP);
            SetWindowPos(hwnd, -1, currentX, currentY, ResWidth, ResHeight, SWP_SHOWWINDOW);
        }
        Debug.Log(WinStyle);



        //#endif
    }

    void SetWindowCanTransparent(bool can)
    {

        Application.runInBackground = true;
        //去边框并且透明
        SetWindowLong(hwnd, GWL_EXSTYLE, WS_EX_LAYERED);
        if (can)//是否透明穿透窗体
        {
            int intExTemp = GetWindowLong(hwnd, GWL_EXSTYLE);

            SetWindowLong(hwnd, GWL_EXSTYLE, intExTemp | WS_EX_TRANSPARENT | WS_EX_LAYERED);
        }

        SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_BORDER & ~WS_CAPTION);


    }
    void OnApplicationQuit()
    {
        //程序退出的时候设置窗体为0像素，从打开到走到awake也需要一定是时间
        //会先有窗体边框，然后透明，这样会有闪一下的效果，
        //设置窗体为0像素后，下次打开是就是0像素，走到awake再设置回来正常的窗口大小
        //便能解决程序加载时会闪白色边框的现象
        SetWindowPos(hwnd, -1, currentX, currentY, 0, 0, SWP_SHOWWINDOW);
    }
    private static bool mouseEnter = false;

    void Update()
    {
        if (MouseInformation.ChangeColor.r == 0 && MouseInformation.ChangeColor.g == 0 && MouseInformation.ChangeColor.b == 0)
        {
            if (TransparentWindow.mouseEnter == false)
            {
                return;
            }
            TransparentWindow.mouseEnter = false;
            Debug.Log("鼠标离开 且置为 穿透");
            SetWindowCanTransparent(true);
            return;
        }
        Debug.Log("鼠标进入 且置为 不穿透");
        if (TransparentWindow.mouseEnter == true)
        {
            return;
        }
        TransparentWindow.mouseEnter = true;

        SetWindowCanTransparent(false);
        //弃用的窗口移动
        //if (Input.GetMouseButtonDown(0))
        //{

        //    xx = 0f;
        //    bx = true;
        //}
        //if (bx && xx >= 0f)
        //{ //这样做为了区分界面上面其它需要滑动的操作
        //    ReleaseCapture();
        //    SendMessage(hwnd, 0xA1, 0x02, 0);
        //    SendMessage(hwnd, 0x0202, 0, 0);


        //}
        //if (bx)
        //    xx += Time.deltaTime;
        //if (Input.GetMouseButtonUp(0))
        //{
        //    xx = 0f;
        //    bx = false;
        //}
    }

    public void SetTop(bool isTop)
    {

        istop = isTop;
        // 检查窗口句柄是否有效
        if (hwnd == IntPtr.Zero)
        {
            Debug.LogError("窗口句柄无效！");
            return;
        }
        RECT rect;
        int cx = currentX;
        int cy = currentY;
        if (GetWindowRect(hwnd, out rect))
        {
            cx = rect.left;
            cy = rect.top;
        }
        currentX = cx;
        currentY = cy;
        if (isTop == true)
        {
            SetWindowPos(hwnd, -1, currentX, currentY, ResWidth, ResHeight, SWP_SHOWWINDOW);
            var margins = new MARGINS() { cxLeftWidth = -1 };
            DwmExtendFrameIntoClientArea(hwnd, ref margins);

        }
        else
        {
            SetWindowPos(hwnd, -2, currentX, currentY, ResWidth, ResHeight, SWP_SHOWWINDOW);
            var margins = new MARGINS() { cxLeftWidth = -1 };
            DwmExtendFrameIntoClientArea(hwnd, ref margins);
        }

    }


    public void SetSize(int size)
    {
        // 检查窗口句柄是否有效
        if (hwnd == IntPtr.Zero)
        {
            Debug.LogError("窗口句柄无效！");
            return;
        }
        int rw = ResWidth;
        int rh = ResHeight;
        int cx = currentX;
        int cy = currentY;


        RECT rect;
        if (GetWindowRect(hwnd, out rect))
        {
            cx = rect.left;
            cy = rect.top;
        }

        cx = cx + (ResWidth - size) / 2;
        cy = cy + (ResHeight - size) / 2;


        rw = size;
        rh = size;
        ResWidth = rw;
        ResHeight = rh;
        currentX = cx;
        currentY = cy;

        if (istop == true)
        {
            SetWindowPos(hwnd, -1, currentX, currentY, ResWidth, ResHeight, SWP_SHOWWINDOW);
        }
        else
        {
            SetWindowPos(hwnd, -2, currentX, currentY, ResWidth, ResHeight, SWP_SHOWWINDOW);
        }
    }

    public void ShowTaskBar()
    {
        ShowWindow(hwnd, SW_RESTORE);
    }
    /// <summary>
    /// Hide TaskBar
    /// </summary>
    public void HideTaskBar()
    {
        ShowWindow(hwnd, SW_HIDE);
    }

}