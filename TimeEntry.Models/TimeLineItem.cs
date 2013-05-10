using System;
using System.Text.RegularExpressions;

namespace TimeEntry.Models
{
    public class TimeLineItem
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger ( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region Constants
        private const string StartTimeGroup = "startTime";
        private const string EndTimeGroup = "endTime";
        private const string ProjectGroup = "projectName";
        private const string CommentGroup = "comment";
        private const string TimePattern = @"\d*\d(:\d*\d)*\s*([ap]m)*";
        private static string TimeLineEntryPattern = string.Format( @"(?<{1}>{0})\s*-\s*(?<{2}>{0})\s+(?<{3}>\S+)\s*(?<{4}>.*)$",
            TimePattern,
            StartTimeGroup,
            EndTimeGroup,
            ProjectGroup,
            CommentGroup );
        #endregion

        #region Properties

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ProjectName { get; set; }
        public string Comment { get; set; }

        #endregion

        #region Members

        public double TimeSpentInHours()
        {
            double totalMinutes = 0.0;

            if ( this.EndTime > this.StartTime )  // Same Day Task
            {
                totalMinutes = ( this.EndTime - this.StartTime ).TotalMinutes;
            }
            else // Task must have crossed over midnight
            {
                DateTime midnight = DateTime.Parse( "12:00 am" );

                // Since the user isn't entering days and only times in the controls we have to do some funky math
                double minutesBeforeMidnight = ( 24.0 * 60.0 ) + ( midnight - this.StartTime ).TotalMinutes;
                double minutesAfterMidnight  = ( this.EndTime - midnight ).TotalMinutes;

                totalMinutes = minutesBeforeMidnight + minutesAfterMidnight;
            }

            return totalMinutes / 60.0;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return string.Format( "Project Name: {0} StartTime: {1} EndTime: {2} Comment: {3}",
                this.ProjectName,
                this.StartTime.ToShortTimeString(),
                this.EndTime.ToShortTimeString(),
                this.Comment );
        }

        #endregion

        #region Static Members

        public static TimeLineItem Parse( string timeLineEntry )
        {
            logger.Info( string.Format( "timeLineEntry: '{0}'", timeLineEntry ) );

            Regex timeEntryRegEx = new Regex( TimeLineEntryPattern );
            Match m = timeEntryRegEx.Match( timeLineEntry );

            if ( m.Success )
            {

                logger.Info( "Match Success" );

                // The user may have only entered 8 as the time and not 8:00. Until I can figure out how to do that in the DateTime.Parse
                // method just do a low tech convert here.
                string startTime = m.Groups[ StartTimeGroup ].Value;
                string endTime = m.Groups[ EndTimeGroup ].Value;
                string projectName = m.Groups[ ProjectGroup ].Value;
                string comment = m.Groups[ CommentGroup ].Value;

                logger.Info( string.Format( "startTime: '{0}'", startTime ) );
                logger.Info( string.Format( "endTime: '{0}'", endTime ) );
                logger.Info( string.Format( "projectName: '{0}'", projectName ) );
                logger.Info( string.Format( "comment: '{0}'", comment ) );

                startTime = TimeUtilities.AddMinutesField( startTime );
                endTime = TimeUtilities.AddMinutesField( endTime );

                // if neither time is military time then we need to default am and pm
                if ( ! TimeUtilities.IsMilitaryTime( startTime ) && ! TimeUtilities.IsMilitaryTime( endTime ) )
                {
                    TimeUtilities.DefaultAMorPM( ref startTime, ref endTime );
                }

                TimeLineItem tli = new TimeLineItem()
                {
                    StartTime = DateTime.Parse( startTime ),
                    EndTime = DateTime.Parse( endTime ),
                    ProjectName = projectName,
                    Comment = comment.Trim()
                };

                logger.Info( tli.ToString() );

                return tli;
            }
            else
                throw new FormatException( "String was not recognized as a valid TimeLineItem." );
        }
        #endregion
    }
}
