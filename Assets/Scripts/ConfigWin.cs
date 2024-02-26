using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;

public class ConfigWin : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr CreateWindowEx(
        int dwExStyle,
        string lpClassName,
        string lpWindowName,
        int dwStyle,
        int x,
        int y,
        int nWidth,
        int nHeight,
        IntPtr hWndParent,
        IntPtr hMenu,
        IntPtr hInstance,
        IntPtr lpParam
    );

    [DllImport("user32.dll")]
    private static extern bool DestroyWindow(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    private const int GWL_STYLE = -16;
    private const int GWL_EXSTYLE = -20;
    private const int WS_SYSMENU = 0x00080000;
    private const int WS_CAPTION = 0x00C00000;
    private const int WS_VISIBLE = 0x10000000;
    private const int WS_OVERLAPPEDWINDOW = 0xCF0000;

    private IntPtr windowHandle;

    void Start()
    {
        // 创建一个独立窗体
        CreateNativeWindow();
    }

    void OnDestroy()
    {
        // 销毁窗体
        DestroyNativeWindow();
    }

    void CreateNativeWindow()
    {
        windowHandle = CreateWindowEx(
            0,
            "STATIC",
            "My Native Window",
            WS_VISIBLE | WS_OVERLAPPEDWINDOW,
            Screen.width / 4,
            Screen.height / 4,
            400,
            300,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero
        );

        // 激活系统菜单（包括关闭按钮）
        SetWindowLong(windowHandle, GWL_STYLE, GetWindowLong(windowHandle, GWL_STYLE) | WS_SYSMENU | WS_CAPTION);
    }

    void DestroyNativeWindow()
    {
        // 销毁窗体
        if (windowHandle != IntPtr.Zero)
        {
            DestroyWindow(windowHandle);
        }
    }
}
