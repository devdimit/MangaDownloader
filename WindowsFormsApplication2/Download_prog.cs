using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{//
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;//запрет изменения размера
        }
        //глобальные переменные
        bool count = false;
        int lost_part = 0;//максимальное значение прогресс бара
        int schet = 0;//счетчик прогресс бара
        int end_potok = 0;//код состояния потока
        string buf2;
        bool pause_download = false;//t-пауза включена, f-выключена
        Func_saver func_saver = new Func_saver();//класс хранения функций

        string HTML_first_page;//код страницы где главы лежат
        string site_link;
        string global_name;//имя манги
        mang_info[] arr_mang_inf = new mang_info[2000];//массив из класса, описание глав + ссылка
        WebClient webClient = new WebClient();

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.ico_3_;           //иконка
            int i = 0;
            Link_to_manga.Text = "http://readmanga.me/****";
            Link_to_manga.ForeColor = Color.Gray;
            status.Text = "Проверка соединения с интернетом";
            try
            {
                /*    // проверка соединения с http://readmanga.me.        
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://readmanga.me");
                    request.Timeout = 1000;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream ReceiveStream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(ReceiveStream1, true);
                    string responseFromServer = sr.ReadToEnd();
                    response.Close();
                    status.Text = "Состояние ОК";*/

            }
            catch (Exception ex)
            {
                i = 1;
            }
            /*     try
                 {
                     // проверка соединения с интернетом.        
                     HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://google.com");
                     request.Timeout = 1000;
                     HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                     Stream ReceiveStream1 = response.GetResponseStream();
                     StreamReader sr = new StreamReader(ReceiveStream1, true);
                     string responseFromServer = sr.ReadToEnd();
                     response.Close();
                     status.Text = "Состояние ОК";

                 }
                 catch (Exception ex)
                 {
                     i=2;             
                 }*/
            switch (i)
            {
                case 1:
                    {
                        status.Text = "Соединение с интернетом обнаружено, но не удалось подключиться к сайту readmanga.me \nПроверьте ваш фаервол или настройки сетевого подключения";
                        status.ForeColor = Color.Red;
                        break;
                    }
                case 2:
                    {
                        status.Text = "Не удалось подключиться к интернету\nПроверьте ваш фаервол или настройки сетевого подключения";
                        status.ForeColor = Color.Red;
                        break;
                    }
                default:
                    {
                        status.Text = "Статус ОК";
                        status.ForeColor = Color.Green;
                        break;

                    }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Выберете папку для сохранения глав";//описание
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Text_way_to_save.Text = folderBrowserDialog1.SelectedPath;  //путь записывается в text_way_to_save
            }
        }





        private void Link_to_manga_Enter(object sender, EventArgs e)
        {
            if (count == false && Link_to_manga.Text == "http://readmanga.me/****")
            {
                Link_to_manga.Text = null;
                Link_to_manga.ForeColor = Color.Black;
                count = true;
            }

        }

        private void Select_all_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Found_parts.Items.Count; i++)
            {
                Found_parts.SetItemChecked(i, true);
            }
        }

        private void stop_Click(object sender, EventArgs e)
        {   //отключение потока загрузки
            //настройка gui

                backgroundWorker1.CancelAsync();
                Text_way_to_save.Enabled = true;
                Link_to_manga.Enabled = true;
                Found_parts.Enabled = true;
                Way_to_save.Enabled = true;
                Search_parts.Enabled = true;
                Select_no.Enabled = true;
                Select_all.Enabled = true;
                Download_this.Enabled = true;
                stop.Enabled = false;
                display_link.Text = "Ссылка на закачиваемую страницу: Закачка не производится";
                pause_download = false;
                pause.Text = "Пауза";
                pause.Enabled = false;

        }

        private void Download_this_Click(object sender, EventArgs e)
        {   //try
            //настройка gui
            stop.Enabled = true;
            Text_way_to_save.Enabled = false;
            Link_to_manga.Enabled = false;
            Found_parts.Enabled=false;
            Way_to_save.Enabled = false;
            Search_parts.Enabled = false;
            Select_no.Enabled = false;
            Select_all.Enabled = false;
            Download_this.Enabled = false;
            //конфиги паузы
            pause_download = false;
            pause.Text = "Пауза";
            pause.Enabled = true;



            status.Text = "Скачивание началось";
            status.ForeColor = Color.Green;
            //конфиги прогрес бара

            for (int i = 0; i < Found_parts.Items.Count; i++)
            {
                if (Found_parts.GetItemChecked(i) == true) lost_part++;
            }
            progressBar1.Maximum = lost_part;
            progressBar1.Value = 0;
            //многопоточность настройки

            backgroundWorker1.RunWorkerAsync();
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;



        }
           


         
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string subpath;//путь к главам
            string p1;
            string p2;
            string p3;
            int s1;
            int s2;
            int s3;
            int s4;
            int start;
            int end;
            string buf1;
            string buf3;
            int ibuf1;//буфер
            int count;
            string fullpath;
            lost_part = 0;
            schet = 0;//счетчик прогресс бара
            end_potok=0;
           //     i++;
             //   backgroundWorker1.ReportProgress(i);
               // Thread.Sleep(50);
                   string path = Text_way_to_save.Text + @"\" + global_name;
            if (path.IndexOf(@":\") == -1)
            {
                status.Text = "Неверно указан путь";
                status.ForeColor = Color.Red;
            }
            else
            {
                for (int i = 0; i < Found_parts.Items.Count; i++)
                {
                    if (Found_parts.GetItemChecked(i) == true) lost_part++;
                    arr_mang_inf[i].filter();//фильтрация имени манги и глав от запрещенных знаков
                }
                //progressBar1.Maximum = lost_part;
                //     System.Threading.Thread.Sleep(1000);
                DirectoryInfo dirInfo = new DirectoryInfo(path);//создание главной папки
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                if (site_link.IndexOf("mintmanga.com") != -1 || site_link.IndexOf("readmanga.me") != -1)
                {
                    for (int i = 0; i < Found_parts.Items.Count; i++)
                    {

                        if (Found_parts.GetItemChecked(i) == true && backgroundWorker1.CancellationPending != true)
                        {
                            subpath = arr_mang_inf[i].sub_name.ToString();
                            dirInfo.CreateSubdirectory(subpath);//создана папка для главы
                            fullpath = Text_way_to_save.Text + @"\" + global_name + @"\" + subpath;
                            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(arr_mang_inf[i].link + @"?mature=1");//запрос
                            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();//ответ

                            StreamReader stream = new StreamReader(
                                  resp.GetResponseStream(), Encoding.UTF8);//созданеи потока для получения ответа, перекодтровать в utf8
                            HTML_first_page = stream.ReadToEnd();//слушать поток до конца
                            // label2.Text = arr_mang_inf[i].link;
                            start = HTML_first_page.IndexOf(@"rm_h.init");
                            end = HTML_first_page.IndexOf(@"</script>", start);
                            buf1 = HTML_first_page.Substring(start, end - start);
                            start = end = s1 = 0;
                            count = 1;
                            while (s1 != -1 && backgroundWorker1.CancellationPending != true)
                            {
                                while (pause_download) System.Threading.Thread.Sleep(1000);//пауза

                                s1 = buf1.IndexOf(@"['", start);
                                s2 = buf1.IndexOf(@"'http://", start);
                                s3 = buf1.IndexOf(@"/',", start);
                                s4 = buf1.IndexOf(@",", s3 + 4);
                                p1 = buf1.Substring(s1 + 2, s2 - s1 - 4);//часть 2 ссылки
                                p2 = buf1.Substring(s2 + 1, s3 - s2);//часть 1 ссылки
                                p3 = buf1.Substring(s3 + 4, s4 - s3 - 5);//часть 3 ссылки
                                ibuf1 = p3.IndexOf(@"/", 1);
                                buf3 = p3.Substring(ibuf1 + 1, p3.Length - ibuf1 - 1);
                                buf2 = p2  + p1 + p3;//получение ссылки
                                start = buf1.IndexOf("],", start + 1);
                                s1 = buf1.IndexOf(@"['", start);
                                // webClient.DownloadFile(@"http://e1.adultmanga.me/auto/06/46/98/Nightmare_funk_001_127.png", Text_way_to_save.Text + @"\1.png");
                                Console.WriteLine(buf2);
                                Console.WriteLine(fullpath);
                                Console.WriteLine(buf3);
                                buf3 = func_saver.filter_nowindows_symbols(buf3);
                                webClient.DownloadFile(buf2, fullpath + @"\" + buf3);
                                count++;
                                backgroundWorker1.ReportProgress(schet, buf2);

                            }
                            schet++;
                            backgroundWorker1.ReportProgress(schet);
                            end_potok = 1;

                           // System.Threading.Thread.Sleep(100);


                        }
                    }
               //     status.Text = "Скачивание завершено. Статус ОК";
                 //   status.ForeColor = Color.Green;
                    backgroundWorker1.CancelAsync();

                }

                if (site_link.IndexOf("manga24") != -1)
                {
                    for (int i = 0; i < Found_parts.Items.Count; i++)
                    {
                        if (Found_parts.GetItemChecked(i) == true && backgroundWorker1.CancellationPending != true)
                        {
                            while (pause_download) System.Threading.Thread.Sleep(1000);//пауза
                            subpath = arr_mang_inf[i].sub_name.ToString();
                            dirInfo.CreateSubdirectory(subpath);//создана папка для главы
                            fullpath = Text_way_to_save.Text + @"\" + global_name + @"\" + subpath;
                            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(arr_mang_inf[i].link);//запрос
                            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();//ответ

                            StreamReader stream = new StreamReader(
                                  resp.GetResponseStream(), Encoding.UTF8);//созданеи потока для получения ответа, перекодтровать в utf8
                            HTML_first_page = stream.ReadToEnd();//слушать поток до конца
                            s1 = HTML_first_page.IndexOf(@"dir:");
                            s2 = HTML_first_page.IndexOf(",", s1);
                            buf1 = HTML_first_page.Substring(s1 + 6, s2 - s1 - 7);//прямая ссылка на сервер на папку, содержащую мангу
                            s1 = HTML_first_page.IndexOf("images: [[");
                            s2 = HTML_first_page.IndexOf("page", s1);
                            buf3 = HTML_first_page.Substring(s1 + 9, s2 - s1 - 20);//получение массива страниц
                            start = end = s1 = 0;
                            count = 1;


                            while (s1 != -1 && backgroundWorker1.CancellationPending != true)
                            {
                                s1 = buf3.IndexOf(@"[", start);
                                s2 = buf3.IndexOf(@",", s1 + 1);
                                p1 = buf3.Substring(s1 + 2, s2 - s1 - 3);
                                buf2 = buf1 + p1;//получение ссылки на изображение
                                start = buf3.IndexOf("]", s1 + 1);//найти конец старого массива с данныеми
                                s1 = buf3.IndexOf(@"[", start);//найти начало нового массива с данными
                                // webClient.DownloadFile(@"http://e1.adultmanga.me/auto/06/46/98/Nightmare_funk_001_127.png", Text_way_to_save.Text + @"\1.png");
                                p1 = func_saver.filter_nowindows_symbols(p1);
                                webClient.DownloadFile(buf2, fullpath + @"\" + p1);
                                count++;
                                backgroundWorker1.ReportProgress(schet, buf2);

                            }
                            schet++;
                            backgroundWorker1.ReportProgress(schet);

                            //System.Threading.Thread.Sleep(100);
                            end_potok = 1;

                        }
                    }
                }

            
            backgroundWorker1.CancelAsync();
            }
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = schet;
            display_link.Text = "Ссылка на закачиваемую страницу: " + buf2;
            //label1.Text = (e.ProgressPercentage.ToString() + "%");
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Text_way_to_save.Enabled = true;
            Link_to_manga.Enabled = true;
            Found_parts.Enabled = true;
            Way_to_save.Enabled = true;
            Search_parts.Enabled = true;
            Select_no.Enabled = true;
            Select_all.Enabled = true;
            Download_this.Enabled = true;
            stop.Enabled = false;
            pause_download = false;
            pause.Text = "Пауза";
            pause.Enabled = false;



           status.Text = "Статус ОК. Закачка не производится";
          //  if (end_potok == 0) status.Text = "Закачка прервана";
            display_link.Text = "Ссылка на закачиваемую страницу: Закачка не производится";

        }


        private void Select_no_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Found_parts.Items.Count; i++)
            {
                Found_parts.SetItemChecked(i, false);
            }

        }

        private void Link_to_manga_Leave(object sender, EventArgs e)
        {
            if (Link_to_manga.Text == "")
            {
                Link_to_manga.Text = "http://readmanga.me/****";
                Link_to_manga.ForeColor = Color.Gray;
                count = false;
            }
        }

        private void Search_parts_Click(object sender, EventArgs e)
        {
            int start = 0;//стартовая позиця для indexof шаг1
            int buf_start = 0;//буферный счетчик
            int buf_end = 0;
            int i = 0;//счетчик массива
            int length;//длина строки буфера
            string text;//буферная строка
            string link;//ссылка на адрес главы
            int j = 0;
            string name;//название главы full
            string sn;//буфер названий для папки
            int buf1;
            int buf2;
            Found_parts.Items.Clear();
            try
            {
                site_link = Link_to_manga.Text;//получение ссылки из поля
                int substr_link = site_link.IndexOf("http://");
                if (substr_link == -1)
                {
                    site_link = "http://" + "" + site_link;//проверка и правка адреса строки на протокол http
                }
                if (site_link.IndexOf(@"/", site_link.Length - 2) != -1) site_link = site_link.Remove(site_link.Length - 1, 1);//проверка на последний слеш
                if (site_link.IndexOf("mintmanga.com") != -1 || site_link.IndexOf("readmanga.me") != -1)
                {
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site_link);//запрос
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();//ответ

                    StreamReader stream = new StreamReader(
                          resp.GetResponseStream(), Encoding.UTF8);//созданеи потока для получения ответа, перекодтровать в utf8
                    HTML_first_page = stream.ReadToEnd();//слушать поток до конца
                    status.Text = "Статус ОК";
                    status.ForeColor = Color.Green;
                    //поиск подстроки с названием и ссылкой
                    while ((start = HTML_first_page.IndexOf("<td class=", start + 1)) != -1)
                    {
                        length = HTML_first_page.IndexOf("</td>", start) - start;
                        text = HTML_first_page.Substring(start, length);//блок с сылкой и названием
                        buf_start = text.IndexOf("<a href=");
                        buf_end = text.IndexOf("title");
                        link = text.Substring(buf_start + 9, buf_end - buf_start - 11);//получить адрес главы
                        j = link.IndexOf("/", 1);
                        link = link.Substring(j, link.Length - j);
                        link = site_link + link;
                        buf_start = buf_end;//title
                        if ((buf1 = text.IndexOf("<sup>", buf_start + 11)) == -1)
                        {
                            buf2 = 4;
                            buf_end = text.IndexOf("</a>");

                        }
                        else
                        {
                            buf2 = 5;
                            buf_end = text.IndexOf("<", buf_start + 11);

                        }
                        name = text.Substring(buf_start + 51, buf_end - buf_start - 67);//получение имени
                        buf_start = name.IndexOf("  ");//позиция первый двойной пробел 
                        buf_end = name.LastIndexOf("  ");//позиция последний двойной пробел 
                        global_name = name.Substring(0, buf_start - 2);
                        sn = name.Substring(buf_end + 2, name.Length - buf_end - buf2);
                        name = name.Remove(buf_start - 2, buf_end - buf_start + 4);//удаление ненужных пробелов
                        name = name.Insert(buf_start - 2, "  ");//название+глава
                        arr_mang_inf[i] = new mang_info(link, name, sn);//создание массива объектов -глав
                        Found_parts.Items.Add(name, true);//добавление глав в chekbox found_parts
                        i++;
                        About_found.Text = "Найдено глав" + i.ToString();
                    }


                    /*
                    //получение названия манги
                     int name_length;//длина названия
                    int link_length;//длина ссылки без http://
                     int position;//позиция "/"
                     string name_manga;//название манги транслитом
                      string nohttp;//строка без http://
                 
                 
                    nohttp = site_link.Substring(7, site_link.Length - 7);//строка без http://
                    link_length = nohttp.Length;//длина адреса без http://
                    position = nohttp.IndexOf("/");
                    name_manga = nohttp.Substring(position + 1, link_length - position - 1);//название манги с _ вместо " "
                    name_manga = name_manga.Replace("_", " ");
                    //name_manga.Replace()
                    label1.Text = name_manga;
                   // label1.Text = name_manga.IndexOf("e", 2).ToString();//поиск е со 2 символа
                     */
                }//конец readmanga и minitmanga
                if (site_link.IndexOf("manga24") != -1)
                {
                    int k = 0;
                    k = site_link.IndexOf(@"/", 10);
                    name = site_link.Substring(k + 1, site_link.Length - k - 1);
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site_link);//запрос
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();//ответ

                    StreamReader stream = new StreamReader(
                          resp.GetResponseStream(), Encoding.UTF8);//созданеи потока для получения ответа, перекодтровать в utf8
                    HTML_first_page = stream.ReadToEnd();//слушать поток до конца
                    status.Text = "Статус ОК";
                    status.ForeColor = Color.Green;
                    //поиск подстроки с названием и ссылкой
                    while ((start = HTML_first_page.IndexOf("</span>            <a href=", start + 1)) != -1)
                    {
                        length = HTML_first_page.IndexOf("</a>", start) - start;
                        text = HTML_first_page.Substring(start, length);//блок с сылкой и названием
                        buf_start = text.IndexOf("<a href=");
                        buf_end = text.IndexOf("title");
                        link = text.Substring(buf_start + 9, buf_end - buf_start - 11);//получить адрес главы
                        j = link.IndexOf("/", 1);
                        link = link.Substring(j, link.Length - j);
                        link = site_link + link;
                        global_name = name;
                        buf_start = buf_end; ;//title
                        buf_end = text.IndexOf(">", buf_start + 1);
                        sn = text.Substring(buf_start + 7, buf_end - buf_start - 8);//получение имени
                        arr_mang_inf[i] = new mang_info(link, name, sn);//создание массива объектов -глав
                        Found_parts.Items.Add(sn, true);//добавление глав в chekbox found_parts
                        i++;
                        About_found.Text = "Найдено глав" + i.ToString();
                    }


                }
            }
            catch (Exception ex)
            {
                status.Text = "Проверьте правильность написания ссылки";
                status.ForeColor = Color.Red;
            }


        }

        private void About_found_Click(object sender, EventArgs e)
        {

        }

        private void About_Click(object sender, EventArgs e)
        {
            About f2 = new About();


            f2.ShowDialog();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void status_Click(object sender, EventArgs e)
        {

        }

        private void display_link_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {   pause_download=!pause_download;
            if (pause_download == false) pause.Text = "Пауза";
            if (pause_download == true) pause.Text="Продолжить загрузку";
        }



    }

    public class mang_info//класс описания главы + ссылка
    {
        public string link;//ссылка
        public string name;//название
        public string sub_name;//название главы для папки
        public mang_info(string l, string n, string sn)
        {
            link = l;
            name = n;
            sub_name = sn;
        }
        public void filter()
        {
            name = name.Replace(@">", " ");
            name = name.Replace(@"<", " ");
            name = name.Replace(@"?", " ");
            name = name.Replace(@":", " ");
            name = name.Replace(@"|", " ");
            name = name.Replace(@"*", " ");
            name = name.Replace(@"\", " ");
            name = name.Replace(@"/", " ");
            name = name.Replace(@"'", " ");

            sub_name = sub_name.Replace(@">", " ");
            sub_name = sub_name.Replace(@"<", " ");
            sub_name = sub_name.Replace(@"?", " ");
            sub_name = sub_name.Replace(@":", " ");
            sub_name = sub_name.Replace(@"|", " ");
            sub_name = sub_name.Replace(@"*", " ");
            sub_name = sub_name.Replace(@"\", " ");
            sub_name = sub_name.Replace(@"/", " ");
            sub_name = sub_name.Replace(@"'", " ");


        }
    }
    public class Func_saver//сюда будут писаться всякие функции
    {
        public string filter_nowindows_symbols( string str)
        {
            str = str.Replace(@">", " ");
            str = str.Replace(@"<", " ");
            str = str.Replace(@"?", " ");
            str = str.Replace(@":", " ");
            str = str.Replace(@"|", " ");
            str = str.Replace(@"*", " ");
            str = str.Replace(@"\", " ");
            str = str.Replace(@"/", " ");
            str = str.Replace(@"'", " ");
            return str;
        }
    }


}
