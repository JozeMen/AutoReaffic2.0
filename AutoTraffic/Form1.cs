﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTraffic
{
    public partial class Form1 : Form
    {
        Modeling form2;
        private int CountWays;
        private int CountLines;
        private string roadType;
        private bool isTonnel = false;

        public int getCountWays
        {
            get { return CountWays; }
        }

        public int getCountLines
        {
            get { return CountLines; }
        }
        public Form1()
        {
            InitializeComponent();
            numericUpDownWay.Minimum = 1;
            numericUpDownWay.Maximum = 2;
            numericUpDownWay.ReadOnly = true;
            numericUpDownLines.Minimum = 2;
            numericUpDownLines.Maximum = 4;
            numericUpDownLines.ReadOnly = true;
/*            numericUpDownLights.Minimum = 0;
            numericUpDownLights.Maximum = 2;
            numericUpDownLights.ReadOnly = true;
*/            comboBox1.Items.AddRange(new string[] { "город", "загород", "тоннель" });
            comboBox1.SelectedItem = "город";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*if (comboBox1.item)
            {
                var stringNumber = "123";
                int numericValue;
                bool isNumber = int.TryParse(stringNumber, out numericValue);
            }*/

            CountLines = (int)numericUpDownLines.Value;
            CountWays = (int)numericUpDownWay.Value;
            roadType = comboBox1.Text;

            form2 = new Modeling();
            int time = -1;
            form2.setCountLines = CountLines;
            form2.setCountWays = CountWays;
            form2.setRoaType = roadType;
            //form2.setTimeTrafficLight = (string.IsNullOrEmpty(textBoxLights.Text))
            //    ? 0
            //    : int.TryParse(textBoxLights.Text, out time);
            // : int.Parse(textBoxLights.Text);
            form2.setTimeTrafficLight = (int)numericUpDownTonnel.Value;
            //if (string.IsNullOrEmpty(textBoxLights.Text))
            //{
            //    form2.setTimeTrafficLight = 0;
            //}
            //else
            //{
            //    if (int.TryParse(textBoxLights.Text, out time))
            //    {
            //        form2.setTimeTrafficLight = time;
            //    }
            //    else 
            //    {
            //        form2.setTimeTrafficLight = 0;
            //    }
                
            //}
            form2.ShowDialog();
        }
        private void pictureBoxDev_MouseEnter (object sender, EventArgs e)
        {
            MessageBox.Show("Создатели: \nПоздеев Сергей \nЖучков Константин \nПетропавловская Антонина", "О разработчиках", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }
        private void pictureBoxSys_MouseEnter(object sender, EventArgs e)
        {
            MessageBox.Show("Система моделирования движения транспорта \nна автодороге (в тоннеле / на автостраде) ", "О системе", MessageBoxButtons.OK, MessageBoxIcon.Question);
            
        }




        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "тоннель")
            {
                label2.Visible = false;
                label3.Visible = false;
                numericUpDownLines.Visible = false;
                numericUpDownWay.Visible = false;
                labelLights.Visible = true;
                numericUpDownTonnel.Visible = true;
                //MessageBox.Show("ДА!");
            }
            else
            {
                label2.Visible = true;
                label3.Visible = true;
                numericUpDownLines.Visible = true;
                numericUpDownWay.Visible = true;
                labelLights.Visible = false;
                numericUpDownTonnel.Visible = false;
            }
        }
    }
}
