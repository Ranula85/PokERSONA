using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TurnFights
{
    public partial class HelpInfo : Form
    {
        public HelpInfo()
        {
            InitializeComponent();
        }
        private void mapCircleBtt_Click(object sender, EventArgs e)
        {
            circleImg.Visible = true; tableImg.Visible = false; mapCircleBtt.Enabled = false; mapTableBtt.Enabled = true;
        }

        private void mapTableBtt_Click(object sender, EventArgs e)
        {
            circleImg.Visible = false; tableImg.Visible = true; mapCircleBtt.Enabled = true; mapTableBtt.Enabled = false;
        }
    }
}