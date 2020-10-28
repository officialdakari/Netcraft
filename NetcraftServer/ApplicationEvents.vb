Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ' Для MyApplication имеются следующие события:
    ' Startup: возникает при запуске приложения перед созданием начальной формы.
    ' Shutdown: возникает после закрытия всех форм приложения.  Это событие не создается, если происходит аварийное завершение работы приложения.
    ' UnhandledException: возникает, если в приложении обнаруживается необработанное исключение.
    ' StartupNextInstance: возникает при запуске приложения, допускающего одновременное выполнение только одного экземпляра, если это приложение уже активно. 
    ' NetworkAvailabilityChanged: возникает при изменении состояния подключения — при подключении или отключении.
    Partial Friend Class MyApplication
        Declare Function SetErrorMode Lib "KERNEL32.DLL" (ByVal mode As ErrorModes) As ErrorModes

        Enum ErrorModes As UInteger
            SYSTEM_DEFAULT = &H0
            SEM_FAILCRITICALERRORS = &H1
            SEM_NOALIGNMENTFAULTEXCEPT = &H4
            SEM_NOGPFAULTERRORBOX = &H2
            SEM_NOOPENFILEERRORBOX = &H8000
        End Enum
        Protected Overrides Function OnStartup(eventArgs As StartupEventArgs) As Boolean
            SetErrorMode(SetErrorMode(ErrorModes.SYSTEM_DEFAULT) Or ErrorModes.SEM_NOGPFAULTERRORBOX Or ErrorModes.SEM_FAILCRITICALERRORS Or ErrorModes.SEM_NOOPENFILEERRORBOX)
            Return MyBase.OnStartup(eventArgs)
        End Function
    End Class
End Namespace
