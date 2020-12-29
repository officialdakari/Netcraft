using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minecraft2D
{
    public partial class Logs : Form
    {
        public Logs()
        {
            InitializeComponent();
        }
        public static Logs Instance { get; private set; }
        private void Logs_Load(object sender, EventArgs e)
        {
            Instance = this;
        }

        public void Log(string str)
        {
            listBox1.Items.Add(str);
        }
    }
}
