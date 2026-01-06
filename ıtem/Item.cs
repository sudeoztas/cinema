using System;

namespace CinemaManagementSystem.Items
{
    [Serializable]
    public abstract class Item
    {
        private string _name;
        private double _price;

        public string Name
        {
            get => _name;
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Item name cannot be empty.");
                _name = value;
            }
        }

        public double Price
        {
            get => _price;
            protected set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative.");
                _price = value;
            }
        }

        protected Item(string name, double price)
        {
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"{Name} ({Price}€)";
        }
    }
}
