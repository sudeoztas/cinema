using System;

namespace CinemaManagementSystem
{
    [Serializable]
    public abstract class Person
    {
        // ================= ATTRIBUTES =================

        private string _name;
        private string _surname;
        private DateTime _birthDate;
        private GenderEnum _gender;

        // ================= PROPERTIES =================

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty.");
                _name = value;
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Surname cannot be empty.");
                _surname = value;
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Birth date cannot be in the future.");
                _birthDate = value;
            }
        }

        public GenderEnum Gender
        {
            get => _gender;
            set => _gender = value;
        }

        // ================= DERIVED ATTRIBUTE =================
        // /Age : int
        public int Age
        {
            get
            {
                int age = DateTime.Now.Year - BirthDate.Year;
                if (DateTime.Now.DayOfYear < BirthDate.DayOfYear)
                    age--;
                return age;
            }
        }

        // ================= CONSTRUCTOR =================
        protected Person(string name, string surname, DateTime birthDate, GenderEnum gender)
        {
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            Gender = gender;
        }

        // ================= OVERRIDE =================
        public override string ToString()
        {
            return $"{Name} {Surname}, Age: {Age}, Gender: {Gender}";
        }
    }
}
