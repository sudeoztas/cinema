using System.Collections.Generic;

namespace CinemaManagementSystem.ContractTypeForEmployee
{
    public class FullTimeContract : IEmployeeContract
    {
        private static List<FullTimeContract> _contracts = new();
        private Employee _employee;

        public static IReadOnlyList<FullTimeContract> Contracts => _contracts.AsReadOnly();

        public FullTimeContract(Employee employee)
        {
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
