using global::System;
using global::System.ComponentModel;
using global::System.Diagnostics;

namespace Minecraft2D.My
{
    internal static partial class MyProject
    {
        internal partial class MyForms
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            public Chat m_Chat;

            public Chat Chat
            {
                [DebuggerHidden]
                get
                {
                    m_Chat = Create__Instance__(m_Chat);
                    return m_Chat;
                }

                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Chat))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Chat);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Form1 m_Form1;

            public Form1 Form1
            {
                [DebuggerHidden]
                get
                {
                    m_Form1 = Create__Instance__(m_Form1);
                    return m_Form1;
                }

                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Form1))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Form1);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Gamesettings m_Gamesettings;

            public Gamesettings Gamesettings
            {
                [DebuggerHidden]
                get
                {
                    m_Gamesettings = Create__Instance__(m_Gamesettings);
                    return m_Gamesettings;
                }

                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Gamesettings))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Gamesettings);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public HelpWindow m_HelpWindow;

            public HelpWindow HelpWindow
            {
                [DebuggerHidden]
                get
                {
                    m_HelpWindow = Create__Instance__(m_HelpWindow);
                    return m_HelpWindow;
                }

                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_HelpWindow))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_HelpWindow);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public LoginForm1 m_LoginForm1;

            public LoginForm1 LoginForm1
            {
                [DebuggerHidden]
                get
                {
                    m_LoginForm1 = Create__Instance__(m_LoginForm1);
                    return m_LoginForm1;
                }

                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_LoginForm1))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_LoginForm1);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public MainMenu m_MainMenu;

            public MainMenu MainMenu
            {
                [DebuggerHidden]
                get
                {
                    m_MainMenu = Create__Instance__(m_MainMenu);
                    return m_MainMenu;
                }

                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_MainMenu))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_MainMenu);
                }
            }
        }
    }
}