using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Minecraft2D
{

    public class CommandButton : Button
    {
        private bool _commandLink;
        private string _commandLinkNote;

        public CommandButton() : base()
        {
            // Set default property values on the base class to avoid the Obsolete warning
            base.FlatStyle = FlatStyle.System;
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("Specifies this button should use the command link style. " + "(Only applies under Windows Vista and later.)")]
        public bool CommandLink
        {
            get
            {
                return _commandLink;
            }
            set
            {
                if (_commandLink != value)
                {
                    _commandLink = value;
                    this.UpdateCommandLink();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Description("Sets the description text for a command link button. " + "(Only applies under Windows Vista and later.)")]
        public string CommandLinkNote
        {
            get
            {
                return _commandLinkNote;
            }
            set
            {
                if (_commandLinkNote != value)
                {
                    _commandLinkNote = value;
                    this.UpdateCommandLink();
                }
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [Obsolete("This property is not supported on the ButtonEx control.")]
        [DefaultValue(typeof(FlatStyle), "System")]
        public new FlatStyle FlatStyle
        {
            // Set the default flat style to "System", and hide this property because
            // none of the custom properties will work without it set to "System"
            get
            {
                return base.FlatStyle;
            }
            set
            {
                base.FlatStyle = value;
            }
        }

        private const int BS_COMMANDLINK = 0xE;
        private const int BCM_SETNOTE = 0x1609;

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        private void UpdateCommandLink()
        {
            this.RecreateHandle();
            SendMessage(this.Handle, BCM_SETNOTE, IntPtr.Zero, _commandLinkNote);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                if (this.CommandLink)
                    cp.Style = cp.Style | BS_COMMANDLINK;

                return cp;
            }
        }
    }

}
