using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

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
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // _Timer1
            // 
            this._Timer1.Enabled = true;
            this._Timer1.Interval = 50;
            this._Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // _Button1
            // 
            this._Button1.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this._Button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this._Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this._Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button1.Location = new System.Drawing.Point(478, 159);
            this._Button1.Name = "_Button1";
            this._Button1.Size = new System.Drawing.Size(310, 36);
            this._Button1.TabIndex = 1;
            this._Button1.Text = "Сетевая игра";
            this._Button1.UseVisualStyleBackColor = true;
            this._Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // TextBox1
            // 
            this.TextBox1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TextBox1.Location = new System.Drawing.Point(500, 201);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(202, 27);
            this.TextBox1.TabIndex = 2;
            this.TextBox1.Text = "127.0.0.1";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.ForeColor = System.Drawing.Color.White;
            this.Label2.Location = new System.Drawing.Point(478, 208);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(19, 13);
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
            this._Button3.Location = new System.Drawing.Point(713, 297);
            this._Button3.Name = "_Button3";
            this._Button3.Size = new System.Drawing.Size(75, 30);
            this._Button3.TabIndex = 5;
            this._Button3.Text = "YouTube";
            this._Button3.UseVisualStyleBackColor = false;
            this._Button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Label3.ForeColor = System.Drawing.Color.White;
            this.Label3.Location = new System.Drawing.Point(12, 346);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(17, 17);
            this.Label3.TabIndex = 6;
            this.Label3.Text = ">";
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
            this._Button4.Location = new System.Drawing.Point(713, 333);
            this._Button4.Name = "_Button4";
            this._Button4.Size = new System.Drawing.Size(75, 30);
            this._Button4.TabIndex = 7;
            this._Button4.Text = "Discord";
            this._Button4.UseVisualStyleBackColor = false;
            this._Button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // _Button2
            // 
            this._Button2.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this._Button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this._Button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this._Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Button2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button2.Location = new System.Drawing.Point(478, 112);
            this._Button2.Name = "_Button2";
            this._Button2.Size = new System.Drawing.Size(310, 36);
            this._Button2.TabIndex = 8;
            this._Button2.Text = "Официальный сервер";
            this._Button2.UseVisualStyleBackColor = true;
            this._Button2.Click += new System.EventHandler(this.Button2_Click_1);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.BackColor = System.Drawing.Color.Transparent;
            this.Label5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Label5.ForeColor = System.Drawing.Color.White;
            this.Label5.Location = new System.Drawing.Point(12, 9);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(225, 18);
            this.Label5.TabIndex = 10;
            this.Label5.Text = "NetCraft 1.1-ALPHA-U10102020";
            // 
            // _Button5
            // 
            this._Button5.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this._Button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this._Button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkOrange;
            this._Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Button5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Button5.Location = new System.Drawing.Point(708, 201);
            this._Button5.Name = "_Button5";
            this._Button5.Size = new System.Drawing.Size(80, 27);
            this._Button5.TabIndex = 11;
            this._Button5.Text = "Ping";
            this._Button5.UseVisualStyleBackColor = true;
            this._Button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.White;
            this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel1.Controls.Add(this.LabelPlayers);
            this.Panel1.Controls.Add(this.LabelDesc);
            this.Panel1.Controls.Add(this.LabelName);
            this.Panel1.Location = new System.Drawing.Point(120, 112);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(352, 111);
            this.Panel1.TabIndex = 12;
            this.Panel1.Visible = false;
            // 
            // LabelPlayers
            // 
            this.LabelPlayers.AutoSize = true;
            this.LabelPlayers.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.LabelPlayers.Location = new System.Drawing.Point(306, 85);
            this.LabelPlayers.Name = "LabelPlayers";
            this.LabelPlayers.Size = new System.Drawing.Size(41, 19);
            this.LabelPlayers.TabIndex = 15;
            this.LabelPlayers.Text = "-1/-1";
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
            this.Label6.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Label6.ForeColor = System.Drawing.Color.White;
            this.Label6.Location = new System.Drawing.Point(12, 27);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(151, 18);
            this.Label6.TabIndex = 13;
            this.Label6.Text = "by GladCypress3030";
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.Location = new System.Drawing.Point(478, 9);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(310, 59);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 14;
            this.PictureBox1.TabStop = false;
            // 
            // PictureBox2
            // 
            this.PictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox2.Image = global::Minecraft2D.My.Resources.Resources.c_edition;
            this.PictureBox2.Location = new System.Drawing.Point(478, 65);
            this.PictureBox2.Name = "PictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(310, 18);
            this.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox2.TabIndex = 15;
            this.PictureBox2.TabStop = false;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(800, 372);
            this.Controls.Add(this.PictureBox2);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this._Button5);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this._Button2);
            this.Controls.Add(this._Button4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this._Button3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this._Button1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetCraft";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.LocationChanged += new System.EventHandler(this.MainMenu_LocationChanged);
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
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
        internal PictureBox PictureBox1;
        internal PictureBox PictureBox2;
    }
}