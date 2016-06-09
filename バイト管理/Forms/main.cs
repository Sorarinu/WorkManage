using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.IO;
using System.Data.OleDb;
using System.Threading;
using System.Data.SQLite;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Collections;

namespace WorkManage
{
    public partial class main : Form
    {
        /*------------------------
         定数宣言
        ------------------------*/
        private const int MAX = 42; //コントロール配列の最大数
        private const string AccessKey = "AIzaSyAqnjvV-F7QqX25Z3J7cIxFq3ixJyuGr0E"; //GoogleカレンダーAPIのアクセスキー
        private const string DATABASE_NAME = "Manage.db"; //データベースファイル名
        private const int ID = 0;        //DB制御
        private const int CDate = 1;
        private const int CYear = 2;
        private const int CMonth = 3;
        private const int CDay = 4;
        private const int HW = 5;
        private const int WST = 6;
        private const int WET = 7;
        private const int BST = 8;
        private const int BET = 9;
        private const int MEMO = 10;

        /*------------------------
         データベース関係
        ------------------------*/
        private string sql; //SQL文
        private string gDataBaseFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), DATABASE_NAME);
        private string gDataSource = "Data Source=" + Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), DATABASE_NAME) + ";Default Timeout=2;Synchronous=Off";
        private SQLiteDataReader rd;
        private SQLiteCommand command;  // SQLコマンド
        private SQLiteConnection conn;

        private FormDragResizer formResizer;
        private FormDragMover formMover;
        private InifileUtils ini = new InifileUtils();  //INI書き込み用コンストラクタ
        public ToolTip ToolTip1;    //マウスオーバー時のツールチップ
        public Label[] lblArray = new Label[MAX];   //内容
        public Label[] DaysArray = new Label[MAX];  //日付
        private GetHoliday.NationalHoliday[] NationalHolidays;  //祝日
        public ArrayList LocalHoliday = new ArrayList();
        private int[] Holiday = new int[10] { 0, 6, 7, 13, 14, 20, 21, 27, 28, 34 }; //休日
        private bool WindowFlg = false;    //フォームが全画面かどうか
        private WebClient downloadClient = null;
        private bool UpdateState = false;

        /*------------------------
         変更前のウィンドウサイズ
        ------------------------*/
        public struct BeforWindowState
        {
            public int y;
            public int x;
            public int Location_x;
            public int Location_y;
        }
        public BeforWindowState BWS;

        private void InitializeForm()
        {
            // Formのイニシャル処理で生成する
            formResizer = new FormDragResizer(this, panel2, FormDragResizer.ResizeDirection.All, 10);
            formMover = new FormDragMover(panel1, this, 8);
        }

        /// <summary>
        /// 初期化、ローカルデータベースへの接続
        /// データベースにテーブルが無い場合は作成
        /// </summary>
        public main()
        {
            /*------------------------
             二重起動チェック
            ------------------------*/
            if (System.Diagnostics.Process.GetProcessesByName( System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                //すでに起動していると判断する
                MessageBox.Show( this, "既に起動しています", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            /*------------------------
             旧ファイル削除
            ------------------------*/
            if (Environment.CommandLine.IndexOf("/up", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                try
                {
                    string[] args = Environment.GetCommandLineArgs();
                    int pid = Convert.ToInt32(args[2]);
                    Process.GetProcessById(pid).WaitForExit();    // 終了待ち
                }
                catch (Exception) { }

                File.Delete("WorkManage.old");
            }

            SQLiteCommand command;  // SQLコマンド

            InitializeComponent();
            InitializeForm();

            BWS = new BeforWindowState();

            /*------------------------
             データベース生成
            ------------------------*/
            using (conn = new SQLiteConnection(gDataSource))
            {
                try
                {
                    conn.Open();
                    
                    // テーブルの作成
                    sql = "create table Manage (" +
                          " id integer primary key AUTOINCREMENT," +
                          " CDate text, " +
                          " CYear integer, " +
                          " CMonth integer," +
                          " CDay integer," +
                          " HourlyWage integer," +
                          " WorkStartTime text," +
                          " WorkEndTime text," +
                          " BreakStartTime text," +
                          " BreakEndTime text," +
                          " Memo text" +
                          " )";

                    command = new SQLiteCommand(sql, conn);
                    command.ExecuteNonQuery();
                }
                catch (Exception) { }
                finally
                {
                    conn.Close();
                } 
            }

            //祝日として扱わないもの
            LocalHoliday.Add("銀行休業日");
            LocalHoliday.Add("クリスマス");
            LocalHoliday.Add("大晦日");
        }

        /// <summary>
        /// 初期化、当月のカレンダー出力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_Load(object sender, EventArgs e)
        {
            int i;
            string date;

            CalendarMonth month = new CalendarMonth();
            InifileUtils ini;
            
            ToolTip1 = new ToolTip(this.components);
            ToolTip1.InitialDelay = 100;   //ToolTipが表示されるまでの時間
            ToolTip1.ReshowDelay = 100;    //ToolTipが表示されている時に、別のToolTipを表示するまでの時間
            ToolTip1.AutoPopDelay = 10000;  //ToolTipを表示する時間
            ToolTip1.ShowAlways = true;     //フォームがアクティブでない時でもToolTipを表示する

            #region コントロール配列作成
            lblArray[0] = t1;
            lblArray[1] = t2;
            lblArray[2] = t3;
            lblArray[3] = t4;
            lblArray[4] = t5;
            lblArray[5] = t6;
            lblArray[6] = t7;
            lblArray[7] = t8;
            lblArray[8] = t9;
            lblArray[9] = t10;
            lblArray[10] = t11;
            lblArray[11] = t12;
            lblArray[12] = t13;
            lblArray[13] = t14;
            lblArray[14] = t15;
            lblArray[15] = t16;
            lblArray[16] = t17;
            lblArray[17] = t18;
            lblArray[18] = t19;
            lblArray[19] = t20;
            lblArray[20] = t21;
            lblArray[21] = t22;
            lblArray[22] = t23;
            lblArray[23] = t24;
            lblArray[24] = t25;
            lblArray[25] = t26;
            lblArray[26] = t27;
            lblArray[27] = t28;
            lblArray[28] = t29;
            lblArray[29] = t30;
            lblArray[30] = t31;
            lblArray[31] = t32; 
            lblArray[32] = t33;
            lblArray[33] = t34;
            lblArray[34] = t35;
            lblArray[35] = t36;
            lblArray[36] = t37;
            lblArray[37] = t38;
            lblArray[38] = t39;
            lblArray[39] = t40;
            lblArray[40] = t41;
            lblArray[41] = t42;

            DaysArray[0] = d1;
            DaysArray[1] = d2;
            DaysArray[2] = d3;
            DaysArray[3] = d4;
            DaysArray[4] = d5;
            DaysArray[5] = d6;
            DaysArray[6] = d7;
            DaysArray[7] = d8;
            DaysArray[8] = d9;
            DaysArray[9] = d10;
            DaysArray[10] = d11;
            DaysArray[11] = d12;
            DaysArray[12] = d13;
            DaysArray[13] = d14;
            DaysArray[14] = d15;
            DaysArray[15] = d16;
            DaysArray[16] = d17;
            DaysArray[17] = d18;
            DaysArray[18] = d19;
            DaysArray[19] = d20;
            DaysArray[20] = d21;
            DaysArray[21] = d22;
            DaysArray[22] = d23;
            DaysArray[23] = d24;
            DaysArray[24] = d25;
            DaysArray[25] = d26;
            DaysArray[26] = d27;
            DaysArray[27] = d28;
            DaysArray[28] = d29;
            DaysArray[29] = d30;
            DaysArray[30] = d31;
            DaysArray[31] = d32;
            DaysArray[32] = d33;
            DaysArray[33] = d34;
            DaysArray[34] = d35;
            DaysArray[35] = d36;
            DaysArray[36] = d37;
            DaysArray[37] = d38;
            DaysArray[38] = d39;
            DaysArray[39] = d40;
            DaysArray[40] = d41;
            DaysArray[41] = d42;
            #endregion

            #region イベントハンドラ登録
            for (i = 0; i < MAX; i++)
            {
                lblArray[i].MouseMove += new MouseEventHandler(MouseMove);
                lblArray[i].MouseLeave += new EventHandler(MouseLeave);
                lblArray[i].Click += new EventHandler(Click);
            }
            #endregion            

            timer1.Enabled = true;

            /*------------------------
             時給・締日取得
            ------------------------*/
            if (System.IO.File.Exists(@"settings.ini")) //INIファイルが存在する
            {
                ini = new InifileUtils();
                TimeMoneyH.Text = ini.getValueString("HourlyWage", "Weekday");
                TimeMoneyK.Text = ini.getValueString("HourlyWage", "Holiday");
                txtClosingDay.Text = ini.getValueString("ClosingDay", "Day");
            }
            //存在しない場合は作成
            else
            {
                Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
                StreamWriter writer = new StreamWriter(@"settings.ini", true, sjisEnc);

                writer.WriteLine("[HourlyWage]\r\nWeekday=0\r\nHoliday=0\r\n\r\n[ClosingDay]\r\nDay=0");
                writer.Close();

                ini = new InifileUtils();
                TimeMoneyH.Text = ini.getValueString("HourlyWage", "Weekday");
                TimeMoneyK.Text = ini.getValueString("HourlyWage", "Holiday");
                txtClosingDay.Text = ini.getValueString("ClosingDay", "Day");
            }

            date = DateTime.Today.ToString("yyyy-MM-dd");
            txtYear.Text = DateTime.Today.ToString("yyyy");
            cmbMonth.Text = DateTime.Today.ToString("MM");

            month = GetCalendarMonth(date);

            /*------------------------
             現在年の祝日を取得
            ------------------------*/
            NationalHolidays = GetHoliday.GetNationalHolidays(int.Parse(DateTime.Today.ToString("yyyy")), AccessKey, LocalHoliday);

            /*------------------------
             出力
            ------------------------*/
            Out_Calendar(txtYear.Text, month, NationalHolidays, LocalHoliday);  //カレンダーの出力
            GET_DB_Data();  //データ取得
            ToolTip_Reload();

            StatRun.Text = "準備完了";
            StatRun.ForeColor = Color.White;

            StatErr.Text = "NOERROR";
            StatErr.ForeColor = Color.White;
        }

        /// <summary>
        /// フォームが表示されたら警告を出力・アップデート確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_Shown(object sender, EventArgs e)
        {
            /*------------------------
             アップデート確認
            ------------------------*/
            Update_Check(0);

            if(UpdateState == true){
                if (TimeMoneyH.Text == "0" || TimeMoneyK.Text == "0")
                {
                    MessageBox.Show(this, "時給の設定がされていません\r\n上部メニューバーから、時給の設定を行ってください", "お知らせ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (txtClosingDay.Text == "0")
                {
                    MessageBox.Show(this, "時給の締日が設定されていません\r\n上部メニューバーから、締日の設定を行ってください", "お知らせ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// 最新版の確認
        /// </summary>
        /// <param name="state">
        /// 0 : 起動時
        /// 1 : アップデート確認ボタン動作時
        /// </param>
        private void Update_Check(int state)
        {
            string s;
            int iCompare;

            try
            {
                WebClient wc = new WebClient();
                Stream st = wc.OpenRead("http://www.nxtg-t.net/downloads/version.txt");
                StreamReader sr = new StreamReader(st, Encoding.GetEncoding(51932));

                s = sr.ReadLine();

                st.Close();
                wc.Dispose();

                /*------------------------
                 サーバデータとバージョン比較
                ------------------------*/
                iCompare = string.CompareOrdinal(Application.ProductVersion, s);

                if (iCompare < 0)
                {
                    DialogResult result = MessageBox.Show(this, "最新バージョンがあります。\r\n今すぐ更新しますか？", "お知らせ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    switch (result)
                    {
                        case DialogResult.Yes:
                            try
                            {
                                int pid = System.Diagnostics.Process.GetCurrentProcess().Id;

                                /*------------------------
                                 旧ファイルを.oldへ
                                ------------------------*/
                                File.Delete("WorkManage.old");
                                File.Delete("readme.txt");
                                File.Move("WorkManage.exe", "WorkManage.old");

                                /*------------------------
                                 新ファイルダウンロード
                                ------------------------*/
                                #region 実行ファイル
                                //ダウンロードしたファイルの保存先
                                string fileName = "WorkManage.exe";
                                //ダウンロード元URL
                                Uri u = new Uri("http://www.nxtg-t.net/downloads/" + s + "/WorkManage.exe");
                                //WebClientの作成
                                if (downloadClient == null) downloadClient = new System.Net.WebClient();
                                
                                //非同期ダウンロードを開始
                                downloadClient.DownloadFileAsync(u, fileName);
                                #endregion

                                #region ReadMeテキスト
                                //ダウンロードしたファイルの保存先
                                fileName = "readme.txt";
                                //ダウンロード元URL
                                u = new Uri("http://www.nxtg-t.net/downloads/" + s + "/readme.txt");
                                //WebClientの作成
                                if (downloadClient == null)
                                {
                                    downloadClient = new System.Net.WebClient();
                                    //イベントハンドラの作成
                                    downloadClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(downloadClient_DownloadFileCompleted);
                                }
                                else
                                {
                                    downloadClient = null;
                                    downloadClient = new System.Net.WebClient();
                                    //イベントハンドラの作成
                                    downloadClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(downloadClient_DownloadFileCompleted);
                                }

                                //非同期ダウンロードを開始
                                downloadClient.DownloadFileAsync(u, fileName);
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(this, "バージョンアップに失敗しました\r\n\r\nエラー:" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                File.Move("WorkManage.old", "WorkManage.exe");
                                return;
                            }
                            break;

                        case DialogResult.No:
                            UpdateState = true;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    switch (state)
                    {
                        case 0:
                            UpdateState = true;
                            break;

                        case 1:
                            MessageBox.Show(this, "お使いのソフトウェアは最新です\r\n更新の必要はありません", "お知らせ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// アップデートの結果出力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void downloadClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(this, "バージョンアップに失敗しました\r\n\r\nエラー:" + e.Error.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.Move("WorkManage.old", "WorkManage.exe");
                return;
            }
            else
            {
                MessageBox.Show(this, "ダウンロードが完了しました\r\n最新バージョンに更新するため再起動します", "お知らせ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start("WorkManage.exe", "/up " + Process.GetCurrentProcess().Id);
                this.Close();
            }
        }


        /// <summary>
        /// データベースから勤務時間等データの取得
        /// </summary>
        public void GET_DB_Data()
        {
            int i;
            string month;
            TimeSpan ts;    //勤務時間
            TimeSpan bts;   //休憩時間
            double Money;

            month = lblMonth.Text;

            //背景色初期化
            for (i = 0; i < MAX; i++) lblArray[i].BackColor = Color.FromKnownColor(KnownColor.Silver);

            using (conn = new SQLiteConnection(gDataSource))
            {
                try
                {
                    conn.Open();

                    sql = "select * from Manage where CYear=" + lblYear.Text + " and CMonth=" + month;

                    command = new SQLiteCommand(sql, conn);
                    rd = command.ExecuteReader();

                    while (rd.Read())   // データがあるだけ繰り返す
                    {
                        try
                        {
                            for (i = 0; i < MAX; i++)
                            {
                                if ((lblMonth.Text == rd.GetValue(CMonth).ToString()) && (DaysArray[i].Text == rd.GetValue(CDay).ToString())) //日付照合
                                {
                                    if ((rd.GetValue(BST).ToString() != "00:00:00") && (rd.GetValue(BET).ToString() != "00:00:00"))  //休憩時間があったら
                                    {
                                        bts = DateTime.Parse(rd.GetValue(BET).ToString()) - DateTime.Parse(rd.GetValue(BST).ToString());    //休憩時間取得
                                        ts = (DateTime.Parse(rd.GetValue(WET).ToString()) - DateTime.Parse(rd.GetValue(WST).ToString())) - bts; //勤務時間取得
                                    }
                                    else ts = DateTime.Parse(rd.GetValue(WET).ToString()) - DateTime.Parse(rd.GetValue(WST).ToString());   //勤務時間取得

                                    Money = double.Parse(rd.GetValue(HW).ToString()) * ts.TotalHours;
                                    lblArray[i].Text = Money.ToString();   //対応する日付へ出力

                                    //メモがあれば色付け
                                    if (rd.GetValue(MEMO).ToString() != "NULL")
                                    {
                                        lblArray[i].BackColor = Color.FromKnownColor(KnownColor.SkyBlue);
                                    }
                                    else if (rd.GetValue(MEMO).ToString() == "NULL")
                                    {
                                        lblArray[i].BackColor = Color.FromKnownColor(KnownColor.Silver);
                                    }
                                }
                            }
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                    WorkCalc();
                }
            }
        }

        /// <summary>
        /// ツールチップに出力するデータの取得
        /// </summary>
        /// <param name="Month">出力する月</param>
        /// <param name="Day">出力する日</param>
        /// <returns>時給、時間などの勤務情報が入った文字列</returns>
        private string GET_DB_Data_About(string Month, string Day)
        {
            string str = "";
            string Memo_str = "";
            TimeSpan WST_ts;
            TimeSpan WET_ts;
            TimeSpan BST_ts;
            TimeSpan BET_ts;

            using (conn = new SQLiteConnection(gDataSource))
            {
                try
                {
                    conn.Open();

                    sql = "select * from Manage where CYear=" + lblYear.Text + " and CMonth=" + Month + " and CDay=" + Day;

                    command = new SQLiteCommand(sql, conn);
                    SQLiteDataReader rd = command.ExecuteReader();

                    try
                    {
                        if (rd.Read())
                        {
                            //時間部分取得
                            WST_ts = DateTime.Parse(rd.GetValue(WST).ToString()).TimeOfDay;
                            WET_ts = DateTime.Parse(rd.GetValue(WET).ToString()).TimeOfDay;
                            BST_ts = DateTime.Parse(rd.GetValue(BST).ToString()).TimeOfDay;
                            BET_ts = DateTime.Parse(rd.GetValue(BET).ToString()).TimeOfDay;

                            Memo_str = rd.GetValue(MEMO).ToString();

                            str = "時給　　　　　　：" + rd.GetValue(HW).ToString() + " 円\r\n" + "勤務開始時間：" + WST_ts.ToString() + "\r\n" + "勤務終了時間：" + WET_ts.ToString() + "\r\n" + "休憩開始時間：" + BST_ts.ToString() + "\r\n" + "休憩終了時間：" + BET_ts.ToString() + "\r\n" + "メモ：" + Memo_str.ToString();
                            return str;
                        }
                        else
                        {
                            str = "時給　　　　　　：NULL\r\n勤務開始時間：NULL\r\n勤務終了時間：NULL\r\n休憩開始時間：NULL\r\n休憩終了時間：NULL\r\nメモ：NULL";
                            return str;
                        }
                    }
                    catch (Exception)
                    {
                        str = "時給：　　　　　　NULL\r\n勤務開始時間：NULL\r\n勤務終了時間：NULL\r\n休憩開始時間：NULL\r\n休憩終了時間：NULL\r\nメモ：NULL";
                        return str;
                    }
                }
                catch (Exception)
                {
                    str = "時給：　　　　　　NULL\r\n勤務開始時間：NULL\r\n勤務終了時間：NULL\r\n休憩開始時間：NULL\r\n休憩終了時間：NULL\r\nメモ：NULL";
                    return str;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 勤務時間・勤務日数計算
        /// </summary>
        public void WorkCalc()
        {
            float TimeH = 0;
            float TimeK = 0;
            int WorkDayH = 0;
            int WorkDayK = 0;
            float TotalMoneyTmp = 0;

            string month;
            string tmpDay;
            string tmpDay2;
            string tmpMonth;
            string tmpMonth2;
            string tmpYear;
            TimeSpan ts;    //勤務時間
            TimeSpan bts;   //休憩時間
            DateTime  d;    //最終日

            if ((TimeMoneyH.Text == "0" || TimeMoneyK.Text == "0" || txtClosingDay.Text == "0") || (TimeMoneyH.Text == "" || TimeMoneyK.Text == "" || txtClosingDay.Text == ""))
            {
               return;
            }

            month = lblMonth.Text;

            using (conn = new SQLiteConnection(gDataSource))
            {
                try
                {
                    conn.Open();

                    /*------------------------
                     2桁補正
                    ------------------------*/
                    if ((int.Parse(txtClosingDay.Text) + 1).ToString().Length != 2) tmpDay = "0" + (int.Parse(txtClosingDay.Text) + 1).ToString();
                    else tmpDay = (int.Parse(txtClosingDay.Text) + 1).ToString();

                    if ((int.Parse(lblMonth.Text) - 1).ToString().Length != 2) tmpMonth = "0" + (int.Parse(lblMonth.Text) - 1).ToString();
                    else tmpMonth = (int.Parse(lblMonth.Text) - 1).ToString();

                    if (lblMonth.Text.Length != 2) tmpMonth2 = "0" + lblMonth.Text;
                    else tmpMonth2 = lblMonth.Text;

                    if (txtClosingDay.Text.Length != 2) tmpDay2 = "0" + txtClosingDay.Text;
                    else tmpDay2 = txtClosingDay.Text;

                    /*------------------------
                     月始め判定
                    ------------------------*/
                    if (DateTime.TryParse(lblYear.Text + "/" + (int.Parse(lblMonth.Text) - 1) + "/01", out d))
                    {
                        d = d.AddMonths(1).AddDays(-1);
                    }

                    //1月なら前年12月に
                    if (tmpMonth == "00")
                    {
                        tmpYear = (int.Parse(lblYear.Text) - 1).ToString();
                        tmpMonth = "12";
                    }
                    else
                    {
                        tmpYear = lblYear.Text;
                    }

                    // DBからデータを取得する
                    if (txtClosingDay.Text == d.Day.ToString())
                    { //前月の最終日
                        sql = "select * from Manage where CDate between '" + tmpYear + "-" + tmpMonth + "-" + tmpDay + "' and '" +
                            lblYear.Text + "-" + tmpMonth2 + "-" + tmpDay2 + "' order by CDate ASC";
                    }
                    else
                    {
                        sql = "select * from Manage where CDate between '" + tmpYear + "-" + tmpMonth + "-" + tmpDay + "' and '" +
                            lblYear.Text + "-" + tmpMonth2 + "-" + tmpDay2 + "' order by CDate ASC";
                    }

                    command = new SQLiteCommand(sql, conn);
                    rd = command.ExecuteReader();

                    while (rd.Read())
                    {
                        try
                        {
                            if ((rd.GetValue(BST).ToString() != "00:00:00") && (rd.GetValue(BET).ToString() != "00:00:00"))  //休憩時間があったら
                            {
                                //休日・祝日
                                if (rd.GetValue(HW).ToString() == TimeMoneyK.Text)
                                {
                                    bts = DateTime.Parse(rd.GetValue(BET).ToString()) - DateTime.Parse(rd.GetValue(BST).ToString());    //休憩時間取得
                                    ts = (DateTime.Parse(rd.GetValue(WET).ToString()) - DateTime.Parse(rd.GetValue(WST).ToString())) - bts; //勤務時間取得

                                    WorkDayK += 1;
                                    TimeK += (float)ts.TotalHours;
                                }
                                //平日
                                else if (rd.GetValue(HW).ToString() == TimeMoneyH.Text)
                                {
                                    bts = DateTime.Parse(rd.GetValue(BET).ToString()) - DateTime.Parse(rd.GetValue(BST).ToString());    //休憩時間取得
                                    ts = (DateTime.Parse(rd.GetValue(WET).ToString()) - DateTime.Parse(rd.GetValue(WST).ToString())) - bts; //勤務時間取得

                                    WorkDayH += 1;
                                    TimeH += (float)ts.TotalHours;
                                }
                            }
                            else
                            {
                                //休日・祝日
                                if (rd.GetValue(HW).ToString() == TimeMoneyK.Text)
                                {
                                    ts = DateTime.Parse(rd.GetValue(WET).ToString()) - DateTime.Parse(rd.GetValue(WST).ToString());   //勤務時間取得

                                    WorkDayK += 1;
                                    TimeK += (float)ts.TotalHours;
                                }
                                //平日
                                else if (rd.GetValue(HW).ToString() == TimeMoneyH.Text)
                                {
                                    ts = DateTime.Parse(rd.GetValue(WET).ToString()) - DateTime.Parse(rd.GetValue(WST).ToString());   //勤務時間取得

                                    WorkDayH += 1;
                                    TimeH += (float)ts.TotalHours;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }

            /*------------------------
             出力
            ------------------------*/
            WorkDaysH.Text = WorkDayH.ToString();   //平日の勤務日数
            WorkDaysK.Text = WorkDayK.ToString();   //休日の勤務日数
            WorkDay.Text = (WorkDayH + WorkDayK).ToString();    //合計勤務日数


            WorkTimeH.Text = TimeH.ToString();      //平日の勤務時間
            WorkTimeK.Text = TimeK.ToString();      //休日の勤務時間
            WorkTime.Text = (TimeH + TimeK).ToString();

            TotalMoneyTmp = (int.Parse(TimeMoneyH.Text) * TimeH) + (int.Parse(TimeMoneyK.Text) * TimeK);
            TotalMoney.Text = TotalMoneyTmp.ToString(); //支給金額
        }

        /// <summary>
        /// カレンダーオールクリア
        /// </summary>
        private void All_Clear_Calendar()
        {
            for (int i = 0; i < MAX; i++)
            {
                lblArray[i].Text = "";
                DaysArray[i].Text = "";
            }
        }

        /// <summary>
        /// カレンダーの内容クリア
        /// </summary>
        private void Clear_Calendar()
        {
            for (int i = 0; i < MAX; i++) lblArray[i].Text = "";
        }

        /// <summary>
        /// カレンダー切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGO_Click(object sender, EventArgs e)
        {
            string YEAR, MONTH;
            string date;

            if (txtYear.Text == "" || cmbMonth.Text == "")
            {
                MessageBox.Show("値が無効です");
                return;
            }
            else
            {
                YEAR = txtYear.Text;
                MONTH = cmbMonth.Text;
            }

            date = YEAR + "-" + MONTH + "-1";

            CalendarMonth month = new CalendarMonth();
            NationalHolidays = GetHoliday.GetNationalHolidays(int.Parse(txtYear.Text), AccessKey, LocalHoliday);

            month = GetCalendarMonth(date);

            /*------------------------
             出力
            ------------------------*/
            Out_Calendar(YEAR, month, NationalHolidays, LocalHoliday);

            GET_DB_Data();  //データ取得

            ToolTip_Reload();
        }

        /// <summary>
        /// カレンダー出力
        /// </summary>
        /// <param name="year">出力するカレンダーの年</param>
        /// <param name="month">出力するカレンダーの月</param>
        /// <param name="NationalHoliday">祝日情報</param>
        /// <param name="LocalHoliday">休日情報</param>
        private void Out_Calendar(string year, CalendarMonth month, GetHoliday.NationalHoliday[] NationalHoliday, ArrayList LocalHoliday)
        {
            int i = 0;
            int WEEK = 0, DAY = 0;
            bool flg = false;
            string tmpDate;
            string tmpDay;
            string tmpMonth;
            string TodayDate;
            string Date;
            DateTime dt1;

            All_Clear_Calendar();

            lblMonth.Text = month.Weeks[1].Days[0].Month.ToString();
            lblYear.Text = year;

            try
            {
                for (i = 0; i < MAX; i++)
                {
                    if (DAY == 7)
                    {
                        WEEK += 1;
                        DAY = 0;

                        DaysArray[i].Text = month.Weeks[WEEK].Days[DAY].Day.ToString();

                        DAY += 1;
                    }
                    else
                    {
                        if (flg != true)
                        {
                            if (month.Weeks[WEEK].Days[DAY].Day.ToString() != "1") DaysArray[i].Text = "";
                            else
                            {
                                flg = true;
                                DaysArray[i].Text = month.Weeks[WEEK].Days[DAY].Day.ToString();
                            }
                        }
                        else DaysArray[i].Text = month.Weeks[WEEK].Days[DAY].Day.ToString();

                        DAY += 1;
                    }
                }
            }
            catch (Exception) { }

            for (i = 0; i < MAX; i++) DaysArray[i].ForeColor = Color.Black;

            /*------------------------
             祝日色付け
            ------------------------*/
            foreach (GetHoliday.NationalHoliday NationalHoliday2 in NationalHoliday)
            {
                for (i = 0; i < MAX; i++)
                {
                    if (DaysArray[i].Text != "")
                    {
                        /*------------------------
                         2桁補正
                        ------------------------*/
                        if (DaysArray[i].Text.Length != 2) tmpDay = "0" + DaysArray[i].Text;
                        else tmpDay = DaysArray[i].Text;

                        if (lblMonth.Text.Length != 2) tmpMonth = "0" + lblMonth.Text;
                        else tmpMonth = lblMonth.Text;

                        tmpDate = txtYear.Text + "/" + tmpMonth + "/" + tmpDay + " 0:00:00";

                        dt1 = new DateTime(2014, 1, 1, 0, 0, 0); //初期化

                        try
                        {
                            dt1 = DateTime.Parse(tmpDate);
                            if (string.Compare(tmpDate, NationalHoliday2.Date.ToString()) == 0) DaysArray[i].ForeColor = Color.Red; //祝日
                        }
                        catch (Exception) { }

                        if (string.Compare(dt1.ToString("ddd"), "土") == 0) DaysArray[i].ForeColor = Color.Blue; //土曜日
                        else if (string.Compare(dt1.ToString("ddd"), "日") == 0) DaysArray[i].ForeColor = Color.Red; //日曜日
                    }
                }
            }

            /*------------------------
             当日色付け
            ------------------------*/
            TodayDate = DateTime.Today.ToString("yyyy-MM-dd");

            for (i = 0; i < MAX; i++)
            {
                /*------------------------
                 2桁補正
                ------------------------*/
                if (DaysArray[i].Text.Length != 2) tmpDay = "0" + DaysArray[i].Text;
                else tmpDay = DaysArray[i].Text;

                if (lblMonth.Text.Length != 2) tmpMonth = "0" + lblMonth.Text;
                else tmpMonth = lblMonth.Text;

                Date = txtYear.Text + "-" + tmpMonth + "-";

                if (TodayDate == Date + tmpDay)
                {
                    DaysArray[i].BackColor = Color.LightPink;
                }
                else
                {
                    DaysArray[i].BackColor = Color.DarkGray;
                }
            }
        }

        /// <summary>
        /// カレンダー情報計算
        /// </summary>
        /// <param name="date">YYYY-MM-DD形式の文字列</param>
        /// <returns>指定した年月の日付情報</returns>
        public CalendarMonth GetCalendarMonth(String date)
        {
            //月の週のリスト
            List<CalendarWeek> weeks = new List<CalendarWeek>();

            //日付を管理する変数
            int day = 1;

            //対象の日付オブジェクトを取得する
            DateTime target = DateTime.Parse(date);

            //当月の日数を求める
            int daysInMonth = DateTime.DaysInMonth(target.Year, target.Month);

            //当月1日の曜日を求める
            DateTime firstDay = target.AddDays(-(target.Day - 1));
            int dayOfWeek = (int)firstDay.DayOfWeek;

            //前月の最終日を取得する
            DateTime prevMonth = target.AddMonths(-1);
            int daysInPrevMonth = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);

            //第一週のリストを作成する
            CalendarDay[] weekDays = new CalendarDay[7];

            //今月1日から週の終わりまでを配列にいれる
            for (int i = dayOfWeek; i < 7; i++, day++)
            {
                CalendarDay calDate = new CalendarDay();
                calDate.Year = target.Year;
                calDate.Month = target.Month;
                calDate.Day = day;
                weekDays[i] = calDate;
            }

            //前月月末から週の始めまでを配列に入れる
            for (int i = dayOfWeek - 1, x = daysInPrevMonth; i >= 0; i--, x--)
            {
                CalendarDay calDate = new CalendarDay();
                calDate.Year = prevMonth.Year;
                calDate.Month = prevMonth.Month;
                calDate.Day = x;
                weekDays[i] = calDate;
            }

            weeks.Add(new CalendarWeek(weekDays));

            //月末までの残りの日数を週毎にコレクションに入れる
            for (; day <= daysInMonth; )
            {
                weekDays = new CalendarDay[7];

                for (int i = 0; i < 7; i++)
                {
                    CalendarDay calDate = new CalendarDay();
                    calDate.Year = target.Year;
                    calDate.Month = target.Month;
                    calDate.Day = day;

                    weekDays[i] = calDate;
                    day++;

                    if (day > daysInMonth)
                    {
                        break;
                    }
                }

                weeks.Add(new CalendarWeek(weekDays));
            }

            //月に週を設定する
            CalendarMonth month = new CalendarMonth();
            month.Weeks = weeks.ToArray();

            return month;
        }

        /// <summary>
        /// マウス移動時の色変化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region マウス移動時の色変化
        private new void MouseMove(object sender, MouseEventArgs e)
        {
            ((Label)sender).BackColor = Color.FromArgb(0xE0, 0xE0, 0xE0);
        }

        private new void MouseLeave(object sender, EventArgs e)
        {
            if (ToolTip1.GetToolTip((Label)sender).ToString().IndexOf("メモ：NULL") > 0) ((Label)sender).BackColor = Color.FromKnownColor(KnownColor.Silver);
            else ((Label)sender).BackColor = Color.FromKnownColor(KnownColor.SkyBlue);
        }
        #endregion

        /// <summary>
        /// 各種表示切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            string ErrStr = "ERROR: ";
            string str = "";
            string str2 = "";
            string TodayDate;
            string tmpDay;
            string tmpMonth;
            string Date;

            TimeMoneyH.Text = ini.getValueString("HourlyWage", "Weekday");
            TimeMoneyK.Text = ini.getValueString("HourlyWage", "Holiday");
            txtClosingDay.Text = ini.getValueString("ClosingDay", "Day");

            /*------------------------
             エラー文出力
            ------------------------*/
            if (TimeMoneyH.Text == "0" || TimeMoneyK.Text == "0")
            {
                str = "時給未設定";
            }
            else
            {
                str = "";
            }
            if (txtClosingDay.Text == "0")
            {
                str2 = "締日未設定";
            }
            else
            {
                str2 = "";
            }

            if (str == "" && str2 == "")
            {
                StatErr.Text = "NOERROR";
                statusStrip1.BackColor = Color.DodgerBlue;
                panel2.BackColor = Color.DodgerBlue;
            }
            else if (str == "" && str2 != "")
            {
                ErrStr += str2;
                StatErr.Text = ErrStr;

                statusStrip1.BackColor = Color.Red;
                panel2.BackColor = Color.Red;
            }
            else if(str != "" && str2 == "")
            {
                ErrStr += str;
                StatErr.Text = ErrStr;

                statusStrip1.BackColor = Color.Red;
                panel2.BackColor = Color.Red;
            }
            else if (str != "" && str2 != "")
            {
                ErrStr += str + ", " + str2;
                StatErr.Text = ErrStr;

                statusStrip1.BackColor = Color.Red;
                panel2.BackColor = Color.Red;
            }

            /*------------------------
             暫定、確定の切り替え
            ------------------------*/
            //2ケタ補正
            if (txtClosingDay.Text.Length != 2) tmpDay = "0" + txtClosingDay.Text;
            else tmpDay = txtClosingDay.Text;

            if (lblMonth.Text.Length != 2) tmpMonth = "0" + lblMonth.Text;
            else tmpMonth = lblMonth.Text;

            Date = txtYear.Text + "-" + tmpMonth + "-" + tmpDay;

            TodayDate = DateTime.Today.ToString("yyyy-MM-dd");

            if (TodayDate.CompareTo(Date) == 1)
            {
                MoneyStatus.Text = "確定";
                MoneyStatus.ForeColor = Color.Red;
            }
            else
            {
                MoneyStatus.Text = "暫定";
                MoneyStatus.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// メニューバーの終了ボタン動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 数値固定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            //0～9と、バックスペース以外の時は、イベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b') e.Handled = true;
        }

        /// <summary>
        /// バージョン情報表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkManageソフトウェアのバージョン情報ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Soft_Information frm_Info = new Soft_Information();
            frm_Info.ShowDialog(this);
            frm_Info.Dispose();
        }

        /// <summary>
        /// 時給設定フォーム表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 時給設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoneySettings frm_moneySettings = new MoneySettings(this);

            timer1.Enabled = false;
            
            statusStrip1.BackColor = Color.DarkOrange;
            panel2.BackColor = Color.DarkOrange;
            frm_moneySettings.ShowDialog(this);
            frm_moneySettings.Dispose();
        }

        /// <summary>
        /// 給料の締日設定フォーム表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 給料締日設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClosingDaySettings frm_ClosingDay = new ClosingDaySettings(this);

            timer1.Enabled = false;
            
            statusStrip1.BackColor = Color.DarkOrange;
            panel2.BackColor = Color.DarkOrange;
            frm_ClosingDay.ShowDialog(this);
            frm_ClosingDay.Dispose();
        }

        /// <summary>
        /// データベース登録フォーム表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private new void Click(object sender, EventArgs e)
        {
            int i;

            if ((TimeMoneyH.Text == "0" || TimeMoneyK.Text == "0" || txtClosingDay.Text == "0") || (TimeMoneyH.Text == "" || TimeMoneyK.Text == "" || txtClosingDay.Text == ""))
            {
                MessageBox.Show(this, "時給の設定および締日の設定がされていない為登録・削除ができません\r\n上部メニューバーから、時給の設定を行ってください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                timer1.Enabled = false;
            
                //選択されたコントロールの番号を取得
                i = int.Parse((((Label)sender).Name).Substring(1)) - 1;

                if (lblArray[i].Text != "")
                {
                    statusStrip1.BackColor = Color.FromArgb(104, 33, 122);
                    panel2.BackColor = Color.FromArgb(104, 33, 122);

                    DialogResult result = MessageBox.Show("登録されているデータを削除しますか？",
                                                          "お知らせ",
                                                          MessageBoxButtons.YesNoCancel,
                                                          MessageBoxIcon.Exclamation,
                                                          MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Yes)
                    {
                        //はい
                        sql = "delete from Manage where CYear=" + txtYear.Text + " and CMonth=" + lblMonth.Text + " and CDay=" + DaysArray[i].Text;

                        timer1.Enabled = false;

                        using (conn = new SQLiteConnection(gDataSource))
                        {
                            try
                            {
                                conn.Open();

                                command = new SQLiteCommand(sql, conn);
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error : " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            finally
                            {
                                statusStrip1.BackColor = Color.DodgerBlue;
                                panel2.BackColor = Color.DodgerBlue;

                                conn.Close();
                                Clear_Calendar();
                                ToolTip_Reload();

                                GET_DB_Data();
                            }
                        }

                        timer1.Enabled = true;

                        return;
                    }
                    else if (result == DialogResult.No)
                    {
                        statusStrip1.BackColor = Color.DodgerBlue;
                        panel2.BackColor = Color.DodgerBlue;
                        return;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        statusStrip1.BackColor = Color.DodgerBlue;
                        panel2.BackColor = Color.DodgerBlue;
                        return;
                    }
                }
                else
                {
                    if (DaysArray[i].Text != "")
                    {
                        statusStrip1.BackColor = Color.FromArgb(104, 33, 122);
                        panel2.BackColor = Color.FromArgb(104, 33, 122);

                        TimeSet frm_DBset = new TimeSet(this, i, AccessKey);
                        frm_DBset.ShowDialog(this);
                        frm_DBset.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// ツールチップの更新
        /// </summary>
        public void ToolTip_Reload()
        {
            for (int i = 0; i < MAX; i++) ToolTip1.SetToolTip(lblArray[i], GET_DB_Data_About(lblMonth.Text, DaysArray[i].Text));
        }

        /// <summary>
        /// メニューバー各種ボタン切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region メニューバー各種ボタン切り替え
        private void btnWindowClose_MouseMove(object sender, MouseEventArgs e)
        {
            btnWindowClose.Image = WorkManage.Properties.Resources.WindowClose_On;
        }

        private void btnWindowClose_MouseLeave(object sender, EventArgs e)
        {
            btnWindowClose.Image = WorkManage.Properties.Resources.WindowClose;
        }

        private void btnWindowChange_MouseMove(object sender, MouseEventArgs e)
        {
            if (WindowFlg == false)
            {
                btnWindowChange.Image = WorkManage.Properties.Resources.WindowChange_Normal_On;
            }
            else if (WindowFlg == true)
            {
                btnWindowChange.Image = WorkManage.Properties.Resources.WindowChange_On;
            }
        }

        private void btnWindowChange_MouseLeave(object sender, EventArgs e)
        {
            if (WindowFlg == false)
            {
                btnWindowChange.Image = WorkManage.Properties.Resources.WindowChange_Normal;
            }
            else if (WindowFlg == true)
            {
                btnWindowChange.Image = WorkManage.Properties.Resources.WindowChange;
            }
        }

        private void btnWindowMin_MouseMove(object sender, MouseEventArgs e)
        {
            btnWindowMin.Image = WorkManage.Properties.Resources.WindowMin_On;
        }

        private void btnWindowMin_MouseLeave(object sender, EventArgs e)
        {
            btnWindowMin.Image = WorkManage.Properties.Resources.WindowMin;
        }
        #endregion

        /// <summary>
        /// メニューバー各種ボタンクリック時の動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region メニューバー各種ボタンクリック時の動作
        private void btnWindowClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnWindowChange_Click(object sender, EventArgs e)
        {
            if (WindowFlg == false)
            {
                int h, w;

                //元の大きさ・位置を記憶
                BWS.x = this.Size.Width;
                BWS.y = this.Size.Height;
                BWS.Location_x = this.Location.X;
                BWS.Location_y = this.Location.Y;

                //ディスプレイの作業領域の高さ
                h = System.Windows.Forms.Screen.GetWorkingArea(this).Height;
                //ディスプレイの作業領域の幅
                w = System.Windows.Forms.Screen.GetWorkingArea(this).Width;

                this.Location = new Point( System.Windows.Forms.Screen.GetWorkingArea(this).Left, System.Windows.Forms.Screen.GetWorkingArea(this).Top);
                this.Height = h;
                this.Width = w;

                btnWindowChange.Image = WorkManage.Properties.Resources.WindowChange;

                toolTip2.SetToolTip(btnWindowChange, "元に戻す");

                WindowFlg = true;
            }
            else if (WindowFlg == true)
            {
                // 元の大きさと位置に戻す
                this.Location = new Point(BWS.Location_x, BWS.Location_y);
                this.Height = BWS.y;
                this.Width = BWS.x;
                btnWindowChange.Image = WorkManage.Properties.Resources.WindowChange_Normal;

                toolTip2.SetToolTip(btnWindowChange, "最大化");

                WindowFlg = false;
            }
        }

        private void btnWindowMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion

        /// <summary>
        /// 最新バージョンの確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void アップデートの確認ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update_Check(1);
        }

        /// <summary>
        /// 上部ダブルクリックで最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            if (WindowFlg == false)
            {
                int h, w;

                //元の大きさ・位置を記憶
                BWS.x = this.Size.Width;
                BWS.y = this.Size.Height;
                BWS.Location_x = this.Location.X;
                BWS.Location_y = this.Location.Y;

                //ディスプレイの作業領域の高さ
                h = System.Windows.Forms.Screen.GetWorkingArea(this).Height;
                //ディスプレイの作業領域の幅
                w = System.Windows.Forms.Screen.GetWorkingArea(this).Width;

                this.Location = new Point(System.Windows.Forms.Screen.GetWorkingArea(this).Left, System.Windows.Forms.Screen.GetWorkingArea(this).Top);
                this.Height = h;
                this.Width = w;

                btnWindowChange.Image = WorkManage.Properties.Resources.WindowChange;

                toolTip2.SetToolTip(btnWindowChange, "元に戻す");

                WindowFlg = true;
            }
            else if (WindowFlg == true)
            {
                // 元の大きさと位置に戻す
                this.Location = new Point(BWS.Location_x, BWS.Location_y);
                this.Height = BWS.y;
                this.Width = BWS.x;
                btnWindowChange.Image = WorkManage.Properties.Resources.WindowChange_Normal;

                toolTip2.SetToolTip(btnWindowChange, "最大化");

                WindowFlg = false;
            }
        }

        /// <summary>
        /// 勤務先選択フォームの出力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 勤務先選択ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkDestination frm = new WorkDestination(this);

            timer1.Enabled = false;

            statusStrip1.BackColor = Color.DarkOrange;
            panel2.BackColor = Color.DarkOrange;
            frm.ShowDialog(this);
            frm.Dispose();
        }
    }
}
