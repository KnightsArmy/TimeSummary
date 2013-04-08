using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimeEntry.Models.Tests
{
    [TestClass]
    public class TimeLineItemTests
    {
        #region Parsing Tests

        [TestMethod]
        public void ParseVerboseEntry()
        {
            // Arrange
            string startTime = "9:00 am";
            string endTime = "9:30 am";
            string projectName = "AT";
            string comment = "Timesheets & Emails";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0} - {1} {2} {3}", startTime, endTime, projectName, comment ) );

            // Assert
            Assert.AreEqual( DateTime.Parse( startTime ), sut.StartTime );
            Assert.AreEqual( DateTime.Parse( endTime ), sut.EndTime );
            Assert.AreEqual( projectName, sut.ProjectName );
            Assert.AreEqual( comment, sut.Comment );
        }

        [TestMethod]
        public void ParseWithNoonInTime()
        {
            // Arrange
            string startTime = "10:30";
            string endTime = "12:00";
            string projectName = "AT";
            string comment = "Timesheets & Emails";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0} - {1} {2} {3}", startTime, endTime, projectName, comment ) );

            // Assert
            Assert.AreEqual( DateTime.Parse( startTime ), sut.StartTime );
            Assert.AreEqual( DateTime.Parse( endTime ), sut.EndTime );
            Assert.AreEqual( projectName, sut.ProjectName );
            Assert.AreEqual( comment, sut.Comment );
        }

        [TestMethod]
        public void ParseOddSpacingWithProjectEntry()
        {
            // Arrange
            string startTime = "9:00 am";
            string endTime = "9:30 am";
            string projectName = "AT";
            string comment = "Timesheets & Emails";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0} - {1}  {2} {3}", startTime, endTime, projectName, comment ) );

            // Assert
            Assert.AreEqual( DateTime.Parse( startTime ), sut.StartTime );
            Assert.AreEqual( DateTime.Parse( endTime ), sut.EndTime );
            Assert.AreEqual( projectName, sut.ProjectName );
            Assert.AreEqual( comment, sut.Comment );
        }

        [TestMethod]
        public void ParseWithMilitaryTime()
        {
            // Arrange
            string startTime = "13:25";
            string endTime = "14:00";
            string projectName = "AT";
            string comment = "Timesheets & Emails";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0} - {1} {2} {3}", startTime, endTime, projectName, comment ) );

            // Assert
            Assert.AreEqual( DateTime.Parse( startTime ), sut.StartTime );
            Assert.AreEqual( DateTime.Parse( endTime ), sut.EndTime );
            Assert.AreEqual( projectName, sut.ProjectName );
            Assert.AreEqual( comment, sut.Comment );
        }

        [TestMethod]
        public void ParseEntryWithoutAMorPM()
        {
            // Arrange
            string startTime = "9:00";
            string endTime = "9:30";
            string projectName = "AT";
            string comment = "Timesheets & Emails";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0} - {1} {2} {3}", startTime, endTime, projectName, comment ) );

            // Assert
            Assert.AreEqual( DateTime.Parse( startTime ), sut.StartTime );
            Assert.AreEqual( DateTime.Parse( endTime ), sut.EndTime );
            Assert.AreEqual( projectName, sut.ProjectName );
            Assert.AreEqual( comment, sut.Comment );
        }

        [TestMethod]
        public void ParseEntryFirstWithAMandSecondWithoutAM()
        {
            // Arrange
            string startTime = "9:00 am";
            string endTime = "9:30";
            string projectName = "AT";
            string comment = "Timesheets & Emails";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0} - {1} {2} {3}", startTime, endTime, projectName, comment ) );

            // Assert
            Assert.AreEqual( DateTime.Parse( startTime ), sut.StartTime );
            Assert.AreEqual( DateTime.Parse( endTime ), sut.EndTime );
            Assert.AreEqual( projectName, sut.ProjectName );
            Assert.AreEqual( comment, sut.Comment );
        }

        [TestMethod]
        public void ParseCompactEntry()
        {
            // Arrange
            string startTime = "9:00";
            string endTime = "9:30";
            string projectName = "AT";
            string comment = "Timesheets & Emails";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0}-{1} {2} {3}", startTime, endTime, projectName, comment ) );

            // Assert
            Assert.AreEqual( DateTime.Parse( startTime ), sut.StartTime );
            Assert.AreEqual( DateTime.Parse( endTime ), sut.EndTime );
            Assert.AreEqual( projectName, sut.ProjectName );
            Assert.AreEqual( comment, sut.Comment );
        }

        [TestMethod]
        public void ParseTerseEntry()
        {
            // Arrange
            string userEnteredStartTime = "9";
            string userEnteredEndTime = "10";
            string projectName = "AT";
            string comment = "Timesheets & Emails";

            string startTimeAfterConversion = userEnteredStartTime + ":00";
            string endTimeAfterConversion = userEnteredEndTime + ":00";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0}-{1} {2} {3}", userEnteredStartTime, userEnteredEndTime, projectName, comment ) );

            // Assert
            Assert.AreEqual( DateTime.Parse( startTimeAfterConversion ), sut.StartTime );
            Assert.AreEqual( DateTime.Parse( endTimeAfterConversion ), sut.EndTime );
            Assert.AreEqual( projectName, sut.ProjectName );
            Assert.AreEqual( comment, sut.Comment );
        }

        [TestMethod]
        public void ParseTerseEntryWithAM()
        {
            // Arrange
            string userEnteredStartTime = "9 am";
            string startTimeAfterConversion = "9:00 am";
            string userEnteredEndTime = "10 am";
            string endTimeAfterConversion = "10:00 am";
            string projectName = "AT";
            string comment = "Timesheets & Emails";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0}-{1} {2} {3}", userEnteredStartTime, userEnteredEndTime, projectName, comment ) );

            // Assert
            Assert.AreEqual( DateTime.Parse( startTimeAfterConversion ), sut.StartTime );
            Assert.AreEqual( DateTime.Parse( endTimeAfterConversion ), sut.EndTime );
            Assert.AreEqual( projectName, sut.ProjectName );
            Assert.AreEqual( comment, sut.Comment );
        }

        [TestMethod]
        public void ParseWithoutComment()
        {
            // Arrange
            string userEnteredStartTime = "9";
            string userEnteredEndTime = "10";
            string projectName = "AT";

            string startTimeAfterConversion = userEnteredStartTime + ":00";
            string endTimeAfterConversion = userEnteredEndTime + ":00";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0}-{1} {2}", userEnteredStartTime, userEnteredEndTime, projectName ) );

            // Assert
            Assert.AreEqual( DateTime.Parse( startTimeAfterConversion ), sut.StartTime );
            Assert.AreEqual( DateTime.Parse( endTimeAfterConversion ), sut.EndTime );
            Assert.AreEqual( projectName, sut.ProjectName );
            Assert.AreEqual( string.Empty, sut.Comment );
        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void ParseWithoutProjectName()
        {
            // Arrange
            string userEnteredStartTime = "9:00";
            string userEnteredEndTime = "10:00";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0}-{1}", userEnteredStartTime, userEnteredEndTime ) );

        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void ParseWithoutProjectNameAndExtraWhiteSpace()
        {
            // Arrange
            string userEnteredStartTime = "9:00";
            string userEnteredEndTime = "10:00";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0}-{1} ", userEnteredStartTime, userEnteredEndTime ) );

        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void ParseWithInvalidTime()
        {
            // Arrange
            string startTime = "Invalid";
            string endTime = "10";
            string projectName = "AT";
            string comment = "Timesheets & Emails";

            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0}-{1} {2} {3}", startTime, endTime, projectName, comment ) );
        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void ParseWithCraziness()
        {
            // Act
            TimeLineItem sut = TimeLineItem.Parse( "a;dfja;dsfja alksdjf; asdkfja;sdfj asdf;asdf asdf" );
        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void ParseWithStringEmpty()
        {
            // Act
            TimeLineItem sut = TimeLineItem.Parse( string.Empty );
        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void ParseWithOnlyNewLine()
        {
            // Act
            TimeLineItem sut = TimeLineItem.Parse( "\n" );
        }

        #endregion

        #region Time Spent Tests

        [TestMethod]
        public void TimeSpentSameDayTest()
        {
            // Arrange
            string startTime = "9:00 am";
            string endTime = "9:30 am";
            string projectName = "AT";
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0} - {1} {2}", startTime, endTime, projectName ) );

            // Act
            double timeSpent = sut.TimeSpentInHours();

            // Assert
            Assert.AreEqual( 0.5, timeSpent );
        }

        [TestMethod]
        public void TimeSpentCrossOverMidnightTest()
        {
            // Arrange
            string startTime = "11:00 pm";
            string endTime = "1:30 am";
            string projectName = "AT";
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0} - {1} {2}", startTime, endTime, projectName ) );

            // Act
            double timeSpent = sut.TimeSpentInHours();

            // Assert
            Assert.AreEqual( 2.5, timeSpent );
        }

        [TestMethod]
        public void TimeSpentCrossOverNoonWithAMPMSpecifiedTest()
        {
            // Arrange
            string startTime = "11:00 am";
            string endTime = "12:30 pm";
            string projectName = "AT";
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0} - {1} {2}", startTime, endTime, projectName ) );

            // Act
            double timeSpent = sut.TimeSpentInHours();

            // Assert
            Assert.AreEqual( 1.5, timeSpent );
        }

        [TestMethod]
        public void TimeSpentCrossOverNoonWithoutAMPMSpecifiedTest()
        {
            // Arrange
            string startTime = "11:00";
            string endTime = "1:30";
            string projectName = "AT";
            TimeLineItem sut = TimeLineItem.Parse( string.Format( "{0} - {1} {2}", startTime, endTime, projectName ) );

            // Act
            double timeSpent = sut.TimeSpentInHours();

            // Assert
            Assert.AreEqual( 2.5, timeSpent );
        }
        #endregion
    }
}
