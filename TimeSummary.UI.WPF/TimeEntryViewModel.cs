using MahApps.Metro.Controls;
﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TimeEntry.Models;
using System.Windows.Input;

namespace TimeSummary.UI.WPF
{
    public class TimeEntryViewModel : ObservableObject
    {
        private string _timeEntryInput;
        private string _timeEntryOutput;
        private List<TimeLineItem> _timeLineItems;
        private List<TimeSummaryItem> _timeSummaryItems;

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

        public List<TimeLineItem> TimeLineItems
        {
            get { return _timeLineItems; }
            set
            {
                base.Set( () => TimeLineItems, ref _timeLineItems, value );
            }
        }

        public List<TimeSummaryItem> TimeSummaryItems
        {
            get { return _timeSummaryItems; }
            set
            {
                base.Set( () => TimeSummaryItems, ref _timeSummaryItems, value );
            }
        }

        public ICommand ParseCommand { get; private set; }
        public ICommand CopyCommentToClipboardCommand { get; private set; }

        public TimeEntryViewModel()
        {
            this.ParseCommand = new RelayCommand( ParseCommandOnExecute, ParseCommandCanExecute );
            this.CopyCommentToClipboardCommand = new RelayCommand<string>( CopyCommentToClipboardCommandOnExecute, CopyCommentToClipboardCommandCanExecute );
            this.TimeLineItems = new List<TimeLineItem>();
            this.TimeSummaryItems = new List<TimeSummaryItem>();
        }

        public bool CopyCommentToClipboardCommandCanExecute( string projectName )
        {
            return true;
        }

        public void CopyCommentToClipboardCommandOnExecute( string projectName )
        {
            Clipboard.Clear();

            var comments =  ( from project in this.TimeSummaryItems
                            where project.ProjectName == projectName
                            from comment in project.Comments
                            select comment ).Distinct();

            StringBuilder commentText = new StringBuilder();

            foreach ( var comment in comments )
            {
                if ( comment != null && comment != string.Empty)
                  commentText.AppendLine( comment.ToString() );
            }

            Clipboard.SetText( commentText.ToString().Trim() );
        }

        private bool ParseCommandCanExecute()
        {
            return true;
        }

        private void ParseContentsOfInputBox()
        {
            foreach ( string timeEntry in this.TimeEntryInput.Split( '\n' ) )
            {
                try
                {
                    if ( timeEntry != string.Empty && timeEntry != "\r" ) this.TimeLineItems.Add( TimeLineItem.Parse( timeEntry ) );
                }
                catch
                {
                    MessageBox.Show( string.Format( "Couldn't parse: {0}", timeEntry ) );
                }
            }
        }

        public void CreateTimeSummaryLists()
        {
            this.TimeSummaryItems = ( from li in this.TimeLineItems
                                      group li by li.ProjectName.ToUpper() into g
                                      orderby g.Key
                                      select new TimeSummaryItem { ProjectName = g.Key, TimeSpentInHours = g.Sum( li => li.TimeSpentInHours() ) } ).ToList();

            this.AddCommentsToSummaryItems();
        }

        private void ResetTimeEntries()
        {
            this.TimeLineItems.Clear();
            this.TimeSummaryItems.Clear();
        }

        public void AddCommentsToSummaryItems()
        {
            foreach ( var li in this.TimeSummaryItems )
            {
                var comments = ( from cli in this.TimeLineItems
                               where cli.ProjectName.ToUpper() == li.ProjectName.ToUpper() && cli.Comment != string.Empty
                               select cli.Comment ).Distinct();

                // output the comments for each line item on a seperate line
                foreach ( var comment in comments )
                {
                    li.Comments.Add( comment );
                }
            }
        }

        public string FormatOutput()
        {
            StringBuilder outputString = new StringBuilder();

            //foreach ( var li in this.TimeSummaryItems )
            //{
                // output the total time spent for the day on a bucket
                //outputString.AppendLine( string.Format( "{0}   {1:0.00} Hours", li.ProjectName, li.TimeSpentInHours ) );

                // output the comments for each line item on a seperate line
                //foreach ( var comment in li.Comments )
                //{
                //    outputString.AppendLine( string.Format( "{0}", comment.Replace( '\r', ' ' ) ) );
                //}

                //outputString.AppendLine();
            //}

            // Output a summary line
            //outputString.AppendLine( "----------------------------------------------------------" );
            outputString.AppendLine( string.Format( "Total Time Worked: {0:0.00} Hours", this.TimeSummaryItems.Sum( x => x.TimeSpentInHours ) ) );

            return outputString.ToString();
        }

        private void ParseCommandOnExecute()
        {
            this.ResetTimeEntries();
            this.ParseContentsOfInputBox();
            this.CreateTimeSummaryLists();
            this.TimeEntryOutput = this.FormatOutput();
        }
    }
}
