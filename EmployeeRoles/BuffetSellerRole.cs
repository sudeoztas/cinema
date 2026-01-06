namespace CinemaManagementSystem.EmployeeRoles
{
    public class BuffetSellerRole : IEmployeeRole
    {
        public string RoleName => "Buffet Seller";

        private decimal _totalSales;

        public decimal TotalSales
        {
            get => _totalSales;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("Total sales cannot be negative.");
                _totalSales = value;
            }
        }

        public BuffetSellerRole()
        {
            _totalSales = 0;
        }

        public void SellItem()
        {
            TotalSales++;
        }
    }
}
