using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeEntry.Models
{
    /// <summary>
    /// This class represents a summarization of all the time spent on a particular project throughout the day.
    /// For example, if I spend 3 different 30 minute time chunks on AT then an instance of the class would have AT as the project
    /// name and 1.5 as the time spent in hours.
    /// </summary>
    public class TimeSummaryItem
    {
        public string ProjectName { get; set; }
        public double TimeSpentInHours { get; set; }
        public List<string> Comments { get; set; }

        public TimeSummaryItem() : this( string.Empty, 0.0, null ) { }

        public TimeSummaryItem( string projectName, double timeSpentInHours, List<string> comments )
        {
            this.ProjectName = projectName;
            this.TimeSpentInHours = timeSpentInHours;

            if ( comments == null )
            {
                this.Comments = new List<string>();
            }
            else
            {
                this.Comments = comments;
            }
        }
    }
}
