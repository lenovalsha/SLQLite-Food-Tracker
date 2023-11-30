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
using System.IO;

namespace SLQLite_Food_Tracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
       
       
        private void btnCreate_Click(object sender, EventArgs e)
        {
            //ShowData();
            dgvData.Rows.Clear();
            if (!string.IsNullOrEmpty(txtFood.Text) && (cmbMeal.SelectedItem != null))
            {
            Helper.Create(txtFood.Text, cmbMeal.Text, dtpDate.Value);
            Helper.Read(dgvData);
            }else
            {
                MessageBox.Show("Please ensure all data is filled in");
                Helper.Read(dgvData);

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvData.Columns.Add("Id", "ID");      // Add a column for Id
            dgvData.Columns.Add("FoodDate", "Date");
            dgvData.Columns.Add("Name", "Name");
            dgvData.Columns.Add("MealId", "Meal");
            dgvData.Columns[0].Visible = false;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Helper.GetMealsList(cmbMeal);
            Helper.Read(dgvData);
            dgvData.SelectionChanged += DgvData_SelectionChanged; ;

        }

        private void DgvData_SelectionChanged(object sender, EventArgs e)
        {
          
           

            Helper.SelectedRow(dgvData, txtFood, cmbMeal, dtpDate);
            

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvData.Rows.Clear();
            Helper.ShowCertainDate(dtDisplayToDGV.Value,dgvData);
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            dgvData.Rows.Clear();
            Helper.ShowCertainDate(DateTime.Today, dgvData);
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            Helper.Read(dgvData);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Helper.Update(dgvData, btnUpdate, txtFood, cmbMeal, dtpDate);
            Helper.Read(dgvData);


        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Helper.Delete();
            Helper.Read(dgvData);

        }

        private void btnCreate_DragOver(object sender, DragEventArgs e)
        {
           
        }

        private void btnCreate_MouseHover(object sender, EventArgs e)
        {
            btnCreate.BackColor = Color.DarkGreen;
        }

        private void btnCreate_MouseLeave(object sender, EventArgs e)
        {
            btnCreate.BackColor = Color.FromArgb(255, 255, 128);

        }

        private void btnUpdate_MouseHover(object sender, EventArgs e)
        {
            btnUpdate.BackColor = Color.Yellow;
        }

        private void btnUpdate_MouseLeave(object sender, EventArgs e)
        {
            btnCreate.BackColor = Color.FromArgb(255, 255, 128);
        }
    }
}
