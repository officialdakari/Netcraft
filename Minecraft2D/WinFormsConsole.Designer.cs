using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Minecraft2D
{
    partial class WinFormsConsole
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
            this.components = new System.ComponentModel.Container();
            this._TextBox1 = new System.Windows.Forms.TextBox();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // _TextBox1
            // 
            this._TextBox1.BackColor = System.Drawing.Color.Black;
            this._TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._TextBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this._TextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._TextBox1.Font = new System.Drawing.Font("Terminal", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._TextBox1.ForeColor = System.Drawing.Color.White;
            this._TextBox1.Location = new System.Drawing.Point(0, 0);
            this._TextBox1.MaxLength = 24983256;
            this._TextBox1.Multiline = true;
            this._TextBox1.Name = "_TextBox1";
            this._TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._TextBox1.ShortcutsEnabled = false;
            this._TextBox1.Size = new System.Drawing.Size(796, 419);
            this._TextBox1.TabIndex = 0;
            this._TextBox1.Text = "TEST";
            this._TextBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            this._TextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox1_KeyDown);
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            // 
            // WinFormsConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._TextBox1);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "WinFormsConsole";
            this.Size = new System.Drawing.Size(796, 419);
            this.Load += new System.EventHandler(this.ConsoleBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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
