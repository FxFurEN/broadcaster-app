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
    /// Логика взаимодействия для EditZakazchik.xaml
    /// </summary>
    public partial class EditZakazchik : Window
    {
        string connectionString = "Data Source=DESKTOP-C71347J\\SQLEXPRESS;Initial Catalog=db_Ychet_TeleComanies;Integrated Security=True;";


        private DataRowView selectedZakachik;
        public EditZakazchik()
        {
            InitializeComponent();
            LoadCustomers();
        }

        // Метод для загрузки всех заказчиков в ComboBox
        private void LoadCustomers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, FIO, Date_of_start, Phone, Bank_rekvezit FROM tbl_Zakazchiki";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
            }
        }

        // Метод для загрузки данных в окно EditAgents
        public void LoadData(DataRowView zakazchik)
        {
            selectedZakachik = zakazchik;
            FIOTextBox.Text = zakazchik["FIO"].ToString();
            DateOfStartTextBox.Text = zakazchik["Date_of_start"].ToString();
            PhoneTextBox.Text = zakazchik["Phone"].ToString();
            Bank_rekvezitTextBox.Text = zakazchik["Bank_rekvezit"].ToString();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Получите новые значения из TextBox
            string newFIO = FIOTextBox.Text;
            DateTime newDateOfStart = DateTime.Parse(DateOfStartTextBox.Text);
            string newPhone = PhoneTextBox.Text;
            string newBankRekvezit = Bank_rekvezitTextBox.Text;

            // Обновите DataRowView, чтобы он отразил изменения
            selectedZakachik["FIO"] = newFIO;
            selectedZakachik["Date_of_start"] = newDateOfStart;
            selectedZakachik["Phone"] = newPhone;
            selectedZakachik["Bank_rekvezit"] = newBankRekvezit;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Выполните операцию UPDATE SQL, чтобы сохранить изменения в базе данных
                string query = "UPDATE tbl_Zakazchiki SET FIO = @FIO, Date_of_start = @DateOfStart, Phone = @Phone, Bank_rekvezit = @BankRekvezit WHERE ID = @ZakazchikID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FIO", newFIO);
                command.Parameters.AddWithValue("@DateOfStart", newDateOfStart);
                command.Parameters.AddWithValue("@Phone", newPhone);
                command.Parameters.AddWithValue("@BankRekvezit", newBankRekvezit);
                command.Parameters.AddWithValue("@ZakazchikID", selectedZakachik["ID"]);

                command.ExecuteNonQuery();
            }

            Zakazchiki mainWindow = Application.Current.Windows.OfType<Zakazchiki>().FirstOrDefault();
            if (mainWindow != null)
            {
                mainWindow.LoadZakazchikiData();
            }
            this.Close();
        }
    }
}
