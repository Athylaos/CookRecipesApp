using CookRecipesApp.Model.User;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CookRecipesApp.Model.Recepie
{
    public class RecepieDbModel
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }
        [NotNull]
        public int UserId { get; set; }
        [NotNull]
        public string Title { get; set; } = string.Empty;

        [NotNull]
        public string CoockingProcess { get; set; } = string.Empty;
        [NotNull]
        public int CoockingTime { get; set; }
        [NotNull]
        public int Servings { get; set; }
        [NotNull]
        public int ServingUnitId { get; set; }
        [NotNull]
        public int Difficulty { get; set; }

        public float Calories { get; set; }
        public float Proteins { get; set; }
        public float Fats { get; set; }
        public float Carbohydrates { get; set; }
        public float Fiber { get; set; }

        public string RecepieCreated { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int UsersRated { get; set; }



    }
}
