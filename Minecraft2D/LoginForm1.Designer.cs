using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Minecraft2D
{
    [DesignerGenerated()]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726")]
    public partial class LoginForm1 : Form
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

        internal PictureBox LogoPictureBox;
        internal Label UsernameLabel;
        internal TextBox UsernameTextBox;
        private Button _OK;

        internal Button OK
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _OK;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_OK != null)
                {
                    _OK.Click -= OK_Click;
                }

                _OK = value;
                if (_OK != null)
                {
                    _OK.Click += OK_Click;
                }
            }
        }

        private Button _Cancel;

        internal Button Cancel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Cancel;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Cancel != null)
                {
                    _Cancel.Click -= Cancel_Click;
                }

                _Cancel = value;
                if (_Cancel != null)
                {
                    _Cancel.Click += Cancel_Click;
                }
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm1));
            LogoPictureBox = new PictureBox();
            UsernameLabel = new Label();
            UsernameTextBox = new TextBox();
            _OK = new Button();
            _OK.Click += new EventHandler(OK_Click);
            _Cancel = new Button();
            _Cancel.Click += new EventHandler(Cancel_Click);
            ((System.ComponentModel.ISupportInitialize)LogoPictureBox).BeginInit();
            SuspendLayout();
            // 
            // LogoPictureBox
            // 
            LogoPictureBox.Image = (Image)resources.GetObject("LogoPictureBox.Image");
            LogoPictureBox.Location = new Point(0, 0);
            LogoPictureBox.Name = "LogoPictureBox";
            LogoPictureBox.Size = new Size(165, 193);
            LogoPictureBox.TabIndex = 0;
            LogoPictureBox.TabStop = false;
            // 
            // UsernameLabel
            // 
            UsernameLabel.Location = new Point(172, 54);
            UsernameLabel.Name = "UsernameLabel";
            UsernameLabel.Size = new Size(220, 23);
            UsernameLabel.TabIndex = 0;
            UsernameLabel.Text = "Имя игрока";
            UsernameLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // UsernameTextBox
            // 
            UsernameTextBox.Location = new Point(174, 74);
            UsernameTextBox.Name = "UsernameTextBox";
            UsernameTextBox.Size = new Size(220, 22);
            UsernameTextBox.TabIndex = 1;
            // 
            // OK
            // 
            _OK.Location = new Point(197, 161);
            _OK.Name = "_OK";
            _OK.Size = new Size(94, 23);
            _OK.TabIndex = 4;
            _OK.Text = "&ОК";
            // 
            // Cancel
            // 
            _Cancel.DialogResult = DialogResult.Cancel;
            _Cancel.Location = new Point(300, 161);
            _Cancel.Name = "_Cancel";
            _Cancel.Size = new Size(94, 23);
            _Cancel.TabIndex = 5;
            _Cancel.Text = "&Отмена";
            // 
            // LoginForm1
            // 
            AcceptButton = _OK;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = _Cancel;
            ClientSize = new Size(401, 192);
            Controls.Add(_Cancel);
            Controls.Add(_OK);
            Controls.Add(UsernameTextBox);
            Controls.Add(UsernameLabel);
            Controls.Add(LogoPictureBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm1";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Netcraft";
            ((System.ComponentModel.ISupportInitialize)LogoPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}