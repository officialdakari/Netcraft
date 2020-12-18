using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Minecraft2D
{
    public partial class LoginForm1
    {
        Lang lang;
        public LoginForm1()
        {
            InitializeComponent();
            _OK.Name = "OK";
            _Cancel.Name = "Cancel";
        }

        // TODO: вставить код для настраиваемой аутентификации с использованием переданного имени пользователя и пароля 
        // (См. статью по адресу https://go.microsoft.com/fwlink/?LinkId=35339).  
        // Пользовательский принципал можно затем подключить к принципалу потока следующим образом: 
        // My.User.CurrentPrincipal = CustomPrincipal
        // где CustomPrincipal - реализация интерфейса IPrincipal, используемая для аутентификации. 
        // Впоследствии My.User будет возвращать идентификационную информацию, заключенную в объекте CustomPrincipal,
        // например, имя пользователя, отображаемое имя и т.д.

        private void OK_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.OK;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Cancel;
        }

        private void LoginForm1_Load(object sender, EventArgs e)
        {
            lang = Lang.FromFile($"./lang/{Utils.LANGUAGE}.txt");
            _OK.Text = lang.get("button.ok");
            _Cancel.Text = lang.get("button.cancel");
            UsernameTextBox.Text = "";
        }

        private void _Cancel_MouseEnter(object sender, EventArgs e)
        {
            ((Control)sender).BackgroundImage = My.Resources.Resources.buttonbghover;
            Control ctrl = (Control)sender;
            ControlPaint.DrawBorder(ctrl.CreateGraphics(), ctrl.Bounds, Color.Red, ButtonBorderStyle.Solid);
        }

        private void _Cancel_MouseLeave(object sender, EventArgs e)
        {
            ((Control)sender).BackgroundImage = My.Resources.Resources.buttonbg;
            ((Control)sender).Invalidate();
        }
        Random rnd = new Random();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string[] arr1 = File.ReadAllLines("./resources/randomizer/first.txt");
                string[] arr2 = File.ReadAllLines("./resources/randomizer/second.txt");
                string first = arr1[rnd.Next(0, arr1.Length - 1)];
                string second = arr2[rnd.Next(0, arr2.Length - 1)];
                UsernameTextBox.Text = $"{first}{second}{rnd.Next(25, 99).ToString()}";
                DialogResult = DialogResult.OK;
                Close();
            } catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}