
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Minecraft2D.My
{
    // Для MyApplication имеются следующие события:
    // Startup: возникает при запуске приложения перед созданием начальной формы.
    // Shutdown: возникает после закрытия всех форм приложения.  Это событие не создается, если происходит аварийное завершение работы приложения.
    // UnhandledException: возникает, если в приложении обнаруживается необработанное исключение.
    // StartupNextInstance: возникает при запуске приложения, допускающего одновременное выполнение только одного экземпляра, если это приложение уже активно. 
    // NetworkAvailabilityChanged: возникает при изменении состояния подключения — при подключении или отключении.
    internal partial class MyApplication
    {
        [DllImport("KERNEL32.DLL")]
        static extern ErrorModes SetErrorMode(ErrorModes mode);

        public enum ErrorModes : uint
        {
            SYSTEM_DEFAULT = 0x0U,
            SEM_FAILCRITICALERRORS = 0x0001U,
            SEM_NOALIGNMENTFAULTEXCEPT = 0x0004U,
            SEM_NOGPFAULTERRORBOX = 0x0002U,
            SEM_NOOPENFILEERRORBOX = 0x8000U
        }

        protected override bool OnUnhandledException(Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs e)
        {
            Form1.GetInstance().toNotice = $"OOPS, NETCRAFT IS CRASHED...\r\n\r\nAn unhandled exception in Netcraft occured.\r\n\r\nInformation:\r\n[Game version] {MainMenu.GetInstance().Ver}\r\n[Your OS: Platform] {Environment.OSVersion.Platform}\r\n[Your OS: ServicePack] {Environment.OSVersion.ServicePack}\r\n[Your OS: Version] {Environment.OSVersion.VersionString}\r\n" +
                "\r\nException information\r\n" + e.Exception.ToString();
            e.ExitApplication = false;
            Form1.GetInstance().toNoticeType = 1;
            Form1.GetInstance().Close();
            return base.OnUnhandledException(e);
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        internal static void threadException(object sender, System.UnhandledExceptionEventArgs e)
        {
            if(Form1.instance != null && Form1.GetInstance().Visible)
            {
                Form1.GetInstance().toNotice = $"OOPS, NETCRAFT IS CRASHED...\r\n\r\nAn unhandled exception in Netcraft occured.\r\n\r\nInformation:\r\n[Game version] {MainMenu.GetInstance().Ver}\r\n[Your OS: Platform] {Environment.OSVersion.Platform}\r\n[Your OS: ServicePack] {Environment.OSVersion.ServicePack}\r\n[Your OS: Version] {Environment.OSVersion.VersionString}\r\n" +
                    "\r\nException information\r\n" + e.ExceptionObject.ToString();
                Form1.GetInstance().toNoticeType = 1;
                Form1.GetInstance().Close();
            } else
            {
                MainMenu.GetInstance().notice($"OOPS, NETCRAFT IS CRASHED...\r\n\r\nAn unhandled exception in Netcraft occured.\r\n\r\nInformation:\r\n[Game version] {MainMenu.GetInstance().Ver}\r\n[Your OS: Platform] {Environment.OSVersion.Platform}\r\n[Your OS: ServicePack] {Environment.OSVersion.ServicePack}\r\n[Your OS: Version] {Environment.OSVersion.VersionString}\r\n" +
                    "\r\nException information\r\n" + e.ExceptionObject.ToString(), 1);
            }
        }

        internal static void threadException(object sender, ThreadExceptionEventArgs e)
        {
            if(Form1.instance != null && Form1.GetInstance().Visible)
            {
                Form1.GetInstance().toNotice = $"OOPS, NETCRAFT IS CRASHED...\r\n\r\nAn unhandled exception in Netcraft occured.\r\n\r\nInformation:\r\n[Game version] {MainMenu.GetInstance().Ver}\r\n[Your OS: Platform] {Environment.OSVersion.Platform}\r\n[Your OS: ServicePack] {Environment.OSVersion.ServicePack}\r\n[Your OS: Version] {Environment.OSVersion.VersionString}\r\n" +
                    "\r\nException information\r\n" + e.Exception.ToString();
                Form1.GetInstance().toNoticeType = 1;
                Form1.GetInstance().Close();
            } else
            {
                MainMenu.GetInstance().notice($"OOPS, NETCRAFT IS CRASHED...\r\n\r\nAn unhandled exception in Netcraft occured.\r\n\r\nInformation:\r\n[Game version] {MainMenu.GetInstance().Ver}\r\n[Your OS: Platform] {Environment.OSVersion.Platform}\r\n[Your OS: ServicePack] {Environment.OSVersion.ServicePack}\r\n[Your OS: Version] {Environment.OSVersion.VersionString}\r\n" +
                    "\r\nException information\r\n" + e.Exception.ToString(), 1);
            }
        }

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            
            return base.OnStartup(eventArgs);
        }

    }
}