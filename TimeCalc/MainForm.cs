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
            if (unknownFormat.ToLower().EndsWith("p") || unknownFormat.ToLower().EndsWith("pm")) 
            {
                string stripped = unknownFormat;

                if (unknownFormat.ToLower().EndsWith("p"))
                {
                    stripped = unknownFormat.Substring(
                        0, unknownFormat.Length - 1); // remove 1
                }
                else if (unknownFormat.ToLower().EndsWith("pm"))
                {
                    stripped = unknownFormat.Substring(
                        0, unknownFormat.Length - 2); // remove 2
                }

                string[] split = stripped.Split(':');
                int hours = int.Parse(split[0]);
                int minutes = 0;

                if (split.Length > 1)
                {
                    minutes = int.Parse(split[1]);
                }

                // there's some confusion about the number 12; most people think like this:
                //   12pm = 12:00 -- only possible case
                //   12am = 00:00
                //   12 = 12:00
                int twentyFourHour = (hours == 12) ? 12 : hours + 12;

                return string.Format("{0}:{1}", twentyFourHour.ToString(), minutes.ToString());
            }
            else
            {
                string stripped = unknownFormat;

                if (unknownFormat.ToLower().EndsWith("a"))
                {
                    stripped = unknownFormat.Substring(
                        0, unknownFormat.Length - 1); // remove 1
                }
                else if (unknownFormat.ToLower().EndsWith("am"))
                {
                    stripped = unknownFormat.Substring(
                        0, unknownFormat.Length - 2); // remove 2
                }

                string[] split = stripped.Split(':');
                int hours = int.Parse(split[0]);
                int minutes = 0;

                if (split.Length > 1)
                {
                    minutes = int.Parse(split[1]);
                }

                bool hasAmAffix = unknownFormat.ToLower().EndsWith("a") 
                    || unknownFormat.ToLower().EndsWith("am");

                // there's some confusion about the number 12; most people think like this:
                //   12pm = 12:00
                //   12am = 00:00 -- possible case
                //   12 = 12:00 -- possible case
                if ((hours == 12) && hasAmAffix)
                {
                    hours = 0;
                }

                return string.Format("{0}:{1}", hours.ToString(), minutes.ToString());
            }
        }

        private void calculate()
        {
            string start = toTwentyFourHour(startTextBox.Text);
            string end = toTwentyFourHour(endTextBox.Text);

            string[] startSplit = start.Split(':');
            string[] endSplit = end.Split(':');

            int startHours = int.Parse(startSplit[0]);
            int endHours = int.Parse(endSplit[0]);

            int startMins = 0;
            if (startSplit.Length > 1)
            {
                startMins = int.Parse(startSplit[1]);
            }

            int endMins = 0;
            if (endSplit.Length > 1)
            {
                endMins = int.Parse(endSplit[1]);
            }

            int resultHours;
            if (startHours > endHours) // clock elapsed?
            {
                resultHours = (24 - startHours) + endHours;
            }
            else
            {
                resultHours = endHours - startHours;
            }

            // merge mins and hours for accurate minute translation
            int resultMins = (resultHours * 60) + (endMins - startMins);

            // convert mins to hours, and have a remainder of mins
            resultHours = 0;
            while (resultMins >= 60) {
                resultHours++;
                resultMins -= 60;
            }

            hoursTextBox.Text = string.Format(
                "{0}:{1}",
                resultHours.ToString(),
                resultMins.ToString().PadLeft(2, '0'));

            // also yield a weird decimal version of mins
            int resultMinsDec = (int)(((float)resultMins / 60) * 10);

            decTextBox.Text = resultHours.ToString();
            if (resultMinsDec != 0)
            {
                decTextBox.Text += "." + resultMinsDec;
            }
        }

        private void startTextBox_Enter(object sender, EventArgs e)
        {
            startTextBox.SelectAll();
        }

        private void endTextBox_Enter(object sender, EventArgs e)
        {
            endTextBox.SelectAll();
        }
    }
}
