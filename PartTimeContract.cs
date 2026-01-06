using System.Collections.Generic;

namespace CinemaManagementSystem.ContractTypeForEmployee
{
    public class PartTimeContract : IEmployeeContract
    {
        private static List<PartTimeContract> _contracts = new();
        private Employee _employee;

        public int HoursPerWeek { get; }

        public static IReadOnlyList<PartTimeContract> Contracts => _contracts.AsReadOnly();

        public PartTimeContract(int hoursPerWeek, Employee employee)
        {
            if (hoursPerWeek <= 0 || hoursPerWeek > 30)
                throw new ArgumentException("Invalid part-time hours.");

            HoursPerWeek = hoursPerWeek;
            _employee = employee;

            employee.SetContract(this);
            _contracts.Add(this);
        }

        public void RemoveFromExtent()
        {
            _contracts.Remove(this);
        }
    }
}
