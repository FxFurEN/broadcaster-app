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
using System.Windows.Shapes;

namespace laba6_11
{
    /// <summary>
    /// Логика взаимодействия для AddAgentsPage.xaml
    /// </summary>
    public partial class AddAgentsPage : Window
    {

        private DataTable zakazchikiTable = new DataTable();
        string connectionString = "Data Source=DESKTOP-C71347J\\SQLEXPRESS;Initial Catalog=db_Ychet_TeleComanies;Integrated Security=True;";


        public AddAgentsPage()
        {
            InitializeComponent();
            ZakazchikComboBox.DisplayMemberPath = "FIO";
            ZakazchikComboBox.SelectedValuePath = "ID";

            // Заполните DataTable данными из базы данных
            FillZakazchikiTable();
            
        }

        private void FillZakazchikiTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT ID, FIO FROM tbl_Zakazchiki"; // Включите столбец "ID" в запрос

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(zakazchikiTable);
                }
            }

            ZakazchikComboBox.ItemsSource = zakazchikiTable.DefaultView;

            // Установите выбранным первого заказчика, если он доступен
            if (zakazchikiTable.Rows.Count > 0)
            {
                ZakazchikComboBox.SelectedIndex = 0;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string fio = FIOTextBox.Text;
            if (string.IsNullOrWhiteSpace(fio))
            {
                MessageBox.Show("Пожалуйста, введите ФИО.");
                return;
            }

            if (!int.TryParse(GeneralTimeTextBox.Text, out int generalTime) || generalTime <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное общее время рекламы.");
                return;
            }

            // Получите выбранного заказчика из ComboBox
            if (ZakazchikComboBox.SelectedItem is DataRowView selectedRow)
            {
                 int zakazchikID = (int)selectedRow["ID"];

                // Создайте SQL-запрос на вставку данных в таблицу
                string insertQuery = "INSERT INTO tbl_Agents (FIO, General_time_of_start_reklams, ID_zakazchika) VALUES (@FIO, @GeneralTime, @ZakazchikID)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        // Добавьте параметры к запросу
                        command.Parameters.AddWithValue("@FIO", fio);
                        command.Parameters.AddWithValue("@GeneralTime", generalTime);
                        command.Parameters.AddWithValue("@ZakazchikID", zakazchikID);

                        // Выполните SQL-запрос
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Агент добавлен.");
                            Agents mainWindow = Application.Current.Windows.OfType<Agents>().FirstOrDefault();
                            if (mainWindow != null)
                            {
                                mainWindow.LoadAgentData(); 
                            }
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при добавлении данных.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказчика.");
            }
        }

        



    }
}