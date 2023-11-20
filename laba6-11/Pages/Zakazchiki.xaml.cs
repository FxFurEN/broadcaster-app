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
using laba6_11.Charts;

namespace laba6_11
{
    /// <summary>
    /// Логика взаимодействия для ReklamaInPrograms.xaml
    /// </summary>
    public partial class Zakazchiki : Window
    {
        string connectionString = "Data Source=DESKTOP-C71347J\\SQLEXPRESS;Initial Catalog=db_Ychet_TeleComanies;Integrated Security=True;";

        public Zakazchiki()
        {
            InitializeComponent();
            LoadZakazchikiData();
            LoadToComboBoxSearch();


        }
        private void comboBoxSearch2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxSearch2.SelectedIndex == 1) 
            {
                datePicker.Visibility = Visibility.Visible;
                searchTextBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                datePicker.Visibility = Visibility.Collapsed;
                searchTextBox.Visibility = Visibility.Visible;
            }
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxSearch2.SelectedIndex == 1)
            {
                DateTime? selectedDate = datePicker.SelectedDate;

                if (selectedDate.HasValue)
                {
                    string searchText = selectedDate.Value.ToString("yyyy-MM-dd");
                    PerformSearch(searchText);
                }
            }
        }

        public void LoadZakazchikiData()
        {
            string query = "SELECT * FROM tbl_Zakazchiki";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                zakazchikiDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }
        private void LoadToComboBoxSearch()
        {
            foreach (DataGridColumn column in zakazchikiDataGrid.Columns)
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
                if (button.Tag != null && int.TryParse(button.Tag.ToString(), out int zakazchikID))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string sql = "DELETE FROM tbl_Zakazchiki WHERE ID = @ID";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@ID", zakazchikID);
                            command.ExecuteNonQuery();
                            LoadZakazchikiData();
                        }


                    }
                }
                else
                {
                    MessageBox.Show("Invalid Zakazchik ID");
                }
            }
        }
        private void OpenAddPage_Click(object sender, RoutedEventArgs e)
        {
            AddZakazchikPage addAgentWindow = new AddZakazchikPage();
            addAgentWindow.ShowDialog();


        }

        private void ZakazchikiDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (zakazchikiDataGrid.SelectedItem is DataRowView selectedRow)
            {
                EditZakazchik editPage = new EditZakazchik();
                // Передайте выбранные данные в окно EditAgents
                editPage.LoadData(selectedRow);
                editPage.ShowDialog();
            }
        }
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (comboBoxSearch2.SelectedIndex != 1)
            {
                string searchText = searchTextBox.Text;
                PerformSearch(searchText);
            }
        }
        private void PerformSearch(string searchText)
        {
            string[] massProcedureName = new string[4] { "SortZakazchikiByName", "SortZakazchikiByDate", "SearchZakazchikByPhone", "SearchZakazchikByBankRekvezit" };
            string[] massNameColumnToSearch = new string[4] { "@ZakazchikName", "@StartDate", "@PhoneNumber", "@BankRekvezit" };
            int i = comboBoxSearch2.SelectedIndex;

            if (i >= 0 && i < massProcedureName.Length && i < massNameColumnToSearch.Length)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(massProcedureName[i], connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (massNameColumnToSearch[i] == "@StartDate")
                        {
                            if (DateTime.TryParse(searchText, out DateTime startDate))
                            {
                                command.Parameters.AddWithValue(massNameColumnToSearch[i], startDate);
                            }
                            else
                            {
                                MessageBox.Show("Неверный формат даты. Введите дату в формате ГГГГ-ММ-ДД (например, 2023-10-31).");
                                return;
                            }
                        }
                        else
                        {
                            command.Parameters.AddWithValue(massNameColumnToSearch[i], searchText);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        zakazchikiDataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
            else
            {
                MessageBox.Show("Ошибка");
            }
        }
        private void OpenReportPage_Click(object sender, RoutedEventArgs e)
        {
            ReportZakazchiki reportPage = new ReportZakazchiki();
            reportPage.ShowDialog();
        }
        private void OpenCharts1Page_Click(object sender, RoutedEventArgs e)
        {
            PieChartsZakazchiki chartsPage = new PieChartsZakazchiki();
            chartsPage.ShowDialog();
        }

        private void OpenCharts2Page_Click(object sender, RoutedEventArgs e)
        {
            ColumnChartsZakazchik chartsPage = new ColumnChartsZakazchik();
            chartsPage.ShowDialog();
        }

        private void OpenMainPage_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainPage = new MainWindow();
            mainPage.Show();
            this.Close();
        }




    }

}