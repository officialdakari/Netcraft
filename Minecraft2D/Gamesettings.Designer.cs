using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Minecraft2D
{
    [DesignerGenerated()]
    public partial class Gamesettings : Form
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
            _Button1 = new Button();
            _Button1.Click += new EventHandler(Button1_Click);
            Label1 = new Label();
            GroupBox1 = new GroupBox();
            CheckBox2 = new CheckBox();
            _CheckBox1 = new CheckBox();
            _CheckBox1.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged);
            _Button2 = new Button();
            _Button2.Click += new EventHandler(Button2_Click);
            _Button3 = new Button();
            _Button3.Click += new EventHandler(Button3_Click);
            GroupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // Button1
            // 
            _Button1.BackColor = Color.White;
            _Button1.FlatAppearance.BorderColor = Color.Lime;
            _Button1.FlatAppearance.MouseDownBackColor = Color.Lime;
            _Button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(Conversions.ToInteger(Conversions.ToByte(255)), Conversions.ToInteger(Conversions.ToByte(128)), Conversions.ToInteger(Conversions.ToByte(0)));
            _Button1.FlatStyle = FlatStyle.Flat;
            _Button1.ForeColor = Color.Black;
            _Button1.Location = new Point(478, 92);
            _Button1.Name = "_Button1";
            _Button1.Size = new Size(310, 36);
            _Button1.TabIndex = 0;
            _Button1.Text = "Вернуться в игру";
            _Button1.UseVisualStyleBackColor = false;
            // 
            // Label1
            // 
            Label1.BackColor = Color.Transparent;
            Label1.ForeColor = Color.White;
            Label1.Location = new Point(475, 9);
            Label1.Name = "Label1";
            Label1.Size = new Size(313, 62);
            Label1.TabIndex = 1;
            Label1.Text = "Игра приостановлена";
            Label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // GroupBox1
            // 
            GroupBox1.BackColor = Color.Transparent;
            GroupBox1.Controls.Add(CheckBox2);
            GroupBox1.Controls.Add(_CheckBox1);
            GroupBox1.Location = new Point(12, 62);
            GroupBox1.Name = "GroupBox1";
            GroupBox1.Size = new Size(411, 224);
            GroupBox1.TabIndex = 2;
            GroupBox1.TabStop = false;
            GroupBox1.Visible = false;
            // 
            // CheckBox2
            // 
            CheckBox2.AutoSize = true;
            CheckBox2.BackColor = Color.Transparent;
            CheckBox2.ForeColor = Color.White;
            CheckBox2.Location = new Point(6, 72);
            CheckBox2.Name = "CheckBox2";
            CheckBox2.Size = new Size(176, 22);
            CheckBox2.TabIndex = 1;
            CheckBox2.Text = "Выделение контура";
            CheckBox2.UseVisualStyleBackColor = false;
            // 
            // CheckBox1
            // 
            _CheckBox1.AutoSize = true;
            _CheckBox1.BackColor = Color.Transparent;
            _CheckBox1.ForeColor = Color.White;
            _CheckBox1.Location = new Point(6, 44);
            _CheckBox1.Name = "_CheckBox1";
            _CheckBox1.Size = new Size(340, 22);
            _CheckBox1.TabIndex = 0;
            _CheckBox1.Text = "Сенсорный режим (Кнопки перемещения)";
            _CheckBox1.UseVisualStyleBackColor = false;
            // 
            // Button2
            // 
            _Button2.BackColor = Color.White;
            _Button2.FlatAppearance.BorderColor = Color.Lime;
            _Button2.FlatAppearance.MouseDownBackColor = Color.Lime;
            _Button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(Conversions.ToInteger(Conversions.ToByte(255)), Conversions.ToInteger(Conversions.ToByte(128)), Conversions.ToInteger(Conversions.ToByte(0)));
            _Button2.FlatStyle = FlatStyle.Flat;
            _Button2.ForeColor = Color.Black;
            _Button2.Location = new Point(478, 134);
            _Button2.Name = "_Button2";
            _Button2.Size = new Size(310, 36);
            _Button2.TabIndex = 3;
            _Button2.Text = "Настройки";
            _Button2.UseVisualStyleBackColor = false;
            // 
            // Button3
            // 
            _Button3.BackColor = Color.White;
            _Button3.FlatAppearance.BorderColor = Color.Red;
            _Button3.FlatAppearance.MouseDownBackColor = Color.Lime;
            _Button3.FlatAppearance.MouseOverBackColor = Color.FromArgb(Conversions.ToInteger(Conversions.ToByte(255)), Conversions.ToInteger(Conversions.ToByte(128)), Conversions.ToInteger(Conversions.ToByte(0)));
            _Button3.FlatStyle = FlatStyle.Flat;
            _Button3.ForeColor = Color.Black;
            _Button3.Location = new Point(478, 176);
            _Button3.Name = "_Button3";
            _Button3.Size = new Size(310, 36);
            _Button3.TabIndex = 4;
            _Button3.Text = "Выйти из игры";
            _Button3.UseVisualStyleBackColor = false;
            // 
            // Gamesettings
            // 
            AutoScaleDimensions = new SizeF(9.0f, 18.0f);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Tan;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(800, 372);
            ControlBox = false;
            Controls.Add(_Button3);
            Controls.Add(_Button2);
            Controls.Add(GroupBox1);
            Controls.Add(Label1);
            Controls.Add(_Button1);
            Font = new Font("Arial", 11.25f, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(204));
            ForeColor = Color.Black;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Gamesettings";
            StartPosition = FormStartPosition.CenterParent;
            TransparencyKey = Color.Tan;
            GroupBox1.ResumeLayout(false);
            GroupBox1.PerformLayout();
            VisibleChanged += new EventHandler(Gamesettings_VisibleChanged);
            FormClosing += new FormClosingEventHandler(Gamesettings_FormClosing);
            Load += new EventHandler(Gamesettings_Load);
            ResumeLayout(false);
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

        internal Label Label1;
        internal GroupBox GroupBox1;
        private CheckBox _CheckBox1;

        internal CheckBox CheckBox1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _CheckBox1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_CheckBox1 != null)
                {
                    _CheckBox1.CheckedChanged -= CheckBox1_CheckedChanged;
                }

                _CheckBox1 = value;
                if (_CheckBox1 != null)
                {
                    _CheckBox1.CheckedChanged += CheckBox1_CheckedChanged;
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
                    _Button2.Click -= Button2_Click;
                }

                _Button2 = value;
                if (_Button2 != null)
                {
                    _Button2.Click += Button2_Click;
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
                    _Button3.Click -= Button3_Click;
                }

                _Button3 = value;
                if (_Button3 != null)
                {
                    _Button3.Click += Button3_Click;
                }
            }
        }

        internal CheckBox CheckBox2;
    }
}