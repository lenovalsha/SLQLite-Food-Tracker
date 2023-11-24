using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormUI;

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
        public void Create()
        {
            //using (DataContextL contextL= new DataContextL())
            //{
            //if (cmbMeal.SelectedItem != null && txtFood.Text != String.Empty)
            //{

            //   contextL.Foods.Add(MakeNewFood());
            //    contextL.SaveChanges(); //goes through the context class and looks through the pending changes to add to the database (update, creations, deletions)
            //    //if (MakeNewFood().FoodDate >= DateTime.Today && MakeNewFood().FoodDate < DateTime.Today.AddDays(1))
            //    //{
            //    //    ShowToday();
            //    //}
            //    //else
            //    //    ShowAll();

            //}
            //else
            //{
            //    MessageBox.Show("Enter all data necessary");
            //}
            //    var name = txtFood.Text;
                

            //    contextL.Foods.Add(new Food() { Name = name });
            //}
        }
        public void Read()
        {

        }
        public void Update() { } 
        public void Delete()
        {

        }
    }
}
