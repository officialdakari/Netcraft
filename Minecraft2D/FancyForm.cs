using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace NCore
{
    class FancyForm : Form
    {
        private ListBox listBox1;

        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(50, 76);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(85, 56);
            this.listBox1.TabIndex = 0;
            // 
            // FancyForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.listBox1);
            this.Name = "FancyForm";
            this.ResumeLayout(false);

        }
    }
}
