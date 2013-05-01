using MahApps.Metro.Controls;
﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TimeEntry.Models;
using System.Windows.Input;
using System.Windows;

namespace TimeSummary.UI.WPF
{
    public class TimeEntryViewModel : ObservableObject
    {
        private string _timeEntryInput;
        private string _timeEntryOutput;

        public string TimeEntryInput
        {
            get { return _timeEntryInput; }
            set
            {
                base.Set( () => TimeEntryInput, ref _timeEntryInput, value );
            }
        }

        public string TimeEntryOutput
        {
            get { return _timeEntryOutput; }
            set
            {
                base.Set( () => TimeEntryOutput, ref _timeEntryOutput, value );
            }
        }

        public const string InputHelpText = "Paste Text Here";

        public ICommand ParseCommand { get; private set; }



        public TimeEntryViewModel()
        {
            this.ParseCommand = new RelayCommand( ParseCommandOnExecute, ParseCommandCanExecute );
            //this.TimeEntryInput = InputHelpText;
        }

        private bool ParseCommandCanExecute()
        {
            return true;
        }

        private void ParseContentsOfInputBox( List<TimeLineItem> lineItems )
        {
            foreach ( string timeEntry in this.TimeEntryInput.Split( '\n' ) )
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
        }

        public List<TimeSummaryItem> CreateTimeSummaryLists( List<TimeLineItem> lineItems )
        {
            var timeGroups = ( from li in lineItems
                               group li by li.ProjectName.ToUpper() into g
                               orderby g.Key
                               select new TimeSummaryItem { ProjectName = g.Key, TimeSpentInHours = g.Sum( li => li.TimeSpentInHours() ) } ).ToList();

            return timeGroups;
        }

        private void ParseCommandOnExecute()
        {
            string total = string.Empty;
            List<TimeLineItem> lineItems = new List<TimeLineItem>();

            this.ParseContentsOfInputBox( lineItems );

            StringBuilder outputString = new StringBuilder();

            var timeGroups = from li in lineItems
                             group li by li.ProjectName.ToUpper() into g
                             orderby g.Key
                             select new { UpperCaseProjectName = g.Key, TotalHours = g.Sum( li => li.TimeSpentInHours() ) };

            List<TimeSummaryItem> timeSummaryItems = this.CreateTimeSummaryLists( lineItems );

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


            this.TimeEntryOutput = outputString.ToString();

        }
    }
}
