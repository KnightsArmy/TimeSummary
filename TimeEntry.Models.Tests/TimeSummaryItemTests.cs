using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimeEntry.Models.Tests
{
    [TestClass]
    public class TimeSummaryItemTests
    {
        [TestMethod]
        public void AddCommentTest()
        {
            // Arrange
            string projectName = "AT";
            double timeInHours = 1.5;
            string comment = "Timesheets & Emails";

            // Act
            TimeSummaryItem sut = new TimeSummaryItem() { ProjectName = projectName, TimeSpentInHours = timeInHours };
            sut.Comments.Add( comment );

            // Assert
            Assert.AreEqual( 1, sut.Comments.Count );
            Assert.AreEqual( comment, sut.Comments[0] );
        }
    }
}

