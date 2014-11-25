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
        private const string _TimePattern = @"(\d*\d):*(\d*\d)*\s*([ap]m)*";

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

        public static void DefaultAMorPM( ref string startTime, ref string endTime )
        {
            // if neither have am or pm and the absolute value of the end hours are less than the start hours default am for the start and pm for the end
            // if one has am or pm but the other doesn't do the opposite.
            Regex timeEntryRegEx = new Regex( _TimePattern );
            Match startTimeMatch = timeEntryRegEx.Match( startTime );
            Match endTimeMatch = timeEntryRegEx.Match( endTime );

            if ( startTimeMatch.Success && endTimeMatch.Success )
            {
                int startHours = int.Parse( startTimeMatch.Groups[ 1 ].Value );
                int startMinutes = 0;
                if ( startTimeMatch.Groups[ 2 ].Value.Length > 0 )
                    startMinutes = int.Parse( startTimeMatch.Groups[ 2 ].Value );

                string startAmpm = startTimeMatch.Groups[ 3 ].Value;

                int endHours = int.Parse( endTimeMatch.Groups[ 1 ].Value );
                int endMinutes = 0;
                if ( endTimeMatch.Groups[ 2 ].Value.Length > 0 )
                    endMinutes = int.Parse( endTimeMatch.Groups[ 2 ].Value );
                string endAmpm = endTimeMatch.Groups[ 3 ].Value;

                if ( startAmpm.Length > 0 && endAmpm.Length > 0 )
                    return;
                else if ( startAmpm.Length == 0 && endAmpm.Length == 0 )
                {
                    if ( ( startHours + startMinutes / 60 > endHours + endMinutes / 60 ) && endHours == 12 )
                    {
                        startAmpm = "pm";
                        endAmpm = "am";
                    }
                    else if ( ( startHours + startMinutes / 60 > endHours + endMinutes / 60 ) && startHours != 12 )
                    {
                        startAmpm = "am";
                        endAmpm = "pm";
                    }
                    else if ( ( startHours + startMinutes / 60 < endHours + endMinutes / 60 ) && endHours == 12 )
                    {
                        startAmpm = "pm";
                        endAmpm = "am";
                    }
                    else
                    {
                        startAmpm = "am";
                        endAmpm = "am";
                    }

                }
                else if ( startAmpm.Length == 0 && endAmpm.Length > 0 )
                {
                    // Since I am rarely working at 12 am, but almost always working at 12 pm, default to PM. This means
                    // if I mean AM I have to explicitly mark it as 12 am in my time entry.
                    if ( startHours.Equals( 12 ) )
                    {
                        startAmpm = "pm";
                    }
                    else if ( startHours + startMinutes / 60 > endHours + endMinutes / 60 )
                    {
                        if ( endAmpm.ToLower() == "am" ) startAmpm = "pm";
                        if ( endAmpm.ToLower() == "pm" ) startAmpm = "am";
                    }
                    else
                    {
                        startAmpm = endAmpm;
                    }
                }
                else // startAmPM has a value but end doesn't
                {
                    // Since I am rarely working at 12 am, but almost always working at 12 pm, default to PM. This means
                    // if I mean AM I have to explicitly mark it as 12 am in my time entry.
                    if ( endHours.Equals( 12 ) )
                    {
                        endAmpm = "pm";
                    }
                    else if ( startHours + startMinutes / 60 > endHours + endMinutes / 60 )
                    {
                        if ( startAmpm.ToLower() == "am" ) endAmpm = "pm";
                        if ( startAmpm.ToLower() == "pm" ) endAmpm = "am";
                    }
                    else
                    {
                        endAmpm = startAmpm;
                    }
                }

                startTime = FormatTimeString( startHours, startMinutes, startAmpm );
                endTime = FormatTimeString( endHours, endMinutes, endAmpm );
            }

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
                string hours = m.Groups[ 1 ].Value;
                string minutes = "00";
                string ampm = m.Groups[ 3 ].Value;

                time = FormatTimeString( hours, minutes, ampm );
            }

            return time;
        }

        static public string FormatTimeString( int hours, int minutes, string ampm )
        {
            return FormatTimeString( hours.ToString(), string.Format( "{0:00}", minutes ), ampm );
        }

        static public string FormatTimeString( string hours, string minutes, string ampm )
        {
            string time = string.Format( "{0}:{1}", hours, minutes );

            if ( !string.IsNullOrEmpty( ampm ) )
                time = string.Format( "{0} {1}", time, ampm );

            return time;
        }
    }
}
