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
    /// Логика взаимодействия для AddZakazchikPage.xaml
    /// </summary>
    public partial class AddZakazchikPage : Window
    {
        private DataTable zakazchikiTable = new DataTable();
        string connectionString = "Data Source=DESKTOP-C71347J\\SQLEXPRESS;Initial Catalog=db_Ychet_TeleComanies;Integrated Security=True;";

        public AddZakazchikPage()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string fio = FIOTextBox.Text;
            DateTime dateOfStart = DateOfStartPicker.SelectedDate ?? DateTime.MinValue;
            string phone = PhoneTextBox.Text;
            string bankRekvezit = BankRekvezitTextBox.Text;

            if (string.IsNullOrWhiteSpace(fio) || dateOfStart == DateTime.MinValue || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(bankRekvezit))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Создайте SQL-запрос на вставку данных в таблицу
            string insertQuery = "INSERT INTO tbl_Zakazchiki (FIO, Date_of_start, Phone, Bank_rekvezit) VALUES (@FIO, @DateOfStart, @Phone, @BankRekvezit)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Добавьте параметры к запросу
                    command.Parameters.AddWithValue("@FIO", fio);
                    command.Parameters.AddWithValue("@DateOfStart", dateOfStart);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@BankRekvezit", bankRekvezit);

                    // Выполните SQL-запрос
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Заказчик добавлен.");
                        Zakazchiki mainWindow = Application.Current.Windows.OfType<Zakazchiki>().FirstOrDefault();
                        if (mainWindow != null)
                        {
                            mainWindow.LoadZakazchikiData();
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
    }
}
