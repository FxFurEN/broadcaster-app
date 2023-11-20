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

namespace laba6_11
{
    /// <summary>
    /// Логика взаимодействия для ColumnChartsAgent.xaml
    /// </summary>
    public partial class ColumnChartsAgent : Window
    {
        string connectionString = "Data Source=DESKTOP-C71347J\\SQLEXPRESS;Initial Catalog=db_Ychet_TeleComanies;Integrated Security=True;";
        public ColumnChartsAgent()
        {
            InitializeComponent();
            LoadAndBindData();
        }

        public class AgreementData
        {
            public double Value { get; set; }
            public string Label { get; set; }
        }
        private void LoadAndBindData()
        {
            try
            {
                string query = "SELECT AgentName, GTSO FROM vw_AgentWithCustomerName";
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
                var agreementDataList = new List<AgreementData>();
                foreach (DataRow row in dataTable.Rows)
                {
                    string agreementName = row["AgentName"].ToString();
                    double agreementKolvoStoron = Convert.ToDouble(row["GTSO"]);

                    labels.Add(agreementName);
                    values.Add(agreementKolvoStoron);
                    agreementDataList.Add(new AgreementData { Value = agreementKolvoStoron, Label = agreementName });

                }
                var columnSeries = new ColumnSeries
                {
                    Title = "",
                    Values = new ChartValues<double>(),
                    DataLabels = false,
                    LabelPoint = point =>
                    {
                        var agreementData = agreementDataList[(int)point.X];
                        return $"Назв.: {agreementData.Label}, Кол-Во: {agreementData.Value}";
                    }
                };
                int temp = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    columnSeries.Values.Add(values[temp]);
                    temp++;
                }

                cartesianChart.Series = new SeriesCollection { columnSeries };



            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }
    }
}
