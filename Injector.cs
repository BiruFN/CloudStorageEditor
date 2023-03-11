using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public static class Injector
{
    public static void Inject(int processId, string path)
    {
        if (!File.Exists(path))
        {
            return;
        }
        var handle = OpenProcess(2 | 0x0400 | 8 | 0x0020 | 0x0010, false, processId);

        var loadLibrary = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

        var size = (uint)((path.Length + 1) * Marshal.SizeOf(typeof(char)));
        var address = VirtualAllocEx(handle, IntPtr.Zero, size, 0x1000 | 0x2000, 4);

        WriteProcessMemory(handle, address, Encoding.Default.GetBytes(path), size, out UIntPtr bytesWritten);

        CreateRemoteThread(handle, IntPtr.Zero, 0, loadLibrary, address, 0, IntPtr.Zero);
    }

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
}
