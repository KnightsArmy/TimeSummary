using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TimeEntry.Models
{
    public class TimeUtilities
    {
        private const string _TimePattern = @"(\d*\d)(:\d*\d)*\s*([ap]m)*";

        /// <summary>
        /// Any time without am or pm will be defaulted as follows
        /// assumes business day is less than 12 hours
        /// </summary>
        /// <param name="inputTime"></param>
        /// <param name="businessDayStartTime">Assumes time is before noon</param>
        /// <param name="businessDayEndTime">Assumes time is after noon</param>
        /// <returns></returns>
        public static string DefaultAMorPM( string inputTime, TimeSpan businessDayStartTime, TimeSpan businessDayEndTime )
        {
            TimeSpan noon = new TimeSpan( 12, 0, 0 );
            if ( businessDayStartTime > noon )
                throw new NotSupportedException( "Start of business day after 12:00 pm not supported." );

            if ( businessDayEndTime < noon )
                throw new NotSupportedException( "End of business day before 12:00 pm not supported." );

            if ( ( businessDayEndTime - businessDayStartTime ) > noon )
                throw new NotSupportedException( "More than a 12 hour workday not supported." );

            TimeSpan inputTimeSpan = TimeSpan.Parse( inputTime );

            double inputDouble = inputTimeSpan.TotalHours;

            //if ( IsTimeOfDayBetween( DateTime.Parse( inputTime ), businessDayStartTime, businessDayEndTime ) )
            //{
            if ( inputDouble == 12 )
                inputTime += " pm";
            else if ( inputDouble < 12 && inputDouble > businessDayStartTime.TotalHours )
                inputTime += " am";
            else
            {
                TimeSpan tempInputTimeSpan = inputTimeSpan + new TimeSpan( 12, 0, 0 );
                if ( tempInputTimeSpan < businessDayEndTime )
                {
                    inputTime += " pm";
                }
                else
                    throw new NotSupportedException( "Can't default am or pm, time isn't in the business day" );
            }

            return inputTime;
        }

        static public bool IsTimeOfDayBetween( DateTime time, TimeSpan startTime, TimeSpan endTime )
        {
            if ( endTime == startTime )
            {
                return true;
            }
            else if ( endTime < startTime )
            {
                return time.TimeOfDay <= endTime ||
                    time.TimeOfDay >= startTime;
            }
            else
            {
                return time.TimeOfDay >= startTime &&
                    time.TimeOfDay <= endTime;
            }
        }

        static public bool IsMilitaryTime( string time )
        {
            Regex timeEntryRegEx = new Regex( _TimePattern );
            Match m = timeEntryRegEx.Match( time );

            if ( m.Success )
            {
                if ( m.Groups[ 3 ].Value.Length > 0 ) // if they specified am/pm it's not military time
                    return false;
                else if ( int.Parse( m.Groups[ 1 ].Value ) > 12 ) // if they use 13 or greater for the hour it is military time
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        static public string AddMinutesField( string time )
        {
            Regex timeEntryRegEx = new Regex( _TimePattern );
            Match m = timeEntryRegEx.Match( time );

            if ( m.Success && m.Groups[ 2 ].Value.Length == 0 )
            {
                string hours = m.Groups[1].Value;
                string minutes = "00";
                string ampm = m.Groups[3].Value;

                time = string.Format( "{0}:{1}", hours, minutes );

                if ( ! string.IsNullOrEmpty( ampm ) )
                    time = string.Format( "{0} {1}", time, m.Groups[ 3 ].Value );
            }

            return time;
        }
    }
}
