using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Minecraft2D
{
    [DesignerGenerated()]
    public partial class ConsoleBox : UserControl
    {

        // Пользовательский элемент управления (UserControl) переопределяет метод Dispose для очистки списка компонентов.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Является обязательной для конструктора форм Windows Forms
        private System.ComponentModel.IContainer components;

        // Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
        // Для ее изменения используйте конструктор форм Windows Form.  
        // Не изменяйте ее в редакторе исходного кода.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _TextBox1 = new TextBox();
            _TextBox1.TextChanged += new EventHandler(TextBox1_TextChanged);
            _TextBox1.KeyDown += new KeyEventHandler(TextBox1_KeyDown);
            Timer1 = new Timer(components);
            SuspendLayout();
            // 
            // TextBox1
            // 
            _TextBox1.BackColor = Color.Black;
            _TextBox1.BorderStyle = BorderStyle.None;
            _TextBox1.Cursor = Cursors.Arrow;
            _TextBox1.Dock = DockStyle.Fill;
            _TextBox1.Font = new Font("Video Terminal Screen", 12.0f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
            _TextBox1.ForeColor = Color.White;
            _TextBox1.Location = new Point(0, 0);
            _TextBox1.MaxLength = 24983256;
            _TextBox1.Multiline = true;
            _TextBox1.Name = "_TextBox1";
            _TextBox1.ScrollBars = ScrollBars.Both;
            _TextBox1.ShortcutsEnabled = false;
            _TextBox1.Size = new Size(796, 419);
            _TextBox1.TabIndex = 0;
            _TextBox1.Text = "TEST";
            // 
            // Timer1
            // 
            Timer1.Enabled = true;
            // 
            // ConsoleBox
            // 
            AutoScaleDimensions = new SizeF(7.0f, 14.0f);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(_TextBox1);
            Font = new Font("Consolas", 9.0f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(204));
            Name = "ConsoleBox";
            Size = new Size(796, 419);
            Load += new EventHandler(ConsoleBox_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox _TextBox1;

        private TextBox TextBox1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _TextBox1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_TextBox1 != null)
                {
                    _TextBox1.TextChanged -= TextBox1_TextChanged;
                    _TextBox1.KeyDown -= TextBox1_KeyDown;
                }

                _TextBox1 = value;
                if (_TextBox1 != null)
                {
                    _TextBox1.TextChanged += TextBox1_TextChanged;
                    _TextBox1.KeyDown += TextBox1_KeyDown;
                }
            }
        }

        private Timer Timer1;
    }
}