using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeEntry.Models;
using TimeSummary.UI.WPF;
using System.Collections.Generic;

namespace TimeSummary.UI.Tests
{
    [TestClass]
    public class TimeEntryViewModelTests
    {
        [TestMethod]
        public void CreateTimeSummaryGroupsTest()
        {
            // Arrange
            TimeLineItem tl1 = new TimeLineItem() { ProjectName = "AT", StartTime = DateTime.Parse( "1:00 pm" ), EndTime = DateTime.Parse( "2:00 pm" ) };
            TimeLineItem tl2 = new TimeLineItem() { ProjectName = "RE", StartTime = DateTime.Parse( "2:00 pm" ), EndTime = DateTime.Parse( "3:00 pm" ) };
            TimeLineItem tl3 = new TimeLineItem() { ProjectName = "AT", StartTime = DateTime.Parse( "3:00 pm" ), EndTime = DateTime.Parse( "4:00 pm" ) };

            List<TimeLineItem> items = new List<TimeLineItem>();
            items.Add( tl1 );
            items.Add( tl2 );
            items.Add( tl3 );

            // Act
            TimeEntryViewModel sut = new TimeEntryViewModel();
            List<TimeSummaryItem> results = sut.CreateTimeSummaryLists( items );

            // Assert
            Assert.AreEqual( 2, results.Count );
        }
    }
}
