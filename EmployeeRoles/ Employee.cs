using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using CinemaManagementSystem.EmployeeRoles;
using CinemaManagementSystem.ContractTypeForEmployee;
using CinemaManagementSystem.PersistenceForAllClasses;

namespace CinemaManagementSystem
{
    [Serializable]
    public class Employee : Person, IExtent<Employee>
    {
        // ================= STATIC =================
        [XmlIgnore]
        private static double _minSalary = 3000;

        // ================= EMPLOYEE ATTRIBUTES =================
        private DateTime _startDate;
        private DateTime? _endDate;
        private double _salary;

        // ================= ROLE ASPECT (COMPOSITION) =================
        [XmlIgnore]
        private IEmployeeRole _role;

        [XmlIgnore]
        public IEmployeeRole Role => _role;

        internal void SetRole(IEmployeeRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            if (_role != null)
                throw new InvalidOperationException("Employee already has a role.");

            _role = role;
        }

        // ================= CONTRACT ASPECT (COMPOSITION) =================
        [XmlIgnore]
        private IEmployeeContract? _contract;

        [XmlIgnore]
        public IEmployeeContract? Contract => _contract;

        internal void SetContract(IEmployeeContract contract)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (_contract != null)
                throw new InvalidOperationException("Employee already has a contract.");

            _contract = contract;
        }

        public void ChangeContract(IEmployeeContract newContract)
        {
            if (newContract == null)
                throw new ArgumentNullException(nameof(newContract));

            _contract?.RemoveFromExtent();
            _contract = newContract;
        }

        // ================= PROPERTIES =================
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Start date cannot be in the future.");
                _startDate = value;
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (value > DateTime.Now || value < StartDate)
                    throw new ArgumentException("Invalid end date.");
                _endDate = value;
            }
        }

        public double Salary
        {
            get => _salary;
            set
            {
                if (value < _minSalary)
                    throw new ArgumentException("Salary cannot be less than minimum salary.");
                _salary = value;
            }
        }

        // ================= DERIVED =================
        [XmlIgnore]
        public int YearsOfService
        {
            get
            {
                var end = EndDate ?? DateTime.Now;
                int years = end.Year - StartDate.Year;
                if (end.DayOfYear < StartDate.DayOfYear)
                    years--;
                return years;
            }
        }

        // ================= EXTENT =================
        private static List<Employee> _employees = new();
        public static IReadOnlyList<Employee> Employees => _employees.AsReadOnly();

        private static void AddEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            _employees.Add(employee);
        }

        public static void ClearAllEmployees()
        {
            _employees.Clear();
        }

        // ================= CONSTRUCTORS =================
        public Employee() { }

        public Employee(
            string name,
            string surname,
            DateTime birthDate,
            GenderEnum gender,
            DateTime startDate,
            double salary,
            IEmployeeRole role,
            DateTime? endDate = null)
            : base(name, surname, birthDate, gender)
        {
            StartDate = startDate;
            Salary = salary;
            EndDate = endDate;
            SetRole(role);

            AddEmployee(this);
        }

        // ================= PERSISTENCE =================
        public static void Save(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<Employee>));
            using var fs = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(fs, _employees);
        }

        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Employee file not found.");

            XmlSerializer serializer = new(typeof(List<Employee>));
            using var reader = new StreamReader(filePath);
            _employees = (List<Employee>)serializer.Deserialize(reader) ?? new();
        }

        // ================= EXTENT INTERFACE =================
        public List<Employee> GetExtent() => _employees;

        public void ReplaceExtent(List<Employee> newExtent)
        {
            _employees = newExtent ?? new();
        }

        // ================= OVERRIDE =================
        public override string ToString()
        {
            string end = EndDate.HasValue ? EndDate.Value.ToShortDateString() : "Present";
            return $"{base.ToString()}, Salary: {Salary}€, " +
                   $"Started: {StartDate:dd/MM/yyyy}, End: {end}, " +
                   $"Years of Service: {YearsOfService}";
        }
    }
}
