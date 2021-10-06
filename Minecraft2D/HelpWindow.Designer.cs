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
            this._ListBox1 = new System.Windows.Forms.ListBox();
            this.RichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // _ListBox1
            // 
            this._ListBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this._ListBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this._ListBox1.FormattingEnabled = true;
            this._ListBox1.ItemHeight = 16;
            this._ListBox1.Location = new System.Drawing.Point(0, 0);
            this._ListBox1.Margin = new System.Windows.Forms.Padding(4);
            this._ListBox1.Name = "_ListBox1";
            this._ListBox1.Size = new System.Drawing.Size(198, 493);
            this._ListBox1.TabIndex = 0;
            this._ListBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBox1_MouseDoubleClick);
            // 
            // RichTextBox1
            // 
            this.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RichTextBox1.Location = new System.Drawing.Point(198, 0);
            this.RichTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.RichTextBox1.Name = "RichTextBox1";
            this.RichTextBox1.ReadOnly = true;
            this.RichTextBox1.Size = new System.Drawing.Size(609, 493);
            this.RichTextBox1.TabIndex = 1;
            this.RichTextBox1.Text = "Please select something from the list";
            // 
            // HelpWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 493);
            this.Controls.Add(this.RichTextBox1);
            this.Controls.Add(this._ListBox1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "HelpWindow";
            this.Text = "Обучение";
            this.Load += new System.EventHandler(this.HelpWindow_Load);
            this.ResumeLayout(false);

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