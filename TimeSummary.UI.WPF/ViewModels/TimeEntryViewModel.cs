using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TimeEntry.Models;

namespace TimeSummary.UI.WPF.ViewModels
{
  public class TimeEntryViewModel : ObservableObject
  {
    private string _timeEntryInput;
    private string _timeEntryOutput;

    public TimeEntryViewModel()
    {
      this.ParseCommand = new RelayCommand(ParseCommandOnExecute, ParseCommandCanExecute);
      this.TimeEntryInput = "Paste Text Here";
    }

    private bool ParseCommandCanExecute()
    {
      return !string.IsNullOrWhiteSpace(this.TimeEntryInput);
    }

    private void ParseCommandOnExecute()
    {
      string total = string.Empty;
      List<TimeLineItem> lineItems = new List<TimeLineItem>();

      foreach (string timeEntry in this.TimeEntryInput.Split('\n'))
      {
        try
        {
          if (timeEntry != string.Empty && timeEntry != "\r" && timeEntry != "Paste Text Here\r" && timeEntry != "Paste Text Here") lineItems.Add(TimeLineItem.Parse(timeEntry));
        }
        catch
        {
          MessageBox.Show(string.Format("Couldn't parse: {0}", timeEntry));
        }

      }

      StringBuilder outputString = new StringBuilder();

      var timeGroups = from li in lineItems
                       group li by li.ProjectName.ToUpper() into g
                       orderby g.Key
                       select new { UpperCaseProjectName = g.Key, TotalHours = g.Sum(li => li.TimeSpentInHours()) };



      foreach (var li in timeGroups)
      {
        // output the total time spent for the day on a bucket
        outputString.AppendLine(string.Format("{0}   {1:0.00} Hours", li.UpperCaseProjectName, li.TotalHours));

        var comments = from cli in lineItems
                       where cli.ProjectName.ToUpper() == li.UpperCaseProjectName && cli.Comment != string.Empty
                       select cli.Comment;

        // output the comments for each line item on a seperate line
        foreach (var comment in comments)
        {
          outputString.AppendLine(string.Format("{0}", comment.Replace('\r', ' ')));
        }

        outputString.AppendLine();
      }

      // Output a summary line
      outputString.AppendLine("----------------------------------------------------------");
      outputString.AppendLine(string.Format("Total Time Worked: {0:0.00} Hours", timeGroups.Sum(x => x.TotalHours)));


      this.TimeEntryOutput = outputString.ToString();
    }

    public ICommand ParseCommand { get; private set; }

    public string TimeEntryInput
    {
      get { return _timeEntryInput; }
      set
      {
        base.Set(()=>TimeEntryInput, ref _timeEntryInput, value);
      }
    }

    public string TimeEntryOutput
    {
      get { return _timeEntryOutput; }
      set
      {
        base.Set(()=>TimeEntryOutput, ref _timeEntryOutput, value);
      }
    }
  }
}
