using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
namespace SLQLite_Food_Tracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Food MakeNewFood()
        {
            var newFood = new Food
            {
                Name = txtFood.Text,
                MealId = (cmbMeal.SelectedItem as Meal).Id,
                FoodDate = dtpDate.Value
            };

            return newFood;
        }
        //show Data in table
        private 
        public void Create()
        {
           
        }
        public void Read()
        {

        }
        public void Update() { } 
        public void Delete()
        {

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

        }
    }
}
