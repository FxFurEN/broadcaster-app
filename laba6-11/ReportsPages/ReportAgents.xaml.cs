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
using System.Windows.Shapes;

namespace laba6_11
{
    /// <summary>
    /// Логика взаимодействия для ReportAgents.xaml
    /// </summary>
    public partial class ReportAgents : Window
    {
        public ReportAgents()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ReportAgents_Loaded);

        }
        private void ReportAgents_Loaded( object sender, RoutedEventArgs e)
        {
            this.ReportViewer.ReportPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"C:\4 курс\бд\laba6-11\laba6-11\Resources\ReportAgents.rdl");
            this.ReportViewer.RefreshReport();
        }
    }
}
