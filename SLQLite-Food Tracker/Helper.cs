using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Drawing.Text;
using System.Windows.Forms;

namespace SLQLite_Food_Tracker
{
    public static class Helper
    {
        public static string filepath = "..\\..\\Files\\FoodTracker.db";
        public static string connString = $@"Data Source={filepath}; Version=3";


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
                cmb.Items.Add(new {Name = name});  
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
                      "WHERE FOODDATE >= @date AND FoodDate < @endDate";
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
            cmd.Parameters.AddWithValue("foodDate", foodDate);

            cmd.ExecuteNonQuery();
            con.Close();
        }
        public static void Read(DataGridView tmpDataGridView)
        {
            tmpDataGridView.Columns.Add("Id", "ID");      // Add a column for Id
            tmpDataGridView.Columns.Add("FoodDate", "Date");
            tmpDataGridView.Columns.Add("Name", "Name");
            tmpDataGridView.Columns.Add("MealId", "Meal");


            var conn = new SQLiteConnection(connString);
            conn.Open();
            var cmd = new SQLiteCommand(conn);
            cmd.CommandText = "SELECT FOODS.Id, FOODS.FoodDate, FOODS.Name, MEALS.Name AS MealName\r\nFROM FOODS\r\nJOIN MEALS ON FOODS.MealId = MEALS.Id";
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int id = dr.GetInt32(0);
                DateTime foodDate = dr.GetDateTime(1);// Assuming Id is of type integer
                string name = dr.GetString(2);  // Assuming Name is of type string
                string mealName = dr.GetString(3); 
                tmpDataGridView.Rows.Insert(0, id, foodDate, name,mealName);
            }
            conn.Close();
        }
        public static void Update() { }
        public static void Delete()
        {

        }

    }
}
