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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chat));
            this.RichTextBox1 = new System.Windows.Forms.RichTextBox();
            this._TextBox1 = new System.Windows.Forms.TextBox();
            this._Button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendAPrivateMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kickPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.banPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RichTextBox1
            // 
            this.RichTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichTextBox1.BackColor = System.Drawing.Color.Black;
            this.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RichTextBox1.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RichTextBox1.ForeColor = System.Drawing.Color.White;
            this.RichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.RichTextBox1.Name = "RichTextBox1";
            this.RichTextBox1.ReadOnly = true;
            this.RichTextBox1.Size = new System.Drawing.Size(660, 427);
            this.RichTextBox1.TabIndex = 0;
            this.RichTextBox1.Text = "Шрифт Courier New\nabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\n1234567890" +
    " !@#$%^&*()_+ []{}\"\'|/.,`:;*?<>\n";
            this.RichTextBox1.TextChanged += new System.EventHandler(this.RichTextBox1_TextChanged);
            // 
            // _TextBox1
            // 
            this._TextBox1.BackColor = System.Drawing.Color.Black;
            this._TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._TextBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this._TextBox1.Font = new System.Drawing.Font("Courier New", 12F);
            this._TextBox1.ForeColor = System.Drawing.Color.White;
            this._TextBox1.Location = new System.Drawing.Point(23, 0);
            this._TextBox1.Name = "_TextBox1";
            this._TextBox1.Size = new System.Drawing.Size(523, 19);
            this._TextBox1.TabIndex = 1;
            this._TextBox1.Text = "Hello";
            this._TextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox1_KeyDown);
            // 
            // _Button1
            // 
            this._Button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this._Button1.Dock = System.Windows.Forms.DockStyle.Right;
            this._Button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this._Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Button1.ForeColor = System.Drawing.Color.White;
            this._Button1.Location = new System.Drawing.Point(546, 0);
            this._Button1.Name = "_Button1";
            this._Button1.Size = new System.Drawing.Size(113, 23);
            this._Button1.TabIndex = 2;
            this._Button1.Text = "chat.button.send";
            this._Button1.UseVisualStyleBackColor = false;
            this._Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.listBox1.ForeColor = System.Drawing.Color.White;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(659, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(215, 457);
            this.listBox1.TabIndex = 3;
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendAPrivateMessageToolStripMenuItem,
            this.kickPlayerToolStripMenuItem,
            this.banPlayerToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(206, 70);
            // 
            // sendAPrivateMessageToolStripMenuItem
            // 
            this.sendAPrivateMessageToolStripMenuItem.Name = "sendAPrivateMessageToolStripMenuItem";
            this.sendAPrivateMessageToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.sendAPrivateMessageToolStripMenuItem.Text = "Send a private message";
            this.sendAPrivateMessageToolStripMenuItem.Click += new System.EventHandler(this.sendAPrivateMessageToolStripMenuItem_Click);
            // 
            // kickPlayerToolStripMenuItem
            // 
            this.kickPlayerToolStripMenuItem.Name = "kickPlayerToolStripMenuItem";
            this.kickPlayerToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.kickPlayerToolStripMenuItem.Text = "Kick player";
            this.kickPlayerToolStripMenuItem.Click += new System.EventHandler(this.kickPlayerToolStripMenuItem_Click);
            // 
            // banPlayerToolStripMenuItem
            // 
            this.banPlayerToolStripMenuItem.Name = "banPlayerToolStripMenuItem";
            this.banPlayerToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.banPlayerToolStripMenuItem.Text = "Ban player";
            this.banPlayerToolStripMenuItem.Click += new System.EventHandler(this.banPlayerToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this._TextBox1);
            this.panel1.Controls.Add(this._Button1);
            this.panel1.Location = new System.Drawing.Point(1, 433);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(659, 23);
            this.panel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Courier New", 12F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = ">";
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(874, 457);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.RichTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Chat";
            this.ShowIcon = false;
            this.Text = "chat.title";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Chat_HelpButtonClicked);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Chat_FormClosing);
            this.Load += new System.EventHandler(this.Chat_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

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
        internal ListBox listBox1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem sendAPrivateMessageToolStripMenuItem;
        private ToolStripMenuItem kickPlayerToolStripMenuItem;
        private ToolStripMenuItem banPlayerToolStripMenuItem;
        private Panel panel1;
        private Label label1;

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