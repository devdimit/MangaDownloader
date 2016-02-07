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
using Ionic.Zip;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;//запрет изменения размера
        }
        //глобальные переменные
        //классы и объекты
        Func_saver func_saver = new Func_saver();//класс хранения функций
        mang_info[] arr_mang_inf = new mang_info[2000];//массив из класса, описание глав + ссылка

        //переменные потока
        int schet = 0;//максимальное значение прогресс бара
        bool pause_download = false;//пауза при загрузке. t-пауза включена, f-выключена
        int err = 0;//проверка правильности пути папки. 0-верно, 1 неверно
        string buf2;//ссылка на закачиваемую страницу, передающаяся в display_link
        int count_page;//счетчик страниц в главе. передается в прогресс бар
        WebClient webClient = new WebClient();//вебклиент,функция - скачивает главу в потоке


        //универсальные переменные
        bool count = false;//проверяет попытку пользователя ввести ссылку на мангу
        string HTML_first_page;//код страницы, полученной из HttpWebResponse
        string site_link;//ссылка из поля Link_to_manga.Text, дополнительно обрабатывается при нажатии search_parts
        string global_name;//имя манги. при загрузке создается корневая папка с этим именем, в которой лежат папки с главами
        string subpath; //глобальные переменные

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.ico_3_;           //иконка
            //int i = 0;
            Link_to_manga.Text = "http://readmanga.me/****";
            Link_to_manga.ForeColor = Color.Gray;
            status.Text = "Проверка соединения с интернетом";
        }

        private void button1_Click(object sender, EventArgs e)//кнопка выбора пути к папке
        {
            folderBrowserDialog1.Description = "Выберете папку для сохранения глав";//описание
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Text_way_to_save.Text = folderBrowserDialog1.SelectedPath;  //путь записывается в text_way_to_save
            }
        }

        private void Link_to_manga_Enter(object sender, EventArgs e)//ввод ссылки на мангу 
        {   //если ссылка на мангу установлена по умолчанию в http://readmanga.me/****, то очистить текстовое поле Link_to_manga 
            if (count == false && Link_to_manga.Text == "http://readmanga.me/****")
            {
                Link_to_manga.Text = null;//очистить текстовое поле Link_to_manga
                Link_to_manga.ForeColor = Color.Black;// изменить цвет текста на черный
                count = true;//была произведена попытка ввести название
            }

        }

        private void Select_all_Click(object sender, EventArgs e)//выбрать все checkbox
        {
            for (int i = 0; i < Found_parts.Items.Count; i++)
            {
                Found_parts.SetItemChecked(i, true);
            }
        }

        private void stop_Click(object sender, EventArgs e)//остановка загрузки
        {   //отключение потока загрузки
            //настройка gui
                err = 0;
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
                convert_cbr.Enabled = true;

                display_link.Text = "Ссылка на закачиваемую страницу: Закачка не производится";
                pause_download = false;
                pause.Text = "Пауза";
                pause.Enabled = false;
                down_paret_now.Text = "Скачиваемая глава: Закачка не производится";


        }

        private void Download_this_Click(object sender, EventArgs e)//старт загрузки
        {   //try
            int j=0;//счетчик
            //счетчик выбраных для скачивания глав
            for (int i = 0; i < Found_parts.Items.Count; i++)
            {
                if (Found_parts.GetItemChecked(i) == true)
                {
                    j++;
                }
            }
            //проверка выбраных глав для скачивания. процесс начнется только если выбрана хоть 1 глава и выбран путь сохранения
            if (j != 0 && Text_way_to_save.Text.IndexOf(@":\") != -1)
            {
                err = 0;
                //настройка gui
                stop.Enabled = true;
                Text_way_to_save.Enabled = false;
                Link_to_manga.Enabled = false;
                Found_parts.Enabled = false;
                Way_to_save.Enabled = false;
                Search_parts.Enabled = false;
                Select_no.Enabled = false;
                Select_all.Enabled = false;
                Download_this.Enabled = false;
                convert_cbr.Enabled = false;

                //конфиги паузы
                pause_download = false;
                pause.Text = "Пауза";
                pause.Enabled = true;
                progressBar1.Value = 0;//сброс прогресс бара



                status.Text = "Скачивание началось";
                status.ForeColor = Color.Green;
                //конфиги прогрес бара

                progressBar1.Value = 0;
                //многопоточность настройки

                backgroundWorker1.RunWorkerAsync();
                backgroundWorker1.WorkerSupportsCancellation = true;
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.DoWork += backgroundWorker1_DoWork;
                backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
                backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            }
            else 
            {   
                if(j==0)
                {
                    status.Text = "Главы не выбраны";
                    status.ForeColor = Color.Red;
                }
                if (Text_way_to_save.Text.IndexOf(@":\") == -1)
                {
                    status.Text = "Не выбран путь сохранения";
                    status.ForeColor = Color.Red;
                }

            }


        }
           


         
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            subpath="";//путь к главам
            string p1;
            string p2;
            string p3;
            int s1;
            int s2;
            int s3;
            int s4;
            int start;
            int end;
            count_page = 1;
            string img_path;
            string folder_cbr_name;
            string path;
            string buf1;
            string buf3;
            int ibuf1;//буфер
            string fullpath;
            schet = 0;//счетчик прогресс бара
           //     i++;
             //   backgroundWorker1.ReportProgress(i);
               // Thread.Sleep(50);
                   path = Text_way_to_save.Text + @"\" + global_name;
                   path = func_saver.filter_disk_folder(path);
                   Console.WriteLine(path);
            if (path.IndexOf(@":\") == -1)
            {
                err = 1;
                backgroundWorker1.ReportProgress(err, "err");//отправление состояния из потока

            }
            else
            {
                for (int i = 0; i < Found_parts.Items.Count; i++)
                {
                    arr_mang_inf[i].name=func_saver.filter_nowindows_symbols(arr_mang_inf[i].name);//фильтрация имени манги и глав от запрещенных знаков
                    arr_mang_inf[i].sub_name = func_saver.filter_nowindows_symbols(arr_mang_inf[i].sub_name);//фильтрация имени манги и глав от запрещенных знаков

                }
                DirectoryInfo dirInfo = new DirectoryInfo(path);//создание главной папки
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                if (site_link.IndexOf("mintmanga.com") != -1 || site_link.IndexOf("readmanga.me") != -1)
                {

                    if (convert_cbr.Checked == true) dirInfo.CreateSubdirectory(@"cbr");//создана папка для глав cbr
                    folder_cbr_name = path + @"\"+"cbr";

                    //логика скачивания с mintmanga и readmanga
                    for (int i = 0; i < Found_parts.Items.Count; i++)
                    {
                             ZipFile zip = new ZipFile();//создание архива
                             zip.ProvisionalAlternateEncoding = Encoding.GetEncoding("cp866");//подключение русского языка

                        if (Found_parts.GetItemChecked(i) == true && backgroundWorker1.CancellationPending != true)
                        {
                            subpath = arr_mang_inf[i].sub_name.ToString();
                            subpath = func_saver.filter_foldername(subpath);
                            Console.WriteLine(subpath);
                            dirInfo.CreateSubdirectory(subpath);//создана папка для главы
                            Console.WriteLine(subpath);
                            fullpath = path + @"\" + subpath;
                            HTML_first_page = func_saver.get_HTML(arr_mang_inf[i].link + @"?mature=1");//получить html
                            schet = 0;
                            start = HTML_first_page.IndexOf(@"rm_h.init");
                            end = HTML_first_page.IndexOf(@"</script>", start);
                            buf1 = HTML_first_page.Substring(start, end - start);
                            start = end = s1 = 0;
                            count_page = 1;
                            
                            //считается число страниц в главе
                            while (s1 != -1)
                            {   
                                s1 = buf1.IndexOf(@"['", start);
                                start = buf1.IndexOf("],", start + 1);
                                s1 = buf1.IndexOf(@"['", start);
                                schet++;
                            }
                            backgroundWorker1.ReportProgress(schet, "");//счетчик страниц в главе


                            start = end = s1 = 0;
                            count_page = 1;
                            //скачиваются страницы
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
                                Console.WriteLine(buf2);
                                Console.WriteLine(fullpath);
                                Console.WriteLine(buf3);
                                buf3 = func_saver.filter_nowindows_symbols(buf3);
                                img_path = fullpath + @"\" + func_saver.convert_number_page(count_page) + ".jpg";//путь к изображению
                                webClient.DownloadFile(buf2, img_path);
                                backgroundWorker1.ReportProgress(count_page, buf2);//передается счетчик страниц
                                count_page++;
                                if (convert_cbr.Checked == true) zip.AddFile(img_path);//добавление изображения в архив
                            }
                            if (convert_cbr.Checked == true) zip.Save(folder_cbr_name + @"\" + subpath + @".cbr");//сохранение архива

                        }
                    }
                    backgroundWorker1.CancelAsync();
                }

                if (site_link.IndexOf("manga24") != -1)
                {
                    if (convert_cbr.Checked == true) dirInfo.CreateSubdirectory(@"cbr");//создана папка для глав cbr
                    folder_cbr_name = path + @"\cbr";

                    //логика скачивания с manga24
                    for (int i = 0; i < Found_parts.Items.Count; i++)
                    {
                        ZipFile zip = new ZipFile();//создание архива
                        zip.ProvisionalAlternateEncoding = Encoding.GetEncoding("cp866");//подключение русского языка

                        if (Found_parts.GetItemChecked(i) == true && backgroundWorker1.CancellationPending != true)
                        {
                            subpath = arr_mang_inf[i].sub_name.ToString();
                            subpath = func_saver.filter_foldername(subpath);
                            dirInfo.CreateSubdirectory(subpath);//создана папка для главы
                            
                            fullpath = path + @"\" + subpath;
                            HTML_first_page = func_saver.get_HTML(arr_mang_inf[i].link);//получить html
                            s1 = HTML_first_page.IndexOf(@"dir:");
                            s2 = HTML_first_page.IndexOf(",", s1);
                            buf1 = HTML_first_page.Substring(s1 + 6, s2 - s1 - 7);//прямая ссылка на сервер на папку, содержащую мангу
                            s1 = HTML_first_page.IndexOf("images: [[");
                            s2 = HTML_first_page.IndexOf("page", s1);
                            buf3 = HTML_first_page.Substring(s1 + 9, s2 - s1 - 20);//получение массива страниц
                            schet = 0;
                            start = end = s1 = 0;
                            count_page = 1;

                            //считается число страниц в главе
                            while (s1 != -1)
                            {
                                s1 = buf3.IndexOf(@"[", start);
                                start = buf3.IndexOf("]", s1 + 1);//найти конец старого массива с данныеми
                                s1 = buf3.IndexOf(@"[", start);//найти начало нового массива с данными
                                schet++;
                            }
                            backgroundWorker1.ReportProgress(schet, "");//число страниц в главе

                            start = end = s1 = 0;
                            count_page = 1;

                            //скачивание глав
                            while (s1 != -1 && backgroundWorker1.CancellationPending != true)
                            {
                                while (pause_download) System.Threading.Thread.Sleep(1000);//пауза
                                s1 = buf3.IndexOf(@"[", start);
                                s2 = buf3.IndexOf(@",", s1 + 1);
                                p1 = buf3.Substring(s1 + 2, s2 - s1 - 3);
                                buf2 = buf1 + p1;//получение ссылки на изображение
                                start = buf3.IndexOf("]", s1 + 1);//найти конец старого массива с данныеми
                                s1 = buf3.IndexOf(@"[", start);//найти начало нового массива с данными
                                p1 = func_saver.filter_nowindows_symbols(p1);
                                img_path = fullpath + @"\" + func_saver.convert_number_page(count_page) + ".jpg";//путь к изображению

                                webClient.DownloadFile(buf2, img_path);
                                count_page++;
                                backgroundWorker1.ReportProgress(count_page, buf2);
                                if (convert_cbr.Checked == true) zip.AddFile(img_path);//добавление изображения в архив
                            }
                            if (convert_cbr.Checked == true) zip.Save(folder_cbr_name + @"\" + subpath + @".cbr");//сохранение архива
                        }
                    }
                }
                if (site_link.IndexOf("mangachan.ru") != -1)
                {
                    if (convert_cbr.Checked == true) dirInfo.CreateSubdirectory(@"cbr");//создана папка для глав cbr
                    folder_cbr_name = path + @"\cbr";

                    //логика скачивания с mangachan
                    for (int i = 0; i < Found_parts.Items.Count; i++)
                    {
                        ZipFile zip = new ZipFile();//создание архива
                        zip.ProvisionalAlternateEncoding = Encoding.GetEncoding("cp866");//подключение русского языка

                        if (Found_parts.GetItemChecked(i) == true && backgroundWorker1.CancellationPending != true)
                        {
                            
                            subpath = arr_mang_inf[i].sub_name.ToString();
                            subpath = func_saver.filter_foldername(subpath);
                            dirInfo.CreateSubdirectory(subpath);//создана папка для главы
                            
                            fullpath = path + @"\" + subpath;
                            HTML_first_page = func_saver.get_HTML(arr_mang_inf[i].link);//получить html
                            s1 = HTML_first_page.IndexOf("\"thumbs\":[\"");//маска начала списка глав
                            s2 = HTML_first_page.IndexOf("\"]", s1);//маска конца списка глав
                            buf1 = HTML_first_page.Substring(s1 + 11, s2 - s1 - 10);//прямая ссылка на сервер на папку, содержащую мангу
                            schet = 0;
                            start = end = s1 = 0;
                            count_page = 1;
                            Console.WriteLine(buf1);
                            //счетчик страниц в главе
                            while (s1 != -1)
                            {
                                //здесь buf1 не трогать
                                s1 = buf1.IndexOf(@"http://", start);
                                start = s1 + 1;
                                s1 = buf1.IndexOf(",", start);//проверка последний ли адрес конец массива
                                schet++;                            //подсчет страниц

                            }

                            backgroundWorker1.ReportProgress(schet, "");//передать максимальное число страниц


                            start = end = s1 = 0;
                            count_page = 1;
                            Console.WriteLine(buf1);
                            // скачивание файлов
                            while (s1 != -1 && backgroundWorker1.CancellationPending != true)
                            {   
                                //здесь buf1 не трогать
                                while (pause_download) System.Threading.Thread.Sleep(1000);//пауза
                                s1 = buf1.IndexOf(@"http://", start);
                                start=s1+1;
                                s2 = buf1.IndexOf("\"", s1 + 1);//конец ссылки
                                p1 = buf1.Substring(s1, s2-s1 );//ссылка на миниатюру. не оьраьотана
                                buf2 = p1.Substring(7, p1.IndexOf("."));//нахождение img части ссылки
                                buf3 = buf2.Replace("im", "img");
                                p1 = p1.Replace(buf2, buf3);
                                Console.WriteLine(p1);
                                p1 = p1.Replace("manganew_thumbs", "manganew");//замененная ссылка. верная
                                buf2 = p1;
                                s1 = buf1.IndexOf(",", start);//проверка последний ли адрес в массиве
                                img_path = fullpath + @"\" + func_saver.convert_number_page(count_page) + ".jpg";//путь к изображению
                                webClient.DownloadFile(buf2, img_path);//скачивание изображения
                                count_page++;//подсчет страниц
                                backgroundWorker1.ReportProgress(count_page, buf2);
                                if (convert_cbr.Checked == true) zip.AddFile(img_path);//добавление изображения в архив
                            }
                            if (convert_cbr.Checked == true) zip.Save(folder_cbr_name + @"\" + subpath + @".cbr");//сохранение архива
                        }
                    }
                }
                if (site_link.IndexOf("hentaichan.ru") != -1)
                {
                    if (convert_cbr.Checked == true) dirInfo.CreateSubdirectory(@"cbr");//создана папка для глав cbr
                    folder_cbr_name = path + @"\cbr";

                    //логика скачивания с hentaichan
                      for (int i = 0; i < Found_parts.Items.Count; i++)
                      {
                          ZipFile zip = new ZipFile();//создание архива
                          zip.ProvisionalAlternateEncoding = Encoding.GetEncoding("cp866");//подключение русского языка

                        if (Found_parts.GetItemChecked(i) == true && backgroundWorker1.CancellationPending != true)
                        {

                            subpath = arr_mang_inf[i].sub_name.ToString();
                            subpath = func_saver.filter_foldername(subpath);
                            dirInfo.CreateSubdirectory(subpath);//создана папка для главы

                            fullpath = path + @"\" + subpath;
                            HTML_first_page = func_saver.get_HTML(arr_mang_inf[i].link);//получить html
                            s1 = HTML_first_page.IndexOf("\"thumbs\":[\"");//маска начала списка глав
                            s2 = HTML_first_page.IndexOf("\"]", s1);//маска конца списка глав
                            buf1 = HTML_first_page.Substring(s1 + 11, s2 - s1 - 10);//прямая ссылка на сервер на папку, содержащую мангу
                            schet = 0;
                            start = end = s1 = 0;
                            count_page = 1;
                            Console.WriteLine(buf1);
                            //счетчик страниц в главе
                            while (s1 != -1)
                            {
                                //здесь buf1 не трогать
                                s1 = buf1.IndexOf(@"http://", start);
                                start = s1 + 1;
                                s1 = buf1.IndexOf(",", start);//проверка последний ли адрес конец массива
                                schet++;                            //подсчет страниц

                            }

                            backgroundWorker1.ReportProgress(schet, "");//передать максимальное число страниц


                            start = end = s1 = 0;
                            count_page = 1;
                            Console.WriteLine(buf1);
                            // скачивание файлов
                            while (s1 != -1 && backgroundWorker1.CancellationPending != true)
                            {
                                //здесь buf1 не трогать
                                while (pause_download) System.Threading.Thread.Sleep(1000);//пауза
                                s1 = buf1.IndexOf(@"http://", start);
                                start = s1 + 1;
                                s2 = buf1.IndexOf("\"", s1 + 1);//конец ссылки
                                p1 = buf1.Substring(s1, s2 - s1);//ссылка на миниатюру. не оьраьотана
                                buf2 = p1.Substring(7, p1.IndexOf("."));//нахождение img части ссылки
                                p1 = p1.Replace("manganew_thumbs", "manganew");//замененная ссылка. верная
                                buf2 = p1;
                                s1 = buf1.IndexOf(",", start);//проверка последний ли адрес в массиве
                                img_path = fullpath + @"\" + func_saver.convert_number_page(count_page) + ".jpg";//путь к изображению
                                webClient.DownloadFile(buf2, img_path);//скачивание изображения
                                count_page++;//подсчет страниц
                                backgroundWorker1.ReportProgress(count_page, buf2);
                                if (convert_cbr.Checked == true) zip.AddFile(img_path);//добавление изображения в архив
                            }
                            if (convert_cbr.Checked == true) zip.Save(folder_cbr_name + @"\" + subpath + @".cbr");//сохранение архива
                        }
                    }
                }

            
            backgroundWorker1.CancelAsync();
            }
        }

        //вывод информации при загрузке
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {   
            //проверка правильности пути
            if(err==1)
             {
                 status.Text = "Неверно указан путь";
                 status.ForeColor = Color.Red;
             }
            //настройка прогресбара progressBar1
            progressBar1.Maximum = schet;
            progressBar1.Value = count_page - 1;
            //отображение текущей загрузки: название главы и ссылка на загружаемое изображение
            down_paret_now.Text = "Скачиваемая глава: " + subpath;
            display_link.Text = "Ссылка на закачиваемую страницу: " + buf2;
        }

        //завершение процесса загрузки
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {   
            //конфиги интерфейса и состояния загрузки
            err = 0;
            progressBar1.Value = schet;
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
            convert_cbr.Enabled = true;
            //сообщения
           down_paret_now.Text = "Скачиваемая глава: Закачка не производится";
           status.Text = "Статус ОК. Закачка не производится";
           display_link.Text = "Ссылка на закачиваемую страницу: Закачка не производится";

        }


        private void Select_no_Click(object sender, EventArgs e)//снять checkbox со всех найденых глав полей  Found_parts
        {
            for (int i = 0; i < Found_parts.Items.Count; i++)
            {
                Found_parts.SetItemChecked(i, false);
            }

        }

        private void Link_to_manga_Leave(object sender, EventArgs e)//при покидании текстового поля Link_to_manga
        {
            if (Link_to_manga.Text == "")//если название введено не было
            {
                Link_to_manga.Text = "http://readmanga.me/****";//ввести маску поля по умолчанию
                Link_to_manga.ForeColor = Color.Gray;//цвет текста серый
                count = false;//ссылка введена не была
            }
        }

        private void Search_parts_Click(object sender, EventArgs e)
        {

            Found_parts.Items.Clear();//очистка поля found_parts
            try
            {
                site_link = Link_to_manga.Text;//получение ссылки из поля
                
                //фильтрация адреса
                site_link = func_saver.filter_site_link(site_link);
                //если сайт mintmanga или  readmanga
                if (site_link.IndexOf("mintmanga.com") != -1 || site_link.IndexOf("readmanga.me") != -1)
                    readmanga_searchparts(site_link);//поиск глав

                if (site_link.IndexOf("manga24") != -1)
                    manga24_searchparts(site_link);

                if (site_link.IndexOf("mangachan.ru") != -1)
                    mangachan_searchparts(site_link);
                if (site_link.IndexOf("hentaichan.ru") != -1)
                    hchan_searchparts(site_link);

            }
            catch (Exception ex)
            {
                status.Text = "Проверьте правильность написания ссылки";
                status.ForeColor = Color.Red;
            }


        }




      
        private void About_Click(object sender, EventArgs e)//открывает откно с инфой о программе
        {
            About f2 = new About();


            f2.ShowDialog();
        }

        private void pause_Click(object sender, EventArgs e)
        {
            pause_download = !pause_download;
            if (pause_download == false) pause.Text = "Пауза";
            if (pause_download == true) pause.Text = "Продолжить загрузку";
        }


        //функционал формы

        //поиск глав
        public void readmanga_searchparts(string site_link)
        {
            {
                int start = 0;//стартовая позиця для indexof шаг1
                int buf_start = 0;//буферный счетчик. начало
                int buf_end = 0;//буферный счетчик. конец
                int i = 0;//счетчик массива
                int length;//длина строки буфера
                string text;//буферная строка
                string link;//ссылка на главу манги
                int j = 0;
                string name;//название главы full
                string sn;//название для папки главы
                int buf1;
                int buf2;
                //логика поиска глав mintmanga и readmanga
                HTML_first_page = func_saver.get_HTML(site_link);
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
                }
                About_found.Text = "Найдено глав" + i.ToString();//количество найденых глав

            }//конец readmanga и minitmanga
        }
        public void manga24_searchparts(string site_link)
        {
            {
                int start = 0;//стартовая позиця для indexof шаг1
                int buf_start = 0;//буферный счетчик. начало
                int buf_end = 0;//буферный счетчик. конец
                int i = 0;//счетчик массива
                int length;//длина строки буфера
                string text;//буферная строка
                string link;//ссылка на главу манги
                int j = 0;
                string name;//название главы full
                string sn;//название для папки главы

                //логика поиска глав manga24
                //получение названия для корневой папки
                j = site_link.IndexOf(@"/", 10);
                name = site_link.Substring(j + 1, site_link.Length - j - 1);
                //полученик кода страницы
                HTML_first_page = func_saver.get_HTML(site_link);
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
            }//конец manga24
        }
        public void mangachan_searchparts(string site_link)
        {
            {
                int start = 0;//стартовая позиця для indexof шаг1
                int i = 0;//счетчик массива
                string link;//ссылка на главу манги
                int j = 0;
                string name;//название главы full
                string sn;//название для папки главы

                //логика поиска глав mangachan
                string substr;
                int pos_substr1;
                int pos_substr2;
                string table_html;//код таблицы с главами
                int t_count = 0;//счетчик найденых таблиц

                HTML_first_page = func_saver.get_HTML(site_link);
                substr = "href='" + site_link;//маска названия

                //получение названия для корневой папки
                pos_substr1 = HTML_first_page.IndexOf(substr);
                pos_substr1 = HTML_first_page.IndexOf(">", pos_substr1);
                pos_substr2 = HTML_first_page.IndexOf("</a>", pos_substr1);
                global_name = HTML_first_page.Substring(pos_substr1 + 1, pos_substr2 - pos_substr1 - 1);
                name = global_name;
                Console.WriteLine(global_name);

                status.Text = "Статус ОК";
                status.ForeColor = Color.Green;

                //поиск таблицы с главами
                substr = "<table class=\"table_cha\"";//маска начала таблицы
                while (HTML_first_page.IndexOf(substr, t_count) != -1)//пока находятся страницы
                {
                    pos_substr1 = HTML_first_page.IndexOf(substr, t_count);
                    t_count = pos_substr1 + 1;
                    pos_substr1 = HTML_first_page.IndexOf("<td><tr", pos_substr1);
                    pos_substr2 = HTML_first_page.IndexOf("</table>", pos_substr1);
                    table_html = HTML_first_page.Substring(pos_substr1 + 1, pos_substr2 - pos_substr1 - 1);

                    //разбор таблицы. поиск подстроки с главой : названием и ссылкой
                    while ((start = table_html.IndexOf("<a href='", start + 1)) != -1)
                    {
                        pos_substr1 = table_html.IndexOf("' title", start);
                        pos_substr2 = pos_substr1 - start - 9;
                        link = table_html.Substring(start + 9, pos_substr2);//блок с сылкой
                        Console.WriteLine(link);
                        link = "http://mangachan.ru" + link;
                        Console.WriteLine(link);
                        //получение названия главы
                        start = table_html.IndexOf(">", pos_substr1);
                        pos_substr1 = table_html.IndexOf("</", pos_substr1);
                        pos_substr2 = pos_substr1 - start - 1;
                        sn = table_html.Substring(start + 1, pos_substr2);//блок с названием главы
                        sn = sn.Replace("&nbsp;", "");
                        Console.WriteLine(sn);
                        arr_mang_inf[i] = new mang_info(link, name, sn);//создание массива объектов -глав
                        Found_parts.Items.Add(sn, true);//добавление глав в chekbox found_parts
                        i++;
                        About_found.Text = "Найдено глав" + i.ToString();
                    }

                }
            }//конец mangachan
        }
        public void hchan_searchparts(string site_link)
        {
            {
                int start = 0;//стартовая позиця для indexof шаг1
                int buf_start = 0;//буферный счетчик. начало
                int buf_end = 0;//буферный счетчик. конец
                int i = 1;//счетчик массива
                int k=0;//счетчик глав
                string text;//буферная строка
                string link;//ссылка на главу манги
                int j = 0;
                string name;//название главы full
                string sn;//название для папки главы
                string parts;
                string site_link_buf;
                string site_link_buf_while;

                //полученик кода страницы
                HTML_first_page = func_saver.get_HTML(site_link);
                status.Text = "Статус ОК";
                status.ForeColor = Color.Green;

                //получить название манги
                buf_start = HTML_first_page.IndexOf("title_top_a");
                buf_start = HTML_first_page.IndexOf("\">", buf_start + 1);
                buf_end = HTML_first_page.IndexOf("</a>", buf_start + 1);
                name = HTML_first_page.Substring(buf_start + 2, buf_end - buf_start - 2);//получение имени из заголовка
                Console.WriteLine(name);
                //получение глав
                if (HTML_first_page.IndexOf("Все главы") != -1)
                {   
                    //название корневой папки
                    buf_start = name.IndexOf(" - г");//название папки
                    global_name = name.Substring(0, buf_start);

                    //фильтр на ссылку. изменение на страницу с главами
                    buf_start = site_link.IndexOf(@"html?", 0);//отсеять после html
                    if (buf_start != -1) site_link = site_link.Substring(0, buf_start + 4);

                    buf_start = site_link.IndexOf(@"/", 10);//старт поиска /
                    site_link_buf = site_link.Remove(buf_start + 1, 5);//замена manga на related
                    site_link_buf = site_link_buf.Insert(buf_start + 1, "related");//замена manga на related

                    //получение и вывод числа глав
                    HTML_first_page = func_saver.get_HTML(site_link_buf + "?offset=" + 0);
                    buf_start = HTML_first_page.IndexOf("pagination_related");
                    buf_start = HTML_first_page.IndexOf("<b>", buf_start+1);
                    buf_end = HTML_first_page.IndexOf("Резул", buf_start + 1);
                    parts = HTML_first_page.Substring(buf_start + 3, buf_end - buf_start - 4);
                    Console.WriteLine(parts);
                    About_found.Text = "Найдено глав " + parts;
                    
                   //поиск ссылок и названий глав
                    while (i!=0)
                    {   

                        i = 0;
                        site_link_buf_while = site_link_buf + "?offset=" + j;
                        HTML_first_page = func_saver.get_HTML(site_link_buf_while);

                        while ((start = HTML_first_page.IndexOf("<div class=\"related\">", start + 1)) != -1)
                        {
                            buf_start = HTML_first_page.IndexOf("<a href=", start + 1);
                            buf_start = HTML_first_page.IndexOf("\"", buf_start);
                            buf_end = HTML_first_page.IndexOf("\"", buf_start + 1);
                            text = HTML_first_page.Substring(buf_start + 8, buf_end - buf_start-8);//получение относительного пути к ссылке
                            link = "http://hentaichan.ru/online/" + text;//ссылка на онлайн читалку для одной главы
                            Console.WriteLine(link);

                            //получить название главы
                            buf_start = HTML_first_page.IndexOf("<h2><a href=", buf_start);
                            buf_start = HTML_first_page.IndexOf(">", buf_start+5);
                            buf_end = HTML_first_page.IndexOf("<", buf_start + 1);
                            sn = HTML_first_page.Substring(buf_start + 1, buf_end - buf_start - 1);

                            Console.WriteLine(sn);
                            i++;//количество глав в возвращенной странице
                            arr_mang_inf[k] = new mang_info(link, name, sn);//создание массива объектов -глав
                            k++;//счетчик глав
                            Found_parts.Items.Add(sn, true);//добавление глав в chekbox found_parts

                        }
                        j=j + 10;//параметр поиска глав

                    }
                }
                else 
                {
                    i = 0;
                    //если сингл 
                    global_name = name;
                    buf_start = site_link.IndexOf(@"/", 10);//старт поиска слеш1
                    site_link = site_link.Remove(buf_start+1, 5);
                    site_link = site_link.Insert(buf_start + 1, "online");//замена manga на online
                    //фильтр адреса
                    buf_start = site_link.IndexOf(@"html?", 0);
                    if (buf_start != -1) site_link = site_link.Substring(0, buf_start + 4);
                    link = site_link;//ссылка на онлайн читалку
                    sn = name;//название главы=название сингла
                    Console.WriteLine(name);
                    arr_mang_inf[i] = new mang_info(link, name, sn);//создание массива объектов -глав
                    Found_parts.Items.Add(sn, true);//добавление глав в chekbox found_parts
                    About_found.Text = "Найдено глав" + 1;

                }
            }//конец hentaichan
        }


    }

    public class mang_info//класс описания главы + ссылка
    {
        public string link;//ссылка
        public string name;//название корневой папки
        public string sub_name;//название главы для папки
        public mang_info(string l, string n, string sn)
        {
            link = l;
            name = n;
            sub_name = sn;
        }
    }
    public class Func_saver//сюда будут писаться всякие функции
    {
        //фильтрация строк на символы, запрещеныне windows
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

        //фильтрация пути папок с главами
        public string filter_foldername(string str)
        {   
            str = str.Replace(@">", " ");
            str = str.Replace(@"<", " ");
            str = str.Replace(@"?", " ");
            str = str.Replace(@":", "");
            str = str.Replace(@"|", " ");
            str = str.Replace(@"*", " ");
            str = str.Replace(@"'", " ");
            return str;
        }

        //фильтрация пути  корневой папки 
        public string filter_disk_folder(string str) 
        {
            str = str.Replace(@">", " ");
            str = str.Replace(@"<", " ");
            str = str.Replace(@"?", " ");
            str = str.Replace(@":", "");
            str = str.Replace(@"|", " ");
            str = str.Replace(@"*", " ");
            //    str = str.Replace(@"/", " ");
            str = str.Replace(@"'", " ");
            str = str.Insert(1, @":");
            return str;

        }

        //получение html кода
        public string get_HTML(string url)
        {
            string html_str;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);//запрос
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();//ответ
            StreamReader stream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);//созданеи потока для получения ответа, перекодтровать в utf8
            html_str = stream.ReadToEnd();//слушать ответ до конца
            return html_str;
        }

        //фильтрация и корректировка ссылки
        public string filter_site_link(string site_link) 
        {
            int substr_link = site_link.IndexOf("http://");
            if (substr_link == -1)
            {
                site_link = "http://" + "" + site_link;//проверка и правка адреса строки на протокол http
            }
            if (site_link.IndexOf(@"/", site_link.Length - 2) != -1) site_link = site_link.Remove(site_link.Length - 1, 1);//убирает последний слеш если есть
            return site_link;
        }

        //создание формата изображения типа "abc" т.е 001
        public string convert_number_page(int count)
        {
            string str;
            str = count.ToString();
            if (count < 100) str = "0" + count;
            if (count < 10) str = "00" + count;

            return str;
        }
        //создать архив для cbr
   
    }
   

}
