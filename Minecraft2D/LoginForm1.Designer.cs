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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm1));
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this._OK = new System.Windows.Forms.Button();
            this._Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.BackColor = System.Drawing.Color.Transparent;
            this.UsernameLabel.ForeColor = System.Drawing.Color.White;
            this.UsernameLabel.Location = new System.Drawing.Point(12, 15);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(220, 23);
            this.UsernameLabel.TabIndex = 0;
            this.UsernameLabel.Text = "text.playername";
            this.UsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.UsernameTextBox.ForeColor = System.Drawing.Color.White;
            this.UsernameTextBox.Location = new System.Drawing.Point(12, 41);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(392, 20);
            this.UsernameTextBox.TabIndex = 1;
            this.UsernameTextBox.Text = "Example";
            // 
            // _OK
            // 
            this._OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._OK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("_OK.BackgroundImage")));
            this._OK.FlatAppearance.BorderSize = 0;
            this._OK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this._OK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this._OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._OK.ForeColor = System.Drawing.Color.White;
            this._OK.Location = new System.Drawing.Point(207, 70);
            this._OK.Name = "_OK";
            this._OK.Size = new System.Drawing.Size(94, 23);
            this._OK.TabIndex = 4;
            this._OK.Text = "ОК";
            this._OK.UseMnemonic = false;
            this._OK.Click += new System.EventHandler(this.OK_Click);
            this._OK.MouseEnter += new System.EventHandler(this._Cancel_MouseEnter);
            this._OK.MouseLeave += new System.EventHandler(this._Cancel_MouseLeave);
            // 
            // _Cancel
            // 
            this._Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._Cancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("_Cancel.BackgroundImage")));
            this._Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._Cancel.FlatAppearance.BorderSize = 0;
            this._Cancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this._Cancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this._Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Cancel.ForeColor = System.Drawing.Color.White;
            this._Cancel.Location = new System.Drawing.Point(310, 70);
            this._Cancel.Name = "_Cancel";
            this._Cancel.Size = new System.Drawing.Size(94, 23);
            this._Cancel.TabIndex = 5;
            this._Cancel.Text = "Отмена";
            this._Cancel.UseMnemonic = false;
            this._Cancel.Click += new System.EventHandler(this.Cancel_Click);
            this._Cancel.MouseEnter += new System.EventHandler(this._Cancel_MouseEnter);
            this._Cancel.MouseLeave += new System.EventHandler(this._Cancel_MouseLeave);
            // 
            // LoginForm1
            // 
            this.AcceptButton = this._OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Minecraft2D.My.Resources.Resources.menubackground;
            this.CancelButton = this._Cancel;
            this.ClientSize = new System.Drawing.Size(411, 101);
            this.Controls.Add(this._Cancel);
            this.Controls.Add(this._OK);
            this.Controls.Add(this.UsernameTextBox);
            this.Controls.Add(this.UsernameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Netcraft";
            this.Load += new System.EventHandler(this.LoginForm1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}