using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimeCalc
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void calcButton_Click(object sender, EventArgs e)
        {
            try
            {
                calculate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Oops! {0}", ex.Message),
                    "It broke!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string toTwentyFourHour(string unknownFormat)
        {
            if (unknownFormat.ToLower().EndsWith("pm")) 
            {
                string[] split = unknownFormat.Split(':');
                string minutes = split[1].Substring(
                    0, split[1].Length - 2);
                string tweleveHourString = split[0];
                int twentyFourHour = int.Parse(tweleveHourString) + 12;
                return string.Format(
                    "{0}:{1}",
                    twentyFourHour.ToString().PadLeft(2, '0'),
                    minutes.PadLeft(2, '0'));
            }
            else if (unknownFormat.ToLower().EndsWith("am")) 
            {
                // remove 'am' so it's now 24 hr
                return unknownFormat.Substring(
                    0, unknownFormat.Length - 2);
            }
            else
            {
                // guess 24 hour
                return unknownFormat;
            }
        }

        private void calculate()
        {
            startTextBox.Text = toTwentyFourHour(startTextBox.Text);
            endTextBox.Text = toTwentyFourHour(endTextBox.Text);

            string[] startSplit = startTextBox.Text.Split(':');
            string[] endSplit = endTextBox.Text.Split(':');

            int startHours = int.Parse(startSplit[0]);
            int startMins = int.Parse(startSplit[1]);
            int endHours = int.Parse(endSplit[0]);
            int endMins = int.Parse(endSplit[1]);

            int resultHours;
            if (startHours > endHours) // clock elapsed?
            {
                resultHours = (24 - startHours) + endHours;
            }
            else
            {
                resultHours = startHours - endHours;
            }

            // merge mins and hours for accurate minute translation
            int resultMins = (resultHours * 60) + (startMins - endMins);

            // convert mins to hours, and have a remainder of mins
            resultHours = 0;
            while (resultMins >= 60) {
                resultHours++;
                resultMins -= 60;
            }

            hoursTextBox.Text = string.Format(
                "{0}:{1}",
                resultHours.ToString(),
                resultMins.ToString());

            // also yield a weird decimal version of mins
            int resultMinsDec = (int)(((float)resultMins / 60) * 10);

            decTextBox.Text = string.Format(
                "{0}.{1}",
                resultHours.ToString(),
                resultMinsDec);
        }
    }
}
