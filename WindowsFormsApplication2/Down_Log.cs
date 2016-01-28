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
            label1.Text = "Версия v1.01\nДанная программа предназначена для скачивания манги с сайтов:\n readmanga.me \n mintmanga.com\n manga24.ru\n\nРазработчик DevDimit@yandex.ru\nПринимаю заказы на создание небоольших программ под Windows";

        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
