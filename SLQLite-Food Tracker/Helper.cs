using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SLQLite_Food_Tracker
{
    public static class Helper
    {
        public static string filepath = "..\\..\\Files\\FoodTracker.db";
        public static string connString = $@"Data Source={filepath}; Version=3";
        public static int selectedFood;
        public static void InitializeDatabase()
        {
            try
            {
                if (!File.Exists(filepath))
                {
                    SQLiteConnection.CreateFile(filepath);
                    using (var connection = new SQLiteConnection(connString))
                    {
                        connection.Open();

                        string createMealTableQry = @"CREATE TABLE IF NOT EXISTS Meals (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL);";

                        string createFoodableQry = @"CREATE TABLE IF NOT EXISTS Foods (Id INTEGER PRIMARY KEY AUTOINCREMENT,Name TEXT NOT NULL, MealId INTEGER, FoodDate Date NOT NULL, Foreign Key (MealId) References Meals(Id));";

                        using (var command = new SQLiteCommand(connection))
                        {
                            command.CommandText = createMealTableQry;
                            command.ExecuteNonQuery();


                            command.CommandText = createFoodableQry;
                            command.ExecuteNonQuery();
                        }
                    }
                    AddMealsAutomatically();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static void AddMealsAutomatically()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string[] meals = { "Breakfast", "Lunch", "Supper", "Snack" };

                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    for (int i = 0; i < meals.Length; i++)
                    {
                        cmd.CommandText = @"INSERT INTO Meals(Name) Values(@name)";
                        cmd.Parameters.AddWithValue("@name", meals[i]);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }
            }
        }
        //get the list of meals
        public static void GetMealsList(ComboBox cmb)
        {
            cmb.DisplayMember = "Name";
            var conn = new SQLiteConnection(connString);
            conn.Open();
            var cmd = new SQLiteCommand(conn);

            cmd.CommandText = "SELECT * FROM MEALS";
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int id = dr.GetInt32(0);        // Assuming Id is of type integer
                string name = dr.GetString(1);  // Assuming Name is of type string
                cmb.Items.Add(new { Name = name });
                //tmpDataGridView.Rows.Insert(0, id, name);
            }
            conn.Close();
        }
        private static int GetMealIdByName(string mealName, SQLiteConnection conn)
        {
            var cmd = new SQLiteCommand(conn);
            cmd.CommandText = "Select Id from MEALS Where Name=@name";
            //Console.WriteLine("Executing query: " + cmd.CommandText);
            //Console.WriteLine("Parameter value: " + mealName);
            cmd.Parameters.AddWithValue("name", mealName);
            object result = cmd.ExecuteScalar();
            // Check if the result is not null before converting to int
            int id = result != null ? Convert.ToInt32(result) : -1;
            return id;
        }
        public static void ShowCertainDate(DateTime thisDate, DataGridView tmpDataGridView)
        {

            var conn = new SQLiteConnection(connString);
            conn.Open();
            var cmd = new SQLiteCommand(conn);

            cmd.CommandText = "SELECT FOODS.Id, FOODS.FoodDate, FOODS.Name, MEALS.Name AS MealName " +
                      "FROM FOODS " +
                      "JOIN MEALS ON FOODS.MealId = MEALS.Id " +
                      "WHERE FOODDATE >= @date AND FoodDate < @endDate ORDER BY FOODDATE ASC";
            cmd.Parameters.Add(new SQLiteParameter("@date", thisDate.Date));
            cmd.Parameters.Add(new SQLiteParameter("@endDate", thisDate.Date.AddDays(1)));
            Console.WriteLine("Executing query: " + cmd.CommandText);
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int id = dr.GetInt32(0);
                DateTime foodDate = dr.GetDateTime(1);// Assuming Id is of type integer
                string name = dr.GetString(2);  // Assuming Name is of type string
                string mealName = dr.GetString(3);
                tmpDataGridView.Rows.Insert(0, id, foodDate, name, mealName);
            }
            conn.Close();

        }

        public static void Create(string tmpname, string mealName, DateTime foodDate)
        {
            var con = new SQLiteConnection(connString);
            con.Open();
            var cmd = new SQLiteCommand(con);
            int mealId = GetMealIdByName(mealName, con);
            cmd.CommandText = "INSERT INTO FOODS (NAME, MealId, FoodDate) VALUES(@name, @mealId,@foodDate)";

            string name = tmpname;
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("mealId", mealId);
            cmd.Parameters.AddWithValue("foodDate", foodDate.ToLocalTime());

            cmd.ExecuteNonQuery();
            con.Close();
        }
        public static void Read(DataGridView tmpDataGridView)
        {
            tmpDataGridView.Rows.Clear();

            var conn = new SQLiteConnection(connString);
            conn.Open();
            var cmd = new SQLiteCommand(conn);
            cmd.CommandText = "SELECT FOODS.Id, FOODS.FoodDate, FOODS.Name, MEALS.Name AS MealName\r\nFROM FOODS\r\nJOIN MEALS ON FOODS.MealId = MEALS.Id ORDER BY FOODDATE ASC";
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int id = dr.GetInt32(0);
                DateTime foodDate = dr.GetDateTime(1);// Assuming Id is of type integer
                string name = dr.GetString(2);  // Assuming Name is of type string
                string mealName = dr.GetString(3);
                tmpDataGridView.Rows.Insert(0, id, foodDate, name, mealName);
            }
            conn.Close();
        }
        public static void Update(DataGridView tmpDataGridView, Button btnUpdate, TextBox txtName, ComboBox cmbMealName, DateTimePicker dtpFoodDate)
        {

            if (selectedFood != null)
            {
                int firstColumnIntValue = Convert.ToInt32(selectedFood);
                // Now you can use firstColumnIntValue
                var conn = new SQLiteConnection(connString);
                conn.Open();
                var cmd = new SQLiteCommand(conn);
                int mealId = GetMealIdByName(cmbMealName.Text, conn);
                cmd.CommandText = "UPDATE FOODS SET NAME=@NAME, MEALID=@MEALID, FOODDATE=@FOODDATE WHERE ID =@ID";
                cmd.Parameters.AddWithValue("@ID", selectedFood);
                cmd.Parameters.AddWithValue("@NAME", txtName.Text);
                cmd.Parameters.AddWithValue("@MEALID", mealId);
                cmd.Parameters.AddWithValue("@FOODDATE", dtpFoodDate.Value);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("The selected entry has been updated");
            }




            //show it to our users what they are editing
        }
        public static void Delete()
        {
            //SelectedRow(tmpDataGridView, txtName, cmbMealName, dtpFoodDate);
            var conn = new SQLiteConnection(connString);
            conn.Open();
            var cmd = new SQLiteCommand(conn);
            cmd.CommandText = "DELETE FROM FOODS WHERE ID=@ID";
            cmd.Parameters.AddWithValue("ID", selectedFood);
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("The entry has been deleted");

        }
        public static void SelectedRow(DataGridView tmpDataGridView, TextBox txtName, ComboBox cmbMealName, DateTimePicker dtpFoodDate)
        {

            if (tmpDataGridView.SelectedCells.Count > 0)
            {
                var selectedRowIndex = (int)tmpDataGridView.SelectedCells[0].RowIndex;

                // Check if the selected row index is valid
                if (selectedRowIndex >= 0 && selectedRowIndex < tmpDataGridView.Rows.Count)
                {
                    // Get the selected row
                    DataGridViewRow selectedRow = tmpDataGridView.Rows[selectedRowIndex];

                    // Check if the selected cell is not null
                    if (selectedRow.Cells["NAME"].Value != null)
                    {
                        selectedFood = (int)selectedRow.Cells[0].Value;
                        txtName.Text = selectedRow.Cells["NAME"].Value.ToString();
                        cmbMealName.Text = selectedRow.Cells["MEALID"].Value?.ToString() ?? string.Empty;

                        // Check if the value is not null before converting to string
                        object foodDateValue = selectedRow.Cells["FOODDATE"].Value;
                        string foodDateString = foodDateValue != null ? foodDateValue.ToString() : string.Empty;

                        if (DateTime.TryParse(foodDateString, out DateTime foodDate))
                        {
                            dtpFoodDate.Value = foodDate;
                        }
                    }
                    else
                    {
                        // Handle the case where the selected cell is null (empty row)
                        txtName.Text = string.Empty;
                        cmbMealName.Text = string.Empty;
                        dtpFoodDate.Value = DateTime.Now; // Set a default value or handle as needed
                    }
                }
            }
        }

    }
}
