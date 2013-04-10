using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeSummary.UI.WPF.ViewModels;

namespace TimeSummary.UI.Tests
{
  [TestClass]
  public class TimeEntryViewModelTests
  {
    [TestMethod]
    public void ParseCommand_WhenLowerCaseProjectNameIsEntered_ShouldDisplayComments()
    {
      var sut = new TimeEntryViewModel {TimeEntryInput = "9-11 stuff bug fixes"};
      sut.ParseCommand.Execute(null);
      Assert.IsTrue(sut.TimeEntryOutput.Contains("bug fixes"));
    }
  }
}
