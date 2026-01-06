using System.Collections.Generic;

namespace CinemaManagementSystem.ContractTypeForEmployee
{
    public class InternContract : IEmployeeContract
    {
        private static List<InternContract> _contracts = new();
        private Employee _employee;

        public string UniversityName { get; }
        public int Duration { get; }

        public static IReadOnlyList<InternContract> Contracts => _contracts.AsReadOnly();

        public InternContract(string universityName, int duration, Employee employee)
        {
            UniversityName = universityName;
            Duration = duration;
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
