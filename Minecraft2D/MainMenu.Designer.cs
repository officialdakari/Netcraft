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
            this.Panel1 = new System.Windows.Forms.Panel();
            this.LabelPlayers = new System.Windows.Forms.Label();
            this.LabelDesc = new System.Windows.Forms.Label();
            this.LabelName = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.block = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.block)).BeginInit();
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
            this._Button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button1.ForeColor = System.Drawing.Color.White;
            this._Button1.Location = new System.Drawing.Point(478, 146);
            this._Button1.Name = "_Button1";
            this._Button1.Size = new System.Drawing.Size(310, 36);
            this._Button1.TabIndex = 1;
            this._Button1.TabStop = false;
            this._Button1.Text = "multiplayer";
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
            this.TextBox1.Location = new System.Drawing.Point(500, 188);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(202, 27);
            this.TextBox1.TabIndex = 2;
            this.TextBox1.TabStop = false;
            this.TextBox1.Text = "127.0.0.1";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Font = new System.Drawing.Font("Arial", 11.25F);
            this.Label2.ForeColor = System.Drawing.Color.White;
            this.Label2.Location = new System.Drawing.Point(475, 192);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(25, 17);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "IP:";
            // 
            // _Button3
            // 
            this._Button3.BackColor = System.Drawing.Color.Red;
            this._Button3.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this._Button3.FlatAppearance.BorderSize = 3;
            this._Button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this._Button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this._Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Button3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button3.ForeColor = System.Drawing.Color.White;
            this._Button3.Location = new System.Drawing.Point(722, 3);
            this._Button3.Name = "_Button3";
            this._Button3.Size = new System.Drawing.Size(75, 30);
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
            this.Label3.Location = new System.Drawing.Point(3, 43);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(690, 23);
            this.Label3.TabIndex = 6;
            this.Label3.Text = "❯ Hello world";
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
            this._Button4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button4.ForeColor = System.Drawing.Color.White;
            this._Button4.Location = new System.Drawing.Point(722, 39);
            this._Button4.Name = "_Button4";
            this._Button4.Size = new System.Drawing.Size(75, 30);
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
            this._Button2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button2.ForeColor = System.Drawing.Color.White;
            this._Button2.Location = new System.Drawing.Point(478, 104);
            this._Button2.Name = "_Button2";
            this._Button2.Size = new System.Drawing.Size(310, 36);
            this._Button2.TabIndex = 8;
            this._Button2.TabStop = false;
            this._Button2.Text = "singleplayer";
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
            this.Label5.Font = new System.Drawing.Font("Courier New", 12F);
            this.Label5.ForeColor = System.Drawing.Color.White;
            this.Label5.Location = new System.Drawing.Point(3, 7);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(108, 18);
            this.Label5.TabIndex = 10;
            this.Label5.Text = "nc version";
            // 
            // _Button5
            // 
            this._Button5.BackColor = System.Drawing.Color.White;
            this._Button5.BackgroundImage = global::Minecraft2D.My.Resources.Resources.buttonbg;
            this._Button5.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this._Button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this._Button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this._Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Button5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button5.ForeColor = System.Drawing.Color.White;
            this._Button5.Location = new System.Drawing.Point(708, 188);
            this._Button5.Name = "_Button5";
            this._Button5.Size = new System.Drawing.Size(80, 27);
            this._Button5.TabIndex = 11;
            this._Button5.TabStop = false;
            this._Button5.Text = "Query";
            this._Button5.UseVisualStyleBackColor = false;
            this._Button5.Click += new System.EventHandler(this.Button5_Click);
            this._Button5.MouseEnter += new System.EventHandler(this._Button5_MouseEnter);
            this._Button5.MouseLeave += new System.EventHandler(this._Button5_MouseLeave);
            this._Button5.MouseHover += new System.EventHandler(this._Button5_MouseHover);
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.White;
            this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel1.Controls.Add(this.LabelPlayers);
            this.Panel1.Controls.Add(this.LabelDesc);
            this.Panel1.Controls.Add(this.LabelName);
            this.Panel1.Location = new System.Drawing.Point(117, 104);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(352, 111);
            this.Panel1.TabIndex = 12;
            this.Panel1.Visible = false;
            // 
            // LabelPlayers
            // 
            this.LabelPlayers.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.LabelPlayers.Location = new System.Drawing.Point(21, 86);
            this.LabelPlayers.Name = "LabelPlayers";
            this.LabelPlayers.Size = new System.Drawing.Size(326, 19);
            this.LabelPlayers.TabIndex = 15;
            this.LabelPlayers.Text = "-1/-1";
            this.LabelPlayers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelDesc
            // 
            this.LabelDesc.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.LabelDesc.Location = new System.Drawing.Point(3, 27);
            this.LabelDesc.Name = "LabelDesc";
            this.LabelDesc.Size = new System.Drawing.Size(344, 55);
            this.LabelDesc.TabIndex = 14;
            this.LabelDesc.Text = "Description";
            // 
            // LabelName
            // 
            this.LabelName.AutoSize = true;
            this.LabelName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.LabelName.Location = new System.Drawing.Point(3, 8);
            this.LabelName.Name = "LabelName";
            this.LabelName.Size = new System.Drawing.Size(53, 19);
            this.LabelName.TabIndex = 13;
            this.LabelName.Text = "Name";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.Font = new System.Drawing.Font("Courier New", 12F);
            this.Label6.ForeColor = System.Drawing.Color.White;
            this.Label6.Location = new System.Drawing.Point(3, 25);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(288, 18);
            this.Label6.TabIndex = 13;
            this.Label6.Text = "by DarkCoder15 & TheNonameee";
            this.Label6.UseMnemonic = false;
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
            this.panel2.Location = new System.Drawing.Point(0, 299);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 73);
            this.panel2.TabIndex = 16;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.textBox2);
            this.panel3.Location = new System.Drawing.Point(6, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(443, 93);
            this.panel3.TabIndex = 17;
            this.panel3.Visible = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.panel4.Controls.Add(this.button2);
            this.panel4.Controls.Add(this.button1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 65);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(443, 28);
            this.panel4.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.button2.Dock = System.Windows.Forms.DockStyle.Left;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Courier New", 12F);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(89, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 28);
            this.button2.TabIndex = 1;
            this.button2.TabStop = false;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_2);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.button1.Dock = System.Windows.Forms.DockStyle.Left;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Courier New", 12F);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 28);
            this.button1.TabIndex = 0;
            this.button1.TabStop = false;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.textBox2.Font = new System.Drawing.Font("Courier New", 12F);
            this.textBox2.ForeColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(0, 0);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(443, 65);
            this.textBox2.TabIndex = 0;
            this.textBox2.Text = "abcdefghiABCDEFGHIабвгдеёАБВГДЕЁ0123456789/\\.,[];\'";
            this.textBox2.WordWrap = false;
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.Location = new System.Drawing.Point(478, 5);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(310, 85);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 14;
            this.PictureBox1.TabStop = false;
            this.PictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
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
            this.textBox3.Location = new System.Drawing.Point(6, 276);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(337, 27);
            this.textBox3.TabIndex = 18;
            this.textBox3.TabStop = false;
            this.textBox3.Text = "http://netcraft.ddns.net/netcraft/sprite/1.png";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 256);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "URL скина:";
            // 
            // block
            // 
            this.block.Image = global::Minecraft2D.Properties.Resources.diamond_ore;
            this.block.Location = new System.Drawing.Point(125, 241);
            this.block.Name = "block";
            this.block.Size = new System.Drawing.Size(32, 32);
            this.block.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.block.TabIndex = 20;
            this.block.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.BackgroundImage = global::Minecraft2D.Properties.Resources.netcraft;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(800, 372);
            this.Controls.Add(this.block);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this._Button5);
            this.Controls.Add(this._Button2);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this._Button1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.block)).EndInit();
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

        internal Panel Panel1;
        internal Label LabelPlayers;
        internal Label LabelDesc;
        internal Label LabelName;
        internal Label Label6;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Button button2;
        private Button button1;
        private TextBox textBox2;
        internal PictureBox PictureBox1;
        private NotifyIcon notifyIcon1;
        internal TextBox textBox3;
        internal Label label1;
        private PictureBox block;
        private Timer timer1;
    }
}