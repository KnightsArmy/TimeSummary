using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimeEntry.Models.Tests
{
    [TestClass]
    public class TimeUtilitiesTests
    {
        [TestMethod]
        public void DefaultAMorPMValueIsBusinessTimeAndBeforeNoon()
        {
            // Arrange
            string time = "11:30";
            TimeSpan businessDayStartTime = new TimeSpan( 8, 0, 0 ); // 8 am 
            TimeSpan businessDayEndTime = new TimeSpan( 18, 0, 0 ); // 6 pm 

            // Act
            string result = TimeUtilities.DefaultAMorPM( time, businessDayStartTime, businessDayEndTime );

            // Assert
            Assert.AreEqual( time + " am", result ); 
        }

        [TestMethod]
        public void DefaultAMorPMValueIsBusinessTimeAndAfterNoon()
        {
            // Arrange
            string time = "1:30";
            TimeSpan businessDayStartTime = new TimeSpan( 8, 0, 0 ); // 8 am 
            TimeSpan businessDayEndTime = new TimeSpan( 18, 0, 0 ); // 6 pm 

            // Act
            string result = TimeUtilities.DefaultAMorPM( time, businessDayStartTime, businessDayEndTime );

            // Assert
            Assert.AreEqual( time + " pm", result ); 
        }

        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void DefaultAMorPMValueStartTimeAfterNoonTest()
        {
            // Arrange
            string time = "11:30";
            TimeSpan businessDayStartTime = new TimeSpan( 13, 0, 0 ); // 1 pm 
            TimeSpan businessDayEndTime = new TimeSpan( 18, 0, 0 ); // 6 pm 

            // Act
            string result = TimeUtilities.DefaultAMorPM( time, businessDayStartTime, businessDayEndTime );
        }

        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void DefaultAMorPMValueEndTimeBeforeNoonTest()
        {
            // Arrange
            string time = "11:30";
            TimeSpan businessDayStartTime = new TimeSpan( 13, 0, 0 ); // 1 pm 
            TimeSpan businessDayEndTime = new TimeSpan( 8, 0, 0 ); // 6 pm 

            // Act
            string result = TimeUtilities.DefaultAMorPM( time, businessDayStartTime, businessDayEndTime );
        }

        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void DefaultAMorPMValueMoreThan12HourWorkday()
        {
            // Arrange
            string time = "11:30";
            TimeSpan businessDayStartTime = new TimeSpan( 8, 0, 0 ); // 8 am 
            TimeSpan businessDayEndTime = new TimeSpan( 8, 0, 0 ); // 8 pm 

            // Act
            string result = TimeUtilities.DefaultAMorPM( time, businessDayStartTime, businessDayEndTime );

        }

        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void DefaultAMorPMValueOutsideOfBusinessDay()
        {
            // Arrange
            string time = "7:30";
            TimeSpan businessDayStartTime = new TimeSpan( 8, 0, 0 ); // 8 am 
            TimeSpan businessDayEndTime = new TimeSpan( 18, 0, 0 ); // 6 pm 

            // Act
            string result = TimeUtilities.DefaultAMorPM( time, businessDayStartTime, businessDayEndTime );
        }

        [TestMethod]
        public void DefaultAMorPMWithoutCrossingNoonNoAmOrPm()
        {
            // Arrange
            string startTime = "1:30";
            string endTime = "3:30";

            // Act
            TimeUtilities.DefaultAMorPM( ref startTime, ref endTime );

            // Assert
            Assert.AreEqual( "1:30 am", startTime );
            Assert.AreEqual( "3:30 am", endTime );
        }

        [TestMethod]
        public void DefaultAMorPMWithoutCrossingNoonOneAm()
        {
            // Arrange
            string startTime = "1:30 am";
            string endTime = "3:30";

            // Act
            TimeUtilities.DefaultAMorPM( ref startTime, ref endTime );

            // Assert
            Assert.AreEqual( "1:30 am", startTime );
            Assert.AreEqual( "3:30 am", endTime );
        }

        [TestMethod]
        public void DefaultAMorPMCrossingNoonNoAMorPM()
        {
            // Arrange
            string startTime = "11:30";
            string endTime = "2:30";

            // Act
            TimeUtilities.DefaultAMorPM( ref startTime, ref endTime );

            // Assert
            Assert.AreEqual( "11:30 am", startTime );
            Assert.AreEqual( "2:30 pm", endTime );
        }

        [TestMethod]
        public void DefaultAMorPMCrossingNoonWithOneAM()
        {
            // Arrange
            string startTime = "11:30 am";
            string endTime = "2:30";

            // Act
            TimeUtilities.DefaultAMorPM( ref startTime, ref endTime );

            // Assert
            Assert.AreEqual( "11:30 am", startTime );
            Assert.AreEqual( "2:30 pm", endTime );
        }

        [TestMethod]
        public void DefaultAMorPMCrossingNoonWithOnePM()
        {
            // Arrange
            string startTime = "11:30 pm";
            string endTime = "2:30";

            // Act
            TimeUtilities.DefaultAMorPM( ref startTime, ref endTime );

            // Assert
            Assert.AreEqual( "11:30 pm", startTime );
            Assert.AreEqual( "2:30 am", endTime );
        }

        [TestMethod]
        public void IsMilitaryTimeExpectTrue()
        {
            // Arrange
            string time = "14:30";

            // Act
            bool result = TimeUtilities.IsMilitaryTime( time );

            // Assert
            Assert.IsTrue( result );
        }


        [TestMethod]
        public void IsMilitaryTimeExpectFalse()
        {
            // Arrange
            string time = "09:30";

            // Act
            bool result = TimeUtilities.IsMilitaryTime( time );

            // Assert
            Assert.IsFalse( result );
        }


        [TestMethod]
        public void IsMilitaryTimeExpectFalseBecauseOfAM()
        {
            // Arrange
            string time = "09:30 am";

            // Act
            bool result = TimeUtilities.IsMilitaryTime( time );

            // Assert
            Assert.IsFalse( result );
        }

        [TestMethod]
        public void AddMinutesTerse()
        {
            // Arrange
            string time = "9";

            // Act
            string result = TimeUtilities.AddMinutesField( time );

            // Assert
            Assert.AreEqual( "9:00", result );
        }

        [TestMethod]
        public void AddMinutesTerseWithAM()
        {
            // Arrange
            string time = "9 am";

            // Act
            string result = TimeUtilities.AddMinutesField( time );

            // Assert
            Assert.AreEqual( "9:00 am", result );
        }
    }
}
