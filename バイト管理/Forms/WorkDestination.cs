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
    public partial class WorkDestination : Form
    {
        private main f1;

        public WorkDestination(main f)
        {
            f1 = f;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WorkDestination_FormClosed(object sender, FormClosedEventArgs e)
        {
            f1.statusStrip1.BackColor = Color.DodgerBlue;
            f1.panel2.BackColor = Color.DodgerBlue;
            f1.timer1.Enabled = true;
        }
    }
}
