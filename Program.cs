using System;
using System.Text;
using System.Threading;
namespace BounceMyTerminal
{
    static class Program
    {
        static void Main(string[] args)
        {
            IntPtr hwnd = WindowsAPI.FindWindow("CASCADIA_HOSTING_WINDOW_CLASS", null);
            if (hwnd == IntPtr.Zero)
                hwnd = WindowsAPI.GetConsoleWindow();

            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();
            // DVD Logo Ascii
            Console.WriteLine("⠀⠀⣸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⢀⣾⣿⣿⣿⣿⣿⣿⣿⣿⣶⣦⡀\n⠀⢠⣿⣿⡿⠀⠀⠈⢹⣿⣿⡿⣿⣿⣇⠀⣠⣿⣿⠟⣽⣿⣿⠇⠀⠀⢹⣿⣿⣿\n⠀⢸⣿⣿⡇⠀⢀⣠⣾⣿⡿⠃⢹⣿⣿⣶⣿⡿⠋⢰⣿⣿⡿⠀⠀⣠⣼⣿⣿⠏\n⠀⣿⣿⣿⣿⣿⣿⠿⠟⠋⠁⠀⠀⢿⣿⣿⠏⠀⠀⢸⣿⣿⣿⣿⣿⡿⠟⠋⠁⠀\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣀⣀⣸⣟⣁⣀⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\n⣠⣴⣶⣾⣿⣿⣻⡟⣻⣿⢻⣿⡟⣛⢻⣿⡟⣛⣿⡿⣛⣛⢻⣿⣿⣶⣦⣄⡀⠀\n⠉⠛⠻⠿⠿⠿⠷⣼⣿⣿⣼⣿⣧⣭⣼⣿⣧⣭⣿⣿⣬⡭⠾⠿⠿⠿⠛⠉⠀");
            Bounce(hwnd);
        }

        

        private static void Bounce(IntPtr hwnd)
        {
            int windowWidth = 400;
            int windowHeight = 400;

            // Get initial screen size
            var (screenWidth, screenHeight) = GetScreenSize();

            int x = 0;
            int y = 0;
            // Speeds 
            int dx = 5;
            int dy = 5; 

            // Ensure we don't start off-screen
            if (screenWidth < windowWidth) windowWidth = screenWidth;
            if (screenHeight < windowHeight) windowHeight = screenHeight;

            while (true)
            {
                WindowsAPI.SetWindowPos(hwnd,
                    new IntPtr(-1), // HWND_Topmost
                    x, y, windowWidth, windowHeight,
                    WindowsAPI.SWP_NOACTIVATE);
                x += dx;
                y += dy;
                // Check Boundaries so we know to reverse the direction when an edge is hit.
                // This is kinda funky on the terminal but whatever it works
                // Right Edge
                if (x + windowWidth >= screenWidth)
                {
                    x = screenWidth - windowWidth;
                    dx = -dx;
                }
                // Left Edge
                else if (x <= 0)
                {
                    x = 0;
                    dx = -dx;
                }

                // Bottom Edge
                if (y + windowHeight >= screenHeight)
                {
                    y = screenHeight - windowHeight;
                    dy = -dy; 
                }
                // Top Edge
                else if (y <= 0)
                {
                    y = 0;
                    dy = -dy;
                }

                // Sleep to control speed and save CPU
                // 16ms Should be around 60 FPS. (I think so)
                Thread.Sleep(16);
            }
        }

        private static (int screenWidth, int screenHeight) GetScreenSize()
        {
            WindowsAPI.SetProcessDPIAware();
            int screenWidth = WindowsAPI.GetSystemMetrics(WindowsAPI.SM_CXSCREEN);
            int screenHeight = WindowsAPI.GetSystemMetrics(WindowsAPI.SM_CYSCREEN);
            return (screenWidth, screenHeight);
        }
    }
}