using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Minecraft2D
{
    public partial class HelpWindow
    {
        public HelpWindow()
        {
            InitializeComponent();
            _ListBox1.Name = "ListBox1";
        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!Information.IsNothing(ListBox1.SelectedItem))
            {
                string a = ListBox1.SelectedItem.ToString();
                if (HT.ContainsKey(a))
                {
                    RichTextBox1.Text = Conversions.ToString(HT[a]);
                }
            }
        }

        private Hashtable HT = new Hashtable();

        private void HelpWindow_Load(object sender, EventArgs e)
        {
            HT.Add("Перемещение", "Чтобы перемещаться используйте клавиши A, D, W. Кнопка A перемещает игрока влево, D - вправо, W - прыжок.");
            HT.Add("Добывание и строительство", "Чтобы сломать блок, возьмите в руки предмет (Подробнее в разделе \"Инвентарь\") и нажмите ЛКМ по блоку. Чтобы ставить блоки, возьмите блок в руку и нажмите ПКМ в то место куда Вы хотите поставить блок. Используйте СКМ (Колёсико мыши) для того чтобы поставить фоновый блок.");
            HT.Add("Инвентарь", "Чтобы открыть инвентарь нажмите E или нажмите кнопку \"Инвентарь\" в игре. Чтобы взять предмет в руку нажмите дважды по предмету который Вы хотите взять в руку.");
            HT.Add("Вход в игру", "Пока-что в игре есть только сетевой режим игры. Чтобы запустить свой сервер, убедитесь что Вы установили \".NET Core 2.1\"." + Constants.vbCrLf + "1. Откройте \"cmd.exe\" в папке с NCore (NCore в архиве вместе с игрой)." + Constants.vbCrLf + "2. В командой строке введите \"dotnet NCore.dll\"" + Constants.vbCrLf + "3. Сервер запущен.");
            HT.Add("Сенсорное управление", "Игра также предусмотрена для Windows планшетов." + Constants.vbCrLf + "Чтобы включить сенсорное управление:" + Constants.vbCrLf + "1. Нажмите кнопку [Пауза] в игре" + Constants.vbCrLf + "2. В меню паузы выберите [Настройки]" + Constants.vbCrLf + "3. Включите \"Кнопки перемещения\"");
            HT.Add("Фоновые блоки", "В игре есть фоновые блоки. Чтобы их разместить используйте колёсико мыши (СКМ). С виду фоновые блоки затемнены, а также игроки могут через них ходить.");
            HT.Add("Секретная комбинация", "Чтобы включить No-clip введите в чат \"IDDQDD\". Также Вам надо отключить себе проверку перемещения (NCore: команда \"/nmc\").");
            HT.Add("PvP и уровень здоровья", "В игре есть шкала здоровья и PvP. Шкала здоровья отображается в левом верхнем углу в игре. Чтобы удалить игрока нажмите по нему ЛКМ. Себя ударить нельзя, а если Вы это сделаете с читами то Вы будете отключены с сервера (В NCore). Игрок имеет 100 единиц здоровья (знаю, это много, но временно). Каждые мечи наносят своё количество урона. Также есть урон от падения.");
            HT.Add("Чат и команды", "В игре также есть чат и команды. Чат открывается в отдельном окне. Если Вы закрыли чат, Вы можете снова его открыть нажав кнопку [Чат] в игре. Команды начинаются с символа \"/\" (В NCore). Чтобы получить список команд используйте \"/help\" (В NCore).");
            ListBox1.Items.AddRange(HT.Keys.Cast<string>().ToArray());
        }
    }
}