using System;
using System.Windows.Forms;

namespace Minecraft2D
{
    public partial class LoginForm1
    {
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
    }
}