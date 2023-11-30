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
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Helper.GetMealsList(cmbMeal);
            Helper.Read(dgvData);
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
            Helper.DisplayAll(dgvData);
        }
    }
}
