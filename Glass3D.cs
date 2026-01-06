using System;

namespace CinemaManagementSystem.Items
{
    [Serializable]
    public class Glass3D : Item
    {
        public bool IsReusable { get; }

        public Glass3D(double price, bool isReusable)
            : base("3D Glasses", price)
        {
            IsReusable = isReusable;
        }
    }
}
