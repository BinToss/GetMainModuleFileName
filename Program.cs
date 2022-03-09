using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace GetMainModuleFileName
{
    internal static class Extensions
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool QueryFullProcessImageName(
            [In] SafeHandle hProcess,
            [In] uint dwFlags,
            [Out] StringBuilder lpExeName,
            [In, Out] ref uint lpdwSize);

        public static string? GetMainModuleFileName(this Process process, int buffer = 1024)
        {
            var fileNameBuilder = new StringBuilder(buffer);
            uint bufferLength = (uint)fileNameBuilder.Capacity + 1;
            return QueryFullProcessImageName(process.SafeHandle, 0, fileNameBuilder, ref bufferLength) ?
                fileNameBuilder.ToString() :
                null;
        }
    }

    public static class Invoke
    {
        public static void Main()
        {
            Console.WriteLine("Input process name or keyword");
            string i = GetInput();
            Process p = Process.GetProcesses().First(p => p.ProcessName.ToLower().Contains(i));
            Console.WriteLine(p.ProcessName);
            string? path = p.GetMainModuleFileName();
            Console.WriteLine(path ?? "null");
            Console.WriteLine("StartInfo: " + p.MainModule?.FileName);
        }
        private static string GetInput()
        {
            string i = string.Empty;
            bool success = false;
            while (!success)
            {
                i = Console.ReadLine() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(i))
                {
                    success = true;
                }
            }
            return i;
        }
    }
}
