using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SQLite;
using System.IO;

namespace WorkManage
{
    public partial class TimeSet : Form
    {
        /*------------------------
         定数宣言
        ------------------------*/
        private const int MAX = 42;
        private const string DATABASE_NAME = "Manage.db"; //データベースファイル名
        private const int ID = 0;
        private const int CYear = 1;
        private const int CMonth = 2;
        private const int CDay = 3;
        private const int HW = 4;
        private const int WST = 5;
        private const int WET = 6;
        private const int BST = 7;
        private const int BET = 8;

        private main f1;
        private int i;
        private int[] Holiday = new int[10] { 0, 6, 7, 13, 14, 20, 21, 27, 28, 34 }; //休日
        private GetHoliday.NationalHoliday[] NationalHoliday;  //祝日
        private string AccessKey; //GoogleカレンダーAPIのアクセスキー

        /*------------------------
         データベース関係
        ------------------------*/
        private string sql; //SQL文
        private string gDataBaseFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), DATABASE_NAME);
        private string gDataSource = "Data Source=" + Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), DATABASE_NAME) + ";Default Timeout=2;Synchronous=Off";
        private SQLiteCommand command;  // SQLコマンド

        public TimeSet(main f, int Ci, string Key)
        {
            f1 = f;
            i = Ci;
            AccessKey = Key;

            InitializeComponent();
        }

        private void TimeSet_Load(object sender, EventArgs e)
        {
            this.Top = f1.Top + (f1.Height - this.Height) / 2;
            this.Left = f1.Left + (f1.Width - this.Width) / 2;

            //現在年の日本の祝日を取得する
            NationalHoliday = GetHoliday.GetNationalHolidays(int.Parse(DateTime.Today.ToString("yyyy")), AccessKey, f1.LocalHoliday);
        }

        /// <summary>
        /// データベースへ勤務情報を登録
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string memo_str = "NULL";
            string tmpDate;
            string tmpDay;
            string tmpMonth;
            bool flg = false;
            DateTime cmpWorkStartTime;
            DateTime cmpWorkEndTime;
            DateTime cmpBreakStartTime;
            DateTime cmpBreakEndTime;

            DateTime dt1 = new DateTime(2014, 1, 1, 0, 0, 0); //初期化

            if ((cmbWST_h.Text == "" || cmbWST_m.Text == "")
                    || (cmbWET_h.Text == "" || cmbWET_m.Text == "")
                    || (cmbBST_h.Text == "" || cmbBST_m.Text == "")
                    || (cmbBET_h.Text == "" || cmbBET_m.Text == ""))
            {
                MessageBox.Show(this, "入力に誤りがあります", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                cmpWorkStartTime = DateTime.Parse(f1.lblYear.Text + "/" + f1.lblMonth.Text + "/" + f1.DaysArray[i].Text + " " + cmbWST_h.Text + ":" + cmbWST_m.Text + ":00");
                cmpWorkEndTime = DateTime.Parse(f1.lblYear.Text + "/" + f1.lblMonth.Text + "/" + f1.DaysArray[i].Text + " " + cmbWET_h.Text + ":" + cmbWET_m.Text + ":00");
                cmpBreakStartTime = DateTime.Parse(f1.lblYear.Text + "/" + f1.lblMonth.Text + "/" + f1.DaysArray[i].Text + " " + cmbBST_h.Text + ":" + cmbBST_m.Text + ":00");
                cmpBreakEndTime = DateTime.Parse(f1.lblYear.Text + "/" + f1.lblMonth.Text + "/" + f1.DaysArray[i].Text + " " + cmbBET_h.Text + ":" + cmbBET_m.Text + ":00");

                if ((cmpWorkStartTime.CompareTo(cmpWorkEndTime) == 1)
                    || (cmpBreakStartTime.CompareTo(cmpBreakEndTime) == 1))
                {
                    MessageBox.Show(this, "入力に誤りがあります", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    //指定年の日本の祝日を取得する
                    NationalHoliday = GetHoliday.GetNationalHolidays(int.Parse(f1.lblYear.Text), AccessKey, f1.LocalHoliday);

                    foreach (GetHoliday.NationalHoliday NationalHoliday2 in NationalHoliday)
                    {
                        if (f1.DaysArray[i].Text != "")
                        {
                            //2ケタ補正
                            if (f1.DaysArray[i].Text.Length != 2) tmpDay = "0" + f1.DaysArray[i].Text;
                            else tmpDay = f1.DaysArray[i].Text;

                            if (f1.lblMonth.Text.Length != 2) tmpMonth = "0" + f1.lblMonth.Text;
                            else tmpMonth = f1.lblMonth.Text;

                            tmpDate = f1.lblYear.Text + "/" + tmpMonth + "/" + tmpDay + " 0:00:00";

                            try
                            {
                                dt1 = DateTime.Parse(tmpDate);
                                if (string.Compare(tmpDate, NationalHoliday2.Date.ToString()) == 0) flg = true; //祝日       
                            }
                            catch (Exception) { }

                                             
                        }
                    }

                    //2ケタ補正
                    if (f1.DaysArray[i].Text.Length != 2) tmpDay = "0" + f1.DaysArray[i].Text;
                    else tmpDay = f1.DaysArray[i].Text;

                    if (f1.lblMonth.Text.Length != 2) tmpMonth = "0" + f1.lblMonth.Text;
                    else tmpMonth = f1.lblMonth.Text;

                    if (txtMemo.Text == "") memo_str = "NULL";
                    else memo_str = "\r\n" + txtMemo.Text;

                    if (flg == true)
                    {
                        sql = "insert into Manage (CDate, CYear, CMonth, CDay, HourlyWage, WorkStartTime, WorkEndTime, BreakStartTime, BreakEndTime, Memo) values ("
                                + "'" + f1.lblYear.Text + "-" + tmpMonth + "-" + tmpDay + "', "
                                + f1.lblYear.Text + ", "
                                + f1.lblMonth.Text + ", "
                                + f1.DaysArray[i].Text + ", "
                                + f1.TimeMoneyK.Text + ", "
                                + "'" + cmbWST_h.Text + ":" + cmbWST_m.Text + ":00', "
                                + "'" + cmbWET_h.Text + ":" + cmbWET_m.Text + ":00', "
                                + "'" + cmbBST_h.Text + ":" + cmbBST_m.Text + ":00', "
                                + "'" + cmbBET_h.Text + ":" + cmbBET_m.Text + ":00', "
                                + "'" + memo_str + "')";
                    }
                    else if (string.Compare(dt1.ToString("ddd"), "土") == 0) //土曜日
                    {
                        sql = "insert into Manage (CDate, CYear, CMonth, CDay, HourlyWage, WorkStartTime, WorkEndTime, BreakStartTime, BreakEndTime, Memo) values ("
                                + "'" + f1.lblYear.Text + "-" + tmpMonth + "-" + tmpDay + "', "
                                + f1.lblYear.Text + ", "
                                + f1.lblMonth.Text + ", "
                                + f1.DaysArray[i].Text + ", "
                                + f1.TimeMoneyK.Text + ", "
                                + "'" + cmbWST_h.Text + ":" + cmbWST_m.Text + ":00', "
                                + "'" + cmbWET_h.Text + ":" + cmbWET_m.Text + ":00', "
                                + "'" + cmbBST_h.Text + ":" + cmbBST_m.Text + ":00', "
                                + "'" + cmbBET_h.Text + ":" + cmbBET_m.Text + ":00', "
                                + "'" + memo_str + "')";
                    }
                    else if (string.Compare(dt1.ToString("ddd"), "日") == 0) //日曜日
                    {
                        sql = "insert into Manage (CDate, CYear, CMonth, CDay, HourlyWage, WorkStartTime, WorkEndTime, BreakStartTime, BreakEndTime, Memo) values ("
                                + "'" + f1.lblYear.Text + "-" + tmpMonth + "-" + tmpDay + "', "
                                + f1.lblYear.Text + ", "
                                + f1.lblMonth.Text + ", "
                                + f1.DaysArray[i].Text + ", "
                                + f1.TimeMoneyK.Text + ", "
                                + "'" + cmbWST_h.Text + ":" + cmbWST_m.Text + ":00', "
                                + "'" + cmbWET_h.Text + ":" + cmbWET_m.Text + ":00', "
                                + "'" + cmbBST_h.Text + ":" + cmbBST_m.Text + ":00', "
                                + "'" + cmbBET_h.Text + ":" + cmbBET_m.Text + ":00', "
                                + "'" + memo_str + "')";
                    }
                    else
                    {
                        sql = "insert into Manage (CDate, CYear, CMonth, CDay, HourlyWage, WorkStartTime, WorkEndTime, BreakStartTime, BreakEndTime, Memo) values ("
                                + "'" + f1.lblYear.Text + "-" + tmpMonth + "-" + tmpDay + "', "
                                + f1.lblYear.Text + ", "
                                + f1.lblMonth.Text + ", "
                                + f1.DaysArray[i].Text + ", "
                                + f1.TimeMoneyH.Text + ", "
                                + "'" + cmbWST_h.Text + ":" + cmbWST_m.Text + ":00', "
                                + "'" + cmbWET_h.Text + ":" + cmbWET_m.Text + ":00', "
                                + "'" + cmbBST_h.Text + ":" + cmbBST_m.Text + ":00', "
                                + "'" + cmbBET_h.Text + ":" + cmbBET_m.Text + ":00', "
                                + "'" + memo_str + "')";
                    }

                    f1.timer1.Enabled = false;

                    using (SQLiteConnection conn = new SQLiteConnection(gDataSource))
                    {
                        try
                        {
                            conn.Open();

                            command = new SQLiteCommand(sql, conn);
                            command.ExecuteNonQuery();

                            f1.ToolTip_Reload();
                            f1.timer1.Enabled = true;

                            Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "Error : " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// メインフォームのステータスバーの色を変える
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            f1.GET_DB_Data();

            f1.statusStrip1.BackColor = Color.DodgerBlue;
            f1.panel2.BackColor = Color.DodgerBlue;
            f1.timer1.Enabled = true;
        }
    }
}
