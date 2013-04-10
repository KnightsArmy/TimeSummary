using MahApps.Metro.Controls;
using System.Windows;
using TimeSummary.UI.WPF.ViewModels;

namespace TimeSummary.UI.WPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : MetroWindow
  {
    private HelpWindow _helpWindow;

    public MainWindow()
    {
      this.DataContext = new TimeEntryViewModel();
      InitializeComponent();
    }

    private HelpWindow HelpWindow
    {
      get
      {
        if (_helpWindow == null)
        {
          _helpWindow = new HelpWindow();
          _helpWindow.Closed += (sender, args) => _helpWindow = null;
        }
        return _helpWindow;
      }
    }

    private void txtHelp_Click(object sender, RoutedEventArgs e)
    {
      this.HelpWindow.Show();
    }
  }
}
