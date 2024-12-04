using System.Runtime.InteropServices;
using System.Text;
using System;
using RobloxAutoLauncher;
using System.Drawing;

public class User32 // user32.dll imports
{
	[DllImport("User32.dll")]
	public static extern int GetWindowLong(IntPtr hwnd, int nIndex);

	[DllImport("User32.dll")]
	public static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

	[DllImport("User32.dll")]
	public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc,
		WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

	[DllImport("User32.dll")]
	public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr voidProcessId);

	[DllImport("User32.dll", EntryPoint = "SetWindowPos")]
	public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

	[DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("User32.dll", SetLastError = true)]
    public static extern bool GetWindowRect(IntPtr hWnd, out ProcessRectangle lpRect);

    [DllImport("User32.dll", SetLastError = true)]
    public static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

    [DllImport("User32.dll")]
	public static extern IntPtr GetForegroundWindow();

	public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
}