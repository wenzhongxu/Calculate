﻿using Calculate.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculate
{
    public partial class CalcMain : Form
    {
        public CalcMain()
        {
            InitializeComponent();
        }

        private void BtnCalc_Click(object sender, EventArgs e)
        {
            string expression = this.txtExe.Text;
            Calculator calc = new Calculator();
            string resultcalc = calc.Calc(expression); ;
            this.txtResult.Text = resultcalc;
        }
    }
}
