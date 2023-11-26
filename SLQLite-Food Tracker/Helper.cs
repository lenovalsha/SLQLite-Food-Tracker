using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Drawing.Text;

namespace SLQLite_Food_Tracker
{
    public static class Helper
    {
        private static string filepath = "..\\..\\Files\\FoodTracker.db";
        private static string connString = $@"Data Source={filepath}; Version=3";

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

                        string createFoodableQry = @"CREATE TABLE IF NOT EXISTS Foods (Id INTEGER PRIMARY KEY AUTOINCREMENT,Name TEXT NOT NULL);";

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
            using(SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string[] meals = {"Breakfast", "Lunch", "Supper", "Snack" };

                using(SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    for(int i=0;i<meals.Length;i++)
                    {
                        cmd.CommandText = @"INSERT INTO Meals(Name) Values(@name)";
                        cmd.Parameters.AddWithValue("@name", meals[i]);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }
            }
        }
       
    }
}
