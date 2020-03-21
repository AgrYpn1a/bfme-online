using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace BfmeOnline.Launcher.Source
{
    using DWORD = System.Int32;

    [Flags]
    public enum ProcessAccessType
    {
        PROCESS_TERMINATE = (0x0001),
        PROCESS_CREATE_THREAD = (0x0002),
        PROCESS_SET_SESSIONID = (0x0004),
        PROCESS_VM_OPERATION = (0x0008),
        PROCESS_VM_READ = (0x0010),
        PROCESS_VM_WRITE = (0x0020),
        PROCESS_DUP_HANDLE = (0x0040),
        PROCESS_CREATE_PROCESS = (0x0080),
        PROCESS_SET_QUOTA = (0x0100),
        PROCESS_SET_INFORMATION = (0x0200),
        PROCESS_QUERY_INFORMATION = (0x0400),
        PROCESS_QUERY_LIMITED_INFORMATION = (0x1000)
    }

    /// <summary>
    /// Use this to manage acccess to the process. Currently
    /// supports accessing only one process at the time.
    /// </summary>
    internal sealed class MemoryHijacker
    {

        [DllImport("Kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccessType dwDesiredAccess, bool bInheritHandle, DWORD dwProcessId);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr ReadProcessMemory(DWORD hProcess, DWORD lpBaseAddress, byte[] lpBuffer, DWORD dwSize, ref DWORD lpNumberOfBytesToRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        private static Process _process;
        private static IntPtr _pHandle;

        public static void OpenProcess(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
            {
                Logger.LogMessage("Could not find process game.dat", "memscan", Logger.LogType.Error);
                return;
            }

            _process = processes[0];
            DWORD pid = _process.Id;
            DWORD pAddress = (DWORD)_process.MainModule.BaseAddress;

            Console.WriteLine("Base address: {0:X}", pAddress);

            _pHandle = OpenProcess(ProcessAccessType.PROCESS_VM_OPERATION | ProcessAccessType.PROCESS_VM_READ,
                false, pid);

            if (_pHandle != null)
            {
                Logger.LogMessage("Got a handle.", "memscan");

                byte[] buffer = new byte[4];
                DWORD bytesRead = 0;
                IntPtr ptr = ReadProcessMemory((DWORD) _pHandle, pAddress + 0x00ef0898, buffer, buffer.Length, ref bytesRead);

                Logger.LogMessage("Ptr " + ptr);
                Logger.LogMessage($"{BitConverter.ToInt32(buffer).ToString("X")}");
            }
            else
            {
                Logger.LogMessage("Could not get a handle to game.dat", "memscan", Logger.LogType.Error);
                Logger.LogMessage("Closing...", "memscan", Logger.LogType.Error);
                CloseProcess();
            }
        }

        public static void CloseProcess()
        {
            if (!CloseHandle(_pHandle))
                throw new Exception("Failed to close the process handle. This may mess up something.");
        }

    }
}
