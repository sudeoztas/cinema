using System;

namespace CinemaManagementSystem.Items
{
    [Serializable]
    public class Snack : Item
    {
        public int Calories { get; }

        public Snack(string name, double price, int calories)
            : base(name, price)
        {
            if (calories <= 0)
                throw new ArgumentException("Calories must be positive.");

            Calories = calories;
        }
    }
}
