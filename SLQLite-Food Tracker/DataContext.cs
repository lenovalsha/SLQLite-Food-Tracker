using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SLQLite_Food_Tracker;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WinFormUI
{
    public class DataContextL: DbContext
    {
      
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = FoodData.db"); //make sure to create this class
        }
        //create our tables
        public DbSet<Food> Foods { get; set; } //Table
        public DbSet<Meal> Meals { get; set; } //Table

    }
}
