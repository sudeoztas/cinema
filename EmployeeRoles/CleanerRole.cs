using CinemaManagementSystem.Enums;

namespace CinemaManagementSystem.EmployeeRoles
{
    public class CleanerRole : IEmployeeRole
    {
        public CleaningTypeEnum CleaningType { get; }

        public string RoleName => "Cleaner";

        public CleanerRole(CleaningTypeEnum cleaningType)
        {
            CleaningType = cleaningType;
        }
    }
}
