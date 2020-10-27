using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Minecraft2D
{
    [DesignerGenerated()]
    public partial class HelpWindow : Form
    {

        // Форма переопределяет dispose для очистки списка компонентов.
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
            _ListBox1 = new ListBox();
            _ListBox1.MouseDoubleClick += new MouseEventHandler(ListBox1_MouseDoubleClick);
            RichTextBox1 = new RichTextBox();
            SuspendLayout();
            // 
            // ListBox1
            // 
            _ListBox1.Dock = DockStyle.Left;
            _ListBox1.Font = new Font("Arial", 9.75f, FontStyle.Bold);
            _ListBox1.FormattingEnabled = true;
            _ListBox1.ItemHeight = 16;
            _ListBox1.Location = new Point(0, 0);
            _ListBox1.Margin = new Padding(4, 4, 4, 4);
            _ListBox1.Name = "_ListBox1";
            _ListBox1.Size = new Size(198, 493);
            _ListBox1.TabIndex = 0;
            // 
            // RichTextBox1
            // 
            RichTextBox1.BorderStyle = BorderStyle.None;
            RichTextBox1.Dock = DockStyle.Fill;
            RichTextBox1.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(204));
            RichTextBox1.Location = new Point(198, 0);
            RichTextBox1.Margin = new Padding(4, 4, 4, 4);
            RichTextBox1.Name = "RichTextBox1";
            RichTextBox1.Size = new Size(609, 493);
            RichTextBox1.TabIndex = 1;
            RichTextBox1.Text = "Please select something from the list";
            // 
            // HelpWindow
            // 
            AutoScaleDimensions = new SizeF(8.0f, 16.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(807, 493);
            Controls.Add(RichTextBox1);
            Controls.Add(_ListBox1);
            Font = new Font("Arial", 9.75f, FontStyle.Bold);
            Margin = new Padding(4, 4, 4, 4);
            Name = "HelpWindow";
            Text = "Обучение";
            Load += new EventHandler(HelpWindow_Load);
            ResumeLayout(false);
        }

        private ListBox _ListBox1;

        internal ListBox ListBox1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ListBox1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ListBox1 != null)
                {
                    _ListBox1.MouseDoubleClick -= ListBox1_MouseDoubleClick;
                }

                _ListBox1 = value;
                if (_ListBox1 != null)
                {
                    _ListBox1.MouseDoubleClick += ListBox1_MouseDoubleClick;
                }
            }
        }

        internal RichTextBox RichTextBox1;
    }
}