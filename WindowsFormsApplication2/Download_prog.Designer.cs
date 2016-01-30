namespace WindowsFormsApplication2
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Way_to_save = new System.Windows.Forms.Button();
            this.Found_parts = new System.Windows.Forms.CheckedListBox();
            this.Text_way_to_save = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Link_to_manga = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Search_parts = new System.Windows.Forms.Button();
            this.Select_all = new System.Windows.Forms.Button();
            this.Select_no = new System.Windows.Forms.Button();
            this.Download_this = new System.Windows.Forms.Button();
            this.About = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.About_found = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.stop = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.display_link = new System.Windows.Forms.Label();
            this.pause = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.down_paret_now = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Way_to_save
            // 
            this.Way_to_save.Location = new System.Drawing.Point(458, 50);
            this.Way_to_save.Name = "Way_to_save";
            this.Way_to_save.Size = new System.Drawing.Size(158, 20);
            this.Way_to_save.TabIndex = 0;
            this.Way_to_save.Text = "Выбрать путь сохранения";
            this.Way_to_save.UseVisualStyleBackColor = true;
            this.Way_to_save.Click += new System.EventHandler(this.button1_Click);
            // 
            // Found_parts
            // 
            this.Found_parts.FormattingEnabled = true;
            this.Found_parts.Location = new System.Drawing.Point(5, 124);
            this.Found_parts.Name = "Found_parts";
            this.Found_parts.Size = new System.Drawing.Size(447, 319);
            this.Found_parts.TabIndex = 0;
            this.Found_parts.TabStop = false;
            this.Found_parts.UseTabStops = false;
            // 
            // Text_way_to_save
            // 
            this.Text_way_to_save.Location = new System.Drawing.Point(5, 50);
            this.Text_way_to_save.Name = "Text_way_to_save";
            this.Text_way_to_save.Size = new System.Drawing.Size(447, 20);
            this.Text_way_to_save.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Место сохранения файлов";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ссылка на мангу";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // Link_to_manga
            // 
            this.Link_to_manga.Location = new System.Drawing.Point(5, 86);
            this.Link_to_manga.Name = "Link_to_manga";
            this.Link_to_manga.Size = new System.Drawing.Size(447, 20);
            this.Link_to_manga.TabIndex = 5;
            this.Link_to_manga.Enter += new System.EventHandler(this.Link_to_manga_Enter);
            this.Link_to_manga.Leave += new System.EventHandler(this.Link_to_manga_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Найденые главы";
            // 
            // Search_parts
            // 
            this.Search_parts.Location = new System.Drawing.Point(458, 86);
            this.Search_parts.Name = "Search_parts";
            this.Search_parts.Size = new System.Drawing.Size(158, 20);
            this.Search_parts.TabIndex = 7;
            this.Search_parts.Text = "Поиск глав по ссылке";
            this.Search_parts.UseVisualStyleBackColor = true;
            this.Search_parts.Click += new System.EventHandler(this.Search_parts_Click);
            // 
            // Select_all
            // 
            this.Select_all.Location = new System.Drawing.Point(458, 124);
            this.Select_all.Name = "Select_all";
            this.Select_all.Size = new System.Drawing.Size(158, 20);
            this.Select_all.TabIndex = 8;
            this.Select_all.Text = "Выбрать все";
            this.Select_all.UseVisualStyleBackColor = true;
            this.Select_all.Click += new System.EventHandler(this.Select_all_Click);
            // 
            // Select_no
            // 
            this.Select_no.Location = new System.Drawing.Point(458, 148);
            this.Select_no.Name = "Select_no";
            this.Select_no.Size = new System.Drawing.Size(158, 20);
            this.Select_no.TabIndex = 9;
            this.Select_no.Text = "Снять выбор со всех";
            this.Select_no.UseVisualStyleBackColor = true;
            this.Select_no.Click += new System.EventHandler(this.Select_no_Click);
            // 
            // Download_this
            // 
            this.Download_this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Download_this.Location = new System.Drawing.Point(458, 182);
            this.Download_this.Name = "Download_this";
            this.Download_this.Size = new System.Drawing.Size(158, 127);
            this.Download_this.TabIndex = 10;
            this.Download_this.Text = "Начать скачиваниевыбраных глав";
            this.Download_this.UseVisualStyleBackColor = true;
            this.Download_this.Click += new System.EventHandler(this.Download_this_Click);
            // 
            // About
            // 
            this.About.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.About.Location = new System.Drawing.Point(458, 399);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(158, 48);
            this.About.TabIndex = 11;
            this.About.Text = "О программе";
            this.About.UseVisualStyleBackColor = true;
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // About_found
            // 
            this.About_found.AutoSize = true;
            this.About_found.Location = new System.Drawing.Point(346, 107);
            this.About_found.Name = "About_found";
            this.About_found.Size = new System.Drawing.Size(77, 13);
            this.About_found.TabIndex = 12;
            this.About_found.Text = "Найдено глав";
            this.About_found.Click += new System.EventHandler(this.About_found_Click);
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.status.ForeColor = System.Drawing.Color.Green;
            this.status.Location = new System.Drawing.Point(110, 514);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(91, 13);
            this.status.TabIndex = 14;
            this.status.Text = "Состояние ОК";
            this.status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.status.Click += new System.EventHandler(this.status_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(567, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Программа для скачивания манги с сайов http://mintmanga.com, http://readmanga.me/" +
    ", http://mangachan.ru/  и";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(5, 499);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(597, 12);
            this.progressBar1.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 480);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(608, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Прогресс скачивания текущей главы. Во время скачивания в программе невозможно про" +
    "изводить другие действия.";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // stop
            // 
            this.stop.Enabled = false;
            this.stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.stop.Location = new System.Drawing.Point(458, 355);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(158, 34);
            this.stop.TabIndex = 19;
            this.stop.Text = "Стоп";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // display_link
            // 
            this.display_link.AutoSize = true;
            this.display_link.Location = new System.Drawing.Point(2, 452);
            this.display_link.Name = "display_link";
            this.display_link.Size = new System.Drawing.Size(324, 13);
            this.display_link.TabIndex = 20;
            this.display_link.Text = "Ссылка на закачиваемую страницу: Закачка не производится";
            this.display_link.Click += new System.EventHandler(this.display_link_Click);
            // 
            // pause
            // 
            this.pause.Enabled = false;
            this.pause.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pause.Location = new System.Drawing.Point(458, 315);
            this.pause.Name = "pause";
            this.pause.Size = new System.Drawing.Size(158, 34);
            this.pause.TabIndex = 21;
            this.pause.Text = "Пауза";
            this.pause.UseVisualStyleBackColor = true;
            this.pause.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(432, 524);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(184, 13);
            this.textBox1.TabIndex = 24;
            this.textBox1.Text = "Мои контакты DevDimit@yandex.ru ";
            // 
            // down_paret_now
            // 
            this.down_paret_now.AutoSize = true;
            this.down_paret_now.Location = new System.Drawing.Point(2, 467);
            this.down_paret_now.Name = "down_paret_now";
            this.down_paret_now.Size = new System.Drawing.Size(244, 13);
            this.down_paret_now.TabIndex = 27;
            this.down_paret_now.Text = "Скачиваемая глава: Закачка не производится";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "http://manga24.ru/";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(5, 540);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(597, 13);
            this.textBox2.TabIndex = 29;
            this.textBox2.Text = "Если вам понравилась данная программа то можете отблагодарить автора отправив люб" +
    "ую сумму на QIWI кошелек";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(5, 554);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(597, 13);
            this.textBox3.TabIndex = 30;
            this.textBox3.Text = "+79115613305";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 571);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.down_paret_now);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pause);
            this.Controls.Add(this.display_link);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.status);
            this.Controls.Add(this.About_found);
            this.Controls.Add(this.About);
            this.Controls.Add(this.Download_this);
            this.Controls.Add(this.Select_no);
            this.Controls.Add(this.Select_all);
            this.Controls.Add(this.Search_parts);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Link_to_manga);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Text_way_to_save);
            this.Controls.Add(this.Found_parts);
            this.Controls.Add(this.Way_to_save);
            this.Name = "Form1";
            this.Text = "MangaDownloader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Way_to_save;
        private System.Windows.Forms.CheckedListBox Found_parts;
        private System.Windows.Forms.TextBox Text_way_to_save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Link_to_manga;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Search_parts;
        private System.Windows.Forms.Button Select_all;
        private System.Windows.Forms.Button Select_no;
        private System.Windows.Forms.Button Download_this;
        private System.Windows.Forms.Button About;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label About_found;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button stop;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label display_link;
        private System.Windows.Forms.Button pause;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label down_paret_now;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
    }
}

