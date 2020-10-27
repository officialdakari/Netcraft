using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Minecraft2D
{
    static class ProcessSuspend
    {
        public enum ThreadAccess : int
        {
            TERMINATE = 0x1,
            SUSPEND_RESUME = 0x2,
            GET_CONTEXT = 0x8,
            SET_CONTEXT = 0x10,
            SET_INFORMATION = 0x20,
            QUERY_INFORMATION = 0x40,
            SET_THREAD_TOKEN = 0x80,
            IMPERSONATE = 0x100,
            DIRECT_IMPERSONATION = 0x200
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        public static extern uint ResumeThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hHandle);

        public static void SuspendProcess(Process process)
        {
            foreach (ProcessThread t in process.Threads)
            {
                IntPtr th;
                th = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)t.Id);
                if (th != IntPtr.Zero)
                {
                    SuspendThread(th);
                    CloseHandle(th);
                }
            }
        }

        public static void ResumeProcess(Process process)
        {
            foreach (ProcessThread t in process.Threads)
            {
                IntPtr th;
                th = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)t.Id);
                if (th != IntPtr.Zero)
                {
                    ResumeThread(th);
                    CloseHandle(th);
                }
            }
        }
    }
}