using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeEntry.Models;

namespace TimeSummary.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TimeEntryWindow : MetroWindow
    {
        public TimeEntryWindow()
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
