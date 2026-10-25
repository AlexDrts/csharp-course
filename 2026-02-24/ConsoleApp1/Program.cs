using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

class WindowManager
{
    [DllImport("user32.dll")]
    static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll")]
    static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    public List<(IntPtr, string)> windows = new List<(IntPtr, string)>();

    public void FindActiveWindows()
    {
        windows.Clear();

        EnumWindows((hWnd, lParam) =>
        {
            if (!IsWindowVisible(hWnd))
                return true;

            StringBuilder sbTitle = new StringBuilder(256);
            GetWindowText(hWnd, sbTitle, sbTitle.Capacity);
            string title = sbTitle.ToString();

            if (string.IsNullOrWhiteSpace(title))
                return true;

            StringBuilder sbClass = new StringBuilder(256);
            GetClassName(hWnd, sbClass, sbClass.Capacity);
            string className = sbClass.ToString();

            string[] systemClasses = { "Shell_TrayWnd", "Progman", "Button", "DV2ControlHost", "MsgrIMEWindowClass" };
            foreach (var sysClass in systemClasses)
            {
                if (className == sysClass)
                    return true; 
            }

            windows.Add((hWnd, title));

            return true;
        }, IntPtr.Zero);
    }

    public void ForceClose(IntPtr hWnd)
    {
        GetWindowThreadProcessId(hWnd, out uint pid);
        try
        {
            Process proc = Process.GetProcessById((int)pid);
            Console.WriteLine($"Примусово закриваємо {proc.ProcessName}, PID={proc.Id}");
            proc.Kill();
            proc.WaitForExit();
            Console.WriteLine("Процес закрито!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при закритті: {ex.Message}");
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var wm = new WindowManager();

        while (true)
        {
            wm.FindActiveWindows();

            Console.WriteLine("\nСписок активних вікон користувача:");
            for (int i = 0; i < wm.windows.Count; i++)
            {
                Console.WriteLine($"{i} - {wm.windows[i].Item2}");
            }

            Console.Write("\nВибери номер вікна для примусового закриття (або -1 для оновлення): ");
            if (!int.TryParse(Console.ReadLine(), out int index))
            {
                Console.WriteLine("Невірний вибір!");
                continue;
            }

            if (index == -1) continue;

            if (index < 0 || index >= wm.windows.Count)
            {
                Console.WriteLine("Невірний вибір!");
                continue;
            }

            var hWnd = wm.windows[index].Item1;
            wm.ForceClose(hWnd);
        }
    }
}
