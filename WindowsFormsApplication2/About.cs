using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;//запрет изменения размера
            this.Icon = Properties.Resources.ico_3_;           //иконка
            label1.Text = "Версия v1.02\nДанная программа предназначена для скачивания манги с сайтов:\n readmanga.me \n mintmanga.com\n manga24.ru\n\nДанная программа имеет открытый исходный код и распространяется бесплатно. \nАвтор не несет ответственности за последствия сторонней модификации кода программы.\nВопросы и предложения по поводу программы отправлять на почту разработчика DevDimit@yandex.ru\nПринимаю заказы на создание небоольших программ под Windows";

        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
