using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Minecraft2D
{
    [DesignerGenerated()]
    public partial class Chat : Form
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
            this.RichTextBox1 = new System.Windows.Forms.RichTextBox();
            this._TextBox1 = new System.Windows.Forms.TextBox();
            this._Button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RichTextBox1
            // 
            this.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RichTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.RichTextBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.RichTextBox1.Name = "RichTextBox1";
            this.RichTextBox1.ReadOnly = true;
            this.RichTextBox1.Size = new System.Drawing.Size(561, 342);
            this.RichTextBox1.TabIndex = 0;
            this.RichTextBox1.Text = "";
            this.RichTextBox1.TextChanged += new System.EventHandler(this.RichTextBox1_TextChanged);
            // 
            // _TextBox1
            // 
            this._TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._TextBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._TextBox1.Location = new System.Drawing.Point(0, 348);
            this._TextBox1.Name = "_TextBox1";
            this._TextBox1.Size = new System.Drawing.Size(469, 26);
            this._TextBox1.TabIndex = 1;
            this._TextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox1_KeyDown);
            // 
            // _Button1
            // 
            this._Button1.Location = new System.Drawing.Point(475, 348);
            this._Button1.Name = "_Button1";
            this._Button1.Size = new System.Drawing.Size(86, 26);
            this._Button1.TabIndex = 2;
            this._Button1.Text = "Отправить";
            this._Button1.UseVisualStyleBackColor = true;
            this._Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 377);
            this.Controls.Add(this._Button1);
            this.Controls.Add(this._TextBox1);
            this.Controls.Add(this.RichTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Chat";
            this.Text = "Чат";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Chat_FormClosing);
            this.Load += new System.EventHandler(this.Chat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal RichTextBox RichTextBox1;
        private TextBox _TextBox1;

        internal TextBox TextBox1
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
                    _TextBox1.KeyDown -= TextBox1_KeyDown;
                }

                _TextBox1 = value;
                if (_TextBox1 != null)
                {
                    _TextBox1.KeyDown += TextBox1_KeyDown;
                }
            }
        }

        private Button _Button1;

        internal Button Button1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Button1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Button1 != null)
                {
                    _Button1.Click -= Button1_Click;
                }

                _Button1 = value;
                if (_Button1 != null)
                {
                    _Button1.Click += Button1_Click;
                }
            }
        }
    }
}