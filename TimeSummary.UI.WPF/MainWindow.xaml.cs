using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using TimeEntry.Models;

namespace TimeSummary.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnParse_Click( object sender, RoutedEventArgs e )
        {
            // Testing Logging
            //logger.Info( "Test" );

            string total = string.Empty;
            List<TimeLineItem> lineItems = new List<TimeLineItem>();

            foreach ( string timeEntry in txtInput.Text.Split( '\n' ) )
            {
                try
                {
                    if ( timeEntry != string.Empty && timeEntry != "\r" && timeEntry != "Paste Text Here\r" && timeEntry != "Paste Text Here" ) lineItems.Add( TimeLineItem.Parse( timeEntry ) );
                }
                catch
                {
                    MessageBox.Show( string.Format( "Couldn't parse: {0}", timeEntry ) );
                }

            }

            StringBuilder outputString = new StringBuilder();

            var timeGroups = from li in lineItems
                             group li by li.ProjectName.ToUpper() into g
                             orderby g.Key
                             select new { UpperCaseProjectName = g.Key, TotalHours = g.Sum( li => li.TimeSpentInHours() ) };



            foreach ( var li in timeGroups )
            {
                // output the total time spent for the day on a bucket
                outputString.AppendLine( string.Format( "{0}   {1:0.00} Hours", li.UpperCaseProjectName, li.TotalHours ) );

                var comments = from cli in lineItems
                               where cli.ProjectName.ToUpper() == li.UpperCaseProjectName && cli.Comment != string.Empty
                               select cli.Comment;

                // output the comments for each line item on a seperate line
                foreach ( var comment in comments )
                {
                    outputString.AppendLine( string.Format( "{0}", comment.Replace( '\r', ' ' ) ) );
                }

                outputString.AppendLine();
            }

            // Output a summary line
            outputString.AppendLine( "----------------------------------------------------------" );
            outputString.AppendLine( string.Format( "Total Time Worked: {0:0.00} Hours", timeGroups.Sum( x => x.TotalHours ) ) );


            txtOutput.Text = outputString.ToString();
        }

        private void txtHelp_Click( object sender, RoutedEventArgs e )
        {
            var helpWindow = new HelpWindow();
            helpWindow.Show();
        }
    }
}
