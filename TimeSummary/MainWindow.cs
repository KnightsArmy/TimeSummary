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
               outputString.AppendLine( string.Format( "{0,-5}{1:0.00} Hours", li.ProjectName, li.TotalHours ) );

               var comments = from cli in lineItems
                              where cli.ProjectName == li.ProjectName
                              select cli.Comment;

               foreach ( var comment in comments )
               {
                   outputString.AppendLine( string.Format( "     {0}", comment ) );
               }
            }

            txtOutput.Text = outputString.ToString();
        }

    }
}
