using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLQLite_Food_Tracker
{
    public class Food
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FoodDate { get; set; }

        public Meal Meal { get; set; }
        public int MealId { get; set; }
    }
}
