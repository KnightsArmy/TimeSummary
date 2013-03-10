using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using TimeEntry.Models;

namespace TimeSummary
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            string total = string.Empty;
            List<TimeLineItem> lineItems = new List<TimeLineItem>();

            foreach ( string timeEntry in txtInput.Text.Split( '\n' ) )
            {
                try
                {
                    lineItems.Add( TimeLineItem.Parse( timeEntry ) );
                }
                catch
                {
                    MessageBox.Show( string.Format( "Couldn't parse: {0}", timeEntry ) );
                }

            }

            StringBuilder outputString = new StringBuilder();

            foreach (TimeLineItem tli in lineItems)
            {
                outputString.AppendLine(tli.ToString());
            }

            txtOutput.Text = outputString.ToString();
        }
    }
}
