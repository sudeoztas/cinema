namespace CinemaManagementSystem.EmployeeRoles
{
    public class ReceptionistRole : IEmployeeRole
    {
        public int DeskNumber { get; }

        public string RoleName => "Receptionist";

        public ReceptionistRole(int deskNumber)
        {
            if (deskNumber <= 0)
                throw new ArgumentException("Desk number must be positive.");

            DeskNumber = deskNumber;
        }
    }
}
