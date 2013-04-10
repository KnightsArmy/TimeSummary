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
        public MainWindow()
        {
            this.DataContext = new TimeEntryViewModel();
            InitializeComponent();
        }

        private void txtHelp_Click( object sender, RoutedEventArgs e )
        {
            var helpWindow = new HelpWindow();
            helpWindow.Show();
        }
    }
}
