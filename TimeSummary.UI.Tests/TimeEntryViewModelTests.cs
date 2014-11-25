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
            sut.TimeLineItems = items;
            sut.CreateTimeSummaryLists();

            // Assert
            Assert.AreEqual( 2, sut.TimeSummaryItems.Count );
        }


        [TestMethod]
        public void CopyCommentToClipboardOnlyOneCommentTest()
        {
            // Arrange
            TimeLineItem tl1 = new TimeLineItem() { ProjectName = "AT", StartTime = DateTime.Parse( "1:00 pm" ), EndTime = DateTime.Parse( "2:00 pm" ), Comment = "CMT1" };
            TimeLineItem tl2 = new TimeLineItem() { ProjectName = "RE", StartTime = DateTime.Parse( "2:00 pm" ), EndTime = DateTime.Parse( "3:00 pm" ) };
            TimeLineItem tl3 = new TimeLineItem() { ProjectName = "AT", StartTime = DateTime.Parse( "3:00 pm" ), EndTime = DateTime.Parse( "4:00 pm" ) };

            List<TimeLineItem> items = new List<TimeLineItem>();
            items.Add( tl1 );
            items.Add( tl2 );
            items.Add( tl3 );

            // Act
            TimeEntryViewModel sut = new TimeEntryViewModel();
            sut.TimeLineItems = items;
            sut.CreateTimeSummaryLists();
            sut.CopyCommentToClipboardCommandOnExecute( "AT" );

            string results = System.Windows.Forms.Clipboard.GetText();

            // Assert
            Assert.AreEqual( tl1.Comment, results );
        }


        [TestMethod]
        public void CopyCommentToClipboardMoreThanOneCommentTest()
        {
            // Arrange
            TimeLineItem tl1 = new TimeLineItem() { ProjectName = "AT", StartTime = DateTime.Parse( "1:00 pm" ), EndTime = DateTime.Parse( "2:00 pm" ), Comment = "CMT1" };
            TimeLineItem tl2 = new TimeLineItem() { ProjectName = "RE", StartTime = DateTime.Parse( "2:00 pm" ), EndTime = DateTime.Parse( "3:00 pm" ) };
            TimeLineItem tl3 = new TimeLineItem() { ProjectName = "AT", StartTime = DateTime.Parse( "3:00 pm" ), EndTime = DateTime.Parse( "4:00 pm" ), Comment = "CMT2" };

            List<TimeLineItem> items = new List<TimeLineItem>();
            items.Add( tl1 );
            items.Add( tl2 );
            items.Add( tl3 );

            // Act
            TimeEntryViewModel sut = new TimeEntryViewModel();
            sut.TimeLineItems = items;
            sut.CreateTimeSummaryLists();
            sut.CopyCommentToClipboardCommandOnExecute( "AT" );

            System.Threading.Thread.Sleep( 500 );

            string results = System.Windows.Forms.Clipboard.GetText();

            // Assert
            Assert.AreEqual( tl1.Comment + "\r\n" + tl3.Comment, results );
        }

        [TestMethod]
        public void RemoveDuplicateCommentsTest()
        {
            // Arrange
            string testComment = "TestComment";
            TimeLineItem tl1 = new TimeLineItem() { ProjectName = "AT", StartTime = DateTime.Parse( "1:00 pm" ), EndTime = DateTime.Parse( "2:00 pm" ), Comment = testComment };
            TimeLineItem tl2 = new TimeLineItem() { ProjectName = "RE", StartTime = DateTime.Parse( "2:00 pm" ), EndTime = DateTime.Parse( "3:00 pm" ) };
            TimeLineItem tl3 = new TimeLineItem() { ProjectName = "AT", StartTime = DateTime.Parse( "3:00 pm" ), EndTime = DateTime.Parse( "4:00 pm" ), Comment = testComment };

            List<TimeLineItem> items = new List<TimeLineItem>();
            items.Add( tl1 );
            items.Add( tl2 );
            items.Add( tl3 );

            // Act
            TimeEntryViewModel sut = new TimeEntryViewModel();
            sut.TimeLineItems = items;
            sut.CreateTimeSummaryLists();
            sut.CopyCommentToClipboardCommandOnExecute( "AT" );

            System.Threading.Thread.Sleep( 500 );

            string results = System.Windows.Forms.Clipboard.GetText();

            // Assert
            Assert.AreEqual( tl1.Comment, results );
        }


        [TestMethod]
        public void PasteAndParseCommandTest()
        {
            // Arrange (Get some initial data into the app)
            string testComment = "TestComment";
            TimeLineItem tl1 = new TimeLineItem() { ProjectName = "AT", StartTime = DateTime.Parse( "1:00 pm" ), EndTime = DateTime.Parse( "2:00 pm" ), Comment = testComment };
            List<TimeLineItem> items = new List<TimeLineItem>();
            items.Add( tl1 );
            TimeEntryViewModel sut = new TimeEntryViewModel();
            sut.TimeLineItems = items;
            sut.CreateTimeSummaryLists();
            sut.CopyCommentToClipboardCommandOnExecute( "AT" );

            // Arrange (Here is our new data to paste in and parse)
            System.Windows.Forms.Clipboard.SetData( "Text", "10:15 - 11 DM Eric's Weekly\r\n11-12 AT Email" );
			sut.PasteAndParseCommand.Execute( null );

            // Assert
            Assert.AreEqual( 2, sut.TimeSummaryItems.Count );
        }
    }
}
