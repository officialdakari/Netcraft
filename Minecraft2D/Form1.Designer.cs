using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Minecraft2D
{
    [DesignerGenerated()]
    public partial class Form1 : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this._Ticker = new System.Windows.Forms.Timer(this.components);
            this._Button1 = new System.Windows.Forms.Button();
            this._ListBox1 = new System.Windows.Forms.ListBox();
            this._Timer2 = new System.Windows.Forms.Timer(this.components);
            this._Button2 = new System.Windows.Forms.Button();
            this._Timer3 = new System.Windows.Forms.Timer(this.components);
            this._ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._localPlayer = new Minecraft2D.TransparentPicBox();
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._Warning = new System.Windows.Forms.Label();
            this._ButtonLeft = new System.Windows.Forms.Button();
            this._ButtonJump = new System.Windows.Forms.Button();
            this._ButtonRight = new System.Windows.Forms.Button();
            this._Button3 = new System.Windows.Forms.Button();
            this._ButtonAttack = new System.Windows.Forms.Button();
            this._Button4 = new System.Windows.Forms.Button();
            this._ListBox2 = new System.Windows.Forms.ListBox();
            this.R1 = new Minecraft2D.TransparentPicBox();
            ((System.ComponentModel.ISupportInitialize)(this._localPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.R1)).BeginInit();
            this.SuspendLayout();
            // 
            // Timer1
            // 
            this.Timer1.Interval = 30;
            // 
            // _Ticker
            // 
            this._Ticker.Interval = 10;
            this._Ticker.Tick += new System.EventHandler(this.Ticker_Tick);
            // 
            // _Button1
            // 
            this._Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._Button1.BackColor = System.Drawing.SystemColors.Control;
            this._Button1.Location = new System.Drawing.Point(1008, 0);
            this._Button1.Name = "_Button1";
            this._Button1.Size = new System.Drawing.Size(75, 23);
            this._Button1.TabIndex = 2;
            this._Button1.Text = "Инвентарь";
            this._Button1.UseVisualStyleBackColor = false;
            this._Button1.Click += new System.EventHandler(this.Button1_Click);
            this._Button1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._Button1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            // 
            // _ListBox1
            // 
            this._ListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._ListBox1.FormattingEnabled = true;
            this._ListBox1.Location = new System.Drawing.Point(610, 0);
            this._ListBox1.Name = "_ListBox1";
            this._ListBox1.Size = new System.Drawing.Size(392, 264);
            this._ListBox1.TabIndex = 3;
            this._ListBox1.Visible = false;
            this._ListBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._ListBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._ListBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBox1_MouseDoubleClick);
            this._ListBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBox1_MouseDown);
            // 
            // _Timer2
            // 
            this._Timer2.Enabled = true;
            this._Timer2.Interval = 10;
            this._Timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // _Button2
            // 
            this._Button2.BackColor = System.Drawing.SystemColors.Control;
            this._Button2.Location = new System.Drawing.Point(3, 0);
            this._Button2.Name = "_Button2";
            this._Button2.Size = new System.Drawing.Size(75, 23);
            this._Button2.TabIndex = 4;
            this._Button2.Text = "Чат";
            this._Button2.UseVisualStyleBackColor = false;
            this._Button2.Click += new System.EventHandler(this.Button2_Click);
            this._Button2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._Button2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            // 
            // _Timer3
            // 
            this._Timer3.Enabled = true;
            this._Timer3.Tick += new System.EventHandler(this.Timer3_Tick);
            // 
            // _ProgressBar1
            // 
            this._ProgressBar1.BackColor = System.Drawing.SystemColors.Control;
            this._ProgressBar1.ForeColor = System.Drawing.Color.Red;
            this._ProgressBar1.Location = new System.Drawing.Point(84, 0);
            this._ProgressBar1.Name = "_ProgressBar1";
            this._ProgressBar1.Size = new System.Drawing.Size(149, 23);
            this._ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._ProgressBar1.TabIndex = 5;
            this._ProgressBar1.Value = 100;
            this._ProgressBar1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._ProgressBar1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            // 
            // ToolTip1
            // 
            this.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ToolTip1.ToolTipTitle = "Информация";
            // 
            // _localPlayer
            // 
            this._localPlayer.BackColor = System.Drawing.Color.Transparent;
            this._localPlayer.Image = ((System.Drawing.Image)(resources.GetObject("_localPlayer.Image")));
            this._localPlayer.Location = new System.Drawing.Point(0, 0);
            this._localPlayer.Name = "_localPlayer";
            this._localPlayer.Size = new System.Drawing.Size(47, 92);
            this._localPlayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._localPlayer.TabIndex = 14;
            this._localPlayer.TabStop = false;
            this.ToolTip1.SetToolTip(this._localPlayer, "Локальный игрок");
            this._localPlayer.Move += new System.EventHandler(this._localPlayer_Move);
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.Name = "ContextMenuStrip1";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // _Warning
            // 
            this._Warning.AutoSize = true;
            this._Warning.BackColor = System.Drawing.Color.Transparent;
            this._Warning.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Warning.ForeColor = System.Drawing.Color.Red;
            this._Warning.Location = new System.Drawing.Point(0, 51);
            this._Warning.Name = "_Warning";
            this._Warning.Size = new System.Drawing.Size(17, 18);
            this._Warning.TabIndex = 8;
            this._Warning.Text = ">";
            this._Warning.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._Warning.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            // 
            // _ButtonLeft
            // 
            this._ButtonLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ButtonLeft.BackColor = System.Drawing.SystemColors.Control;
            this._ButtonLeft.Font = new System.Drawing.Font("Wingdings", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this._ButtonLeft.Location = new System.Drawing.Point(8, 507);
            this._ButtonLeft.Name = "_ButtonLeft";
            this._ButtonLeft.Size = new System.Drawing.Size(70, 61);
            this._ButtonLeft.TabIndex = 9;
            this._ButtonLeft.Text = "ß";
            this._ButtonLeft.UseCompatibleTextRendering = true;
            this._ButtonLeft.UseVisualStyleBackColor = false;
            this._ButtonLeft.Visible = false;
            this._ButtonLeft.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._ButtonLeft.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._ButtonLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button3_MouseDown);
            this._ButtonLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button3_MouseUp);
            // 
            // _ButtonJump
            // 
            this._ButtonJump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ButtonJump.BackColor = System.Drawing.SystemColors.Control;
            this._ButtonJump.Font = new System.Drawing.Font("Wingdings", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this._ButtonJump.Location = new System.Drawing.Point(1008, 507);
            this._ButtonJump.Name = "_ButtonJump";
            this._ButtonJump.Size = new System.Drawing.Size(70, 61);
            this._ButtonJump.TabIndex = 10;
            this._ButtonJump.Text = "ñ";
            this._ButtonJump.UseCompatibleTextRendering = true;
            this._ButtonJump.UseVisualStyleBackColor = false;
            this._ButtonJump.Visible = false;
            this._ButtonJump.Click += new System.EventHandler(this.Button5_Click);
            this._ButtonJump.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._ButtonJump.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._ButtonJump.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonJump_MouseDown);
            // 
            // _ButtonRight
            // 
            this._ButtonRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ButtonRight.BackColor = System.Drawing.SystemColors.Control;
            this._ButtonRight.Font = new System.Drawing.Font("Wingdings", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this._ButtonRight.Location = new System.Drawing.Point(84, 507);
            this._ButtonRight.Name = "_ButtonRight";
            this._ButtonRight.Size = new System.Drawing.Size(70, 61);
            this._ButtonRight.TabIndex = 11;
            this._ButtonRight.Text = "à";
            this._ButtonRight.UseCompatibleTextRendering = true;
            this._ButtonRight.UseVisualStyleBackColor = false;
            this._ButtonRight.Visible = false;
            this._ButtonRight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._ButtonRight.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._ButtonRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button6_MouseDown);
            this._ButtonRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button6_MouseUp);
            // 
            // _Button3
            // 
            this._Button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._Button3.BackColor = System.Drawing.SystemColors.Control;
            this._Button3.Location = new System.Drawing.Point(1008, 29);
            this._Button3.Name = "_Button3";
            this._Button3.Size = new System.Drawing.Size(75, 23);
            this._Button3.TabIndex = 12;
            this._Button3.Text = "Пауза";
            this._Button3.UseVisualStyleBackColor = false;
            this._Button3.Click += new System.EventHandler(this.Button3_Click_1);
            this._Button3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._Button3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            // 
            // _ButtonAttack
            // 
            this._ButtonAttack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ButtonAttack.BackColor = System.Drawing.SystemColors.Control;
            this._ButtonAttack.Enabled = false;
            this._ButtonAttack.Font = new System.Drawing.Font("Webdings", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this._ButtonAttack.ForeColor = System.Drawing.Color.Red;
            this._ButtonAttack.Location = new System.Drawing.Point(932, 507);
            this._ButtonAttack.Name = "_ButtonAttack";
            this._ButtonAttack.Size = new System.Drawing.Size(70, 61);
            this._ButtonAttack.TabIndex = 13;
            this._ButtonAttack.Text = "r";
            this._ButtonAttack.UseVisualStyleBackColor = false;
            this._ButtonAttack.Visible = false;
            this._ButtonAttack.Click += new System.EventHandler(this.ButtonAttack_Click);
            this._ButtonAttack.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._ButtonAttack.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            // 
            // _Button4
            // 
            this._Button4.BackColor = System.Drawing.SystemColors.Control;
            this._Button4.Location = new System.Drawing.Point(3, 25);
            this._Button4.Name = "_Button4";
            this._Button4.Size = new System.Drawing.Size(75, 23);
            this._Button4.TabIndex = 16;
            this._Button4.Text = "Крафт";
            this._Button4.UseVisualStyleBackColor = false;
            this._Button4.Click += new System.EventHandler(this.Button4_Click_1);
            this._Button4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._Button4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            // 
            // _ListBox2
            // 
            this._ListBox2.FormattingEnabled = true;
            this._ListBox2.Location = new System.Drawing.Point(84, 25);
            this._ListBox2.Name = "_ListBox2";
            this._ListBox2.Size = new System.Drawing.Size(314, 160);
            this._ListBox2.TabIndex = 17;
            this._ListBox2.Visible = false;
            this._ListBox2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBox2_MouseDoubleClick);
            // 
            // R1
            // 
            this.R1.BackColor = System.Drawing.Color.Transparent;
            this.R1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.R1.Location = new System.Drawing.Point(54, 146);
            this.R1.Name = "R1";
            this.R1.Size = new System.Drawing.Size(48, 48);
            this.R1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.R1.TabIndex = 15;
            this.R1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(1086, 580);
            this.Controls.Add(this._ListBox2);
            this.Controls.Add(this._Button4);
            this.Controls.Add(this.R1);
            this.Controls.Add(this._localPlayer);
            this.Controls.Add(this._ButtonAttack);
            this.Controls.Add(this._Button3);
            this.Controls.Add(this._ButtonRight);
            this.Controls.Add(this._ButtonJump);
            this.Controls.Add(this._ButtonLeft);
            this.Controls.Add(this._Warning);
            this.Controls.Add(this._ProgressBar1);
            this.Controls.Add(this._Button2);
            this.Controls.Add(this._ListBox1);
            this.Controls.Add(this._Button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(9999, 619);
            this.Name = "Form1";
            this.Text = "Netcraft";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.Move += new System.EventHandler(this.Form1_Move);
            ((System.ComponentModel.ISupportInitialize)(this._localPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.R1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal Timer Timer1;
        private Timer _Ticker;

        internal Timer Ticker
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Ticker;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Ticker != null)
                {
                    _Ticker.Tick -= Ticker_Tick;
                }

                _Ticker = value;
                if (_Ticker != null)
                {
                    _Ticker.Tick += Ticker_Tick;
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
                    _Button1.KeyDown -= Form1_KeyDown;
                    _Button1.KeyUp -= Form1_KeyUp;
                    _Button1.Click -= Button1_Click;
                }

                _Button1 = value;
                if (_Button1 != null)
                {
                    _Button1.KeyDown += Form1_KeyDown;
                    _Button1.KeyUp += Form1_KeyUp;
                    _Button1.Click += Button1_Click;
                }
            }
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
                    _ListBox1.KeyDown -= Form1_KeyDown;
                    _ListBox1.KeyUp -= Form1_KeyUp;
                    _ListBox1.MouseDown -= ListBox1_MouseDown;
                    _ListBox1.MouseDoubleClick -= ListBox1_MouseDoubleClick;
                }

                _ListBox1 = value;
                if (_ListBox1 != null)
                {
                    _ListBox1.KeyDown += Form1_KeyDown;
                    _ListBox1.KeyUp += Form1_KeyUp;
                    _ListBox1.MouseDown += ListBox1_MouseDown;
                    _ListBox1.MouseDoubleClick += ListBox1_MouseDoubleClick;
                }
            }
        }

        private Timer _Timer2;

        internal Timer Timer2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Timer2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Timer2 != null)
                {
                    _Timer2.Tick -= Timer2_Tick;
                }

                _Timer2 = value;
                if (_Timer2 != null)
                {
                    _Timer2.Tick += Timer2_Tick;
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
                    _Button2.KeyDown -= Form1_KeyDown;
                    _Button2.KeyUp -= Form1_KeyUp;
                    _Button2.Click -= Button2_Click;
                }

                _Button2 = value;
                if (_Button2 != null)
                {
                    _Button2.KeyDown += Form1_KeyDown;
                    _Button2.KeyUp += Form1_KeyUp;
                    _Button2.Click += Button2_Click;
                }
            }
        }

        private Timer _Timer3;

        internal Timer Timer3
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Timer3;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Timer3 != null)
                {
                    _Timer3.Tick -= Timer3_Tick;
                }

                _Timer3 = value;
                if (_Timer3 != null)
                {
                    _Timer3.Tick += Timer3_Tick;
                }
            }
        }

        private ProgressBar _ProgressBar1;

        internal ProgressBar ProgressBar1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ProgressBar1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ProgressBar1 != null)
                {
                    _ProgressBar1.KeyDown -= Form1_KeyDown;
                    _ProgressBar1.KeyUp -= Form1_KeyUp;
                }

                _ProgressBar1 = value;
                if (_ProgressBar1 != null)
                {
                    _ProgressBar1.KeyDown += Form1_KeyDown;
                    _ProgressBar1.KeyUp += Form1_KeyUp;
                }
            }
        }

        internal ToolTip ToolTip1;
        internal ContextMenuStrip ContextMenuStrip1;
        private Label _Warning;

        internal Label Warning
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Warning;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Warning != null)
                {
                    _Warning.KeyDown -= Form1_KeyDown;
                    _Warning.KeyUp -= Form1_KeyUp;
                }

                _Warning = value;
                if (_Warning != null)
                {
                    _Warning.KeyDown += Form1_KeyDown;
                    _Warning.KeyUp += Form1_KeyUp;
                }
            }
        }

        private Button _ButtonLeft;

        internal Button ButtonLeft
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ButtonLeft;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ButtonLeft != null)
                {
                    _ButtonLeft.KeyDown -= Form1_KeyDown;
                    _ButtonLeft.KeyUp -= Form1_KeyUp;
                    _ButtonLeft.MouseDown -= Button3_MouseDown;
                    _ButtonLeft.MouseUp -= Button3_MouseUp;
                }

                _ButtonLeft = value;
                if (_ButtonLeft != null)
                {
                    _ButtonLeft.KeyDown += Form1_KeyDown;
                    _ButtonLeft.KeyUp += Form1_KeyUp;
                    _ButtonLeft.MouseDown += Button3_MouseDown;
                    _ButtonLeft.MouseUp += Button3_MouseUp;
                }
            }
        }

        private Button _ButtonJump;

        internal Button ButtonJump
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ButtonJump;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ButtonJump != null)
                {
                    _ButtonJump.KeyDown -= Form1_KeyDown;
                    _ButtonJump.KeyUp -= Form1_KeyUp;
                    _ButtonJump.Click -= Button5_Click;
                    _ButtonJump.MouseDown -= ButtonJump_MouseDown;
                }

                _ButtonJump = value;
                if (_ButtonJump != null)
                {
                    _ButtonJump.KeyDown += Form1_KeyDown;
                    _ButtonJump.KeyUp += Form1_KeyUp;
                    _ButtonJump.Click += Button5_Click;
                    _ButtonJump.MouseDown += ButtonJump_MouseDown;
                }
            }
        }

        private Button _ButtonRight;

        internal Button ButtonRight
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ButtonRight;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ButtonRight != null)
                {
                    _ButtonRight.KeyDown -= Form1_KeyDown;
                    _ButtonRight.KeyUp -= Form1_KeyUp;
                    _ButtonRight.MouseDown -= Button6_MouseDown;
                    _ButtonRight.MouseUp -= Button6_MouseUp;
                }

                _ButtonRight = value;
                if (_ButtonRight != null)
                {
                    _ButtonRight.KeyDown += Form1_KeyDown;
                    _ButtonRight.KeyUp += Form1_KeyUp;
                    _ButtonRight.MouseDown += Button6_MouseDown;
                    _ButtonRight.MouseUp += Button6_MouseUp;
                }
            }
        }

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
                    _Button3.KeyDown -= Form1_KeyDown;
                    _Button3.KeyUp -= Form1_KeyUp;
                    _Button3.Click -= Button3_Click_1;
                }

                _Button3 = value;
                if (_Button3 != null)
                {
                    _Button3.KeyDown += Form1_KeyDown;
                    _Button3.KeyUp += Form1_KeyUp;
                    _Button3.Click += Button3_Click_1;
                }
            }
        }

        private Button _ButtonAttack;

        internal Button ButtonAttack
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ButtonAttack;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ButtonAttack != null)
                {
                    _ButtonAttack.KeyDown -= Form1_KeyDown;
                    _ButtonAttack.KeyUp -= Form1_KeyUp;
                    _ButtonAttack.Click -= ButtonAttack_Click;
                }

                _ButtonAttack = value;
                if (_ButtonAttack != null)
                {
                    _ButtonAttack.KeyDown += Form1_KeyDown;
                    _ButtonAttack.KeyUp += Form1_KeyUp;
                    _ButtonAttack.Click += ButtonAttack_Click;
                }
            }
        }

        private TransparentPicBox _localPlayer;

        internal TransparentPicBox localPlayer
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _localPlayer;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_localPlayer != null)
                {
                    _localPlayer.LocationChanged -= (_, __) => Test();
                    //_localPlayer.LocationChanged -= Test;
                }

                _localPlayer = value;
                if (_localPlayer != null)
                {
                    _localPlayer.LocationChanged += (_, __) => Test();
                    //_localPlayer.LocationChanged += Test;
                }
            }
        }

        internal TransparentPicBox R1;
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
                    _Button4.KeyDown -= Form1_KeyDown;
                    _Button4.KeyUp -= Form1_KeyUp;
                    _Button4.Click -= Button4_Click_1;
                }

                _Button4 = value;
                if (_Button4 != null)
                {
                    _Button4.KeyDown += Form1_KeyDown;
                    _Button4.KeyUp += Form1_KeyUp;
                    _Button4.Click += Button4_Click_1;
                }
            }
        }

        private ListBox _ListBox2;

        internal ListBox ListBox2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ListBox2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ListBox2 != null)
                {
                    _ListBox2.MouseDoubleClick -= ListBox2_MouseDoubleClick;
                }

                _ListBox2 = value;
                if (_ListBox2 != null)
                {
                    _ListBox2.MouseDoubleClick += ListBox2_MouseDoubleClick;
                }
            }
        }
    }
}