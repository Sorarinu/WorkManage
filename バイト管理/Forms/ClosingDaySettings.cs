using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkManage
{
    public partial class ClosingDaySettings : Form
    {
        private InifileUtils ini = new InifileUtils();
        private main f1;

        public ClosingDaySettings(main f)
        {
            f1 = f;
            InitializeComponent();
        }

        /// <summary>
        /// iniファイルへ給料締日を書き出す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtClosingDay.Text == "0")
            {
                MessageBox.Show(this, "0日は登録できません\r\n1日以上で入力してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else ini.setValue("ClosingDay", "Day", txtClosingDay.Text); //書き込み

            Close();
        }

        /// <summary>
        /// テキストボックスの初期値を設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosingDaySettings_Load(object sender, EventArgs e)
        {
            txtClosingDay.Text = ini.getValueString("ClosingDay", "Day");
        }

        /// <summary>
        /// 数値固定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtClosingDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            //0～9と、バックスペース以外の時は、イベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b') e.Handled = true;
        }

        /// <summary>
        /// メインフォームのステータスバーの色を変える
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosingDaySettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            f1.statusStrip1.BackColor = Color.DodgerBlue;
            f1.panel2.BackColor = Color.DodgerBlue;
            f1.timer1.Enabled = true;
        }
    }
}
