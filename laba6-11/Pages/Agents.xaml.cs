using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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


namespace laba6_11
{
    /// <summary>
    /// Логика взаимодействия для Agents.xaml
    /// </summary>
    public partial class Agents : Window
    {
        string connectionString = "Data Source=DESKTOP-C71347J\\SQLEXPRESS;Initial Catalog=db_Ychet_TeleComanies;Integrated Security=True;";

        public Agents()
        {
            InitializeComponent();
            LoadAgentData();
            LoadToComboBoxSearch();


        }
        public void LoadAgentData()
        {
            string query = "SELECT * FROM vw_AgentWithCustomerName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                agentDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }
        private void LoadToComboBoxSearch()
        {
            foreach (DataGridColumn column in agentDataGrid.Columns)
            {
                if (column.Header.ToString() != "Actions")
                {
                    comboBoxSearch2.Items.Add(column.Header);

                }
            }

            if (comboBoxSearch2.Items.Count > 0)
            {
                comboBoxSearch2.SelectedIndex = 0;
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag != null && int.TryParse(button.Tag.ToString(), out int agentID))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string sql = "DELETE FROM tbl_Agents WHERE ID = @AgentID";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@AgentID", agentID);
                            command.ExecuteNonQuery();
                            LoadAgentData();
                        }


                    }
                }
                else
                {
                    MessageBox.Show("Invalid Agent ID");
                }
            }
        }
        private void OpenAddPage_Click(object sender, RoutedEventArgs e)
        {
            AddAgentsPage addAgentWindow = new AddAgentsPage();
            addAgentWindow.ShowDialog();


        }

        private void AgentDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (agentDataGrid.SelectedItem is DataRowView selectedRow)
            {
                EditAgents editPage = new EditAgents();
                // Передайте выбранные данные в окно EditAgents
                editPage.LoadData(selectedRow);
                editPage.ShowDialog();
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = searchTextBox.Text;
            string[] massProcedureName = new string[3] { "SearchAgentName", "SearchGeneralADDTime", "SearchCustemerName" };
            string[] massNameColumnToSearch = new string[3] { "@AgentName", "@GTSO", "@CustomerName" };
            int i = comboBoxSearch2.SelectedIndex;
            // Check if the selected index is within a valid range
            if (i >= 0 && i < massProcedureName.Length && i < massNameColumnToSearch.Length)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(massProcedureName[i], connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(massNameColumnToSearch[i], searchText);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        agentDataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
            else
            {
                MessageBox.Show("Ошибка") ;
            }


        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (agentDataGrid.SelectedItem is DataRowView selectedRow)
            {

                string templatePath = @"Shablon.docx";
                string agentName = selectedRow["AgentName"].ToString();
                string customerName = selectedRow["CustomerName"].ToString();
                DateTime dateToday = DateTime.Now;
                string dateString = dateToday.ToString();

                string nameFile = "Договор" + "_" + DateTime.Now.Year + "." +  DateTime.Now.Hour  + "_" + customerName;
                string newDocumentPath = $"./Exports/{nameFile}.docx";


                string[] stringMass = new string[] {
                            agentName,
                            customerName,
                            dateString

                        };
                string[] replaceText = new string[] {
                            "AgentName",
                            "CustomerName",
                            "dateToday"


                        };
                T2CardGen n0 = new T2CardGen();
                n0.genDock(templatePath, newDocumentPath, replaceText, stringMass);
            }
            else
            {
                MessageBox.Show("Вы должны выбрать Агента!");
            }
        }

        private void OpenReportPage_Click(object sender, RoutedEventArgs e)
        {
            ReportAgents reportPage = new ReportAgents();
            reportPage.ShowDialog();
        }

        private void OpenCharts1Page_Click(object sender, RoutedEventArgs e)
        {
            PieChartsAgents chartsPage = new PieChartsAgents();
            chartsPage.ShowDialog();
        }

        private void OpenCharts2Page_Click(object sender, RoutedEventArgs e)
        {
            ColumnChartsAgent chartsPage = new ColumnChartsAgent();
            chartsPage.ShowDialog();
        }

        private void OpenMainPage_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainPage = new MainWindow();
            mainPage.Show();
            this.Close();
        }

        private void agentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
