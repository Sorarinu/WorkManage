using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WorkManage;

namespace WorkManage
{
    public partial class MoneySettings : Form
    {
        private InifileUtils ini = new InifileUtils();
        private main f1;

        public MoneySettings(main f)
        {
            f1 = f;
            InitializeComponent();
        }

        /// <summary>
        /// iniファイルに時給を書き込む
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtH.Text == "0" || txtK.Text == "")
            {
                MessageBox.Show(this, "0円は登録できません\r\n1円以上で入力してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                //書き込み
                ini.setValue("HourlyWage", "Weekday", txtH.Text);
                ini.setValue("HourlyWage", "Holiday", txtK.Text);
            }
            Close();
        }

        /// <summary>
        /// テキストボックスの初期値を設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoneySettings_Load(object sender, EventArgs e)
        {
            txtH.Text = ini.getValueString("HourlyWage", "Weekday");
            txtK.Text = ini.getValueString("HourlyWage", "Holiday");
        }

        /// <summary>
        /// 数値固定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtH_KeyPress(object sender, KeyPressEventArgs e)
        {
            //0～9と、バックスペース以外の時は、イベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private void txtK_KeyPress(object sender, KeyPressEventArgs e)
        {
            //0～9と、バックスペース以外の時は、イベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// メインフォームのステータスバーの色を変える
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoneySettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            f1.statusStrip1.BackColor = Color.DodgerBlue;
            f1.panel2.BackColor = Color.DodgerBlue;
            f1.timer1.Enabled = true;
        }
    }
}
