using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minecraft2D
{
    public partial class NConsole : Form
    {
        public NConsole()
        {
            InitializeComponent();
        }

        public static NConsole instance { get; private set; }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void NConsole_Load(object sender, EventArgs e)
        {
            instance = this;
        }

        public static string Join(char seperator, string[] elements, string start = "", string end = "")
        {
            string str = "";
            foreach (var e in elements)
            {
                str += start + e + end + seperator;
            }
            str = str.TrimEnd(seperator);
            return str;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var HT = new Hashtable();
                HT.Add("class", Color.Blue);
                HT.Add("interface", Color.Blue);
                HT.Add("public", Color.Blue);
                HT.Add("try", Color.Blue);
                HT.Add("catch", Color.Blue);
                HT.Add("finally", Color.Blue);
                HT.Add("get", Color.Blue);
                HT.Add("set", Color.Blue);
                HT.Add("for", Color.Blue);
                HT.Add("foreach", Color.Blue);
                HT.Add("as", Color.Blue);
                HT.Add("in", Color.Blue);
                HT.Add("private", Color.Blue);
                HT.Add("internal", Color.Blue);
                HT.Add("protected", Color.Blue);
                HT.Add("ctx", Color.Blue);
                HT.Add("self", Color.Blue);
                HT.Add("object", Color.Blue);
                HT.Add("var", Color.Blue);
                HT.Add("true", Color.Blue);
                HT.Add("false", Color.Blue);
                HT.Add("null", Color.Blue);
                HT.Add("void", Color.Blue);
                HT.Add("static", Color.Blue);
                HT.Add("new", Color.Blue);
                HT.Add("\\|\\|", Color.Blue);
                HT.Add("&&", Color.Blue);
                HT.Add("int", Color.Magenta);
                HT.Add("string", Color.Magenta);
                HT.Add("uint", Color.Magenta);
                HT.Add("decimal", Color.Magenta);
                HT.Add("double", Color.Magenta);
                HT.Add("long", Color.Magenta);
                HT.Add("short", Color.Magenta);
                HT.Add("ushort", Color.Magenta);
                HT.Add("ulong", Color.Magenta);
                HT.Add("Material", Color.Magenta);
                HT.Add("EnumBlockType", Color.Magenta);
                //HT.Add("\\d*", Color.LimeGreen);
                HT.Add("if", Color.Red);
                HT.Add("else", Color.Red);
                HT.Add("async", Color.Red);
                HT.Add("await", Color.Red);
                HT.Add("NetcraftPlayer", Color.Green);
                HT.Add("Block", Color.Green);
                HT.Add("IBlockMetadata", Color.Green);
                HT.Add("BlockChest", Color.Green);
                HT.Add("using", Color.Blue);
                a = richTextBox2.SelectionStart;
                string Words_dont_come_easy = Join('|', HT.Keys.Cast<string>().ToArray(), @"\b", @"\b");
                MatchCollection allIp = Regex.Matches(richTextBox2.Text, Words_dont_come_easy, RegexOptions.IgnoreCase);
                foreach (Match ip in allIp)
                {
                    richTextBox2.SelectionStart = ip.Index;
                    richTextBox2.SelectionLength = ip.Length;
                    // rtb1.SelectionBackColor = Color.Yellow
                    richTextBox2.SelectionColor = (Color)HT[ip.Value];
                }

                richTextBox2.SelectionStart = a;
                richTextBox2.SelectionLength = 0;
                richTextBox2.SelectionColor = Color.Black;
                richTextBox2.ScrollToCaret();
            } catch(Exception)
            {

            }
        }
        int a;
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                Form1.GetInstance().Send("eval?" + richTextBox2.Text);
                richTextBox2.Text = "";
                e.SuppressKeyPress = true;
            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(richTextBox2, e);
        }
    }
}
