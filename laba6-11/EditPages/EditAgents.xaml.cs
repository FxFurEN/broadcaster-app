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
    public partial class EditAgents : Window
    {

       
        string connectionString = "Data Source=DESKTOP-C71347J\\SQLEXPRESS;Initial Catalog=db_Ychet_TeleComanies;Integrated Security=True;";


        private DataRowView selectedAgent;

        public EditAgents()
        {
            InitializeComponent();

            // Вызывайте метод LoadCustomers для загрузки заказчиков при инициализации окна
            LoadCustomers();
        }

        // Метод для загрузки всех заказчиков в ComboBox
        private void LoadCustomers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, FIO FROM tbl_Zakazchiki";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ZakazchikComboBox.Items.Add(new KeyValuePair<int, string>((int)reader["ID"], reader["FIO"].ToString()));
                }
            }
        }

        // Метод для загрузки данных в окно EditAgents
        public void LoadData(DataRowView agent)
        {
            selectedAgent = agent;
            FIOTextBox.Text = agent["AgentName"].ToString();
            GeneralTimeTextBox.Text = agent["GTSO"].ToString();

            // Получите ФИО заказчика из DataRowView
            string customerName = agent["CustomerName"].ToString();

            // Найдите соответствующий элемент в ComboBox и установите его как выбранный
            foreach (KeyValuePair<int, string> item in ZakazchikComboBox.Items)
            {
                if (item.Value == customerName)
                {
                    ZakazchikComboBox.SelectedValue = item.Key;
                    break;
                }
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Получите новые значения из TextBox и ComboBox
            string newFIO = FIOTextBox.Text;
            int newGeneralTime = int.Parse(GeneralTimeTextBox.Text);

            // Получите выбранного заказчика из ComboBox
            int newCustomerID = ((KeyValuePair<int, string>)ZakazchikComboBox.SelectedItem).Key;

            // Обновите DataRowView, чтобы он отразил изменения
            selectedAgent["AgentName"] = newFIO;
            selectedAgent["GTSO"] = newGeneralTime;
            selectedAgent["CustomerName"] = ZakazchikComboBox.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Выполните операцию UPDATE SQL, чтобы сохранить изменения в базе данных
                string query = "UPDATE tbl_Agents SET FIO = @FIO, General_time_of_start_reklams = @GeneralTime, ID_zakazchika = @CustomerID WHERE ID = @AgentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FIO", newFIO);
                command.Parameters.AddWithValue("@GeneralTime", newGeneralTime);
                command.Parameters.AddWithValue("@CustomerID", newCustomerID);
                command.Parameters.AddWithValue("@AgentID", selectedAgent["ID_agent"]);

                command.ExecuteNonQuery();
            }

            Agents mainWindow = Application.Current.Windows.OfType<Agents>().FirstOrDefault();
            if (mainWindow != null)
            {
                mainWindow.LoadAgentData();
            }
            this.Close();
        }


    }
}