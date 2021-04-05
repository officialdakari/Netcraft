using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using System.IO;

namespace Minecraft2D
{
    [DesignerGenerated()]
    public partial class MainMenu : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this._Timer1 = new System.Windows.Forms.Timer(this.components);
            this._Button1 = new System.Windows.Forms.Button();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this._Button3 = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this._Button4 = new System.Windows.Forms.Button();
            this._Button2 = new System.Windows.Forms.Button();
            this.Label5 = new System.Windows.Forms.Label();
            this._Button5 = new System.Windows.Forms.Button();
            this.Label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // _Timer1
            // 
            this._Timer1.Interval = 50;
            this._Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // _Button1
            // 
            this._Button1.BackColor = System.Drawing.Color.White;
            this._Button1.BackgroundImage = global::Minecraft2D.My.Resources.Resources.buttonbg;
            this._Button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this._Button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this._Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this._Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Button1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button1.ForeColor = System.Drawing.Color.White;
            this._Button1.Location = new System.Drawing.Point(302, 197);
            this._Button1.Margin = new System.Windows.Forms.Padding(4);
            this._Button1.Name = "_Button1";
            this._Button1.Size = new System.Drawing.Size(476, 66);
            this._Button1.TabIndex = 1;
            this._Button1.TabStop = false;
            this._Button1.Text = "Multiplayer";
            this._Button1.UseVisualStyleBackColor = false;
            this._Button1.Click += new System.EventHandler(this.Button1_Click);
            this._Button1.MouseEnter += new System.EventHandler(this._Button2_MouseEnter);
            this._Button1.MouseLeave += new System.EventHandler(this._Button2_MouseLeave);
            this._Button1.MouseHover += new System.EventHandler(this._Button2_MouseHover);
            // 
            // TextBox1
            // 
            this.TextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.TextBox1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TextBox1.ForeColor = System.Drawing.Color.White;
            this.TextBox1.Location = new System.Drawing.Point(452, 285);
            this.TextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(211, 27);
            this.TextBox1.TabIndex = 2;
            this.TextBox1.TabStop = false;
            this.TextBox1.Text = "127.0.0.1";
            // 
            // Label2
            // 
            this.Label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Label2.ForeColor = System.Drawing.Color.White;
            this.Label2.Location = new System.Drawing.Point(298, 274);
            this.Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(161, 46);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "Server address:";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // _Button3
            // 
            this._Button3.BackColor = System.Drawing.Color.Red;
            this._Button3.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this._Button3.FlatAppearance.BorderSize = 3;
            this._Button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this._Button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this._Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Button3.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button3.ForeColor = System.Drawing.Color.White;
            this._Button3.Location = new System.Drawing.Point(847, 4);
            this._Button3.Margin = new System.Windows.Forms.Padding(4);
            this._Button3.Name = "_Button3";
            this._Button3.Size = new System.Drawing.Size(175, 37);
            this._Button3.TabIndex = 5;
            this._Button3.TabStop = false;
            this._Button3.Text = "YouTube";
            this._Button3.UseVisualStyleBackColor = false;
            this._Button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // Label3
            // 
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Label3.ForeColor = System.Drawing.Color.Lime;
            this.Label3.Location = new System.Drawing.Point(4, 53);
            this.Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(835, 28);
            this.Label3.TabIndex = 6;
            this.Label3.Text = "[Netcraft:netcraft] Hello, world!";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Label3.Click += new System.EventHandler(this.Label3_Click);
            this.Label3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Label3_MouseClick);
            // 
            // _Button4
            // 
            this._Button4.BackColor = System.Drawing.Color.SteelBlue;
            this._Button4.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this._Button4.FlatAppearance.BorderSize = 3;
            this._Button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this._Button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this._Button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Button4.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button4.ForeColor = System.Drawing.Color.White;
            this._Button4.Location = new System.Drawing.Point(847, 48);
            this._Button4.Margin = new System.Windows.Forms.Padding(4);
            this._Button4.Name = "_Button4";
            this._Button4.Size = new System.Drawing.Size(175, 37);
            this._Button4.TabIndex = 7;
            this._Button4.TabStop = false;
            this._Button4.Text = "Discord";
            this._Button4.UseVisualStyleBackColor = false;
            this._Button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // _Button2
            // 
            this._Button2.BackColor = System.Drawing.Color.White;
            this._Button2.BackgroundImage = global::Minecraft2D.My.Resources.Resources.buttonbg;
            this._Button2.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this._Button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this._Button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this._Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Button2.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button2.ForeColor = System.Drawing.Color.White;
            this._Button2.Location = new System.Drawing.Point(302, 126);
            this._Button2.Margin = new System.Windows.Forms.Padding(4);
            this._Button2.Name = "_Button2";
            this._Button2.Size = new System.Drawing.Size(476, 63);
            this._Button2.TabIndex = 8;
            this._Button2.TabStop = false;
            this._Button2.Text = "Singleplayer";
            this._Button2.UseVisualStyleBackColor = false;
            this._Button2.Click += new System.EventHandler(this.Button2_Click_1);
            this._Button2.MouseEnter += new System.EventHandler(this._Button2_MouseEnter);
            this._Button2.MouseLeave += new System.EventHandler(this._Button2_MouseLeave);
            this._Button2.MouseHover += new System.EventHandler(this._Button2_MouseHover);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.BackColor = System.Drawing.Color.Transparent;
            this.Label5.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Label5.ForeColor = System.Drawing.Color.White;
            this.Label5.Location = new System.Drawing.Point(4, 9);
            this.Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(168, 18);
            this.Label5.TabIndex = 10;
            this.Label5.Text = "Netcraft version";
            this.Label5.Click += new System.EventHandler(this.Label5_Click);
            // 
            // _Button5
            // 
            this._Button5.BackColor = System.Drawing.Color.White;
            this._Button5.BackgroundImage = global::Minecraft2D.My.Resources.Resources.buttonbg;
            this._Button5.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this._Button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this._Button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this._Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Button5.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button5.ForeColor = System.Drawing.Color.White;
            this._Button5.Location = new System.Drawing.Point(671, 282);
            this._Button5.Margin = new System.Windows.Forms.Padding(4);
            this._Button5.Name = "_Button5";
            this._Button5.Size = new System.Drawing.Size(107, 33);
            this._Button5.TabIndex = 11;
            this._Button5.TabStop = false;
            this._Button5.Text = "Query";
            this._Button5.UseVisualStyleBackColor = false;
            this._Button5.Click += new System.EventHandler(this.Button5_Click);
            this._Button5.MouseEnter += new System.EventHandler(this._Button5_MouseEnter);
            this._Button5.MouseLeave += new System.EventHandler(this._Button5_MouseLeave);
            this._Button5.MouseHover += new System.EventHandler(this._Button5_MouseHover);
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Label6.ForeColor = System.Drawing.Color.White;
            this.Label6.Location = new System.Drawing.Point(4, 31);
            this.Label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(308, 18);
            this.Label6.TabIndex = 13;
            this.Label6.Text = "By DarkCoder15 and TheNonameee";
            this.Label6.UseMnemonic = false;
            this.Label6.Click += new System.EventHandler(this.Label6_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.Label3);
            this.panel2.Controls.Add(this._Button3);
            this.panel2.Controls.Add(this._Button4);
            this.panel2.Controls.Add(this.Label6);
            this.panel2.Controls.Add(this.Label5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 458);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1035, 90);
            this.panel2.TabIndex = 16;
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.Location = new System.Drawing.Point(302, 13);
            this.PictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(476, 105);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 14;
            this.PictureBox1.TabStop = false;
            this.PictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox2_Paint);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Нажмите дважды сюда, чтобы закрыть Netcraft.";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick_1);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.textBox3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox3.ForeColor = System.Drawing.Color.White;
            this.textBox3.Location = new System.Drawing.Point(7, 423);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(448, 27);
            this.textBox3.TabIndex = 18;
            this.textBox3.TabStop = false;
            this.textBox3.Text = "http://netcraft.ddns.net/netcraft/sprite/1.png";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 396);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 19;
            this.label1.Text = "URL скина:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.BackgroundImage = global::Minecraft2D.Properties.Resources.netcraft;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1035, 548);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this._Button5);
            this.Controls.Add(this._Button2);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this._Button1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainMenu";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Netcraft C#";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainMenu_FormClosing);
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.LocationChanged += new System.EventHandler(this.MainMenu_LocationChanged);
            this.VisibleChanged += new System.EventHandler(this.MainMenu_VisibleChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox2_Paint);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Timer _Timer1;

        internal Timer Timer1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Timer1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Timer1 != null)
                {
                    _Timer1.Tick -= Timer1_Tick;
                }

                _Timer1 = value;
                if (_Timer1 != null)
                {
                    _Timer1.Tick += Timer1_Tick;
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

        internal TextBox TextBox1;
        internal Label Label2;
        private Button _Button3;

        internal Button Button3
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Button3;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Button3 != null)
                {
                    _Button3.Click -= Button3_Click;
                }

                _Button3 = value;
                if (_Button3 != null)
                {
                    _Button3.Click += Button3_Click;
                }
            }
        }

        internal Label Label3;
        private Button _Button4;

        internal Button Button4
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Button4;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Button4 != null)
                {
                    _Button4.Click -= Button4_Click;
                }

                _Button4 = value;
                if (_Button4 != null)
                {
                    _Button4.Click += Button4_Click;
                }
            }
        }

        private Button _Button2;

        internal Button Button2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Button2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Button2 != null)
                {
                    _Button2.Click -= Button2_Click_1;
                }

                _Button2 = value;
                if (_Button2 != null)
                {
                    _Button2.Click += Button2_Click_1;
                }
            }
        }

        internal Label Label5;
        private Button _Button5;

        internal Button Button5
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Button5;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Button5 != null)
                {
                    _Button5.Click -= Button5_Click;
                }

                _Button5 = value;
                if (_Button5 != null)
                {
                    _Button5.Click += Button5_Click;
                }
            }
        }
        internal Label Label6;
        private Panel panel2;
        internal PictureBox PictureBox1;
        private NotifyIcon notifyIcon1;
        internal TextBox textBox3;
        internal Label label1;
    }
}