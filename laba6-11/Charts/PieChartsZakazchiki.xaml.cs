using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

namespace laba6_11.Charts
{
    /// <summary>
    /// Логика взаимодействия для PieChartsZakazchiki.xaml
    /// </summary>
    public partial class PieChartsZakazchiki : Window
    {
        string connectionString = "Data Source=DESKTOP-C71347J\\SQLEXPRESS;Initial Catalog=db_Ychet_TeleComanies;Integrated Security=True;";

        public PieChartsZakazchiki()
        {
            InitializeComponent();
            LoadAndBindData();
        }

        private void LoadAndBindData()
        {
            try
            {
                string query = "SELECT FIO, Bank_rekvezit FROM tbl_Zakazchiki";
                DataTable dataTable = new DataTable();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }

                ChartValues<double> values = new ChartValues<double>();
                List<string> labels = new List<string>();

                foreach (DataRow row in dataTable.Rows)
                {
                    string agreementName = row["FIO"].ToString();
                    Int64 agreementKolvoStoron = Convert.ToInt64(row["Bank_rekvezit"]);

                    labels.Add(agreementName);
                    values.Add(agreementKolvoStoron);
                }

                PieChart.Series = new SeriesCollection();

                for (int i = 0; i < values.Count; i++)
                {
                    var series = new PieSeries
                    {
                        Title = labels[i],
                        Values = new ChartValues<double> { values[i] }
                    };

                    PieChart.Series.Add(series);
                }
                PieChart.LegendLocation = LegendLocation.Right;

                PieChart.AxisX.Add(new Axis
                {
                    Labels = labels
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }
    }
}
