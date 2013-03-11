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

        private void btnCompute_Click( object sender, EventArgs e )
        {
            string total = string.Empty;
            List<TimeLineItem> lineItems = new List<TimeLineItem>();

            foreach ( string timeEntry in txtInput.Text.Split( '\n' ) )
            {
                try
                {
                    if ( timeEntry != string.Empty && timeEntry != "\r" ) lineItems.Add( TimeLineItem.Parse( timeEntry ) );
                }
                catch
                {
                    MessageBox.Show( string.Format( "Couldn't parse: {0}", timeEntry ) );
                }

            }

            StringBuilder outputString = new StringBuilder();

            var timeGroups = from li in lineItems
                             group li by li.ProjectName into g
                             orderby g.Key
                             select new { ProjectName = g.Key, TotalHours = g.Sum( li => li.TimeSpentInHours() ) };



            foreach ( var li in timeGroups )
            {
                // output the total time spent for the day on a bucket
                outputString.AppendLine( string.Format( "{0,-5}{1:0.00} Hours", li.ProjectName, li.TotalHours ) );

                var comments = from cli in lineItems
                               where cli.ProjectName == li.ProjectName && cli.Comment != string.Empty
                               select cli.Comment;

                // output the comments for each line item on a seperate line
                foreach ( var comment in comments )
                {
                    outputString.AppendLine( string.Format( "          {0}", comment ) );
                }
            }

            // Output a summary line
            outputString.AppendLine( "----------------------------------------------------------" );
            outputString.AppendLine( string.Format( "Total Time Worked: {0:0.00} Hours", timeGroups.Sum( x => x.TotalHours ) ) );


            txtOutput.Text = outputString.ToString();
        }

    }
}
